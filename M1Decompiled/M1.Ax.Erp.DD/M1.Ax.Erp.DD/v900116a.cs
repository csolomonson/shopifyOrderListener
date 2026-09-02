using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.116", "Add fields to SalesOrderDeliveries table", "2015-12-15")]
public class v900116a
{
	public v900116a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderDeliveries", "omdWeight"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderDeliveries", "omdWeight", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderDeliveries Set omdWeight = omlWeight From SalesOrderLines Inner Join SalesOrderDeliveries On OMLSALESORDERID = OMDSALESORDERID And OMLSALESORDERLINEID = OMDSALESORDERLINEID");
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderDeliveries", "omdExtendedWeight"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderDeliveries", "omdExtendedWeight", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderDeliveries Set omdExtendedWeight = omdWeight*omdDeliveryQuantity");
	}
}
