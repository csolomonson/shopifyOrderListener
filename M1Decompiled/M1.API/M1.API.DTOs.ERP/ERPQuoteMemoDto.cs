using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPQuoteMemoDto
{
	[JsonProperty("qmkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string qmkCreatedBy { get; set; }

	[JsonProperty("qmkCreatedDate", Order = 2)]
	public DateTime? qmkCreatedDate { get; set; }

	[JsonProperty("qmkUniqueID", Order = 3)]
	public Guid qmkUniqueID { get; set; }

	[JsonProperty("qmkClosed", Order = 4)]
	public bool qmkClosed { get; set; }

	[JsonProperty("qmkLongDescriptionRtf", Order = 5)]
	public string qmkLongDescriptionRtf { get; set; }

	[JsonProperty("qmkLongDescriptionText", Order = 6)]
	public string qmkLongDescriptionText { get; set; }

	[JsonProperty("qmkMemoDate", Order = 7)]
	[Required(ErrorMessage = "qmkMemoDate is required.")]
	public DateTime? qmkMemoDate { get; set; }

	[JsonProperty("qmkQuoteID", Order = 8)]
	[Required(ErrorMessage = "qmkQuoteID is required.")]
	[MaxLength(10)]
	public string qmkQuoteID { get; set; }

	[JsonProperty("qmkRowVersion", Order = 9)]
	public byte[] qmkRowVersion { get; set; }

	[JsonProperty("qmkQuoteMemoID", Order = 10)]
	[Required(ErrorMessage = "qmkQuoteMemoID is required.")]
	public short qmkQuoteMemoID { get; set; }

	[JsonProperty("qmkShortDescription", Order = 11)]
	[Required(ErrorMessage = "qmkShortDescription is required.")]
	[MaxLength(50)]
	public string qmkShortDescription { get; set; }

	[JsonProperty("qmkShowInQuotes", Order = 12)]
	public bool qmkShowInQuotes { get; set; }

	[JsonProperty("qmkShowInSalesOrders", Order = 13)]
	public bool qmkShowInSalesOrders { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
