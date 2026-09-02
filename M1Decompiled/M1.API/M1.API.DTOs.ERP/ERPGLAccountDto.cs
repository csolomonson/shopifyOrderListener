using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLAccountDto
{
	[JsonProperty("glaGlAccountID", Order = 1)]
	[Required(ErrorMessage = "glaGlAccountID is required.")]
	[MaxLength(11)]
	public string glaGlAccountID { get; set; }

	[JsonProperty("glaCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string glaCreatedBy { get; set; }

	[JsonProperty("glaCreatedDate", Order = 3)]
	public DateTime? glaCreatedDate { get; set; }

	[JsonProperty("glaUniqueID", Order = 4)]
	public Guid glaUniqueID { get; set; }

	[JsonProperty("glaExternalGlCode", Order = 5)]
	[MaxLength(11)]
	public string glaExternalGlCode { get; set; }

	[JsonProperty("glaGlChartID", Order = 6)]
	[Required(ErrorMessage = "glaGlChartID is required.")]
	[MaxLength(5)]
	public string glaGlChartID { get; set; }

	[JsonProperty("glaGlDepartmentID", Order = 7)]
	[Required(ErrorMessage = "glaGlDepartmentID is required.")]
	[MaxLength(3)]
	public string glaGlDepartmentID { get; set; }

	[JsonProperty("glaGlDivisionID", Order = 8)]
	[Required(ErrorMessage = "glaGlDivisionID is required.")]
	[MaxLength(3)]
	public string glaGlDivisionID { get; set; }

	[JsonProperty("glaInactiveDate", Order = 9)]
	public DateTime? glaInactiveDate { get; set; }

	[JsonProperty("glaInactive", Order = 10)]
	public bool glaInactive { get; set; }

	[JsonProperty("glaRowVersion", Order = 11)]
	public byte[] glaRowVersion { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
