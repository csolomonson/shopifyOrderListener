using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PurchaseOrderAccounts to support unicode", "2013-10-17")]
public class v810RebuildPurchaseOrderAccounts
{
	public v810RebuildPurchaseOrderAccounts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderAccounts", new DmoField[10]
		{
			new DmoField("pmxPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmxPurchaseOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("pmxPurchaseOrderAccountID", "smallint", 4, 0, nullable: false),
			new DmoField("pmxExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("pmxPercent", "numeric", 9, 5, nullable: false),
			new DmoField("pmxAmount", "money", 12, 2, nullable: false),
			new DmoField("pmxClosed", "bit", 1, 0, nullable: false),
			new DmoField("pmxCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pmxCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pmxUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("PMXPURCHASEORDERID,PMXPURCHASEORDERLINEID,PMXPURCHASEORDERACCOUNTID", unique: true),
			new DmoIndex("PMXUNIQUEID", unique: true),
			new DmoIndex("pmxPurchaseOrderID", unique: false),
			new DmoIndex("pmxPurchaseOrderLineID", unique: false),
			new DmoIndex("pmxPurchaseOrderAccountID", unique: false),
			new DmoIndex("pmxClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
