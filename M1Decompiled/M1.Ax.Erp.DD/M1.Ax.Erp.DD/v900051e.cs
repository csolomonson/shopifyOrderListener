using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.051", "Add fields to LotNumberStatuses table", "2015-06-25")]
public class v900051e
{
	public v900051e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "LotNumberStatuses"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberStatuses", new DmoField[10]
			{
				new DmoField("absPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("absPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("absLotNumberID", "nvarchar", 30, 0, nullable: false),
				new DmoField("absPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("absPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("absStatus", "tinyint", 1, 0, nullable: false),
				new DmoField("absQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("absCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("absCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("absUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("absPartID,absPartRevisionID,absLotNumberID,absPartWarehouseLocationID,absPartBinID,absStatus", unique: true),
				new DmoIndex("absUniqueID", unique: true),
				new DmoIndex("absStatus", unique: false)
			});
		}
	}
}
