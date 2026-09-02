using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeeSalesBudgetDto
{
	[JsonProperty("lnsAnnualAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lnsAnnualAmount { get; set; }

	[JsonProperty("lnsEmployeeID", Order = 2)]
	[Required(ErrorMessage = "lnsEmployeeID is required.")]
	[MaxLength(10)]
	public string lnsEmployeeID { get; set; }

	[JsonProperty("lnsEndDate", Order = 3)]
	public DateTime? lnsEndDate { get; set; }

	[JsonProperty("lnsUniqueID", Order = 4)]
	public Guid lnsUniqueID { get; set; }

	[JsonProperty("lnsRowVersion", Order = 5)]
	public byte[] lnsRowVersion { get; set; }

	[JsonProperty("lnsSalesBudgetYearID", Order = 6)]
	[Required(ErrorMessage = "lnsSalesBudgetYearID is required.")]
	public short lnsSalesBudgetYearID { get; set; }

	[JsonProperty("lnsStartDate", Order = 7)]
	public DateTime? lnsStartDate { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
