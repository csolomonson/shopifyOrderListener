using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLandedCostChargeDetailDto
{
	[JsonProperty("rmiCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string rmiCreatedBy { get; set; }

	[JsonProperty("rmiCreatedDate", Order = 2)]
	public DateTime? rmiCreatedDate { get; set; }

	[JsonProperty("rmiUniqueID", Order = 3)]
	public Guid rmiUniqueID { get; set; }

	[JsonProperty("rmiEstTotalCost", Order = 4)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmiEstTotalCost { get; set; }

	[JsonProperty("rmiEstTotalCostForeign", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmiEstTotalCostForeign { get; set; }

	[JsonProperty("rmiLandedCostChargeID", Order = 6)]
	[Required(ErrorMessage = "rmiLandedCostChargeID is required.")]
	public short rmiLandedCostChargeID { get; set; }

	[JsonProperty("rmiLandedCostID", Order = 7)]
	[Required(ErrorMessage = "rmiLandedCostID is required.")]
	[MaxLength(10)]
	public string rmiLandedCostID { get; set; }

	[JsonProperty("rmiPurchaseOrderID", Order = 8)]
	[MaxLength(10)]
	public string rmiPurchaseOrderID { get; set; }

	[JsonProperty("rmiPurchaseOrderLineID", Order = 9)]
	public short rmiPurchaseOrderLineID { get; set; }

	[JsonProperty("rmiRowVersion", Order = 10)]
	public byte[] rmiRowVersion { get; set; }

	[JsonProperty("rmiLandedCostChargeDetailID", Order = 11)]
	[Required(ErrorMessage = "rmiLandedCostChargeDetailID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int rmiLandedCostChargeDetailID { get; set; }

	[JsonProperty("rmiTotalCost", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmiTotalCost { get; set; }

	[JsonProperty("rmiTotalCostForeign", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmiTotalCostForeign { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
