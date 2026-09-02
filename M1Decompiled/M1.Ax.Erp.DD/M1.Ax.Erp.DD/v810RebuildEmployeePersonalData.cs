using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeePersonalData to support unicode", "2013-10-17")]
public class v810RebuildEmployeePersonalData
{
	public v810RebuildEmployeePersonalData(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePersonalData", new DmoField[48]
		{
			new DmoField("lmdEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmdEmployeeFirstName", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdEmployeeMiddleName", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdEmployeeLastName", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmdAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmdAddressLine3", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmdCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("lmdState", "nvarchar", 3, 0, nullable: false),
			new DmoField("lmdPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmdCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdMobileNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdFaxNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdPersonalEmailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmdTaxFileNumber", "nvarchar", 11, 0, nullable: false),
			new DmoField("lmdEmploymentDeclarationOnFile", "bit", 1, 0, nullable: false),
			new DmoField("lmdEmploymentDeclarationDate", "date", 14, 0, nullable: true),
			new DmoField("lmdLaborRate", "numeric", 8, 4, nullable: false),
			new DmoField("lmdBirthDate", "date", 14, 0, nullable: true),
			new DmoField("lmdGender", "nvarchar", 1, 0, nullable: false),
			new DmoField("lmdMaritalStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("lmdPayrollDefinitionID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmdPayrollExportEmployeeID", "nvarchar", 8, 0, nullable: false),
			new DmoField("lmdPAYGSummaryType", "nvarchar", 1, 0, nullable: false),
			new DmoField("lmdContact1Name", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmdContact1HomePhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdContact1WorkPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdContact1MobilePhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdContact1Relationship", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmdContact2Name", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmdContact2HomePhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdContact2WorkPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdContact2MobilePhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdContact2Relationship", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmdPayrollEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmdCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmdCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmdUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("lmdNZTaxCode", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmdEmploymentStatus", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmdResidencyStatus", "nvarchar", 25, 0, nullable: false),
			new DmoField("lmdWorkingHolidayMaker", "bit", 1, 0, nullable: false),
			new DmoField("lmdTaxFreeThresholdClaimed", "bit", 1, 0, nullable: false),
			new DmoField("lmdStdntFinSupplSchemeLoan", "bit", 1, 0, nullable: false),
			new DmoField("lmdStudyTrainLoanRepayment", "bit", 1, 0, nullable: false),
			new DmoField("lmdBasisOfPayment", "nvarchar", 1, 0, nullable: false),
			new DmoField("lmdHomeCountry", "nvarchar", 2, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("LMDEMPLOYEEID", unique: true),
			new DmoIndex("LMDUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
