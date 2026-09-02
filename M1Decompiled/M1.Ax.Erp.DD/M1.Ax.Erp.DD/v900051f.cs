using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.051", "Add fields to SerialNumberStatuses table", "2015-06-25")]
public class v900051f
{
	public v900051f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "SerialNumberStatuses"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberStatuses", new DmoField[10]
			{
				new DmoField("snsPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("snsPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("snsSerialNumberID", "nvarchar", 30, 0, nullable: false),
				new DmoField("snsPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("snsPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("snsStatus", "tinyint", 2, 0, nullable: false),
				new DmoField("snsQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("snsCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("snsCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("snsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("snsPartID,snsPartRevisionID,snsSerialNumberID,snsPartWarehouseLocationID,snsPartBinID,snsStatus", unique: true),
				new DmoIndex("snsUniqueID", unique: true),
				new DmoIndex("snsStatus", unique: false)
			});
		}
	}
}
