using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseReceiptDto
{
	[JsonProperty("wrpClosedDate", Order = 1)]
	public DateTime? wrpClosedDate { get; set; }

	[JsonProperty("wrpWarehouseReceiptID", Order = 2)]
	[Required(ErrorMessage = "wrpWarehouseReceiptID is required.")]
	[MaxLength(10)]
	public string wrpWarehouseReceiptID { get; set; }

	[JsonProperty("wrpCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string wrpCreatedBy { get; set; }

	[JsonProperty("wrpCreatedDate", Order = 4)]
	public DateTime? wrpCreatedDate { get; set; }

	[JsonProperty("wrpDestinationWarehouseID", Order = 5)]
	[Required(ErrorMessage = "wrpDestinationWarehouseID is required.")]
	[MaxLength(5)]
	public string wrpDestinationWarehouseID { get; set; }

	[JsonProperty("wrpUniqueID", Order = 6)]
	public Guid wrpUniqueID { get; set; }

	[JsonProperty("wrpFreightCharge", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal wrpFreightCharge { get; set; }

	[JsonProperty("wrpClosed", Order = 8)]
	public bool wrpClosed { get; set; }

	[JsonProperty("wrpPosted", Order = 9)]
	public bool wrpPosted { get; set; }

	[JsonProperty("wrpReversalEntry", Order = 10)]
	public bool wrpReversalEntry { get; set; }

	[JsonProperty("wrpReversed", Order = 11)]
	public bool wrpReversed { get; set; }

	[JsonProperty("wrpPostedDate", Order = 12)]
	public DateTime? wrpPostedDate { get; set; }

	[JsonProperty("wrpReceiptDate", Order = 13)]
	[Required(ErrorMessage = "wrpReceiptDate is required.")]
	public DateTime? wrpReceiptDate { get; set; }

	[JsonProperty("wrpRowVersion", Order = 14)]
	public byte[] wrpRowVersion { get; set; }

	[JsonProperty("wrpShippingMethodID", Order = 15)]
	[MaxLength(5)]
	public string wrpShippingMethodID { get; set; }

	[JsonProperty("wrpShippingPaymentTypeID", Order = 16)]
	[MaxLength(5)]
	public string wrpShippingPaymentTypeID { get; set; }

	[JsonProperty("wrpSourceWarehouseID", Order = 17)]
	[Required(ErrorMessage = "wrpSourceWarehouseID is required.")]
	[MaxLength(5)]
	public string wrpSourceWarehouseID { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
