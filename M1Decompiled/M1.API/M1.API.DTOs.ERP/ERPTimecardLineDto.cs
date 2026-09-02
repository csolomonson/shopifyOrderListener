using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPTimecardLineDto
{
	[JsonProperty("lmlActualEndTime", Order = 1)]
	public DateTime? lmlActualEndTime { get; set; }

	[JsonProperty("lmlActualStartTime", Order = 2)]
	public DateTime? lmlActualStartTime { get; set; }

	[JsonProperty("lmlCompletionType", Order = 3)]
	public byte lmlCompletionType { get; set; }

	[JsonProperty("lmlCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string lmlCreatedBy { get; set; }

	[JsonProperty("lmlCreatedDate", Order = 5)]
	public DateTime? lmlCreatedDate { get; set; }

	[JsonProperty("lmlEmployeeID", Order = 6)]
	[Required(ErrorMessage = "lmlEmployeeID is required.")]
	[MaxLength(10)]
	public string lmlEmployeeID { get; set; }

	[JsonProperty("lmlUniqueID", Order = 7)]
	public Guid lmlUniqueID { get; set; }

	[JsonProperty("lmlExpenseID", Order = 8)]
	[Required(ErrorMessage = "lmlExpenseID is required.")]
	[MaxLength(5)]
	public string lmlExpenseID { get; set; }

	[JsonProperty("lmlGoodQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmlGoodQuantity { get; set; }

	[JsonProperty("lmlIndirectLaborID", Order = 10)]
	[MaxLength(5)]
	public string lmlIndirectLaborID { get; set; }

	[JsonProperty("lmlActive", Order = 11)]
	public bool lmlActive { get; set; }

	[JsonProperty("lmlCreatedFromPayrollSession", Order = 12)]
	public bool lmlCreatedFromPayrollSession { get; set; }

	[JsonProperty("lmlLaborHoursCalculated", Order = 13)]
	public bool lmlLaborHoursCalculated { get; set; }

	[JsonProperty("lmlMachineHoursCalculated", Order = 14)]
	public bool lmlMachineHoursCalculated { get; set; }

	[JsonProperty("lmlPostedToWip", Order = 15)]
	public bool lmlPostedToWip { get; set; }

	[JsonProperty("lmlSuspended", Order = 16)]
	public bool lmlSuspended { get; set; }

	[JsonProperty("lmlTransferredToPayroll", Order = 17)]
	public bool lmlTransferredToPayroll { get; set; }

	[JsonProperty("lmlJobAssemblyID", Order = 18)]
	public int lmlJobAssemblyID { get; set; }

	[JsonProperty("lmlJobID", Order = 19)]
	[MaxLength(20)]
	public string lmlJobID { get; set; }

	[JsonProperty("lmlJobOperationID", Order = 20)]
	public int lmlJobOperationID { get; set; }

	[JsonProperty("lmlLaborCost", Order = 21)]
	[Range(0.0, 99999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmlLaborCost { get; set; }

	[JsonProperty("lmlLaborDescriptionRtf", Order = 22)]
	[MaxLength(50)]
	public string lmlLaborDescriptionRtf { get; set; }

	[JsonProperty("lmlLaborDescriptionText", Order = 23)]
	[MaxLength(50)]
	public string lmlLaborDescriptionText { get; set; }

	[JsonProperty("lmlLaborHours", Order = 24)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmlLaborHours { get; set; }

	[JsonProperty("lmlMachineHours", Order = 25)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmlMachineHours { get; set; }

	[JsonProperty("lmlOverheadCost", Order = 26)]
	[Range(0.0, 99999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmlOverheadCost { get; set; }

	[JsonProperty("lmlProcessID", Order = 27)]
	[MaxLength(5)]
	public string lmlProcessID { get; set; }

	[JsonProperty("lmlProjectAreaID", Order = 28)]
	[MaxLength(15)]
	public string lmlProjectAreaID { get; set; }

	[JsonProperty("lmlProjectID", Order = 29)]
	[MaxLength(10)]
	public string lmlProjectID { get; set; }

	[JsonProperty("lmlReworkQuantity", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmlReworkQuantity { get; set; }

	[JsonProperty("lmlReworkReasonID", Order = 31)]
	[MaxLength(5)]
	public string lmlReworkReasonID { get; set; }

	[JsonProperty("lmlRoundedEndTime", Order = 32)]
	public DateTime? lmlRoundedEndTime { get; set; }

	[JsonProperty("lmlRoundedStartTime", Order = 33)]
	public DateTime? lmlRoundedStartTime { get; set; }

	[JsonProperty("lmlRowVersion", Order = 34)]
	public byte[] lmlRowVersion { get; set; }

	[JsonProperty("lmlScrapQuantity", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmlScrapQuantity { get; set; }

	[JsonProperty("lmlScrapReasonID", Order = 36)]
	[MaxLength(5)]
	public string lmlScrapReasonID { get; set; }

	[JsonProperty("lmlTimecardLineID", Order = 37)]
	[Required(ErrorMessage = "lmlTimecardLineID is required.")]
	public short lmlTimecardLineID { get; set; }

	[JsonProperty("lmlSetupPercentCompleted", Order = 38)]
	public short lmlSetupPercentCompleted { get; set; }

	[JsonProperty("lmlShiftID", Order = 39)]
	public short lmlShiftID { get; set; }

	[JsonProperty("lmlSource", Order = 40)]
	[Required(ErrorMessage = "lmlSource is required.")]
	public byte lmlSource { get; set; }

	[JsonProperty("lmlTimecardID", Order = 41)]
	[Required(ErrorMessage = "lmlTimecardID is required.")]
	public int lmlTimecardID { get; set; }

	[JsonProperty("lmlTimecardType", Order = 42)]
	[Required(ErrorMessage = "lmlTimecardType is required.")]
	public byte lmlTimecardType { get; set; }

	[JsonProperty("lmlWorkCenterID", Order = 43)]
	[Required(ErrorMessage = "lmlWorkCenterID is required.")]
	[MaxLength(5)]
	public string lmlWorkCenterID { get; set; }

	[JsonProperty("lmlWorkType", Order = 44)]
	public byte lmlWorkType { get; set; }

	[JsonProperty("customFields", Order = 45)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
