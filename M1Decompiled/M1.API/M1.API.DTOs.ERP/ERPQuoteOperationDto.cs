using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPQuoteOperationDto
{
	[JsonProperty("qmoAdditionalSetupHours", Order = 1)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoAdditionalSetupHours { get; set; }

	[JsonProperty("qmoAdditionalSetupQuantity", Order = 2)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoAdditionalSetupQuantity { get; set; }

	[JsonProperty("qmoCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string qmoCreatedBy { get; set; }

	[JsonProperty("qmoCreatedDate", Order = 4)]
	public DateTime? qmoCreatedDate { get; set; }

	[JsonProperty("qmoDocuments", Order = 5)]
	[MaxLength(50)]
	public string qmoDocuments { get; set; }

	[JsonProperty("qmoUniqueID", Order = 6)]
	public Guid qmoUniqueID { get; set; }

	[JsonProperty("qmoEstimatedUnitCost", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoEstimatedUnitCost { get; set; }

	[JsonProperty("qmoInspectionType", Order = 8)]
	public byte qmoInspectionType { get; set; }

	[JsonProperty("qmoClosed", Order = 9)]
	public bool qmoClosed { get; set; }

	[JsonProperty("qmoMachinesToSchedule", Order = 10)]
	public short qmoMachinesToSchedule { get; set; }

	[JsonProperty("qmoMachineType", Order = 11)]
	[Required(ErrorMessage = "qmoMachineType is required.")]
	public byte qmoMachineType { get; set; }

	[JsonProperty("qmoMinimumCharge", Order = 12)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoMinimumCharge { get; set; }

	[JsonProperty("qmoMoveTime", Order = 13)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoMoveTime { get; set; }

	[JsonProperty("qmoOperationType", Order = 14)]
	[Required(ErrorMessage = "qmoOperationType is required.")]
	public byte qmoOperationType { get; set; }

	[JsonProperty("qmoOverheadRate", Order = 15)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoOverheadRate { get; set; }

	[JsonProperty("qmoOverlap", Order = 16)]
	public byte qmoOverlap { get; set; }

	[JsonProperty("qmoOverlapDestinationLink", Order = 17)]
	public byte qmoOverlapDestinationLink { get; set; }

	[JsonProperty("qmoOverlapOffsetTime", Order = 18)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoOverlapOffsetTime { get; set; }

	[JsonProperty("qmoOverlapOperationID", Order = 19)]
	public int qmoOverlapOperationID { get; set; }

	[JsonProperty("qmoOverlapSourceLink", Order = 20)]
	public byte qmoOverlapSourceLink { get; set; }

	[JsonProperty("qmoPartID", Order = 21)]
	[MaxLength(30)]
	public string qmoPartID { get; set; }

	[JsonProperty("qmoPartRevisionID", Order = 22)]
	[MaxLength(15)]
	public string qmoPartRevisionID { get; set; }

	[JsonProperty("qmoPlantDepartmentID", Order = 23)]
	[MaxLength(5)]
	public string qmoPlantDepartmentID { get; set; }

	[JsonProperty("qmoPlantID", Order = 24)]
	[MaxLength(5)]
	public string qmoPlantID { get; set; }

	[JsonProperty("qmoProcessID", Order = 25)]
	[Required(ErrorMessage = "qmoProcessID is required.")]
	[MaxLength(5)]
	public string qmoProcessID { get; set; }

	[JsonProperty("qmoProcessLongDescriptionRtf", Order = 26)]
	public string qmoProcessLongDescriptionRtf { get; set; }

	[JsonProperty("qmoProcessLongDescriptionText", Order = 27)]
	public string qmoProcessLongDescriptionText { get; set; }

	[JsonProperty("qmoProcessShortDescription", Order = 28)]
	[Required(ErrorMessage = "qmoProcessShortDescription is required.")]
	[MaxLength(50)]
	public string qmoProcessShortDescription { get; set; }

	[JsonProperty("qmoProductionRate", Order = 29)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoProductionRate { get; set; }

	[JsonProperty("qmoProductionStandard", Order = 30)]
	[Range(0.0, 999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoProductionStandard { get; set; }

	[JsonProperty("qmoPurchaseLocationID", Order = 31)]
	[MaxLength(5)]
	public string qmoPurchaseLocationID { get; set; }

	[JsonProperty("qmoQuantityBreak1", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityBreak1 { get; set; }

	[JsonProperty("qmoQuantityBreak2", Order = 33)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityBreak2 { get; set; }

	[JsonProperty("qmoQuantityBreak3", Order = 34)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityBreak3 { get; set; }

	[JsonProperty("qmoQuantityBreak4", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityBreak4 { get; set; }

	[JsonProperty("qmoQuantityBreak5", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityBreak5 { get; set; }

	[JsonProperty("qmoQuantityBreak6", Order = 37)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityBreak6 { get; set; }

	[JsonProperty("qmoQuantityBreak7", Order = 38)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityBreak7 { get; set; }

	[JsonProperty("qmoQuantityBreak8", Order = 39)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityBreak8 { get; set; }

	[JsonProperty("qmoQuantityBreak9", Order = 40)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityBreak9 { get; set; }

	[JsonProperty("qmoQuantityPerAssembly", Order = 41)]
	[Required(ErrorMessage = "qmoQuantityPerAssembly is required.")]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuantityPerAssembly { get; set; }

	[JsonProperty("qmoQueueTime", Order = 42)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQueueTime { get; set; }

	[JsonProperty("qmoQuoteAssemblyID", Order = 43)]
	public int qmoQuoteAssemblyID { get; set; }

	[JsonProperty("qmoQuoteID", Order = 44)]
	[Required(ErrorMessage = "qmoQuoteID is required.")]
	[MaxLength(10)]
	public string qmoQuoteID { get; set; }

	[JsonProperty("qmoQuoteLineID", Order = 45)]
	[Required(ErrorMessage = "qmoQuoteLineID is required.")]
	public short qmoQuoteLineID { get; set; }

	[JsonProperty("qmoQuotingRate", Order = 46)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoQuotingRate { get; set; }

	[JsonProperty("qmoRowVersion", Order = 47)]
	public byte[] qmoRowVersion { get; set; }

	[JsonProperty("qmoQuoteOperationID", Order = 48)]
	[Required(ErrorMessage = "qmoQuoteOperationID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int qmoQuoteOperationID { get; set; }

	[JsonProperty("qmoSetupCharge", Order = 49)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoSetupCharge { get; set; }

	[JsonProperty("qmoSetupHours", Order = 50)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoSetupHours { get; set; }

	[JsonProperty("qmoSetupRate", Order = 51)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoSetupRate { get; set; }

	[JsonProperty("qmoSfeMessageRTF", Order = 52)]
	[MaxLength(50)]
	public string qmoSfeMessageRTF { get; set; }

	[JsonProperty("qmoSfeMessageText", Order = 53)]
	[MaxLength(50)]
	public string qmoSfeMessageText { get; set; }

	[JsonProperty("qmoStandardFactor", Order = 54)]
	[Required(ErrorMessage = "qmoStandardFactor is required.")]
	[MaxLength(2)]
	public string qmoStandardFactor { get; set; }

	[JsonProperty("qmoSupplierOrganizationID", Order = 55)]
	[MaxLength(10)]
	public string qmoSupplierOrganizationID { get; set; }

	[JsonProperty("qmoUnitCost1", Order = 56)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoUnitCost1 { get; set; }

	[JsonProperty("qmoUnitCost2", Order = 57)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoUnitCost2 { get; set; }

	[JsonProperty("qmoUnitCost3", Order = 58)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoUnitCost3 { get; set; }

	[JsonProperty("qmoUnitCost4", Order = 59)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoUnitCost4 { get; set; }

	[JsonProperty("qmoUnitCost5", Order = 60)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoUnitCost5 { get; set; }

	[JsonProperty("qmoUnitCost6", Order = 61)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoUnitCost6 { get; set; }

	[JsonProperty("qmoUnitCost7", Order = 62)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoUnitCost7 { get; set; }

	[JsonProperty("qmoUnitCost8", Order = 63)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoUnitCost8 { get; set; }

	[JsonProperty("qmoUnitCost9", Order = 64)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmoUnitCost9 { get; set; }

	[JsonProperty("qmoUnitOfMeasure", Order = 65)]
	[MaxLength(2)]
	public string qmoUnitOfMeasure { get; set; }

	[JsonProperty("qmoWorkCenterID", Order = 66)]
	[Required(ErrorMessage = "qmoWorkCenterID is required.")]
	[MaxLength(5)]
	public string qmoWorkCenterID { get; set; }

	[JsonProperty("qmoWorkCenterMachineID", Order = 67)]
	public short qmoWorkCenterMachineID { get; set; }

	[JsonProperty("customFields", Order = 68)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
