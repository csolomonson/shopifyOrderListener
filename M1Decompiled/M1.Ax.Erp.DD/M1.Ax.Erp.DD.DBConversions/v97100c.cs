using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.100", "Update lmdStateAus field with lmdState field in the EmployeePersonalData table when the upgrading process occurs", "2024-30-01")]
public class v97100c
{
	public v97100c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePersonalData", "lmdStateAus"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE EmployeePersonalData SET lmdStateAus = lmdState WHERE lmdState IN ('ACT', 'NSW', 'NT', 'QLD', 'SA', 'TAS', 'VIC', 'WA') AND (SELECT xadRegion FROM DatasetProperties) = 'AUS'");
		}
	}
}
