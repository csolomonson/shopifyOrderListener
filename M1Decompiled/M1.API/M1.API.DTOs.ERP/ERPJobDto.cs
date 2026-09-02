using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPJobDto
{
	[JsonProperty("jmpCallID", Order = 1)]
	[MaxLength(10)]
	public string jmpCallID { get; set; }

	[JsonProperty("jmpClosedDate", Order = 2)]
	public DateTime? jmpClosedDate { get; set; }

	[JsonProperty("jmpJobID", Order = 3)]
	[Required(ErrorMessage = "jmpJobID is required.")]
	[MaxLength(20)]
	public string jmpJobID { get; set; }

	[JsonProperty("jmpCompletedDate", Order = 4)]
	public DateTime? jmpCompletedDate { get; set; }

	[JsonProperty("jmpCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string jmpCreatedBy { get; set; }

	[JsonProperty("jmpCreatedDate", Order = 6)]
	public DateTime? jmpCreatedDate { get; set; }

	[JsonProperty("jmpCustomerOrganizationID", Order = 7)]
	[MaxLength(10)]
	public string jmpCustomerOrganizationID { get; set; }

	[JsonProperty("jmpDocuments", Order = 8)]
	[MaxLength(50)]
	public string jmpDocuments { get; set; }

	[JsonProperty("jmpUniqueID", Order = 9)]
	public Guid jmpUniqueID { get; set; }

	[JsonProperty("jmpInventoryQuantity", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpInventoryQuantity { get; set; }

	[JsonProperty("jmpClosed", Order = 11)]
	public bool jmpClosed { get; set; }

	[JsonProperty("jmpFirm", Order = 12)]
	public bool jmpFirm { get; set; }

	[JsonProperty("jmpNestlinkProcessed", Order = 13)]
	public bool jmpNestlinkProcessed { get; set; }

	[JsonProperty("jmpOnHold", Order = 14)]
	public bool jmpOnHold { get; set; }

	[JsonProperty("jmpPlanningComplete", Order = 15)]
	public bool jmpPlanningComplete { get; set; }

	[JsonProperty("jmpProductionComplete", Order = 16)]
	public bool jmpProductionComplete { get; set; }

	[JsonProperty("jmpReadyToPrint", Order = 17)]
	public bool jmpReadyToPrint { get; set; }

	[JsonProperty("jmpReleasedToFloor", Order = 18)]
	public bool jmpReleasedToFloor { get; set; }

	[JsonProperty("jmpScheduleComplete", Order = 19)]
	public bool jmpScheduleComplete { get; set; }

	[JsonProperty("jmpScheduleLocked", Order = 20)]
	public bool jmpScheduleLocked { get; set; }

	[JsonProperty("jmpTimeAndMaterial", Order = 21)]
	public bool jmpTimeAndMaterial { get; set; }

	[JsonProperty("jmpJobDate", Order = 22)]
	[Required(ErrorMessage = "jmpJobDate is required.")]
	public DateTime? jmpJobDate { get; set; }

	[JsonProperty("jmpJobPriorityID", Order = 23)]
	public short jmpJobPriorityID { get; set; }

	[JsonProperty("jmpNonConformanceID", Order = 24)]
	[MaxLength(10)]
	public string jmpNonConformanceID { get; set; }

	[JsonProperty("jmpOrderQuantity", Order = 25)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpOrderQuantity { get; set; }

	[JsonProperty("jmpPartBinID", Order = 26)]
	[Required(ErrorMessage = "jmpPartBinID is required.")]
	[MaxLength(15)]
	public string jmpPartBinID { get; set; }

	[JsonProperty("jmpPartForecastPeriodID", Order = 27)]
	public short jmpPartForecastPeriodID { get; set; }

	[JsonProperty("jmpPartForecastYearID", Order = 28)]
	public short jmpPartForecastYearID { get; set; }

	[JsonProperty("jmpPartID", Order = 29)]
	[Required(ErrorMessage = "jmpPartID is required.")]
	[MaxLength(30)]
	public string jmpPartID { get; set; }

	[JsonProperty("jmpPartLongDescriptionRtf", Order = 30)]
	public string jmpPartLongDescriptionRtf { get; set; }

	[JsonProperty("jmpPartLongDescriptionText", Order = 31)]
	public string jmpPartLongDescriptionText { get; set; }

	[JsonProperty("jmpPartRevisionID", Order = 32)]
	[MaxLength(15)]
	public string jmpPartRevisionID { get; set; }

	[JsonProperty("jmpPartShortDescription", Order = 33)]
	[Required(ErrorMessage = "jmpPartShortDescription is required.")]
	[MaxLength(50)]
	public string jmpPartShortDescription { get; set; }

	[JsonProperty("jmpPartWareHouseLocationID", Order = 34)]
	[MaxLength(5)]
	public string jmpPartWareHouseLocationID { get; set; }

	[JsonProperty("jmpPlannerEmployeeID", Order = 35)]
	[MaxLength(10)]
	public string jmpPlannerEmployeeID { get; set; }

	[JsonProperty("jmpPlantDepartmentID", Order = 36)]
	[MaxLength(5)]
	public string jmpPlantDepartmentID { get; set; }

	[JsonProperty("jmpPlantID", Order = 37)]
	[MaxLength(5)]
	public string jmpPlantID { get; set; }

	[JsonProperty("jmpProductionDueDate", Order = 38)]
	public DateTime? jmpProductionDueDate { get; set; }

	[JsonProperty("jmpProductionNotesRTF", Order = 39)]
	[MaxLength(50)]
	public string jmpProductionNotesRTF { get; set; }

	[JsonProperty("jmpProductionNotesText", Order = 40)]
	[MaxLength(50)]
	public string jmpProductionNotesText { get; set; }

	[JsonProperty("jmpProductionQuantity", Order = 41)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpProductionQuantity { get; set; }

	[JsonProperty("jmpProjectAreaID", Order = 42)]
	[MaxLength(15)]
	public string jmpProjectAreaID { get; set; }

	[JsonProperty("jmpProjectID", Order = 43)]
	[MaxLength(10)]
	public string jmpProjectID { get; set; }

	[JsonProperty("jmpQuantityCompleted", Order = 44)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpQuantityCompleted { get; set; }

	[JsonProperty("jmpQuantityReceivedToInventory", Order = 45)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpQuantityReceivedToInventory { get; set; }

	[JsonProperty("jmpQuantityShipped", Order = 46)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpQuantityShipped { get; set; }

	[JsonProperty("jmpQuoteID", Order = 47)]
	[MaxLength(10)]
	public string jmpQuoteID { get; set; }

	[JsonProperty("jmpQuoteLineID", Order = 48)]
	public short jmpQuoteLineID { get; set; }

	[JsonProperty("jmpReworkDate", Order = 49)]
	public DateTime? jmpReworkDate { get; set; }

	[JsonProperty("jmpReworkQuantity", Order = 50)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpReworkQuantity { get; set; }

	[JsonProperty("jmpRmaClaimID", Order = 51)]
	[MaxLength(10)]
	public string jmpRmaClaimID { get; set; }

	[JsonProperty("jmpRmaClaimLineID", Order = 52)]
	public short jmpRmaClaimLineID { get; set; }

	[JsonProperty("jmpRowVersion", Order = 53)]
	public byte[] jmpRowVersion { get; set; }

	[JsonProperty("jmpScheduledDueDate", Order = 54)]
	public DateTime? jmpScheduledDueDate { get; set; }

	[JsonProperty("jmpScheduledDueHour", Order = 55)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpScheduledDueHour { get; set; }

	[JsonProperty("jmpScheduledStartDate", Order = 56)]
	public DateTime? jmpScheduledStartDate { get; set; }

	[JsonProperty("jmpScheduledStartHour", Order = 57)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpScheduledStartHour { get; set; }

	[JsonProperty("jmpScrapQuantity", Order = 58)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpScrapQuantity { get; set; }

	[JsonProperty("jmpScrapQuantityCompleted", Order = 59)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmpScrapQuantityCompleted { get; set; }

	[JsonProperty("jmpShipLocationID", Order = 60)]
	[MaxLength(5)]
	public string jmpShipLocationID { get; set; }

	[JsonProperty("jmpShipOrganizationID", Order = 61)]
	[MaxLength(10)]
	public string jmpShipOrganizationID { get; set; }

	[JsonProperty("jmpSourceMethodID", Order = 62)]
	[MaxLength(30)]
	public string jmpSourceMethodID { get; set; }

	[JsonProperty("jmpSourceRevisionID", Order = 63)]
	[MaxLength(15)]
	public string jmpSourceRevisionID { get; set; }

	[JsonProperty("jmpUnitOfMeasure", Order = 64)]
	[MaxLength(2)]
	public string jmpUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 65)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
