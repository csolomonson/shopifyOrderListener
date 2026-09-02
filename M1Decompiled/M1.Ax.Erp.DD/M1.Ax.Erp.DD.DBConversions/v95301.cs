using System.Collections.Generic;
using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.301", "Clean other allowances types in Allowances Table and Payroll Tables", "2022-07-01")]
public class v95301
{
	public v95301(DBConversionParms parms)
	{
		foreach (string item in new List<string> { "UPDATE Allowances SET paoAusOtherAllowanceType = '' where paoAusAllowanceType <> 'OD'", "UPDATE PayrollLines SET panAusOtherAllowanceType = '' where panAusAllowanceType <> 'OD'" })
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, item);
		}
	}
}
