using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchasePlannerLineDto
{
	[JsonProperty("pplCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string pplCreatedBy { get; set; }

	[JsonProperty("pplCreatedDate", Order = 2)]
	public DateTime? pplCreatedDate { get; set; }

	[JsonProperty("pplDataMissing", Order = 3)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int pplDataMissing { get; set; }

	[JsonProperty("pplUniqueID", Order = 4)]
	public Guid pplUniqueID { get; set; }

	[JsonProperty("pplExtendedCostBase", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pplExtendedCostBase { get; set; }

	[JsonProperty("pplCompleted", Order = 6)]
	public bool pplCompleted { get; set; }

	[JsonProperty("pplNonStockedItem", Order = 7)]
	public bool pplNonStockedItem { get; set; }

	[JsonProperty("pplPhantomOrKitPart", Order = 8)]
	public bool pplPhantomOrKitPart { get; set; }

	[JsonProperty("pplLastRunDate", Order = 9)]
	public DateTime? pplLastRunDate { get; set; }

	[JsonProperty("pplLineID", Order = 10)]
	[Required(ErrorMessage = "pplLineID is required.")]
	public int pplLineID { get; set; }

	[JsonProperty("pplLotSize", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pplLotSize { get; set; }

	[JsonProperty("pplMaximumQuantity", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pplMaximumQuantity { get; set; }

	[JsonProperty("pplMinimumQuantity", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pplMinimumQuantity { get; set; }

	[JsonProperty("pplPartID", Order = 14)]
	[MaxLength(30)]
	public string pplPartID { get; set; }

	[JsonProperty("pplPartRevisionID", Order = 15)]
	[MaxLength(15)]
	public string pplPartRevisionID { get; set; }

	[JsonProperty("pplPartShortDescription", Order = 16)]
	[MaxLength(50)]
	public string pplPartShortDescription { get; set; }

	[JsonProperty("pplPlantID", Order = 17)]
	[MaxLength(5)]
	public string pplPlantID { get; set; }

	[JsonProperty("pplQuantityOnHand", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pplQuantityOnHand { get; set; }

	[JsonProperty("pplReorderMethod", Order = 19)]
	public byte pplReorderMethod { get; set; }

	[JsonProperty("pplRowVersion", Order = 20)]
	public byte[] pplRowVersion { get; set; }

	[JsonProperty("pplSessionID", Order = 21)]
	[Required(ErrorMessage = "pplSessionID is required.")]
	[MaxLength(10)]
	public string pplSessionID { get; set; }

	[JsonProperty("pplWarehouseID", Order = 22)]
	[MaxLength(5)]
	public string pplWarehouseID { get; set; }

	[JsonProperty("customFields", Order = 23)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
