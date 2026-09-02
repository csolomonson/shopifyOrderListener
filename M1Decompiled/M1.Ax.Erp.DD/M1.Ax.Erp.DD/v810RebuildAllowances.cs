using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Allowances to support unicode", "2013-10-17")]
public class v810RebuildAllowances
{
	public v810RebuildAllowances(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", new DmoField[33]
		{
			new DmoField("paoAllowanceID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paoAllowanceDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("paoPaidBy", "tinyint", 1, 0, nullable: false),
			new DmoField("paoMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("paoAllowanceTaxMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("paoDefaultAmount", "money", 10, 2, nullable: false),
			new DmoField("paoDefaultPercent", "numeric", 8, 4, nullable: false),
			new DmoField("paoExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paoAccrualGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paoOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("paoPrintOnPaySlip", "bit", 1, 0, nullable: false),
			new DmoField("paoExcludeLeaveLoading", "bit", 1, 0, nullable: false),
			new DmoField("paoIncludeInGrossAmount", "bit", 1, 0, nullable: false),
			new DmoField("paoPeriod1", "bit", 1, 0, nullable: false),
			new DmoField("paoPeriod2", "bit", 1, 0, nullable: false),
			new DmoField("paoPeriod3", "bit", 1, 0, nullable: false),
			new DmoField("paoPeriod4", "bit", 1, 0, nullable: false),
			new DmoField("paoPeriod5", "bit", 1, 0, nullable: false),
			new DmoField("paoPeriod6", "bit", 1, 0, nullable: false),
			new DmoField("paoDefaultRate", "numeric", 8, 4, nullable: false),
			new DmoField("paoIncludeInGrossPAYG", "bit", 1, 0, nullable: false),
			new DmoField("paoIncludeInTaxCalc", "bit", 1, 0, nullable: false),
			new DmoField("paoSuperannuation", "bit", 1, 0, nullable: false),
			new DmoField("paoExcludeTerminateAmt", "bit", 1, 0, nullable: false),
			new DmoField("paoCanadaTaxBoxInfo", "tinyint", 2, 0, nullable: false),
			new DmoField("paoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("paoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("paoUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("paoIncludeInHolidayPayRate", "bit", 1, 0, nullable: false),
			new DmoField("paoIncludeInDeductionCalc", "bit", 1, 0, nullable: false),
			new DmoField("paoSuperannuationFundID", "nvarchar", 10, 0, nullable: false),
			new DmoField("paoAusAllowanceType", "nvarchar", 2, 0, nullable: false),
			new DmoField("paoAusOtherAllowanceType", "nvarchar", 40, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("PAOALLOWANCEID", unique: true),
			new DmoIndex("PAOUNIQUEID", unique: true),
			new DmoIndex("paoOrganizationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
