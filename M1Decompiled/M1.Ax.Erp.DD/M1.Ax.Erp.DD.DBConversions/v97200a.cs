using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.200", "Add field xapPMPurPlannerIncWhsQties to ProductionProperties table", "2024-03-27")]
public class v97200a
{
	public v97200a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapPMPurPlannerIncWhsQties"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapPMPurPlannerIncWhsQties", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapPMPurPlannerIncWhsQties = 0");
		}
	}
}
