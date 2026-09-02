using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.091", "Add fields to ShippingProperties table", "2015-10-06")]
public class v900091a
{
	public v900091a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmUSDCurrencyCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmUSDCurrencyCode", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
