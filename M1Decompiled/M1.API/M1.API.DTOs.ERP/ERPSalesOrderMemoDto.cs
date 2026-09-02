using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderMemoDto
{
	[JsonProperty("omkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string omkCreatedBy { get; set; }

	[JsonProperty("omkCreatedDate", Order = 2)]
	public DateTime? omkCreatedDate { get; set; }

	[JsonProperty("omkUniqueID", Order = 3)]
	public Guid omkUniqueID { get; set; }

	[JsonProperty("omkClosed", Order = 4)]
	public bool omkClosed { get; set; }

	[JsonProperty("omkLongDescriptionRtf", Order = 5)]
	public string omkLongDescriptionRtf { get; set; }

	[JsonProperty("omkLongDescriptionText", Order = 6)]
	public string omkLongDescriptionText { get; set; }

	[JsonProperty("omkMemoDate", Order = 7)]
	[Required(ErrorMessage = "omkMemoDate is required.")]
	public DateTime? omkMemoDate { get; set; }

	[JsonProperty("omkRowVersion", Order = 8)]
	public byte[] omkRowVersion { get; set; }

	[JsonProperty("omkSalesOrderID", Order = 9)]
	[Required(ErrorMessage = "omkSalesOrderID is required.")]
	[MaxLength(10)]
	public string omkSalesOrderID { get; set; }

	[JsonProperty("omkSalesOrderMemoID", Order = 10)]
	[Required(ErrorMessage = "omkSalesOrderMemoID is required.")]
	public short omkSalesOrderMemoID { get; set; }

	[JsonProperty("omkShortDescription", Order = 11)]
	[Required(ErrorMessage = "omkShortDescription is required.")]
	[MaxLength(50)]
	public string omkShortDescription { get; set; }

	[JsonProperty("omkShowInArInvoices", Order = 12)]
	public bool omkShowInArInvoices { get; set; }

	[JsonProperty("omkShowInSalesOrders", Order = 13)]
	public bool omkShowInSalesOrders { get; set; }

	[JsonProperty("omkShowInShipments", Order = 14)]
	public bool omkShowInShipments { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
