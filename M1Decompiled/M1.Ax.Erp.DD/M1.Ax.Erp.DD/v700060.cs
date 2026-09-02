using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.060", "Add Salary Sacrifice field to Financial Properties", "2008-03-06")]
public class v700060
{
	public v700060(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafRecalcSalarySacrifice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafRecalcSalarySacrifice", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
