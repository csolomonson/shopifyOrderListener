using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Timecards to support unicode", "2013-10-17")]
public class v810RebuildTimecards
{
	public v810RebuildTimecards(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Timecards", new DmoField[45]
		{
			new DmoField("lmpTimecardID", "int", 9, 0, nullable: false),
			new DmoField("lmpEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmpPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmpTimecardDate", "date", 14, 0, nullable: true),
			new DmoField("lmpShiftID", "smallint", 3, 0, nullable: false),
			new DmoField("lmpShiftBreakID", "tinyint", 1, 0, nullable: false),
			new DmoField("lmpRoundedStartTime", "datetime", 14, 0, nullable: true),
			new DmoField("lmpRoundedEndTime", "datetime", 14, 0, nullable: true),
			new DmoField("lmpActualStartTime", "datetime", 14, 0, nullable: true),
			new DmoField("lmpActualEndTime", "datetime", 14, 0, nullable: true),
			new DmoField("lmpNoteRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmpNoteText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmpSource", "tinyint", 1, 0, nullable: false),
			new DmoField("lmpLeaveAccrualID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmpActive", "bit", 1, 0, nullable: false),
			new DmoField("lmpTransferredToPayroll", "bit", 1, 0, nullable: false),
			new DmoField("lmpTransferredDate", "date", 14, 0, nullable: true),
			new DmoField("lmpPayrollHours", "numeric", 8, 2, nullable: false),
			new DmoField("lmpMachineHours", "numeric", 8, 2, nullable: false),
			new DmoField("lmpOTPeriod1Hours", "numeric", 8, 2, nullable: false),
			new DmoField("lmpOTPeriod2Hours", "numeric", 8, 2, nullable: false),
			new DmoField("lmpOTPeriod3Hours", "numeric", 8, 2, nullable: false),
			new DmoField("lmpOTPeriod4Hours", "numeric", 8, 2, nullable: false),
			new DmoField("lmpOTPeriod1PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmpOTPeriod2PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmpOTPeriod3PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmpOTPeriod4PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmpStandardHours", "numeric", 8, 2, nullable: false),
			new DmoField("lmpTotalPayrollHours", "numeric", 9, 2, nullable: false),
			new DmoField("lmpStandardPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmpOtherPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmpOtherHours", "numeric", 8, 2, nullable: false),
			new DmoField("lmpLastEndTime", "datetime", 14, 0, nullable: true),
			new DmoField("lmpPostedToWIP", "bit", 1, 0, nullable: false),
			new DmoField("lmpPostedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmpPaidDate", "date", 14, 0, nullable: true),
			new DmoField("lmpAutoClockedOut", "bit", 1, 0, nullable: false),
			new DmoField("lmpProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmpCreatedFromPayrollSession", "bit", 1, 0, nullable: false),
			new DmoField("lmpExchangeID", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmpUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("lmpCreatedFromMobile", "bit", 1, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("LMPTIMECARDID", unique: true),
			new DmoIndex("LMPUNIQUEID", unique: true),
			new DmoIndex("lmpEmployeeID", unique: false),
			new DmoIndex("lmpPlantDepartmentID", unique: false),
			new DmoIndex("lmpPlantID", unique: false),
			new DmoIndex("lmpTimecardDate", unique: false),
			new DmoIndex("lmpRoundedStartTime", unique: false),
			new DmoIndex("lmpRoundedEndTime", unique: false),
			new DmoIndex("lmpLeaveAccrualID", unique: false),
			new DmoIndex("lmpActive", unique: false),
			new DmoIndex("lmpTransferredToPayroll", unique: false),
			new DmoIndex("lmpProjectID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
