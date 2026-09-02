using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.321", "Add fields to Form1099Years table", "2015-02-12")]
public class v800321h
{
	public v800321h(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099Years", "apyIncludeNoneInOther"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099Years", "apyIncludeNoneInOther", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
