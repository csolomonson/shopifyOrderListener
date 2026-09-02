using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using M1.Cipher;
using M1.Extensions;
using SimpleImpersonation;

namespace M1.Core;

public class MetadataInfo
{
	public string FileShareLocation = string.Empty;

	public Dictionary<string, string> Metadata = new Dictionary<string, string>();

	public void LoadMetadata(AppContext context)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("server={0} ;database={1};integrated Security=SSPI", context.Registry.MetadataServer, context.Registry.MetadataDB);
		SqlConnection sqlConnection = new SqlConnection(stringBuilder.ToString());
		SqlCommand sqlCommand = new SqlCommand("dbo.GetMetaDataByOu", sqlConnection);
		sqlCommand.CommandType = CommandType.StoredProcedure;
		sqlConnection.Open();
		DataTable dataTable = new DataTable();
		dataTable.Load(sqlCommand.ExecuteReader());
		if (dataTable.Rows.Count > 0)
		{
			DataRow row = dataTable.Rows[0];
			Metadata = row.Table.Columns.Cast<DataColumn>().ToDictionary((DataColumn c) => c.ColumnName, (DataColumn c) => row[c].ToString());
		}
		dataTable.Dispose();
		sqlCommand.Dispose();
		sqlCommand = new SqlCommand("dbo.GetGUIDByOu", sqlConnection);
		sqlCommand.CommandType = CommandType.StoredProcedure;
		dataTable = new DataTable();
		dataTable.Load(sqlCommand.ExecuteReader());
		if (dataTable.Rows.Count > 0)
		{
			DataRow row2 = dataTable.Rows[0];
			foreach (KeyValuePair<string, string> item in row2.Table.Columns.Cast<DataColumn>().ToDictionary((DataColumn c) => c.ColumnName, (DataColumn c) => row2[c].ToString()))
			{
				if (item.Key.Equals("GUID", StringComparison.CurrentCultureIgnoreCase))
				{
					Metadata.Add(item.Key, item.Value.ToLower());
				}
				else
				{
					Metadata.Add(item.Key, item.Value);
				}
			}
		}
		sqlConnection.Close();
		FileShareLocation = GetMetaData("FileShare_Path").AddBackslash();
		Metadata.Add("OuName", GetOUName());
		Metadata.Add("DataDictionary", GetDataDictionary());
	}

	public string GetMetaData(string key, string defaultValue = "")
	{
		string result = defaultValue;
		if (Metadata.ContainsKey(key))
		{
			result = Metadata[key].ToString();
		}
		return result;
	}

	private string GetOUName()
	{
		string result = string.Empty;
		try
		{
			using PrincipalContext context = new PrincipalContext(ContextType.Domain, Environment.UserDomainName);
			UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, Environment.UserName);
			if (userPrincipal != null)
			{
				result = ((DirectoryEntry)userPrincipal.GetUnderlyingObject()).Properties["company"][0].ToString();
			}
		}
		catch
		{
		}
		return result;
	}

	private string GetDataDictionary()
	{
		string result = string.Empty;
		string userName = Environment.UserName;
		string metaData = GetMetaData("Elevated_User");
		if (string.Compare(userName, metaData, ignoreCase: true) == 0)
		{
			result = ReallyGetDataDictionary();
		}
		else
		{
			string password = Cipher_Static.Decrypt(GetMetaData("Elevated_PW"), GetMetaData("GUID"));
			using (Impersonation.LogonUser(Environment.UserDomainName, metaData, password, LogonType.NewCredentials))
			{
				result = ReallyGetDataDictionary();
			}
		}
		return result;
	}

	private string ReallyGetDataDictionary()
	{
		string result = string.Empty;
		SqlConnection sqlConnection = new SqlConnection(string.Format("server={0};integrated Security=SSPI;database=MASTER", GetMetaData("Server_Instance").ToString()).ToString());
		SqlCommand sqlCommand = new SqlCommand();
		sqlCommand.Connection = sqlConnection;
		sqlCommand.CommandText = $"WITH tmpDB(DataBaseName) AS(SELECT[Name] As[DatabaseName]FROM master.sys.databases WHERE[name] NOT IN('master', 'model', 'tempdb', 'msdb')) SELECT[DatabaseName] as DataDictionary FROM tmpDB WHERE HAS_DBACCESS([DatabaseName]) = 1 AND (OBJECT_ID(N'[' + DataBaseName + N'].[dbo].[DDInfo]', N'U') IS NOT NULL) ORDER BY[DatabaseName]";
		sqlConnection.Open();
		DataTable dataTable = new DataTable();
		dataTable.Load(sqlCommand.ExecuteReader());
		if (dataTable.Rows.Count > 0)
		{
			_ = dataTable.Rows[0];
			if (dataTable.Columns.Contains("DataDictionary"))
			{
				result = dataTable.Rows[0]["DataDictionary"].ToString();
			}
		}
		sqlConnection.Close();
		return result;
	}

	public void LoadThirdPartyMetadata(ref AppContext context, string connectionString = "")
	{
		_ = string.Empty;
		string empty = string.Empty;
		SqlConnection sqlConnection = new SqlConnection(connectionString);
		if (context != null && context.IsHosted)
		{
			context.DDServerManager.ConnectionInfo.Server = GetMetaData("Server_Instance");
			context.DDServerManager.ConnectionInfo.NetworkLibrary = string.Empty;
			context.DDServerManager.ConnectionInfo.SqlUserID = GetMetaData("M1Admin_User");
			context.DDServerManager.sqlPassword = Cipher_Static.Decrypt(GetMetaData("M1Admin_PW"), GetMetaData("GUID"));
			connectionString = string.Format("server={0};integrated Security=SSPI;database={1}", GetMetaData("Server_Instance"), GetMetaData("DataDictionary"));
			sqlConnection = new SqlConnection(connectionString.ToString());
		}
		else if (string.IsNullOrEmpty(connectionString))
		{
			context.Server.IniSettings.LoadM1IniSettings(context.Server.Location + "m1.ini");
			string databaseName = context.Server.IniSettings.Get("DataDictionary", "M1DD");
			context.DDServerManager.ConnectionInfo.Server = context.Server.IniSettings.Get("DBServer", "(local)");
			context.DDServerManager.ConnectionInfo.SqlUserID = context.Server.IniSettings.Get("DBUserID", "sa");
			context.DDServerManager.sqlPassword = context.Server.IniSettings.Get("DBPwd", string.Empty);
			context.DDServerManager.sqlPassword = (string.IsNullOrEmpty(context.DDServerManager.sqlPassword) ? string.Empty : context.DBServerManager.Decrypt(context.DDServerManager.sqlPassword));
			context.DDServerManager.ConnectionInfo.TrustedConnection = context.Server.IniSettings.GetAsBool("DBTrustedConnection", defaultValue: false);
			context.DDServerManager.ConnectionInfo.Server = context.Server.IniSettings.Get("DBServer", "(local)");
			context.DDServerManager.ConnectionInfo.NetworkLibrary = context.Server.IniSettings.Get("DBNetworkLibrary", "dbmssocn");
			sqlConnection = context.DDServerManager.GetConnection(null, databaseName, openImmediately: false);
		}
		SqlCommand sqlCommand = new SqlCommand();
		sqlCommand.Connection = sqlConnection;
		sqlCommand.CommandText = $"SELECT ddEasyOrder, ddEDI, ddMobile FROM DDInfo";
		sqlConnection.Open();
		DataTable dataTable = new DataTable();
		dataTable.Load(sqlCommand.ExecuteReader());
		if (dataTable.Rows.Count > 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			foreach (KeyValuePair<ThirdParty, ThirdPartyDefinition> entity in ThirdPartyInfo.Entities)
			{
				Dictionary<string, string> dictionary = ThirdPartyInfo.ConvertStringToProperties(((dataRow[entity.Value.Field] == null) ? string.Empty : dataRow[entity.Value.Field]).ToString());
				ThirdPartyInfo.Entities[entity.Key].Properties.Clear();
				foreach (KeyValuePair<string, string> item in dictionary)
				{
					empty = $"{entity.Key.ToString()}_{item.Key}";
					if (!Metadata.ContainsKey(empty))
					{
						Metadata.Add(empty, item.Value);
					}
					else
					{
						Metadata[empty] = item.Value;
					}
					ThirdPartyInfo.Set(entity.Key, item.Key, item.Value);
				}
			}
		}
		sqlConnection.Close();
	}
}
