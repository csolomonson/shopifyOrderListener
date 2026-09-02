using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Deductions to support unicode", "2013-10-17")]
public class v810RebuildDeductions
{
	public v810RebuildDeductions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", new DmoField[40]
		{
			new DmoField("padDeductionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("padDeductionDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("padPaidBy", "tinyint", 1, 0, nullable: false),
			new DmoField("padMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("padDeductionTaxMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("padDefaultAmount", "money", 10, 2, nullable: false),
			new DmoField("padDefaultPercent", "numeric", 8, 4, nullable: false),
			new DmoField("padDeductionCategory", "nvarchar", 1, 0, nullable: false),
			new DmoField("padTaxBoxInfo", "nvarchar", 2, 0, nullable: false),
			new DmoField("padExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("padAccrualGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("padOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("padPrintOnPaySlip", "bit", 1, 0, nullable: false),
			new DmoField("padLimitPerCheck", "money", 10, 2, nullable: false),
			new DmoField("padLimitPerPayPeriod", "money", 10, 2, nullable: false),
			new DmoField("padLimitPerYear", "money", 10, 2, nullable: false),
			new DmoField("padDeductionPeriod1", "bit", 1, 0, nullable: false),
			new DmoField("padDeductionPeriod2", "bit", 1, 0, nullable: false),
			new DmoField("padDeductionPeriod3", "bit", 1, 0, nullable: false),
			new DmoField("padDeductionPeriod4", "bit", 1, 0, nullable: false),
			new DmoField("padDeductionPeriod5", "bit", 1, 0, nullable: false),
			new DmoField("padDeductionPeriod6", "bit", 1, 0, nullable: false),
			new DmoField("padDefaultRate", "numeric", 8, 4, nullable: false),
			new DmoField("padSalarySacrifice", "bit", 1, 0, nullable: false),
			new DmoField("padUSBox14A", "bit", 1, 0, nullable: false),
			new DmoField("padUSBox14B", "bit", 1, 0, nullable: false),
			new DmoField("padUSBox14C", "bit", 1, 0, nullable: false),
			new DmoField("padUSBox14Description", "nvarchar", 5, 0, nullable: false),
			new DmoField("padSuperannuation", "bit", 1, 0, nullable: false),
			new DmoField("padCanadaTaxBoxInfo", "tinyint", 2, 0, nullable: false),
			new DmoField("padUnionFees", "bit", 1, 0, nullable: false),
			new DmoField("padCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("padCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("padUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("padChildSupport", "bit", 1, 0, nullable: false),
			new DmoField("padStudentLoan", "bit", 1, 0, nullable: false),
			new DmoField("padStudentLoanType", "tinyint", 1, 0, nullable: false),
			new DmoField("padSuperannuationFundID", "nvarchar", 10, 0, nullable: false),
			new DmoField("padWageExcess", "money", 10, 2, nullable: false),
			new DmoField("padAusDeductionType", "nvarchar", 1, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("PADDEDUCTIONID", unique: true),
			new DmoIndex("PADUNIQUEID", unique: true),
			new DmoIndex("padOrganizationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
