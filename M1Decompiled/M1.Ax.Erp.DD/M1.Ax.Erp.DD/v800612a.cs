using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.612", "Add fields to Allowances table", "2015-12-03")]
public class v800612a
{
	public v800612a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoIncludeInDeductionCalc"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoIncludeInDeductionCalc", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
