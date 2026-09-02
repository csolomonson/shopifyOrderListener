using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.180", "Add Landed Cost ID to GL Journals", "2008-11-18")]
public class v710180c
{
	public v710180c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournals", "glpLandedCostID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournals", "glpLandedCostID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
