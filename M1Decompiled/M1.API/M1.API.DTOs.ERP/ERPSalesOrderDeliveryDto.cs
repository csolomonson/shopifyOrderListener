using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderDeliveryDto
{
	[JsonProperty("omdAmountToInvoice", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdAmountToInvoice { get; set; }

	[JsonProperty("omdAmountToInvoiceForeign", Order = 2)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdAmountToInvoiceForeign { get; set; }

	[JsonProperty("omdAvalaraNonTaxReasonID", Order = 3)]
	[MaxLength(5)]
	public string omdAvalaraNonTaxReasonID { get; set; }

	[JsonProperty("omdCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string omdCreatedBy { get; set; }

	[JsonProperty("omdCreatedDate", Order = 5)]
	public DateTime? omdCreatedDate { get; set; }

	[JsonProperty("omdCustomerOrganizationID", Order = 6)]
	[MaxLength(10)]
	public string omdCustomerOrganizationID { get; set; }

	[JsonProperty("omdDeliveryDate", Order = 7)]
	[Required(ErrorMessage = "omdDeliveryDate is required.")]
	public DateTime? omdDeliveryDate { get; set; }

	[JsonProperty("omdDeliveryQuantity", Order = 8)]
	[Required(ErrorMessage = "omdDeliveryQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdDeliveryQuantity { get; set; }

	[JsonProperty("omdDeliveryType", Order = 9)]
	[Required(ErrorMessage = "omdDeliveryType is required.")]
	public byte omdDeliveryType { get; set; }

	[JsonProperty("omdUniqueID", Order = 10)]
	public Guid omdUniqueID { get; set; }

	[JsonProperty("omdExtendedWeight", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdExtendedWeight { get; set; }

	[JsonProperty("omdFreightAmountBase", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdFreightAmountBase { get; set; }

	[JsonProperty("omdFreightAmountForeign", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdFreightAmountForeign { get; set; }

	[JsonProperty("omdClosed", Order = 14)]
	public bool omdClosed { get; set; }

	[JsonProperty("omdDifferentLocation", Order = 15)]
	public bool omdDifferentLocation { get; set; }

	[JsonProperty("omdFirm", Order = 16)]
	public bool omdFirm { get; set; }

	[JsonProperty("omdInvoicedComplete", Order = 17)]
	public bool omdInvoicedComplete { get; set; }

	[JsonProperty("omdKitPart", Order = 18)]
	public bool omdKitPart { get; set; }

	[JsonProperty("omdPickInProgress", Order = 19)]
	public bool omdPickInProgress { get; set; }

	[JsonProperty("omdReceivedComplete", Order = 20)]
	public bool omdReceivedComplete { get; set; }

	[JsonProperty("omdRequiresInspection", Order = 21)]
	public bool omdRequiresInspection { get; set; }

	[JsonProperty("omdShippedComplete", Order = 22)]
	public bool omdShippedComplete { get; set; }

	[JsonProperty("omdPartBinID", Order = 23)]
	[Required(ErrorMessage = "omdPartBinID is required.")]
	[MaxLength(15)]
	public string omdPartBinID { get; set; }

	[JsonProperty("omdPartID", Order = 24)]
	[Required(ErrorMessage = "omdPartID is required.")]
	[MaxLength(30)]
	public string omdPartID { get; set; }

	[JsonProperty("omdPartRevisionID", Order = 25)]
	[MaxLength(15)]
	public string omdPartRevisionID { get; set; }

	[JsonProperty("omdPartWarehouseLocationID", Order = 26)]
	[Required(ErrorMessage = "omdPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string omdPartWarehouseLocationID { get; set; }

	[JsonProperty("omdPurchaseLocationID", Order = 27)]
	[MaxLength(5)]
	public string omdPurchaseLocationID { get; set; }

	[JsonProperty("omdPurchaseUnitCostBase", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdPurchaseUnitCostBase { get; set; }

	[JsonProperty("omdPurchaseUnitCostForeign", Order = 29)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdPurchaseUnitCostForeign { get; set; }

	[JsonProperty("omdQuantityAllocated", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdQuantityAllocated { get; set; }

	[JsonProperty("omdQuantityInvoiced", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdQuantityInvoiced { get; set; }

	[JsonProperty("omdQuantityOnOrder", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdQuantityOnOrder { get; set; }

	[JsonProperty("omdQuantityReceived", Order = 33)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdQuantityReceived { get; set; }

	[JsonProperty("omdQuantityShipped", Order = 34)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdQuantityShipped { get; set; }

	[JsonProperty("omdRowVersion", Order = 35)]
	public byte[] omdRowVersion { get; set; }

	[JsonProperty("omdSalesOrderID", Order = 36)]
	[Required(ErrorMessage = "omdSalesOrderID is required.")]
	[MaxLength(10)]
	public string omdSalesOrderID { get; set; }

	[JsonProperty("omdSalesOrderLineID", Order = 37)]
	[Required(ErrorMessage = "omdSalesOrderLineID is required.")]
	public short omdSalesOrderLineID { get; set; }

	[JsonProperty("omdSalesOrderDeliveryID", Order = 38)]
	[Required(ErrorMessage = "omdSalesOrderDeliveryID is required.")]
	public short omdSalesOrderDeliveryID { get; set; }

	[JsonProperty("omdShipContactID", Order = 39)]
	[MaxLength(5)]
	public string omdShipContactID { get; set; }

	[JsonProperty("omdShipLocationID", Order = 40)]
	[MaxLength(5)]
	public string omdShipLocationID { get; set; }

	[JsonProperty("omdShippingMethodID", Order = 41)]
	[MaxLength(5)]
	public string omdShippingMethodID { get; set; }

	[JsonProperty("omdShippingPaymentTypeID", Order = 42)]
	[MaxLength(5)]
	public string omdShippingPaymentTypeID { get; set; }

	[JsonProperty("omdSupplierOrganizationID", Order = 43)]
	[MaxLength(10)]
	public string omdSupplierOrganizationID { get; set; }

	[JsonProperty("omdWeight", Order = 44)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omdWeight { get; set; }

	[JsonProperty("customFields", Order = 45)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
