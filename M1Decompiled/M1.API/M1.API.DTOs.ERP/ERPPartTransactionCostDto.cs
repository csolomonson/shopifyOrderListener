using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartTransactionCostDto
{
	[JsonProperty("intActualUnitDutyCost", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intActualUnitDutyCost { get; set; }

	[JsonProperty("intActualUnitFreightCost", Order = 2)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intActualUnitFreightCost { get; set; }

	[JsonProperty("intActualUnitLaborCost", Order = 3)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intActualUnitLaborCost { get; set; }

	[JsonProperty("intActualUnitMaterialCost", Order = 4)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intActualUnitMaterialCost { get; set; }

	[JsonProperty("intActualUnitMiscCost", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intActualUnitMiscCost { get; set; }

	[JsonProperty("intActualUnitOverheadCost", Order = 6)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intActualUnitOverheadCost { get; set; }

	[JsonProperty("intActualUnitSubcontractCost", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intActualUnitSubcontractCost { get; set; }

	[JsonProperty("intCostType", Order = 8)]
	public byte intCostType { get; set; }

	[JsonProperty("intCreatedBy", Order = 9)]
	[MaxLength(20)]
	public string intCreatedBy { get; set; }

	[JsonProperty("intCreatedDate", Order = 10)]
	public DateTime? intCreatedDate { get; set; }

	[JsonProperty("intUniqueID", Order = 11)]
	public Guid intUniqueID { get; set; }

	[JsonProperty("intPartTransactionID", Order = 12)]
	[Required(ErrorMessage = "intPartTransactionID is required.")]
	public int intPartTransactionID { get; set; }

	[JsonProperty("intPrevUnitDutyCost", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intPrevUnitDutyCost { get; set; }

	[JsonProperty("intPrevUnitFreightCost", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intPrevUnitFreightCost { get; set; }

	[JsonProperty("intPrevUnitLaborCost", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intPrevUnitLaborCost { get; set; }

	[JsonProperty("intPrevUnitMaterialCost", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intPrevUnitMaterialCost { get; set; }

	[JsonProperty("intPrevUnitMiscCost", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intPrevUnitMiscCost { get; set; }

	[JsonProperty("intPrevUnitOverheadCost", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intPrevUnitOverheadCost { get; set; }

	[JsonProperty("intPrevUnitSubcontractCost", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intPrevUnitSubcontractCost { get; set; }

	[JsonProperty("intQuantity", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intQuantity { get; set; }

	[JsonProperty("intRowVersion", Order = 21)]
	public byte[] intRowVersion { get; set; }

	[JsonProperty("intPartTransactionCostID", Order = 22)]
	[Required(ErrorMessage = "intPartTransactionCostID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int intPartTransactionCostID { get; set; }

	[JsonProperty("intSourceTableName", Order = 23)]
	[MaxLength(30)]
	public string intSourceTableName { get; set; }

	[JsonProperty("intSourceTableUniqueID", Order = 24)]
	public Guid intSourceTableUniqueID { get; set; }

	[JsonProperty("intUnitDutyCost", Order = 25)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intUnitDutyCost { get; set; }

	[JsonProperty("intUnitFreightCost", Order = 26)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intUnitFreightCost { get; set; }

	[JsonProperty("intUnitLaborCost", Order = 27)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intUnitLaborCost { get; set; }

	[JsonProperty("intUnitMaterialCost", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intUnitMaterialCost { get; set; }

	[JsonProperty("intUnitMiscCost", Order = 29)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intUnitMiscCost { get; set; }

	[JsonProperty("intUnitOverheadCost", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intUnitOverheadCost { get; set; }

	[JsonProperty("intUnitSubcontractCost", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal intUnitSubcontractCost { get; set; }

	[JsonProperty("customFields", Order = 32)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
