using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APInvoiceMemos to support unicode", "2013-10-17")]
public class v810RebuildAPInvoiceMemos
{
	public v810RebuildAPInvoiceMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceMemos", new DmoField[11]
		{
			new DmoField("apiAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("apiAPInvoiceMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("apiMemoDate", "date", 14, 0, nullable: true),
			new DmoField("apiShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("apiLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("apiLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("apiShowInAPInvoices", "bit", 1, 0, nullable: false),
			new DmoField("apiShowInAPPayments", "bit", 1, 0, nullable: false),
			new DmoField("apiCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apiCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("apiUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("APIAPINVOICEID,APIAPINVOICEMEMOID", unique: true),
			new DmoIndex("APIUNIQUEID", unique: true),
			new DmoIndex("apiAPInvoiceID", unique: false),
			new DmoIndex("apiAPInvoiceMemoID", unique: false),
			new DmoIndex("apiMemoDate", unique: false),
			new DmoIndex("apiShowInAPInvoices", unique: false),
			new DmoIndex("apiShowInAPPayments", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
