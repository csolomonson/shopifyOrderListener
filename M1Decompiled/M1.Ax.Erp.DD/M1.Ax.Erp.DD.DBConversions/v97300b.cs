using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.300", "Update field xsmFedExAuthenticationMethod to ShippingProperties table", "2024-05-29")]
public class v97300b
{
	public v97300b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFedExAuthenticationMethod"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ShippingProperties SET xsmFedExAuthenticationMethod = CASE WHEN xsmFdxAccountNumber = '' THEN 3 ELSE 1 END");
		}
	}
}
