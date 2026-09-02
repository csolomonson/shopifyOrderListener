using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShipmentFreightLinkDto
{
	[JsonProperty("smxCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string smxCreatedBy { get; set; }

	[JsonProperty("smxCreatedDate", Order = 2)]
	public DateTime? smxCreatedDate { get; set; }

	[JsonProperty("smxUniqueID", Order = 3)]
	public Guid smxUniqueID { get; set; }

	[JsonProperty("smxFreightCharges", Order = 4)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smxFreightCharges { get; set; }

	[JsonProperty("smxFreightPackageID", Order = 5)]
	[Required(ErrorMessage = "smxFreightPackageID is required.")]
	public short smxFreightPackageID { get; set; }

	[JsonProperty("smxFreightShipmentID", Order = 6)]
	[Required(ErrorMessage = "smxFreightShipmentID is required.")]
	[MaxLength(10)]
	public string smxFreightShipmentID { get; set; }

	[JsonProperty("smxClosed", Order = 7)]
	public bool smxClosed { get; set; }

	[JsonProperty("smxLinkPctCharge", Order = 8)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smxLinkPctCharge { get; set; }

	[JsonProperty("smxPackagePartialCount", Order = 9)]
	[Range(0.0, 999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smxPackagePartialCount { get; set; }

	[JsonProperty("smxPackagePartialWeight", Order = 10)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smxPackagePartialWeight { get; set; }

	[JsonProperty("smxRowVersion", Order = 11)]
	public byte[] smxRowVersion { get; set; }

	[JsonProperty("smxShipmentFreightLinkID", Order = 12)]
	[Required(ErrorMessage = "smxShipmentFreightLinkID is required.")]
	public short smxShipmentFreightLinkID { get; set; }

	[JsonProperty("smxShipmentID", Order = 13)]
	[Required(ErrorMessage = "smxShipmentID is required.")]
	[MaxLength(10)]
	public string smxShipmentID { get; set; }

	[JsonProperty("smxShipmentLineID", Order = 14)]
	[Required(ErrorMessage = "smxShipmentLineID is required.")]
	public short smxShipmentLineID { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
