using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.200", "Add fields xsmUPSAccessToken, xsmUPSRefreshToken to ShippingProperties table", "2024-04-28")]
public class v97200d
{
	public v97200d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmUPSAccessToken"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmUPSAccessToken", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmUPSRefreshToken"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmUPSRefreshToken", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
