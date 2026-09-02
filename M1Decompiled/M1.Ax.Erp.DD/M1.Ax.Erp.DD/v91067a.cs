using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.067", "Remove fields from ProductionProperties table", "2016-06-03")]
public class v91067a
{
	public v91067a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapQMNumberOfDecimals"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapQMNumberOfDecimals", dropTriggers: true);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS81.', 'M1CONTROLS92.')");
	}
}
