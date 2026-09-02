using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.036", "Add Income Tax Year Total Allowances table", "2010-05-12")]
public class v800036
{
	public v800036(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotalAllowances"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotalAllowances");
		}
	}
}
