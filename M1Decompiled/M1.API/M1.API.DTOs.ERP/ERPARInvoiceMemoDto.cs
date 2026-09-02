using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPARInvoiceMemoDto
{
	[JsonProperty("ariArInvoiceID", Order = 1)]
	[Required(ErrorMessage = "ariArInvoiceID is required.")]
	[MaxLength(10)]
	public string ariArInvoiceID { get; set; }

	[JsonProperty("ariCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string ariCreatedBy { get; set; }

	[JsonProperty("ariCreatedDate", Order = 3)]
	public DateTime? ariCreatedDate { get; set; }

	[JsonProperty("ariUniqueID", Order = 4)]
	public Guid ariUniqueID { get; set; }

	[JsonProperty("ariLongDescriptionRtf", Order = 5)]
	public string ariLongDescriptionRtf { get; set; }

	[JsonProperty("ariLongDescriptionText", Order = 6)]
	public string ariLongDescriptionText { get; set; }

	[JsonProperty("ariMemoDate", Order = 7)]
	[Required(ErrorMessage = "ariMemoDate is required.")]
	public DateTime? ariMemoDate { get; set; }

	[JsonProperty("ariRowVersion", Order = 8)]
	public byte[] ariRowVersion { get; set; }

	[JsonProperty("ariArInvoiceMemoID", Order = 9)]
	[Required(ErrorMessage = "ariArInvoiceMemoID is required.")]
	public short ariArInvoiceMemoID { get; set; }

	[JsonProperty("ariShortDescription", Order = 10)]
	[Required(ErrorMessage = "ariShortDescription is required.")]
	[MaxLength(50)]
	public string ariShortDescription { get; set; }

	[JsonProperty("ariShowInArInvoices", Order = 11)]
	public bool ariShowInArInvoices { get; set; }

	[JsonProperty("ariShowInArPayments", Order = 12)]
	public bool ariShowInArPayments { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
