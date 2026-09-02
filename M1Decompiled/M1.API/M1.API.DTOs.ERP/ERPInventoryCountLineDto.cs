using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPInventoryCountLineDto
{
	[JsonProperty("imqBinDescription", Order = 1)]
	[MaxLength(50)]
	public string imqBinDescription { get; set; }

	[JsonProperty("imqCountedBy", Order = 2)]
	[MaxLength(3)]
	public string imqCountedBy { get; set; }

	[JsonProperty("imqCountedDate", Order = 3)]
	public DateTime? imqCountedDate { get; set; }

	[JsonProperty("imqCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string imqCreatedBy { get; set; }

	[JsonProperty("imqCreatedDate", Order = 5)]
	public DateTime? imqCreatedDate { get; set; }

	[JsonProperty("imqUniqueID", Order = 6)]
	public Guid imqUniqueID { get; set; }

	[JsonProperty("imqFinalCount", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imqFinalCount { get; set; }

	[JsonProperty("imqInventoryCountID", Order = 8)]
	[Required(ErrorMessage = "imqInventoryCountID is required.")]
	public int imqInventoryCountID { get; set; }

	[JsonProperty("imqPartBinID", Order = 9)]
	[Required(ErrorMessage = "imqPartBinID is required.")]
	[MaxLength(15)]
	public string imqPartBinID { get; set; }

	[JsonProperty("imqPartClassID", Order = 10)]
	[MaxLength(5)]
	public string imqPartClassID { get; set; }

	[JsonProperty("imqPartID", Order = 11)]
	[Required(ErrorMessage = "imqPartID is required.")]
	[MaxLength(30)]
	public string imqPartID { get; set; }

	[JsonProperty("imqPartRevisionID", Order = 12)]
	[MaxLength(15)]
	public string imqPartRevisionID { get; set; }

	[JsonProperty("imqPartShortDescription", Order = 13)]
	[MaxLength(50)]
	public string imqPartShortDescription { get; set; }

	[JsonProperty("imqPartWarehouseLocationID", Order = 14)]
	[Required(ErrorMessage = "imqPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string imqPartWarehouseLocationID { get; set; }

	[JsonProperty("imqQuantityOnHand", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imqQuantityOnHand { get; set; }

	[JsonProperty("imqRowVersion", Order = 16)]
	public byte[] imqRowVersion { get; set; }

	[JsonProperty("imqInventoryCountLineID", Order = 17)]
	[Required(ErrorMessage = "imqInventoryCountLineID is required.")]
	[Range(0, 9999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imqInventoryCountLineID { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
