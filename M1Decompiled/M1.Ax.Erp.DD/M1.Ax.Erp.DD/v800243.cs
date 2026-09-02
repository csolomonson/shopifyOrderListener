using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.243", "Add Column wflSequenceTask to WorkFlowLines table.", "2012-05-24")]
public class v800243
{
	public v800243(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlowLines", "wflSequenceTask"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLines", "wflSequenceTask", "numeric", 4, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
