using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderPickListLines to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderPickListLines
{
	public v810RebuildSalesOrderPickListLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderPickListLines", new DmoField[16]
		{
			new DmoField("omyPickListSessionID", "int", 9, 0, nullable: false),
			new DmoField("omyPickListLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omySalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omySalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omySalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("omyPickDate", "datetime", 14, 0, nullable: true),
			new DmoField("omyOpenQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("omyPickQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("omyDeliveryDate", "datetime", 14, 0, nullable: true),
			new DmoField("omyPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("omyPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omyPartWareHouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omyPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("omyCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("OMYPICKLISTSESSIONID,OMYPICKLISTLINEID", unique: true),
			new DmoIndex("OMYUNIQUEID", unique: true),
			new DmoIndex("omyPickListSessionID", unique: false),
			new DmoIndex("omyPickListLineID", unique: false),
			new DmoIndex("omySalesOrderID", unique: false),
			new DmoIndex("omySalesOrderLineID", unique: false),
			new DmoIndex("omySalesOrderDeliveryID", unique: false),
			new DmoIndex("omyPartID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
