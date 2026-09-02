using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRFQQuantityDto
{
	[JsonProperty("rqqCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string rqqCreatedBy { get; set; }

	[JsonProperty("rqqCreatedDate", Order = 2)]
	public DateTime? rqqCreatedDate { get; set; }

	[JsonProperty("rqqUniqueID", Order = 3)]
	public Guid rqqUniqueID { get; set; }

	[JsonProperty("rqqClosed", Order = 4)]
	public bool rqqClosed { get; set; }

	[JsonProperty("rqqLeadTime", Order = 5)]
	public short rqqLeadTime { get; set; }

	[JsonProperty("rqqPriceBase", Order = 6)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rqqPriceBase { get; set; }

	[JsonProperty("rqqPriceForeign", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rqqPriceForeign { get; set; }

	[JsonProperty("rqqQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rqqQuantity { get; set; }

	[JsonProperty("rqqRfqID", Order = 9)]
	[Required(ErrorMessage = "rqqRfqID is required.")]
	[MaxLength(10)]
	public string rqqRfqID { get; set; }

	[JsonProperty("rqqRfqLineID", Order = 10)]
	[Required(ErrorMessage = "rqqRfqLineID is required.")]
	public short rqqRfqLineID { get; set; }

	[JsonProperty("rqqRfqSupplierID", Order = 11)]
	[Required(ErrorMessage = "rqqRfqSupplierID is required.")]
	public short rqqRfqSupplierID { get; set; }

	[JsonProperty("rqqRowVersion", Order = 12)]
	public byte[] rqqRowVersion { get; set; }

	[JsonProperty("rqqRfqQuantityID", Order = 13)]
	[Required(ErrorMessage = "rqqRfqQuantityID is required.")]
	public short rqqRfqQuantityID { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
