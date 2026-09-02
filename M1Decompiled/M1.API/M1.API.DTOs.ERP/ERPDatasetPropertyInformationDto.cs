using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPDatasetPropertyInformationDto
{
	public string xadAddressLine1 { get; set; }

	public string xadAddressLine2 { get; set; }

	public string xadAddressLine3 { get; set; }

	public string xadBankAccountID { get; set; }

	public byte xadBuyQuantityDecimals { get; set; }

	public string xadCArPPRegistrationNumber { get; set; }

	public int xadCASubmissionReference { get; set; }

	public string xadCATransmitterNumber { get; set; }

	public string xadCity { get; set; }

	public int xadColor { get; set; }

	public string xadCompanyMessageRTF { get; set; }

	public string xadCompanyMessageText { get; set; }

	public string xadCountry { get; set; }

	public string xadCountryCode { get; set; }

	public string xadCreatedBy { get; set; }

	public DateTime? xadCreatedDate { get; set; }

	public string xadCreditCardBankAccountID { get; set; }

	public string xadCurrencyRateID { get; set; }

	public decimal xadDayStartTimeFri { get; set; }

	public decimal xadDayStartTimeMon { get; set; }

	public decimal xadDayStartTimeSat { get; set; }

	public decimal xadDayStartTimeSun { get; set; }

	public decimal xadDayStartTimeThu { get; set; }

	public decimal xadDayStartTimeTue { get; set; }

	public decimal xadDayStartTimeWed { get; set; }

	public string xadDescription { get; set; }

	public string xadEmailAddress { get; set; }

	public Guid xadUniqueID { get; set; }

	public string xadExtensionVersions { get; set; }

	public string xadFaxNumber { get; set; }

	public string xadFederalID { get; set; }

	public int xadForeColor { get; set; }

	public string xadGlChartPrefix { get; set; }

	public string xadGlDepartmentID { get; set; }

	public string xadGlDivisionID { get; set; }

	public decimal xadHoursFri { get; set; }

	public decimal xadHoursMon { get; set; }

	public decimal xadHoursSat { get; set; }

	public decimal xadHoursSun { get; set; }

	public decimal xadHoursThu { get; set; }

	public decimal xadHoursTue { get; set; }

	public decimal xadHoursWed { get; set; }

	public string xadIntraCompanyOrganizationID { get; set; }

	public byte xadInventoryQuantityDecimals { get; set; }

	public bool xadAllowIntraCompanyTrans { get; set; }

	public bool xadBackupCheck { get; set; }

	public bool xadDisableLotNumbers { get; set; }

	public bool xadDisableOrganizationParts { get; set; }

	public bool xadDisableRetention { get; set; }

	public bool xadDisableRevisions { get; set; }

	public bool xadDisableSerialNumbers { get; set; }

	public bool xadDisableWarehouses { get; set; }

	public bool xadEditInExplorers { get; set; }

	public bool xadEnableM1Email { get; set; }

	public bool xadEnableM1Home { get; set; }

	public bool xadEnableMultiCurrency { get; set; }

	public bool xadEnableNonNettable { get; set; }

	public bool xadEnableOutlookDesktop { get; set; }

	public bool xadEnableOutlookOnline { get; set; }

	public bool xadExportFollowups { get; set; }

	public bool xadExtendedSearchOptions { get; set; }

	public bool xadIgnoreSSLCertValidate { get; set; }

	public bool xadSuppressAddressOnReports { get; set; }

	public bool xadUpdateMasterDataInFinPkg { get; set; }

	public bool xadViewForeign { get; set; }

	public string xadLanguage { get; set; }

	public string xadMailProvider { get; set; }

	public string xadMailServer { get; set; }

	public int xadMaxGridRow { get; set; }

	public int xadMaxItemsOnGantt { get; set; }

	public string xadName { get; set; }

	public string xadPhoneNumber { get; set; }

	public string xadPostCode { get; set; }

	public string xadRegion { get; set; }

	public byte[] xadRowVersion { get; set; }

	public byte xadSellQuantityDecimals { get; set; }

	public string xadState { get; set; }

	public byte xadTimeFormat { get; set; }

	public string xadTimeZone { get; set; }

	public string xadTINType { get; set; }

	public string xadUpgradeVersions { get; set; }

	public string xadUpsInterfaceFolderName { get; set; }

	public string xadVersion { get; set; }

	public DateTime? xadVersion92UpgradeDate { get; set; }

	public string xadWebAddress { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
