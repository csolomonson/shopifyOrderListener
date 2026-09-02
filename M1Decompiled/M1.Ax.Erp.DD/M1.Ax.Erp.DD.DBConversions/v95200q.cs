using System.Collections.Generic;
using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Update allowance code based on new ATO documentation on payroll lines table", "2022-01-31")]
public class v95200q
{
	public v95200q(DBConversionParms parms)
	{
		foreach (string item in new List<string> { "UPDATE PayrollLines SET panAusAllowanceType = 'CD' where panAusAllowanceType = 'C'", "UPDATE PayrollLines SET panAusAllowanceType = 'AD' where panAusAllowanceType = 'T'", "UPDATE PayrollLines SET panAusAllowanceType = 'LD' where panAusAllowanceType = 'L'", "UPDATE PayrollLines SET panAusAllowanceType = 'MD' where panAusAllowanceType = 'M'", "UPDATE PayrollLines SET panAusAllowanceType = 'RD' where panAusAllowanceType = 'R'", "UPDATE PayrollLines SET panAusAllowanceType = 'OD' where panAusAllowanceType = 'O'" })
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, item);
		}
	}
}
