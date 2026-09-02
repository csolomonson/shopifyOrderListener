using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartClassDto
{
	[JsonProperty("imcPartClassID", Order = 1)]
	[Required(ErrorMessage = "imcPartClassID is required.")]
	[MaxLength(5)]
	public string imcPartClassID { get; set; }

	[JsonProperty("imcCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string imcCreatedBy { get; set; }

	[JsonProperty("imcCreatedDate", Order = 3)]
	public DateTime? imcCreatedDate { get; set; }

	[JsonProperty("imcDescription", Order = 4)]
	[Required(ErrorMessage = "imcDescription is required.")]
	[MaxLength(50)]
	public string imcDescription { get; set; }

	[JsonProperty("imcUniqueID", Order = 5)]
	public Guid imcUniqueID { get; set; }

	[JsonProperty("imcFdxHandlingCost", Order = 6)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imcFdxHandlingCost { get; set; }

	[JsonProperty("imcFdxPackageHeight", Order = 7)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imcFdxPackageHeight { get; set; }

	[JsonProperty("imcFdxPackageLength", Order = 8)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imcFdxPackageLength { get; set; }

	[JsonProperty("imcFdxPackageWidth", Order = 9)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imcFdxPackageWidth { get; set; }

	[JsonProperty("imcFdxPackaging", Order = 10)]
	[MaxLength(14)]
	public string imcFdxPackaging { get; set; }

	[JsonProperty("imcFdxPackagingCost", Order = 11)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imcFdxPackagingCost { get; set; }

	[JsonProperty("imcFdxShipCostMarkupPct", Order = 12)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imcFdxShipCostMarkupPct { get; set; }

	[JsonProperty("imcInactiveDate", Order = 13)]
	public DateTime? imcInactiveDate { get; set; }

	[JsonProperty("imcInventoryGlAccountID", Order = 14)]
	[MaxLength(11)]
	public string imcInventoryGlAccountID { get; set; }

	[JsonProperty("imcInvInInspectionGlAccountID", Order = 15)]
	[MaxLength(11)]
	public string imcInvInInspectionGlAccountID { get; set; }

	[JsonProperty("imcInvInTransferGlAccountID", Order = 16)]
	[MaxLength(11)]
	public string imcInvInTransferGlAccountID { get; set; }

	[JsonProperty("imcInvToReturnGlAccountID", Order = 17)]
	[MaxLength(11)]
	public string imcInvToReturnGlAccountID { get; set; }

	[JsonProperty("imcInactive", Order = 18)]
	public bool imcInactive { get; set; }

	[JsonProperty("imcFdxNonstandardContainer", Order = 19)]
	public bool imcFdxNonstandardContainer { get; set; }

	[JsonProperty("imcFdxOneItemPerShipment", Order = 20)]
	public bool imcFdxOneItemPerShipment { get; set; }

	[JsonProperty("imcRequiresInspection", Order = 21)]
	public bool imcRequiresInspection { get; set; }

	[JsonProperty("imcParentPartClassID", Order = 22)]
	[MaxLength(5)]
	public string imcParentPartClassID { get; set; }

	[JsonProperty("imcPartImageFileName", Order = 23)]
	[MaxLength(70)]
	public string imcPartImageFileName { get; set; }

	[JsonProperty("imcPickingMethod", Order = 24)]
	[Required(ErrorMessage = "imcPickingMethod is required.")]
	public byte imcPickingMethod { get; set; }

	[JsonProperty("imcReorderMethod", Order = 25)]
	public byte imcReorderMethod { get; set; }

	[JsonProperty("imcRowVersion", Order = 26)]
	public byte[] imcRowVersion { get; set; }

	[JsonProperty("imcWeight", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imcWeight { get; set; }

	[JsonProperty("customFields", Order = 28)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
