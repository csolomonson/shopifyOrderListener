using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.094", "Add lmeUseEmail to Employees table", "2010-12-17")]
public class v800094
{
	public v800094(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Employees", "lmeUseEmail"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Employees", "lmeUseEmail", "numeric", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
