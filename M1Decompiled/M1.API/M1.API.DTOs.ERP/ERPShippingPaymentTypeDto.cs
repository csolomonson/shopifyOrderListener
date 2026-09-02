using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShippingPaymentTypeDto
{
	[JsonProperty("xayShippingPaymentTypeID", Order = 1)]
	[Required(ErrorMessage = "xayShippingPaymentTypeID is required.")]
	[MaxLength(5)]
	public string xayShippingPaymentTypeID { get; set; }

	[JsonProperty("xayCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xayCreatedBy { get; set; }

	[JsonProperty("xayCreatedDate", Order = 3)]
	public DateTime? xayCreatedDate { get; set; }

	[JsonProperty("xayDescription", Order = 4)]
	[Required(ErrorMessage = "xayDescription is required.")]
	[MaxLength(50)]
	public string xayDescription { get; set; }

	[JsonProperty("xayUniqueID", Order = 5)]
	public Guid xayUniqueID { get; set; }

	[JsonProperty("xayInactiveDate", Order = 6)]
	public DateTime? xayInactiveDate { get; set; }

	[JsonProperty("xayInactive", Order = 7)]
	public bool xayInactive { get; set; }

	[JsonProperty("xayDoNotXferShipCostsToAr", Order = 8)]
	public bool xayDoNotXferShipCostsToAr { get; set; }

	[JsonProperty("xayRowVersion", Order = 9)]
	public byte[] xayRowVersion { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
