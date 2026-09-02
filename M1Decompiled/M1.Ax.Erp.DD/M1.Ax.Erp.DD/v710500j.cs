using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Update Overtime Include Standard In Payroll Rates", "2009-03-31")]
public class v710500j
{
	public v710500j(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PayrollRates SET payOvertimeIncludesStandard = 0 WHERE payPayType <> 'O' ");
	}
}
