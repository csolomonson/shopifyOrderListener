using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APInvoiceExpenseAccounts to support unicode", "2013-10-17")]
public class v810RebuildAPInvoiceExpenseAccounts
{
	public v810RebuildAPInvoiceExpenseAccounts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceExpenseAccounts", new DmoField[10]
		{
			new DmoField("apxAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("apxAPInvoiceLineID", "smallint", 4, 0, nullable: false),
			new DmoField("apxAPInvoiceExpenseAccountID", "smallint", 4, 0, nullable: false),
			new DmoField("apxExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apxPercent", "numeric", 9, 5, nullable: false),
			new DmoField("apxAmount", "money", 12, 2, nullable: false),
			new DmoField("apxPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("apxCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apxCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("apxUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("APXAPINVOICEID,APXAPINVOICELINEID,APXAPINVOICEEXPENSEACCOUNTID", unique: true),
			new DmoIndex("APXUNIQUEID", unique: true),
			new DmoIndex("apxAPInvoiceID", unique: false),
			new DmoIndex("apxAPInvoiceLineID", unique: false),
			new DmoIndex("apxAPInvoiceExpenseAccountID", unique: false),
			new DmoIndex("apxPostedToGL", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
