using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPInspectionComponentDto
{
	[JsonProperty("qamAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamAdditionalQuantity { get; set; }

	[JsonProperty("qamComponentQtyToInspect", Order = 2)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamComponentQtyToInspect { get; set; }

	[JsonProperty("qamCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string qamCreatedBy { get; set; }

	[JsonProperty("qamCreatedDate", Order = 4)]
	public DateTime? qamCreatedDate { get; set; }

	[JsonProperty("qamDescription", Order = 5)]
	[Required(ErrorMessage = "qamDescription is required.")]
	[MaxLength(50)]
	public string qamDescription { get; set; }

	[JsonProperty("qamUniqueID", Order = 6)]
	public Guid qamUniqueID { get; set; }

	[JsonProperty("qamInspectionID", Order = 7)]
	[Required(ErrorMessage = "qamInspectionID is required.")]
	[MaxLength(10)]
	public string qamInspectionID { get; set; }

	[JsonProperty("qamInspectionLineID", Order = 8)]
	[Required(ErrorMessage = "qamInspectionLineID is required.")]
	public short qamInspectionLineID { get; set; }

	[JsonProperty("qamInspectionType", Order = 9)]
	public byte qamInspectionType { get; set; }

	[JsonProperty("qamInvParentQtyAccepted", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamInvParentQtyAccepted { get; set; }

	[JsonProperty("qamInvParentQtyToReturn", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamInvParentQtyToReturn { get; set; }

	[JsonProperty("qamInvParentQtyToScrap", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamInvParentQtyToScrap { get; set; }

	[JsonProperty("qamInvQuantityAccepted", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamInvQuantityAccepted { get; set; }

	[JsonProperty("qamInvQuantityToReturn", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamInvQuantityToReturn { get; set; }

	[JsonProperty("qamInvQuantityToScrap", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamInvQuantityToScrap { get; set; }

	[JsonProperty("qamInspectionComplete", Order = 16)]
	public bool qamInspectionComplete { get; set; }

	[JsonProperty("qamManualInspectionFinalized", Order = 17)]
	public bool qamManualInspectionFinalized { get; set; }

	[JsonProperty("qamPosted", Order = 18)]
	public bool qamPosted { get; set; }

	[JsonProperty("qamJobAssemblyID", Order = 19)]
	public int qamJobAssemblyID { get; set; }

	[JsonProperty("qamJobID", Order = 20)]
	[MaxLength(20)]
	public string qamJobID { get; set; }

	[JsonProperty("qamJobMaterialComponentID", Order = 21)]
	public int qamJobMaterialComponentID { get; set; }

	[JsonProperty("qamJobMaterialID", Order = 22)]
	public int qamJobMaterialID { get; set; }

	[JsonProperty("qamJobMatParentQtyAccepted", Order = 23)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamJobMatParentQtyAccepted { get; set; }

	[JsonProperty("qamJobMatParentQtyToReturn", Order = 24)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamJobMatParentQtyToReturn { get; set; }

	[JsonProperty("qamJobMatParentQtyToScrap", Order = 25)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamJobMatParentQtyToScrap { get; set; }

	[JsonProperty("qamJobMatQuantityAccepted", Order = 26)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamJobMatQuantityAccepted { get; set; }

	[JsonProperty("qamJobMatQuantityToReturn", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamJobMatQuantityToReturn { get; set; }

	[JsonProperty("qamJobMatQuantityToScrap", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamJobMatQuantityToScrap { get; set; }

	[JsonProperty("qamParentQtyToInspect", Order = 29)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamParentQtyToInspect { get; set; }

	[JsonProperty("qamPartBinID", Order = 30)]
	[Required(ErrorMessage = "qamPartBinID is required.")]
	[MaxLength(15)]
	public string qamPartBinID { get; set; }

	[JsonProperty("qamPartID", Order = 31)]
	[Required(ErrorMessage = "qamPartID is required.")]
	[MaxLength(30)]
	public string qamPartID { get; set; }

	[JsonProperty("qamPartRevisionID", Order = 32)]
	[MaxLength(15)]
	public string qamPartRevisionID { get; set; }

	[JsonProperty("qamPartWarehouseLocationID", Order = 33)]
	[Required(ErrorMessage = "qamPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string qamPartWarehouseLocationID { get; set; }

	[JsonProperty("qamQuantityPerParent", Order = 34)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamQuantityPerParent { get; set; }

	[JsonProperty("qamInspectionComponentID", Order = 35)]
	[Required(ErrorMessage = "qamInspectionComponentID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int qamInspectionComponentID { get; set; }

	[JsonProperty("qamSourceTableName", Order = 36)]
	[MaxLength(30)]
	public string qamSourceTableName { get; set; }

	[JsonProperty("qamSourceTableUniqueID", Order = 37)]
	public Guid qamSourceTableUniqueID { get; set; }

	[JsonProperty("qamUnitOfMeasure", Order = 38)]
	[MaxLength(2)]
	public string qamUnitOfMeasure { get; set; }

	[JsonProperty("qamWeight", Order = 39)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qamWeight { get; set; }

	[JsonProperty("customFields", Order = 40)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
