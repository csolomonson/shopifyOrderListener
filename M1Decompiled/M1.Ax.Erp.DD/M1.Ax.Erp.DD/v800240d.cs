using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.240", "Add columns wfpJobId to the WORKFLOWS tables new WorkFlow Functionality.", "2012-04-03")]
public class v800240d
{
	public v800240d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkFlows", "wfpJobId"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlows", "wfpJobId", "char", 20, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
