using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.579", "Add Wage Excess field to Deductions table", "2015-10-07")]
public class v800579a
{
	public v800579a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Deductions", "padWageExcess"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", "padWageExcess", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
