using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPBankAccountDto
{
	[JsonProperty("glnBankAccountName", Order = 1)]
	[MaxLength(50)]
	public string glnBankAccountName { get; set; }

	[JsonProperty("glnBankAccountNumber", Order = 2)]
	[MaxLength(24)]
	public string glnBankAccountNumber { get; set; }

	[JsonProperty("glnBankInitials", Order = 3)]
	[MaxLength(3)]
	public string glnBankInitials { get; set; }

	[JsonProperty("glnBankName", Order = 4)]
	[Required(ErrorMessage = "glnBankName is required.")]
	[MaxLength(30)]
	public string glnBankName { get; set; }

	[JsonProperty("glnBic", Order = 5)]
	[MaxLength(50)]
	public string glnBic { get; set; }

	[JsonProperty("glnBsbNumber", Order = 6)]
	[MaxLength(10)]
	public string glnBsbNumber { get; set; }

	[JsonProperty("glnCanadianEftType", Order = 7)]
	[MaxLength(5)]
	public string glnCanadianEftType { get; set; }

	[JsonProperty("glnCashGlAccountID", Order = 8)]
	[Required(ErrorMessage = "glnCashGlAccountID is required.")]
	[MaxLength(11)]
	public string glnCashGlAccountID { get; set; }

	[JsonProperty("glnBankAccountID", Order = 9)]
	[Required(ErrorMessage = "glnBankAccountID is required.")]
	[MaxLength(5)]
	public string glnBankAccountID { get; set; }

	[JsonProperty("glnCreatedBy", Order = 10)]
	[MaxLength(20)]
	public string glnCreatedBy { get; set; }

	[JsonProperty("glnCreatedDate", Order = 11)]
	public DateTime? glnCreatedDate { get; set; }

	[JsonProperty("glnCurrencyRateID", Order = 12)]
	[MaxLength(5)]
	public string glnCurrencyRateID { get; set; }

	[JsonProperty("glnDataCenterCode", Order = 13)]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glnDataCenterCode { get; set; }

	[JsonProperty("glnDescription", Order = 14)]
	[Required(ErrorMessage = "glnDescription is required.")]
	[MaxLength(50)]
	public string glnDescription { get; set; }

	[JsonProperty("glnDirectEntryUserID", Order = 15)]
	[MaxLength(6)]
	public string glnDirectEntryUserID { get; set; }

	[JsonProperty("glnDirectEntryUserName", Order = 16)]
	[MaxLength(30)]
	public string glnDirectEntryUserName { get; set; }

	[JsonProperty("glnEftApDescription", Order = 17)]
	[MaxLength(20)]
	public string glnEftApDescription { get; set; }

	[JsonProperty("glnEftCompanyID", Order = 18)]
	[MaxLength(10)]
	public string glnEftCompanyID { get; set; }

	[JsonProperty("glnEftCompanyName", Order = 19)]
	[MaxLength(30)]
	public string glnEftCompanyName { get; set; }

	[JsonProperty("glnEftDiscretionaryData", Order = 20)]
	[MaxLength(20)]
	public string glnEftDiscretionaryData { get; set; }

	[JsonProperty("glnEftFileID", Order = 21)]
	[MaxLength(10)]
	public string glnEftFileID { get; set; }

	[JsonProperty("glnEftFileIDModifier", Order = 22)]
	[MaxLength(1)]
	public string glnEftFileIDModifier { get; set; }

	[JsonProperty("glnEftFileLocation", Order = 23)]
	[MaxLength(50)]
	public string glnEftFileLocation { get; set; }

	[JsonProperty("glnEftPayrollDescription", Order = 24)]
	[MaxLength(20)]
	public string glnEftPayrollDescription { get; set; }

	[JsonProperty("glnEftReferenceCode", Order = 25)]
	[MaxLength(8)]
	public string glnEftReferenceCode { get; set; }

	[JsonProperty("glnUniqueID", Order = 26)]
	public Guid glnUniqueID { get; set; }

	[JsonProperty("glnFileCreationNumber", Order = 27)]
	public short glnFileCreationNumber { get; set; }

	[JsonProperty("glnIban", Order = 28)]
	[MaxLength(50)]
	public string glnIban { get; set; }

	[JsonProperty("glnInactiveDate", Order = 29)]
	public DateTime? glnInactiveDate { get; set; }

	[JsonProperty("glnAChFormat", Order = 30)]
	public bool glnAChFormat { get; set; }

	[JsonProperty("glnInactive", Order = 31)]
	public bool glnInactive { get; set; }

	[JsonProperty("glnEftCreateOffsettingDebit", Order = 32)]
	public bool glnEftCreateOffsettingDebit { get; set; }

	[JsonProperty("glnPayrollOnly", Order = 33)]
	public bool glnPayrollOnly { get; set; }

	[JsonProperty("glnLanguageCode", Order = 34)]
	[MaxLength(1)]
	public string glnLanguageCode { get; set; }

	[JsonProperty("glnNextEftNumber", Order = 35)]
	[Range(0, 999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int glnNextEftNumber { get; set; }

	[JsonProperty("glnNextPaymentNumber", Order = 36)]
	[Range(0, 999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int glnNextPaymentNumber { get; set; }

	[JsonProperty("glnNZEftType", Order = 37)]
	[MaxLength(5)]
	public string glnNZEftType { get; set; }

	[JsonProperty("glnOrganizationID", Order = 38)]
	[MaxLength(10)]
	public string glnOrganizationID { get; set; }

	[JsonProperty("glnRowVersion", Order = 39)]
	public byte[] glnRowVersion { get; set; }

	[JsonProperty("customFields", Order = 40)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
