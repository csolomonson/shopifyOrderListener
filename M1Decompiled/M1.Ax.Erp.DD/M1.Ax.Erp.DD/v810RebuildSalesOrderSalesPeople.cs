using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderSalesPeople to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderSalesPeople
{
	public v810RebuildSalesOrderSalesPeople(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderSalesPeople", new DmoField[8]
		{
			new DmoField("omiSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omiSequenceID", "smallint", 4, 0, nullable: false),
			new DmoField("omiSalesEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omiPercent", "numeric", 6, 2, nullable: false),
			new DmoField("omiClosed", "bit", 1, 0, nullable: false),
			new DmoField("omiCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("omiCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omiUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("OMISALESORDERID,OMISEQUENCEID", unique: true),
			new DmoIndex("OMIUNIQUEID", unique: true),
			new DmoIndex("omiSalesOrderID", unique: false),
			new DmoIndex("omiSequenceID", unique: false),
			new DmoIndex("omiClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
