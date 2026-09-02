using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPDatasetPropertyDto
{
	[JsonProperty("xadAddressLine1", Order = 1)]
	[MaxLength(50)]
	public string xadAddressLine1 { get; set; }

	[JsonProperty("xadAddressLine2", Order = 2)]
	[MaxLength(50)]
	public string xadAddressLine2 { get; set; }

	[JsonProperty("xadAddressLine3", Order = 3)]
	[MaxLength(50)]
	public string xadAddressLine3 { get; set; }

	[JsonProperty("xadBankAccountID", Order = 4)]
	[MaxLength(5)]
	public string xadBankAccountID { get; set; }

	[JsonProperty("xadBuyQuantityDecimals", Order = 5)]
	public byte xadBuyQuantityDecimals { get; set; }

	[JsonProperty("xadCArPPRegistrationNumber", Order = 6)]
	[MaxLength(7)]
	public string xadCArPPRegistrationNumber { get; set; }

	[JsonProperty("xadCASubmissionReference", Order = 7)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xadCASubmissionReference { get; set; }

	[JsonProperty("xadCATransmitterNumber", Order = 8)]
	[MaxLength(8)]
	public string xadCATransmitterNumber { get; set; }

	[JsonProperty("xadCity", Order = 9)]
	[MaxLength(30)]
	public string xadCity { get; set; }

	[JsonProperty("xadColor", Order = 10)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xadColor { get; set; }

	[JsonProperty("xadCompanyMessageRTF", Order = 11)]
	[MaxLength(50)]
	public string xadCompanyMessageRTF { get; set; }

	[JsonProperty("xadCompanyMessageText", Order = 12)]
	[MaxLength(50)]
	public string xadCompanyMessageText { get; set; }

	[JsonProperty("xadCountry", Order = 13)]
	[MaxLength(20)]
	public string xadCountry { get; set; }

	[JsonProperty("xadCountryCode", Order = 14)]
	[MaxLength(5)]
	public string xadCountryCode { get; set; }

	[JsonProperty("xadCreatedBy", Order = 15)]
	[MaxLength(20)]
	public string xadCreatedBy { get; set; }

	[JsonProperty("xadCreatedDate", Order = 16)]
	public DateTime? xadCreatedDate { get; set; }

	[JsonProperty("xadCreditCardBankAccountID", Order = 17)]
	[MaxLength(5)]
	public string xadCreditCardBankAccountID { get; set; }

	[JsonProperty("xadCurrencyRateID", Order = 18)]
	[MaxLength(5)]
	public string xadCurrencyRateID { get; set; }

	[JsonProperty("xadDayStartTimeFri", Order = 19)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadDayStartTimeFri { get; set; }

	[JsonProperty("xadDayStartTimeMon", Order = 20)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadDayStartTimeMon { get; set; }

	[JsonProperty("xadDayStartTimeSat", Order = 21)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadDayStartTimeSat { get; set; }

	[JsonProperty("xadDayStartTimeSun", Order = 22)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadDayStartTimeSun { get; set; }

	[JsonProperty("xadDayStartTimeThu", Order = 23)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadDayStartTimeThu { get; set; }

	[JsonProperty("xadDayStartTimeTue", Order = 24)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadDayStartTimeTue { get; set; }

	[JsonProperty("xadDayStartTimeWed", Order = 25)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadDayStartTimeWed { get; set; }

	[JsonProperty("xadDescription", Order = 26)]
	[Required(ErrorMessage = "xadDescription is required.")]
	[MaxLength(50)]
	public string xadDescription { get; set; }

	[JsonProperty("xadEmailAddress", Order = 27)]
	[MaxLength(50)]
	public string xadEmailAddress { get; set; }

	[JsonProperty("xadUniqueID", Order = 28)]
	public Guid xadUniqueID { get; set; }

	[JsonProperty("xadExtensionVersions", Order = 29)]
	[MaxLength(4)]
	public string xadExtensionVersions { get; set; }

	[JsonProperty("xadFaxNumber", Order = 30)]
	[MaxLength(20)]
	public string xadFaxNumber { get; set; }

	[JsonProperty("xadFederalID", Order = 31)]
	[MaxLength(20)]
	public string xadFederalID { get; set; }

	[JsonProperty("xadForeColor", Order = 32)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xadForeColor { get; set; }

	[JsonProperty("xadGlChartPrefix", Order = 33)]
	[MaxLength(2)]
	public string xadGlChartPrefix { get; set; }

	[JsonProperty("xadGlDepartmentID", Order = 34)]
	[MaxLength(3)]
	public string xadGlDepartmentID { get; set; }

	[JsonProperty("xadGlDivisionID", Order = 35)]
	[MaxLength(3)]
	public string xadGlDivisionID { get; set; }

	[JsonProperty("xadHoursFri", Order = 36)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadHoursFri { get; set; }

	[JsonProperty("xadHoursMon", Order = 37)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadHoursMon { get; set; }

	[JsonProperty("xadHoursSat", Order = 38)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadHoursSat { get; set; }

	[JsonProperty("xadHoursSun", Order = 39)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadHoursSun { get; set; }

	[JsonProperty("xadHoursThu", Order = 40)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadHoursThu { get; set; }

	[JsonProperty("xadHoursTue", Order = 41)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadHoursTue { get; set; }

	[JsonProperty("xadHoursWed", Order = 42)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xadHoursWed { get; set; }

	[JsonProperty("xadIntraCompanyOrganizationID", Order = 43)]
	[MaxLength(10)]
	public string xadIntraCompanyOrganizationID { get; set; }

	[JsonProperty("xadInventoryQuantityDecimals", Order = 44)]
	public byte xadInventoryQuantityDecimals { get; set; }

	[JsonProperty("xadAllowIntraCompanyTrans", Order = 45)]
	public bool xadAllowIntraCompanyTrans { get; set; }

	[JsonProperty("xadBackupCheck", Order = 46)]
	public bool xadBackupCheck { get; set; }

	[JsonProperty("xadDisableLotNumbers", Order = 47)]
	public bool xadDisableLotNumbers { get; set; }

	[JsonProperty("xadDisableOrganizationParts", Order = 48)]
	public bool xadDisableOrganizationParts { get; set; }

	[JsonProperty("xadDisableRetention", Order = 49)]
	public bool xadDisableRetention { get; set; }

	[JsonProperty("xadDisableRevisions", Order = 50)]
	public bool xadDisableRevisions { get; set; }

	[JsonProperty("xadDisableSerialNumbers", Order = 51)]
	public bool xadDisableSerialNumbers { get; set; }

	[JsonProperty("xadDisableWarehouses", Order = 52)]
	public bool xadDisableWarehouses { get; set; }

	[JsonProperty("xadEditInExplorers", Order = 53)]
	public bool xadEditInExplorers { get; set; }

	[JsonProperty("xadEnableM1Email", Order = 54)]
	public bool xadEnableM1Email { get; set; }

	[JsonProperty("xadEnableM1Home", Order = 55)]
	public bool xadEnableM1Home { get; set; }

	[JsonProperty("xadEnableMultiCurrency", Order = 56)]
	public bool xadEnableMultiCurrency { get; set; }

	[JsonProperty("xadEnableNonNettable", Order = 57)]
	public bool xadEnableNonNettable { get; set; }

	[JsonProperty("xadEnableOutlookDesktop", Order = 58)]
	public bool xadEnableOutlookDesktop { get; set; }

	[JsonProperty("xadEnableOutlookOnline", Order = 59)]
	public bool xadEnableOutlookOnline { get; set; }

	[JsonProperty("xadExportFollowups", Order = 60)]
	public bool xadExportFollowups { get; set; }

	[JsonProperty("xadExtendedSearchOptions", Order = 61)]
	public bool xadExtendedSearchOptions { get; set; }

	[JsonProperty("xadIgnoreSSLCertValidate", Order = 62)]
	public bool xadIgnoreSSLCertValidate { get; set; }

	[JsonProperty("xadSuppressAddressOnReports", Order = 63)]
	public bool xadSuppressAddressOnReports { get; set; }

	[JsonProperty("xadUpdateMasterDataInFinPkg", Order = 64)]
	public bool xadUpdateMasterDataInFinPkg { get; set; }

	[JsonProperty("xadViewForeign", Order = 65)]
	public bool xadViewForeign { get; set; }

	[JsonProperty("xadLanguage", Order = 66)]
	[MaxLength(10)]
	public string xadLanguage { get; set; }

	[JsonProperty("xadMailProvider", Order = 67)]
	[MaxLength(15)]
	public string xadMailProvider { get; set; }

	[JsonProperty("xadMailServer", Order = 68)]
	[MaxLength(100)]
	public string xadMailServer { get; set; }

	[JsonProperty("xadMaxGridRow", Order = 69)]
	[Range(0, 9999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xadMaxGridRow { get; set; }

	[JsonProperty("xadMaxItemsOnGantt", Order = 70)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xadMaxItemsOnGantt { get; set; }

	[JsonProperty("xadName", Order = 71)]
	[Required(ErrorMessage = "xadName is required.")]
	[MaxLength(50)]
	public string xadName { get; set; }

	[JsonProperty("xadPhoneNumber", Order = 72)]
	[MaxLength(20)]
	public string xadPhoneNumber { get; set; }

	[JsonProperty("xadPostCode", Order = 73)]
	[MaxLength(10)]
	public string xadPostCode { get; set; }

	[JsonProperty("xadRegion", Order = 74)]
	[MaxLength(3)]
	public string xadRegion { get; set; }

	[JsonProperty("xadRowVersion", Order = 75)]
	public byte[] xadRowVersion { get; set; }

	[JsonProperty("xadSellQuantityDecimals", Order = 76)]
	public byte xadSellQuantityDecimals { get; set; }

	[JsonProperty("xadState", Order = 77)]
	[MaxLength(3)]
	public string xadState { get; set; }

	[JsonProperty("xadTimeFormat", Order = 78)]
	public byte xadTimeFormat { get; set; }

	[JsonProperty("xadTimeZone", Order = 79)]
	[MaxLength(100)]
	public string xadTimeZone { get; set; }

	[JsonProperty("xadTINType", Order = 80)]
	[MaxLength(20)]
	public string xadTINType { get; set; }

	[JsonProperty("xadUpgradeVersions", Order = 81)]
	[MaxLength(50)]
	public string xadUpgradeVersions { get; set; }

	[JsonProperty("xadUpsInterfaceFolderName", Order = 82)]
	[MaxLength(50)]
	public string xadUpsInterfaceFolderName { get; set; }

	[JsonProperty("xadVersion", Order = 83)]
	[Required(ErrorMessage = "xadVersion is required.")]
	[MaxLength(10)]
	public string xadVersion { get; set; }

	[JsonProperty("xadVersion92UpgradeDate", Order = 84)]
	public DateTime? xadVersion92UpgradeDate { get; set; }

	[JsonProperty("xadWebAddress", Order = 85)]
	[MaxLength(100)]
	public string xadWebAddress { get; set; }

	[JsonProperty("customFields", Order = 86)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
