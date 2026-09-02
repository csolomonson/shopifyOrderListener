using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollHeaderTotalLines to support unicode", "2013-10-17")]
public class v810RebuildPayrollHeaderTotalLines
{
	public v810RebuildPayrollHeaderTotalLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotalLines", new DmoField[46]
		{
			new DmoField("paiPayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("paiPayrollHeaderID", "int", 7, 0, nullable: false),
			new DmoField("paiPayrollHeaderTotalLineID", "int", 6, 0, nullable: false),
			new DmoField("paiTimecardID", "int", 9, 0, nullable: false),
			new DmoField("paiTimecardLineID", "smallint", 4, 0, nullable: false),
			new DmoField("paiTimecardDate", "date", 14, 0, nullable: true),
			new DmoField("paiStandardHours", "numeric", 8, 2, nullable: false),
			new DmoField("paiStandardPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiOTPeriod1Hours", "numeric", 8, 2, nullable: false),
			new DmoField("paiOTPeriod1PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiOTPeriod2Hours", "numeric", 8, 2, nullable: false),
			new DmoField("paiOTPeriod2PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiOTPeriod3Hours", "numeric", 8, 2, nullable: false),
			new DmoField("paiOTPeriod3PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiOTPeriod4Hours", "numeric", 8, 2, nullable: false),
			new DmoField("paiOTPeriod4PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiPublicHolidayPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiShiftGroup", "tinyint", 1, 0, nullable: false),
			new DmoField("paiGrossPayAmount", "numeric", 9, 2, nullable: false),
			new DmoField("paiPayrollEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("paiStandardGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paiOTPeriod1GLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paiOTPeriod2GLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paiOTPeriod4GLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paiOTPeriod3GLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paiBreakPeriod1Hours", "numeric", 8, 2, nullable: false),
			new DmoField("paiBreakPeriod2Hours", "numeric", 8, 2, nullable: false),
			new DmoField("paiBreakPeriod3Hours", "numeric", 8, 2, nullable: false),
			new DmoField("paiBreakPeriod4Hours", "numeric", 8, 2, nullable: false),
			new DmoField("paiBreakPeriod1PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiBreakPeriod2PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiBreakPeriod3PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiBreakPeriod4PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiBreakPeriod1GLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paiBreakPeriod2GLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paiBreakPeriod3GLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paiBreakPeriod4GLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paiPayRate", "numeric", 8, 4, nullable: false),
			new DmoField("paiRDODay", "numeric", 8, 4, nullable: false),
			new DmoField("paiNumberOfPays", "tinyint", 2, 0, nullable: false),
			new DmoField("paiLeaveAccrualID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paiPostedtoGL", "bit", 1, 0, nullable: false),
			new DmoField("paiCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("paiCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("paiUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("paiProcessPayRate", "numeric", 8, 4, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("PAIPAYROLLSESSIONID,PAIPAYROLLHEADERID,PAIPAYROLLHEADERTOTALLINEID", unique: true),
			new DmoIndex("PAIUNIQUEID", unique: true),
			new DmoIndex("paiPayrollSessionID", unique: false),
			new DmoIndex("paiPayrollHeaderID", unique: false),
			new DmoIndex("paiPayrollHeaderTotalLineID", unique: false),
			new DmoIndex("paiTimecardID", unique: false),
			new DmoIndex("paiTimecardLineID", unique: false),
			new DmoIndex("paiTimecardDate", unique: false),
			new DmoIndex("paiPayrollEmployeeID", unique: false),
			new DmoIndex("paiLeaveAccrualID", unique: false),
			new DmoIndex("paiPostedtoGL", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
