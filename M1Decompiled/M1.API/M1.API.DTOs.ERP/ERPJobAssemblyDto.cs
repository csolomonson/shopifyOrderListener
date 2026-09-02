using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPJobAssemblyDto
{
	[JsonProperty("jmaAssemblyOverlap", Order = 1)]
	public byte jmaAssemblyOverlap { get; set; }

	[JsonProperty("jmaCompletedDate", Order = 2)]
	public DateTime? jmaCompletedDate { get; set; }

	[JsonProperty("jmaCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string jmaCreatedBy { get; set; }

	[JsonProperty("jmaCreatedDate", Order = 4)]
	public DateTime? jmaCreatedDate { get; set; }

	[JsonProperty("jmaDocuments", Order = 5)]
	[MaxLength(50)]
	public string jmaDocuments { get; set; }

	[JsonProperty("jmaUniqueID", Order = 6)]
	public Guid jmaUniqueID { get; set; }

	[JsonProperty("jmaEstimatedUnitCost", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaEstimatedUnitCost { get; set; }

	[JsonProperty("jmaInventoryQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaInventoryQuantity { get; set; }

	[JsonProperty("jmaClosed", Order = 9)]
	public bool jmaClosed { get; set; }

	[JsonProperty("jmaIssuedComplete", Order = 10)]
	public bool jmaIssuedComplete { get; set; }

	[JsonProperty("jmaProductionComplete", Order = 11)]
	public bool jmaProductionComplete { get; set; }

	[JsonProperty("jmaPullAllFromStock", Order = 12)]
	public bool jmaPullAllFromStock { get; set; }

	[JsonProperty("jmaReceivedComplete", Order = 13)]
	public bool jmaReceivedComplete { get; set; }

	[JsonProperty("jmaJobID", Order = 14)]
	[Required(ErrorMessage = "jmaJobID is required.")]
	[MaxLength(20)]
	public string jmaJobID { get; set; }

	[JsonProperty("jmaLevel", Order = 15)]
	[Required(ErrorMessage = "jmaLevel is required.")]
	public short jmaLevel { get; set; }

	[JsonProperty("jmaOrderQuantity", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaOrderQuantity { get; set; }

	[JsonProperty("jmaOverlapDestinationLink", Order = 17)]
	public byte jmaOverlapDestinationLink { get; set; }

	[JsonProperty("jmaOverlapOffsetTime", Order = 18)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaOverlapOffsetTime { get; set; }

	[JsonProperty("jmaOverlapOperationID", Order = 19)]
	public int jmaOverlapOperationID { get; set; }

	[JsonProperty("jmaOverlapSourceLink", Order = 20)]
	public byte jmaOverlapSourceLink { get; set; }

	[JsonProperty("jmaOverlapSourceOperationID", Order = 21)]
	public int jmaOverlapSourceOperationID { get; set; }

	[JsonProperty("jmaOverlapType", Order = 22)]
	public byte jmaOverlapType { get; set; }

	[JsonProperty("jmaParentAssemblyID", Order = 23)]
	public int jmaParentAssemblyID { get; set; }

	[JsonProperty("jmaPartBinID", Order = 24)]
	[Required(ErrorMessage = "jmaPartBinID is required.")]
	[MaxLength(15)]
	public string jmaPartBinID { get; set; }

	[JsonProperty("jmaPartID", Order = 25)]
	[Required(ErrorMessage = "jmaPartID is required.")]
	[MaxLength(30)]
	public string jmaPartID { get; set; }

	[JsonProperty("jmaPartLongDescriptionRtf", Order = 26)]
	public string jmaPartLongDescriptionRtf { get; set; }

	[JsonProperty("jmaPartLongDescriptionText", Order = 27)]
	public string jmaPartLongDescriptionText { get; set; }

	[JsonProperty("jmaPartRevisionID", Order = 28)]
	[MaxLength(15)]
	public string jmaPartRevisionID { get; set; }

	[JsonProperty("jmaPartShortDescription", Order = 29)]
	[Required(ErrorMessage = "jmaPartShortDescription is required.")]
	[MaxLength(50)]
	public string jmaPartShortDescription { get; set; }

	[JsonProperty("jmaPartWareHouseLocationID", Order = 30)]
	[MaxLength(5)]
	public string jmaPartWareHouseLocationID { get; set; }

	[JsonProperty("jmaProductionNotesRTF", Order = 31)]
	[MaxLength(50)]
	public string jmaProductionNotesRTF { get; set; }

	[JsonProperty("jmaProductionNotesText", Order = 32)]
	[MaxLength(50)]
	public string jmaProductionNotesText { get; set; }

	[JsonProperty("jmaProductionQuantity", Order = 33)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaProductionQuantity { get; set; }

	[JsonProperty("jmaQuantityCompleted", Order = 34)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaQuantityCompleted { get; set; }

	[JsonProperty("jmaQuantityIssued", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaQuantityIssued { get; set; }

	[JsonProperty("jmaQuantityPerParent", Order = 36)]
	[Required(ErrorMessage = "jmaQuantityPerParent is required.")]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaQuantityPerParent { get; set; }

	[JsonProperty("jmaQuantityReceivedToInventory", Order = 37)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaQuantityReceivedToInventory { get; set; }

	[JsonProperty("jmaQuantityToInspect", Order = 38)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaQuantityToInspect { get; set; }

	[JsonProperty("jmaQuantityToMake", Order = 39)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaQuantityToMake { get; set; }

	[JsonProperty("jmaQuantityToPull", Order = 40)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaQuantityToPull { get; set; }

	[JsonProperty("jmaQuantityToReturn", Order = 41)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaQuantityToReturn { get; set; }

	[JsonProperty("jmaReworkDate", Order = 42)]
	public DateTime? jmaReworkDate { get; set; }

	[JsonProperty("jmaReworkQuantity", Order = 43)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaReworkQuantity { get; set; }

	[JsonProperty("jmaRowVersion", Order = 44)]
	public byte[] jmaRowVersion { get; set; }

	[JsonProperty("jmaScheduledDueDate", Order = 45)]
	public DateTime? jmaScheduledDueDate { get; set; }

	[JsonProperty("jmaScheduledDueHour", Order = 46)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaScheduledDueHour { get; set; }

	[JsonProperty("jmaScheduledStartDate", Order = 47)]
	public DateTime? jmaScheduledStartDate { get; set; }

	[JsonProperty("jmaScheduledStartHour", Order = 48)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaScheduledStartHour { get; set; }

	[JsonProperty("jmaScrapQuantity", Order = 49)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaScrapQuantity { get; set; }

	[JsonProperty("jmaScrapQuantityCompleted", Order = 50)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmaScrapQuantityCompleted { get; set; }

	[JsonProperty("jmaJobAssemblyID", Order = 51)]
	[Required(ErrorMessage = "jmaJobAssemblyID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int jmaJobAssemblyID { get; set; }

	[JsonProperty("jmaSourceMethodID", Order = 52)]
	[MaxLength(30)]
	public string jmaSourceMethodID { get; set; }

	[JsonProperty("jmaSourceRevisionID", Order = 53)]
	[MaxLength(15)]
	public string jmaSourceRevisionID { get; set; }

	[JsonProperty("jmaUnitOfMeasure", Order = 54)]
	[MaxLength(2)]
	public string jmaUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 55)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
