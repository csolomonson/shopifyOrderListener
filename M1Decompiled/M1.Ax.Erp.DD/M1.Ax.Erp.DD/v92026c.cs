using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.026", "Add fields to MaterialIssueComponents table", "2016-11-21")]
public class v92026c
{
	public v92026c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents", "inkReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", "inkReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
