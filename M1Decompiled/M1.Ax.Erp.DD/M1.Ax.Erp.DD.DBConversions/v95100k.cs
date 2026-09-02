using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.100", "Updating mrpPlantIDs field to MRPSessions table", "2021-11-03")]
public class v95100k
{
	public v95100k(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSessions", "mrpPlantID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSessions", "mrpPlantID", "mrpPlantIDs", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSessions", "mrpPlantIDs"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSessions", "mrpPlantIDs", "nvarchar(max)", 5, 0, isNullable: true, parms.Messages);
		}
	}
}
