using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCurrencyRateDto
{
	[JsonProperty("mcpApGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string mcpApGlAccountID { get; set; }

	[JsonProperty("mcpArGlAccountID", Order = 2)]
	[MaxLength(11)]
	public string mcpArGlAccountID { get; set; }

	[JsonProperty("mcpCurrencyRateID", Order = 3)]
	[Required(ErrorMessage = "mcpCurrencyRateID is required.")]
	[MaxLength(5)]
	public string mcpCurrencyRateID { get; set; }

	[JsonProperty("mcpCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string mcpCreatedBy { get; set; }

	[JsonProperty("mcpCreatedDate", Order = 5)]
	public DateTime? mcpCreatedDate { get; set; }

	[JsonProperty("mcpDescription", Order = 6)]
	[Required(ErrorMessage = "mcpDescription is required.")]
	[MaxLength(50)]
	public string mcpDescription { get; set; }

	[JsonProperty("mcpUniqueID", Order = 7)]
	public Guid mcpUniqueID { get; set; }

	[JsonProperty("mcpExchangeGainGlAccountID", Order = 8)]
	[MaxLength(11)]
	public string mcpExchangeGainGlAccountID { get; set; }

	[JsonProperty("mcpExchangeLossGlAccountID", Order = 9)]
	[MaxLength(11)]
	public string mcpExchangeLossGlAccountID { get; set; }

	[JsonProperty("mcpRowVersion", Order = 10)]
	public byte[] mcpRowVersion { get; set; }

	[JsonProperty("mcpSymbol", Order = 11)]
	[Required(ErrorMessage = "mcpSymbol is required.")]
	[MaxLength(4)]
	public string mcpSymbol { get; set; }

	[JsonProperty("mcpUnrealisedExGainGlAccountID", Order = 12)]
	[MaxLength(11)]
	public string mcpUnrealisedExGainGlAccountID { get; set; }

	[JsonProperty("mcpUnrealisedExLossGlAccountID", Order = 13)]
	[MaxLength(11)]
	public string mcpUnrealisedExLossGlAccountID { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
