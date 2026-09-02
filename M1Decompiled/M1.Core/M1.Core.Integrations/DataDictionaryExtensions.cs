using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace M1.Core.Integrations;

public static class DataDictionaryExtensions
{
	public static IReadOnlyList<IntegrationServiceInfoRecord> GetIntegrations(this M1DataDictionary dd, IntegrationServiceInfoRecordType integrationType)
	{
		DataTable dataTable = dd.GetDataTable("\r\nSELECT diIntegrationType, diUsername, diPassword, diDatabaseId, \r\ndiPollingFrequency, diInactive, diIsSynced, diTenantId\r\nFROM IntegrationServiceInfo \r\nWHERE diIntegrationType = '" + integrationType.ToString() + "';");
		List<IntegrationServiceInfoRecord> list = new List<IntegrationServiceInfoRecord>();
		foreach (DataRow row in dataTable.Rows)
		{
			IntegrationServiceInfoRecord integrationServiceInfoRecord = new IntegrationServiceInfoRecord();
			if (!Enum.TryParse<IntegrationServiceInfoRecordType>(Convert.ToString(row["diIntegrationType"]), ignoreCase: true, out var result))
			{
				throw new Exception(string.Format("Invalid value for diIntegrationType {0}.", row["diIntegrationType"]));
			}
			integrationServiceInfoRecord.IntegrationType = result;
			integrationServiceInfoRecord.Username = Convert.ToString(row["diUsername"]);
			integrationServiceInfoRecord.Password = Convert.ToString(row["diPassword"]);
			integrationServiceInfoRecord.DatabaseId = Convert.ToString(row["diDatabaseId"]);
			integrationServiceInfoRecord.PollingFrequency = Convert.ToInt16(row["diPollingFrequency"]);
			integrationServiceInfoRecord.Inactive = Convert.ToBoolean(row["diInactive"]);
			integrationServiceInfoRecord.IsSynced = Convert.ToBoolean(row["diIsSynced"]);
			integrationServiceInfoRecord.TenantId = Convert.ToString(row["diTenantId"].ToString());
			list.Add(integrationServiceInfoRecord);
		}
		return list;
	}

	public static void InsertIntegration(this M1DataDictionary dd, IntegrationServiceInfoRecord integration)
	{
		using SqlCommand sqlCommand = dd.NewSqlCommand("\r\nINSERT INTO IntegrationServiceInfo \r\n    (diIntegrationType, diUsername, diPassword, diDatabaseId,\r\n    diPollingFrequency, diInactive, diTenantId, diIsSynced)\r\nVALUES (@IntegrationType, @Username, @Password, @DatabaseId, \r\n    @PollingFrequency, @Inactive, @TenantId, @IsSynced);");
		sqlCommand.Parameters.Add("@UserID", SqlDbType.NVarChar);
		sqlCommand.Parameters.Add("@IntegrationType", SqlDbType.NVarChar);
		sqlCommand.Parameters.Add("@Username", SqlDbType.NVarChar);
		sqlCommand.Parameters.Add("@Password", SqlDbType.NVarChar);
		sqlCommand.Parameters.Add("@DatabaseId", SqlDbType.NVarChar);
		sqlCommand.Parameters.Add("@PollingFrequency", SqlDbType.SmallInt);
		sqlCommand.Parameters.Add("@Inactive", SqlDbType.Bit);
		sqlCommand.Parameters.Add("@TenantId", SqlDbType.NVarChar);
		sqlCommand.Parameters.Add("@IsSynced", SqlDbType.Bit);
		sqlCommand.Parameters["@UserID"].Value = integration.Username;
		sqlCommand.Parameters["@IntegrationType"].Value = integration.IntegrationType.ToString();
		sqlCommand.Parameters["@Username"].Value = integration.Username;
		sqlCommand.Parameters["@Password"].Value = integration.Password;
		sqlCommand.Parameters["@DatabaseId"].Value = integration.DatabaseId;
		sqlCommand.Parameters["@PollingFrequency"].Value = integration.PollingFrequency;
		sqlCommand.Parameters["@Inactive"].Value = integration.Inactive;
		sqlCommand.Parameters["@TenantId"].Value = integration.TenantId;
		sqlCommand.Parameters["@IsSynced"].Value = integration.IsSynced;
		sqlCommand.Connection.Open();
		using (sqlCommand.Connection)
		{
			sqlCommand.ExecuteNonQuery();
		}
	}

	public static Guid? GetCompanyId(this M1DataDictionary dd)
	{
		DataTable dataTable = dd.GetDataTable("SELECT ddCompanyId FROM DDInfo;");
		if (dataTable.Rows.Count > 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			if (!string.IsNullOrWhiteSpace(dataRow["ddCompanyId"].ToString()))
			{
				return Guid.Parse(dataRow["ddCompanyId"].ToString());
			}
		}
		return null;
	}

	public static string GetWebRegionId(this M1DataDictionary dd)
	{
		DataTable dataTable = dd.GetDataTable("SELECT ddWebRegion FROM DDInfo");
		if (dataTable.Rows.Count > 0)
		{
			string text = dataTable.Rows[0]["ddWebRegion"].ToString();
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
		}
		return string.Empty;
	}

	public static M1CloudCredentials GetCloudCredentials(this M1DataDictionary dd)
	{
		Guid? companyId = dd.GetCompanyId();
		if (!companyId.HasValue)
		{
			throw new Exception("Cloud connection company is not configured.");
		}
		DataTable dataTable = dd.GetDataTable("select diUsername, diPassword From IntegrationServiceInfo WHERE diIntegrationType='CloudConnection'");
		if (dataTable.Rows.Count <= 0)
		{
			throw new Exception("Cloud connection credentials are not configured.");
		}
		DataRow dataRow = dataTable.Rows[0];
		return new M1CloudCredentials
		{
			CompanyId = companyId.Value,
			Username = Convert.ToString(dataRow["diUsername"]),
			EncryptedPassword = Convert.ToString(dataRow["diPassword"])
		};
	}

	public static void DisableIntegration(this M1DataDictionary dd, string databaseId, IntegrationServiceInfoRecordType type)
	{
		dd.ExecuteCommand("UPDATE IntegrationServiceInfo SET diInactive = 1 WHERE diDatabaseId = '" + databaseId + "'AND diIntegrationType = '" + type.ToString() + "';");
	}

	public static void EnableIntegration(this M1DataDictionary dd, string databaseId, IntegrationServiceInfoRecordType type)
	{
		dd.ExecuteCommand("UPDATE IntegrationServiceInfo SET diInactive = 0 WHERE diDatabaseId = '" + databaseId + "' AND diIntegrationType = '" + type.ToString() + "';");
	}

	public static void RemoveIntegration(this M1DataDictionary dd, string databaseId, IntegrationServiceInfoRecordType type)
	{
		dd.ExecuteCommand("DELETE FROM IntegrationServiceInfo WHERE diDatabaseId = '" + databaseId + "' AND diIntegrationType = '" + type.ToString() + "';");
	}

	public static void RemoveCloudConnection(this M1DataDictionary dd)
	{
		dd.ExecuteCommand("DELETE FROM IntegrationServiceInfo WHERE diIntegrationType='CloudConnection'");
		dd.ExecuteCommand("DELETE FROM IntegrationServiceInfo WHERE diIntegrationType='ShopFloor'");
	}
}
