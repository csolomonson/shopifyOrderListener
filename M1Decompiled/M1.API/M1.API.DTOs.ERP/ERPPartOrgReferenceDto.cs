using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartOrgReferenceDto
{
	[JsonProperty("imzConversionFactor", Order = 1)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imzConversionFactor { get; set; }

	[JsonProperty("imzCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string imzCreatedBy { get; set; }

	[JsonProperty("imzCreatedDate", Order = 3)]
	public DateTime? imzCreatedDate { get; set; }

	[JsonProperty("imzUniqueID", Order = 4)]
	public Guid imzUniqueID { get; set; }

	[JsonProperty("imzInactive", Order = 5)]
	public bool imzInactive { get; set; }

	[JsonProperty("imzPurchased", Order = 6)]
	public bool imzPurchased { get; set; }

	[JsonProperty("imzSold", Order = 7)]
	public bool imzSold { get; set; }

	[JsonProperty("imzLeadTime", Order = 8)]
	public short imzLeadTime { get; set; }

	[JsonProperty("imzLotSize", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imzLotSize { get; set; }

	[JsonProperty("imzMinimumPurchaseQuantity", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imzMinimumPurchaseQuantity { get; set; }

	[JsonProperty("imzOrganizationID", Order = 11)]
	[Required(ErrorMessage = "imzOrganizationID is required.")]
	[MaxLength(10)]
	public string imzOrganizationID { get; set; }

	[JsonProperty("imzOrgPartID", Order = 12)]
	[MaxLength(30)]
	public string imzOrgPartID { get; set; }

	[JsonProperty("imzOrgPartShortDescription", Order = 13)]
	[MaxLength(50)]
	public string imzOrgPartShortDescription { get; set; }

	[JsonProperty("imzPartID", Order = 14)]
	[Required(ErrorMessage = "imzPartID is required.")]
	[MaxLength(30)]
	public string imzPartID { get; set; }

	[JsonProperty("imzPartRevisionID", Order = 15)]
	[MaxLength(15)]
	public string imzPartRevisionID { get; set; }

	[JsonProperty("imzPurchaseUnitOfMeasure", Order = 16)]
	[MaxLength(2)]
	public string imzPurchaseUnitOfMeasure { get; set; }

	[JsonProperty("imzRowVersion", Order = 17)]
	public byte[] imzRowVersion { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
