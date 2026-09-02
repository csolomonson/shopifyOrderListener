using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.300", "Add fields xsmFedExTokenExpiresInTrack, xsmFedExAccessTokenTrack to ShippingProperties table", "2024-08-02")]
public class v97300g
{
	public v97300g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFedExTokenExpiresInTrack"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmFedExTokenExpiresInTrack", "datetime", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingProperties", "xsmFedExAccessTokenTrack"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", "xsmFedExAccessTokenTrack", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
