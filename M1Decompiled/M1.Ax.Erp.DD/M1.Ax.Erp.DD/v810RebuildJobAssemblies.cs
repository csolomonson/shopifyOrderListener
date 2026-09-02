using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert JobAssemblies to support unicode", "2013-10-17")]
public class v810RebuildJobAssemblies
{
	public v810RebuildJobAssemblies(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", new DmoField[53]
		{
			new DmoField("jmaJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmaJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("jmaLevel", "smallint", 3, 0, nullable: false),
			new DmoField("jmaParentAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("jmaSourceMethodID", "nvarchar", 30, 0, nullable: false),
			new DmoField("jmaSourceRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("jmaPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("jmaPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("jmaPartWareHouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("jmaPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("jmaUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("jmaPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("jmaPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("jmaPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("jmaQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("jmaEstimatedUnitCost", "numeric", 15, 5, nullable: false),
			new DmoField("jmaOrderQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("jmaInventoryQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("jmaScrapQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("jmaReworkQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("jmaQuantityToInspect", "numeric", 15, 5, nullable: false),
			new DmoField("jmaQuantityToReturn", "numeric", 15, 5, nullable: false),
			new DmoField("jmaScheduledStartHour", "numeric", 5, 2, nullable: false),
			new DmoField("jmaQuantityReceivedToInventory", "numeric", 15, 5, nullable: false),
			new DmoField("jmaReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("jmaScheduledDueHour", "numeric", 5, 2, nullable: false),
			new DmoField("jmaProductionQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("jmaQuantityToMake", "numeric", 15, 5, nullable: false),
			new DmoField("jmaQuantityToPull", "numeric", 15, 5, nullable: false),
			new DmoField("jmaQuantityIssued", "numeric", 15, 5, nullable: false),
			new DmoField("jmaPullAllFromStock", "bit", 1, 0, nullable: false),
			new DmoField("jmaIssuedComplete", "bit", 1, 0, nullable: false),
			new DmoField("jmaProductionNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("jmaProductionNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("jmaScheduledStartDate", "date", 14, 0, nullable: true),
			new DmoField("jmaScheduledDueDate", "date", 14, 0, nullable: true),
			new DmoField("jmaOverlapSourceOperationID", "int", 5, 0, nullable: false),
			new DmoField("jmaAssemblyOverlap", "tinyint", 1, 0, nullable: false),
			new DmoField("jmaOverlapSourceLink", "tinyint", 1, 0, nullable: false),
			new DmoField("jmaOverlapDestinationLink", "tinyint", 1, 0, nullable: false),
			new DmoField("jmaOverlapOffsetTime", "numeric", 8, 2, nullable: false),
			new DmoField("jmaOverlapOperationID", "int", 5, 0, nullable: false),
			new DmoField("jmaOverlapType", "tinyint", 1, 0, nullable: false),
			new DmoField("jmaDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("jmaProductionComplete", "bit", 1, 0, nullable: false),
			new DmoField("jmaQuantityCompleted", "numeric", 15, 5, nullable: false),
			new DmoField("jmaCompletedDate", "date", 14, 0, nullable: true),
			new DmoField("jmaReworkDate", "datetime", 14, 0, nullable: true),
			new DmoField("jmaClosed", "bit", 1, 0, nullable: false),
			new DmoField("jmaScrapQuantityCompleted", "numeric", 15, 5, nullable: false),
			new DmoField("jmaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("jmaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[18]
		{
			new DmoIndex("JMAJOBID,JMAJOBASSEMBLYID", unique: true),
			new DmoIndex("JMAUNIQUEID", unique: true),
			new DmoIndex("jmaJobID", unique: false),
			new DmoIndex("jmaJobAssemblyID", unique: false),
			new DmoIndex("jmaLevel", unique: false),
			new DmoIndex("jmaSourceMethodID", unique: false),
			new DmoIndex("jmaSourceRevisionID", unique: false),
			new DmoIndex("jmaPartID", unique: false),
			new DmoIndex("jmaPartRevisionID", unique: false),
			new DmoIndex("jmaPartWareHouseLocationID", unique: false),
			new DmoIndex("jmaPartBinID", unique: false),
			new DmoIndex("jmaScheduledStartDate", unique: false),
			new DmoIndex("jmaScheduledDueDate", unique: false),
			new DmoIndex("jmaOverlapSourceOperationID", unique: false),
			new DmoIndex("jmaOverlapOperationID", unique: false),
			new DmoIndex("jmaProductionComplete", unique: false),
			new DmoIndex("jmaClosed", unique: false),
			new DmoIndex("jmaReworkDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
