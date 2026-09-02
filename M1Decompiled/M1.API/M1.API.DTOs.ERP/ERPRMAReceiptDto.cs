using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRMAReceiptDto
{
	[JsonProperty("rrpArInvoiceContactID", Order = 1)]
	[MaxLength(5)]
	public string rrpArInvoiceContactID { get; set; }

	[JsonProperty("rrpArInvoiceLocationID", Order = 2)]
	[MaxLength(5)]
	public string rrpArInvoiceLocationID { get; set; }

	[JsonProperty("rrpClosedDate", Order = 3)]
	public DateTime? rrpClosedDate { get; set; }

	[JsonProperty("rrpRmaReceiptID", Order = 4)]
	[Required(ErrorMessage = "rrpRmaReceiptID is required.")]
	[MaxLength(10)]
	public string rrpRmaReceiptID { get; set; }

	[JsonProperty("rrpCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string rrpCreatedBy { get; set; }

	[JsonProperty("rrpCreatedDate", Order = 6)]
	public DateTime? rrpCreatedDate { get; set; }

	[JsonProperty("rrpCurrencyRateID", Order = 7)]
	[MaxLength(5)]
	public string rrpCurrencyRateID { get; set; }

	[JsonProperty("rrpCustomerOrganizationID", Order = 8)]
	[Required(ErrorMessage = "rrpCustomerOrganizationID is required.")]
	[MaxLength(10)]
	public string rrpCustomerOrganizationID { get; set; }

	[JsonProperty("rrpDeliveryDocket", Order = 9)]
	[MaxLength(20)]
	public string rrpDeliveryDocket { get; set; }

	[JsonProperty("rrpUniqueID", Order = 10)]
	public Guid rrpUniqueID { get; set; }

	[JsonProperty("rrpExchangeRate", Order = 11)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrpExchangeRate { get; set; }

	[JsonProperty("rrpFreightCharge", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrpFreightCharge { get; set; }

	[JsonProperty("rrpFreightChargeForeign", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrpFreightChargeForeign { get; set; }

	[JsonProperty("rrpClosed", Order = 14)]
	public bool rrpClosed { get; set; }

	[JsonProperty("rrpCustomRate", Order = 15)]
	public bool rrpCustomRate { get; set; }

	[JsonProperty("rrpPosted", Order = 16)]
	public bool rrpPosted { get; set; }

	[JsonProperty("rrpReversalEntry", Order = 17)]
	public bool rrpReversalEntry { get; set; }

	[JsonProperty("rrpReversed", Order = 18)]
	public bool rrpReversed { get; set; }

	[JsonProperty("rrpPlantDepartmentID", Order = 19)]
	[MaxLength(5)]
	public string rrpPlantDepartmentID { get; set; }

	[JsonProperty("rrpPlantID", Order = 20)]
	[MaxLength(5)]
	public string rrpPlantID { get; set; }

	[JsonProperty("rrpPostedDate", Order = 21)]
	public DateTime? rrpPostedDate { get; set; }

	[JsonProperty("rrpProjectID", Order = 22)]
	[MaxLength(10)]
	public string rrpProjectID { get; set; }

	[JsonProperty("rrpReceiptDate", Order = 23)]
	[Required(ErrorMessage = "rrpReceiptDate is required.")]
	public DateTime? rrpReceiptDate { get; set; }

	[JsonProperty("rrpRowVersion", Order = 24)]
	public byte[] rrpRowVersion { get; set; }

	[JsonProperty("rrpShipContactID", Order = 25)]
	[MaxLength(5)]
	public string rrpShipContactID { get; set; }

	[JsonProperty("rrpShipLocationID", Order = 26)]
	[MaxLength(5)]
	public string rrpShipLocationID { get; set; }

	[JsonProperty("rrpShipOrganizationID", Order = 27)]
	[Required(ErrorMessage = "rrpShipOrganizationID is required.")]
	[MaxLength(10)]
	public string rrpShipOrganizationID { get; set; }

	[JsonProperty("rrpShippingMethodID", Order = 28)]
	[MaxLength(5)]
	public string rrpShippingMethodID { get; set; }

	[JsonProperty("customFields", Order = 29)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
