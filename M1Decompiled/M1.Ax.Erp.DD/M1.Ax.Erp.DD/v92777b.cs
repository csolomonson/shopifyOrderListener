using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.777", "Alter fields in ShippingProperties table", "2018-09-14")]
public class v92777b
{
	public v92777b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmUPSAccountNo"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmUPSAccountNo", "nvarchar", 6, 0, isNullable: false, parms.Messages);
		}
	}
}
