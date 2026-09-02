using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.300", "Add field xsmFedExAuthenticationMethod to ShippingProperties table", "2024-05-29")]
public class v97300a
{
	public v97300a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFedExAuthenticationMethod"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmFedExAuthenticationMethod", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
