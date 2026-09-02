using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.100", "Add customer field to MRP Supplier table", "2021-11-22")]
public class v95100o
{
	public v95100o(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSupply", "mrsCustomerOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSupply", "mrsCustomerOrganizationID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
