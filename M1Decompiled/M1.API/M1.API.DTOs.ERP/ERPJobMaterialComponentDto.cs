using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPJobMaterialComponentDto
{
	[JsonProperty("jmtAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtAdditionalQuantity { get; set; }

	[JsonProperty("jmtCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string jmtCreatedBy { get; set; }

	[JsonProperty("jmtCreatedDate", Order = 3)]
	public DateTime? jmtCreatedDate { get; set; }

	[JsonProperty("jmtDescription", Order = 4)]
	[Required(ErrorMessage = "jmtDescription is required.")]
	[MaxLength(50)]
	public string jmtDescription { get; set; }

	[JsonProperty("jmtUniqueID", Order = 5)]
	public Guid jmtUniqueID { get; set; }

	[JsonProperty("jmtClosed", Order = 6)]
	public bool jmtClosed { get; set; }

	[JsonProperty("jmtPullAllFromStock", Order = 7)]
	public bool jmtPullAllFromStock { get; set; }

	[JsonProperty("jmtReceivedComplete", Order = 8)]
	public bool jmtReceivedComplete { get; set; }

	[JsonProperty("jmtJobAssemblyID", Order = 9)]
	public int jmtJobAssemblyID { get; set; }

	[JsonProperty("jmtJobID", Order = 10)]
	[Required(ErrorMessage = "jmtJobID is required.")]
	[MaxLength(20)]
	public string jmtJobID { get; set; }

	[JsonProperty("jmtJobMaterialID", Order = 11)]
	[Required(ErrorMessage = "jmtJobMaterialID is required.")]
	public int jmtJobMaterialID { get; set; }

	[JsonProperty("jmtMaterialQuantity", Order = 12)]
	[Required(ErrorMessage = "jmtMaterialQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtMaterialQuantity { get; set; }

	[JsonProperty("jmtParentQuantity", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtParentQuantity { get; set; }

	[JsonProperty("jmtPartBinID", Order = 14)]
	[Required(ErrorMessage = "jmtPartBinID is required.")]
	[MaxLength(15)]
	public string jmtPartBinID { get; set; }

	[JsonProperty("jmtPartID", Order = 15)]
	[Required(ErrorMessage = "jmtPartID is required.")]
	[MaxLength(30)]
	public string jmtPartID { get; set; }

	[JsonProperty("jmtPartRevisionID", Order = 16)]
	[MaxLength(15)]
	public string jmtPartRevisionID { get; set; }

	[JsonProperty("jmtPartWarehouseLocationID", Order = 17)]
	[Required(ErrorMessage = "jmtPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string jmtPartWarehouseLocationID { get; set; }

	[JsonProperty("jmtQuantityAllocated", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtQuantityAllocated { get; set; }

	[JsonProperty("jmtQuantityPerParent", Order = 19)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtQuantityPerParent { get; set; }

	[JsonProperty("jmtQuantityReceived", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtQuantityReceived { get; set; }

	[JsonProperty("jmtQuantityToInspect", Order = 21)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtQuantityToInspect { get; set; }

	[JsonProperty("jmtQuantityToReturn", Order = 22)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtQuantityToReturn { get; set; }

	[JsonProperty("jmtRowVersion", Order = 23)]
	public byte[] jmtRowVersion { get; set; }

	[JsonProperty("jmtScrapQuantityReceived", Order = 24)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtScrapQuantityReceived { get; set; }

	[JsonProperty("jmtJobMaterialComponentID", Order = 25)]
	[Required(ErrorMessage = "jmtJobMaterialComponentID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int jmtJobMaterialComponentID { get; set; }

	[JsonProperty("jmtUnitOfMeasure", Order = 26)]
	[MaxLength(2)]
	public string jmtUnitOfMeasure { get; set; }

	[JsonProperty("jmtWeight", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmtWeight { get; set; }

	[JsonProperty("customFields", Order = 28)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
