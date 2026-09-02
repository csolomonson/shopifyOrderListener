using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARInvoiceSalesPeople to support unicode", "2013-10-17")]
public class v810RebuildARInvoiceSalesPeople
{
	public v810RebuildARInvoiceSalesPeople(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceSalesPeople", new DmoField[10]
		{
			new DmoField("arjARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arjSequenceID", "smallint", 4, 0, nullable: false),
			new DmoField("arjSalesEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arjPercent", "numeric", 6, 2, nullable: false),
			new DmoField("arjRate", "numeric", 6, 2, nullable: false),
			new DmoField("arjAmount", "money", 12, 2, nullable: false),
			new DmoField("arjPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("arjCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("arjCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("arjUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("ARJARINVOICEID,ARJSEQUENCEID", unique: true),
			new DmoIndex("ARJUNIQUEID", unique: true),
			new DmoIndex("arjARInvoiceID", unique: false),
			new DmoIndex("arjSequenceID", unique: false),
			new DmoIndex("arjPostedToGL", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
