using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.017", "Add fields to SalesOrderDeliveries table", "2015-02-10")]
public class v900017c
{
	public v900017c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderDeliveries", "omdKitPart"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderDeliveries", "omdKitPart", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderDeliveries Set omdKitPart = 1 From SalesOrderDeliveries Inner Join Parts On omdPartID = impPartID Where impPhantomOrKitPart <> 0");
	}
}
