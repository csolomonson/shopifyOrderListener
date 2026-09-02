using System.Collections.Generic;
using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Update allowance code based on new ATO documentation on stp allowance table", "2022-01-31")]
public class v95200r
{
	public v95200r(DBConversionParms parms)
	{
		foreach (string item in new List<string> { "UPDATE STPAllowances SET staAllowanceType = 'CD' where staAllowanceType = 'C'", "UPDATE STPAllowances SET staAllowanceType = 'AD' where staAllowanceType = 'T'", "UPDATE STPAllowances SET staAllowanceType = 'LD' where staAllowanceType = 'L'", "UPDATE STPAllowances SET staAllowanceType = 'MD' where staAllowanceType = 'M'", "UPDATE STPAllowances SET staAllowanceType = 'RD' where staAllowanceType = 'R'", "UPDATE STPAllowances SET staAllowanceType = 'OD' where staAllowanceType = 'O'" })
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, item);
		}
	}
}
