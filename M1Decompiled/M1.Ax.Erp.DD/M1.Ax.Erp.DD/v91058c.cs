using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.058", "Add fields to ProductionProperties table", "2016-05-18")]
public class v91058c
{
	public v91058c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapJMJobMaterialSource"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapJMJobMaterialSource", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapJMJobMaterialSource"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapJMJobMaterialSource = 2");
		}
	}
}
