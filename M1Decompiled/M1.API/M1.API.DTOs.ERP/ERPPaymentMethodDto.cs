using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPaymentMethodDto
{
	[JsonProperty("xahArPaymentSessionRule", Order = 1)]
	public byte xahArPaymentSessionRule { get; set; }

	[JsonProperty("xahBankAccountID", Order = 2)]
	[MaxLength(5)]
	public string xahBankAccountID { get; set; }

	[JsonProperty("xahPaymentMethodID", Order = 3)]
	[Required(ErrorMessage = "xahPaymentMethodID is required.")]
	[MaxLength(5)]
	public string xahPaymentMethodID { get; set; }

	[JsonProperty("xahCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string xahCreatedBy { get; set; }

	[JsonProperty("xahCreatedDate", Order = 5)]
	public DateTime? xahCreatedDate { get; set; }

	[JsonProperty("xahDescription", Order = 6)]
	[Required(ErrorMessage = "xahDescription is required.")]
	[MaxLength(50)]
	public string xahDescription { get; set; }

	[JsonProperty("xahUniqueID", Order = 7)]
	public Guid xahUniqueID { get; set; }

	[JsonProperty("xahInactiveDate", Order = 8)]
	public DateTime? xahInactiveDate { get; set; }

	[JsonProperty("xahInactive", Order = 9)]
	public bool xahInactive { get; set; }

	[JsonProperty("xahDoNotOpenCashDrawer", Order = 10)]
	public bool xahDoNotOpenCashDrawer { get; set; }

	[JsonProperty("xahPmAmex", Order = 11)]
	public bool xahPmAmex { get; set; }

	[JsonProperty("xahPmCash", Order = 12)]
	public bool xahPmCash { get; set; }

	[JsonProperty("xahPmCheck", Order = 13)]
	public bool xahPmCheck { get; set; }

	[JsonProperty("xahPmDiners", Order = 14)]
	public bool xahPmDiners { get; set; }

	[JsonProperty("xahPmDiscover", Order = 15)]
	public bool xahPmDiscover { get; set; }

	[JsonProperty("xahPmEnroute", Order = 16)]
	public bool xahPmEnroute { get; set; }

	[JsonProperty("xahPmJAL", Order = 17)]
	public bool xahPmJAL { get; set; }

	[JsonProperty("xahPmJCB", Order = 18)]
	public bool xahPmJCB { get; set; }

	[JsonProperty("xahPmMasterCard", Order = 19)]
	public bool xahPmMasterCard { get; set; }

	[JsonProperty("xahPmPurchaseOrder", Order = 20)]
	public bool xahPmPurchaseOrder { get; set; }

	[JsonProperty("xahPmStoreCredit", Order = 21)]
	public bool xahPmStoreCredit { get; set; }

	[JsonProperty("xahPmVisa", Order = 22)]
	public bool xahPmVisa { get; set; }

	[JsonProperty("xahRefundPriority", Order = 23)]
	public byte xahRefundPriority { get; set; }

	[JsonProperty("xahRowVersion", Order = 24)]
	public byte[] xahRowVersion { get; set; }

	[JsonProperty("xahSettlementTime", Order = 25)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xahSettlementTime { get; set; }

	[JsonProperty("customFields", Order = 26)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
