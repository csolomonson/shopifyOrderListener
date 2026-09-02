using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.300", "Changing length on xasFdxHomeDeliveryType field in ShippingMethods table", "2024-07-29")]
public class v97300e
{
	public v97300e(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingMethods", "xasFdxHomeDeliveryType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingMethods", "xasFdxHomeDeliveryType", "nvarchar", 12, 0, isNullable: false, parms.Messages);
		}
	}
}
