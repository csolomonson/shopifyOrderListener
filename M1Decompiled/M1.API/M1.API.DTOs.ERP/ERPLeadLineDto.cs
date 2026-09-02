using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLeadLineDto
{
	[JsonProperty("lolCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string lolCreatedBy { get; set; }

	[JsonProperty("lolCreatedDate", Order = 2)]
	public DateTime? lolCreatedDate { get; set; }

	[JsonProperty("lolCurrencyRateID", Order = 3)]
	[MaxLength(5)]
	public string lolCurrencyRateID { get; set; }

	[JsonProperty("lolDescription", Order = 4)]
	[Required(ErrorMessage = "lolDescription is required.")]
	[MaxLength(50)]
	public string lolDescription { get; set; }

	[JsonProperty("lolDiscountAmount", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolDiscountAmount { get; set; }

	[JsonProperty("lolDiscountAmountForeign", Order = 6)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolDiscountAmountForeign { get; set; }

	[JsonProperty("lolDiscountPercent", Order = 7)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolDiscountPercent { get; set; }

	[JsonProperty("lolUniqueID", Order = 8)]
	public Guid lolUniqueID { get; set; }

	[JsonProperty("lolExchangeRate", Order = 9)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolExchangeRate { get; set; }

	[JsonProperty("lolForecastDate", Order = 10)]
	public DateTime? lolForecastDate { get; set; }

	[JsonProperty("lolGrossAmount", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolGrossAmount { get; set; }

	[JsonProperty("lolGrossAmountForeign", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolGrossAmountForeign { get; set; }

	[JsonProperty("lolCreatedFromMobile", Order = 13)]
	public bool lolCreatedFromMobile { get; set; }

	[JsonProperty("lolCustomRate", Order = 14)]
	public bool lolCustomRate { get; set; }

	[JsonProperty("lolTransferredToQuote", Order = 15)]
	public bool lolTransferredToQuote { get; set; }

	[JsonProperty("lolLeadDate", Order = 16)]
	[Required(ErrorMessage = "lolLeadDate is required.")]
	public DateTime? lolLeadDate { get; set; }

	[JsonProperty("lolLeadID", Order = 17)]
	[Required(ErrorMessage = "lolLeadID is required.")]
	[MaxLength(10)]
	public string lolLeadID { get; set; }

	[JsonProperty("lolOrgPartID", Order = 18)]
	[MaxLength(30)]
	public string lolOrgPartID { get; set; }

	[JsonProperty("lolOrgPartShortDescription", Order = 19)]
	[MaxLength(50)]
	public string lolOrgPartShortDescription { get; set; }

	[JsonProperty("lolPartGroupID", Order = 20)]
	[MaxLength(5)]
	public string lolPartGroupID { get; set; }

	[JsonProperty("lolPartID", Order = 21)]
	[Required(ErrorMessage = "lolPartID is required.")]
	[MaxLength(30)]
	public string lolPartID { get; set; }

	[JsonProperty("lolPartPriceID", Order = 22)]
	public int lolPartPriceID { get; set; }

	[JsonProperty("lolPartRevisionID", Order = 23)]
	[MaxLength(15)]
	public string lolPartRevisionID { get; set; }

	[JsonProperty("lolQuantity", Order = 24)]
	[Required(ErrorMessage = "lolQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolQuantity { get; set; }

	[JsonProperty("lolResolutionReasonID", Order = 25)]
	[MaxLength(5)]
	public string lolResolutionReasonID { get; set; }

	[JsonProperty("lolRevenueForecast", Order = 26)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolRevenueForecast { get; set; }

	[JsonProperty("lolRevenueForecastForeign", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolRevenueForecastForeign { get; set; }

	[JsonProperty("lolRowVersion", Order = 28)]
	public byte[] lolRowVersion { get; set; }

	[JsonProperty("lolLeadLineID", Order = 29)]
	[Required(ErrorMessage = "lolLeadLineID is required.")]
	public short lolLeadLineID { get; set; }

	[JsonProperty("lolUnitOfMeasure", Order = 30)]
	[MaxLength(2)]
	public string lolUnitOfMeasure { get; set; }

	[JsonProperty("lolUnitSalePriceBase", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolUnitSalePriceBase { get; set; }

	[JsonProperty("lolUnitSalePriceForeign", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lolUnitSalePriceForeign { get; set; }

	[JsonProperty("customFields", Order = 33)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
