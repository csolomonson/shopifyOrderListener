using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.126", "Set FedEx Account Number blank if the value is zero", "2016-01-27")]
public class v900126a
{
	public v900126a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFdxAccountNumber"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ShippingProperties Set xsmFdxAccountNumber = '' Where xsmFdxAccountNumber='0'");
		}
	}
}
