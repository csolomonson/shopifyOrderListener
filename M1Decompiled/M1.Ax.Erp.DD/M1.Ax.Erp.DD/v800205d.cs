using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add Exclude Termination Amounts in Allowances", "2011-12-06")]
public class v800205d
{
	public v800205d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoExcludeTerminateAmt"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoExcludeTerminateAmt", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
