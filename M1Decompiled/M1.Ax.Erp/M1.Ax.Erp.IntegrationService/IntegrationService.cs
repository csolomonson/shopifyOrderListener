using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.IntegrationService;

public class IntegrationService
{
	public bool CreateTransactionQueueRecord(M1Database database, SqlTransaction transaction, IntegrationServiceConstants.IntegrationType integrationType, IntegrationServiceConstants.ApiAction apiAction, IntegrationServiceConstants.EntityType entityType, IntegrationServiceConstants.Status status, string sourceTableName, Guid sourceTableUniqueId, int integrationCustomModuleId)
	{
		if (!(database.GetService(typeof(M1DataDictionary)) is M1DataDictionary m1DataDictionary))
		{
			return false;
		}
		if (!m1DataDictionary.ProductCode.IsCustomModulePurchased(integrationCustomModuleId))
		{
			return false;
		}
		string text = string.Empty;
		if (IsIntegrationTypeInactive(m1DataDictionary, integrationType, database.ID))
		{
			status = IntegrationServiceConstants.Status.Failed;
			text = "The integration account you are using is marked as inactive. Please contact ECI Solutions to reactivate your integration.";
		}
		SqlCommand sqlCommand = database.NewSqlCommand("INSERT INTO IntegrationTransactionQueue (itqIntegrationType, itqApiAction, itqEntityType, itqStatus, itqSourceTableName, itqSourceTableUniqueID, itqErrorMessage, itqCreatedBy, itqCreatedDate) VALUES (@IntegrationType, @ApiAction, @EntityType, @Status, @SourceTableName, @SourceTableUniqueID, @ErrorMessage, @CreatedBy, @CreatedDate) ");
		sqlCommand.Parameters.Add(new SqlParameter("@IntegrationType", SqlDbType.NVarChar)).Value = integrationType;
		sqlCommand.Parameters.Add(new SqlParameter("@ApiAction", SqlDbType.NVarChar)).Value = apiAction;
		sqlCommand.Parameters.Add(new SqlParameter("@EntityType", SqlDbType.NVarChar)).Value = entityType;
		sqlCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar)).Value = status;
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableName", SqlDbType.NVarChar)).Value = sourceTableName;
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableUniqueID", SqlDbType.UniqueIdentifier)).Value = sourceTableUniqueId;
		sqlCommand.Parameters.Add(new SqlParameter("@ErrorMessage", SqlDbType.NVarChar)).Value = ((!string.IsNullOrWhiteSpace(text)) ? ((IConvertible)text) : ((IConvertible)DBNull.Value));
		sqlCommand.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar)).Value = database.User.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = DateTime.Now;
		return database.ExecuteCommand(sqlCommand, transaction) != 0;
	}

	public void ResetStatusInGrid(M1BindingSource bindingSource, IntegrationServiceConstants.IntegrationType integrationType, int integrationCustomModuleId, string integrationModuleRole)
	{
		bool num = bindingSource.Database.Security.IsInRole(integrationModuleRole);
		bool flag = IsIntegrationTypeInactive(bindingSource.DataDictionary, integrationType, bindingSource.Database.ID);
		if (!(!num || flag))
		{
			DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null && currentAsDataRow.Table.Columns.Contains("itqStatus") && currentAsDataRow.Table.Columns.Contains("itqStatusUpdatedDate") && currentAsDataRow.Field<string>("itqStatus").Equals("Failed", StringComparison.CurrentCultureIgnoreCase))
			{
				currentAsDataRow.SetField("itqStatus", "Pending");
				currentAsDataRow.SetField("itqStatusUpdatedDate", DateTime.Now);
				bindingSource.SaveData();
			}
		}
	}

	public bool DoesPendingRecordExist(M1Database database, SqlTransaction transaction, IntegrationServiceConstants.IntegrationType integrationType, IntegrationServiceConstants.EntityType entityType, string sourceTable, int integrationCustomModuleId)
	{
		if (!(database.GetService(typeof(M1DataDictionary)) is M1DataDictionary m1DataDictionary))
		{
			return false;
		}
		if (!m1DataDictionary.ProductCode.IsCustomModulePurchased(integrationCustomModuleId))
		{
			return false;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(Count(*),0) From IntegrationTransactionQueue Where itqIntegrationType = @IntegrationType And itqEntityType = @EntityType And itqStatus = @Status And itqSourceTableName = @SourceTableName");
		sqlCommand.Parameters.Add(new SqlParameter("@IntegrationType", SqlDbType.NVarChar)).Value = integrationType;
		sqlCommand.Parameters.Add(new SqlParameter("@EntityType", SqlDbType.NVarChar)).Value = entityType;
		sqlCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar)).Value = IntegrationServiceConstants.Status.Pending;
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableName", SqlDbType.NVarChar)).Value = sourceTable;
		return (int)database.ExecuteScalar(sqlCommand, transaction) != 0;
	}

	public bool TimecardsToTransferCheck(M1Database database, SqlTransaction transaction, IntegrationServiceConstants.IntegrationType integrationType, int integrationCustomModuleId)
	{
		if (!(database.GetService(typeof(M1DataDictionary)) is M1DataDictionary m1DataDictionary))
		{
			return false;
		}
		if (!m1DataDictionary.ProductCode.IsCustomModulePurchased(integrationCustomModuleId) || IsIntegrationTypeInactive(m1DataDictionary, integrationType, database.ID))
		{
			return false;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(Count(*),0) From Timecards Where lmpActive = 0 and lmpTransferredToPayroll = 0 and lmpPayrollHours <> 0");
		return (int)database.ExecuteScalar(sqlCommand, transaction) != 0;
	}

	public bool IsIntegrationTypeInactive(M1DataDictionary dataDictionary, IntegrationServiceConstants.IntegrationType integrationType, string databaseId)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select IsNull(Count(*),0) From IntegrationServiceInfo Where diIntegrationType = @IntegrationType and diDatabaseId = @DatabaseId and diInactive = 1");
		sqlCommand.Parameters.Add(new SqlParameter("@IntegrationType", SqlDbType.NVarChar)).Value = integrationType;
		sqlCommand.Parameters.Add(new SqlParameter("@DatabaseId", SqlDbType.NVarChar)).Value = databaseId;
		return (int)dataDictionary.ExecuteScalar(sqlCommand) != 0;
	}
}
