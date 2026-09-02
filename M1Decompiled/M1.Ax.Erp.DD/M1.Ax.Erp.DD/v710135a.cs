using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.135", "Add Default Time Labor Part Group to Financial Pro", "2008-09-11")]
public class v710135a
{
	public v710135a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafARDefaultLaborPartGroupID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafARDefaultLaborPartGroupID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
