using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.135", "Increase xsmFdxHomeDeliveryType Length", "2011-05-18")]
public class v800135
{
	public v800135(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFdxHomeDeliveryType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmFdxHomeDeliveryType", "varchar", 12, 0, parms.Messages);
		}
	}
}
