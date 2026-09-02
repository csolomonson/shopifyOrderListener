using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARInvoiceMemos to support unicode", "2013-10-17")]
public class v810RebuildARInvoiceMemos
{
	public v810RebuildARInvoiceMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceMemos", new DmoField[11]
		{
			new DmoField("ariARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ariARInvoiceMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("ariMemoDate", "date", 14, 0, nullable: true),
			new DmoField("ariShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("ariLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("ariLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("ariShowInARInvoices", "bit", 1, 0, nullable: false),
			new DmoField("ariShowInARPayments", "bit", 1, 0, nullable: false),
			new DmoField("ariCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ariCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ariUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("ARIARINVOICEID,ARIARINVOICEMEMOID", unique: true),
			new DmoIndex("ARIUNIQUEID", unique: true),
			new DmoIndex("ariARInvoiceID", unique: false),
			new DmoIndex("ariARInvoiceMemoID", unique: false),
			new DmoIndex("ariMemoDate", unique: false),
			new DmoIndex("ariShowInARInvoices", unique: false),
			new DmoIndex("ariShowInARPayments", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
