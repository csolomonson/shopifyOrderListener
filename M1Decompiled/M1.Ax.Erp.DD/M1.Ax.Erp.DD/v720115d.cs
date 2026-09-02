using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.115", "Add Pay Slip Description to Income Tax Types", "2010-03-02")]
public class v720115d
{
	public v720115d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTypes", "pafPaySlipDescription"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTypes", "pafPaySlipDescription", "char", 50, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
