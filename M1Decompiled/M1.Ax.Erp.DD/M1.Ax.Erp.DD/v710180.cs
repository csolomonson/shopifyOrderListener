using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.180", "Add field to workflowlines", "2008-12-11")]
public class v710180
{
	public v710180(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLines", "wflSequence"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLines", "wflSequence", "numeric", 4, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
