using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APTaxablePaymentTotals to support unicode", "2013-10-17")]
public class v810RebuildAPTaxablePaymentTotals
{
	public v810RebuildAPTaxablePaymentTotals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APTaxablePaymentTotals", new DmoField[26]
		{
			new DmoField("tptAPTaxablePaymentID", "smallint", 4, 0, nullable: false),
			new DmoField("tptAPTaxablePaymentTotalID", "smallint", 4, 0, nullable: false),
			new DmoField("tptOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("tptPayeeBusinessNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("tptPayeeBusinessName", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("tptPayeeLastName", "nvarchar", 30, 0, nullable: false),
			new DmoField("tptPayeeFirstGivenName", "nvarchar", 15, 0, nullable: false),
			new DmoField("tptPayeeSecondGivenName", "nvarchar", 15, 0, nullable: false),
			new DmoField("tptPayeeTradingName", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("tptPayeeAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("tptPayeeAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("tptPayeeCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("tptPayeeState", "nvarchar", 3, 0, nullable: false),
			new DmoField("tptPayeePostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("tptPayeeCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("tptPayeePhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("tptPayeeBankBSBNumber", "nvarchar", 10, 0, nullable: false),
			new DmoField("tptPayeeBankAccountNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("tptGrossAmountPaid", "money", 11, 0, nullable: false),
			new DmoField("tptTotalTaxWithheld", "money", 11, 0, nullable: false),
			new DmoField("tptTotalGST", "money", 11, 0, nullable: false),
			new DmoField("tptAmendmentIndicator", "nvarchar", 1, 0, nullable: false),
			new DmoField("tptClosed", "bit", 1, 0, nullable: false),
			new DmoField("tptCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("tptCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("tptUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("TPTAPTAXABLEPAYMENTID,TPTAPTAXABLEPAYMENTTOTALID", unique: true),
			new DmoIndex("TPTUNIQUEID", unique: true),
			new DmoIndex("tptAPTaxablePaymentID", unique: false),
			new DmoIndex("tptAPTaxablePaymentTotalID", unique: false),
			new DmoIndex("tptOrganizationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
