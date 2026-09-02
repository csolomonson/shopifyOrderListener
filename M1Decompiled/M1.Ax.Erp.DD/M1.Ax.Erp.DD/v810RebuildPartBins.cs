using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartBins to support unicode", "2013-10-17")]
public class v810RebuildPartBins
{
	public v810RebuildPartBins(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartBins", new DmoField[17]
		{
			new DmoField("imbPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imbPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imbWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imbPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imbDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imbQuantityOnHand", "numeric", 15, 5, nullable: false),
			new DmoField("imbConversionFactor", "numeric", 14, 8, nullable: false),
			new DmoField("imbBinQuantityOnHand", "numeric", 15, 5, nullable: false),
			new DmoField("imbQuantityAllocated", "numeric", 15, 5, nullable: false),
			new DmoField("imbQuantityToInspect", "numeric", 15, 5, nullable: false),
			new DmoField("imbQuantityToReturn", "numeric", 15, 5, nullable: false),
			new DmoField("imbDefaultBin", "bit", 1, 0, nullable: false),
			new DmoField("imbQuantityOnOrderSales", "numeric", 15, 5, nullable: false),
			new DmoField("imbQuantityOnOrderPurchases", "numeric", 15, 5, nullable: false),
			new DmoField("imbCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imbCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imbUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("IMBPARTID,IMBPARTREVISIONID,IMBWAREHOUSEID,IMBPARTBINID", unique: true),
			new DmoIndex("IMBUNIQUEID", unique: true),
			new DmoIndex("imbPartID", unique: false),
			new DmoIndex("imbPartRevisionID", unique: false),
			new DmoIndex("imbWarehouseID", unique: false),
			new DmoIndex("imbPartBinID", unique: false),
			new DmoIndex("imbDefaultBin", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
