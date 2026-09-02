using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.571", "Convert FedEx Account Number to alpha numeric field", "2016-01-22")]
public class v800571a
{
	public v800571a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFdxAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmFdxAccountNumber", "varchar", 15, 0, parms.Messages);
		}
	}
}
