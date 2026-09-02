using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartPriceDto
{
	[JsonProperty("imiCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string imiCreatedBy { get; set; }

	[JsonProperty("imiCreatedDate", Order = 2)]
	public DateTime? imiCreatedDate { get; set; }

	[JsonProperty("imiCurrencyRateID", Order = 3)]
	[MaxLength(5)]
	public string imiCurrencyRateID { get; set; }

	[JsonProperty("imiCustomerGroupID", Order = 4)]
	[MaxLength(5)]
	public string imiCustomerGroupID { get; set; }

	[JsonProperty("imiEndDate", Order = 5)]
	public DateTime? imiEndDate { get; set; }

	[JsonProperty("imiUniqueID", Order = 6)]
	public Guid imiUniqueID { get; set; }

	[JsonProperty("imiInventoryPrice", Order = 7)]
	public bool imiInventoryPrice { get; set; }

	[JsonProperty("imiLocationID", Order = 8)]
	[MaxLength(5)]
	public string imiLocationID { get; set; }

	[JsonProperty("imiOrganizationID", Order = 9)]
	[MaxLength(10)]
	public string imiOrganizationID { get; set; }

	[JsonProperty("imiPartGroupID", Order = 10)]
	[MaxLength(5)]
	public string imiPartGroupID { get; set; }

	[JsonProperty("imiPartID", Order = 11)]
	[MaxLength(30)]
	public string imiPartID { get; set; }

	[JsonProperty("imiPartRevisionID", Order = 12)]
	[MaxLength(15)]
	public string imiPartRevisionID { get; set; }

	[JsonProperty("imiPriceType", Order = 13)]
	[Required(ErrorMessage = "imiPriceType is required.")]
	public byte imiPriceType { get; set; }

	[JsonProperty("imiQuoteID", Order = 14)]
	[MaxLength(10)]
	public string imiQuoteID { get; set; }

	[JsonProperty("imiRfqID", Order = 15)]
	[MaxLength(10)]
	public string imiRfqID { get; set; }

	[JsonProperty("imiRowVersion", Order = 16)]
	public byte[] imiRowVersion { get; set; }

	[JsonProperty("imiPartPriceID", Order = 17)]
	[Required(ErrorMessage = "imiPartPriceID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imiPartPriceID { get; set; }

	[JsonProperty("imiStartDate", Order = 18)]
	public DateTime? imiStartDate { get; set; }

	[JsonProperty("customFields", Order = 19)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
