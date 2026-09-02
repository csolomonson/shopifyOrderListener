using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMaterialIssueLineDto
{
	[JsonProperty("injCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string injCreatedBy { get; set; }

	[JsonProperty("injCreatedDate", Order = 2)]
	public DateTime? injCreatedDate { get; set; }

	[JsonProperty("injUniqueID", Order = 3)]
	public Guid injUniqueID { get; set; }

	[JsonProperty("injEstimatedQuantity", Order = 4)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injEstimatedQuantity { get; set; }

	[JsonProperty("injHeatLot", Order = 5)]
	[MaxLength(50)]
	public string injHeatLot { get; set; }

	[JsonProperty("injInvIssueQuantity", Order = 6)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injInvIssueQuantity { get; set; }

	[JsonProperty("injInvScrapQuantity", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injInvScrapQuantity { get; set; }

	[JsonProperty("injCreateJobSeq", Order = 8)]
	public bool injCreateJobSeq { get; set; }

	[JsonProperty("injIssueComplete", Order = 9)]
	public bool injIssueComplete { get; set; }

	[JsonProperty("injKitPart", Order = 10)]
	public bool injKitPart { get; set; }

	[JsonProperty("injPosted", Order = 11)]
	public bool injPosted { get; set; }

	[JsonProperty("injReversed", Order = 12)]
	public bool injReversed { get; set; }

	[JsonProperty("injIssueType", Order = 13)]
	[Required(ErrorMessage = "injIssueType is required.")]
	public byte injIssueType { get; set; }

	[JsonProperty("injJobAsmIssueQuantity", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injJobAsmIssueQuantity { get; set; }

	[JsonProperty("injJobAsmScrapQuantity", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injJobAsmScrapQuantity { get; set; }

	[JsonProperty("injJobAssemblyID", Order = 16)]
	public int injJobAssemblyID { get; set; }

	[JsonProperty("injJobID", Order = 17)]
	[MaxLength(20)]
	public string injJobID { get; set; }

	[JsonProperty("injJobMaterialID", Order = 18)]
	public int injJobMaterialID { get; set; }

	[JsonProperty("injJobMatIssueQuantity", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injJobMatIssueQuantity { get; set; }

	[JsonProperty("injJobMatReturnIssueQuantity", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injJobMatReturnIssueQuantity { get; set; }

	[JsonProperty("injJobMatReturnScrapQuantity", Order = 21)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injJobMatReturnScrapQuantity { get; set; }

	[JsonProperty("injJobMatScrapQuantity", Order = 22)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injJobMatScrapQuantity { get; set; }

	[JsonProperty("injJobOpenQuantity", Order = 23)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injJobOpenQuantity { get; set; }

	[JsonProperty("injJobType", Order = 24)]
	public byte injJobType { get; set; }

	[JsonProperty("injLongDescriptionRtf", Order = 25)]
	public string injLongDescriptionRtf { get; set; }

	[JsonProperty("injLongDescriptionText", Order = 26)]
	public string injLongDescriptionText { get; set; }

	[JsonProperty("injMaterialIssueID", Order = 27)]
	[Required(ErrorMessage = "injMaterialIssueID is required.")]
	[MaxLength(10)]
	public string injMaterialIssueID { get; set; }

	[JsonProperty("injMiscIssueReasonID", Order = 28)]
	[MaxLength(5)]
	public string injMiscIssueReasonID { get; set; }

	[JsonProperty("injPartBinID", Order = 29)]
	[Required(ErrorMessage = "injPartBinID is required.")]
	[MaxLength(15)]
	public string injPartBinID { get; set; }

	[JsonProperty("injPartID", Order = 30)]
	[Required(ErrorMessage = "injPartID is required.")]
	[MaxLength(30)]
	public string injPartID { get; set; }

	[JsonProperty("injPartRevisionID", Order = 31)]
	[MaxLength(15)]
	public string injPartRevisionID { get; set; }

	[JsonProperty("injPartWarehouseLocationID", Order = 32)]
	[Required(ErrorMessage = "injPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string injPartWarehouseLocationID { get; set; }

	[JsonProperty("injPlantID", Order = 33)]
	[MaxLength(5)]
	public string injPlantID { get; set; }

	[JsonProperty("injProjectAreaID", Order = 34)]
	[MaxLength(15)]
	public string injProjectAreaID { get; set; }

	[JsonProperty("injProjectID", Order = 35)]
	[MaxLength(10)]
	public string injProjectID { get; set; }

	[JsonProperty("injQuantityAllocated", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injQuantityAllocated { get; set; }

	[JsonProperty("injQuantityOnHand", Order = 37)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal injQuantityOnHand { get; set; }

	[JsonProperty("injReference", Order = 38)]
	[MaxLength(30)]
	public string injReference { get; set; }

	[JsonProperty("injReverseMaterialIssueID", Order = 39)]
	[MaxLength(10)]
	public string injReverseMaterialIssueID { get; set; }

	[JsonProperty("injReverseMaterialIssueLineID", Order = 40)]
	public short injReverseMaterialIssueLineID { get; set; }

	[JsonProperty("injRowVersion", Order = 41)]
	public byte[] injRowVersion { get; set; }

	[JsonProperty("injMaterialIssueLineID", Order = 42)]
	[Required(ErrorMessage = "injMaterialIssueLineID is required.")]
	public short injMaterialIssueLineID { get; set; }

	[JsonProperty("customFields", Order = 43)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
