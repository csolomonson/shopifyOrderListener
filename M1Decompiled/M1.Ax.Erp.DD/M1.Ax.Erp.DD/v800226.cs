using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.226", "Refresh boolean fields in properties tables", "2012-02-26")]
public class v800226
{
	public v800226(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE DatasetProperties SET xadExtendedSearchOptions = 1 Where xadExtendedSearchOptions = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE DatasetProperties SET xadExportFollowups = 1 Where xadExportFollowups = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE DatasetProperties SET xadDisableRetention = 1 Where xadDisableRetention = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafAgeByDaysInMonth = 1 Where xafAgeByDaysInMonth = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafAPExpressPost = 1 Where xafAPExpressPost = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafARExpressPost = 1 Where xafARExpressPost = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafPAAssignNumbersToEFT = 1 Where xafPAAssignNumbersToEFT = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafPAShowHolidaysForSalary = 1 Where xafPAShowHolidaysForSalary = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafPAExpressPost = 1 Where xafPAExpressPost = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafGLExpressPost = 1 Where xafGLExpressPost = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafPADeleteZeroPayHeaders = 1 Where xafPADeleteZeroPayHeaders = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafAPAssignNumbersToEFT = 1 Where xafAPAssignNumbersToEFT = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafIncludeLLInTermination = 1 Where xafIncludeLLInTermination = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FInancialProperties SET xafARIncludeTaxInDepositCalc = 1 Where xafARIncludeTaxInDepositCalc = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCPromptForLaborDescription = 1 Where xapDCPromptForLaborDescription = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCPromptForReason = 1 Where xapDCPromptForReason = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCSplitDirectLaborHours = 1 Where xapDCSplitDirectLaborHours = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapCMCustomerTaxable = 1 Where xapCMCustomerTaxable = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCEnableWorkQueue = 1 Where xapDCEnableWorkQueue = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapJMExcessQuantity = 1 Where xapJMExcessQuantity = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapOMIncludeOrderLineInJob = 1 Where xapOMIncludeOrderLineInJob = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapOMIncludeOrderDeliveryInJob = 1 Where xapOMIncludeOrderDeliveryInJob = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCPromptForClockInPassword = 1 Where xapDCPromptForClockInPassword = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCPromptForActivityPassword = 1 Where xapDCPromptForActivityPassword = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCPromptForAuditPassword = 1 Where xapDCPromptForAuditPassword = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCEnableCreateSequence = 1 Where xapDCEnableCreateSequence = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMTransferCustomer = 1 Where xapIMTransferCustomer = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMOverwriteMethod = 1 Where xapIMOverwriteMethod = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCEnableIssueMaterial = 1 Where xapDCEnableIssueMaterial = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapSMDeleteZeroShipmentLines = 1 Where xapSMDeleteZeroShipmentLines = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCSplitIndirectLaborHours = 1 Where xapDCSplitIndirectLaborHours = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCEnableTimecardAudit = 1 Where xapDCEnableTimecardAudit = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMRefreshMaterial = 1 Where xapIMRefreshMaterial = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMRefreshMaterialCosts = 1 Where xapIMRefreshMaterialCosts = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMTransferMaterial = 1 Where xapIMTransferMaterial = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMCopyPartMemos = 1 Where xapIMCopyPartMemos = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMCopyPartRules = 1 Where xapIMCopyPartRules = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMCopyPartOrgReferences = 1 Where xapIMCopyPartOrgReferences = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapDCAllowNegativeQty = 1 Where xapDCAllowNegativeQty = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMOverwriteDocuments = 1 Where xapIMOverwriteDocuments = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapJMMinimizeGaps = 1 Where xapJMMinimizeGaps = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapQMOverwriteMethod = 1 Where xapQMOverwriteMethod = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapQMOverwriteDocuments = 1 Where xapQMOverwriteDocuments = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapQMRefreshMaterial = 1 Where xapQMRefreshMaterial = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapQMRefreshMaterialCosts = 1 Where xapQMRefreshMaterialCosts = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapJMOverwriteMethod = 1 Where xapJMOverwriteMethod = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapJMOverwriteDocuments = 1 Where xapJMOverwriteDocuments = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapJMRefreshMaterial = 1 Where xapJMRefreshMaterial = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapJMRefreshMaterialCosts = 1 Where xapJMRefreshMaterialCosts = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapIMTransferDescriptions = 1 Where xapIMTransferDescriptions = 1");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapCMCreditLimitSourceOrder = 1 Where xapCMCreditLimitSourceOrder = 1");
	}
}
