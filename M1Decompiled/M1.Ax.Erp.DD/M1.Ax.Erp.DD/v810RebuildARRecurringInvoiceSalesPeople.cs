using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARRecurringInvoiceSalesPeople to support unicode", "2013-10-17")]
public class v810RebuildARRecurringInvoiceSalesPeople
{
	public v810RebuildARRecurringInvoiceSalesPeople(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoiceSalesPeople", new DmoField[9]
		{
			new DmoField("aroARRecurringInvoiceID", "int", 6, 0, nullable: false),
			new DmoField("aroSequenceID", "smallint", 4, 0, nullable: false),
			new DmoField("aroSalesEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("aroPercent", "numeric", 6, 2, nullable: false),
			new DmoField("aroRate", "numeric", 6, 2, nullable: false),
			new DmoField("aroAmount", "money", 12, 2, nullable: false),
			new DmoField("aroCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("aroCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("aroUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("AROARRECURRINGINVOICEID,AROSEQUENCEID", unique: true),
			new DmoIndex("AROUNIQUEID", unique: true),
			new DmoIndex("aroARRecurringInvoiceID", unique: false),
			new DmoIndex("aroSequenceID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
