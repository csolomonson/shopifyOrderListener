using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.826", "Convert STP tables to support unicode", "2020-01-12")]
public class v92826a
{
	public v92826a(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPTerminationPayment", new DmoField[14]
		{
			new DmoField("sttSessionID", "int", 9, 0, nullable: false),
			new DmoField("sttLineID", "smallint", 4, 0, nullable: false),
			new DmoField("sttTerminationID", "smallint", 4, 0, nullable: false),
			new DmoField("sttTerminationCode", "nvarchar", 1, 0, nullable: false),
			new DmoField("sttPayeeETPPaymentDate", "date", 14, 0, nullable: true),
			new DmoField("sttTerminationPmtTaxFreeComp", "money", 12, 2, nullable: false),
			new DmoField("sttTerminationPmtTaxableComp", "money", 12, 2, nullable: false),
			new DmoField("sttPayeeTotalETPPAYGWAmount", "money", 12, 2, nullable: false),
			new DmoField("sttSTPSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("sttFullFileReplacement", "bit", 1, 0, nullable: false),
			new DmoField("sttSTPFFRSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("sttCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("sttCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("sttUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("sttSessionID,sttLineID,sttTerminationID", unique: true),
			new DmoIndex("sttUniqueID", unique: true),
			new DmoIndex("sttFullFileReplacement", unique: false)
		}, mergeCustomFields: true);
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPSessions", new DmoField[35]
		{
			new DmoField("stpSessionID", "int", 9, 0, nullable: false),
			new DmoField("stpTaxYear", "smallint", 4, 0, nullable: false),
			new DmoField("stpPayerOrganisationName", "nvarchar", 200, 0, nullable: false),
			new DmoField("stpContactName", "nvarchar", 200, 0, nullable: false),
			new DmoField("stpEmailAddress", "nvarchar", 200, 0, nullable: false),
			new DmoField("stpPhoneNumber", "nvarchar", 16, 0, nullable: false),
			new DmoField("stpAddressLine1", "nvarchar", 38, 0, nullable: false),
			new DmoField("stpAddressLine2", "nvarchar", 38, 0, nullable: false),
			new DmoField("stpSuburb", "nvarchar", 46, 0, nullable: false),
			new DmoField("stpState", "nvarchar", 3, 0, nullable: false),
			new DmoField("stpPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("stpCountryCode", "nvarchar", 2, 0, nullable: false),
			new DmoField("stpPayerTotalPAYGW", "money", 12, 2, nullable: false),
			new DmoField("stpPayerTotalGrossPay", "money", 12, 2, nullable: false),
			new DmoField("stpPayUpdateDate", "datetime", 14, 0, nullable: true),
			new DmoField("stpRunDateTimeStamp", "datetime", 14, 0, nullable: true),
			new DmoField("stpFullFileReplacement", "bit", 1, 0, nullable: false),
			new DmoField("stpPayerDeclarerIdentifier", "nvarchar", 200, 0, nullable: false),
			new DmoField("stpDeclarationDate", "datetime", 14, 0, nullable: true),
			new DmoField("stpPayerDeclaration", "bit", 1, 0, nullable: false),
			new DmoField("stpBMSIdentifier", "nvarchar", 200, 0, nullable: false),
			new DmoField("stpABN", "nvarchar", 20, 0, nullable: false),
			new DmoField("stpPayerBranchCode", "nvarchar", 3, 0, nullable: false),
			new DmoField("stpSTPCalculated", "bit", 1, 0, nullable: false),
			new DmoField("stpSTPSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("stpSTPSubmittedDate", "datetime", 14, 0, nullable: true),
			new DmoField("stpSTPSubmissionID", "nvarchar", 50, 0, nullable: false),
			new DmoField("stpSTPFFRSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("stpSTPFFRSubmittedDate", "datetime", 14, 0, nullable: true),
			new DmoField("stpSTPResponseText", "nvarchar(max)", 0, 0, nullable: true),
			new DmoField("stpSTPResponseRtf", "nvarchar(max)", 0, 0, nullable: true),
			new DmoField("stpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("stpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("stpUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("stpRowVersion", "timestamp", 0, 0, nullable: true)
		}, new DmoIndex[7]
		{
			new DmoIndex("stpSessionID", unique: true),
			new DmoIndex("stpUniqueID", unique: true),
			new DmoIndex("stpTaxYear", unique: false),
			new DmoIndex("stpPayerOrganisationName", unique: false),
			new DmoIndex("stpContactName", unique: false),
			new DmoIndex("stpFullFileReplacement", unique: false),
			new DmoIndex("stpPayerDeclaration", unique: false)
		}, mergeCustomFields: true);
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", new DmoField[83]
		{
			new DmoField("stlSessionID", "int", 9, 0, nullable: false),
			new DmoField("stlLineID", "smallint", 4, 0, nullable: false),
			new DmoField("stlEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("stlTaxFileNumber", "nvarchar", 11, 0, nullable: false),
			new DmoField("stlContractorABN", "nvarchar", 20, 0, nullable: false),
			new DmoField("stlPayeeFamilyName", "nvarchar", 40, 0, nullable: false),
			new DmoField("stlPayeeFirstName", "nvarchar", 40, 0, nullable: false),
			new DmoField("stlPayeeOtherName", "nvarchar", 40, 0, nullable: false),
			new DmoField("stlPayeeBirthDate", "date", 14, 0, nullable: true),
			new DmoField("stlAddressLine1", "nvarchar", 38, 0, nullable: false),
			new DmoField("stlAddressLine2", "nvarchar", 38, 0, nullable: false),
			new DmoField("stlSuburb", "nvarchar", 46, 0, nullable: false),
			new DmoField("stlState", "nvarchar", 3, 0, nullable: false),
			new DmoField("stlPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("stlCountryCode", "nvarchar", 2, 0, nullable: false),
			new DmoField("stlEmailAddress", "nvarchar", 200, 0, nullable: false),
			new DmoField("stlPhoneNumber", "nvarchar", 16, 0, nullable: false),
			new DmoField("stlCommencementDate", "date", 14, 0, nullable: true),
			new DmoField("stlCessationDate", "date", 14, 0, nullable: true),
			new DmoField("stlPeriodStartDate", "date", 14, 0, nullable: true),
			new DmoField("stlPeriodEndDate", "date", 14, 0, nullable: true),
			new DmoField("stlFinalEventIndicator", "bit", 1, 0, nullable: false),
			new DmoField("stlGrossPayments", "money", 12, 2, nullable: false),
			new DmoField("stlFullFileReplacement", "bit", 1, 0, nullable: false),
			new DmoField("stlSTPSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("stlSTPFFRSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("stlTotalINBPAYGWAmount", "money", 12, 2, nullable: false),
			new DmoField("stlCDEPPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("stlWorkingHolidayGrossPay", "money", 12, 2, nullable: false),
			new DmoField("stlWorkingHolidayPAYGWAmount", "money", 12, 2, nullable: false),
			new DmoField("stlOtherSpecifiedGrossPayments", "money", 12, 2, nullable: false),
			new DmoField("stlTotalOtherSpecifiedPAYGWAmt", "money", 12, 2, nullable: false),
			new DmoField("stlLabourHireGrossPayment", "money", 12, 2, nullable: false),
			new DmoField("stlTotalLabourHirePAYGWAmt", "money", 12, 2, nullable: false),
			new DmoField("stlVoluntaryAgreementGrossPay", "money", 12, 2, nullable: false),
			new DmoField("stlTotalVolAgreementPAYGWAmt", "money", 12, 2, nullable: false),
			new DmoField("stlGrossPayForeignEmployment", "money", 12, 2, nullable: false),
			new DmoField("stlForeignEmploymentTaxPaid", "money", 12, 2, nullable: false),
			new DmoField("stlJPDAForeignIncomeAmt", "money", 12, 2, nullable: false),
			new DmoField("stlJPDAForeignIncomeTaxPaid", "money", 12, 2, nullable: false),
			new DmoField("stlExemptForeignIncomeAmt", "money", 12, 2, nullable: false),
			new DmoField("stlTotalFEIJPDAPAYGWAmount", "money", 12, 2, nullable: false),
			new DmoField("stlTotalFEIPAYGWAmount", "money", 12, 2, nullable: false),
			new DmoField("stlSuperLiabilityAmount", "money", 12, 2, nullable: false),
			new DmoField("stlOTEAmount", "money", 12, 2, nullable: false),
			new DmoField("stlReportableEmpSuperContrib", "money", 12, 2, nullable: false),
			new DmoField("stlPayeeRFBTaxableAmount", "money", 12, 2, nullable: false),
			new DmoField("stlPayeeRFBExemptAmount", "money", 12, 2, nullable: false),
			new DmoField("stlPayeeLumpSumPaymentAType", "nvarchar", 1, 0, nullable: false),
			new DmoField("stlPayeeLumpSumPaymentA", "money", 12, 2, nullable: false),
			new DmoField("stlPayeeLumpSumPaymentB", "money", 12, 2, nullable: false),
			new DmoField("stlPayeeLumpSumPaymentD", "money", 12, 2, nullable: false),
			new DmoField("stlPayeeLumpSumPaymentE", "money", 12, 2, nullable: false),
			new DmoField("stlPayeeResidencyStatus", "nvarchar", 25, 0, nullable: false),
			new DmoField("stlPayeeTerminatedIndicator", "bit", 1, 0, nullable: false),
			new DmoField("stlTaxFreeThresholdClaimed", "bit", 1, 0, nullable: false),
			new DmoField("stlStudyAndTrnLoanRepmtInd", "bit", 1, 0, nullable: false),
			new DmoField("stlStdntFinSupplSchemeLoanInd", "bit", 1, 0, nullable: false),
			new DmoField("stlBasisOfPaymentCode", "nvarchar", 1, 0, nullable: false),
			new DmoField("stlPayeeDeclAccpIndicator", "bit", 1, 0, nullable: false),
			new DmoField("stlPayeeDateDeclarationSigned", "date", 14, 0, nullable: true),
			new DmoField("stlPayeeLumpSumPaymentW", "money", 12, 2, nullable: false),
			new DmoField("stlCessationType", "nvarchar", 1, 0, nullable: false),
			new DmoField("stlHomeCountry", "nvarchar", 2, 0, nullable: false),
			new DmoField("stlCashOutLeave", "money", 12, 2, nullable: false),
			new DmoField("stlUnusedLeave", "money", 12, 2, nullable: false),
			new DmoField("stlPaidParentalLeave", "money", 12, 2, nullable: false),
			new DmoField("stlWorkersComp", "money", 12, 2, nullable: false),
			new DmoField("stlAncillaryDefenceLeave", "money", 12, 2, nullable: false),
			new DmoField("stlOtherPaidLeave", "money", 12, 2, nullable: false),
			new DmoField("stlSalarySacrificeSuper", "money", 12, 2, nullable: false),
			new DmoField("stlSalarySacrificeOther", "money", 12, 2, nullable: false),
			new DmoField("stlOrdinaryTimeEarningsAmount", "money", 12, 2, nullable: false),
			new DmoField("stlBonusAmount", "money", 12, 2, nullable: false),
			new DmoField("stlOvertimeAmount", "money", 12, 2, nullable: false),
			new DmoField("stlPreviousEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("stlTaxTreatmentCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("stlEmployeeBasisCode", "nvarchar", 1, 0, nullable: false),
			new DmoField("stlWorkingHolidayMaker", "bit", 1, 0, nullable: false),
			new DmoField("stlDirectorsFees", "money", 12, 2, nullable: false),
			new DmoField("stlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("stlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("stlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("stlSessionID,stlLineID", unique: true),
			new DmoIndex("stlUniqueID", unique: true),
			new DmoIndex("stlEmployeeID", unique: false),
			new DmoIndex("stlFullFileReplacement", unique: false)
		}, mergeCustomFields: true);
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPDeductions", new DmoField[11]
		{
			new DmoField("stdSessionID", "int", 9, 0, nullable: false),
			new DmoField("stdLineID", "smallint", 4, 0, nullable: false),
			new DmoField("stdDeductionID", "smallint", 4, 0, nullable: false),
			new DmoField("stdDeductionType", "nvarchar", 1, 0, nullable: false),
			new DmoField("stdPayeeDeductionAmount", "money", 10, 2, nullable: false),
			new DmoField("stdSTPSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("stdFullFileReplacement", "bit", 1, 0, nullable: false),
			new DmoField("stdSTPFFRSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("stdCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("stdCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("stdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("stdSessionID,stdLineID,stdDeductionID", unique: true),
			new DmoIndex("stdUniqueID", unique: true),
			new DmoIndex("stdFullFileReplacement", unique: false)
		}, mergeCustomFields: true);
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPAllowances", new DmoField[12]
		{
			new DmoField("staSessionID", "int", 9, 0, nullable: false),
			new DmoField("staLineID", "smallint", 4, 0, nullable: false),
			new DmoField("staAllowanceID", "smallint", 4, 0, nullable: false),
			new DmoField("staAllowanceType", "nvarchar", 2, 0, nullable: false),
			new DmoField("staOtherAllowanceType", "nvarchar", 40, 0, nullable: false),
			new DmoField("staPayeeAllowanceAmount", "money", 10, 2, nullable: false),
			new DmoField("staSTPSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("staFullFileReplacement", "bit", 1, 0, nullable: false),
			new DmoField("staSTPFFRSubmitted", "bit", 1, 0, nullable: false),
			new DmoField("staCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("staCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("staUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("staSessionID,staLineID,staAllowanceID", unique: true),
			new DmoIndex("staUniqueID", unique: true),
			new DmoIndex("staFullFileReplacement", unique: false)
		}, mergeCustomFields: true);
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update STPLines Set stlSTPSubmitted = stpSTPSubmitted From STPSessions Inner Join STPLines On stpSessionID = stlSessionID");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update STPAllowances Set staSTPSubmitted = stlSTPSubmitted From STPLines Inner Join STPAllowances On stlSessionID = staSessionID And stlLineID = staLineID");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update STPDeductions Set stdSTPSubmitted = stlSTPSubmitted From STPLines Inner Join STPDeductions On stlSessionID = stdSessionID And stlLineID = stdLineID");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update STPTerminationPayment Set sttSTPSubmitted = stlSTPSubmitted From STPLines Inner Join STPTerminationPayment On stlSessionID = sttSessionID And stlLineID = sttLineID");
	}
}
