using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.075", "Add missing fields to various tables", "2015-08-17")]
public class v900075a
{
	public v900075a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterialComponents", "jmtQuantityAllocated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterialComponents", "jmtQuantityAllocated", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterialComponents", "jmtQuantityAllocated"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobMaterialComponents Set jmtQuantityAllocated = CASE WHEN jmtMaterialQuantity-jmtQuantityReceived <= 0 OR jmtReceivedComplete <> 0 THEN 0 ELSE jmtMaterialQuantity-jmtQuantityReceived END");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injReverseIssue"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injReverseIssue", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoJobReceivedComplete"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoJobReceivedComplete", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderComponents", "omoQuantityAllocated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderComponents", "omoQuantityAllocated", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderComponents", "omoQuantityAllocated"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderComponents Set omoQuantityAllocated = CASE WHEN omoDeliveryQuantity-omoQuantityShipped <= 0 OR omoShippedComplete <> 0 Or omdDeliveryType = 3 THEN 0 ELSE omoDeliveryQuantity-omoQuantityShipped END From SalesOrderComponents Inner Join SalesOrderDeliveries on omoSalesOrderID = omdSalesOrderID and omoSalesOrderLineID = omdSalesOrderLineID and omoSalesOrderDeliveryID = omdSalesOrderDeliveryID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderDeliveries", "omdQuantityAllocated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderDeliveries", "omdQuantityAllocated", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderDeliveries", "omdQuantityAllocated"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderDeliveries Set omdQuantityAllocated = CASE WHEN omdDeliveryQuantity-omdQuantityShipped <= 0 OR omdShippedComplete <> 0 Or (omdDeliveryType <> 3 And omdDeliveryDate <> 5) THEN 0 ELSE omdDeliveryQuantity-omdQuantityShipped END");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderDeliveries", "omdQuantityOnOrder"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderDeliveries", "omdQuantityOnOrder", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderDeliveries", "omdQuantityOnOrder"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderDeliveries Set omdQuantityOnOrder = CASE WHEN omdDeliveryQuantity-omdQuantityShipped <= 0 OR omdShippedComplete <> 0 Or omdDeliveryType = 3 THEN 0 ELSE omdDeliveryQuantity-omdQuantityShipped END");
		}
	}
}
