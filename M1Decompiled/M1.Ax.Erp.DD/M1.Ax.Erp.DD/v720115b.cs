using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.115", "Add Canada Tax Box Info to Allowance/Deductions", "2010-03-02")]
public class v720115b
{
	public v720115b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Deductions", "padCanadaTaxBoxInfo"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", "padCanadaTaxBoxInfo", "numeric", 2, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoCanadaTaxBoxInfo"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoCanadaTaxBoxInfo", "numeric", 2, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
