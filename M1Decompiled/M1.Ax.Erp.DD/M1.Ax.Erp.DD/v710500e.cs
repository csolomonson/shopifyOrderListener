using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Add Retained Earnings Account to GL Divisions", "2009-02-19")]
public class v710500e
{
	public v710500e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLDivisions", "glvRetainedEarningsAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLDivisions", "glvRetainedEarningsAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
