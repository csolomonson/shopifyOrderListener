using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.300", "Add field xsmFedExTokenExpiresIn to ShippingProperties table", "2024-06-12")]
public class v97300d
{
	public v97300d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFedExTokenExpiresIn"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmFedExTokenExpiresIn", "datetime", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
