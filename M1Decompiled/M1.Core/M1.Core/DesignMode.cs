using System.Data;

namespace M1.Core;

public static class DesignMode
{
	public static DataTable DesignModeGetDataTable(string query)
	{
		AppContext appContext = new AppContext(designMode: true);
		appContext.LoadConnectionInformation();
		DataTable dataTable = appContext.DDServerManager.GetDataTable(null, null, appContext.IsHosted ? appContext.Metadata.GetMetaData("DataDictionary") : appContext.Server.IniSettings.Get("DataDictionary", "M1DD"), 0, query);
		appContext.DDServerManager.ClearAllPools();
		return dataTable;
	}
}
