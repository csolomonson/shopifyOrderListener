using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to RMAClaims table", "2014-12-15")]
public class v900014c
{
	public v900014c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaims", "rapActualHoursTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaims", "rapActualHoursTotal", "numeric", 8, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaims", "rapLaborTotalForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAClaims Set rapLaborTotalForeign = rapActualHoursTotal*rapLaborRateForeign");
		}
	}
}
