using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.119", "Add fields to CallMemos table", "2016-01-04")]
public class v900119a
{
	public v900119a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "CallMemos", "kbkShowInCalls"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CallMemos", "kbkShowInCalls", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
