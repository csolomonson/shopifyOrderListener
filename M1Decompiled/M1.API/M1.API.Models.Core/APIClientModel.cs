using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using M1.API.App_Start;
using M1.API.DTOs.Core;
using M1.API.Repositories;
using M1.API.Utilities;
using M1.Cipher;
using M1.Core;

namespace M1.API.Models.Core;

public class APIClientModel : APIBaseModel, IAPIClientModel, IAPIBaseModel, IDisposable
{
	private class ClientContextDataDto
	{
		public DataTable TempDataTabe { get; set; }

		public M1.Core.AppContext ApplicationContext { get; set; }

		public string DataDictionary { get; set; }

		public string NetworkLibrary { get; set; }

		public ClientContextDataDto()
		{
		}

		public ClientContextDataDto(DataTable tempDataTabe, M1.Core.AppContext applicationContext, string dataDictionary, string networkLibrary)
		{
			TempDataTabe = tempDataTabe;
			ApplicationContext = applicationContext;
			DataDictionary = dataDictionary;
			NetworkLibrary = networkLibrary;
		}
	}

	private Dictionary<string, string> Metadata = new Dictionary<string, string>();

	public APIClientRepository clientRepository { get; set; }

	public APIEnums.WebAPIModules ApiModuleId { get; set; }

	private string GetMetaData(string key, string defaultValue = "")
	{
		string result = defaultValue;
		if (Metadata.ContainsKey(key))
		{
			result = Metadata[key].ToString();
		}
		return result;
	}

	private Task<ClientContextDataDto> GetNonHostedAPIIDInfoAsync(M1.Core.AppContext context, string apiId, string m1ModuleId)
	{
		DataTable dataTable = new DataTable();
		string text = string.Empty;
		string networkLibrary = string.Empty;
		using (SqlCommand sqlCommand = new SqlCommand())
		{
			text = context.Server.IniSettings.Get("DataDictionary", "M1DD");
			context.DDServerManager.ConnectionInfo.Server = context.Server.IniSettings.Get("DBServer", "(local)");
			context.DDServerManager.ConnectionInfo.SqlUserID = context.Server.IniSettings.Get("DBUserID", "sa");
			context.DDServerManager.sqlPassword = context.Server.IniSettings.Get("DBPwd", string.Empty);
			context.DDServerManager.sqlPassword = (string.IsNullOrEmpty(context.DDServerManager.sqlPassword) ? string.Empty : context.DBServerManager.Decrypt(context.DDServerManager.sqlPassword));
			context.DDServerManager.ConnectionInfo.TrustedConnection = context.Server.IniSettings.GetAsBool("DBTrustedConnection", defaultValue: false);
			networkLibrary = context.Server.IniSettings.Get("DBNetworkLibrary", "dbmssocn");
			context.DDServerManager.ConnectionInfo.NetworkLibrary = networkLibrary;
			using SqlConnection sqlConnection = context.DDServerManager.GetConnection(null, text, openImmediately: false);
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "\r\nSELECT [daAPIID],[daAPIKey],[daModuleID],[daDatabaseID],[daAPIUserID],[daAPIUserPWD],[daExtraSettings],[daIsReadOnly] \r\nFROM DDAPIINFO \r\nWHERE daAPIID=@apiID AND daModuleID=@moduleID";
			sqlCommand.Parameters.AddWithValue("@apiID", apiId);
			sqlCommand.Parameters.AddWithValue("@moduleID", m1ModuleId);
			sqlConnection.Open();
			dataTable.Load(sqlCommand.ExecuteReader());
		}
		return Task.FromResult(new ClientContextDataDto(dataTable, context, text, networkLibrary));
	}

	private Task<ClientContextDataDto> GetHostedAPIIDInfoAsync(M1.Core.AppContext context, string apiId, string m1ModuleId)
	{
		DataTable dataTable = new DataTable();
		DataTable dataTable2 = new DataTable();
		string text = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		Guid result = Guid.Empty;
		Guid.TryParse(apiId, out result);
		apiId = result.ToString("N").ToUpper();
		stringBuilder.AppendFormat("server={0} ;database={1};integrated Security=SSPI", context.Registry.MetadataServer, context.Registry.MetadataDB);
		using (SqlConnection sqlConnection = new SqlConnection(stringBuilder.ToString()))
		{
			using SqlCommand sqlCommand = new SqlCommand("dbo.GetMetaDatabyGUID", sqlConnection);
			sqlCommand.Parameters.AddWithValue("@CustomerGUID", result);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.Connection = sqlConnection;
			sqlConnection.Open();
			dataTable2.Load(sqlCommand.ExecuteReader());
		}
		if (dataTable2.Rows.Count > 0)
		{
			DataRow row = dataTable2.Rows[0];
			Metadata = row.Table.Columns.Cast<DataColumn>().ToDictionary((DataColumn c) => c.ColumnName, (DataColumn c) => row[c].ToString());
			context.DDServerManager.ConnectionInfo.Server = GetMetaData("Server_Instance");
			context.DDServerManager.ConnectionInfo.NetworkLibrary = context.Registry.NetworkLibrary;
			context.DDServerManager.ConnectionInfo.SqlUserID = GetMetaData("M1Admin_User");
			context.DDServerManager.sqlPassword = Cipher_Static.Decrypt(GetMetaData("M1Admin_PW"), result.ToString().ToLower());
			context.DDServerManager.ConnectionInfo.TrustedConnection = true;
			text = GetMetaData("APIDD");
			text = (string.IsNullOrWhiteSpace(text) ? "M1DD" : text);
			using SqlConnection sqlConnection2 = context.DDServerManager.GetConnection(null, text, openImmediately: false);
			using SqlCommand sqlCommand2 = new SqlCommand();
			sqlCommand2.Connection = sqlConnection2;
			sqlCommand2.CommandText = "\r\nSELECT [daAPIID],[daAPIKey],[daModuleID],[daDatabaseID],[daAPIUserID],[daAPIUserPWD],[daExtraSettings],[daIsReadOnly]\r\nFROM DDAPIINFO\r\nWHERE daAPIID=@apiID AND daModuleID=@moduleID";
			sqlCommand2.Parameters.AddWithValue("@apiID", apiId);
			sqlCommand2.Parameters.AddWithValue("@moduleID", m1ModuleId);
			sqlConnection2.Open();
			dataTable.Load(sqlCommand2.ExecuteReader());
		}
		dataTable2.Dispose();
		ClientContextDataDto result2 = new ClientContextDataDto(dataTable, context, text, context.Registry.NetworkLibrary);
		dataTable.Dispose();
		return Task.FromResult(result2);
	}

	private IDictionary<string, string> GetExtraSettingsDictionary(string extraSettings)
	{
		IDictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrWhiteSpace(extraSettings))
		{
			string[] array = extraSettings.Split('\n');
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					string[] array2 = text.Split('=');
					if (array2.Length > 1)
					{
						dictionary.Add(array2[0], array2[1]);
					}
				}
			}
		}
		return dictionary;
	}

	/// <summary>
	/// Returns the module id from  Uri
	/// </summary>
	/// <param name="request">The request as HttpRequestMessage</param>
	/// <returns>Uri Module id as string</returns>
	public static string GetModuleFromRequestUrl(HttpRequestMessage request)
	{
		Uri requestUri = request.RequestUri;
		return (((object)requestUri != null) ? requestUri.Segments[2].Replace("/", "").Trim().ToUpper() : null) ?? string.Empty;
	}

	/// <summary>
	/// Returns M1 module id from based on uri module id
	/// </summary>
	/// <param name="uriModuleId">The uriModuleId as string</param>
	/// <returns>M1 Module id as string</returns>
	public static string GetM1ModuleCodeForUriModuleCode(string uriModuleId)
	{
		string result = string.Empty;
		switch (uriModuleId.Trim().ToUpper())
		{
		case "EDI":
			result = APIEnums.WebAPIModules.EDI.ToString();
			break;
		case "EOD":
			result = APIEnums.WebAPIModules.EO.ToString();
			break;
		case "BOM":
			result = APIEnums.WebAPIModules.BOM.ToString();
			break;
		case "ERP":
			result = APIEnums.WebAPIModules.ERP.ToString();
			break;
		}
		return result;
	}

	/// <summary>
	/// Get database information from DDAPIINFO table for a given Application ID
	/// </summary>
	/// <param name="apiMetadata"></param>
	/// <returns></returns>
	public virtual Task<APISessionDto> GetDatabaseRelatedInfoFromDDAPIInfoAsync(APIMetadataDto apiMetadata)
	{
		APISessionDto sessionDto = new APISessionDto();
		SqlCommand sqlCommand = new SqlCommand();
		DataTable dataTable = new DataTable();
		using (M1.Core.AppContext appContext = new M1.Core.AppContext(designMode: false, loadMetadata: false))
		{
			appContext.DDServerManager.ConnectionInfo.Server = apiMetadata.Server;
			appContext.DDServerManager.ConnectionInfo.NetworkLibrary = apiMetadata.NetworkLibrary;
			appContext.DDServerManager.ConnectionInfo.SqlUserID = apiMetadata.SqlUserID;
			appContext.DDServerManager.sqlPassword = apiMetadata.SqlPassword;
			appContext.DDServerManager.ConnectionInfo.TrustedConnection = apiMetadata.TrustedConnection || APIStartup.IsHosted;
			SqlConnection sqlConnection = (sqlCommand.Connection = appContext.DDServerManager.GetConnection(null, apiMetadata.DataDictionaryID, openImmediately: false));
			sqlCommand.CommandText = "\r\nSELECT [daAPIID],[daAPIKey],[daModuleID],[daDatabaseID],[daAPIUserID],[daAPIUserPWD],[daExtraSettings],[daIsReadOnly]\r\nFROM DDAPIINFO \r\nWHERE daAPIID=@apiID AND daModuleID=@moduleID";
			sqlCommand.Parameters.AddWithValue("@apiID", apiMetadata.APIID);
			sqlCommand.Parameters.AddWithValue("@moduleID", apiMetadata.M1ModuleID);
			sqlConnection.Open();
			dataTable.Load(sqlCommand.ExecuteReader());
			sqlConnection.Close();
			sqlCommand.Dispose();
		}
		foreach (DataRow row in dataTable.Rows)
		{
			sessionDto.APIID = row.Field<string>("daAPIID")?.Trim();
			sessionDto.DatabaseID = row.Field<string>("daDatabaseID")?.Trim();
			sessionDto.DatadictionaryID = apiMetadata.DataDictionaryID?.Trim();
			sessionDto.APIUserID = row.Field<string>("daAPIUserID")?.Trim();
			sessionDto.APIUserPassword = row.Field<string>("daAPIUserPWD")?.Trim();
			sessionDto.Authenticated = true;
			GetExtraSettingsDictionary(row.Field<string>("daExtraSettings")).ToList().ForEach(delegate(KeyValuePair<string, string> x)
			{
				sessionDto.ExtraSettings.Add(x.Key, x.Value);
			});
			sessionDto.Server = apiMetadata.Server;
			sessionDto.NetworkLibrary = apiMetadata.NetworkLibrary;
			sessionDto.SQLUserID = apiMetadata.SqlUserID;
			sessionDto.SQLUserPassword = apiMetadata.SqlPassword;
			sessionDto.TrustedConnection = apiMetadata.TrustedConnection;
			sessionDto.IsReadOnly = row.Field<bool>("daIsReadOnly");
		}
		dataTable.Dispose();
		return Task.FromResult(sessionDto);
	}

	/// <summary>
	/// Fill temp API Key Store for the current session.
	/// </summary>
	/// <param name="apiID"></param>
	/// <returns></returns>
	public async Task<bool> FillAPIKeyStoreAsync(string m1ModuleId, string apiID)
	{
		_ = string.Empty;
		_ = string.Empty;
		_ = string.Empty;
		_ = string.Empty;
		ClientContextDataDto clientContextDataDto = new ClientContextDataDto();
		_ = string.Empty;
		_ = string.Empty;
		using (M1.Core.AppContext appContext = new M1.Core.AppContext(designMode: false, loadMetadata: false))
		{
			clientContextDataDto = ((!APIStartup.IsHosted) ? (await GetNonHostedAPIIDInfoAsync(appContext, apiID, m1ModuleId)) : (await GetHostedAPIIDInfoAsync(appContext, apiID, m1ModuleId)));
			foreach (DataRow row in clientContextDataDto.TempDataTabe.Rows)
			{
				string sqlUserID = clientContextDataDto.ApplicationContext.DDServerManager.ConnectionInfo.SqlUserID;
				string sqlPassword = clientContextDataDto.ApplicationContext.DDServerManager.sqlPassword;
				string server = clientContextDataDto.ApplicationContext.DDServerManager.ConnectionInfo.Server;
				bool trustedConnection = clientContextDataDto.ApplicationContext.DDServerManager.ConnectionInfo.TrustedConnection;
				string dataDictionary = clientContextDataDto.DataDictionary;
				string networkLibrary = clientContextDataDto.NetworkLibrary;
				string text = row.Field<string>("daModuleID").Trim() + ":" + row.Field<string>("daAPIID").Trim();
				APIMetadataDto addValue = new APIMetadataDto(row.Field<string>("daModuleID").Trim(), row.Field<string>("daAPIID").Trim(), row.Field<string>("daAPIKey").Trim(), row.Field<string>("daAPIUserID").Trim(), row.Field<string>("daAPIUserPWD").Trim(), dataDictionary, server, networkLibrary, trustedConnection, row.Field<bool>("daIsReadOnly"))
				{
					DatabaseId = row.Field<string>("daDatabaseID"),
					ExtraSettings = GetExtraSettingsDictionary(row.Field<string>("daExtraSettings")),
					SqlUserID = sqlUserID,
					SqlPassword = sqlPassword
				};
				APIStartup.APIKeyStore.AddOrUpdate(text.ToLower(), addValue, (string key, APIMetadataDto oldValue) => oldValue);
			}
		}
		return clientContextDataDto.TempDataTabe.Rows.Count > 0;
	}

	/// <summary>
	/// Creates a new API client.
	/// </summary>
	/// <param name="apiSession">The apiSession as APISessionDto</param>
	/// <param name="module">The module as string</param>        
	public virtual Task<APIClientContext> CreateApiDataClientAsync(APISessionDto apiSession, APIEnums.WebAPIModules module)
	{
		string empty = string.Empty;
		APIClientContext aPIClientContext = null;
		APIClientRepository aPIClientRepository = (clientRepository = new APIClientRepository());
		using (aPIClientRepository)
		{
			aPIClientContext = clientRepository.GetApiClientContextAfterLoginVerificationAsync(apiSession, module).Result;
			if (!aPIClientContext.LoginAuthenticated)
			{
				empty = aPIClientContext.LoginErrorOutputString;
				aPIClientContext.LoginErrorOutputString = "Login failed. " + module.ToString() + " service could not connect to the M1 database. Error: [" + empty + "]";
				aPIClientContext.LoginAuthenticated = false;
				clientRepository.DoLogOutAsync(aPIClientContext);
			}
		}
		return Task.FromResult(aPIClientContext);
	}

	/// <summary>
	/// Dispose the Api Client
	/// </summary>
	/// <param name="clientContextDto"></param>
	public Task<bool> DisposeApiDataClientAsync(APIClientContext clientContextDto)
	{
		APIClientRepository aPIClientRepository = (clientRepository = new APIClientRepository());
		using (aPIClientRepository)
		{
			return clientRepository?.DoLogOutAsync(clientContextDto);
		}
	}
}
