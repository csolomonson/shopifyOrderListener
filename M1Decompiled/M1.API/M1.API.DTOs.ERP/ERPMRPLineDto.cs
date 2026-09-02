using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMRPLineDto
{
	[JsonProperty("mrlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string mrlCreatedBy { get; set; }

	[JsonProperty("mrlCreatedDate", Order = 2)]
	public DateTime? mrlCreatedDate { get; set; }

	[JsonProperty("mrlUniqueID", Order = 3)]
	public Guid mrlUniqueID { get; set; }

	[JsonProperty("mrlForecastDemand", Order = 4)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrlForecastDemand { get; set; }

	[JsonProperty("mrlInvQtyInProduction", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrlInvQtyInProduction { get; set; }

	[JsonProperty("mrlCompleted", Order = 6)]
	public bool mrlCompleted { get; set; }

	[JsonProperty("mrlDataMissing", Order = 7)]
	public bool mrlDataMissing { get; set; }

	[JsonProperty("mrlLineID", Order = 8)]
	[Required(ErrorMessage = "mrlLineID is required.")]
	public int mrlLineID { get; set; }

	[JsonProperty("mrlMaximumQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrlMaximumQuantity { get; set; }

	[JsonProperty("mrlMfgLotSize", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrlMfgLotSize { get; set; }

	[JsonProperty("mrlMinimumQuantity", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrlMinimumQuantity { get; set; }

	[JsonProperty("mrlPartID", Order = 12)]
	[Required(ErrorMessage = "mrlPartID is required.")]
	[MaxLength(30)]
	public string mrlPartID { get; set; }

	[JsonProperty("mrlPartRevisionID", Order = 13)]
	[MaxLength(15)]
	public string mrlPartRevisionID { get; set; }

	[JsonProperty("mrlPartShortDescription", Order = 14)]
	[MaxLength(50)]
	public string mrlPartShortDescription { get; set; }

	[JsonProperty("mrlPlantIDs", Order = 15)]
	[MaxLength(100)]
	public string mrlPlantIDs { get; set; }

	[JsonProperty("mrlQuantityAllocated", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrlQuantityAllocated { get; set; }

	[JsonProperty("mrlQuantityOnHand", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrlQuantityOnHand { get; set; }

	[JsonProperty("mrlQuantityToInspect", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mrlQuantityToInspect { get; set; }

	[JsonProperty("mrlRowVersion", Order = 19)]
	public byte[] mrlRowVersion { get; set; }

	[JsonProperty("mrlSessionID", Order = 20)]
	[Required(ErrorMessage = "mrlSessionID is required.")]
	[MaxLength(10)]
	public string mrlSessionID { get; set; }

	[JsonProperty("mrlWarehouseIDs", Order = 21)]
	[MaxLength(100)]
	public string mrlWarehouseIDs { get; set; }

	[JsonProperty("customFields", Order = 22)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
