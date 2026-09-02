using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.823", "Update Kit Part flag in SalesOrderDeliveries", "2019-06-07")]
public class v92823a
{
	public v92823a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderDeliveries Set omdKitPart = 1 From SalesOrderDeliveries Inner Join Parts On omdPartID = impPartID Where impPhantomOrKitPart <> 0");
	}
}
