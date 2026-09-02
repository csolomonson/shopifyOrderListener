using System.Collections.Generic;
using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Update allowance code based on new ATO documentation on allowance table", "2022-01-31")]
public class v95200p
{
	public v95200p(DBConversionParms parms)
	{
		foreach (string item in new List<string> { "UPDATE Allowances SET paoAusAllowanceType = 'CD' where paoAusAllowanceType = 'C'", "UPDATE Allowances SET paoAusAllowanceType = 'AD' where paoAusAllowanceType = 'T'", "UPDATE Allowances SET paoAusAllowanceType = 'LD' where paoAusAllowanceType = 'L'", "UPDATE Allowances SET paoAusAllowanceType = 'MD' where paoAusAllowanceType = 'M'", "UPDATE Allowances SET paoAusAllowanceType = 'RD' where paoAusAllowanceType = 'R'", "UPDATE Allowances SET paoAusAllowanceType = 'OD' where paoAusAllowanceType = 'O'" })
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, item);
		}
	}
}
