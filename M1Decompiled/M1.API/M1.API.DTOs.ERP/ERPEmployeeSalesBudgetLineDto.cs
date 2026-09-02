using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeeSalesBudgetLineDto
{
	[JsonProperty("lnlBudgetAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lnlBudgetAmount { get; set; }

	[JsonProperty("lnlEmployeeID", Order = 2)]
	[Required(ErrorMessage = "lnlEmployeeID is required.")]
	[MaxLength(10)]
	public string lnlEmployeeID { get; set; }

	[JsonProperty("lnlEndDate", Order = 3)]
	public DateTime? lnlEndDate { get; set; }

	[JsonProperty("lnlUniqueID", Order = 4)]
	public Guid lnlUniqueID { get; set; }

	[JsonProperty("lnlRowVersion", Order = 5)]
	public byte[] lnlRowVersion { get; set; }

	[JsonProperty("lnlSalesBudgetPeriodID", Order = 6)]
	[Required(ErrorMessage = "lnlSalesBudgetPeriodID is required.")]
	public short lnlSalesBudgetPeriodID { get; set; }

	[JsonProperty("lnlSalesBudgetYearID", Order = 7)]
	[Required(ErrorMessage = "lnlSalesBudgetYearID is required.")]
	public short lnlSalesBudgetYearID { get; set; }

	[JsonProperty("lnlStartDate", Order = 8)]
	public DateTime? lnlStartDate { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
