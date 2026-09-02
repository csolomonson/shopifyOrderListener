using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.140", "Add WorkFlow tables", "2008-09-24")]
public class v710140a
{
	public v710140a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "WorkFlows"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlows");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "WorkFlowLines"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLines");
		}
	}
}
