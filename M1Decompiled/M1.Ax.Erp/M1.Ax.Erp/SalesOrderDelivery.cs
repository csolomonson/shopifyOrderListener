using System.Data;
using System.Data.SqlClient;
using M1.Ax.Erp.Models;
using M1.Core;

namespace M1.Ax.Erp;

public static class SalesOrderDelivery
{
	private static SqlCommand _insertCommand;

	public static void CreateSod(M1Database database, SalesOrderDeliveryFields listFields)
	{
		_insertCommand = database.NewSqlCommand("Insert Into SalesOrderDeliveries (omdSalesOrderID,omdSalesOrderLineID,omdSalesOrderDeliveryID,omdPartID,omdPartRevisionID,omdPartWarehouseLocationID,omdPartBinID\r\n                                                            ,omdDeliveryQuantity,omdDeliveryDate,omdDeliveryType,omdFirm,omdAmountToInvoice,omdAmountToInvoiceForeign,omdDifferentLocation\r\n                                                            ,omdCustomerOrganizationID,omdShipLocationID,omdShipContactID,omdShippingMethodID,omdShippingPaymentTypeID,omdFreightAmountBase\r\n                                                            ,omdFreightAmountForeign,omdQuantityShipped,omdQuantityInvoiced,omdShippedComplete,omdInvoicedComplete,omdClosed,omdRequiresInspection\r\n                                                            ,omdPurchaseUnitCostBase,omdPickInProgress,omdPurchaseUnitCostForeign,omdSupplierOrganizationID,omdPurchaseLocationID,omdQuantityReceived\r\n                                                            ,omdReceivedComplete,omdAvalaraNonTaxReasonID,omdCreatedBy,omdCreatedDate,omdUniqueID,omdKitPart,omdQuantityAllocated,omdQuantityOnOrder\r\n                                                            ,omdWeight,omdExtendedWeight) \r\n                                                    VALUES (@OrderID,@OrderLineID,@OrderDeliveryID,@PartID,@PartRevisionID,@PartWH,@PartBin,@DeliveryQty,@DeliveryDate,@DeliveryType,@Firm,@AmountToInvoice\r\n                                                            ,@AmountToInvoiceForeign,@DifferentLocation,@CustomerorganizationID,@ShipLocationID,@ShipContactID,@ShippingMethodID,@ShippingPaymentTypeID\r\n                                                            ,@FreightAmountBase,@FreightAmountForeign,@QuantityShipped,@QuantityInvoiced,@ShippedComplete,@InvoicedComplete,@Closed,@RequiresInspection\r\n                                                            ,@PurchaseUnitCostBase,@PickInProgress,@PurchaseUnitCostForeign,@SupplierOrganizationID,@PurchaseLocationID,@QuantityReceived,@ReceivedComplete\r\n                                                            ,@AvalaraNonTaxReasonID,@CreatedBy,@CreatedDate,@UniqueID,@KitPart,@QuantityAllocated,@QuantityOnOrder,@Weight,@ExtendedWeight)");
		_insertCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = listFields.SalesOrderId;
		_insertCommand.Parameters.Add(new SqlParameter("@OrderLineID", SqlDbType.SmallInt)).Value = listFields.SalesOrderLineId;
		_insertCommand.Parameters.Add(new SqlParameter("@OrderDeliveryID", SqlDbType.SmallInt)).Value = listFields.SalesOrderDeliveryId;
		_insertCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = listFields.PartId;
		_insertCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = listFields.PartRevisionId;
		_insertCommand.Parameters.Add(new SqlParameter("@PartWH", SqlDbType.NVarChar)).Value = listFields.PartWarehouseLocationId;
		_insertCommand.Parameters.Add(new SqlParameter("@PartBin", SqlDbType.NVarChar)).Value = listFields.PartBinId;
		_insertCommand.Parameters.Add(new SqlParameter("@DeliveryQty", SqlDbType.Decimal)).Value = listFields.DeliveryQty;
		_insertCommand.Parameters.Add(new SqlParameter("@DeliveryDate", SqlDbType.DateTime)).Value = listFields.DeliveryDate;
		_insertCommand.Parameters.Add(new SqlParameter("@DeliveryType", SqlDbType.TinyInt)).Value = listFields.DeliveryType;
		_insertCommand.Parameters.Add(new SqlParameter("@Firm", SqlDbType.Bit)).Value = listFields.Firm;
		_insertCommand.Parameters.Add(new SqlParameter("@AmountToInvoice", SqlDbType.Money)).Value = listFields.AmountToInvoice;
		_insertCommand.Parameters.Add(new SqlParameter("@AmountToInvoiceForeign", SqlDbType.Money)).Value = listFields.AmountToInvoiceForeign;
		_insertCommand.Parameters.Add(new SqlParameter("@DifferentLocation", SqlDbType.Bit)).Value = listFields.DifferentLocation;
		_insertCommand.Parameters.Add(new SqlParameter("@CustomerorganizationID", SqlDbType.NVarChar)).Value = listFields.CustomerOrganizationId;
		_insertCommand.Parameters.Add(new SqlParameter("@ShipLocationID", SqlDbType.NVarChar)).Value = listFields.ShipLocationId;
		_insertCommand.Parameters.Add(new SqlParameter("@ShipContactID", SqlDbType.NVarChar)).Value = listFields.ShipContactId;
		_insertCommand.Parameters.Add(new SqlParameter("@ShippingMethodID", SqlDbType.NVarChar)).Value = listFields.ShippingMethodId;
		_insertCommand.Parameters.Add(new SqlParameter("@ShippingPaymentTypeID", SqlDbType.NVarChar)).Value = listFields.ShippingPaymentTypeId;
		_insertCommand.Parameters.Add(new SqlParameter("@FreightAmountBase", SqlDbType.Money)).Value = listFields.FreightAmountBase;
		_insertCommand.Parameters.Add(new SqlParameter("@FreightAmountForeign", SqlDbType.Money)).Value = listFields.FreightAmountForeign;
		_insertCommand.Parameters.Add(new SqlParameter("@QuantityShipped", SqlDbType.Decimal)).Value = listFields.QuantityShipped;
		_insertCommand.Parameters.Add(new SqlParameter("@QuantityInvoiced", SqlDbType.Decimal)).Value = listFields.QuantityInvoiced;
		_insertCommand.Parameters.Add(new SqlParameter("@ShippedComplete", SqlDbType.Bit)).Value = listFields.ShippedComplete;
		_insertCommand.Parameters.Add(new SqlParameter("@InvoicedComplete", SqlDbType.Bit)).Value = listFields.InvoicedComplete;
		_insertCommand.Parameters.Add(new SqlParameter("@Closed", SqlDbType.Bit)).Value = listFields.Closed;
		_insertCommand.Parameters.Add(new SqlParameter("@RequiresInspection", SqlDbType.Bit)).Value = listFields.RequiresInspection;
		_insertCommand.Parameters.Add(new SqlParameter("@PurchaseUnitCostBase", SqlDbType.Decimal)).Value = listFields.PurchaseUnitCostBase;
		_insertCommand.Parameters.Add(new SqlParameter("@PickInProgress", SqlDbType.Bit)).Value = listFields.PickInProgress;
		_insertCommand.Parameters.Add(new SqlParameter("@PurchaseUnitCostForeign", SqlDbType.Decimal)).Value = listFields.PurchaseUnitCostForeign;
		_insertCommand.Parameters.Add(new SqlParameter("@SupplierOrganizationID", SqlDbType.NVarChar)).Value = listFields.SupplierOrganizationID;
		_insertCommand.Parameters.Add(new SqlParameter("@PurchaseLocationID", SqlDbType.NVarChar)).Value = listFields.PurchaseLocationID;
		_insertCommand.Parameters.Add(new SqlParameter("@QuantityReceived", SqlDbType.Decimal)).Value = listFields.QuantityReceived;
		_insertCommand.Parameters.Add(new SqlParameter("@ReceivedComplete", SqlDbType.Bit)).Value = listFields.ReceivedComplete;
		_insertCommand.Parameters.Add(new SqlParameter("@AvalaraNonTaxReasonID", SqlDbType.NVarChar)).Value = listFields.AvalaraNonTaxReasonID;
		_insertCommand.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar)).Value = listFields.CreatedBy;
		_insertCommand.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = listFields.CreatedDate;
		_insertCommand.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = listFields.UniqueId;
		_insertCommand.Parameters.Add(new SqlParameter("@KitPart", SqlDbType.Bit)).Value = listFields.KitPart;
		_insertCommand.Parameters.Add(new SqlParameter("@QuantityAllocated", SqlDbType.Decimal)).Value = listFields.QuantityAllocated;
		_insertCommand.Parameters.Add(new SqlParameter("@QuantityOnOrder", SqlDbType.Decimal)).Value = listFields.QuantityOnOrder;
		_insertCommand.Parameters.Add(new SqlParameter("@Weight", SqlDbType.Decimal)).Value = listFields.Weight;
		_insertCommand.Parameters.Add(new SqlParameter("@ExtendedWeight", SqlDbType.Decimal)).Value = listFields.ExtendedWeight;
		database.ExecuteCommand(_insertCommand);
	}
}
