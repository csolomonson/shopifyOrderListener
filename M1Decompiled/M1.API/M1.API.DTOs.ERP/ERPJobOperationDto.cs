using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPJobOperationDto
{
	[JsonProperty("jmoActualProductionHours", Order = 1)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoActualProductionHours { get; set; }

	[JsonProperty("jmoActualSetupHours", Order = 2)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoActualSetupHours { get; set; }

	[JsonProperty("jmoCalculatedUnitCost", Order = 3)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoCalculatedUnitCost { get; set; }

	[JsonProperty("jmoCompletedProductionHours", Order = 4)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoCompletedProductionHours { get; set; }

	[JsonProperty("jmoCompletedSetupHours", Order = 5)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoCompletedSetupHours { get; set; }

	[JsonProperty("jmoCreatedBy", Order = 6)]
	[MaxLength(20)]
	public string jmoCreatedBy { get; set; }

	[JsonProperty("jmoCreatedDate", Order = 7)]
	public DateTime? jmoCreatedDate { get; set; }

	[JsonProperty("jmoDocuments", Order = 8)]
	[MaxLength(50)]
	public string jmoDocuments { get; set; }

	[JsonProperty("jmoDueDate", Order = 9)]
	public DateTime? jmoDueDate { get; set; }

	[JsonProperty("jmoDueHour", Order = 10)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoDueHour { get; set; }

	[JsonProperty("jmoUniqueID", Order = 11)]
	public Guid jmoUniqueID { get; set; }

	[JsonProperty("jmoEstimatedProductionHours", Order = 12)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoEstimatedProductionHours { get; set; }

	[JsonProperty("jmoEstimatedUnitCost", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoEstimatedUnitCost { get; set; }

	[JsonProperty("jmoInspectionStatus", Order = 14)]
	public byte jmoInspectionStatus { get; set; }

	[JsonProperty("jmoInspectionType", Order = 15)]
	public byte jmoInspectionType { get; set; }

	[JsonProperty("jmoAddedOperation", Order = 16)]
	public bool jmoAddedOperation { get; set; }

	[JsonProperty("jmoClosed", Order = 17)]
	public bool jmoClosed { get; set; }

	[JsonProperty("jmoFirm", Order = 18)]
	public bool jmoFirm { get; set; }

	[JsonProperty("jmoInspectionComplete", Order = 19)]
	public bool jmoInspectionComplete { get; set; }

	[JsonProperty("jmoProductionComplete", Order = 20)]
	public bool jmoProductionComplete { get; set; }

	[JsonProperty("jmoPrototypeOperation", Order = 21)]
	public bool jmoPrototypeOperation { get; set; }

	[JsonProperty("jmoSetupComplete", Order = 22)]
	public bool jmoSetupComplete { get; set; }

	[JsonProperty("jmoJobAssemblyID", Order = 23)]
	public int jmoJobAssemblyID { get; set; }

	[JsonProperty("jmoJobID", Order = 24)]
	[Required(ErrorMessage = "jmoJobID is required.")]
	[MaxLength(20)]
	public string jmoJobID { get; set; }

	[JsonProperty("jmoMachinesToSchedule", Order = 25)]
	public short jmoMachinesToSchedule { get; set; }

	[JsonProperty("jmoMachineType", Order = 26)]
	[Required(ErrorMessage = "jmoMachineType is required.")]
	public byte jmoMachineType { get; set; }

	[JsonProperty("jmoMinimumCharge", Order = 27)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoMinimumCharge { get; set; }

	[JsonProperty("jmoMoveTime", Order = 28)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoMoveTime { get; set; }

	[JsonProperty("jmoOperationQuantity", Order = 29)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoOperationQuantity { get; set; }

	[JsonProperty("jmoOperationType", Order = 30)]
	[Required(ErrorMessage = "jmoOperationType is required.")]
	public byte jmoOperationType { get; set; }

	[JsonProperty("jmoOverheadRate", Order = 31)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoOverheadRate { get; set; }

	[JsonProperty("jmoOverlap", Order = 32)]
	public byte jmoOverlap { get; set; }

	[JsonProperty("jmoOverlapDestinationLink", Order = 33)]
	public byte jmoOverlapDestinationLink { get; set; }

	[JsonProperty("jmoOverlapOffsetTime", Order = 34)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoOverlapOffsetTime { get; set; }

	[JsonProperty("jmoOverlapOperationID", Order = 35)]
	public int jmoOverlapOperationID { get; set; }

	[JsonProperty("jmoOverlapSourceLink", Order = 36)]
	public byte jmoOverlapSourceLink { get; set; }

	[JsonProperty("jmoPartBinID", Order = 37)]
	[MaxLength(15)]
	public string jmoPartBinID { get; set; }

	[JsonProperty("jmoPartID", Order = 38)]
	[MaxLength(30)]
	public string jmoPartID { get; set; }

	[JsonProperty("jmoPartRevisionID", Order = 39)]
	[MaxLength(15)]
	public string jmoPartRevisionID { get; set; }

	[JsonProperty("jmoPartWarehouseLocationID", Order = 40)]
	[MaxLength(5)]
	public string jmoPartWarehouseLocationID { get; set; }

	[JsonProperty("jmoPlantDepartmentID", Order = 41)]
	[MaxLength(5)]
	public string jmoPlantDepartmentID { get; set; }

	[JsonProperty("jmoPlantID", Order = 42)]
	[MaxLength(5)]
	public string jmoPlantID { get; set; }

	[JsonProperty("jmoProcessID", Order = 43)]
	[Required(ErrorMessage = "jmoProcessID is required.")]
	[MaxLength(5)]
	public string jmoProcessID { get; set; }

	[JsonProperty("jmoProcessLongDescriptionRtf", Order = 44)]
	public string jmoProcessLongDescriptionRtf { get; set; }

	[JsonProperty("jmoProcessLongDescriptionText", Order = 45)]
	public string jmoProcessLongDescriptionText { get; set; }

	[JsonProperty("jmoProcessShortDescription", Order = 46)]
	[Required(ErrorMessage = "jmoProcessShortDescription is required.")]
	[MaxLength(50)]
	public string jmoProcessShortDescription { get; set; }

	[JsonProperty("jmoProductionRate", Order = 47)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoProductionRate { get; set; }

	[JsonProperty("jmoProductionStandard", Order = 48)]
	[Range(0.0, 999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoProductionStandard { get; set; }

	[JsonProperty("jmoPurchaseLocationID", Order = 49)]
	[MaxLength(5)]
	public string jmoPurchaseLocationID { get; set; }

	[JsonProperty("jmoPurchaseOrderID", Order = 50)]
	[MaxLength(10)]
	public string jmoPurchaseOrderID { get; set; }

	[JsonProperty("jmoQuantityBreak1", Order = 51)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityBreak1 { get; set; }

	[JsonProperty("jmoQuantityBreak2", Order = 52)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityBreak2 { get; set; }

	[JsonProperty("jmoQuantityBreak3", Order = 53)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityBreak3 { get; set; }

	[JsonProperty("jmoQuantityBreak4", Order = 54)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityBreak4 { get; set; }

	[JsonProperty("jmoQuantityBreak5", Order = 55)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityBreak5 { get; set; }

	[JsonProperty("jmoQuantityBreak6", Order = 56)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityBreak6 { get; set; }

	[JsonProperty("jmoQuantityBreak7", Order = 57)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityBreak7 { get; set; }

	[JsonProperty("jmoQuantityBreak8", Order = 58)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityBreak8 { get; set; }

	[JsonProperty("jmoQuantityBreak9", Order = 59)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityBreak9 { get; set; }

	[JsonProperty("jmoQuantityComplete", Order = 60)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityComplete { get; set; }

	[JsonProperty("jmoQuantityPerAssembly", Order = 61)]
	[Required(ErrorMessage = "jmoQuantityPerAssembly is required.")]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityPerAssembly { get; set; }

	[JsonProperty("jmoQuantityToInspect", Order = 62)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityToInspect { get; set; }

	[JsonProperty("jmoQuantityToReturn", Order = 63)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQuantityToReturn { get; set; }

	[JsonProperty("jmoQueueTime", Order = 64)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoQueueTime { get; set; }

	[JsonProperty("jmoRfqID", Order = 65)]
	[MaxLength(10)]
	public string jmoRfqID { get; set; }

	[JsonProperty("jmoRowVersion", Order = 66)]
	public byte[] jmoRowVersion { get; set; }

	[JsonProperty("jmoScrapQuantityReceived", Order = 67)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoScrapQuantityReceived { get; set; }

	[JsonProperty("jmoJobOperationID", Order = 68)]
	[Required(ErrorMessage = "jmoJobOperationID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int jmoJobOperationID { get; set; }

	[JsonProperty("jmoSetupCharge", Order = 69)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoSetupCharge { get; set; }

	[JsonProperty("jmoSetupHours", Order = 70)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoSetupHours { get; set; }

	[JsonProperty("jmoSetupPercentComplete", Order = 71)]
	public short jmoSetupPercentComplete { get; set; }

	[JsonProperty("jmoSetupRate", Order = 72)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoSetupRate { get; set; }

	[JsonProperty("jmoSfeMessageRTF", Order = 73)]
	[MaxLength(50)]
	public string jmoSfeMessageRTF { get; set; }

	[JsonProperty("jmoSfeMessageText", Order = 74)]
	[MaxLength(50)]
	public string jmoSfeMessageText { get; set; }

	[JsonProperty("jmoStandardFactor", Order = 75)]
	[Required(ErrorMessage = "jmoStandardFactor is required.")]
	[MaxLength(2)]
	public string jmoStandardFactor { get; set; }

	[JsonProperty("jmoStartDate", Order = 76)]
	public DateTime? jmoStartDate { get; set; }

	[JsonProperty("jmoStartHour", Order = 77)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoStartHour { get; set; }

	[JsonProperty("jmoSupplierOrganizationID", Order = 78)]
	[MaxLength(10)]
	public string jmoSupplierOrganizationID { get; set; }

	[JsonProperty("jmoUnitCost1", Order = 79)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoUnitCost1 { get; set; }

	[JsonProperty("jmoUnitCost2", Order = 80)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoUnitCost2 { get; set; }

	[JsonProperty("jmoUnitCost3", Order = 81)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoUnitCost3 { get; set; }

	[JsonProperty("jmoUnitCost4", Order = 82)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoUnitCost4 { get; set; }

	[JsonProperty("jmoUnitCost5", Order = 83)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoUnitCost5 { get; set; }

	[JsonProperty("jmoUnitCost6", Order = 84)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoUnitCost6 { get; set; }

	[JsonProperty("jmoUnitCost7", Order = 85)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoUnitCost7 { get; set; }

	[JsonProperty("jmoUnitCost8", Order = 86)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoUnitCost8 { get; set; }

	[JsonProperty("jmoUnitCost9", Order = 87)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmoUnitCost9 { get; set; }

	[JsonProperty("jmoUnitOfMeasure", Order = 88)]
	[MaxLength(2)]
	public string jmoUnitOfMeasure { get; set; }

	[JsonProperty("jmoWorkCenterID", Order = 89)]
	[Required(ErrorMessage = "jmoWorkCenterID is required.")]
	[MaxLength(5)]
	public string jmoWorkCenterID { get; set; }

	[JsonProperty("jmoWorkCenterMachineID", Order = 90)]
	public short jmoWorkCenterMachineID { get; set; }

	[JsonProperty("customFields", Order = 91)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
