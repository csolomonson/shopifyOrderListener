using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.240", "Add WorkFlowLineResources Table", "2012-03-21")]
public class v800240a
{
	public v800240a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources");
			return;
		}
		parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "WorkFlowLineResources");
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources");
	}
}
