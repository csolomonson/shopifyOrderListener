using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.400", "Add field smlWMSPickInProgress to ShipmentLines table", "2024-08-30")]
public class v97400e
{
	public v97400e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentLines", "smlWMSPickInProgress"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentLines", "smlWMSPickInProgress", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
