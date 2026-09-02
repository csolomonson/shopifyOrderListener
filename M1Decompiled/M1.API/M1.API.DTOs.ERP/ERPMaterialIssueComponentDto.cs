using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMaterialIssueComponentDto
{
	[JsonProperty("inkAdditionalQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkAdditionalQuantity { get; set; }

	[JsonProperty("inkCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string inkCreatedBy { get; set; }

	[JsonProperty("inkCreatedDate", Order = 3)]
	public DateTime? inkCreatedDate { get; set; }

	[JsonProperty("inkDescription", Order = 4)]
	[MaxLength(50)]
	public string inkDescription { get; set; }

	[JsonProperty("inkUniqueID", Order = 5)]
	public Guid inkUniqueID { get; set; }

	[JsonProperty("inkInvIssueQuantity", Order = 6)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkInvIssueQuantity { get; set; }

	[JsonProperty("inkInvParentQuantity", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkInvParentQuantity { get; set; }

	[JsonProperty("inkInvParentQuantityScrap", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkInvParentQuantityScrap { get; set; }

	[JsonProperty("inkInvScrapQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkInvScrapQuantity { get; set; }

	[JsonProperty("inkPosted", Order = 10)]
	public bool inkPosted { get; set; }

	[JsonProperty("inkReceivedComplete", Order = 11)]
	public bool inkReceivedComplete { get; set; }

	[JsonProperty("inkReversed", Order = 12)]
	public bool inkReversed { get; set; }

	[JsonProperty("inkJobAssemblyID", Order = 13)]
	public int inkJobAssemblyID { get; set; }

	[JsonProperty("inkJobID", Order = 14)]
	[MaxLength(20)]
	public string inkJobID { get; set; }

	[JsonProperty("inkJobMaterialComponentID", Order = 15)]
	public int inkJobMaterialComponentID { get; set; }

	[JsonProperty("inkJobMaterialID", Order = 16)]
	public int inkJobMaterialID { get; set; }

	[JsonProperty("inkJobMatIssueQuantity", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkJobMatIssueQuantity { get; set; }

	[JsonProperty("inkJobMatParentQuantity", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkJobMatParentQuantity { get; set; }

	[JsonProperty("inkJobMatParentQuantityScrap", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkJobMatParentQuantityScrap { get; set; }

	[JsonProperty("inkJobMatParentReturnQty", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkJobMatParentReturnQty { get; set; }

	[JsonProperty("inkJobMatParentReturnQtyScrap", Order = 21)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkJobMatParentReturnQtyScrap { get; set; }

	[JsonProperty("inkJobMatReturnIssueQuantity", Order = 22)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkJobMatReturnIssueQuantity { get; set; }

	[JsonProperty("inkJobMatReturnScrapQuantity", Order = 23)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkJobMatReturnScrapQuantity { get; set; }

	[JsonProperty("inkJobMatScrapQuantity", Order = 24)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkJobMatScrapQuantity { get; set; }

	[JsonProperty("inkMaterialIssueID", Order = 25)]
	[Required(ErrorMessage = "inkMaterialIssueID is required.")]
	[MaxLength(10)]
	public string inkMaterialIssueID { get; set; }

	[JsonProperty("inkMaterialIssueLineID", Order = 26)]
	[Required(ErrorMessage = "inkMaterialIssueLineID is required.")]
	public short inkMaterialIssueLineID { get; set; }

	[JsonProperty("inkPartBinID", Order = 27)]
	[Required(ErrorMessage = "inkPartBinID is required.")]
	[MaxLength(15)]
	public string inkPartBinID { get; set; }

	[JsonProperty("inkPartID", Order = 28)]
	[Required(ErrorMessage = "inkPartID is required.")]
	[MaxLength(30)]
	public string inkPartID { get; set; }

	[JsonProperty("inkPartRevisionID", Order = 29)]
	[MaxLength(15)]
	public string inkPartRevisionID { get; set; }

	[JsonProperty("inkPartWarehouseLocationID", Order = 30)]
	[Required(ErrorMessage = "inkPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string inkPartWarehouseLocationID { get; set; }

	[JsonProperty("inkQuantityPerParent", Order = 31)]
	[Range(0.0, 9999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkQuantityPerParent { get; set; }

	[JsonProperty("inkReverseMaterialIssueCompID", Order = 32)]
	public int inkReverseMaterialIssueCompID { get; set; }

	[JsonProperty("inkReverseMaterialIssueID", Order = 33)]
	[MaxLength(10)]
	public string inkReverseMaterialIssueID { get; set; }

	[JsonProperty("inkReverseMaterialIssueLineID", Order = 34)]
	public short inkReverseMaterialIssueLineID { get; set; }

	[JsonProperty("inkRowVersion", Order = 35)]
	public byte[] inkRowVersion { get; set; }

	[JsonProperty("inkMaterialIssueComponentID", Order = 36)]
	[Required(ErrorMessage = "inkMaterialIssueComponentID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int inkMaterialIssueComponentID { get; set; }

	[JsonProperty("inkUnitOfMeasure", Order = 37)]
	[MaxLength(2)]
	public string inkUnitOfMeasure { get; set; }

	[JsonProperty("inkWeight", Order = 38)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inkWeight { get; set; }

	[JsonProperty("customFields", Order = 39)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
