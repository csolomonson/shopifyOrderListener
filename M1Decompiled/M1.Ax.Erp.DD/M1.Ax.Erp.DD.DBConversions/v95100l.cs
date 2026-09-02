using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.100", "Add apply period field to Payroll Sessions table", "2021-11-07")]
public class v95100l
{
	public v95100l(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollSessions", "pasApplyPeriod"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollSessions", "pasApplyPeriod", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
