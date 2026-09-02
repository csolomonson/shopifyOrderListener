using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLaserCalculatorDto
{
	[JsonProperty("ccpLaserCalculatorID", Order = 1)]
	[Required(ErrorMessage = "ccpLaserCalculatorID is required.")]
	public Guid ccpLaserCalculatorID { get; set; }

	[JsonProperty("ccpCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string ccpCreatedBy { get; set; }

	[JsonProperty("ccpCreatedDate", Order = 3)]
	public DateTime? ccpCreatedDate { get; set; }

	[JsonProperty("ccpdescription", Order = 4)]
	[MaxLength(30)]
	public string ccpdescription { get; set; }

	[JsonProperty("ccpUniqueID", Order = 5)]
	public Guid ccpUniqueID { get; set; }

	[JsonProperty("ccpExternalFeed", Order = 6)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpExternalFeed { get; set; }

	[JsonProperty("ccpHoleCutTime", Order = 7)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpHoleCutTime { get; set; }

	[JsonProperty("ccpObround", Order = 8)]
	public bool ccpObround { get; set; }

	[JsonProperty("ccpOther", Order = 9)]
	public bool ccpOther { get; set; }

	[JsonProperty("ccpRectangle", Order = 10)]
	public bool ccpRectangle { get; set; }

	[JsonProperty("ccpRound", Order = 11)]
	public bool ccpRound { get; set; }

	[JsonProperty("ccpSquare", Order = 12)]
	public bool ccpSquare { get; set; }

	[JsonProperty("ccpLaserMaterialTypeID", Order = 13)]
	[MaxLength(10)]
	public string ccpLaserMaterialTypeID { get; set; }

	[JsonProperty("ccpLeadInOut", Order = 14)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpLeadInOut { get; set; }

	[JsonProperty("ccpLeadInOutFeed", Order = 15)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpLeadInOutFeed { get; set; }

	[JsonProperty("ccpLeadInOutTime", Order = 16)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpLeadInOutTime { get; set; }

	[JsonProperty("ccplength", Order = 17)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccplength { get; set; }

	[JsonProperty("ccpMeasurementType", Order = 18)]
	[MaxLength(1)]
	public string ccpMeasurementType { get; set; }

	[JsonProperty("ccpNumberOfHoles", Order = 19)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccpNumberOfHoles { get; set; }

	[JsonProperty("ccpPartPerimeter", Order = 20)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpPartPerimeter { get; set; }

	[JsonProperty("ccpPerimeterCutTime", Order = 21)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpPerimeterCutTime { get; set; }

	[JsonProperty("ccpPiercedHoles", Order = 22)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpPiercedHoles { get; set; }

	[JsonProperty("ccpPierceTime", Order = 23)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpPierceTime { get; set; }

	[JsonProperty("ccpQuantity", Order = 24)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpQuantity { get; set; }

	[JsonProperty("ccpRate", Order = 25)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpRate { get; set; }

	[JsonProperty("ccpRowVersion", Order = 26)]
	public byte[] ccpRowVersion { get; set; }

	[JsonProperty("ccpThickness", Order = 27)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpThickness { get; set; }

	[JsonProperty("ccpTotalCutTime", Order = 28)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpTotalCutTime { get; set; }

	[JsonProperty("ccpTotalLeadInOutTime", Order = 29)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpTotalLeadInOutTime { get; set; }

	[JsonProperty("ccpTotalPierceTime", Order = 30)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpTotalPierceTime { get; set; }

	[JsonProperty("ccpWidth", Order = 31)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccpWidth { get; set; }

	[JsonProperty("customFields", Order = 32)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
