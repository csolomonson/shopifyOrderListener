using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.272", "Add fields to PayrollHeaders table", "2017-01-24")]
public class v92272c
{
	public v92272c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaders", "patNumberOfStandardDays"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaders", "patNumberOfStandardDays", "numeric", 6, 3, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
