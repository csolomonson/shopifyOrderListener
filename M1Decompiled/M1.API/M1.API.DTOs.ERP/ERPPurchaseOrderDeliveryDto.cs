using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchaseOrderDeliveryDto
{
	[JsonProperty("pmdContactID", Order = 1)]
	[MaxLength(5)]
	public string pmdContactID { get; set; }

	[JsonProperty("pmdCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string pmdCreatedBy { get; set; }

	[JsonProperty("pmdCreatedDate", Order = 3)]
	public DateTime? pmdCreatedDate { get; set; }

	[JsonProperty("pmdDeliveryDate", Order = 4)]
	[Required(ErrorMessage = "pmdDeliveryDate is required.")]
	public DateTime? pmdDeliveryDate { get; set; }

	[JsonProperty("pmdDeliveryQuantity", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmdDeliveryQuantity { get; set; }

	[JsonProperty("pmdDeliveryType", Order = 6)]
	[Required(ErrorMessage = "pmdDeliveryType is required.")]
	public byte pmdDeliveryType { get; set; }

	[JsonProperty("pmdUniqueID", Order = 7)]
	public Guid pmdUniqueID { get; set; }

	[JsonProperty("pmdClosed", Order = 8)]
	public bool pmdClosed { get; set; }

	[JsonProperty("pmdInTransit", Order = 9)]
	public bool pmdInTransit { get; set; }

	[JsonProperty("pmdInvoicedComplete", Order = 10)]
	public bool pmdInvoicedComplete { get; set; }

	[JsonProperty("pmdReceivedComplete", Order = 11)]
	public bool pmdReceivedComplete { get; set; }

	[JsonProperty("pmdJobAssemblyID", Order = 12)]
	public int pmdJobAssemblyID { get; set; }

	[JsonProperty("pmdJobID", Order = 13)]
	[MaxLength(20)]
	public string pmdJobID { get; set; }

	[JsonProperty("pmdJobMaterialID", Order = 14)]
	public int pmdJobMaterialID { get; set; }

	[JsonProperty("pmdJobOperationID", Order = 15)]
	public int pmdJobOperationID { get; set; }

	[JsonProperty("pmdJobType", Order = 16)]
	public byte pmdJobType { get; set; }

	[JsonProperty("pmdLocationID", Order = 17)]
	[MaxLength(5)]
	public string pmdLocationID { get; set; }

	[JsonProperty("pmdOrganizationID", Order = 18)]
	[MaxLength(10)]
	public string pmdOrganizationID { get; set; }

	[JsonProperty("pmdPurchaseOrderID", Order = 19)]
	[Required(ErrorMessage = "pmdPurchaseOrderID is required.")]
	[MaxLength(10)]
	public string pmdPurchaseOrderID { get; set; }

	[JsonProperty("pmdPurchaseOrderLineID", Order = 20)]
	[Required(ErrorMessage = "pmdPurchaseOrderLineID is required.")]
	public short pmdPurchaseOrderLineID { get; set; }

	[JsonProperty("pmdQuantityInvoiced", Order = 21)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmdQuantityInvoiced { get; set; }

	[JsonProperty("pmdQuantityReceived", Order = 22)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmdQuantityReceived { get; set; }

	[JsonProperty("pmdRowVersion", Order = 23)]
	public byte[] pmdRowVersion { get; set; }

	[JsonProperty("pmdPurchaseOrderDeliveryID", Order = 24)]
	[Required(ErrorMessage = "pmdPurchaseOrderDeliveryID is required.")]
	public short pmdPurchaseOrderDeliveryID { get; set; }

	[JsonProperty("pmdShippingMethodID", Order = 25)]
	[MaxLength(5)]
	public string pmdShippingMethodID { get; set; }

	[JsonProperty("pmdTrackingNumber", Order = 26)]
	[MaxLength(30)]
	public string pmdTrackingNumber { get; set; }

	[JsonProperty("customFields", Order = 27)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
