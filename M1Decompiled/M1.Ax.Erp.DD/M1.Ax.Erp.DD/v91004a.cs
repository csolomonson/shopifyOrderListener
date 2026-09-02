using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.004", "Add fields to EmployeeMemos table", "2016-01-18")]
public class v91004a
{
	public v91004a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeMemos", "lmkShowInEmployees"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeMemos", "lmkShowInEmployees", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
