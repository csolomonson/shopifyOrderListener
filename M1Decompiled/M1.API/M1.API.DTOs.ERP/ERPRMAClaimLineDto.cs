using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRMAClaimLineDto
{
	[JsonProperty("ralActionType", Order = 1)]
	[MaxLength(5)]
	public string ralActionType { get; set; }

	[JsonProperty("ralConversionFactor", Order = 2)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralConversionFactor { get; set; }

	[JsonProperty("ralCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string ralCreatedBy { get; set; }

	[JsonProperty("ralCreatedDate", Order = 4)]
	public DateTime? ralCreatedDate { get; set; }

	[JsonProperty("ralCustomerPo", Order = 5)]
	[MaxLength(40)]
	public string ralCustomerPo { get; set; }

	[JsonProperty("ralDiscountPercent", Order = 6)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralDiscountPercent { get; set; }

	[JsonProperty("ralUniqueID", Order = 7)]
	public Guid ralUniqueID { get; set; }

	[JsonProperty("ralExtendedCost", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralExtendedCost { get; set; }

	[JsonProperty("ralExtendedCostForeign", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralExtendedCostForeign { get; set; }

	[JsonProperty("ralExtendedDiscountBase", Order = 10)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralExtendedDiscountBase { get; set; }

	[JsonProperty("ralExtendedDiscountForeign", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralExtendedDiscountForeign { get; set; }

	[JsonProperty("ralExtendedPrice", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralExtendedPrice { get; set; }

	[JsonProperty("ralExtendedPriceForeign", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralExtendedPriceForeign { get; set; }

	[JsonProperty("ralFullExtendedPriceBase", Order = 14)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralFullExtendedPriceBase { get; set; }

	[JsonProperty("ralFullExtendedPriceForeign", Order = 15)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralFullExtendedPriceForeign { get; set; }

	[JsonProperty("ralFullUnitPriceBase", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralFullUnitPriceBase { get; set; }

	[JsonProperty("ralFullUnitPriceForeign", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralFullUnitPriceForeign { get; set; }

	[JsonProperty("ralCustomerToPayForShipping", Order = 18)]
	public bool ralCustomerToPayForShipping { get; set; }

	[JsonProperty("ralInvoicedComplete", Order = 19)]
	public bool ralInvoicedComplete { get; set; }

	[JsonProperty("ralKitPart", Order = 20)]
	public bool ralKitPart { get; set; }

	[JsonProperty("ralReceivedComplete", Order = 21)]
	public bool ralReceivedComplete { get; set; }

	[JsonProperty("ralRequiresInspection", Order = 22)]
	public bool ralRequiresInspection { get; set; }

	[JsonProperty("ralReturnToSupplier", Order = 23)]
	public bool ralReturnToSupplier { get; set; }

	[JsonProperty("ralTransferredToSalesOrder", Order = 24)]
	public bool ralTransferredToSalesOrder { get; set; }

	[JsonProperty("ralOrgPartID", Order = 25)]
	[MaxLength(30)]
	public string ralOrgPartID { get; set; }

	[JsonProperty("ralOrgPartShortDescription", Order = 26)]
	[MaxLength(50)]
	public string ralOrgPartShortDescription { get; set; }

	[JsonProperty("ralPartBinID", Order = 27)]
	[Required(ErrorMessage = "ralPartBinID is required.")]
	[MaxLength(15)]
	public string ralPartBinID { get; set; }

	[JsonProperty("ralPartGroupID", Order = 28)]
	[MaxLength(5)]
	public string ralPartGroupID { get; set; }

	[JsonProperty("ralPartID", Order = 29)]
	[Required(ErrorMessage = "ralPartID is required.")]
	[MaxLength(30)]
	public string ralPartID { get; set; }

	[JsonProperty("ralPartLongDescriptionRtf", Order = 30)]
	public string ralPartLongDescriptionRtf { get; set; }

	[JsonProperty("ralPartLongDescriptionText", Order = 31)]
	public string ralPartLongDescriptionText { get; set; }

	[JsonProperty("ralPartRevisionID", Order = 32)]
	[MaxLength(15)]
	public string ralPartRevisionID { get; set; }

	[JsonProperty("ralPartShortDescription", Order = 33)]
	[Required(ErrorMessage = "ralPartShortDescription is required.")]
	[MaxLength(50)]
	public string ralPartShortDescription { get; set; }

	[JsonProperty("ralPartWarehouseLocationID", Order = 34)]
	[Required(ErrorMessage = "ralPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string ralPartWarehouseLocationID { get; set; }

	[JsonProperty("ralProjectAreaID", Order = 35)]
	[MaxLength(15)]
	public string ralProjectAreaID { get; set; }

	[JsonProperty("ralProjectID", Order = 36)]
	[MaxLength(10)]
	public string ralProjectID { get; set; }

	[JsonProperty("ralPurchaseLocationID", Order = 37)]
	[MaxLength(5)]
	public string ralPurchaseLocationID { get; set; }

	[JsonProperty("ralQuantity", Order = 38)]
	[Required(ErrorMessage = "ralQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralQuantity { get; set; }

	[JsonProperty("ralQuantityReceived", Order = 39)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralQuantityReceived { get; set; }

	[JsonProperty("ralReceivedDate", Order = 40)]
	public DateTime? ralReceivedDate { get; set; }

	[JsonProperty("ralRequiredDate", Order = 41)]
	public DateTime? ralRequiredDate { get; set; }

	[JsonProperty("ralReturnedDate", Order = 42)]
	public DateTime? ralReturnedDate { get; set; }

	[JsonProperty("ralReturnReasonID", Order = 43)]
	[MaxLength(5)]
	public string ralReturnReasonID { get; set; }

	[JsonProperty("ralRmaClaimID", Order = 44)]
	[Required(ErrorMessage = "ralRmaClaimID is required.")]
	[MaxLength(10)]
	public string ralRmaClaimID { get; set; }

	[JsonProperty("ralRowVersion", Order = 45)]
	public byte[] ralRowVersion { get; set; }

	[JsonProperty("ralSalesOrderDeliveryID", Order = 46)]
	public short ralSalesOrderDeliveryID { get; set; }

	[JsonProperty("ralSalesOrderID", Order = 47)]
	[MaxLength(10)]
	public string ralSalesOrderID { get; set; }

	[JsonProperty("ralSalesOrderLineID", Order = 48)]
	public short ralSalesOrderLineID { get; set; }

	[JsonProperty("ralSalesQuantity", Order = 49)]
	[Required(ErrorMessage = "ralSalesQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralSalesQuantity { get; set; }

	[JsonProperty("ralSalesUnitOfMeasure", Order = 50)]
	[MaxLength(2)]
	public string ralSalesUnitOfMeasure { get; set; }

	[JsonProperty("ralRmaClaimLineID", Order = 51)]
	[Required(ErrorMessage = "ralRmaClaimLineID is required.")]
	public short ralRmaClaimLineID { get; set; }

	[JsonProperty("ralShipmentID", Order = 52)]
	[MaxLength(10)]
	public string ralShipmentID { get; set; }

	[JsonProperty("ralShipmentLineID", Order = 53)]
	public short ralShipmentLineID { get; set; }

	[JsonProperty("ralShippedDate", Order = 54)]
	public DateTime? ralShippedDate { get; set; }

	[JsonProperty("ralShippingMethodID", Order = 55)]
	[MaxLength(5)]
	public string ralShippingMethodID { get; set; }

	[JsonProperty("ralShippingPaymentTypeID", Order = 56)]
	[MaxLength(5)]
	public string ralShippingPaymentTypeID { get; set; }

	[JsonProperty("ralSupplierAuthorizationNumber", Order = 57)]
	[MaxLength(20)]
	public string ralSupplierAuthorizationNumber { get; set; }

	[JsonProperty("ralSupplierOrganizationID", Order = 58)]
	[MaxLength(10)]
	public string ralSupplierOrganizationID { get; set; }

	[JsonProperty("ralSupplierShippingMethodID", Order = 59)]
	[MaxLength(5)]
	public string ralSupplierShippingMethodID { get; set; }

	[JsonProperty("ralSupplierTrackingNumber", Order = 60)]
	[MaxLength(30)]
	public string ralSupplierTrackingNumber { get; set; }

	[JsonProperty("ralTrackingNumber", Order = 61)]
	[MaxLength(30)]
	public string ralTrackingNumber { get; set; }

	[JsonProperty("ralUnitCost", Order = 62)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralUnitCost { get; set; }

	[JsonProperty("ralUnitCostForeign", Order = 63)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralUnitCostForeign { get; set; }

	[JsonProperty("ralUnitDiscountBase", Order = 64)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralUnitDiscountBase { get; set; }

	[JsonProperty("ralUnitDiscountForeign", Order = 65)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralUnitDiscountForeign { get; set; }

	[JsonProperty("ralUnitOfMeasure", Order = 66)]
	[MaxLength(2)]
	public string ralUnitOfMeasure { get; set; }

	[JsonProperty("ralUnitPrice", Order = 67)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralUnitPrice { get; set; }

	[JsonProperty("ralUnitPriceForeign", Order = 68)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ralUnitPriceForeign { get; set; }

	[JsonProperty("customFields", Order = 69)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
