using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.028", "Create PartBinDetails table", "2015-04-08")]
public class v900028aa
{
	public v900028aa(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartBinDetails"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartBinDetails", new DmoField[21]
			{
				new DmoField("imgPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("imgPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("imgWarehouseID", "nvarchar", 5, 0, nullable: false),
				new DmoField("imgPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("imgPartBinDetailID", "int", 4, 0, nullable: false),
				new DmoField("imgTransactionDate", "datetime", 14, 0, nullable: true),
				new DmoField("imgQuantityType", "tinyint", 1, 0, nullable: false),
				new DmoField("imgOriginalQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("imgRemainingQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("imgUnitLaborCost", "numeric", 15, 5, nullable: false),
				new DmoField("imgUnitOverheadCost", "numeric", 15, 5, nullable: false),
				new DmoField("imgUnitMaterialCost", "numeric", 15, 5, nullable: false),
				new DmoField("imgUnitSubcontractCost", "numeric", 15, 5, nullable: false),
				new DmoField("imgUnitDutyCost", "numeric", 15, 5, nullable: false),
				new DmoField("imgUnitFreightCost", "numeric", 15, 5, nullable: false),
				new DmoField("imgUnitMiscCost", "numeric", 15, 5, nullable: false),
				new DmoField("imgSourceTableName", "nvarchar", 30, 0, nullable: false),
				new DmoField("imgSourceTableUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("imgCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("imgCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("imgUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[6]
			{
				new DmoIndex("imgPartID,imgPartRevisionID,imgWarehouseID,imgPartBinID,imgPartBinDetailID", unique: true),
				new DmoIndex("imgUniqueID", unique: true),
				new DmoIndex("imgTransactionDate", unique: false),
				new DmoIndex("imgQuantityType", unique: false),
				new DmoIndex("imgSourceTableName", unique: false),
				new DmoIndex("imgSourceTableUniqueID", unique: false)
			});
		}
	}
}
