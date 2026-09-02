using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollRates to support unicode", "2013-10-17")]
public class v810RebuildPayrollRates
{
	public v810RebuildPayrollRates(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollRates", new DmoField[21]
		{
			new DmoField("payPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("payDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("payPayType", "nvarchar", 1, 0, nullable: false),
			new DmoField("payPayFrequency", "tinyint", 1, 0, nullable: false),
			new DmoField("payShiftGroup1Type", "nvarchar", 1, 0, nullable: false),
			new DmoField("payShiftGroup1Rate", "numeric", 7, 4, nullable: false),
			new DmoField("payShiftGroup2Type", "nvarchar", 1, 0, nullable: false),
			new DmoField("payShiftGroup2Rate", "numeric", 7, 4, nullable: false),
			new DmoField("payShiftGroup3Type", "nvarchar", 1, 0, nullable: false),
			new DmoField("payShiftGroup3Rate", "numeric", 7, 4, nullable: false),
			new DmoField("payOvertimeIncludesStandard", "bit", 1, 0, nullable: false),
			new DmoField("payLeaveLoadingRate", "numeric", 7, 4, nullable: false),
			new DmoField("payUseSupplementalWagesTax", "bit", 1, 0, nullable: false),
			new DmoField("payAccrueRDO", "bit", 1, 0, nullable: false),
			new DmoField("payExportedRateID", "nvarchar", 10, 0, nullable: false),
			new DmoField("payCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("payCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("payUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("payIncludeInHolidayPayRate", "bit", 1, 0, nullable: false),
			new DmoField("payUseProcessRate", "bit", 1, 0, nullable: false),
			new DmoField("payLeaveType", "nvarchar", 1, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("PAYPAYROLLRATEID", unique: true),
			new DmoIndex("PAYUNIQUEID", unique: true),
			new DmoIndex("payPayType", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
