using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using M1.Cipher;

namespace M1.Core.Integrations;

public static class AppContextExtensions
{
	public static async Task UpdateDatabaseUniqueIdAsync(this AppContext appContext, string databaseId, Guid id)
	{
		string baseDatabaseConnectionString = appContext.GetBaseDatabaseConnectionString(databaseId);
		using SqlConnection connection = new SqlConnection(baseDatabaseConnectionString);
		await connection.OpenAsync().ConfigureAwait(continueOnCapturedContext: false);
		SqlCommand sqlCommand = connection.CreateCommand();
		sqlCommand.CommandText = "UPDATE DatasetProperties SET xadUniqueID = @UniqueID;";
		sqlCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.NVarChar)).Value = id.ToString();
		await sqlCommand.ExecuteNonQueryAsync();
		appContext.InstalledDatabases.Refresh();
	}

	public static async Task UpdateDatabaseNameAsync(this AppContext appContext, string databaseId, string name)
	{
		if (name.Length > 50)
		{
			name = name.Substring(0, 50);
		}
		string baseDatabaseConnectionString = appContext.GetBaseDatabaseConnectionString(databaseId);
		using SqlConnection connection = new SqlConnection(baseDatabaseConnectionString);
		await connection.OpenAsync().ConfigureAwait(continueOnCapturedContext: false);
		SqlCommand sqlCommand = connection.CreateCommand();
		sqlCommand.CommandText = "UPDATE DatasetProperties SET xadName = @Name;";
		sqlCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar)).Value = name;
		await sqlCommand.ExecuteNonQueryAsync();
		appContext.InstalledDatabases.Refresh();
	}

	public static DateTime? GetProcessedMessagesMaxDate(this AppContext appContext, string databaseId)
	{
		using (SqlConnection sqlConnection = new SqlConnection(appContext.GetBaseDatabaseConnectionString(databaseId)))
		{
			sqlConnection.Open();
			SqlCommand sqlCommand = sqlConnection.CreateCommand();
			sqlCommand.CommandText = "SELECT MAX(pmsProcessedDateTime) FROM ProcessedMessages;";
			object obj = sqlCommand.ExecuteScalar();
			if (obj is DateTime)
			{
				return (DateTime)obj;
			}
		}
		return null;
	}

	public static IReadOnlyCollection<IntegrationTransactionQueueSummary> GetIntegrationTransactionQueueStatus(this AppContext appContext, IntegrationServiceInfoRecordType type, string databaseId)
	{
		using SqlConnection sqlConnection = new SqlConnection(appContext.GetBaseDatabaseConnectionString(databaseId));
		sqlConnection.Open();
		SqlCommand sqlCommand = sqlConnection.CreateCommand();
		sqlCommand.CommandText = $"\r\nSELECT \r\n\titqStatus,\r\n\tCOUNT(*) AS StatusCount, \r\n\tMAX(itqStatusUpdatedDate) AS StatusDate\r\nFROM IntegrationTransactionQueue itq \r\nWHERE itqIntegrationType = '{type}'\r\nGROUP BY itqStatus";
		using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
		List<IntegrationTransactionQueueSummary> list = new List<IntegrationTransactionQueueSummary>(2);
		if (sqlDataReader.HasRows)
		{
			while (sqlDataReader.Read())
			{
				DateTime? maxDate = null;
				if (!string.IsNullOrWhiteSpace(sqlDataReader["StatusDate"].ToString()))
				{
					maxDate = Convert.ToDateTime(sqlDataReader["StatusDate"]);
				}
				list.Add(new IntegrationTransactionQueueSummary
				{
					Status = Convert.ToString(sqlDataReader["itqStatus"]),
					Count = Convert.ToInt32(sqlDataReader["StatusCount"]),
					MaxDate = maxDate
				});
			}
		}
		return list;
	}

	public static string GetBaseDatabaseConnectionString(this AppContext appContext, string databaseId)
	{
		bool flag = false;
		string text;
		string text2;
		string text3;
		string text4;
		if (appContext.IsHosted)
		{
			text = appContext.Metadata.GetMetaData("Server_Instance");
			text2 = string.Empty;
			text3 = appContext.Metadata.GetMetaData("M1Admin_User");
			text4 = Cipher_Static.Decrypt(appContext.Metadata.GetMetaData("M1Admin_PW"), appContext.Metadata.GetMetaData("GUID"));
		}
		else
		{
			text = appContext.Server.IniSettings.Get("DBServer", "(local)");
			text2 = appContext.Server.IniSettings.Get("DBNetworkLibrary", "dbmssocn");
			text3 = appContext.Server.IniSettings.Get("DBUserID", "sa");
			text4 = appContext.Server.IniSettings.Get("DBPwd", string.Empty);
			text4 = (string.IsNullOrWhiteSpace(text4) ? string.Empty : appContext.DBServerManager.Decrypt(text4));
			flag = appContext.Server.IniSettings.GetAsBool("DBTrustedConnection", defaultValue: false);
		}
		StringBuilder stringBuilder = new StringBuilder("Server=" + text + ";Network Library=" + text2 + ";" + (flag ? "Integrated Security=SSPI" : ("User Id=" + text3 + ";Password=" + text4)));
		stringBuilder.AppendFormat(";Database=" + databaseId);
		return stringBuilder.ToString();
	}

	public static async Task<IntegrationConfigurationRegion> GetCloudConfigurationAsync(this AppContext appContext, M1DataDictionary dataDictionary, string webRegionId = "")
	{
		string dISCOVERY_URL = appContext.Registry.DISCOVERY_URL;
		string environmentName = appContext.Registry.ENVIRONMENT_NAME ?? "Production";
		if (string.IsNullOrEmpty(webRegionId))
		{
			string webRegionId2 = dataDictionary.GetWebRegionId();
			webRegionId = (string.IsNullOrEmpty(webRegionId2) ? (appContext.Registry.ENVIRONMENT_REGION ?? "US") : webRegionId2);
		}
		return await IntegrationConfigurationDiscovery.GetIntegrationConfigurationRegionAsync(dISCOVERY_URL, environmentName, webRegionId);
	}

	public static async Task<IntegrationServiceConfiguration> GetIntegrationServiceConfigurationAsync(this AppContext appContext)
	{
		string dISCOVERY_URL = appContext.Registry.DISCOVERY_URL;
		string environmentName = appContext.Registry.ENVIRONMENT_NAME ?? "Production";
		return await IntegrationConfigurationDiscovery.GetIntegrationServiceConfigurationAsync(dISCOVERY_URL, environmentName);
	}

	public static int DeleteAllTransactionQueueRecords(this AppContext appContext, IntegrationServiceInfoRecordType type, string databaseId)
	{
		using SqlConnection sqlConnection = new SqlConnection(appContext.GetBaseDatabaseConnectionString(databaseId));
		sqlConnection.Open();
		SqlCommand sqlCommand = sqlConnection.CreateCommand();
		sqlCommand.CommandText = "\r\nDELETE FROM\r\nIntegrationTransactionQueue \r\nWHERE itqIntegrationType = '" + type.ToString() + "'";
		return sqlCommand.ExecuteNonQuery();
	}
}
