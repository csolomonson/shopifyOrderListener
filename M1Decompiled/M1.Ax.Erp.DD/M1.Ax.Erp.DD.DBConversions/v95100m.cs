using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.100", "Updating mrpWarehouseIDs field to MRPSessions table", "2021-11-04")]
public class v95100m
{
	public v95100m(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSessions", "mrpWarehouseID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSessions", "mrpWarehouseID", "mrpWarehouseIDs", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSessions", "mrpWarehouseIDs"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSessions", "mrpWarehouseIDs", "nvarchar(max)", 5, 0, isNullable: true, parms.Messages);
		}
	}
}
