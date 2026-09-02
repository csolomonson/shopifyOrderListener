using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.100", "Add another state field to EmployeePersonalData table only for AUS region", "2024-01-26")]
public class v97100b
{
	public v97100b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePersonalData", "lmdStateAus"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePersonalData", "lmdStateAus", "nvarchar", 3, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
