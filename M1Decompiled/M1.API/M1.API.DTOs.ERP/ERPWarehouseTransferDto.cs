using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseTransferDto
{
	[JsonProperty("mwpClosedDate", Order = 1)]
	public DateTime? mwpClosedDate { get; set; }

	[JsonProperty("mwpWarehouseTransferID", Order = 2)]
	[Required(ErrorMessage = "mwpWarehouseTransferID is required.")]
	[MaxLength(10)]
	public string mwpWarehouseTransferID { get; set; }

	[JsonProperty("mwpCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string mwpCreatedBy { get; set; }

	[JsonProperty("mwpCreatedDate", Order = 4)]
	public DateTime? mwpCreatedDate { get; set; }

	[JsonProperty("mwpDestinationWarehouseID", Order = 5)]
	[Required(ErrorMessage = "mwpDestinationWarehouseID is required.")]
	[MaxLength(5)]
	public string mwpDestinationWarehouseID { get; set; }

	[JsonProperty("mwpUniqueID", Order = 6)]
	public Guid mwpUniqueID { get; set; }

	[JsonProperty("mwpFreightCharge", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mwpFreightCharge { get; set; }

	[JsonProperty("mwpClosed", Order = 8)]
	public bool mwpClosed { get; set; }

	[JsonProperty("mwpPosted", Order = 9)]
	public bool mwpPosted { get; set; }

	[JsonProperty("mwpPrintLabels", Order = 10)]
	public bool mwpPrintLabels { get; set; }

	[JsonProperty("mwpPrintPacker", Order = 11)]
	public bool mwpPrintPacker { get; set; }

	[JsonProperty("mwpReversalEntry", Order = 12)]
	public bool mwpReversalEntry { get; set; }

	[JsonProperty("mwpReversed", Order = 13)]
	public bool mwpReversed { get; set; }

	[JsonProperty("mwpNumberOfLabels", Order = 14)]
	public short mwpNumberOfLabels { get; set; }

	[JsonProperty("mwpPostedDate", Order = 15)]
	public DateTime? mwpPostedDate { get; set; }

	[JsonProperty("mwpRowVersion", Order = 16)]
	public byte[] mwpRowVersion { get; set; }

	[JsonProperty("mwpShipDate", Order = 17)]
	[Required(ErrorMessage = "mwpShipDate is required.")]
	public DateTime? mwpShipDate { get; set; }

	[JsonProperty("mwpShippingCommentsRTF", Order = 18)]
	[MaxLength(50)]
	public string mwpShippingCommentsRTF { get; set; }

	[JsonProperty("mwpShippingCommentsText", Order = 19)]
	[MaxLength(50)]
	public string mwpShippingCommentsText { get; set; }

	[JsonProperty("mwpShippingMethodID", Order = 20)]
	[MaxLength(5)]
	public string mwpShippingMethodID { get; set; }

	[JsonProperty("mwpShippingPaymentTypeID", Order = 21)]
	[MaxLength(5)]
	public string mwpShippingPaymentTypeID { get; set; }

	[JsonProperty("mwpSourceWarehouseID", Order = 22)]
	[Required(ErrorMessage = "mwpSourceWarehouseID is required.")]
	[MaxLength(5)]
	public string mwpSourceWarehouseID { get; set; }

	[JsonProperty("mwpTrackingNumber", Order = 23)]
	[MaxLength(30)]
	public string mwpTrackingNumber { get; set; }

	[JsonProperty("customFields", Order = 24)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
