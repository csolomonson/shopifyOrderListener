using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using M1.API.App_Start;
using M1.API.DTOs.Core;
using M1.Cipher;
using M1.Core;

namespace M1.API.Models.Core;

public class ApiKeyService
{
	public static readonly ApiKeyService Current = new ApiKeyService();

	private SemaphoreSlim s_cacheLock = new SemaphoreSlim(1, 1);

	private static MemoryCache s_memCache = new MemoryCache("M1.API");

	private ApiKeyService()
	{
	}

	public async Task<APIMetadataDto> LoadApiKeyAsync(HttpRequestMessage request, string m1ModuleId, string apiID)
	{
		string cacheKey = m1ModuleId + ":" + apiID;
		if (s_memCache.Get(cacheKey) is APIMetadataDto result)
		{
			return result;
		}
		await s_cacheLock.WaitAsync();
		try
		{
			if (s_memCache.Get(cacheKey) is APIMetadataDto result2)
			{
				return result2;
			}
			using M1.Core.AppContext appContext = new M1.Core.AppContext(designMode: false, loadMetadata: false);
			APIMetadataDto aPIMetadataDto = await GetApiKeyFromDataDictionaryAsync(request, appContext, apiID, m1ModuleId);
			if (aPIMetadataDto != null)
			{
				s_memCache.Set(cacheKey, aPIMetadataDto, DateTimeOffset.Now.AddMinutes(5.0));
			}
			return aPIMetadataDto;
		}
		finally
		{
			s_cacheLock.Release();
		}
	}

	protected async Task<APIMetadataDto> GetApiKeyFromDataDictionaryAsync(HttpRequestMessage request, M1.Core.AppContext context, string apiId, string m1ModuleId)
	{
		if (APIStartup.IsHosted)
		{
			return await GetHostedApiMetadataAsync(request, context, apiId, m1ModuleId);
		}
		return await GetOnPremisesApiMetadataAsync(context, apiId, m1ModuleId);
	}

	private static string GetMetaData(Dictionary<string, string> metadata, string key, string defaultValue = "")
	{
		string result = defaultValue;
		if (metadata.ContainsKey(key))
		{
			result = metadata[key].ToString();
		}
		return result;
	}

	private async Task<APIMetadataDto> GetHostedApiMetadataAsync(HttpRequestMessage request, M1.Core.AppContext context, string apiId, string m1ModuleId)
	{
		_ = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		Guid result = Guid.Empty;
		if (m1ModuleId == "ERP")
		{
			IEnumerable<string> source = default(IEnumerable<string>);
			if (!((HttpHeaders)request.Headers).TryGetValues("companyId", ref source))
			{
				return null;
			}
			string text = source.LastOrDefault();
			if (text == null || !Guid.TryParse(text, out result))
			{
				return null;
			}
		}
		else
		{
			Guid.TryParse(apiId, out result);
			apiId = result.ToString("N").ToUpper();
		}
		new DataTable();
		if (result != Guid.Empty)
		{
			DataTable dtTemp = new DataTable();
			stringBuilder.AppendFormat("server={0};database={1};integrated Security=SSPI", context.Registry.MetadataServer, context.Registry.MetadataDB);
			using (SqlConnection sqlConnection = new SqlConnection(stringBuilder.ToString()))
			{
				using SqlCommand sqlCommand = new SqlCommand("dbo.GetMetaDatabyGUID", sqlConnection);
				sqlCommand.Parameters.AddWithValue("@CustomerGUID", result);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.Connection = sqlConnection;
				sqlConnection.Open();
				dtTemp.Load(sqlCommand.ExecuteReader());
			}
			if (dtTemp.Rows.Count > 0)
			{
				DataRow row = dtTemp.Rows[0];
				Dictionary<string, string> metadata = row.Table.Columns.Cast<DataColumn>().ToDictionary((DataColumn c) => c.ColumnName, (DataColumn c) => row[c].ToString());
				context.DDServerManager.ConnectionInfo.Server = GetMetaData(metadata, "Server_Instance");
				context.DDServerManager.ConnectionInfo.NetworkLibrary = context.Registry.NetworkLibrary;
				context.DDServerManager.ConnectionInfo.SqlUserID = GetMetaData(metadata, "M1Admin_User");
				context.DDServerManager.sqlPassword = Cipher_Static.Decrypt(GetMetaData(metadata, "M1Admin_PW"), result.ToString().ToLower());
				context.DDServerManager.ConnectionInfo.TrustedConnection = true;
				string dataDictionary = GetMetaData(metadata, "APIDD");
				dataDictionary = (string.IsNullOrWhiteSpace(dataDictionary) ? "M1DD" : dataDictionary);
				using SqlConnection connection = context.DDServerManager.GetConnection(null, dataDictionary, openImmediately: false);
				DataTable table = new DataTable();
				using (connection)
				{
					using SqlCommand command = new SqlCommand();
					command.Connection = connection;
					command.CommandText = "\r\nSELECT [daAPIID],[daAPIKey],[daModuleID],[daDatabaseID],[daAPIUserID],[daAPIUserPWD],[daExtraSettings],[daIsReadOnly]\r\nFROM DDAPIINFO \r\nWHERE daAPIID=@apiID AND daModuleID=@moduleID";
					command.Parameters.AddWithValue("@apiID", apiId);
					command.Parameters.AddWithValue("@moduleID", m1ModuleId);
					await connection.OpenAsync();
					DataTable dataTable = table;
					dataTable.Load(await command.ExecuteReaderAsync());
				}
				IEnumerator enumerator = table.Rows.GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						DataRow row2 = (DataRow)enumerator.Current;
						return new APIMetadataDto(row2.Field<string>("daModuleID").Trim(), row2.Field<string>("daAPIID").Trim(), row2.Field<string>("daAPIKey").Trim(), row2.Field<string>("daAPIUserID").Trim(), row2.Field<string>("daAPIUserPWD").Trim(), dataDictionary, context.DDServerManager.ConnectionInfo.Server, context.Registry.NetworkLibrary, context.DDServerManager.ConnectionInfo.TrustedConnection, row2.Field<bool>("daIsReadOnly"))
						{
							DatabaseId = row2.Field<string>("daDatabaseID"),
							ExtraSettings = GetExtraSettingsDictionary(row2.Field<string>("daExtraSettings")),
							SqlUserID = context.DDServerManager.ConnectionInfo.SqlUserID,
							SqlPassword = context.DDServerManager.sqlPassword
						};
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			dtTemp.Dispose();
		}
		return null;
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

	private async Task<APIMetadataDto> GetOnPremisesApiMetadataAsync(M1.Core.AppContext context, string apiId, string m1ModuleId)
	{
		DataTable dtTable = new DataTable();
		_ = string.Empty;
		_ = string.Empty;
		using SqlCommand command = new SqlCommand();
		string dataDictionary = context.Server.IniSettings.Get("DataDictionary", "M1DD");
		context.DDServerManager.ConnectionInfo.Server = context.Server.IniSettings.Get("DBServer", "(local)");
		context.DDServerManager.ConnectionInfo.SqlUserID = context.Server.IniSettings.Get("DBUserID", "sa");
		context.DDServerManager.sqlPassword = context.Server.IniSettings.Get("DBPwd", string.Empty);
		context.DDServerManager.sqlPassword = (string.IsNullOrEmpty(context.DDServerManager.sqlPassword) ? string.Empty : context.DBServerManager.Decrypt(context.DDServerManager.sqlPassword));
		context.DDServerManager.ConnectionInfo.TrustedConnection = context.Server.IniSettings.GetAsBool("DBTrustedConnection", defaultValue: false);
		string networkLibrary = context.Server.IniSettings.Get("DBNetworkLibrary", "dbmssocn");
		context.DDServerManager.ConnectionInfo.NetworkLibrary = networkLibrary;
		using (SqlConnection conn = context.DDServerManager.GetConnection(null, dataDictionary, openImmediately: false))
		{
			command.Connection = conn;
			command.CommandText = "\r\nSELECT [daAPIID],[daAPIKey],[daModuleID],[daDatabaseID],[daAPIUserID],[daAPIUserPWD],[daExtraSettings],[daIsReadOnly]\r\nFROM DDAPIINFO\r\nWHERE daAPIID=@apiID AND daModuleID=@moduleID";
			command.Parameters.AddWithValue("@apiID", apiId);
			command.Parameters.AddWithValue("@moduleID", m1ModuleId);
			await conn.OpenAsync();
			dtTable.Load(command.ExecuteReader());
			IEnumerator enumerator = dtTable.Rows.GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					DataRow row = (DataRow)enumerator.Current;
					return new APIMetadataDto(row.Field<string>("daModuleID").Trim(), row.Field<string>("daAPIID").Trim(), row.Field<string>("daAPIKey").Trim(), row.Field<string>("daAPIUserID").Trim(), row.Field<string>("daAPIUserPWD").Trim(), dataDictionary, context.DDServerManager.ConnectionInfo.Server, context.Registry.NetworkLibrary, context.DDServerManager.ConnectionInfo.TrustedConnection, row.Field<bool>("daIsReadOnly"))
					{
						DatabaseId = row.Field<string>("daDatabaseID"),
						ExtraSettings = GetExtraSettingsDictionary(row.Field<string>("daExtraSettings")),
						SqlUserID = context.DDServerManager.ConnectionInfo.SqlUserID,
						SqlPassword = context.DDServerManager.sqlPassword
					};
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
		return null;
	}
}
