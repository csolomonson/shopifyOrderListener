using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPTimecardDto
{
	[JsonProperty("lmpActualEndTime", Order = 1)]
	public DateTime? lmpActualEndTime { get; set; }

	[JsonProperty("lmpActualStartTime", Order = 2)]
	public DateTime? lmpActualStartTime { get; set; }

	[JsonProperty("lmpCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string lmpCreatedBy { get; set; }

	[JsonProperty("lmpCreatedDate", Order = 4)]
	public DateTime? lmpCreatedDate { get; set; }

	[JsonProperty("lmpEmployeeID", Order = 5)]
	[Required(ErrorMessage = "lmpEmployeeID is required.")]
	[MaxLength(10)]
	public string lmpEmployeeID { get; set; }

	[JsonProperty("lmpUniqueID", Order = 6)]
	public Guid lmpUniqueID { get; set; }

	[JsonProperty("lmpExchangeID", Order = 7)]
	[MaxLength(50)]
	public string lmpExchangeID { get; set; }

	[JsonProperty("lmpActive", Order = 8)]
	public bool lmpActive { get; set; }

	[JsonProperty("lmpAutoClockedOut", Order = 9)]
	public bool lmpAutoClockedOut { get; set; }

	[JsonProperty("lmpCreatedFromPayrollSession", Order = 10)]
	public bool lmpCreatedFromPayrollSession { get; set; }

	[JsonProperty("lmpPostedToWip", Order = 11)]
	public bool lmpPostedToWip { get; set; }

	[JsonProperty("lmpTransferredToPayroll", Order = 12)]
	public bool lmpTransferredToPayroll { get; set; }

	[JsonProperty("lmpLastEndTime", Order = 13)]
	public DateTime? lmpLastEndTime { get; set; }

	[JsonProperty("lmpLeaveAccrualID", Order = 14)]
	[MaxLength(5)]
	public string lmpLeaveAccrualID { get; set; }

	[JsonProperty("lmpMachineHours", Order = 15)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmpMachineHours { get; set; }

	[JsonProperty("lmpNoteRtf", Order = 16)]
	[MaxLength(50)]
	public string lmpNoteRtf { get; set; }

	[JsonProperty("lmpNoteText", Order = 17)]
	[MaxLength(50)]
	public string lmpNoteText { get; set; }

	[JsonProperty("lmpOtherHours", Order = 18)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmpOtherHours { get; set; }

	[JsonProperty("lmpOtherPayrollRateID", Order = 19)]
	[MaxLength(5)]
	public string lmpOtherPayrollRateID { get; set; }

	[JsonProperty("lmpOTPeriod1Hours", Order = 20)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmpOTPeriod1Hours { get; set; }

	[JsonProperty("lmpOTPeriod1PayrollRateID", Order = 21)]
	[MaxLength(5)]
	public string lmpOTPeriod1PayrollRateID { get; set; }

	[JsonProperty("lmpOTPeriod2Hours", Order = 22)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmpOTPeriod2Hours { get; set; }

	[JsonProperty("lmpOTPeriod2PayrollRateID", Order = 23)]
	[MaxLength(5)]
	public string lmpOTPeriod2PayrollRateID { get; set; }

	[JsonProperty("lmpOTPeriod3Hours", Order = 24)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmpOTPeriod3Hours { get; set; }

	[JsonProperty("lmpOTPeriod3PayrollRateID", Order = 25)]
	[MaxLength(5)]
	public string lmpOTPeriod3PayrollRateID { get; set; }

	[JsonProperty("lmpOTPeriod4Hours", Order = 26)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmpOTPeriod4Hours { get; set; }

	[JsonProperty("lmpOTPeriod4PayrollRateID", Order = 27)]
	[MaxLength(5)]
	public string lmpOTPeriod4PayrollRateID { get; set; }

	[JsonProperty("lmpPaidDate", Order = 28)]
	public DateTime? lmpPaidDate { get; set; }

	[JsonProperty("lmpPayrollHours", Order = 29)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmpPayrollHours { get; set; }

	[JsonProperty("lmpPlantDepartmentID", Order = 30)]
	[MaxLength(5)]
	public string lmpPlantDepartmentID { get; set; }

	[JsonProperty("lmpPlantID", Order = 31)]
	[MaxLength(5)]
	public string lmpPlantID { get; set; }

	[JsonProperty("lmpPostedDate", Order = 32)]
	public DateTime? lmpPostedDate { get; set; }

	[JsonProperty("lmpProjectID", Order = 33)]
	[MaxLength(10)]
	public string lmpProjectID { get; set; }

	[JsonProperty("lmpRoundedEndTime", Order = 34)]
	public DateTime? lmpRoundedEndTime { get; set; }

	[JsonProperty("lmpRoundedStartTime", Order = 35)]
	public DateTime? lmpRoundedStartTime { get; set; }

	[JsonProperty("lmpRowVersion", Order = 36)]
	public byte[] lmpRowVersion { get; set; }

	[JsonProperty("lmpTimecardID", Order = 37)]
	[Required(ErrorMessage = "lmpTimecardID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int lmpTimecardID { get; set; }

	[JsonProperty("lmpShiftBreakID", Order = 38)]
	public byte lmpShiftBreakID { get; set; }

	[JsonProperty("lmpShiftID", Order = 39)]
	[Required(ErrorMessage = "lmpShiftID is required.")]
	public short lmpShiftID { get; set; }

	[JsonProperty("lmpSource", Order = 40)]
	[Required(ErrorMessage = "lmpSource is required.")]
	public byte lmpSource { get; set; }

	[JsonProperty("lmpStandardHours", Order = 41)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmpStandardHours { get; set; }

	[JsonProperty("lmpStandardPayrollRateID", Order = 42)]
	[MaxLength(5)]
	public string lmpStandardPayrollRateID { get; set; }

	[JsonProperty("lmpTimecardDate", Order = 43)]
	[Required(ErrorMessage = "lmpTimecardDate is required.")]
	public DateTime? lmpTimecardDate { get; set; }

	[JsonProperty("lmpTotalPayrollHours", Order = 44)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmpTotalPayrollHours { get; set; }

	[JsonProperty("lmpTransferredDate", Order = 45)]
	public DateTime? lmpTransferredDate { get; set; }

	[JsonProperty("lmpUtcOffset", Order = 46)]
	public short? lmpUtcOffset { get; set; }

	[JsonProperty("customFields", Order = 47)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
