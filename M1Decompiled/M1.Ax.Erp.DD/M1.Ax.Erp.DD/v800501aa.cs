using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.501", "Add fields to ALLOWANCES table", "2015-05-19")]
public class v800501aa
{
	public v800501aa(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ALLOWANCES", "paoIncludeInHolidayPayRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ALLOWANCES", "paoIncludeInHolidayPayRate", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
