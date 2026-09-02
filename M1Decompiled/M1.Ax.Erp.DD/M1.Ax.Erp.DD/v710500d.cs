using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Resize and Add Total COGS Cost to Job Costs", "2009-02-18")]
public class v710500d
{
	public v710500d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobCosts", "jmcTotalCost"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobCosts", "jmcTotalCost", "numeric", 15, 5, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobCosts", "jmcTotalCOGSCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobCosts", "jmcTotalCOGSCost", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE JobCosts SET jmcTotalCOGSCost = jmcTotalCost Where jmcTotalCost <> 0");
		}
	}
}
