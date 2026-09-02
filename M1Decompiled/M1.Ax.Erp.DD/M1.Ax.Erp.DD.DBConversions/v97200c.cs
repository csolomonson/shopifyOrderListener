using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.200", "Update field xsmUPSAuthenticationMethod to ShippingProperties table", "2024-04-16")]
public class v97200c
{
	public v97200c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmUPSAuthenticationMethod"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ShippingProperties SET xsmUPSAuthenticationMethod = CASE WHEN xsmUPSAccountNo = '' THEN 3 ELSE 1 END");
		}
	}
}
