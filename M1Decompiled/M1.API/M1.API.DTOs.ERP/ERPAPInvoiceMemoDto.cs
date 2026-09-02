using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAPInvoiceMemoDto
{
	[JsonProperty("apiApInvoiceID", Order = 1)]
	[Required(ErrorMessage = "apiApInvoiceID is required.")]
	[MaxLength(10)]
	public string apiApInvoiceID { get; set; }

	[JsonProperty("apiCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string apiCreatedBy { get; set; }

	[JsonProperty("apiCreatedDate", Order = 3)]
	public DateTime? apiCreatedDate { get; set; }

	[JsonProperty("apiUniqueID", Order = 4)]
	public Guid apiUniqueID { get; set; }

	[JsonProperty("apiLongDescriptionRtf", Order = 5)]
	public string apiLongDescriptionRtf { get; set; }

	[JsonProperty("apiLongDescriptionText", Order = 6)]
	public string apiLongDescriptionText { get; set; }

	[JsonProperty("apiMemoDate", Order = 7)]
	[Required(ErrorMessage = "apiMemoDate is required.")]
	public DateTime? apiMemoDate { get; set; }

	[JsonProperty("apiRowVersion", Order = 8)]
	public byte[] apiRowVersion { get; set; }

	[JsonProperty("apiApInvoiceMemoID", Order = 9)]
	[Required(ErrorMessage = "apiApInvoiceMemoID is required.")]
	public short apiApInvoiceMemoID { get; set; }

	[JsonProperty("apiShortDescription", Order = 10)]
	[Required(ErrorMessage = "apiShortDescription is required.")]
	[MaxLength(50)]
	public string apiShortDescription { get; set; }

	[JsonProperty("apiShowInApInvoices", Order = 11)]
	public bool apiShowInApInvoices { get; set; }

	[JsonProperty("apiShowInApPayments", Order = 12)]
	public bool apiShowInApPayments { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
