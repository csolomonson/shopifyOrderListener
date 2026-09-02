using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeDeductions to support unicode", "2013-10-17")]
public class v810RebuildEmployeeDeductions
{
	public v810RebuildEmployeeDeductions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeDeductions", new DmoField[40]
		{
			new DmoField("paeEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("paeEmployeeDeductionID", "smallint", 4, 0, nullable: false),
			new DmoField("paeDeductionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("paeMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("paeDeductionTaxMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("paeDeductionStartDate", "date", 14, 0, nullable: true),
			new DmoField("paeDeductionEndDate", "date", 14, 0, nullable: true),
			new DmoField("paeAmount", "money", 10, 2, nullable: false),
			new DmoField("paePercent", "numeric", 8, 4, nullable: false),
			new DmoField("paeReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("paeExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paeAccrualGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paeLimitPerCheck", "money", 10, 2, nullable: false),
			new DmoField("paeLimitPerPayPeriod", "money", 10, 2, nullable: false),
			new DmoField("paeLimitPerYear", "money", 10, 2, nullable: false),
			new DmoField("paeDecliningBalance", "bit", 1, 0, nullable: false),
			new DmoField("paeDecliningBalanceAmount", "money", 10, 2, nullable: false),
			new DmoField("paeDeductionPeriod1", "bit", 1, 0, nullable: false),
			new DmoField("paeDeductionPeriod2", "bit", 1, 0, nullable: false),
			new DmoField("paeDeductionPeriod3", "bit", 1, 0, nullable: false),
			new DmoField("paeDeductionPeriod4", "bit", 1, 0, nullable: false),
			new DmoField("paeDeductionPeriod5", "bit", 1, 0, nullable: false),
			new DmoField("paeDeductionPeriod6", "bit", 1, 0, nullable: false),
			new DmoField("paeOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("paeRate", "numeric", 8, 4, nullable: false),
			new DmoField("paeSalarySacrifice", "bit", 1, 0, nullable: false),
			new DmoField("paeSuperannuation", "bit", 1, 0, nullable: false),
			new DmoField("paeUnionFees", "bit", 1, 0, nullable: false),
			new DmoField("paeInactive", "bit", 1, 0, nullable: false),
			new DmoField("paeInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("paeCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("paeCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("paeUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("paeChildSupport", "bit", 1, 0, nullable: false),
			new DmoField("paeChildSupportCode", "nvarchar", 1, 0, nullable: false),
			new DmoField("paeMemberID", "nvarchar", 20, 0, nullable: false),
			new DmoField("paeSpouseContribution", "bit", 1, 0, nullable: false),
			new DmoField("paeStudentLoan", "bit", 1, 0, nullable: false),
			new DmoField("paeStudentLoanType", "tinyint", 1, 0, nullable: false),
			new DmoField("paeSuperannuationFundID", "nvarchar", 10, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("PAEEMPLOYEEID,PAEEMPLOYEEDEDUCTIONID", unique: true),
			new DmoIndex("PAEUNIQUEID", unique: true),
			new DmoIndex("paeEmployeeID", unique: false),
			new DmoIndex("paeOrganizationID", unique: false),
			new DmoIndex("paeInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
