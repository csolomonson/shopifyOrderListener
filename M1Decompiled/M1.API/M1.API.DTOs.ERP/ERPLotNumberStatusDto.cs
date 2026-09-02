using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLotNumberStatusDto
{
	[JsonProperty("absCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string absCreatedBy { get; set; }

	[JsonProperty("absCreatedDate", Order = 2)]
	public DateTime? absCreatedDate { get; set; }

	[JsonProperty("absUniqueID", Order = 3)]
	public Guid absUniqueID { get; set; }

	[JsonProperty("absLotNumberID", Order = 4)]
	[Required(ErrorMessage = "absLotNumberID is required.")]
	[MaxLength(30)]
	public string absLotNumberID { get; set; }

	[JsonProperty("absPartBinID", Order = 5)]
	[MaxLength(15)]
	public string absPartBinID { get; set; }

	[JsonProperty("absPartID", Order = 6)]
	[MaxLength(30)]
	public string absPartID { get; set; }

	[JsonProperty("absPartRevisionID", Order = 7)]
	[MaxLength(15)]
	public string absPartRevisionID { get; set; }

	[JsonProperty("absPartWarehouseLocationID", Order = 8)]
	[MaxLength(5)]
	public string absPartWarehouseLocationID { get; set; }

	[JsonProperty("absQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal absQuantity { get; set; }

	[JsonProperty("absRowVersion", Order = 10)]
	public byte[] absRowVersion { get; set; }

	[JsonProperty("absStatus", Order = 11)]
	public byte absStatus { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
