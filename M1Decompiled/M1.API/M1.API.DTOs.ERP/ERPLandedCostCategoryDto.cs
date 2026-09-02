using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLandedCostCategoryDto
{
	[JsonProperty("rmaCategoryType", Order = 1)]
	[Required(ErrorMessage = "rmaCategoryType is required.")]
	public byte rmaCategoryType { get; set; }

	[JsonProperty("rmaLandedCostCategoryID", Order = 2)]
	[Required(ErrorMessage = "rmaLandedCostCategoryID is required.")]
	[MaxLength(5)]
	public string rmaLandedCostCategoryID { get; set; }

	[JsonProperty("rmaCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string rmaCreatedBy { get; set; }

	[JsonProperty("rmaCreatedDate", Order = 4)]
	public DateTime? rmaCreatedDate { get; set; }

	[JsonProperty("rmaDescription", Order = 5)]
	[Required(ErrorMessage = "rmaDescription is required.")]
	[MaxLength(50)]
	public string rmaDescription { get; set; }

	[JsonProperty("rmaUniqueID", Order = 6)]
	public Guid rmaUniqueID { get; set; }

	[JsonProperty("rmaExpenseSplitPercentTotal", Order = 7)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmaExpenseSplitPercentTotal { get; set; }

	[JsonProperty("rmaDefault", Order = 8)]
	public bool rmaDefault { get; set; }

	[JsonProperty("rmaLandedCostMethod", Order = 9)]
	public byte rmaLandedCostMethod { get; set; }

	[JsonProperty("rmaRowVersion", Order = 10)]
	public byte[] rmaRowVersion { get; set; }

	[JsonProperty("rmaSupplierLocationID", Order = 11)]
	[MaxLength(5)]
	public string rmaSupplierLocationID { get; set; }

	[JsonProperty("rmaSupplierOrganizationID", Order = 12)]
	[MaxLength(10)]
	public string rmaSupplierOrganizationID { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
