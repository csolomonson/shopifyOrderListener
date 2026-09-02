using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APTaxablePaymentTotalDetails to support unicode", "2013-10-17")]
public class v810RebuildAPTaxablePaymentTotalDetails
{
	public v810RebuildAPTaxablePaymentTotalDetails(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APTaxablePaymentTotalDetails", new DmoField[13]
		{
			new DmoField("tpdAPTaxablePaymentID", "smallint", 4, 0, nullable: false),
			new DmoField("tpdAPTaxablePaymentTotalID", "smallint", 4, 0, nullable: false),
			new DmoField("tpdAPTaxablePaymentDetailID", "smallint", 4, 0, nullable: false),
			new DmoField("tpdAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("tpdAPPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("tpdAPPaymentHeaderID", "int", 7, 0, nullable: false),
			new DmoField("tpdReportableAmount", "money", 11, 2, nullable: false),
			new DmoField("tpdTaxWithheld", "money", 11, 2, nullable: false),
			new DmoField("tpdGSTAmount", "money", 11, 2, nullable: false),
			new DmoField("tpdClosed", "bit", 1, 0, nullable: false),
			new DmoField("tpdCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("tpdCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("tpdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("TPDAPTAXABLEPAYMENTID,TPDAPTAXABLEPAYMENTTOTALID,TPDAPTAXABLEPAYMENTDETAILID", unique: true),
			new DmoIndex("TPDUNIQUEID", unique: true),
			new DmoIndex("tpdAPTaxablePaymentID", unique: false),
			new DmoIndex("tpdAPTaxablePaymentTotalID", unique: false),
			new DmoIndex("tpdAPTaxablePaymentDetailID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
