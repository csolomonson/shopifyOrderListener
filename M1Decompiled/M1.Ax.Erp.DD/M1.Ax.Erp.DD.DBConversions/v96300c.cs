using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.300", "Add fields to IncomeTaxYears table", "2023-10-30")]
public class v96300c
{
	public v96300c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYears", "papCAEmployee2CPPContributions"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYears", "papCAEmployee2CPPContributions", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYears", "papCAEmployer2CPPContributions"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYears", "papCAEmployer2CPPContributions", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
