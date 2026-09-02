using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.300", "Add fields xsmFedExAccessToken to ShippingProperties table", "2024-05-29")]
public class v97300c
{
	public v97300c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFedExAccessToken"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmFedExAccessToken", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
