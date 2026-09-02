using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.611", "Remove fields to SHIPPINGMETHODS table", "2015-11-24")]
public class v800611a
{
	public v800611a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPPINGMETHODS", "xasViaIFS"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPPINGMETHODS", "xasViaIFS", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPPINGMETHODS", "xasApplyFastestDelivery"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPPINGMETHODS", "xasApplyFastestDelivery", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPPINGMETHODS", "xasUseReceiverDefaults"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPPINGMETHODS", "xasUseReceiverDefaults", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPPINGMETHODS", "xasApplyCostWithDel"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPPINGMETHODS", "xasApplyCostWithDel", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPPINGMETHODS", "xasIFSService"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPPINGMETHODS", "xasIFSService", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPPINGMETHODS", "xasApplyLeastCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPPINGMETHODS", "xasApplyLeastCost", dropTriggers: true);
		}
	}
}
