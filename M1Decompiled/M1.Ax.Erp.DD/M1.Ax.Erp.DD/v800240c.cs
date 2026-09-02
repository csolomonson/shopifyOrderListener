using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.240", "Add columns wflMilestone to the WORKFLOWLINES tables for Implementation CheckList.", "2012-03-21")]
public class v800240c
{
	public v800240c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLines", "wflMileStone"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLines", "wflMileStone", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
