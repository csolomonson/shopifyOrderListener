using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Remove fields to MRPSupply table", "2022-01-25")]
public class v95200o
{
	public v95200o(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSupply", "mrsSupplyQuantity"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSupply", "mrsSupplyQuantity", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSupply", "mrsOriginalQuantity"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSupply", "mrsOriginalQuantity", dropTriggers: true);
		}
	}
}
