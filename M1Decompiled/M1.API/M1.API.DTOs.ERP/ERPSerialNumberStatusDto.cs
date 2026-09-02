using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSerialNumberStatusDto
{
	[JsonProperty("snsCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string snsCreatedBy { get; set; }

	[JsonProperty("snsCreatedDate", Order = 2)]
	public DateTime? snsCreatedDate { get; set; }

	[JsonProperty("snsUniqueID", Order = 3)]
	public Guid snsUniqueID { get; set; }

	[JsonProperty("snsPartBinID", Order = 4)]
	[Required(ErrorMessage = "snsPartBinID is required.")]
	[MaxLength(15)]
	public string snsPartBinID { get; set; }

	[JsonProperty("snsPartID", Order = 5)]
	[Required(ErrorMessage = "snsPartID is required.")]
	[MaxLength(30)]
	public string snsPartID { get; set; }

	[JsonProperty("snsPartRevisionID", Order = 6)]
	[MaxLength(15)]
	public string snsPartRevisionID { get; set; }

	[JsonProperty("snsPartWarehouseLocationID", Order = 7)]
	[Required(ErrorMessage = "snsPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string snsPartWarehouseLocationID { get; set; }

	[JsonProperty("snsQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal snsQuantity { get; set; }

	[JsonProperty("snsRowVersion", Order = 9)]
	public byte[] snsRowVersion { get; set; }

	[JsonProperty("snsSerialNumberID", Order = 10)]
	[Required(ErrorMessage = "snsSerialNumberID is required.")]
	[MaxLength(30)]
	public string snsSerialNumberID { get; set; }

	[JsonProperty("snsStatus", Order = 11)]
	public byte snsStatus { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
