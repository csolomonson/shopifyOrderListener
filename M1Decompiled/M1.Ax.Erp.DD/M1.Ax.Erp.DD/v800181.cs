using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.181", "Add Fedex Payor Type to Freight Shipments", "2011-06-27")]
public class v800181
{
	public v800181(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FreightShipments", "fspFDXPayorType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FreightShipments", "FSPfdxPayorType", "char", 10, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
