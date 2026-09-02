using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShippingMethodModel : ERPBaseModel, IERPShippingMethodModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShippingMethods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShippingMethodRepository iERPShippingMethodRepository = (base.ERPShippingMethodRepository = new ERPShippingMethodRepository(base.ApiClientContext));
		using (iERPShippingMethodRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShippingMethodRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShippingMethodRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShippingMethodRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShippingMethodRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShippingMethod(Guid shippingMethodId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShippingMethodRepository iERPShippingMethodRepository = (base.ERPShippingMethodRepository = new ERPShippingMethodRepository(base.ApiClientContext));
		using (iERPShippingMethodRepository)
		{
			if (!(await base.ERPShippingMethodRepository.DoesShippingMethodExist(shippingMethodId)))
			{
				errorsList.Add($"ShippingMethod [{shippingMethodId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutShippingMethod(ERPShippingMethodDto shippingMethod)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShippingMethodRepository iERPShippingMethodRepository = (base.ERPShippingMethodRepository = new ERPShippingMethodRepository(base.ApiClientContext));
		using (iERPShippingMethodRepository)
		{
			if (!string.IsNullOrWhiteSpace(shippingMethod.xasShippingPaymentTypeID) && !(await base.ERPShippingMethodRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { shippingMethod.xasShippingPaymentTypeID })))
			{
				errorsList.Add("xasShippingPaymentTypeID [" + shippingMethod.xasShippingPaymentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shippingMethod.xasTaxCodeID) && !(await base.ERPShippingMethodRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { shippingMethod.xasTaxCodeID })))
			{
				errorsList.Add("xasTaxCodeID [" + shippingMethod.xasTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shippingMethod.xasSecondTaxCodeID) && !(await base.ERPShippingMethodRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { shippingMethod.xasSecondTaxCodeID })))
			{
				errorsList.Add("xasSecondTaxCodeID [" + shippingMethod.xasSecondTaxCodeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShippingMethodDto>>> Process_GetAllShippingMethods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShippingMethodDto> allShippingMethodsDto = new List<ERPShippingMethodDto>();
		ERPResponseMessageDto<IList<ERPShippingMethodDto>> result;
		try
		{
			IERPShippingMethodRepository iERPShippingMethodRepository = (base.ERPShippingMethodRepository = new ERPShippingMethodRepository(base.ApiClientContext));
			using (iERPShippingMethodRepository)
			{
				foreach (ERPShippingMethodInformationDto item2 in await base.ERPShippingMethodRepository.GetAllShippingMethods(pageSize, pageNumber, filter, orderBy))
				{
					ERPShippingMethodDto item = new ERPShippingMethodDto
					{
						xasAvalaraTaxCodeID = item2.xasAvalaraTaxCodeID,
						xasCarrier = item2.xasCarrier,
						xasCarrierAccountNumber = item2.xasCarrierAccountNumber,
						xasShippingMethodID = item2.xasShippingMethodID,
						xasCreatedBy = item2.xasCreatedBy,
						xasCreatedDate = item2.xasCreatedDate,
						xasDescription = item2.xasDescription,
						xasDistributeCostsOption = item2.xasDistributeCostsOption,
						xasUniqueID = item2.xasUniqueID,
						xasFdxAccessibility = item2.xasFdxAccessibility,
						xasFdxCodCollectionType = item2.xasFdxCodCollectionType,
						xasFdxDropOffType = item2.xasFdxDropOffType,
						xasFdxHomeDeliveryType = item2.xasFdxHomeDeliveryType,
						xasFdxPackageType = item2.xasFdxPackageType,
						xasFdxRateElementBasis = item2.xasFdxRateElementBasis,
						xasFdxRateRequestType = item2.xasFdxRateRequestType,
						xasFdxRateTypeBasis = item2.xasFdxRateTypeBasis,
						xasFdxReturnShipIndicator = item2.xasFdxReturnShipIndicator,
						xasFdxService = item2.xasFdxService,
						xasFdxSignatureOption = item2.xasFdxSignatureOption,
						xasFdxVHCAmountOrPercentage = item2.xasFdxVHCAmountOrPercentage,
						xasFdxVHCLevel = item2.xasFdxVHCLevel,
						xasFdxVHCType = item2.xasFdxVHCType,
						xasFedExBillingOption = item2.xasFedExBillingOption,
						xasInactiveDate = item2.xasInactiveDate,
						xasInactive = item2.xasInactive,
						xasFdxCertificateOfOrigin = item2.xasFdxCertificateOfOrigin,
						xasFdxCod = item2.xasFdxCod,
						xasFdxCommercialInvoice = item2.xasFdxCommercialInvoice,
						xasFdxExportDeclaration = item2.xasFdxExportDeclaration,
						xasFdxHoldAtLocation = item2.xasFdxHoldAtLocation,
						xasFdxInsideDelivery = item2.xasFdxInsideDelivery,
						xasFdxInsidePickup = item2.xasFdxInsidePickup,
						xasFdxNAFTACO = item2.xasFdxNAFTACO,
						xasFdxNonStandardContainer = item2.xasFdxNonStandardContainer,
						xasFdxReturnInstructions = item2.xasFdxReturnInstructions,
						xasFdxSaturdayDelivery = item2.xasFdxSaturdayDelivery,
						xasFdxSaturdayPickup = item2.xasFdxSaturdayPickup,
						xasUpsCertificateOfOrigin = item2.xasUpsCertificateOfOrigin,
						xasUpsCod = item2.xasUpsCod,
						xasUpsCommercialInvoice = item2.xasUpsCommercialInvoice,
						xasUpsNAFTACO = item2.xasUpsNAFTACO,
						xasUpsPackingList = item2.xasUpsPackingList,
						xasUpsPartialInvoice = item2.xasUpsPartialInvoice,
						xasUpsSaturdayDelivery = item2.xasUpsSaturdayDelivery,
						xasUpsUseInterface = item2.xasUpsUseInterface,
						xasReferenceTrackingLink = item2.xasReferenceTrackingLink,
						xasRowVersion = item2.xasRowVersion,
						xasSecondTaxCodeID = item2.xasSecondTaxCodeID,
						xasShipChargeWeb = item2.xasShipChargeWeb,
						xasShippingPaymentTypeID = item2.xasShippingPaymentTypeID,
						xasTaxCodeID = item2.xasTaxCodeID,
						xasTaxStatus = item2.xasTaxStatus,
						xasTrackingLink = item2.xasTrackingLink,
						xasUpsBillingOptionDefault = item2.xasUpsBillingOptionDefault,
						xasUpsCodFundsCode = item2.xasUpsCodFundsCode,
						xasUpsCostCenter = item2.xasUpsCostCenter,
						xasUpsPackageType = item2.xasUpsPackageType,
						xasUpsServiceType = item2.xasUpsServiceType,
						xasUpsWsBillingOption = item2.xasUpsWsBillingOption,
						xasUpsWSPackageType = item2.xasUpsWSPackageType,
						xasUpsWSServiceType = item2.xasUpsWSServiceType,
						xasUSPSEndorsement = item2.xasUSPSEndorsement,
						CustomFields = item2.CustomFields
					};
					allShippingMethodsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShippingMethods]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShippingMethodDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShippingMethodsDto,
				RecordCount = allShippingMethodsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShippingMethodDto>> Process_GetShippingMethod(Guid shippingMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShippingMethodDto shippingMethodDto = null;
		ERPResponseMessageDto<ERPShippingMethodDto> result;
		try
		{
			IERPShippingMethodRepository iERPShippingMethodRepository = (base.ERPShippingMethodRepository = new ERPShippingMethodRepository(base.ApiClientContext));
			using (iERPShippingMethodRepository)
			{
				ERPShippingMethodInformationDto eRPShippingMethodInformationDto = await base.ERPShippingMethodRepository.GetShippingMethod(shippingMethodId);
				shippingMethodDto = new ERPShippingMethodDto
				{
					xasAvalaraTaxCodeID = eRPShippingMethodInformationDto.xasAvalaraTaxCodeID,
					xasCarrier = eRPShippingMethodInformationDto.xasCarrier,
					xasCarrierAccountNumber = eRPShippingMethodInformationDto.xasCarrierAccountNumber,
					xasShippingMethodID = eRPShippingMethodInformationDto.xasShippingMethodID,
					xasCreatedBy = eRPShippingMethodInformationDto.xasCreatedBy,
					xasCreatedDate = eRPShippingMethodInformationDto.xasCreatedDate,
					xasDescription = eRPShippingMethodInformationDto.xasDescription,
					xasDistributeCostsOption = eRPShippingMethodInformationDto.xasDistributeCostsOption,
					xasUniqueID = eRPShippingMethodInformationDto.xasUniqueID,
					xasFdxAccessibility = eRPShippingMethodInformationDto.xasFdxAccessibility,
					xasFdxCodCollectionType = eRPShippingMethodInformationDto.xasFdxCodCollectionType,
					xasFdxDropOffType = eRPShippingMethodInformationDto.xasFdxDropOffType,
					xasFdxHomeDeliveryType = eRPShippingMethodInformationDto.xasFdxHomeDeliveryType,
					xasFdxPackageType = eRPShippingMethodInformationDto.xasFdxPackageType,
					xasFdxRateElementBasis = eRPShippingMethodInformationDto.xasFdxRateElementBasis,
					xasFdxRateRequestType = eRPShippingMethodInformationDto.xasFdxRateRequestType,
					xasFdxRateTypeBasis = eRPShippingMethodInformationDto.xasFdxRateTypeBasis,
					xasFdxReturnShipIndicator = eRPShippingMethodInformationDto.xasFdxReturnShipIndicator,
					xasFdxService = eRPShippingMethodInformationDto.xasFdxService,
					xasFdxSignatureOption = eRPShippingMethodInformationDto.xasFdxSignatureOption,
					xasFdxVHCAmountOrPercentage = eRPShippingMethodInformationDto.xasFdxVHCAmountOrPercentage,
					xasFdxVHCLevel = eRPShippingMethodInformationDto.xasFdxVHCLevel,
					xasFdxVHCType = eRPShippingMethodInformationDto.xasFdxVHCType,
					xasFedExBillingOption = eRPShippingMethodInformationDto.xasFedExBillingOption,
					xasInactiveDate = eRPShippingMethodInformationDto.xasInactiveDate,
					xasInactive = eRPShippingMethodInformationDto.xasInactive,
					xasFdxCertificateOfOrigin = eRPShippingMethodInformationDto.xasFdxCertificateOfOrigin,
					xasFdxCod = eRPShippingMethodInformationDto.xasFdxCod,
					xasFdxCommercialInvoice = eRPShippingMethodInformationDto.xasFdxCommercialInvoice,
					xasFdxExportDeclaration = eRPShippingMethodInformationDto.xasFdxExportDeclaration,
					xasFdxHoldAtLocation = eRPShippingMethodInformationDto.xasFdxHoldAtLocation,
					xasFdxInsideDelivery = eRPShippingMethodInformationDto.xasFdxInsideDelivery,
					xasFdxInsidePickup = eRPShippingMethodInformationDto.xasFdxInsidePickup,
					xasFdxNAFTACO = eRPShippingMethodInformationDto.xasFdxNAFTACO,
					xasFdxNonStandardContainer = eRPShippingMethodInformationDto.xasFdxNonStandardContainer,
					xasFdxReturnInstructions = eRPShippingMethodInformationDto.xasFdxReturnInstructions,
					xasFdxSaturdayDelivery = eRPShippingMethodInformationDto.xasFdxSaturdayDelivery,
					xasFdxSaturdayPickup = eRPShippingMethodInformationDto.xasFdxSaturdayPickup,
					xasUpsCertificateOfOrigin = eRPShippingMethodInformationDto.xasUpsCertificateOfOrigin,
					xasUpsCod = eRPShippingMethodInformationDto.xasUpsCod,
					xasUpsCommercialInvoice = eRPShippingMethodInformationDto.xasUpsCommercialInvoice,
					xasUpsNAFTACO = eRPShippingMethodInformationDto.xasUpsNAFTACO,
					xasUpsPackingList = eRPShippingMethodInformationDto.xasUpsPackingList,
					xasUpsPartialInvoice = eRPShippingMethodInformationDto.xasUpsPartialInvoice,
					xasUpsSaturdayDelivery = eRPShippingMethodInformationDto.xasUpsSaturdayDelivery,
					xasUpsUseInterface = eRPShippingMethodInformationDto.xasUpsUseInterface,
					xasReferenceTrackingLink = eRPShippingMethodInformationDto.xasReferenceTrackingLink,
					xasRowVersion = eRPShippingMethodInformationDto.xasRowVersion,
					xasSecondTaxCodeID = eRPShippingMethodInformationDto.xasSecondTaxCodeID,
					xasShipChargeWeb = eRPShippingMethodInformationDto.xasShipChargeWeb,
					xasShippingPaymentTypeID = eRPShippingMethodInformationDto.xasShippingPaymentTypeID,
					xasTaxCodeID = eRPShippingMethodInformationDto.xasTaxCodeID,
					xasTaxStatus = eRPShippingMethodInformationDto.xasTaxStatus,
					xasTrackingLink = eRPShippingMethodInformationDto.xasTrackingLink,
					xasUpsBillingOptionDefault = eRPShippingMethodInformationDto.xasUpsBillingOptionDefault,
					xasUpsCodFundsCode = eRPShippingMethodInformationDto.xasUpsCodFundsCode,
					xasUpsCostCenter = eRPShippingMethodInformationDto.xasUpsCostCenter,
					xasUpsPackageType = eRPShippingMethodInformationDto.xasUpsPackageType,
					xasUpsServiceType = eRPShippingMethodInformationDto.xasUpsServiceType,
					xasUpsWsBillingOption = eRPShippingMethodInformationDto.xasUpsWsBillingOption,
					xasUpsWSPackageType = eRPShippingMethodInformationDto.xasUpsWSPackageType,
					xasUpsWSServiceType = eRPShippingMethodInformationDto.xasUpsWSServiceType,
					xasUSPSEndorsement = eRPShippingMethodInformationDto.xasUSPSEndorsement,
					CustomFields = eRPShippingMethodInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShippingMethods []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShippingMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shippingMethodDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShippingMethodDto>> Process_PutShippingMethod(ERPShippingMethodDto shippingMethod)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPShippingMethodDto createdObject = null;
		ERPResponseMessageDto<ERPShippingMethodDto> result;
		try
		{
			IERPShippingMethodRepository iERPShippingMethodRepository = (base.ERPShippingMethodRepository = new ERPShippingMethodRepository(base.ApiClientContext));
			using (iERPShippingMethodRepository)
			{
				APIValidationInfoDto postResult = await base.ERPShippingMethodRepository.SaveShippingMethod(shippingMethod);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPShippingMethodInformationDto eRPShippingMethodInformationDto = await base.ERPShippingMethodRepository.GetShippingMethod(shippingMethod.xasUniqueID);
					createdObject = new ERPShippingMethodDto
					{
						xasAvalaraTaxCodeID = eRPShippingMethodInformationDto.xasAvalaraTaxCodeID,
						xasCarrier = eRPShippingMethodInformationDto.xasCarrier,
						xasCarrierAccountNumber = eRPShippingMethodInformationDto.xasCarrierAccountNumber,
						xasShippingMethodID = eRPShippingMethodInformationDto.xasShippingMethodID,
						xasCreatedBy = eRPShippingMethodInformationDto.xasCreatedBy,
						xasCreatedDate = eRPShippingMethodInformationDto.xasCreatedDate,
						xasDescription = eRPShippingMethodInformationDto.xasDescription,
						xasDistributeCostsOption = eRPShippingMethodInformationDto.xasDistributeCostsOption,
						xasUniqueID = eRPShippingMethodInformationDto.xasUniqueID,
						xasFdxAccessibility = eRPShippingMethodInformationDto.xasFdxAccessibility,
						xasFdxCodCollectionType = eRPShippingMethodInformationDto.xasFdxCodCollectionType,
						xasFdxDropOffType = eRPShippingMethodInformationDto.xasFdxDropOffType,
						xasFdxHomeDeliveryType = eRPShippingMethodInformationDto.xasFdxHomeDeliveryType,
						xasFdxPackageType = eRPShippingMethodInformationDto.xasFdxPackageType,
						xasFdxRateElementBasis = eRPShippingMethodInformationDto.xasFdxRateElementBasis,
						xasFdxRateRequestType = eRPShippingMethodInformationDto.xasFdxRateRequestType,
						xasFdxRateTypeBasis = eRPShippingMethodInformationDto.xasFdxRateTypeBasis,
						xasFdxReturnShipIndicator = eRPShippingMethodInformationDto.xasFdxReturnShipIndicator,
						xasFdxService = eRPShippingMethodInformationDto.xasFdxService,
						xasFdxSignatureOption = eRPShippingMethodInformationDto.xasFdxSignatureOption,
						xasFdxVHCAmountOrPercentage = eRPShippingMethodInformationDto.xasFdxVHCAmountOrPercentage,
						xasFdxVHCLevel = eRPShippingMethodInformationDto.xasFdxVHCLevel,
						xasFdxVHCType = eRPShippingMethodInformationDto.xasFdxVHCType,
						xasFedExBillingOption = eRPShippingMethodInformationDto.xasFedExBillingOption,
						xasInactiveDate = eRPShippingMethodInformationDto.xasInactiveDate,
						xasInactive = eRPShippingMethodInformationDto.xasInactive,
						xasFdxCertificateOfOrigin = eRPShippingMethodInformationDto.xasFdxCertificateOfOrigin,
						xasFdxCod = eRPShippingMethodInformationDto.xasFdxCod,
						xasFdxCommercialInvoice = eRPShippingMethodInformationDto.xasFdxCommercialInvoice,
						xasFdxExportDeclaration = eRPShippingMethodInformationDto.xasFdxExportDeclaration,
						xasFdxHoldAtLocation = eRPShippingMethodInformationDto.xasFdxHoldAtLocation,
						xasFdxInsideDelivery = eRPShippingMethodInformationDto.xasFdxInsideDelivery,
						xasFdxInsidePickup = eRPShippingMethodInformationDto.xasFdxInsidePickup,
						xasFdxNAFTACO = eRPShippingMethodInformationDto.xasFdxNAFTACO,
						xasFdxNonStandardContainer = eRPShippingMethodInformationDto.xasFdxNonStandardContainer,
						xasFdxReturnInstructions = eRPShippingMethodInformationDto.xasFdxReturnInstructions,
						xasFdxSaturdayDelivery = eRPShippingMethodInformationDto.xasFdxSaturdayDelivery,
						xasFdxSaturdayPickup = eRPShippingMethodInformationDto.xasFdxSaturdayPickup,
						xasUpsCertificateOfOrigin = eRPShippingMethodInformationDto.xasUpsCertificateOfOrigin,
						xasUpsCod = eRPShippingMethodInformationDto.xasUpsCod,
						xasUpsCommercialInvoice = eRPShippingMethodInformationDto.xasUpsCommercialInvoice,
						xasUpsNAFTACO = eRPShippingMethodInformationDto.xasUpsNAFTACO,
						xasUpsPackingList = eRPShippingMethodInformationDto.xasUpsPackingList,
						xasUpsPartialInvoice = eRPShippingMethodInformationDto.xasUpsPartialInvoice,
						xasUpsSaturdayDelivery = eRPShippingMethodInformationDto.xasUpsSaturdayDelivery,
						xasUpsUseInterface = eRPShippingMethodInformationDto.xasUpsUseInterface,
						xasReferenceTrackingLink = eRPShippingMethodInformationDto.xasReferenceTrackingLink,
						xasRowVersion = eRPShippingMethodInformationDto.xasRowVersion,
						xasSecondTaxCodeID = eRPShippingMethodInformationDto.xasSecondTaxCodeID,
						xasShipChargeWeb = eRPShippingMethodInformationDto.xasShipChargeWeb,
						xasShippingPaymentTypeID = eRPShippingMethodInformationDto.xasShippingPaymentTypeID,
						xasTaxCodeID = eRPShippingMethodInformationDto.xasTaxCodeID,
						xasTaxStatus = eRPShippingMethodInformationDto.xasTaxStatus,
						xasTrackingLink = eRPShippingMethodInformationDto.xasTrackingLink,
						xasUpsBillingOptionDefault = eRPShippingMethodInformationDto.xasUpsBillingOptionDefault,
						xasUpsCodFundsCode = eRPShippingMethodInformationDto.xasUpsCodFundsCode,
						xasUpsCostCenter = eRPShippingMethodInformationDto.xasUpsCostCenter,
						xasUpsPackageType = eRPShippingMethodInformationDto.xasUpsPackageType,
						xasUpsServiceType = eRPShippingMethodInformationDto.xasUpsServiceType,
						xasUpsWsBillingOption = eRPShippingMethodInformationDto.xasUpsWsBillingOption,
						xasUpsWSPackageType = eRPShippingMethodInformationDto.xasUpsWSPackageType,
						xasUpsWSServiceType = eRPShippingMethodInformationDto.xasUpsWSServiceType,
						xasUSPSEndorsement = eRPShippingMethodInformationDto.xasUSPSEndorsement,
						CustomFields = eRPShippingMethodInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ShippingMethod [{shippingMethod.xasUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShippingMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteShippingMethod(Guid shippingMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShippingMethodRepository iERPShippingMethodRepository = (base.ERPShippingMethodRepository = new ERPShippingMethodRepository(base.ApiClientContext));
		using (iERPShippingMethodRepository)
		{
			if (!(await base.ERPShippingMethodRepository.DoesShippingMethodExist(shippingMethodId)))
			{
				base.ErrorsList.Add($"ShippingMethod [{shippingMethodId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPShippingMethodInformationDto eRPShippingMethodInformationDto = await base.ERPShippingMethodRepository.GetShippingMethod(shippingMethodId);
				string text = await base.ERPShippingMethodRepository.WhereUsed("ShippingMethods", new object[1] { eRPShippingMethodInformationDto.xasShippingMethodID }, new object[1] { "xasShippingMethodID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ShippingMethod cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPShippingMethodDto>> Process_DeleteShippingMethod(Guid shippingMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPShippingMethodDto> result;
		try
		{
			IERPShippingMethodRepository iERPShippingMethodRepository = (base.ERPShippingMethodRepository = new ERPShippingMethodRepository(base.ApiClientContext));
			using (iERPShippingMethodRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPShippingMethodRepository.DeleteRowFromTable("ShippingMethods", "xas", shippingMethodId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ShippingMethod [{shippingMethodId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShippingMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPShippingMethodDto()
			};
		}
		return result;
	}
}
