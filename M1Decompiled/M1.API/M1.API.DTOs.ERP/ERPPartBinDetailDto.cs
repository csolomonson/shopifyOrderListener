using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartBinDetailDto
{
	[JsonProperty("imgCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string imgCreatedBy { get; set; }

	[JsonProperty("imgCreatedDate", Order = 2)]
	public DateTime? imgCreatedDate { get; set; }

	[JsonProperty("imgUniqueID", Order = 3)]
	public Guid imgUniqueID { get; set; }

	[JsonProperty("imgOriginalQuantity", Order = 4)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imgOriginalQuantity { get; set; }

	[JsonProperty("imgPartBinID", Order = 5)]
	[Required(ErrorMessage = "imgPartBinID is required.")]
	[MaxLength(15)]
	public string imgPartBinID { get; set; }

	[JsonProperty("imgPartID", Order = 6)]
	[Required(ErrorMessage = "imgPartID is required.")]
	[MaxLength(30)]
	public string imgPartID { get; set; }

	[JsonProperty("imgPartRevisionID", Order = 7)]
	[MaxLength(15)]
	public string imgPartRevisionID { get; set; }

	[JsonProperty("imgQuantityType", Order = 8)]
	public byte imgQuantityType { get; set; }

	[JsonProperty("imgRemainingQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imgRemainingQuantity { get; set; }

	[JsonProperty("imgRowVersion", Order = 10)]
	public byte[] imgRowVersion { get; set; }

	[JsonProperty("imgPartBinDetailID", Order = 11)]
	[Required(ErrorMessage = "imgPartBinDetailID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imgPartBinDetailID { get; set; }

	[JsonProperty("imgSourceTableName", Order = 12)]
	[MaxLength(30)]
	public string imgSourceTableName { get; set; }

	[JsonProperty("imgSourceTableUniqueID", Order = 13)]
	public Guid imgSourceTableUniqueID { get; set; }

	[JsonProperty("imgTransactionDate", Order = 14)]
	[Required(ErrorMessage = "imgTransactionDate is required.")]
	public DateTime? imgTransactionDate { get; set; }

	[JsonProperty("imgUnitDutyCost", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imgUnitDutyCost { get; set; }

	[JsonProperty("imgUnitFreightCost", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imgUnitFreightCost { get; set; }

	[JsonProperty("imgUnitLaborCost", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imgUnitLaborCost { get; set; }

	[JsonProperty("imgUnitMaterialCost", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imgUnitMaterialCost { get; set; }

	[JsonProperty("imgUnitMiscCost", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imgUnitMiscCost { get; set; }

	[JsonProperty("imgUnitOverheadCost", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imgUnitOverheadCost { get; set; }

	[JsonProperty("imgUnitSubcontractCost", Order = 21)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imgUnitSubcontractCost { get; set; }

	[JsonProperty("imgWarehouseID", Order = 22)]
	[Required(ErrorMessage = "imgWarehouseID is required.")]
	[MaxLength(5)]
	public string imgWarehouseID { get; set; }

	[JsonProperty("customFields", Order = 23)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
