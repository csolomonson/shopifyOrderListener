using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartOperations to support unicode", "2013-10-17")]
public class v810RebuildPartOperations
{
	public v810RebuildPartOperations(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOperations", new DmoField[53]
		{
			new DmoField("imoMethodID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imoMethodRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imoMethodAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("imoMethodOperationID", "int", 5, 0, nullable: false),
			new DmoField("imoOperationType", "tinyint", 1, 0, nullable: false),
			new DmoField("imoPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imoPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imoWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imoProcessID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imoProcessShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imoProcessLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imoProcessLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imoQuantityPerAssembly", "numeric", 13, 6, nullable: false),
			new DmoField("imoSetupHours", "numeric", 8, 2, nullable: false),
			new DmoField("imoProductionStandard", "numeric", 10, 4, nullable: false),
			new DmoField("imoStandardFactor", "nvarchar", 2, 0, nullable: false),
			new DmoField("imoOverlap", "tinyint", 1, 0, nullable: false),
			new DmoField("imoMachineType", "tinyint", 1, 0, nullable: false),
			new DmoField("imoWorkCenterMachineID", "smallint", 3, 0, nullable: false),
			new DmoField("imoPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imoPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("imoEstimatedUnitCost", "numeric", 15, 5, nullable: false),
			new DmoField("imoMinimumCharge", "numeric", 8, 2, nullable: false),
			new DmoField("imoSetupCharge", "numeric", 9, 2, nullable: false),
			new DmoField("imoSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("imoPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imoDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imoSFEMessageRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imoSFEMessageText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("imoInspectionType", "tinyint", 1, 0, nullable: false),
			new DmoField("imoMachinesToSchedule", "smallint", 3, 0, nullable: false),
			new DmoField("imoQuantityBreak1", "numeric", 15, 5, nullable: false),
			new DmoField("imoUnitCost1", "numeric", 15, 5, nullable: false),
			new DmoField("imoQuantityBreak2", "numeric", 15, 5, nullable: false),
			new DmoField("imoUnitCost2", "numeric", 15, 5, nullable: false),
			new DmoField("imoQuantityBreak3", "numeric", 15, 5, nullable: false),
			new DmoField("imoUnitCost3", "numeric", 15, 5, nullable: false),
			new DmoField("imoQuantityBreak4", "numeric", 15, 5, nullable: false),
			new DmoField("imoUnitCost4", "numeric", 15, 5, nullable: false),
			new DmoField("imoQuantityBreak5", "numeric", 15, 5, nullable: false),
			new DmoField("imoUnitCost5", "numeric", 15, 5, nullable: false),
			new DmoField("imoQuantityBreak6", "numeric", 15, 5, nullable: false),
			new DmoField("imoUnitCost6", "numeric", 15, 5, nullable: false),
			new DmoField("imoQuantityBreak7", "numeric", 15, 5, nullable: false),
			new DmoField("imoUnitCost7", "numeric", 15, 5, nullable: false),
			new DmoField("imoQuantityBreak8", "numeric", 15, 5, nullable: false),
			new DmoField("imoUnitCost8", "numeric", 15, 5, nullable: false),
			new DmoField("imoQuantityBreak9", "numeric", 15, 5, nullable: false),
			new DmoField("imoUnitCost9", "numeric", 15, 5, nullable: false),
			new DmoField("imoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("IMOMETHODID,IMOMETHODREVISIONID,IMOMETHODASSEMBLYID,IMOMETHODOPERATIONID", unique: true),
			new DmoIndex("IMOUNIQUEID", unique: true),
			new DmoIndex("imoMethodID", unique: false),
			new DmoIndex("imoMethodRevisionID", unique: false),
			new DmoIndex("imoMethodAssemblyID", unique: false),
			new DmoIndex("imoMethodOperationID", unique: false),
			new DmoIndex("imoPlantDepartmentID", unique: false),
			new DmoIndex("imoPlantID", unique: false),
			new DmoIndex("imoWorkCenterMachineID", unique: false),
			new DmoIndex("imoPartID", unique: false),
			new DmoIndex("imoPartRevisionID", unique: false),
			new DmoIndex("imoInspectionType", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
