using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.115", "Add Allowance Tax Exemptions table", "2010-03-02")]
public class v720115
{
	public v720115(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "AllowanceTaxExemptions"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AllowanceTaxExemptions");
		}
	}
}
