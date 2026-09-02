using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.309", "Add fields to ProductionProperties table", "2017-06-21")]
public class v92309a
{
	public v92309a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapPMPurPlannerUseBestPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapPMPurPlannerUseBestPrice", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapPMPurPlannerUseBestPrice = 1");
		}
	}
}
