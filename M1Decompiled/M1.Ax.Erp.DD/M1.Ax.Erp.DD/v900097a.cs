using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.097", "Modify fields in ShippingProperties table", "2015-10-26")]
public class v900097a
{
	public v900097a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmUSDCurrencyCode"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmUSDCurrencyCode", "nvarchar", 5, 0, isNullable: false, parms.Messages);
		}
	}
}
