using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchaseOrderMemoDto
{
	[JsonProperty("pmkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string pmkCreatedBy { get; set; }

	[JsonProperty("pmkCreatedDate", Order = 2)]
	public DateTime? pmkCreatedDate { get; set; }

	[JsonProperty("pmkUniqueID", Order = 3)]
	public Guid pmkUniqueID { get; set; }

	[JsonProperty("pmkClosed", Order = 4)]
	public bool pmkClosed { get; set; }

	[JsonProperty("pmkLongDescriptionRtf", Order = 5)]
	public string pmkLongDescriptionRtf { get; set; }

	[JsonProperty("pmkLongDescriptionText", Order = 6)]
	public string pmkLongDescriptionText { get; set; }

	[JsonProperty("pmkMemoDate", Order = 7)]
	[Required(ErrorMessage = "pmkMemoDate is required.")]
	public DateTime? pmkMemoDate { get; set; }

	[JsonProperty("pmkPurchaseOrderID", Order = 8)]
	[Required(ErrorMessage = "pmkPurchaseOrderID is required.")]
	[MaxLength(10)]
	public string pmkPurchaseOrderID { get; set; }

	[JsonProperty("pmkRowVersion", Order = 9)]
	public byte[] pmkRowVersion { get; set; }

	[JsonProperty("pmkPurchaseOrderMemoID", Order = 10)]
	[Required(ErrorMessage = "pmkPurchaseOrderMemoID is required.")]
	public short pmkPurchaseOrderMemoID { get; set; }

	[JsonProperty("pmkShortDescription", Order = 11)]
	[Required(ErrorMessage = "pmkShortDescription is required.")]
	[MaxLength(50)]
	public string pmkShortDescription { get; set; }

	[JsonProperty("pmkShowInApInvoices", Order = 12)]
	public bool pmkShowInApInvoices { get; set; }

	[JsonProperty("pmkShowInPurchaseOrders", Order = 13)]
	public bool pmkShowInPurchaseOrders { get; set; }

	[JsonProperty("pmkShowInReceipts", Order = 14)]
	public bool pmkShowInReceipts { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
