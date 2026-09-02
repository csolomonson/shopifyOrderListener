using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShippingPropertyModel : ERPBaseModel, IERPShippingPropertyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShippingProperties(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShippingPropertyRepository iERPShippingPropertyRepository = (base.ERPShippingPropertyRepository = new ERPShippingPropertyRepository(base.ApiClientContext));
		using (iERPShippingPropertyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShippingPropertyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShippingPropertyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShippingPropertyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShippingPropertyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShippingProperty(Guid shippingPropertyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShippingPropertyRepository iERPShippingPropertyRepository = (base.ERPShippingPropertyRepository = new ERPShippingPropertyRepository(base.ApiClientContext));
		using (iERPShippingPropertyRepository)
		{
			if (!(await base.ERPShippingPropertyRepository.DoesShippingPropertyExist(shippingPropertyId)))
			{
				errorsList.Add($"ShippingProperty [{shippingPropertyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShippingPropertyDto>>> Process_GetAllShippingProperties(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShippingPropertyDto> allShippingPropertiesDto = new List<ERPShippingPropertyDto>();
		ERPResponseMessageDto<IList<ERPShippingPropertyDto>> result;
		try
		{
			IERPShippingPropertyRepository iERPShippingPropertyRepository = (base.ERPShippingPropertyRepository = new ERPShippingPropertyRepository(base.ApiClientContext));
			using (iERPShippingPropertyRepository)
			{
				foreach (ERPShippingPropertyInformationDto item2 in await base.ERPShippingPropertyRepository.GetAllShippingProperties(pageSize, pageNumber, filter, orderBy))
				{
					ERPShippingPropertyDto item = new ERPShippingPropertyDto
					{
						xsmCreatedBy = item2.xsmCreatedBy,
						xsmCreatedDate = item2.xsmCreatedDate,
						xsmUniqueID = item2.xsmUniqueID,
						xsmFdxAccessibility = item2.xsmFdxAccessibility,
						xsmFdxAccountNumber = item2.xsmFdxAccountNumber,
						xsmFdxAccountNumberOAuth = item2.xsmFdxAccountNumberOAuth,
						xsmFdxAddressLine1 = item2.xsmFdxAddressLine1,
						xsmFdxAddressLine2 = item2.xsmFdxAddressLine2,
						xsmFdxAddrValAccuracyIndicator = item2.xsmFdxAddrValAccuracyIndicator,
						xsmFdxCity = item2.xsmFdxCity,
						xsmFdxClientID = item2.xsmFdxClientID,
						xsmFdxClientIDTrack = item2.xsmFdxClientIDTrack,
						xsmFdxClientSecret = item2.xsmFdxClientSecret,
						xsmFdxClientSecretTrack = item2.xsmFdxClientSecretTrack,
						xsmFdxCodCollectionAmount = item2.xsmFdxCodCollectionAmount,
						xsmFdxCodCollectionType = item2.xsmFdxCodCollectionType,
						xsmFdxCountry = item2.xsmFdxCountry,
						xsmFdxCurrencyType = item2.xsmFdxCurrencyType,
						xsmFdxDeclaredValueCurrency = item2.xsmFdxDeclaredValueCurrency,
						xsmFdxDepartment = item2.xsmFdxDepartment,
						xsmFdxDimensionsUnitOfMeasure = item2.xsmFdxDimensionsUnitOfMeasure,
						xsmFdxDropOffType = item2.xsmFdxDropOffType,
						xsmFdxEmailAddress = item2.xsmFdxEmailAddress,
						xsmFdxFaxNumber = item2.xsmFdxFaxNumber,
						xsmFdxHandlingCost = item2.xsmFdxHandlingCost,
						xsmFdxHomeDeliveryDate = item2.xsmFdxHomeDeliveryDate,
						xsmFdxHomeDeliveryType = item2.xsmFdxHomeDeliveryType,
						xsmFdxHostAddress = item2.xsmFdxHostAddress,
						xsmFdxHostPort = item2.xsmFdxHostPort,
						xsmFdxHostService = item2.xsmFdxHostService,
						xsmFdxLabelFormatType = item2.xsmFdxLabelFormatType,
						xsmFdxLabelImageType = item2.xsmFdxLabelImageType,
						xsmFdxLabelStockType = item2.xsmFdxLabelStockType,
						xsmFdxLabelStoreLocation = item2.xsmFdxLabelStoreLocation,
						xsmFdxLabelType = item2.xsmFdxLabelType,
						xsmFdxLblPrintOrientType = item2.xsmFdxLblPrintOrientType,
						xsmFdxMeterNumber = item2.xsmFdxMeterNumber,
						xsmFdxName = item2.xsmFdxName,
						xsmFdxPackageHeight = item2.xsmFdxPackageHeight,
						xsmFdxPackageLength = item2.xsmFdxPackageLength,
						xsmFdxPackageWidth = item2.xsmFdxPackageWidth,
						xsmFdxPackaging = item2.xsmFdxPackaging,
						xsmFdxPackagingCost = item2.xsmFdxPackagingCost,
						xsmFdxPagerNumber = item2.xsmFdxPagerNumber,
						xsmFdxPayorType = item2.xsmFdxPayorType,
						xsmFdxPersonName = item2.xsmFdxPersonName,
						xsmFdxPhoneNumber = item2.xsmFdxPhoneNumber,
						xsmFdxPostCode = item2.xsmFdxPostCode,
						xsmFdxRateElementBasis = item2.xsmFdxRateElementBasis,
						xsmFdxRateRequestType = item2.xsmFdxRateRequestType,
						xsmFdxRateTypeBasis = item2.xsmFdxRateTypeBasis,
						xsmFdxReturnShipIndicator = item2.xsmFdxReturnShipIndicator,
						xsmFdxShipCostMarkupPct = item2.xsmFdxShipCostMarkupPct,
						xsmFdxShipDocImageType = item2.xsmFdxShipDocImageType,
						xsmFdxSignatureOption = item2.xsmFdxSignatureOption,
						xsmFdxState = item2.xsmFdxState,
						xsmFdxSubscribedServices = item2.xsmFdxSubscribedServices,
						xsmFdxVHCAmountOrPercentage = item2.xsmFdxVHCAmountOrPercentage,
						xsmFdxVHCLevel = item2.xsmFdxVHCLevel,
						xsmFdxVHCType = item2.xsmFdxVHCType,
						xsmFdxWeightUnitOfMeasure = item2.xsmFdxWeightUnitOfMeasure,
						xsmFedExAccessKey = item2.xsmFedExAccessKey,
						xsmFedExAccessToken = item2.xsmFedExAccessToken,
						xsmFedExAccessTokenTrack = item2.xsmFedExAccessTokenTrack,
						xsmFedExAuthenticationMethod = item2.xsmFedExAuthenticationMethod,
						xsmFedExPassword = item2.xsmFedExPassword,
						xsmFedExTokenExpiresIn = item2.xsmFedExTokenExpiresIn,
						xsmFedExTokenExpiresInTrack = item2.xsmFedExTokenExpiresInTrack,
						xsmFedExUserName = item2.xsmFedExUserName,
						xsmFdxBareCostOfDuty = item2.xsmFdxBareCostOfDuty,
						xsmFdxBareTrasportationCost = item2.xsmFdxBareTrasportationCost,
						xsmFdxCod = item2.xsmFdxCod,
						xsmFdxHoldAtLocation = item2.xsmFdxHoldAtLocation,
						xsmFdxInsideDelivery = item2.xsmFdxInsideDelivery,
						xsmFdxInsidePickup = item2.xsmFdxInsidePickup,
						xsmFdxNonstandardContainer = item2.xsmFdxNonstandardContainer,
						xsmFdxOneItemPerShipment = item2.xsmFdxOneItemPerShipment,
						xsmFdxResidentialAddress = item2.xsmFdxResidentialAddress,
						xsmFdxSaturdayDelivery = item2.xsmFdxSaturdayDelivery,
						xsmFdxSaturdayPickup = item2.xsmFdxSaturdayPickup,
						xsmFedExIsProduction = item2.xsmFedExIsProduction,
						xsmUpsIsProduction = item2.xsmUpsIsProduction,
						xsmRowVersion = item2.xsmRowVersion,
						xsmUpsAccessKey = item2.xsmUpsAccessKey,
						xsmUpsAccessToken = item2.xsmUpsAccessToken,
						xsmUpsAccountNo = item2.xsmUpsAccountNo,
						xsmUpsAccountNoOAuth = item2.xsmUpsAccountNoOAuth,
						xsmUpsAuthenticationMethod = item2.xsmUpsAuthenticationMethod,
						xsmUpsLabelStockSize = item2.xsmUpsLabelStockSize,
						xsmUpsLabelStoreLocation = item2.xsmUpsLabelStoreLocation,
						xsmUpsLabelType = item2.xsmUpsLabelType,
						xsmUpsLocIDPref = item2.xsmUpsLocIDPref,
						xsmUpsLocPostCodePref = item2.xsmUpsLocPostCodePref,
						xsmUpsPassword = item2.xsmUpsPassword,
						xsmUpsRefreshToken = item2.xsmUpsRefreshToken,
						xsmUpsUsername = item2.xsmUpsUsername,
						xsmUSDcurrencyCode = item2.xsmUSDcurrencyCode,
						CustomFields = item2.CustomFields
					};
					allShippingPropertiesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShippingProperties]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShippingPropertyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShippingPropertiesDto,
				RecordCount = allShippingPropertiesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShippingPropertyDto>> Process_GetShippingProperty(Guid shippingPropertyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShippingPropertyDto shippingPropertyDto = null;
		ERPResponseMessageDto<ERPShippingPropertyDto> result;
		try
		{
			IERPShippingPropertyRepository iERPShippingPropertyRepository = (base.ERPShippingPropertyRepository = new ERPShippingPropertyRepository(base.ApiClientContext));
			using (iERPShippingPropertyRepository)
			{
				ERPShippingPropertyInformationDto eRPShippingPropertyInformationDto = await base.ERPShippingPropertyRepository.GetShippingProperty(shippingPropertyId);
				shippingPropertyDto = new ERPShippingPropertyDto
				{
					xsmCreatedBy = eRPShippingPropertyInformationDto.xsmCreatedBy,
					xsmCreatedDate = eRPShippingPropertyInformationDto.xsmCreatedDate,
					xsmUniqueID = eRPShippingPropertyInformationDto.xsmUniqueID,
					xsmFdxAccessibility = eRPShippingPropertyInformationDto.xsmFdxAccessibility,
					xsmFdxAccountNumber = eRPShippingPropertyInformationDto.xsmFdxAccountNumber,
					xsmFdxAccountNumberOAuth = eRPShippingPropertyInformationDto.xsmFdxAccountNumberOAuth,
					xsmFdxAddressLine1 = eRPShippingPropertyInformationDto.xsmFdxAddressLine1,
					xsmFdxAddressLine2 = eRPShippingPropertyInformationDto.xsmFdxAddressLine2,
					xsmFdxAddrValAccuracyIndicator = eRPShippingPropertyInformationDto.xsmFdxAddrValAccuracyIndicator,
					xsmFdxCity = eRPShippingPropertyInformationDto.xsmFdxCity,
					xsmFdxClientID = eRPShippingPropertyInformationDto.xsmFdxClientID,
					xsmFdxClientIDTrack = eRPShippingPropertyInformationDto.xsmFdxClientIDTrack,
					xsmFdxClientSecret = eRPShippingPropertyInformationDto.xsmFdxClientSecret,
					xsmFdxClientSecretTrack = eRPShippingPropertyInformationDto.xsmFdxClientSecretTrack,
					xsmFdxCodCollectionAmount = eRPShippingPropertyInformationDto.xsmFdxCodCollectionAmount,
					xsmFdxCodCollectionType = eRPShippingPropertyInformationDto.xsmFdxCodCollectionType,
					xsmFdxCountry = eRPShippingPropertyInformationDto.xsmFdxCountry,
					xsmFdxCurrencyType = eRPShippingPropertyInformationDto.xsmFdxCurrencyType,
					xsmFdxDeclaredValueCurrency = eRPShippingPropertyInformationDto.xsmFdxDeclaredValueCurrency,
					xsmFdxDepartment = eRPShippingPropertyInformationDto.xsmFdxDepartment,
					xsmFdxDimensionsUnitOfMeasure = eRPShippingPropertyInformationDto.xsmFdxDimensionsUnitOfMeasure,
					xsmFdxDropOffType = eRPShippingPropertyInformationDto.xsmFdxDropOffType,
					xsmFdxEmailAddress = eRPShippingPropertyInformationDto.xsmFdxEmailAddress,
					xsmFdxFaxNumber = eRPShippingPropertyInformationDto.xsmFdxFaxNumber,
					xsmFdxHandlingCost = eRPShippingPropertyInformationDto.xsmFdxHandlingCost,
					xsmFdxHomeDeliveryDate = eRPShippingPropertyInformationDto.xsmFdxHomeDeliveryDate,
					xsmFdxHomeDeliveryType = eRPShippingPropertyInformationDto.xsmFdxHomeDeliveryType,
					xsmFdxHostAddress = eRPShippingPropertyInformationDto.xsmFdxHostAddress,
					xsmFdxHostPort = eRPShippingPropertyInformationDto.xsmFdxHostPort,
					xsmFdxHostService = eRPShippingPropertyInformationDto.xsmFdxHostService,
					xsmFdxLabelFormatType = eRPShippingPropertyInformationDto.xsmFdxLabelFormatType,
					xsmFdxLabelImageType = eRPShippingPropertyInformationDto.xsmFdxLabelImageType,
					xsmFdxLabelStockType = eRPShippingPropertyInformationDto.xsmFdxLabelStockType,
					xsmFdxLabelStoreLocation = eRPShippingPropertyInformationDto.xsmFdxLabelStoreLocation,
					xsmFdxLabelType = eRPShippingPropertyInformationDto.xsmFdxLabelType,
					xsmFdxLblPrintOrientType = eRPShippingPropertyInformationDto.xsmFdxLblPrintOrientType,
					xsmFdxMeterNumber = eRPShippingPropertyInformationDto.xsmFdxMeterNumber,
					xsmFdxName = eRPShippingPropertyInformationDto.xsmFdxName,
					xsmFdxPackageHeight = eRPShippingPropertyInformationDto.xsmFdxPackageHeight,
					xsmFdxPackageLength = eRPShippingPropertyInformationDto.xsmFdxPackageLength,
					xsmFdxPackageWidth = eRPShippingPropertyInformationDto.xsmFdxPackageWidth,
					xsmFdxPackaging = eRPShippingPropertyInformationDto.xsmFdxPackaging,
					xsmFdxPackagingCost = eRPShippingPropertyInformationDto.xsmFdxPackagingCost,
					xsmFdxPagerNumber = eRPShippingPropertyInformationDto.xsmFdxPagerNumber,
					xsmFdxPayorType = eRPShippingPropertyInformationDto.xsmFdxPayorType,
					xsmFdxPersonName = eRPShippingPropertyInformationDto.xsmFdxPersonName,
					xsmFdxPhoneNumber = eRPShippingPropertyInformationDto.xsmFdxPhoneNumber,
					xsmFdxPostCode = eRPShippingPropertyInformationDto.xsmFdxPostCode,
					xsmFdxRateElementBasis = eRPShippingPropertyInformationDto.xsmFdxRateElementBasis,
					xsmFdxRateRequestType = eRPShippingPropertyInformationDto.xsmFdxRateRequestType,
					xsmFdxRateTypeBasis = eRPShippingPropertyInformationDto.xsmFdxRateTypeBasis,
					xsmFdxReturnShipIndicator = eRPShippingPropertyInformationDto.xsmFdxReturnShipIndicator,
					xsmFdxShipCostMarkupPct = eRPShippingPropertyInformationDto.xsmFdxShipCostMarkupPct,
					xsmFdxShipDocImageType = eRPShippingPropertyInformationDto.xsmFdxShipDocImageType,
					xsmFdxSignatureOption = eRPShippingPropertyInformationDto.xsmFdxSignatureOption,
					xsmFdxState = eRPShippingPropertyInformationDto.xsmFdxState,
					xsmFdxSubscribedServices = eRPShippingPropertyInformationDto.xsmFdxSubscribedServices,
					xsmFdxVHCAmountOrPercentage = eRPShippingPropertyInformationDto.xsmFdxVHCAmountOrPercentage,
					xsmFdxVHCLevel = eRPShippingPropertyInformationDto.xsmFdxVHCLevel,
					xsmFdxVHCType = eRPShippingPropertyInformationDto.xsmFdxVHCType,
					xsmFdxWeightUnitOfMeasure = eRPShippingPropertyInformationDto.xsmFdxWeightUnitOfMeasure,
					xsmFedExAccessKey = eRPShippingPropertyInformationDto.xsmFedExAccessKey,
					xsmFedExAccessToken = eRPShippingPropertyInformationDto.xsmFedExAccessToken,
					xsmFedExAccessTokenTrack = eRPShippingPropertyInformationDto.xsmFedExAccessTokenTrack,
					xsmFedExAuthenticationMethod = eRPShippingPropertyInformationDto.xsmFedExAuthenticationMethod,
					xsmFedExPassword = eRPShippingPropertyInformationDto.xsmFedExPassword,
					xsmFedExTokenExpiresIn = eRPShippingPropertyInformationDto.xsmFedExTokenExpiresIn,
					xsmFedExTokenExpiresInTrack = eRPShippingPropertyInformationDto.xsmFedExTokenExpiresInTrack,
					xsmFedExUserName = eRPShippingPropertyInformationDto.xsmFedExUserName,
					xsmFdxBareCostOfDuty = eRPShippingPropertyInformationDto.xsmFdxBareCostOfDuty,
					xsmFdxBareTrasportationCost = eRPShippingPropertyInformationDto.xsmFdxBareTrasportationCost,
					xsmFdxCod = eRPShippingPropertyInformationDto.xsmFdxCod,
					xsmFdxHoldAtLocation = eRPShippingPropertyInformationDto.xsmFdxHoldAtLocation,
					xsmFdxInsideDelivery = eRPShippingPropertyInformationDto.xsmFdxInsideDelivery,
					xsmFdxInsidePickup = eRPShippingPropertyInformationDto.xsmFdxInsidePickup,
					xsmFdxNonstandardContainer = eRPShippingPropertyInformationDto.xsmFdxNonstandardContainer,
					xsmFdxOneItemPerShipment = eRPShippingPropertyInformationDto.xsmFdxOneItemPerShipment,
					xsmFdxResidentialAddress = eRPShippingPropertyInformationDto.xsmFdxResidentialAddress,
					xsmFdxSaturdayDelivery = eRPShippingPropertyInformationDto.xsmFdxSaturdayDelivery,
					xsmFdxSaturdayPickup = eRPShippingPropertyInformationDto.xsmFdxSaturdayPickup,
					xsmFedExIsProduction = eRPShippingPropertyInformationDto.xsmFedExIsProduction,
					xsmUpsIsProduction = eRPShippingPropertyInformationDto.xsmUpsIsProduction,
					xsmRowVersion = eRPShippingPropertyInformationDto.xsmRowVersion,
					xsmUpsAccessKey = eRPShippingPropertyInformationDto.xsmUpsAccessKey,
					xsmUpsAccessToken = eRPShippingPropertyInformationDto.xsmUpsAccessToken,
					xsmUpsAccountNo = eRPShippingPropertyInformationDto.xsmUpsAccountNo,
					xsmUpsAccountNoOAuth = eRPShippingPropertyInformationDto.xsmUpsAccountNoOAuth,
					xsmUpsAuthenticationMethod = eRPShippingPropertyInformationDto.xsmUpsAuthenticationMethod,
					xsmUpsLabelStockSize = eRPShippingPropertyInformationDto.xsmUpsLabelStockSize,
					xsmUpsLabelStoreLocation = eRPShippingPropertyInformationDto.xsmUpsLabelStoreLocation,
					xsmUpsLabelType = eRPShippingPropertyInformationDto.xsmUpsLabelType,
					xsmUpsLocIDPref = eRPShippingPropertyInformationDto.xsmUpsLocIDPref,
					xsmUpsLocPostCodePref = eRPShippingPropertyInformationDto.xsmUpsLocPostCodePref,
					xsmUpsPassword = eRPShippingPropertyInformationDto.xsmUpsPassword,
					xsmUpsRefreshToken = eRPShippingPropertyInformationDto.xsmUpsRefreshToken,
					xsmUpsUsername = eRPShippingPropertyInformationDto.xsmUpsUsername,
					xsmUSDcurrencyCode = eRPShippingPropertyInformationDto.xsmUSDcurrencyCode,
					CustomFields = eRPShippingPropertyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShippingProperties []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShippingPropertyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shippingPropertyDto
			};
		}
		return result;
	}
}
