using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.240", "Drop Column wfrPercentAllocation.", "2012-05-21")]
public class v800240f
{
	public v800240f(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLineResources", "wfrPercentAllocation"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLineResources", "wfrPercentAllocation", dropTriggers: true);
		}
	}
}
