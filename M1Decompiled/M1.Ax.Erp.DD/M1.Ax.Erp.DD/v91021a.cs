using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.021", "Update open quantity in various tables", "2016-03-02")]
public class v91021a
{
	public v91021a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentLines", "smlSODeliveryQuantity") && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentLines", "smlSOOpenQuantity"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ShipmentLines Set smlSODeliveryQuantity = omdDeliveryQuantity, smlSOOpenQuantity = Case When omdShippedComplete <> 0 Or(omdDeliveryQuantity - omdQuantityShipped) <= 0 Then 0 Else IsNull(omdDeliveryQuantity - omdQuantityShipped, 0.0) End From ShipmentLines Inner Join SalesOrderDeliveries on smlSalesOrderID = omdSalesOrderID and smlSalesOrderLineID = omdSalesOrderLineID and smlSalesOrderDeliveryID = omdSalesOrderDeliveryID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlPOPurchaseQuantity") && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlPOOpenQuantity"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptLines Set rmlPOPurchaseQuantity = pmlPurchaseQuantity,rmlPOOpenQuantity = CASE WHEN(pmlPurchaseQuantity <= pmlPurchaseQuantityReceived OR pmlReceivedComplete <> 0) THEN 0 ELSE IsNull(pmlPurchaseQuantity - pmlPurchaseQuantityReceived, 0.0) END From ReceiptLines Inner Join PurchaseOrderLines on rmlPurchaseOrderID = pmlPurchaseOrderID and rmlPurchaseOrderLineID = pmlPurchaseOrderLineID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslDMRClaimQuantity") && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipmentLines", "dslDMROpenQuantity"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DMRShipmentLines Set dslDMRClaimQuantity = dmlQuantity, dslDMROpenQuantity = Case When dmlShippedComplete <> 0 Or(dmlQuantity - dmlQuantityShipped) <= 0 Then 0 Else IsNull(dmlQuantity - dmlQuantityShipped, 0.0) End From DMRShipmentLines Inner Join DMRClaimLines on dslDMRClaimID = dmlDMRClaimID and dslDMRClaimLineID = dmlDMRClaimLineID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptLines", "rrlRMAClaimQuantity") && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptLines", "rrlRMAOpenQuantity"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAReceiptLines Set rrlRMAClaimQuantity = ralQuantity,rrlRMAOpenQuantity = CASE WHEN(ralQuantity <= ralQuantityReceived OR ralReceivedComplete <> 0) THEN 0 ELSE IsNull(ralQuantity - ralQuantityReceived, 0.0) END From RMAReceiptLines Inner Join RMAClaimLines on rrlRMAClaimID = ralRMAClaimID and rrlRMAClaimLineID = ralRMAClaimLineID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceiptLines", "wrlWTShippedQuantity") && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceiptLines", "wrlWTOpenQuantity"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update WarehouseReceiptLines Set wrlWTShippedQuantity = mwlShipQuantity,wrlWTOpenQuantity = CASE WHEN(mwlShipQuantity <= mwlReceivedQuantity OR mwlReceivedComplete <> 0) THEN 0 ELSE IsNull(mwlShipQuantity - mwlReceivedQuantity, 0.0) END From WarehouseReceiptLines Inner Join WarehouseTransferLines on wrlWarehouseTransferID = mwlWarehouseTransferID and wrlWarehouseTransferLineID = mwlWarehouseTransferLineID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlWRRequestedQuantity") && parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlWROpenQuantity"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update WarehouseTransferLines Set mwlWRRequestedQuantity = wqlRequestedQuantity,mwlWROpenQuantity = CASE WHEN(wqlRequestedQuantity <= wqlQuantityTransferred OR wqlTransferredComplete <> 0) THEN 0 ELSE IsNull(wqlRequestedQuantity - wqlQuantityTransferred, 0.0) END From WarehouseTransferLines Inner Join WarehouseRequisitionLines on wqlWarehouseRequisitionID = mwlWarehouseRequisitionID and wqlWarehouseRequisitionLineID = mwlWarehouseRequisitionLineID");
		}
	}
}
