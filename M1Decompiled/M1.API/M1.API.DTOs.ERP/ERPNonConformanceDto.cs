using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPNonConformanceDto
{
	[JsonProperty("qarActualHours", Order = 1)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qarActualHours { get; set; }

	[JsonProperty("qarNonConformanceID", Order = 2)]
	[Required(ErrorMessage = "qarNonConformanceID is required.")]
	[MaxLength(10)]
	public string qarNonConformanceID { get; set; }

	[JsonProperty("qarCorrectiveActionCategoryID", Order = 3)]
	[MaxLength(5)]
	public string qarCorrectiveActionCategoryID { get; set; }

	[JsonProperty("qarCorrectiveActionCodeID", Order = 4)]
	[MaxLength(5)]
	public string qarCorrectiveActionCodeID { get; set; }

	[JsonProperty("qarCorrectiveActionDate", Order = 5)]
	public DateTime? qarCorrectiveActionDate { get; set; }

	[JsonProperty("qarCorrectiveActionRTF", Order = 6)]
	[MaxLength(50)]
	public string qarCorrectiveActionRTF { get; set; }

	[JsonProperty("qarCorrectiveActionText", Order = 7)]
	[MaxLength(50)]
	public string qarCorrectiveActionText { get; set; }

	[JsonProperty("qarCorrectiveActionType", Order = 8)]
	public byte qarCorrectiveActionType { get; set; }

	[JsonProperty("qarCreatedBy", Order = 9)]
	[MaxLength(20)]
	public string qarCreatedBy { get; set; }

	[JsonProperty("qarCreatedDate", Order = 10)]
	public DateTime? qarCreatedDate { get; set; }

	[JsonProperty("qarUniqueID", Order = 11)]
	public Guid qarUniqueID { get; set; }

	[JsonProperty("qarHoursAllowed", Order = 12)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qarHoursAllowed { get; set; }

	[JsonProperty("qarHoursRequested", Order = 13)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qarHoursRequested { get; set; }

	[JsonProperty("qarInspectionID", Order = 14)]
	[MaxLength(10)]
	public string qarInspectionID { get; set; }

	[JsonProperty("qarInspectionLineID", Order = 15)]
	public short qarInspectionLineID { get; set; }

	[JsonProperty("qarCorrectiveActionComplete", Order = 16)]
	public bool qarCorrectiveActionComplete { get; set; }

	[JsonProperty("qarJobAssemblyID", Order = 17)]
	public int qarJobAssemblyID { get; set; }

	[JsonProperty("qarJobID", Order = 18)]
	[MaxLength(20)]
	public string qarJobID { get; set; }

	[JsonProperty("qarJobMaterialID", Order = 19)]
	public int qarJobMaterialID { get; set; }

	[JsonProperty("qarJobOperationID", Order = 20)]
	public int qarJobOperationID { get; set; }

	[JsonProperty("qarNonConformanceCategoryID", Order = 21)]
	[MaxLength(5)]
	public string qarNonConformanceCategoryID { get; set; }

	[JsonProperty("qarNonConformanceCauseID", Order = 22)]
	[MaxLength(5)]
	public string qarNonConformanceCauseID { get; set; }

	[JsonProperty("qarNonConformanceCodeID", Order = 23)]
	[MaxLength(5)]
	public string qarNonConformanceCodeID { get; set; }

	[JsonProperty("qarNonConformanceRTF", Order = 24)]
	[MaxLength(50)]
	public string qarNonConformanceRTF { get; set; }

	[JsonProperty("qarNonConformanceText", Order = 25)]
	[MaxLength(50)]
	public string qarNonConformanceText { get; set; }

	[JsonProperty("qarPartBinID", Order = 26)]
	[MaxLength(15)]
	public string qarPartBinID { get; set; }

	[JsonProperty("qarPartID", Order = 27)]
	[MaxLength(30)]
	public string qarPartID { get; set; }

	[JsonProperty("qarPartRevisionID", Order = 28)]
	[MaxLength(15)]
	public string qarPartRevisionID { get; set; }

	[JsonProperty("qarPartShortDescription", Order = 29)]
	[MaxLength(50)]
	public string qarPartShortDescription { get; set; }

	[JsonProperty("qarPartWareHouseLocationID", Order = 30)]
	[MaxLength(5)]
	public string qarPartWareHouseLocationID { get; set; }

	[JsonProperty("qarQuantity", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qarQuantity { get; set; }

	[JsonProperty("qarRepairedByOrganizationID", Order = 32)]
	[MaxLength(10)]
	public string qarRepairedByOrganizationID { get; set; }

	[JsonProperty("qarReportedByEmployeeID", Order = 33)]
	[MaxLength(10)]
	public string qarReportedByEmployeeID { get; set; }

	[JsonProperty("qarRmaClaimID", Order = 34)]
	[MaxLength(10)]
	public string qarRmaClaimID { get; set; }

	[JsonProperty("qarRmaClaimLineID", Order = 35)]
	public short qarRmaClaimLineID { get; set; }

	[JsonProperty("qarRowVersion", Order = 36)]
	public byte[] qarRowVersion { get; set; }

	[JsonProperty("qarSubcontractAmount", Order = 37)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qarSubcontractAmount { get; set; }

	[JsonProperty("qarSubcontractAmountForeign", Order = 38)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qarSubcontractAmountForeign { get; set; }

	[JsonProperty("qarUnitOfMeasure", Order = 39)]
	[MaxLength(2)]
	public string qarUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 40)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
