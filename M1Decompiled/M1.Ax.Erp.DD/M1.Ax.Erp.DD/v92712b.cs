using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.712", "Add fields to PayrollSessions table", "2018-05-04")]
public class v92712b
{
	public v92712b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollSessions", "pasTransferredToSTP"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollSessions", "pasTransferredToSTP", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollSessions", "pasSTPSessionID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollSessions", "pasSTPSessionID", "int", 9, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
