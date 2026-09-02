using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderMemos to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderMemos
{
	public v810RebuildSalesOrderMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderMemos", new DmoField[13]
		{
			new DmoField("omkSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omkSalesOrderMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("omkMemoDate", "date", 14, 0, nullable: true),
			new DmoField("omkShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("omkLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("omkLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("omkClosed", "bit", 1, 0, nullable: false),
			new DmoField("omkShowInSalesOrders", "bit", 1, 0, nullable: false),
			new DmoField("omkShowInShipments", "bit", 1, 0, nullable: false),
			new DmoField("omkShowInARInvoices", "bit", 1, 0, nullable: false),
			new DmoField("omkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("omkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("OMKSALESORDERID,OMKSALESORDERMEMOID", unique: true),
			new DmoIndex("OMKUNIQUEID", unique: true),
			new DmoIndex("omkSalesOrderID", unique: false),
			new DmoIndex("omkSalesOrderMemoID", unique: false),
			new DmoIndex("omkMemoDate", unique: false),
			new DmoIndex("omkClosed", unique: false),
			new DmoIndex("omkShowInSalesOrders", unique: false),
			new DmoIndex("omkShowInShipments", unique: false),
			new DmoIndex("omkShowInARInvoices", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
