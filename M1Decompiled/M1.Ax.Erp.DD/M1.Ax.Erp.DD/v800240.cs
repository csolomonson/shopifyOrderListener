using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.240", "Add columns wflStatus and wflPriority to the WORKFLOWLINES tables for Implementation CheckList.", "2012-03-21")]
public class v800240
{
	public v800240(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLines", "wflStatus"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLines", "wflStatus", "char", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLines", "wflPriority"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLines", "wflPriority", "char", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
