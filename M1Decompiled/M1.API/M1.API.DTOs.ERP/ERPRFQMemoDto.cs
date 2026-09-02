using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRFQMemoDto
{
	[JsonProperty("rqkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string rqkCreatedBy { get; set; }

	[JsonProperty("rqkCreatedDate", Order = 2)]
	public DateTime? rqkCreatedDate { get; set; }

	[JsonProperty("rqkUniqueID", Order = 3)]
	public Guid rqkUniqueID { get; set; }

	[JsonProperty("rqkClosed", Order = 4)]
	public bool rqkClosed { get; set; }

	[JsonProperty("rqkLongDescriptionRtf", Order = 5)]
	public string rqkLongDescriptionRtf { get; set; }

	[JsonProperty("rqkLongDescriptionText", Order = 6)]
	public string rqkLongDescriptionText { get; set; }

	[JsonProperty("rqkMemoDate", Order = 7)]
	[Required(ErrorMessage = "rqkMemoDate is required.")]
	public DateTime? rqkMemoDate { get; set; }

	[JsonProperty("rqkRfqID", Order = 8)]
	[Required(ErrorMessage = "rqkRfqID is required.")]
	[MaxLength(10)]
	public string rqkRfqID { get; set; }

	[JsonProperty("rqkRowVersion", Order = 9)]
	public byte[] rqkRowVersion { get; set; }

	[JsonProperty("rqkRfqMemoID", Order = 10)]
	[Required(ErrorMessage = "rqkRfqMemoID is required.")]
	public short rqkRfqMemoID { get; set; }

	[JsonProperty("rqkShortDescription", Order = 11)]
	[Required(ErrorMessage = "rqkShortDescription is required.")]
	[MaxLength(50)]
	public string rqkShortDescription { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
