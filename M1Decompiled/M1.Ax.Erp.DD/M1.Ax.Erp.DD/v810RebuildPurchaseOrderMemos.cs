using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PurchaseOrderMemos to support unicode", "2013-10-17")]
public class v810RebuildPurchaseOrderMemos
{
	public v810RebuildPurchaseOrderMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderMemos", new DmoField[13]
		{
			new DmoField("pmkPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmkPurchaseOrderMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("pmkMemoDate", "date", 14, 0, nullable: true),
			new DmoField("pmkShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pmkLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pmkLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pmkClosed", "bit", 1, 0, nullable: false),
			new DmoField("pmkShowInPurchaseOrders", "bit", 1, 0, nullable: false),
			new DmoField("pmkShowInReceipts", "bit", 1, 0, nullable: false),
			new DmoField("pmkShowInAPInvoices", "bit", 1, 0, nullable: false),
			new DmoField("pmkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pmkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pmkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("PMKPURCHASEORDERID,PMKPURCHASEORDERMEMOID", unique: true),
			new DmoIndex("PMKUNIQUEID", unique: true),
			new DmoIndex("pmkPurchaseOrderID", unique: false),
			new DmoIndex("pmkPurchaseOrderMemoID", unique: false),
			new DmoIndex("pmkMemoDate", unique: false),
			new DmoIndex("pmkShowInPurchaseOrders", unique: false),
			new DmoIndex("pmkShowInReceipts", unique: false),
			new DmoIndex("pmkShowInAPInvoices", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
