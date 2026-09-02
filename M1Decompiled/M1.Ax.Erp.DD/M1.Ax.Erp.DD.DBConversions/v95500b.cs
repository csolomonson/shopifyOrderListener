using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.500", "Delete top activities were not been supported in M1 Forms", "2022-10-20")]
public class v95500b
{
	public v95500b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "RecentActivityLog"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "DELETE RecentActivityLog WHERE rxlExplorerType NOT IN ('Entry', 'Explorer', 'Report', 'Visualizer')");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "DELETE RecentActivityLog WHERE rxlExplorerType  = 'Visualizer' AND rxlVisualizerID = ''");
		}
	}
}
