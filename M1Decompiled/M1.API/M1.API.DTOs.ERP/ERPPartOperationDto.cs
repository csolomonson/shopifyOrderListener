using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartOperationDto
{
	[JsonProperty("imoCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string imoCreatedBy { get; set; }

	[JsonProperty("imoCreatedDate", Order = 2)]
	public DateTime? imoCreatedDate { get; set; }

	[JsonProperty("imoDocuments", Order = 3)]
	[MaxLength(50)]
	public string imoDocuments { get; set; }

	[JsonProperty("imoUniqueID", Order = 4)]
	public Guid imoUniqueID { get; set; }

	[JsonProperty("imoEstimatedUnitCost", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoEstimatedUnitCost { get; set; }

	[JsonProperty("imoInspectionType", Order = 6)]
	public byte imoInspectionType { get; set; }

	[JsonProperty("imoMachinesToSchedule", Order = 7)]
	public short imoMachinesToSchedule { get; set; }

	[JsonProperty("imoMachineType", Order = 8)]
	[Required(ErrorMessage = "imoMachineType is required.")]
	public byte imoMachineType { get; set; }

	[JsonProperty("imoMethodAssemblyID", Order = 9)]
	public int imoMethodAssemblyID { get; set; }

	[JsonProperty("imoMethodID", Order = 10)]
	[Required(ErrorMessage = "imoMethodID is required.")]
	[MaxLength(30)]
	public string imoMethodID { get; set; }

	[JsonProperty("imoMethodOperationID", Order = 11)]
	[Required(ErrorMessage = "imoMethodOperationID is required.")]
	public int imoMethodOperationID { get; set; }

	[JsonProperty("imoMethodRevisionID", Order = 12)]
	[MaxLength(15)]
	public string imoMethodRevisionID { get; set; }

	[JsonProperty("imoMinimumCharge", Order = 13)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoMinimumCharge { get; set; }

	[JsonProperty("imoMoveTime", Order = 14)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoMoveTime { get; set; }

	[JsonProperty("imoOperationType", Order = 15)]
	[Required(ErrorMessage = "imoOperationType is required.")]
	public byte imoOperationType { get; set; }

	[JsonProperty("imoOverlap", Order = 16)]
	public byte imoOverlap { get; set; }

	[JsonProperty("imoOverlapDestinationLink", Order = 17)]
	public byte imoOverlapDestinationLink { get; set; }

	[JsonProperty("imoOverlapOffsetTime", Order = 18)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoOverlapOffsetTime { get; set; }

	[JsonProperty("imoOverlapOperationID", Order = 19)]
	public int imoOverlapOperationID { get; set; }

	[JsonProperty("imoOverlapSourceLink", Order = 20)]
	public byte imoOverlapSourceLink { get; set; }

	[JsonProperty("imoPartID", Order = 21)]
	[MaxLength(30)]
	public string imoPartID { get; set; }

	[JsonProperty("imoPartRevisionID", Order = 22)]
	[MaxLength(15)]
	public string imoPartRevisionID { get; set; }

	[JsonProperty("imoPlantDepartmentID", Order = 23)]
	[MaxLength(5)]
	public string imoPlantDepartmentID { get; set; }

	[JsonProperty("imoPlantID", Order = 24)]
	[MaxLength(5)]
	public string imoPlantID { get; set; }

	[JsonProperty("imoProcessID", Order = 25)]
	[Required(ErrorMessage = "imoProcessID is required.")]
	[MaxLength(5)]
	public string imoProcessID { get; set; }

	[JsonProperty("imoProcessLongDescriptionRtf", Order = 26)]
	public string imoProcessLongDescriptionRtf { get; set; }

	[JsonProperty("imoProcessLongDescriptionText", Order = 27)]
	public string imoProcessLongDescriptionText { get; set; }

	[JsonProperty("imoProcessShortDescription", Order = 28)]
	[Required(ErrorMessage = "imoProcessShortDescription is required.")]
	[MaxLength(50)]
	public string imoProcessShortDescription { get; set; }

	[JsonProperty("imoProductionStandard", Order = 29)]
	[Range(0.0, 999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoProductionStandard { get; set; }

	[JsonProperty("imoPurchaseLocationID", Order = 30)]
	[MaxLength(5)]
	public string imoPurchaseLocationID { get; set; }

	[JsonProperty("imoQuantityBreak1", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityBreak1 { get; set; }

	[JsonProperty("imoQuantityBreak2", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityBreak2 { get; set; }

	[JsonProperty("imoQuantityBreak3", Order = 33)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityBreak3 { get; set; }

	[JsonProperty("imoQuantityBreak4", Order = 34)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityBreak4 { get; set; }

	[JsonProperty("imoQuantityBreak5", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityBreak5 { get; set; }

	[JsonProperty("imoQuantityBreak6", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityBreak6 { get; set; }

	[JsonProperty("imoQuantityBreak7", Order = 37)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityBreak7 { get; set; }

	[JsonProperty("imoQuantityBreak8", Order = 38)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityBreak8 { get; set; }

	[JsonProperty("imoQuantityBreak9", Order = 39)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityBreak9 { get; set; }

	[JsonProperty("imoQuantityPerAssembly", Order = 40)]
	[Required(ErrorMessage = "imoQuantityPerAssembly is required.")]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQuantityPerAssembly { get; set; }

	[JsonProperty("imoQueueTime", Order = 41)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoQueueTime { get; set; }

	[JsonProperty("imoRowVersion", Order = 42)]
	public byte[] imoRowVersion { get; set; }

	[JsonProperty("imoSetupCharge", Order = 43)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoSetupCharge { get; set; }

	[JsonProperty("imoSetupHours", Order = 44)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoSetupHours { get; set; }

	[JsonProperty("imoSfeMessageRTF", Order = 45)]
	[MaxLength(50)]
	public string imoSfeMessageRTF { get; set; }

	[JsonProperty("imoSfeMessageText", Order = 46)]
	[MaxLength(50)]
	public string imoSfeMessageText { get; set; }

	[JsonProperty("imoStandardFactor", Order = 47)]
	[Required(ErrorMessage = "imoStandardFactor is required.")]
	[MaxLength(2)]
	public string imoStandardFactor { get; set; }

	[JsonProperty("imoSupplierOrganizationID", Order = 48)]
	[MaxLength(10)]
	public string imoSupplierOrganizationID { get; set; }

	[JsonProperty("imoUnitCost1", Order = 49)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoUnitCost1 { get; set; }

	[JsonProperty("imoUnitCost2", Order = 50)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoUnitCost2 { get; set; }

	[JsonProperty("imoUnitCost3", Order = 51)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoUnitCost3 { get; set; }

	[JsonProperty("imoUnitCost4", Order = 52)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoUnitCost4 { get; set; }

	[JsonProperty("imoUnitCost5", Order = 53)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoUnitCost5 { get; set; }

	[JsonProperty("imoUnitCost6", Order = 54)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoUnitCost6 { get; set; }

	[JsonProperty("imoUnitCost7", Order = 55)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoUnitCost7 { get; set; }

	[JsonProperty("imoUnitCost8", Order = 56)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoUnitCost8 { get; set; }

	[JsonProperty("imoUnitCost9", Order = 57)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imoUnitCost9 { get; set; }

	[JsonProperty("imoUnitOfMeasure", Order = 58)]
	[MaxLength(2)]
	public string imoUnitOfMeasure { get; set; }

	[JsonProperty("imoWorkCenterID", Order = 59)]
	[Required(ErrorMessage = "imoWorkCenterID is required.")]
	[MaxLength(5)]
	public string imoWorkCenterID { get; set; }

	[JsonProperty("imoWorkCenterMachineID", Order = 60)]
	public short imoWorkCenterMachineID { get; set; }

	[JsonProperty("customFields", Order = 61)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
