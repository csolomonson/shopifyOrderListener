using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APTaxablePayments to support unicode", "2013-10-17")]
public class v810RebuildAPTaxablePayments
{
	public v810RebuildAPTaxablePayments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APTaxablePayments", new DmoField[28]
		{
			new DmoField("tprAPTaxablePaymentID", "smallint", 4, 0, nullable: false),
			new DmoField("tprTaxYear", "smallint", 4, 0, nullable: false),
			new DmoField("tprPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("tprTotalsCalculatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("tprReportPrintedDate", "date", 14, 0, nullable: true),
			new DmoField("tprCompanyBusinessNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("tprCompanyName", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("tprCompanyAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("tprCompanyAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("tprCompanyCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("tprCompanyState", "nvarchar", 3, 0, nullable: false),
			new DmoField("tprCompanyPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("tprCompanyCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("tprContactPerson", "nvarchar", 50, 0, nullable: false),
			new DmoField("tprContactPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("tprContactFaxNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("tprCompanyEmailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("tprCompanyFileReference", "nvarchar", 16, 0, nullable: false),
			new DmoField("tprBranchNumber", "smallint", 3, 0, nullable: false),
			new DmoField("tprCompanyTradingName", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("tprClosed", "bit", 1, 0, nullable: false),
			new DmoField("tprClosedDate", "date", 14, 0, nullable: true),
			new DmoField("tprTotalRecords", "smallint", 4, 0, nullable: false),
			new DmoField("tprStartDate", "date", 14, 0, nullable: true),
			new DmoField("tprEndDate", "date", 14, 0, nullable: true),
			new DmoField("tprCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("tprCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("tprUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("TPRAPTAXABLEPAYMENTID", unique: true),
			new DmoIndex("TPRUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
