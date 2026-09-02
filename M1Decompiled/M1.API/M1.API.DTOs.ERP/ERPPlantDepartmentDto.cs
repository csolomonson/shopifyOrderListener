using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPlantDepartmentDto
{
	[JsonProperty("xavApApGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string xavApApGlAccountID { get; set; }

	[JsonProperty("xavApBankAccountID", Order = 2)]
	[MaxLength(5)]
	public string xavApBankAccountID { get; set; }

	[JsonProperty("xavApCashGlAccountID", Order = 3)]
	[MaxLength(11)]
	public string xavApCashGlAccountID { get; set; }

	[JsonProperty("xavApDiscountGlAccountID", Order = 4)]
	[MaxLength(11)]
	public string xavApDiscountGlAccountID { get; set; }

	[JsonProperty("xavApFreightGlAccountID", Order = 5)]
	[MaxLength(11)]
	public string xavApFreightGlAccountID { get; set; }

	[JsonProperty("xavArArGlAccountID", Order = 6)]
	[MaxLength(11)]
	public string xavArArGlAccountID { get; set; }

	[JsonProperty("xavArBankAccountID", Order = 7)]
	[MaxLength(5)]
	public string xavArBankAccountID { get; set; }

	[JsonProperty("xavArCashGlAccountID", Order = 8)]
	[MaxLength(11)]
	public string xavArCashGlAccountID { get; set; }

	[JsonProperty("xavArDepositGlAccountID", Order = 9)]
	[MaxLength(11)]
	public string xavArDepositGlAccountID { get; set; }

	[JsonProperty("xavArDiscountGlAccountID", Order = 10)]
	[MaxLength(11)]
	public string xavArDiscountGlAccountID { get; set; }

	[JsonProperty("xavArFreightGlAccountID", Order = 11)]
	[MaxLength(11)]
	public string xavArFreightGlAccountID { get; set; }

	[JsonProperty("xavArSalesGlAccountID", Order = 12)]
	[MaxLength(11)]
	public string xavArSalesGlAccountID { get; set; }

	[JsonProperty("xavPlantDepartmentID", Order = 13)]
	[Required(ErrorMessage = "xavPlantDepartmentID is required.")]
	[MaxLength(5)]
	public string xavPlantDepartmentID { get; set; }

	[JsonProperty("xavCreatedBy", Order = 14)]
	[MaxLength(20)]
	public string xavCreatedBy { get; set; }

	[JsonProperty("xavCreatedDate", Order = 15)]
	public DateTime? xavCreatedDate { get; set; }

	[JsonProperty("xavUniqueID", Order = 16)]
	public Guid xavUniqueID { get; set; }

	[JsonProperty("xavEstablishedDate", Order = 17)]
	public DateTime? xavEstablishedDate { get; set; }

	[JsonProperty("xavInactiveDate", Order = 18)]
	public DateTime? xavInactiveDate { get; set; }

	[JsonProperty("xavInactive", Order = 19)]
	public bool xavInactive { get; set; }

	[JsonProperty("xavUseProperties", Order = 20)]
	public bool xavUseProperties { get; set; }

	[JsonProperty("xavName", Order = 21)]
	[Required(ErrorMessage = "xavName is required.")]
	[MaxLength(50)]
	public string xavName { get; set; }

	[JsonProperty("xavPlantID", Order = 22)]
	[Required(ErrorMessage = "xavPlantID is required.")]
	[MaxLength(5)]
	public string xavPlantID { get; set; }

	[JsonProperty("xavRowVersion", Order = 23)]
	public byte[] xavRowVersion { get; set; }

	[JsonProperty("customFields", Order = 24)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
