using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert QuoteOperations to support unicode", "2013-10-17")]
public class v810RebuildQuoteOperations
{
	public v810RebuildQuoteOperations(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteOperations", new DmoField[60]
		{
			new DmoField("qmoQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmoQuoteLineID", "smallint", 4, 0, nullable: false),
			new DmoField("qmoQuoteAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("qmoQuoteOperationID", "int", 5, 0, nullable: false),
			new DmoField("qmoOperationType", "tinyint", 1, 0, nullable: false),
			new DmoField("qmoPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmoPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmoWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmoProcessID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmoProcessShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qmoProcessLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmoProcessLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmoQuantityPerAssembly", "numeric", 13, 6, nullable: false),
			new DmoField("qmoSetupHours", "numeric", 8, 2, nullable: false),
			new DmoField("qmoAdditionalSetupQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("qmoAdditionalSetupHours", "numeric", 6, 2, nullable: false),
			new DmoField("qmoProductionStandard", "numeric", 10, 4, nullable: false),
			new DmoField("qmoStandardFactor", "nvarchar", 2, 0, nullable: false),
			new DmoField("qmoQuotingRate", "numeric", 8, 2, nullable: false),
			new DmoField("qmoSetupRate", "numeric", 8, 2, nullable: false),
			new DmoField("qmoProductionRate", "numeric", 8, 2, nullable: false),
			new DmoField("qmoOverheadRate", "numeric", 8, 2, nullable: false),
			new DmoField("qmoOverlap", "tinyint", 1, 0, nullable: false),
			new DmoField("qmoMachineType", "tinyint", 1, 0, nullable: false),
			new DmoField("qmoWorkCenterMachineID", "smallint", 3, 0, nullable: false),
			new DmoField("qmoPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("qmoPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("qmoSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmoPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmoEstimatedUnitCost", "numeric", 15, 5, nullable: false),
			new DmoField("qmoMinimumCharge", "numeric", 8, 2, nullable: false),
			new DmoField("qmoSetupCharge", "numeric", 9, 2, nullable: false),
			new DmoField("qmoQuantityBreak1", "numeric", 15, 5, nullable: false),
			new DmoField("qmoUnitCost1", "numeric", 15, 5, nullable: false),
			new DmoField("qmoQuantityBreak2", "numeric", 15, 5, nullable: false),
			new DmoField("qmoUnitCost2", "numeric", 15, 5, nullable: false),
			new DmoField("qmoQuantityBreak3", "numeric", 15, 5, nullable: false),
			new DmoField("qmoUnitCost3", "numeric", 15, 5, nullable: false),
			new DmoField("qmoQuantityBreak4", "numeric", 15, 5, nullable: false),
			new DmoField("qmoUnitCost4", "numeric", 15, 5, nullable: false),
			new DmoField("qmoQuantityBreak5", "numeric", 15, 5, nullable: false),
			new DmoField("qmoUnitCost5", "numeric", 15, 5, nullable: false),
			new DmoField("qmoQuantityBreak6", "numeric", 15, 5, nullable: false),
			new DmoField("qmoUnitCost6", "numeric", 15, 5, nullable: false),
			new DmoField("qmoQuantityBreak7", "numeric", 15, 5, nullable: false),
			new DmoField("qmoUnitCost7", "numeric", 15, 5, nullable: false),
			new DmoField("qmoQuantityBreak8", "numeric", 15, 5, nullable: false),
			new DmoField("qmoUnitCost8", "numeric", 15, 5, nullable: false),
			new DmoField("qmoQuantityBreak9", "numeric", 15, 5, nullable: false),
			new DmoField("qmoUnitCost9", "numeric", 15, 5, nullable: false),
			new DmoField("qmoDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmoSFEMessageRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmoSFEMessageText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmoInspectionType", "tinyint", 1, 0, nullable: false),
			new DmoField("qmoClosed", "bit", 1, 0, nullable: false),
			new DmoField("qmoMachinesToSchedule", "smallint", 3, 0, nullable: false),
			new DmoField("qmoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qmoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qmoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[13]
		{
			new DmoIndex("QMOQUOTEID,QMOQUOTELINEID,QMOQUOTEASSEMBLYID,QMOQUOTEOPERATIONID", unique: true),
			new DmoIndex("QMOUNIQUEID", unique: true),
			new DmoIndex("qmoQuoteID", unique: false),
			new DmoIndex("qmoQuoteLineID", unique: false),
			new DmoIndex("qmoQuoteAssemblyID", unique: false),
			new DmoIndex("qmoQuoteOperationID", unique: false),
			new DmoIndex("qmoPlantDepartmentID", unique: false),
			new DmoIndex("qmoPlantID", unique: false),
			new DmoIndex("qmoWorkCenterMachineID", unique: false),
			new DmoIndex("qmoPartID", unique: false),
			new DmoIndex("qmoPartRevisionID", unique: false),
			new DmoIndex("qmoInspectionType", unique: false),
			new DmoIndex("qmoClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
