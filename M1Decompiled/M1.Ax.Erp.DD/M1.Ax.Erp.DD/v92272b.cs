using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.272", "Add fields to PayrollDefinitions table", "2017-01-24")]
public class v92272b
{
	public v92272b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollDefinitions", "lmrNumberOfStandardDays"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollDefinitions", "lmrNumberOfStandardDays", "numeric", 6, 3, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
