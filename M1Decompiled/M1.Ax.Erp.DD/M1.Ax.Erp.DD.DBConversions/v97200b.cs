using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.200", "Add field xsmUPSAuthenticationMethod to ShippingProperties table", "2024-08-21")]
public class v97200b
{
	public v97200b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmUPSAuthenticationMethod"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmUPSAuthenticationMethod", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
