using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add OT calculation field to Payroll Definitions", "2008-05-21")]
public class v710000q
{
	public v710000q(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollDefinitions", "lmrCalcBeforeShift"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollDefinitions", "lmrCalcBeforeShift", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
