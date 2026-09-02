using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Add Include in Tax Calculations to Allowances", "2009-03-25")]
public class v710500g
{
	public v710500g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoIncludeInTaxCalc"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoIncludeInTaxCalc", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
