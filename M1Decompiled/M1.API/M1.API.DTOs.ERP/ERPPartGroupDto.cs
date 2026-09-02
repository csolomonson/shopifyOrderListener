using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartGroupDto
{
	[JsonProperty("imuArDepositGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string imuArDepositGlAccountID { get; set; }

	[JsonProperty("imuAvalaraTaxCodeID", Order = 2)]
	[MaxLength(10)]
	public string imuAvalaraTaxCodeID { get; set; }

	[JsonProperty("imuPartGroupID", Order = 3)]
	[Required(ErrorMessage = "imuPartGroupID is required.")]
	[MaxLength(5)]
	public string imuPartGroupID { get; set; }

	[JsonProperty("imuCogsLaborGlAccountID", Order = 4)]
	[MaxLength(11)]
	public string imuCogsLaborGlAccountID { get; set; }

	[JsonProperty("imuCogsMaterialGlAccountID", Order = 5)]
	[MaxLength(11)]
	public string imuCogsMaterialGlAccountID { get; set; }

	[JsonProperty("imuCogsOverheadGlAccountID", Order = 6)]
	[MaxLength(11)]
	public string imuCogsOverheadGlAccountID { get; set; }

	[JsonProperty("imuCogsSubcontractGlAccountID", Order = 7)]
	[MaxLength(11)]
	public string imuCogsSubcontractGlAccountID { get; set; }

	[JsonProperty("imuCommissionRate", Order = 8)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imuCommissionRate { get; set; }

	[JsonProperty("imuCommissionType", Order = 9)]
	[Required(ErrorMessage = "imuCommissionType is required.")]
	public byte imuCommissionType { get; set; }

	[JsonProperty("imuCreatedBy", Order = 10)]
	[MaxLength(20)]
	public string imuCreatedBy { get; set; }

	[JsonProperty("imuCreatedDate", Order = 11)]
	public DateTime? imuCreatedDate { get; set; }

	[JsonProperty("imuDescription", Order = 12)]
	[Required(ErrorMessage = "imuDescription is required.")]
	[MaxLength(50)]
	public string imuDescription { get; set; }

	[JsonProperty("imuDiscountGlAccountID", Order = 13)]
	[MaxLength(11)]
	public string imuDiscountGlAccountID { get; set; }

	[JsonProperty("imuUniqueID", Order = 14)]
	public Guid imuUniqueID { get; set; }

	[JsonProperty("imuInactiveDate", Order = 15)]
	public DateTime? imuInactiveDate { get; set; }

	[JsonProperty("imuInactive", Order = 16)]
	public bool imuInactive { get; set; }

	[JsonProperty("imuNextSerialNumberIDFormula", Order = 17)]
	[MaxLength(50)]
	public string imuNextSerialNumberIDFormula { get; set; }

	[JsonProperty("imuNextSerialNumberOption", Order = 18)]
	public byte imuNextSerialNumberOption { get; set; }

	[JsonProperty("imuNextSerialNumberValue", Order = 19)]
	[MaxLength(30)]
	public string imuNextSerialNumberValue { get; set; }

	[JsonProperty("imuParentPartGroupID", Order = 20)]
	[MaxLength(5)]
	public string imuParentPartGroupID { get; set; }

	[JsonProperty("imuPartImageFileName", Order = 21)]
	[MaxLength(70)]
	public string imuPartImageFileName { get; set; }

	[JsonProperty("imuQmLaborMarkup", Order = 22)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imuQmLaborMarkup { get; set; }

	[JsonProperty("imuQmMarkupOption", Order = 23)]
	public byte imuQmMarkupOption { get; set; }

	[JsonProperty("imuQmMaterialMarkup", Order = 24)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imuQmMaterialMarkup { get; set; }

	[JsonProperty("imuQmOverHeadMarkup", Order = 25)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imuQmOverHeadMarkup { get; set; }

	[JsonProperty("imuQmPurchaseToOrderMarkup", Order = 26)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imuQmPurchaseToOrderMarkup { get; set; }

	[JsonProperty("imuQmQuoteMarkupType", Order = 27)]
	public byte imuQmQuoteMarkupType { get; set; }

	[JsonProperty("imuQmQuotingMarkup", Order = 28)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imuQmQuotingMarkup { get; set; }

	[JsonProperty("imuQmSubcontractMarkup", Order = 29)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imuQmSubcontractMarkup { get; set; }

	[JsonProperty("imuRowVersion", Order = 30)]
	public byte[] imuRowVersion { get; set; }

	[JsonProperty("imuSalesGlAccountID", Order = 31)]
	[MaxLength(11)]
	public string imuSalesGlAccountID { get; set; }

	[JsonProperty("customFields", Order = 32)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
