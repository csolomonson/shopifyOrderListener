using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add delivery date to service contract owners", "2011-12-06")]
public class v800205i
{
	public v800205i(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ServiceContractOwners", "kboDeliveryDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ServiceContractOwners", "kboDeliveryDate", "date", 14, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
