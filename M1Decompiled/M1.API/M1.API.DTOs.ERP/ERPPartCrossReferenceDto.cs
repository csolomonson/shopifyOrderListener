using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartCrossReferenceDto
{
	[JsonProperty("imxConversionFactor", Order = 1)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imxConversionFactor { get; set; }

	[JsonProperty("imxCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string imxCreatedBy { get; set; }

	[JsonProperty("imxCreatedDate", Order = 3)]
	public DateTime? imxCreatedDate { get; set; }

	[JsonProperty("imxUniqueID", Order = 4)]
	public Guid imxUniqueID { get; set; }

	[JsonProperty("imxInactive", Order = 5)]
	public bool imxInactive { get; set; }

	[JsonProperty("imxPurchased", Order = 6)]
	public bool imxPurchased { get; set; }

	[JsonProperty("imxSold", Order = 7)]
	public bool imxSold { get; set; }

	[JsonProperty("imxLeadTime", Order = 8)]
	public short imxLeadTime { get; set; }

	[JsonProperty("imxLocationID", Order = 9)]
	[Required(ErrorMessage = "imxLocationID is required.")]
	[MaxLength(5)]
	public string imxLocationID { get; set; }

	[JsonProperty("imxLotSize", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imxLotSize { get; set; }

	[JsonProperty("imxMinimumPurchaseQuantity", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imxMinimumPurchaseQuantity { get; set; }

	[JsonProperty("imxOrganizationID", Order = 12)]
	[Required(ErrorMessage = "imxOrganizationID is required.")]
	[MaxLength(10)]
	public string imxOrganizationID { get; set; }

	[JsonProperty("imxOrgPartID", Order = 13)]
	[MaxLength(30)]
	public string imxOrgPartID { get; set; }

	[JsonProperty("imxOrgPartShortDescription", Order = 14)]
	[MaxLength(50)]
	public string imxOrgPartShortDescription { get; set; }

	[JsonProperty("imxPartID", Order = 15)]
	[Required(ErrorMessage = "imxPartID is required.")]
	[MaxLength(30)]
	public string imxPartID { get; set; }

	[JsonProperty("imxPartRevisionID", Order = 16)]
	[MaxLength(15)]
	public string imxPartRevisionID { get; set; }

	[JsonProperty("imxPurchaseUnitOfMeasure", Order = 17)]
	[MaxLength(2)]
	public string imxPurchaseUnitOfMeasure { get; set; }

	[JsonProperty("imxRowVersion", Order = 18)]
	public byte[] imxRowVersion { get; set; }

	[JsonProperty("customFields", Order = 19)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
