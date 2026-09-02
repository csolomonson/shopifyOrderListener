using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWarehouseBinDto
{
	[JsonProperty("inbWarehouseBinID", Order = 1)]
	[Required(ErrorMessage = "inbWarehouseBinID is required.")]
	[MaxLength(15)]
	public string inbWarehouseBinID { get; set; }

	[JsonProperty("inbCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string inbCreatedBy { get; set; }

	[JsonProperty("inbCreatedDate", Order = 3)]
	public DateTime? inbCreatedDate { get; set; }

	[JsonProperty("inbDescription", Order = 4)]
	[Required(ErrorMessage = "inbDescription is required.")]
	[MaxLength(50)]
	public string inbDescription { get; set; }

	[JsonProperty("inbUniqueID", Order = 5)]
	public Guid inbUniqueID { get; set; }

	[JsonProperty("inbInactiveDate", Order = 6)]
	public DateTime? inbInactiveDate { get; set; }

	[JsonProperty("inbInactive", Order = 7)]
	public bool inbInactive { get; set; }

	[JsonProperty("inbDefaultBin", Order = 8)]
	public bool inbDefaultBin { get; set; }

	[JsonProperty("inbHasQOHQTI", Order = 9)]
	public bool inbHasQOHQTI { get; set; }

	[JsonProperty("inbLongDescriptionRtf", Order = 10)]
	public string inbLongDescriptionRtf { get; set; }

	[JsonProperty("inbLongDescriptionText", Order = 11)]
	public string inbLongDescriptionText { get; set; }

	[JsonProperty("inbRowVersion", Order = 12)]
	public byte[] inbRowVersion { get; set; }

	[JsonProperty("inbWarehouseID", Order = 13)]
	[Required(ErrorMessage = "inbWarehouseID is required.")]
	[MaxLength(5)]
	public string inbWarehouseID { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
