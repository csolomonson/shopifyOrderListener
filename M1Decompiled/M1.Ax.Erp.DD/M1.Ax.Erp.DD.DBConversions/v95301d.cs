using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.301", "Update leaves type id and leaves description from payroll header totals table according to payroll rates table", "2022-07-19")]
public class v95301d
{
	public v95301d(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PayrollHeaderTotals SET PayrollHeaderTotals.pagAusLeaveTypeID = pr.payLeaveType, PayrollHeaderTotals.pagAusLeaveTypeDescription = CASE WHEN pr.payLeaveType = 'C' THEN 'Cash Out of Leave' WHEN pr.payLeaveType = 'U' THEN 'Unused Leave on termination' WHEN pr.payLeaveType = 'P' THEN 'Paid Parental Leave' WHEN pr.payLeaveType = 'W' THEN 'Workers Comp' WHEN pr.payLeaveType = 'A' THEN 'Ancillary and Defence Leave' WHEN pr.payLeaveType = 'O' THEN 'Other Paid Leave' ELSE '<None>' END FROM PayrollHeaderTotals pht INNER JOIN PayrollRates pr ON pr.payPayrollRateID = pht.pagPayrollRateID");
	}
}
