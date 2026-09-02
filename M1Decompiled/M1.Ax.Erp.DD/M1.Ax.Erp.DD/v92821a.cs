using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.821", "Add EDI shipment ready field to Shipments table", "2019-04-02")]
public class v92821a
{
	public v92821a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Shipments", "smpEDIShipmentReady"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Shipments", "smpEDIShipmentReady", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
