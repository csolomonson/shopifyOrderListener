using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IncomeTaxYears to support unicode", "2013-10-17")]
public class v810RebuildIncomeTaxYears
{
	public v810RebuildIncomeTaxYears(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYears", new DmoField[47]
		{
			new DmoField("papIncomeTaxYearID", "smallint", 4, 0, nullable: false),
			new DmoField("papPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("papTotalsCalculatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("papCertificatesPrintedDate", "date", 14, 0, nullable: true),
			new DmoField("papEmployerIDNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("papEmployerName", "nvarchar", 50, 0, nullable: false),
			new DmoField("papEmployerAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("papEmployerAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("papEmployerCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("papEmployerState", "nvarchar", 3, 0, nullable: false),
			new DmoField("papEmployerPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("papEmployerCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("papContactPerson", "nvarchar", 50, 0, nullable: false),
			new DmoField("papContactEmailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("papContactPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("papContactFaxNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("papAuthorizedDate", "date", 14, 0, nullable: true),
			new DmoField("papAuthorizedPerson", "nvarchar", 50, 0, nullable: false),
			new DmoField("papUSEmployerStateIDNumber", "nvarchar", 15, 0, nullable: false),
			new DmoField("papUSOtherEmployerIDNumber", "nvarchar", 15, 0, nullable: false),
			new DmoField("papUSCombinedFederalStateFiler", "bit", 1, 0, nullable: false),
			new DmoField("papUSLastFilingYear", "bit", 1, 0, nullable: false),
			new DmoField("papUSPersonalIDNumber", "nvarchar", 17, 0, nullable: false),
			new DmoField("papAUSBranchNumber", "smallint", 3, 0, nullable: false),
			new DmoField("papClosed", "bit", 1, 0, nullable: false),
			new DmoField("papClosedDate", "date", 14, 0, nullable: true),
			new DmoField("papTotalRecords", "smallint", 4, 0, nullable: false),
			new DmoField("papCABusinessNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("papCAEmploymentIncome", "money", 14, 2, nullable: false),
			new DmoField("papCARPPContributions", "money", 12, 2, nullable: false),
			new DmoField("papCAPensionAdjustment", "money", 14, 2, nullable: false),
			new DmoField("papCAEmployeeCPPContributions", "money", 12, 2, nullable: false),
			new DmoField("papCAEmployerCPPContributions", "money", 12, 2, nullable: false),
			new DmoField("papCAEmployeeEIPremiums", "money", 12, 2, nullable: false),
			new DmoField("papCAEmployerEIPremiums", "money", 12, 2, nullable: false),
			new DmoField("papCAIncomeTaxDeducted", "money", 14, 2, nullable: false),
			new DmoField("papCATotalDeductionReported", "money", 14, 2, nullable: false),
			new DmoField("papCARemittances", "money", 14, 2, nullable: false),
			new DmoField("papCADifference", "money", 14, 2, nullable: false),
			new DmoField("papCAOverpayment", "money", 14, 2, nullable: false),
			new DmoField("papCABalanceDue", "money", 14, 2, nullable: false),
			new DmoField("papCAAmountEnclosed", "money", 14, 2, nullable: false),
			new DmoField("papCASocialInsuranceNumber1", "int", 9, 0, nullable: false),
			new DmoField("papCASocialInsuranceNumber2", "int", 9, 0, nullable: false),
			new DmoField("papCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("papCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("papUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("PAPINCOMETAXYEARID,PAPPLANTID", unique: true),
			new DmoIndex("PAPUNIQUEID", unique: true),
			new DmoIndex("papIncomeTaxYearID", unique: false),
			new DmoIndex("papPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
