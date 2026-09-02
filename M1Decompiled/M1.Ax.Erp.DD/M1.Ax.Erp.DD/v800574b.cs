using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.574", "Add fields to EmployeePersonalData table", "2015-08-07")]
public class v800574b
{
	public v800574b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePersonalData", "lmdEmploymentStatus"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePersonalData", "lmdEmploymentStatus", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
