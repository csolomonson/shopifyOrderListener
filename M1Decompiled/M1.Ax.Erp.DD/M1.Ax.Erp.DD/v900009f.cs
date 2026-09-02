using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.009", "Add fields to MaterialIssueComponents table", "2014-10-31")]
public class v900009f
{
	public v900009f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents", "inkPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", "inkPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
