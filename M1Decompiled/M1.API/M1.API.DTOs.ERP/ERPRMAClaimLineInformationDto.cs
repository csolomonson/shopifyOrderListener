using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRMAClaimLineInformationDto
{
	public string ralActionType { get; set; }

	public decimal ralConversionFactor { get; set; }

	public string ralCreatedBy { get; set; }

	public DateTime? ralCreatedDate { get; set; }

	public string ralCustomerPo { get; set; }

	public decimal ralDiscountPercent { get; set; }

	public Guid ralUniqueID { get; set; }

	public decimal ralExtendedCost { get; set; }

	public decimal ralExtendedCostForeign { get; set; }

	public decimal ralExtendedDiscountBase { get; set; }

	public decimal ralExtendedDiscountForeign { get; set; }

	public decimal ralExtendedPrice { get; set; }

	public decimal ralExtendedPriceForeign { get; set; }

	public decimal ralFullExtendedPriceBase { get; set; }

	public decimal ralFullExtendedPriceForeign { get; set; }

	public decimal ralFullUnitPriceBase { get; set; }

	public decimal ralFullUnitPriceForeign { get; set; }

	public bool ralCustomerToPayForShipping { get; set; }

	public bool ralInvoicedComplete { get; set; }

	public bool ralKitPart { get; set; }

	public bool ralReceivedComplete { get; set; }

	public bool ralRequiresInspection { get; set; }

	public bool ralReturnToSupplier { get; set; }

	public bool ralTransferredToSalesOrder { get; set; }

	public string ralOrgPartID { get; set; }

	public string ralOrgPartShortDescription { get; set; }

	public string ralPartBinID { get; set; }

	public string ralPartGroupID { get; set; }

	public string ralPartID { get; set; }

	public string ralPartLongDescriptionRtf { get; set; }

	public string ralPartLongDescriptionText { get; set; }

	public string ralPartRevisionID { get; set; }

	public string ralPartShortDescription { get; set; }

	public string ralPartWarehouseLocationID { get; set; }

	public string ralProjectAreaID { get; set; }

	public string ralProjectID { get; set; }

	public string ralPurchaseLocationID { get; set; }

	public decimal ralQuantity { get; set; }

	public decimal ralQuantityReceived { get; set; }

	public DateTime? ralReceivedDate { get; set; }

	public DateTime? ralRequiredDate { get; set; }

	public DateTime? ralReturnedDate { get; set; }

	public string ralReturnReasonID { get; set; }

	public string ralRmaClaimID { get; set; }

	public byte[] ralRowVersion { get; set; }

	public short ralSalesOrderDeliveryID { get; set; }

	public string ralSalesOrderID { get; set; }

	public short ralSalesOrderLineID { get; set; }

	public decimal ralSalesQuantity { get; set; }

	public string ralSalesUnitOfMeasure { get; set; }

	public short ralRmaClaimLineID { get; set; }

	public string ralShipmentID { get; set; }

	public short ralShipmentLineID { get; set; }

	public DateTime? ralShippedDate { get; set; }

	public string ralShippingMethodID { get; set; }

	public string ralShippingPaymentTypeID { get; set; }

	public string ralSupplierAuthorizationNumber { get; set; }

	public string ralSupplierOrganizationID { get; set; }

	public string ralSupplierShippingMethodID { get; set; }

	public string ralSupplierTrackingNumber { get; set; }

	public string ralTrackingNumber { get; set; }

	public decimal ralUnitCost { get; set; }

	public decimal ralUnitCostForeign { get; set; }

	public decimal ralUnitDiscountBase { get; set; }

	public decimal ralUnitDiscountForeign { get; set; }

	public string ralUnitOfMeasure { get; set; }

	public decimal ralUnitPrice { get; set; }

	public decimal ralUnitPriceForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
