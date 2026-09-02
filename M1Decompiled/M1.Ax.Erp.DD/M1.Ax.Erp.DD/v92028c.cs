using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.028", "Add fields to ShipmentComponents table", "2016-11-22")]
public class v92028c
{
	public v92028c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentComponents", "smoReverseShipmentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentComponents", "smoReverseShipmentID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentComponents", "smoReverseShipmentLineID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentComponents", "smoReverseShipmentLineID", "smallint", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentComponents", "smoReverseShipmentComponentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentComponents", "smoReverseShipmentComponentID", "smallint", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentComponents", "smoReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentComponents", "smoReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
