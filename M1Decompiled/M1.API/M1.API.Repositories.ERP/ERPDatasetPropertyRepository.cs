using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPDatasetPropertyRepository : APIBaseRepository, IERPDatasetPropertyRepository, IAPIBaseRepository, IDisposable
{
	public ERPDatasetPropertyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesDatasetPropertyExist(Guid datasetPropertyId)
	{
		InitializeParameterLists();
		base.filterList.Add("xadUniqueID|C", datasetPropertyId);
		base.selectList.Add("xadUniqueID");
		return Task.FromResult(GetAsObject("DatasetProperties", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPDatasetPropertyInformationDto>> GetAllDatasetProperties(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPDatasetPropertyInformationDto> collection = new List<ERPDatasetPropertyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[85]
		{
			"xadAddressLine1", "xadAddressLine2", "xadAddressLine3", "xadBankAccountID", "xadBuyQuantityDecimals", "xadCArPPRegistrationNumber", "xadCASubmissionReference", "xadCATransmitterNumber", "xadCity", "xadColor",
			"xadCompanyMessageRTF", "xadCompanyMessageText", "xadCountry", "xadCountryCode", "xadCreatedBy", "xadCreatedDate", "xadCreditCardBankAccountID", "xadCurrencyRateID", "xadDayStartTimeFri", "xadDayStartTimeMon",
			"xadDayStartTimeSat", "xadDayStartTimeSun", "xadDayStartTimeThu", "xadDayStartTimeTue", "xadDayStartTimeWed", "xadDescription", "xadEmailAddress", "xadUniqueID", "xadExtensionVersions", "xadFaxNumber",
			"xadFederalID", "xadForeColor", "xadGlChartPrefix", "xadGlDepartmentID", "xadGlDivisionID", "xadHoursFri", "xadHoursMon", "xadHoursSat", "xadHoursSun", "xadHoursThu",
			"xadHoursTue", "xadHoursWed", "xadIntraCompanyOrganizationID", "xadInventoryQuantityDecimals", "xadAllowIntraCompanyTrans", "xadBackupCheck", "xadDisableLotNumbers", "xadDisableOrganizationParts", "xadDisableRetention", "xadDisableRevisions",
			"xadDisableSerialNumbers", "xadDisableWarehouses", "xadEditInExplorers", "xadEnableM1Email", "xadEnableM1Home", "xadEnableMultiCurrency", "xadEnableNonNettable", "xadEnableOutlookDesktop", "xadEnableOutlookOnline", "xadExportFollowups",
			"xadExtendedSearchOptions", "xadIgnoreSSLCertValidate", "xadSuppressAddressOnReports", "xadUpdateMasterDataInFinPkg", "xadViewForeign", "xadLanguage", "xadMailProvider", "xadMailServer", "xadMaxGridRow", "xadMaxItemsOnGantt",
			"xadName", "xadPhoneNumber", "xadPostCode", "xadRegion", "xadRowVersion", "xadSellQuantityDecimals", "xadState", "xadTimeFormat", "xadTimeZone", "xadTINType",
			"xadUpgradeVersions", "xadUpsInterfaceFolderName", "xadVersion", "xadVersion92UpgradeDate", "xadWebAddress"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("DatasetProperties");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("DatasetProperties", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPDatasetPropertyInformationDto eRPDatasetPropertyInformationDto = new ERPDatasetPropertyInformationDto();
				eRPDatasetPropertyInformationDto.xadAddressLine1 = dataTable.Rows[i].Field<string>("xadAddressLine1");
				eRPDatasetPropertyInformationDto.xadAddressLine2 = dataTable.Rows[i].Field<string>("xadAddressLine2");
				eRPDatasetPropertyInformationDto.xadAddressLine3 = dataTable.Rows[i].Field<string>("xadAddressLine3");
				eRPDatasetPropertyInformationDto.xadBankAccountID = dataTable.Rows[i].Field<string>("xadBankAccountID");
				eRPDatasetPropertyInformationDto.xadBuyQuantityDecimals = dataTable.Rows[i].Field<byte>("xadBuyQuantityDecimals");
				eRPDatasetPropertyInformationDto.xadCArPPRegistrationNumber = dataTable.Rows[i].Field<string>("xadCArPPRegistrationNumber");
				eRPDatasetPropertyInformationDto.xadCASubmissionReference = dataTable.Rows[i].Field<int>("xadCASubmissionReference");
				eRPDatasetPropertyInformationDto.xadCATransmitterNumber = dataTable.Rows[i].Field<string>("xadCATransmitterNumber");
				eRPDatasetPropertyInformationDto.xadCity = dataTable.Rows[i].Field<string>("xadCity");
				eRPDatasetPropertyInformationDto.xadColor = dataTable.Rows[i].Field<int>("xadColor");
				eRPDatasetPropertyInformationDto.xadCompanyMessageRTF = dataTable.Rows[i].Field<string>("xadCompanyMessageRTF");
				eRPDatasetPropertyInformationDto.xadCompanyMessageText = dataTable.Rows[i].Field<string>("xadCompanyMessageText");
				eRPDatasetPropertyInformationDto.xadCountry = dataTable.Rows[i].Field<string>("xadCountry");
				eRPDatasetPropertyInformationDto.xadCountryCode = dataTable.Rows[i].Field<string>("xadCountryCode");
				eRPDatasetPropertyInformationDto.xadCreatedBy = dataTable.Rows[i].Field<string>("xadCreatedBy");
				eRPDatasetPropertyInformationDto.xadCreatedDate = dataTable.Rows[i].Field<DateTime?>("xadCreatedDate");
				eRPDatasetPropertyInformationDto.xadCreditCardBankAccountID = dataTable.Rows[i].Field<string>("xadCreditCardBankAccountID");
				eRPDatasetPropertyInformationDto.xadCurrencyRateID = dataTable.Rows[i].Field<string>("xadCurrencyRateID");
				eRPDatasetPropertyInformationDto.xadDayStartTimeFri = dataTable.Rows[i].Field<decimal>("xadDayStartTimeFri");
				eRPDatasetPropertyInformationDto.xadDayStartTimeMon = dataTable.Rows[i].Field<decimal>("xadDayStartTimeMon");
				eRPDatasetPropertyInformationDto.xadDayStartTimeSat = dataTable.Rows[i].Field<decimal>("xadDayStartTimeSat");
				eRPDatasetPropertyInformationDto.xadDayStartTimeSun = dataTable.Rows[i].Field<decimal>("xadDayStartTimeSun");
				eRPDatasetPropertyInformationDto.xadDayStartTimeThu = dataTable.Rows[i].Field<decimal>("xadDayStartTimeThu");
				eRPDatasetPropertyInformationDto.xadDayStartTimeTue = dataTable.Rows[i].Field<decimal>("xadDayStartTimeTue");
				eRPDatasetPropertyInformationDto.xadDayStartTimeWed = dataTable.Rows[i].Field<decimal>("xadDayStartTimeWed");
				eRPDatasetPropertyInformationDto.xadDescription = dataTable.Rows[i].Field<string>("xadDescription");
				eRPDatasetPropertyInformationDto.xadEmailAddress = dataTable.Rows[i].Field<string>("xadEmailAddress");
				eRPDatasetPropertyInformationDto.xadUniqueID = dataTable.Rows[i].Field<Guid>("xadUniqueID");
				eRPDatasetPropertyInformationDto.xadExtensionVersions = dataTable.Rows[i].Field<string>("xadExtensionVersions");
				eRPDatasetPropertyInformationDto.xadFaxNumber = dataTable.Rows[i].Field<string>("xadFaxNumber");
				eRPDatasetPropertyInformationDto.xadFederalID = dataTable.Rows[i].Field<string>("xadFederalID");
				eRPDatasetPropertyInformationDto.xadForeColor = dataTable.Rows[i].Field<int>("xadForeColor");
				eRPDatasetPropertyInformationDto.xadGlChartPrefix = dataTable.Rows[i].Field<string>("xadGlChartPrefix");
				eRPDatasetPropertyInformationDto.xadGlDepartmentID = dataTable.Rows[i].Field<string>("xadGlDepartmentID");
				eRPDatasetPropertyInformationDto.xadGlDivisionID = dataTable.Rows[i].Field<string>("xadGlDivisionID");
				eRPDatasetPropertyInformationDto.xadHoursFri = dataTable.Rows[i].Field<decimal>("xadHoursFri");
				eRPDatasetPropertyInformationDto.xadHoursMon = dataTable.Rows[i].Field<decimal>("xadHoursMon");
				eRPDatasetPropertyInformationDto.xadHoursSat = dataTable.Rows[i].Field<decimal>("xadHoursSat");
				eRPDatasetPropertyInformationDto.xadHoursSun = dataTable.Rows[i].Field<decimal>("xadHoursSun");
				eRPDatasetPropertyInformationDto.xadHoursThu = dataTable.Rows[i].Field<decimal>("xadHoursThu");
				eRPDatasetPropertyInformationDto.xadHoursTue = dataTable.Rows[i].Field<decimal>("xadHoursTue");
				eRPDatasetPropertyInformationDto.xadHoursWed = dataTable.Rows[i].Field<decimal>("xadHoursWed");
				eRPDatasetPropertyInformationDto.xadIntraCompanyOrganizationID = dataTable.Rows[i].Field<string>("xadIntraCompanyOrganizationID");
				eRPDatasetPropertyInformationDto.xadInventoryQuantityDecimals = dataTable.Rows[i].Field<byte>("xadInventoryQuantityDecimals");
				eRPDatasetPropertyInformationDto.xadAllowIntraCompanyTrans = dataTable.Rows[i].Field<bool>("xadAllowIntraCompanyTrans");
				eRPDatasetPropertyInformationDto.xadBackupCheck = dataTable.Rows[i].Field<bool>("xadBackupCheck");
				eRPDatasetPropertyInformationDto.xadDisableLotNumbers = dataTable.Rows[i].Field<bool>("xadDisableLotNumbers");
				eRPDatasetPropertyInformationDto.xadDisableOrganizationParts = dataTable.Rows[i].Field<bool>("xadDisableOrganizationParts");
				eRPDatasetPropertyInformationDto.xadDisableRetention = dataTable.Rows[i].Field<bool>("xadDisableRetention");
				eRPDatasetPropertyInformationDto.xadDisableRevisions = dataTable.Rows[i].Field<bool>("xadDisableRevisions");
				eRPDatasetPropertyInformationDto.xadDisableSerialNumbers = dataTable.Rows[i].Field<bool>("xadDisableSerialNumbers");
				eRPDatasetPropertyInformationDto.xadDisableWarehouses = dataTable.Rows[i].Field<bool>("xadDisableWarehouses");
				eRPDatasetPropertyInformationDto.xadEditInExplorers = dataTable.Rows[i].Field<bool>("xadEditInExplorers");
				eRPDatasetPropertyInformationDto.xadEnableM1Email = dataTable.Rows[i].Field<bool>("xadEnableM1Email");
				eRPDatasetPropertyInformationDto.xadEnableM1Home = dataTable.Rows[i].Field<bool>("xadEnableM1Home");
				eRPDatasetPropertyInformationDto.xadEnableMultiCurrency = dataTable.Rows[i].Field<bool>("xadEnableMultiCurrency");
				eRPDatasetPropertyInformationDto.xadEnableNonNettable = dataTable.Rows[i].Field<bool>("xadEnableNonNettable");
				eRPDatasetPropertyInformationDto.xadEnableOutlookDesktop = dataTable.Rows[i].Field<bool>("xadEnableOutlookDesktop");
				eRPDatasetPropertyInformationDto.xadEnableOutlookOnline = dataTable.Rows[i].Field<bool>("xadEnableOutlookOnline");
				eRPDatasetPropertyInformationDto.xadExportFollowups = dataTable.Rows[i].Field<bool>("xadExportFollowups");
				eRPDatasetPropertyInformationDto.xadExtendedSearchOptions = dataTable.Rows[i].Field<bool>("xadExtendedSearchOptions");
				eRPDatasetPropertyInformationDto.xadIgnoreSSLCertValidate = dataTable.Rows[i].Field<bool>("xadIgnoreSSLCertValidate");
				eRPDatasetPropertyInformationDto.xadSuppressAddressOnReports = dataTable.Rows[i].Field<bool>("xadSuppressAddressOnReports");
				eRPDatasetPropertyInformationDto.xadUpdateMasterDataInFinPkg = dataTable.Rows[i].Field<bool>("xadUpdateMasterDataInFinPkg");
				eRPDatasetPropertyInformationDto.xadViewForeign = dataTable.Rows[i].Field<bool>("xadViewForeign");
				eRPDatasetPropertyInformationDto.xadLanguage = dataTable.Rows[i].Field<string>("xadLanguage");
				eRPDatasetPropertyInformationDto.xadMailProvider = dataTable.Rows[i].Field<string>("xadMailProvider");
				eRPDatasetPropertyInformationDto.xadMailServer = dataTable.Rows[i].Field<string>("xadMailServer");
				eRPDatasetPropertyInformationDto.xadMaxGridRow = dataTable.Rows[i].Field<int>("xadMaxGridRow");
				eRPDatasetPropertyInformationDto.xadMaxItemsOnGantt = dataTable.Rows[i].Field<int>("xadMaxItemsOnGantt");
				eRPDatasetPropertyInformationDto.xadName = dataTable.Rows[i].Field<string>("xadName");
				eRPDatasetPropertyInformationDto.xadPhoneNumber = dataTable.Rows[i].Field<string>("xadPhoneNumber");
				eRPDatasetPropertyInformationDto.xadPostCode = dataTable.Rows[i].Field<string>("xadPostCode");
				eRPDatasetPropertyInformationDto.xadRegion = dataTable.Rows[i].Field<string>("xadRegion");
				eRPDatasetPropertyInformationDto.xadRowVersion = dataTable.Rows[i].Field<byte[]>("xadRowVersion");
				eRPDatasetPropertyInformationDto.xadSellQuantityDecimals = dataTable.Rows[i].Field<byte>("xadSellQuantityDecimals");
				eRPDatasetPropertyInformationDto.xadState = dataTable.Rows[i].Field<string>("xadState");
				eRPDatasetPropertyInformationDto.xadTimeFormat = dataTable.Rows[i].Field<byte>("xadTimeFormat");
				eRPDatasetPropertyInformationDto.xadTimeZone = dataTable.Rows[i].Field<string>("xadTimeZone");
				eRPDatasetPropertyInformationDto.xadTINType = dataTable.Rows[i].Field<string>("xadTINType");
				eRPDatasetPropertyInformationDto.xadUpgradeVersions = dataTable.Rows[i].Field<string>("xadUpgradeVersions");
				eRPDatasetPropertyInformationDto.xadUpsInterfaceFolderName = dataTable.Rows[i].Field<string>("xadUpsInterfaceFolderName");
				eRPDatasetPropertyInformationDto.xadVersion = dataTable.Rows[i].Field<string>("xadVersion");
				eRPDatasetPropertyInformationDto.xadVersion92UpgradeDate = dataTable.Rows[i].Field<DateTime?>("xadVersion92UpgradeDate");
				eRPDatasetPropertyInformationDto.xadWebAddress = dataTable.Rows[i].Field<string>("xadWebAddress");
				eRPDatasetPropertyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPDatasetPropertyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPDatasetPropertyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPDatasetPropertyInformationDto> GetDatasetProperty(Guid datasetPropertyId)
	{
		ERPDatasetPropertyInformationDto eRPDatasetPropertyInformationDto = new ERPDatasetPropertyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[85]
		{
			"xadAddressLine1", "xadAddressLine2", "xadAddressLine3", "xadBankAccountID", "xadBuyQuantityDecimals", "xadCArPPRegistrationNumber", "xadCASubmissionReference", "xadCATransmitterNumber", "xadCity", "xadColor",
			"xadCompanyMessageRTF", "xadCompanyMessageText", "xadCountry", "xadCountryCode", "xadCreatedBy", "xadCreatedDate", "xadCreditCardBankAccountID", "xadCurrencyRateID", "xadDayStartTimeFri", "xadDayStartTimeMon",
			"xadDayStartTimeSat", "xadDayStartTimeSun", "xadDayStartTimeThu", "xadDayStartTimeTue", "xadDayStartTimeWed", "xadDescription", "xadEmailAddress", "xadUniqueID", "xadExtensionVersions", "xadFaxNumber",
			"xadFederalID", "xadForeColor", "xadGlChartPrefix", "xadGlDepartmentID", "xadGlDivisionID", "xadHoursFri", "xadHoursMon", "xadHoursSat", "xadHoursSun", "xadHoursThu",
			"xadHoursTue", "xadHoursWed", "xadIntraCompanyOrganizationID", "xadInventoryQuantityDecimals", "xadAllowIntraCompanyTrans", "xadBackupCheck", "xadDisableLotNumbers", "xadDisableOrganizationParts", "xadDisableRetention", "xadDisableRevisions",
			"xadDisableSerialNumbers", "xadDisableWarehouses", "xadEditInExplorers", "xadEnableM1Email", "xadEnableM1Home", "xadEnableMultiCurrency", "xadEnableNonNettable", "xadEnableOutlookDesktop", "xadEnableOutlookOnline", "xadExportFollowups",
			"xadExtendedSearchOptions", "xadIgnoreSSLCertValidate", "xadSuppressAddressOnReports", "xadUpdateMasterDataInFinPkg", "xadViewForeign", "xadLanguage", "xadMailProvider", "xadMailServer", "xadMaxGridRow", "xadMaxItemsOnGantt",
			"xadName", "xadPhoneNumber", "xadPostCode", "xadRegion", "xadRowVersion", "xadSellQuantityDecimals", "xadState", "xadTimeFormat", "xadTimeZone", "xadTINType",
			"xadUpgradeVersions", "xadUpsInterfaceFolderName", "xadVersion", "xadVersion92UpgradeDate", "xadWebAddress"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xadUniqueID|C", datasetPropertyId);
		AddCustomFieldsToSelectList("DatasetProperties");
		using (DataTable dataTable = GetAsDataTable("DatasetProperties", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPDatasetPropertyInformationDto);
			}
			eRPDatasetPropertyInformationDto.xadAddressLine1 = dataTable.Rows[0].Field<string>("xadAddressLine1");
			eRPDatasetPropertyInformationDto.xadAddressLine2 = dataTable.Rows[0].Field<string>("xadAddressLine2");
			eRPDatasetPropertyInformationDto.xadAddressLine3 = dataTable.Rows[0].Field<string>("xadAddressLine3");
			eRPDatasetPropertyInformationDto.xadBankAccountID = dataTable.Rows[0].Field<string>("xadBankAccountID");
			eRPDatasetPropertyInformationDto.xadBuyQuantityDecimals = dataTable.Rows[0].Field<byte>("xadBuyQuantityDecimals");
			eRPDatasetPropertyInformationDto.xadCArPPRegistrationNumber = dataTable.Rows[0].Field<string>("xadCArPPRegistrationNumber");
			eRPDatasetPropertyInformationDto.xadCASubmissionReference = dataTable.Rows[0].Field<int>("xadCASubmissionReference");
			eRPDatasetPropertyInformationDto.xadCATransmitterNumber = dataTable.Rows[0].Field<string>("xadCATransmitterNumber");
			eRPDatasetPropertyInformationDto.xadCity = dataTable.Rows[0].Field<string>("xadCity");
			eRPDatasetPropertyInformationDto.xadColor = dataTable.Rows[0].Field<int>("xadColor");
			eRPDatasetPropertyInformationDto.xadCompanyMessageRTF = dataTable.Rows[0].Field<string>("xadCompanyMessageRTF");
			eRPDatasetPropertyInformationDto.xadCompanyMessageText = dataTable.Rows[0].Field<string>("xadCompanyMessageText");
			eRPDatasetPropertyInformationDto.xadCountry = dataTable.Rows[0].Field<string>("xadCountry");
			eRPDatasetPropertyInformationDto.xadCountryCode = dataTable.Rows[0].Field<string>("xadCountryCode");
			eRPDatasetPropertyInformationDto.xadCreatedBy = dataTable.Rows[0].Field<string>("xadCreatedBy");
			eRPDatasetPropertyInformationDto.xadCreatedDate = dataTable.Rows[0].Field<DateTime?>("xadCreatedDate");
			eRPDatasetPropertyInformationDto.xadCreditCardBankAccountID = dataTable.Rows[0].Field<string>("xadCreditCardBankAccountID");
			eRPDatasetPropertyInformationDto.xadCurrencyRateID = dataTable.Rows[0].Field<string>("xadCurrencyRateID");
			eRPDatasetPropertyInformationDto.xadDayStartTimeFri = dataTable.Rows[0].Field<decimal>("xadDayStartTimeFri");
			eRPDatasetPropertyInformationDto.xadDayStartTimeMon = dataTable.Rows[0].Field<decimal>("xadDayStartTimeMon");
			eRPDatasetPropertyInformationDto.xadDayStartTimeSat = dataTable.Rows[0].Field<decimal>("xadDayStartTimeSat");
			eRPDatasetPropertyInformationDto.xadDayStartTimeSun = dataTable.Rows[0].Field<decimal>("xadDayStartTimeSun");
			eRPDatasetPropertyInformationDto.xadDayStartTimeThu = dataTable.Rows[0].Field<decimal>("xadDayStartTimeThu");
			eRPDatasetPropertyInformationDto.xadDayStartTimeTue = dataTable.Rows[0].Field<decimal>("xadDayStartTimeTue");
			eRPDatasetPropertyInformationDto.xadDayStartTimeWed = dataTable.Rows[0].Field<decimal>("xadDayStartTimeWed");
			eRPDatasetPropertyInformationDto.xadDescription = dataTable.Rows[0].Field<string>("xadDescription");
			eRPDatasetPropertyInformationDto.xadEmailAddress = dataTable.Rows[0].Field<string>("xadEmailAddress");
			eRPDatasetPropertyInformationDto.xadUniqueID = dataTable.Rows[0].Field<Guid>("xadUniqueID");
			eRPDatasetPropertyInformationDto.xadExtensionVersions = dataTable.Rows[0].Field<string>("xadExtensionVersions");
			eRPDatasetPropertyInformationDto.xadFaxNumber = dataTable.Rows[0].Field<string>("xadFaxNumber");
			eRPDatasetPropertyInformationDto.xadFederalID = dataTable.Rows[0].Field<string>("xadFederalID");
			eRPDatasetPropertyInformationDto.xadForeColor = dataTable.Rows[0].Field<int>("xadForeColor");
			eRPDatasetPropertyInformationDto.xadGlChartPrefix = dataTable.Rows[0].Field<string>("xadGlChartPrefix");
			eRPDatasetPropertyInformationDto.xadGlDepartmentID = dataTable.Rows[0].Field<string>("xadGlDepartmentID");
			eRPDatasetPropertyInformationDto.xadGlDivisionID = dataTable.Rows[0].Field<string>("xadGlDivisionID");
			eRPDatasetPropertyInformationDto.xadHoursFri = dataTable.Rows[0].Field<decimal>("xadHoursFri");
			eRPDatasetPropertyInformationDto.xadHoursMon = dataTable.Rows[0].Field<decimal>("xadHoursMon");
			eRPDatasetPropertyInformationDto.xadHoursSat = dataTable.Rows[0].Field<decimal>("xadHoursSat");
			eRPDatasetPropertyInformationDto.xadHoursSun = dataTable.Rows[0].Field<decimal>("xadHoursSun");
			eRPDatasetPropertyInformationDto.xadHoursThu = dataTable.Rows[0].Field<decimal>("xadHoursThu");
			eRPDatasetPropertyInformationDto.xadHoursTue = dataTable.Rows[0].Field<decimal>("xadHoursTue");
			eRPDatasetPropertyInformationDto.xadHoursWed = dataTable.Rows[0].Field<decimal>("xadHoursWed");
			eRPDatasetPropertyInformationDto.xadIntraCompanyOrganizationID = dataTable.Rows[0].Field<string>("xadIntraCompanyOrganizationID");
			eRPDatasetPropertyInformationDto.xadInventoryQuantityDecimals = dataTable.Rows[0].Field<byte>("xadInventoryQuantityDecimals");
			eRPDatasetPropertyInformationDto.xadAllowIntraCompanyTrans = dataTable.Rows[0].Field<bool>("xadAllowIntraCompanyTrans");
			eRPDatasetPropertyInformationDto.xadBackupCheck = dataTable.Rows[0].Field<bool>("xadBackupCheck");
			eRPDatasetPropertyInformationDto.xadDisableLotNumbers = dataTable.Rows[0].Field<bool>("xadDisableLotNumbers");
			eRPDatasetPropertyInformationDto.xadDisableOrganizationParts = dataTable.Rows[0].Field<bool>("xadDisableOrganizationParts");
			eRPDatasetPropertyInformationDto.xadDisableRetention = dataTable.Rows[0].Field<bool>("xadDisableRetention");
			eRPDatasetPropertyInformationDto.xadDisableRevisions = dataTable.Rows[0].Field<bool>("xadDisableRevisions");
			eRPDatasetPropertyInformationDto.xadDisableSerialNumbers = dataTable.Rows[0].Field<bool>("xadDisableSerialNumbers");
			eRPDatasetPropertyInformationDto.xadDisableWarehouses = dataTable.Rows[0].Field<bool>("xadDisableWarehouses");
			eRPDatasetPropertyInformationDto.xadEditInExplorers = dataTable.Rows[0].Field<bool>("xadEditInExplorers");
			eRPDatasetPropertyInformationDto.xadEnableM1Email = dataTable.Rows[0].Field<bool>("xadEnableM1Email");
			eRPDatasetPropertyInformationDto.xadEnableM1Home = dataTable.Rows[0].Field<bool>("xadEnableM1Home");
			eRPDatasetPropertyInformationDto.xadEnableMultiCurrency = dataTable.Rows[0].Field<bool>("xadEnableMultiCurrency");
			eRPDatasetPropertyInformationDto.xadEnableNonNettable = dataTable.Rows[0].Field<bool>("xadEnableNonNettable");
			eRPDatasetPropertyInformationDto.xadEnableOutlookDesktop = dataTable.Rows[0].Field<bool>("xadEnableOutlookDesktop");
			eRPDatasetPropertyInformationDto.xadEnableOutlookOnline = dataTable.Rows[0].Field<bool>("xadEnableOutlookOnline");
			eRPDatasetPropertyInformationDto.xadExportFollowups = dataTable.Rows[0].Field<bool>("xadExportFollowups");
			eRPDatasetPropertyInformationDto.xadExtendedSearchOptions = dataTable.Rows[0].Field<bool>("xadExtendedSearchOptions");
			eRPDatasetPropertyInformationDto.xadIgnoreSSLCertValidate = dataTable.Rows[0].Field<bool>("xadIgnoreSSLCertValidate");
			eRPDatasetPropertyInformationDto.xadSuppressAddressOnReports = dataTable.Rows[0].Field<bool>("xadSuppressAddressOnReports");
			eRPDatasetPropertyInformationDto.xadUpdateMasterDataInFinPkg = dataTable.Rows[0].Field<bool>("xadUpdateMasterDataInFinPkg");
			eRPDatasetPropertyInformationDto.xadViewForeign = dataTable.Rows[0].Field<bool>("xadViewForeign");
			eRPDatasetPropertyInformationDto.xadLanguage = dataTable.Rows[0].Field<string>("xadLanguage");
			eRPDatasetPropertyInformationDto.xadMailProvider = dataTable.Rows[0].Field<string>("xadMailProvider");
			eRPDatasetPropertyInformationDto.xadMailServer = dataTable.Rows[0].Field<string>("xadMailServer");
			eRPDatasetPropertyInformationDto.xadMaxGridRow = dataTable.Rows[0].Field<int>("xadMaxGridRow");
			eRPDatasetPropertyInformationDto.xadMaxItemsOnGantt = dataTable.Rows[0].Field<int>("xadMaxItemsOnGantt");
			eRPDatasetPropertyInformationDto.xadName = dataTable.Rows[0].Field<string>("xadName");
			eRPDatasetPropertyInformationDto.xadPhoneNumber = dataTable.Rows[0].Field<string>("xadPhoneNumber");
			eRPDatasetPropertyInformationDto.xadPostCode = dataTable.Rows[0].Field<string>("xadPostCode");
			eRPDatasetPropertyInformationDto.xadRegion = dataTable.Rows[0].Field<string>("xadRegion");
			eRPDatasetPropertyInformationDto.xadRowVersion = dataTable.Rows[0].Field<byte[]>("xadRowVersion");
			eRPDatasetPropertyInformationDto.xadSellQuantityDecimals = dataTable.Rows[0].Field<byte>("xadSellQuantityDecimals");
			eRPDatasetPropertyInformationDto.xadState = dataTable.Rows[0].Field<string>("xadState");
			eRPDatasetPropertyInformationDto.xadTimeFormat = dataTable.Rows[0].Field<byte>("xadTimeFormat");
			eRPDatasetPropertyInformationDto.xadTimeZone = dataTable.Rows[0].Field<string>("xadTimeZone");
			eRPDatasetPropertyInformationDto.xadTINType = dataTable.Rows[0].Field<string>("xadTINType");
			eRPDatasetPropertyInformationDto.xadUpgradeVersions = dataTable.Rows[0].Field<string>("xadUpgradeVersions");
			eRPDatasetPropertyInformationDto.xadUpsInterfaceFolderName = dataTable.Rows[0].Field<string>("xadUpsInterfaceFolderName");
			eRPDatasetPropertyInformationDto.xadVersion = dataTable.Rows[0].Field<string>("xadVersion");
			eRPDatasetPropertyInformationDto.xadVersion92UpgradeDate = dataTable.Rows[0].Field<DateTime?>("xadVersion92UpgradeDate");
			eRPDatasetPropertyInformationDto.xadWebAddress = dataTable.Rows[0].Field<string>("xadWebAddress");
			eRPDatasetPropertyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPDatasetPropertyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPDatasetPropertyInformationDto);
	}
}
