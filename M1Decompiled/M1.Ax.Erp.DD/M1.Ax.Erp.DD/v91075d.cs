using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.075", "Set Part Class Reorder Method to min/max", "2016-06-11")]
public class v91075d
{
	public v91075d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartClasses", "imcReorderMethod"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartClasses set imcReorderMethod = 1 where imcReorderMethod = 0");
		}
	}
}
