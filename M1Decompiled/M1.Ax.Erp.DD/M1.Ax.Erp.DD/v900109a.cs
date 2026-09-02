using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.109", "Alter fields in ShippingProperties table", "2015-11-30")]
public class v900109a
{
	public v900109a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFdxAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmFdxAccountNumber", "nvarchar", 15, 0, isNullable: false, parms.Messages);
		}
	}
}
