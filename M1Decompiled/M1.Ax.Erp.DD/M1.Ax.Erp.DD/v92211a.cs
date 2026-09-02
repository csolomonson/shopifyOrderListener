using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.211", "Add fields to Allowances table", "2017-04-04")]
public class v92211a
{
	public v92211a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoMonthlyThresholdAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoMonthlyThresholdAmount", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
