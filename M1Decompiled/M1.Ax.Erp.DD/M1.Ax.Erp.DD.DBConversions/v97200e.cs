using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.200", "Add field xsmUPSAccountNoOAuth to ShippingProperties table", "2024-05-08")]
public class v97200e
{
	public v97200e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmUPSAccountNoOAuth"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmUPSAccountNoOAuth", "nvarchar", 6, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
