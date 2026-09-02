using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.523", "Add fields to EMPLOYEEBANKACCOUNTS table", "2015-05-19")]
public class v800523af
{
	public v800523af(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEBANKACCOUNTS", "pabEFTDescription"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEBANKACCOUNTS", "pabEFTDescription", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
