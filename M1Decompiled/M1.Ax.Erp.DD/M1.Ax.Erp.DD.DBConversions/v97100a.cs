using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.100", "Add field pahCAEmployer2CPPContributions to IncomeTaxYearTotals table", "2024-01-15")]
public class v97100a
{
	public v97100a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployer2CPPContributions"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahCAEmployer2CPPContributions", "money", 8, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
