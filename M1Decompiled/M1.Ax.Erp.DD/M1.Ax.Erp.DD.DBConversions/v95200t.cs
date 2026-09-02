using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Update working holiday maker value on STPLines", "2022-03-10")]
public class v95200t
{
	public v95200t(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE STPLines SET stlWorkingHolidayMaker = 1 WHERE stlWorkingHolidayGrossPay > 0 OR stlWorkingHolidayPayGwAmount > 0;");
	}
}
