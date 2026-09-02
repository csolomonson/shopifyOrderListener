using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartBinDto
{
	[JsonProperty("imbBinQuantityOnHand", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imbBinQuantityOnHand { get; set; }

	[JsonProperty("imbPartBinID", Order = 2)]
	[Required(ErrorMessage = "imbPartBinID is required.")]
	[MaxLength(15)]
	public string imbPartBinID { get; set; }

	[JsonProperty("imbConversionFactor", Order = 3)]
	[Required(ErrorMessage = "imbConversionFactor is required.")]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imbConversionFactor { get; set; }

	[JsonProperty("imbCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string imbCreatedBy { get; set; }

	[JsonProperty("imbCreatedDate", Order = 5)]
	public DateTime? imbCreatedDate { get; set; }

	[JsonProperty("imbDescription", Order = 6)]
	[MaxLength(50)]
	public string imbDescription { get; set; }

	[JsonProperty("imbUniqueID", Order = 7)]
	public Guid imbUniqueID { get; set; }

	[JsonProperty("imbInactiveBinDate", Order = 8)]
	public DateTime? imbInactiveBinDate { get; set; }

	[JsonProperty("imbInactiveBin", Order = 9)]
	public bool imbInactiveBin { get; set; }

	[JsonProperty("imbDefaultBin", Order = 10)]
	public bool imbDefaultBin { get; set; }

	[JsonProperty("imbPartID", Order = 11)]
	[Required(ErrorMessage = "imbPartID is required.")]
	[MaxLength(30)]
	public string imbPartID { get; set; }

	[JsonProperty("imbPartRevisionID", Order = 12)]
	[MaxLength(15)]
	public string imbPartRevisionID { get; set; }

	[JsonProperty("imbQuantityAllocated", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imbQuantityAllocated { get; set; }

	[JsonProperty("imbQuantityOnHand", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imbQuantityOnHand { get; set; }

	[JsonProperty("imbQuantityOnOrderPurchases", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imbQuantityOnOrderPurchases { get; set; }

	[JsonProperty("imbQuantityOnOrderSales", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imbQuantityOnOrderSales { get; set; }

	[JsonProperty("imbQuantityToInspect", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imbQuantityToInspect { get; set; }

	[JsonProperty("imbQuantityToReturn", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imbQuantityToReturn { get; set; }

	[JsonProperty("imbQuantityToReturnJob", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imbQuantityToReturnJob { get; set; }

	[JsonProperty("imbRowVersion", Order = 20)]
	public byte[] imbRowVersion { get; set; }

	[JsonProperty("imbWarehouseID", Order = 21)]
	[Required(ErrorMessage = "imbWarehouseID is required.")]
	[MaxLength(5)]
	public string imbWarehouseID { get; set; }

	[JsonProperty("customFields", Order = 22)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
