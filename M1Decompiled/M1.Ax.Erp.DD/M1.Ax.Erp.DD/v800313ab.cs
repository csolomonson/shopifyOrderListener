using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.313", "Add fields to PAYROLLRATES table", "2015-05-19")]
public class v800313ab
{
	public v800313ab(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PAYROLLRATES", "payIncludeInHolidayPayRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PAYROLLRATES", "payIncludeInHolidayPayRate", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PAYROLLRATES", "payUseProcessRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PAYROLLRATES", "payUseProcessRate", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
