using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.035", "Add Field to JobMemos", "2008-07-16")]
public class v710035a
{
	public v710035a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMemos", "jmkShowInJobs"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMemos", "jmkShowInJobs", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
