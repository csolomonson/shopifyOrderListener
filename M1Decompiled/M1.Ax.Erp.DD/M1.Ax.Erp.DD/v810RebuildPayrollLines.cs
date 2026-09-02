using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollLines to support unicode", "2013-10-17")]
public class v810RebuildPayrollLines
{
	public v810RebuildPayrollLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollLines", new DmoField[39]
		{
			new DmoField("panPayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("panPayrollHeaderID", "int", 7, 0, nullable: false),
			new DmoField("panPayrollLineID", "smallint", 4, 0, nullable: false),
			new DmoField("panDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("panReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("panAdditionalAmount", "money", 10, 2, nullable: false),
			new DmoField("panAmount", "money", 10, 2, nullable: false),
			new DmoField("panOverrideAmount", "bit", 1, 0, nullable: false),
			new DmoField("panPayrollLineType", "nvarchar", 1, 0, nullable: false),
			new DmoField("panAppliedPayAmount", "money", 10, 2, nullable: false),
			new DmoField("panAppliedHours", "numeric", 9, 2, nullable: false),
			new DmoField("panEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("panEmployeeAllowanceID", "smallint", 4, 0, nullable: false),
			new DmoField("panAllowanceID", "nvarchar", 5, 0, nullable: false),
			new DmoField("panEmployeeDeductionID", "smallint", 4, 0, nullable: false),
			new DmoField("panDeductionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("panEmployeeIncomeTaxID", "smallint", 4, 0, nullable: false),
			new DmoField("panIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("panIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("panIncomeTaxTableID", "nvarchar", 10, 0, nullable: false),
			new DmoField("panAccrualGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("panExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("panOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("panCompleted", "bit", 1, 0, nullable: false),
			new DmoField("panPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("panNumberOfPays", "tinyint", 2, 0, nullable: false),
			new DmoField("panAppliedUnit", "numeric", 12, 4, nullable: false),
			new DmoField("panRate", "numeric", 8, 4, nullable: false),
			new DmoField("panSalarySacrifice", "bit", 1, 0, nullable: false),
			new DmoField("panAUSReportableAmount", "money", 10, 2, nullable: false),
			new DmoField("panAusAllowanceType", "nvarchar", 2, 0, nullable: false),
			new DmoField("panAusOtherAllowanceType", "nvarchar", 40, 0, nullable: false),
			new DmoField("panAusDeductionType", "nvarchar", 1, 0, nullable: false),
			new DmoField("panAusETPCode", "nvarchar", 1, 0, nullable: false),
			new DmoField("panAusETPTaxFreeComponent", "money", 10, 2, nullable: false),
			new DmoField("panAusETPTaxableComponent", "money", 10, 2, nullable: false),
			new DmoField("panCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("panCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("panUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[17]
		{
			new DmoIndex("PANPAYROLLSESSIONID,PANPAYROLLHEADERID,PANPAYROLLLINEID", unique: true),
			new DmoIndex("PANUNIQUEID", unique: true),
			new DmoIndex("panPayrollSessionID", unique: false),
			new DmoIndex("panPayrollHeaderID", unique: false),
			new DmoIndex("panPayrollLineID", unique: false),
			new DmoIndex("panEmployeeID", unique: false),
			new DmoIndex("panEmployeeAllowanceID", unique: false),
			new DmoIndex("panAllowanceID", unique: false),
			new DmoIndex("panEmployeeDeductionID", unique: false),
			new DmoIndex("panDeductionID", unique: false),
			new DmoIndex("panEmployeeIncomeTaxID", unique: false),
			new DmoIndex("panIncomeTaxID", unique: false),
			new DmoIndex("panIncomeTaxTypeID", unique: false),
			new DmoIndex("panIncomeTaxTableID", unique: false),
			new DmoIndex("panOrganizationID", unique: false),
			new DmoIndex("panCompleted", unique: false),
			new DmoIndex("panPostedToGL", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
