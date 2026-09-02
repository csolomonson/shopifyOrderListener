using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Rename column from mrsDemandQuantity to mrsSupplyQuantity on MRPSupply", "2021-11-30")]
public class v95200a
{
	public v95200a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSupply", "mrsDemandQuantity"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSupply", "mrsDemandQuantity", "mrsSupplyQuantity", dropTriggers: true);
		}
	}
}
