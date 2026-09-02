using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.101", "Refresh the allocated quantities from job material, job material component, sales order delivery, sales order component", "2021-08-04")]
public class v94101d
{
	public v94101d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterialComponents", "jmtPullAllFromStock"))
		{
			Part part = new Part();
			M1Database database = parms.Database;
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE JobMaterials Set jmmQuantityAllocated = CASE WHEN jmmPullAllFromStock = 0 THEN 0 ELSE CASE WHEN jmmEstimatedQuantity-jmmQuantityReceived <= 0 OR jmmReceivedComplete <> 0 THEN 0 ELSE jmmEstimatedQuantity-jmmQuantityReceived END END;");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE JobMaterialComponents SET jmtQuantityAllocated = CASE WHEN jmtPullAllFromStock = 0 THEN 0 ELSE CASE WHEN jmtMaterialQuantity-jmtQuantityReceived <= 0 OR jmtReceivedComplete <> 0 THEN 0 ELSE jmtMaterialQuantity-jmtQuantityReceived END END;");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE SalesOrderDeliveries SET omdQuantityAllocated = CASE WHEN omdDeliveryQuantity-omdQuantityShipped <= 0 OR omdDeliveryType<> 2 OR omdShippedComplete <> 0 THEN 0 ELSE omdDeliveryQuantity-omdQuantityShipped END;");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE SalesOrderComponents SET omoQuantityAllocated = CASE WHEN omoDeliveryQuantity-omoQuantityShipped <= 0 OR omdDeliveryType <> 3 OR omoShippedComplete <> 0 THEN 0 ELSE omoDeliveryQuantity - omoQuantityShipped END FROM SalesOrderComponents INNER JOIN SalesOrderDeliveries ON omdSalesOrderID = omoSalesOrderID AND omdSalesOrderLineID = omoSalesOrderLineID AND omdSalesOrderDeliveryID = omoSalesOrderDeliveryID;");
			part.RefreshPartAllocations(database, null);
		}
	}
}
