using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPDatasetPropertyModel : ERPBaseModel, IERPDatasetPropertyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllDatasetProperties(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPDatasetPropertyRepository iERPDatasetPropertyRepository = (base.ERPDatasetPropertyRepository = new ERPDatasetPropertyRepository(base.ApiClientContext));
		using (iERPDatasetPropertyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPDatasetPropertyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPDatasetPropertyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPDatasetPropertyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPDatasetPropertyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetDatasetProperty(Guid datasetPropertyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDatasetPropertyRepository iERPDatasetPropertyRepository = (base.ERPDatasetPropertyRepository = new ERPDatasetPropertyRepository(base.ApiClientContext));
		using (iERPDatasetPropertyRepository)
		{
			if (!(await base.ERPDatasetPropertyRepository.DoesDatasetPropertyExist(datasetPropertyId)))
			{
				errorsList.Add($"DatasetProperty [{datasetPropertyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPDatasetPropertyDto>>> Process_GetAllDatasetProperties(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPDatasetPropertyDto> allDatasetPropertiesDto = new List<ERPDatasetPropertyDto>();
		ERPResponseMessageDto<IList<ERPDatasetPropertyDto>> result;
		try
		{
			IERPDatasetPropertyRepository iERPDatasetPropertyRepository = (base.ERPDatasetPropertyRepository = new ERPDatasetPropertyRepository(base.ApiClientContext));
			using (iERPDatasetPropertyRepository)
			{
				foreach (ERPDatasetPropertyInformationDto item2 in await base.ERPDatasetPropertyRepository.GetAllDatasetProperties(pageSize, pageNumber, filter, orderBy))
				{
					ERPDatasetPropertyDto item = new ERPDatasetPropertyDto
					{
						xadAddressLine1 = item2.xadAddressLine1,
						xadAddressLine2 = item2.xadAddressLine2,
						xadAddressLine3 = item2.xadAddressLine3,
						xadBankAccountID = item2.xadBankAccountID,
						xadBuyQuantityDecimals = item2.xadBuyQuantityDecimals,
						xadCArPPRegistrationNumber = item2.xadCArPPRegistrationNumber,
						xadCASubmissionReference = item2.xadCASubmissionReference,
						xadCATransmitterNumber = item2.xadCATransmitterNumber,
						xadCity = item2.xadCity,
						xadColor = item2.xadColor,
						xadCompanyMessageRTF = item2.xadCompanyMessageRTF,
						xadCompanyMessageText = item2.xadCompanyMessageText,
						xadCountry = item2.xadCountry,
						xadCountryCode = item2.xadCountryCode,
						xadCreatedBy = item2.xadCreatedBy,
						xadCreatedDate = item2.xadCreatedDate,
						xadCreditCardBankAccountID = item2.xadCreditCardBankAccountID,
						xadCurrencyRateID = item2.xadCurrencyRateID,
						xadDayStartTimeFri = item2.xadDayStartTimeFri,
						xadDayStartTimeMon = item2.xadDayStartTimeMon,
						xadDayStartTimeSat = item2.xadDayStartTimeSat,
						xadDayStartTimeSun = item2.xadDayStartTimeSun,
						xadDayStartTimeThu = item2.xadDayStartTimeThu,
						xadDayStartTimeTue = item2.xadDayStartTimeTue,
						xadDayStartTimeWed = item2.xadDayStartTimeWed,
						xadDescription = item2.xadDescription,
						xadEmailAddress = item2.xadEmailAddress,
						xadUniqueID = item2.xadUniqueID,
						xadExtensionVersions = item2.xadExtensionVersions,
						xadFaxNumber = item2.xadFaxNumber,
						xadFederalID = item2.xadFederalID,
						xadForeColor = item2.xadForeColor,
						xadGlChartPrefix = item2.xadGlChartPrefix,
						xadGlDepartmentID = item2.xadGlDepartmentID,
						xadGlDivisionID = item2.xadGlDivisionID,
						xadHoursFri = item2.xadHoursFri,
						xadHoursMon = item2.xadHoursMon,
						xadHoursSat = item2.xadHoursSat,
						xadHoursSun = item2.xadHoursSun,
						xadHoursThu = item2.xadHoursThu,
						xadHoursTue = item2.xadHoursTue,
						xadHoursWed = item2.xadHoursWed,
						xadIntraCompanyOrganizationID = item2.xadIntraCompanyOrganizationID,
						xadInventoryQuantityDecimals = item2.xadInventoryQuantityDecimals,
						xadAllowIntraCompanyTrans = item2.xadAllowIntraCompanyTrans,
						xadBackupCheck = item2.xadBackupCheck,
						xadDisableLotNumbers = item2.xadDisableLotNumbers,
						xadDisableOrganizationParts = item2.xadDisableOrganizationParts,
						xadDisableRetention = item2.xadDisableRetention,
						xadDisableRevisions = item2.xadDisableRevisions,
						xadDisableSerialNumbers = item2.xadDisableSerialNumbers,
						xadDisableWarehouses = item2.xadDisableWarehouses,
						xadEditInExplorers = item2.xadEditInExplorers,
						xadEnableM1Email = item2.xadEnableM1Email,
						xadEnableM1Home = item2.xadEnableM1Home,
						xadEnableMultiCurrency = item2.xadEnableMultiCurrency,
						xadEnableNonNettable = item2.xadEnableNonNettable,
						xadEnableOutlookDesktop = item2.xadEnableOutlookDesktop,
						xadEnableOutlookOnline = item2.xadEnableOutlookOnline,
						xadExportFollowups = item2.xadExportFollowups,
						xadExtendedSearchOptions = item2.xadExtendedSearchOptions,
						xadIgnoreSSLCertValidate = item2.xadIgnoreSSLCertValidate,
						xadSuppressAddressOnReports = item2.xadSuppressAddressOnReports,
						xadUpdateMasterDataInFinPkg = item2.xadUpdateMasterDataInFinPkg,
						xadViewForeign = item2.xadViewForeign,
						xadLanguage = item2.xadLanguage,
						xadMailProvider = item2.xadMailProvider,
						xadMailServer = item2.xadMailServer,
						xadMaxGridRow = item2.xadMaxGridRow,
						xadMaxItemsOnGantt = item2.xadMaxItemsOnGantt,
						xadName = item2.xadName,
						xadPhoneNumber = item2.xadPhoneNumber,
						xadPostCode = item2.xadPostCode,
						xadRegion = item2.xadRegion,
						xadRowVersion = item2.xadRowVersion,
						xadSellQuantityDecimals = item2.xadSellQuantityDecimals,
						xadState = item2.xadState,
						xadTimeFormat = item2.xadTimeFormat,
						xadTimeZone = item2.xadTimeZone,
						xadTINType = item2.xadTINType,
						xadUpgradeVersions = item2.xadUpgradeVersions,
						xadUpsInterfaceFolderName = item2.xadUpsInterfaceFolderName,
						xadVersion = item2.xadVersion,
						xadVersion92UpgradeDate = item2.xadVersion92UpgradeDate,
						xadWebAddress = item2.xadWebAddress,
						CustomFields = item2.CustomFields
					};
					allDatasetPropertiesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all DatasetProperties]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPDatasetPropertyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allDatasetPropertiesDto,
				RecordCount = allDatasetPropertiesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDatasetPropertyDto>> Process_GetDatasetProperty(Guid datasetPropertyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPDatasetPropertyDto datasetPropertyDto = null;
		ERPResponseMessageDto<ERPDatasetPropertyDto> result;
		try
		{
			IERPDatasetPropertyRepository iERPDatasetPropertyRepository = (base.ERPDatasetPropertyRepository = new ERPDatasetPropertyRepository(base.ApiClientContext));
			using (iERPDatasetPropertyRepository)
			{
				ERPDatasetPropertyInformationDto eRPDatasetPropertyInformationDto = await base.ERPDatasetPropertyRepository.GetDatasetProperty(datasetPropertyId);
				datasetPropertyDto = new ERPDatasetPropertyDto
				{
					xadAddressLine1 = eRPDatasetPropertyInformationDto.xadAddressLine1,
					xadAddressLine2 = eRPDatasetPropertyInformationDto.xadAddressLine2,
					xadAddressLine3 = eRPDatasetPropertyInformationDto.xadAddressLine3,
					xadBankAccountID = eRPDatasetPropertyInformationDto.xadBankAccountID,
					xadBuyQuantityDecimals = eRPDatasetPropertyInformationDto.xadBuyQuantityDecimals,
					xadCArPPRegistrationNumber = eRPDatasetPropertyInformationDto.xadCArPPRegistrationNumber,
					xadCASubmissionReference = eRPDatasetPropertyInformationDto.xadCASubmissionReference,
					xadCATransmitterNumber = eRPDatasetPropertyInformationDto.xadCATransmitterNumber,
					xadCity = eRPDatasetPropertyInformationDto.xadCity,
					xadColor = eRPDatasetPropertyInformationDto.xadColor,
					xadCompanyMessageRTF = eRPDatasetPropertyInformationDto.xadCompanyMessageRTF,
					xadCompanyMessageText = eRPDatasetPropertyInformationDto.xadCompanyMessageText,
					xadCountry = eRPDatasetPropertyInformationDto.xadCountry,
					xadCountryCode = eRPDatasetPropertyInformationDto.xadCountryCode,
					xadCreatedBy = eRPDatasetPropertyInformationDto.xadCreatedBy,
					xadCreatedDate = eRPDatasetPropertyInformationDto.xadCreatedDate,
					xadCreditCardBankAccountID = eRPDatasetPropertyInformationDto.xadCreditCardBankAccountID,
					xadCurrencyRateID = eRPDatasetPropertyInformationDto.xadCurrencyRateID,
					xadDayStartTimeFri = eRPDatasetPropertyInformationDto.xadDayStartTimeFri,
					xadDayStartTimeMon = eRPDatasetPropertyInformationDto.xadDayStartTimeMon,
					xadDayStartTimeSat = eRPDatasetPropertyInformationDto.xadDayStartTimeSat,
					xadDayStartTimeSun = eRPDatasetPropertyInformationDto.xadDayStartTimeSun,
					xadDayStartTimeThu = eRPDatasetPropertyInformationDto.xadDayStartTimeThu,
					xadDayStartTimeTue = eRPDatasetPropertyInformationDto.xadDayStartTimeTue,
					xadDayStartTimeWed = eRPDatasetPropertyInformationDto.xadDayStartTimeWed,
					xadDescription = eRPDatasetPropertyInformationDto.xadDescription,
					xadEmailAddress = eRPDatasetPropertyInformationDto.xadEmailAddress,
					xadUniqueID = eRPDatasetPropertyInformationDto.xadUniqueID,
					xadExtensionVersions = eRPDatasetPropertyInformationDto.xadExtensionVersions,
					xadFaxNumber = eRPDatasetPropertyInformationDto.xadFaxNumber,
					xadFederalID = eRPDatasetPropertyInformationDto.xadFederalID,
					xadForeColor = eRPDatasetPropertyInformationDto.xadForeColor,
					xadGlChartPrefix = eRPDatasetPropertyInformationDto.xadGlChartPrefix,
					xadGlDepartmentID = eRPDatasetPropertyInformationDto.xadGlDepartmentID,
					xadGlDivisionID = eRPDatasetPropertyInformationDto.xadGlDivisionID,
					xadHoursFri = eRPDatasetPropertyInformationDto.xadHoursFri,
					xadHoursMon = eRPDatasetPropertyInformationDto.xadHoursMon,
					xadHoursSat = eRPDatasetPropertyInformationDto.xadHoursSat,
					xadHoursSun = eRPDatasetPropertyInformationDto.xadHoursSun,
					xadHoursThu = eRPDatasetPropertyInformationDto.xadHoursThu,
					xadHoursTue = eRPDatasetPropertyInformationDto.xadHoursTue,
					xadHoursWed = eRPDatasetPropertyInformationDto.xadHoursWed,
					xadIntraCompanyOrganizationID = eRPDatasetPropertyInformationDto.xadIntraCompanyOrganizationID,
					xadInventoryQuantityDecimals = eRPDatasetPropertyInformationDto.xadInventoryQuantityDecimals,
					xadAllowIntraCompanyTrans = eRPDatasetPropertyInformationDto.xadAllowIntraCompanyTrans,
					xadBackupCheck = eRPDatasetPropertyInformationDto.xadBackupCheck,
					xadDisableLotNumbers = eRPDatasetPropertyInformationDto.xadDisableLotNumbers,
					xadDisableOrganizationParts = eRPDatasetPropertyInformationDto.xadDisableOrganizationParts,
					xadDisableRetention = eRPDatasetPropertyInformationDto.xadDisableRetention,
					xadDisableRevisions = eRPDatasetPropertyInformationDto.xadDisableRevisions,
					xadDisableSerialNumbers = eRPDatasetPropertyInformationDto.xadDisableSerialNumbers,
					xadDisableWarehouses = eRPDatasetPropertyInformationDto.xadDisableWarehouses,
					xadEditInExplorers = eRPDatasetPropertyInformationDto.xadEditInExplorers,
					xadEnableM1Email = eRPDatasetPropertyInformationDto.xadEnableM1Email,
					xadEnableM1Home = eRPDatasetPropertyInformationDto.xadEnableM1Home,
					xadEnableMultiCurrency = eRPDatasetPropertyInformationDto.xadEnableMultiCurrency,
					xadEnableNonNettable = eRPDatasetPropertyInformationDto.xadEnableNonNettable,
					xadEnableOutlookDesktop = eRPDatasetPropertyInformationDto.xadEnableOutlookDesktop,
					xadEnableOutlookOnline = eRPDatasetPropertyInformationDto.xadEnableOutlookOnline,
					xadExportFollowups = eRPDatasetPropertyInformationDto.xadExportFollowups,
					xadExtendedSearchOptions = eRPDatasetPropertyInformationDto.xadExtendedSearchOptions,
					xadIgnoreSSLCertValidate = eRPDatasetPropertyInformationDto.xadIgnoreSSLCertValidate,
					xadSuppressAddressOnReports = eRPDatasetPropertyInformationDto.xadSuppressAddressOnReports,
					xadUpdateMasterDataInFinPkg = eRPDatasetPropertyInformationDto.xadUpdateMasterDataInFinPkg,
					xadViewForeign = eRPDatasetPropertyInformationDto.xadViewForeign,
					xadLanguage = eRPDatasetPropertyInformationDto.xadLanguage,
					xadMailProvider = eRPDatasetPropertyInformationDto.xadMailProvider,
					xadMailServer = eRPDatasetPropertyInformationDto.xadMailServer,
					xadMaxGridRow = eRPDatasetPropertyInformationDto.xadMaxGridRow,
					xadMaxItemsOnGantt = eRPDatasetPropertyInformationDto.xadMaxItemsOnGantt,
					xadName = eRPDatasetPropertyInformationDto.xadName,
					xadPhoneNumber = eRPDatasetPropertyInformationDto.xadPhoneNumber,
					xadPostCode = eRPDatasetPropertyInformationDto.xadPostCode,
					xadRegion = eRPDatasetPropertyInformationDto.xadRegion,
					xadRowVersion = eRPDatasetPropertyInformationDto.xadRowVersion,
					xadSellQuantityDecimals = eRPDatasetPropertyInformationDto.xadSellQuantityDecimals,
					xadState = eRPDatasetPropertyInformationDto.xadState,
					xadTimeFormat = eRPDatasetPropertyInformationDto.xadTimeFormat,
					xadTimeZone = eRPDatasetPropertyInformationDto.xadTimeZone,
					xadTINType = eRPDatasetPropertyInformationDto.xadTINType,
					xadUpgradeVersions = eRPDatasetPropertyInformationDto.xadUpgradeVersions,
					xadUpsInterfaceFolderName = eRPDatasetPropertyInformationDto.xadUpsInterfaceFolderName,
					xadVersion = eRPDatasetPropertyInformationDto.xadVersion,
					xadVersion92UpgradeDate = eRPDatasetPropertyInformationDto.xadVersion92UpgradeDate,
					xadWebAddress = eRPDatasetPropertyInformationDto.xadWebAddress,
					CustomFields = eRPDatasetPropertyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the DatasetProperties []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDatasetPropertyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = datasetPropertyDto
			};
		}
		return result;
	}
}
