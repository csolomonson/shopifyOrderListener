using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeeDto
{
	[JsonProperty("lmeCallTypeID", Order = 1)]
	[MaxLength(5)]
	public string lmeCallTypeID { get; set; }

	[JsonProperty("lmeCessationType", Order = 2)]
	[MaxLength(1)]
	public string lmeCessationType { get; set; }

	[JsonProperty("lmeEmployeeID", Order = 3)]
	[Required(ErrorMessage = "lmeEmployeeID is required.")]
	[MaxLength(10)]
	public string lmeEmployeeID { get; set; }

	[JsonProperty("lmeCommissionRate", Order = 4)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmeCommissionRate { get; set; }

	[JsonProperty("lmeContactTitleID", Order = 5)]
	[MaxLength(5)]
	public string lmeContactTitleID { get; set; }

	[JsonProperty("lmeCountyCodeID", Order = 6)]
	[MaxLength(5)]
	public string lmeCountyCodeID { get; set; }

	[JsonProperty("lmeCreatedBy", Order = 7)]
	[MaxLength(20)]
	public string lmeCreatedBy { get; set; }

	[JsonProperty("lmeCreatedDate", Order = 8)]
	public DateTime? lmeCreatedDate { get; set; }

	[JsonProperty("lmeDefaultShiftID", Order = 9)]
	public short lmeDefaultShiftID { get; set; }

	[JsonProperty("lmeDefaultWorkCenterID", Order = 10)]
	[MaxLength(5)]
	public string lmeDefaultWorkCenterID { get; set; }

	[JsonProperty("lmeDirectExpenseID", Order = 11)]
	[MaxLength(5)]
	public string lmeDirectExpenseID { get; set; }

	[JsonProperty("lmeEarningType", Order = 12)]
	public byte lmeEarningType { get; set; }

	[JsonProperty("lmeEmployeeName", Order = 13)]
	[Required(ErrorMessage = "lmeEmployeeName is required.")]
	[MaxLength(50)]
	public string lmeEmployeeName { get; set; }

	[JsonProperty("lmeUniqueID", Order = 14)]
	public Guid lmeUniqueID { get; set; }

	[JsonProperty("lmeHireDate", Order = 15)]
	[Required(ErrorMessage = "lmeHireDate is required.")]
	public DateTime? lmeHireDate { get; set; }

	[JsonProperty("lmeHomeProductionDepartmentID", Order = 16)]
	[MaxLength(5)]
	public string lmeHomeProductionDepartmentID { get; set; }

	[JsonProperty("lmeIndirectExpenseID", Order = 17)]
	[MaxLength(5)]
	public string lmeIndirectExpenseID { get; set; }

	[JsonProperty("lmeBuyerEmployee", Order = 18)]
	public bool lmeBuyerEmployee { get; set; }

	[JsonProperty("lmeEngineerEmployee", Order = 19)]
	public bool lmeEngineerEmployee { get; set; }

	[JsonProperty("lmeInspectorEmployee", Order = 20)]
	public bool lmeInspectorEmployee { get; set; }

	[JsonProperty("lmeLockShift", Order = 21)]
	public bool lmeLockShift { get; set; }

	[JsonProperty("lmePayrollEmployee", Order = 22)]
	public bool lmePayrollEmployee { get; set; }

	[JsonProperty("lmePlannerEmployee", Order = 23)]
	public bool lmePlannerEmployee { get; set; }

	[JsonProperty("lmeProjectManagerEmployee", Order = 24)]
	public bool lmeProjectManagerEmployee { get; set; }

	[JsonProperty("lmeQuoterEmployee", Order = 25)]
	public bool lmeQuoterEmployee { get; set; }

	[JsonProperty("lmeSalesEmployee", Order = 26)]
	public bool lmeSalesEmployee { get; set; }

	[JsonProperty("lmeShopEmployee", Order = 27)]
	public bool lmeShopEmployee { get; set; }

	[JsonProperty("lmeSortSfebyWorkcenter", Order = 28)]
	public bool lmeSortSfebyWorkcenter { get; set; }

	[JsonProperty("lmeSupportEmployee", Order = 29)]
	public bool lmeSupportEmployee { get; set; }

	[JsonProperty("lmeLanguage", Order = 30)]
	[MaxLength(10)]
	public string lmeLanguage { get; set; }

	[JsonProperty("lmePassword", Order = 31)]
	[MaxLength(10)]
	public string lmePassword { get; set; }

	[JsonProperty("lmePlantDepartmentID", Order = 32)]
	[MaxLength(5)]
	public string lmePlantDepartmentID { get; set; }

	[JsonProperty("lmePlantID", Order = 33)]
	[MaxLength(5)]
	public string lmePlantID { get; set; }

	[JsonProperty("lmePoApprovalAmount", Order = 34)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmePoApprovalAmount { get; set; }

	[JsonProperty("lmePreviousEmployeeID", Order = 35)]
	[MaxLength(10)]
	public string lmePreviousEmployeeID { get; set; }

	[JsonProperty("lmeRowVersion", Order = 36)]
	public byte[] lmeRowVersion { get; set; }

	[JsonProperty("lmeSOApprovalAmount", Order = 37)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmeSOApprovalAmount { get; set; }

	[JsonProperty("lmeTerminationDate", Order = 38)]
	public DateTime? lmeTerminationDate { get; set; }

	[JsonProperty("lmeTerminationReasonID", Order = 39)]
	[MaxLength(5)]
	public string lmeTerminationReasonID { get; set; }

	[JsonProperty("lmeUseEmail", Order = 40)]
	public byte lmeUseEmail { get; set; }

	[JsonProperty("lmeUseEmailPayslips", Order = 41)]
	public byte lmeUseEmailPayslips { get; set; }

	[JsonProperty("lmeUserID", Order = 42)]
	[MaxLength(20)]
	public string lmeUserID { get; set; }

	[JsonProperty("lmeWorkEmailAddress", Order = 43)]
	[MaxLength(50)]
	public string lmeWorkEmailAddress { get; set; }

	[JsonProperty("customFields", Order = 44)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
