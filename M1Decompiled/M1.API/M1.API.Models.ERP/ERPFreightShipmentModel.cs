using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPFreightShipmentModel : ERPBaseModel, IERPFreightShipmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllFreightShipments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPFreightShipmentRepository iERPFreightShipmentRepository = (base.ERPFreightShipmentRepository = new ERPFreightShipmentRepository(base.ApiClientContext));
		using (iERPFreightShipmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPFreightShipmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPFreightShipmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPFreightShipmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPFreightShipmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetFreightShipment(Guid freightShipmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightShipmentRepository iERPFreightShipmentRepository = (base.ERPFreightShipmentRepository = new ERPFreightShipmentRepository(base.ApiClientContext));
		using (iERPFreightShipmentRepository)
		{
			if (!(await base.ERPFreightShipmentRepository.DoesFreightShipmentExist(freightShipmentId)))
			{
				errorsList.Add($"FreightShipment [{freightShipmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutFreightShipment(ERPFreightShipmentDto freightShipment)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightShipmentRepository iERPFreightShipmentRepository = (base.ERPFreightShipmentRepository = new ERPFreightShipmentRepository(base.ApiClientContext));
		using (iERPFreightShipmentRepository)
		{
			if (!string.IsNullOrWhiteSpace(freightShipment.fspShipOrganizationID) && !(await base.ERPFreightShipmentRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { freightShipment.fspShipOrganizationID })))
			{
				errorsList.Add("fspShipOrganizationID [" + freightShipment.fspShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(freightShipment.fspShipLocationID) && !(await base.ERPFreightShipmentRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { freightShipment.fspShipOrganizationID, freightShipment.fspShipLocationID })))
			{
				errorsList.Add("fspShipLocationID [" + freightShipment.fspShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(freightShipment.fspShippingMethodID) && !(await base.ERPFreightShipmentRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { freightShipment.fspShippingMethodID })))
			{
				errorsList.Add("fspShippingMethodID [" + freightShipment.fspShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(freightShipment.fspUps3rdPartyOrganizationID) && !(await base.ERPFreightShipmentRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { freightShipment.fspUps3rdPartyOrganizationID })))
			{
				errorsList.Add("fspUps3rdPartyOrganizationID [" + freightShipment.fspUps3rdPartyOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(freightShipment.fspUps3rdPartyLocationID) && !(await base.ERPFreightShipmentRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { freightShipment.fspUps3rdPartyOrganizationID, freightShipment.fspUps3rdPartyLocationID })))
			{
				errorsList.Add("fspUps3rdPartyLocationID [" + freightShipment.fspUps3rdPartyLocationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPFreightShipmentDto>>> Process_GetAllFreightShipments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPFreightShipmentDto> allFreightShipmentsDto = new List<ERPFreightShipmentDto>();
		ERPResponseMessageDto<IList<ERPFreightShipmentDto>> result;
		try
		{
			IERPFreightShipmentRepository iERPFreightShipmentRepository = (base.ERPFreightShipmentRepository = new ERPFreightShipmentRepository(base.ApiClientContext));
			using (iERPFreightShipmentRepository)
			{
				foreach (ERPFreightShipmentInformationDto item2 in await base.ERPFreightShipmentRepository.GetAllFreightShipments(pageSize, pageNumber, filter, orderBy))
				{
					ERPFreightShipmentDto item = new ERPFreightShipmentDto
					{
						fspCarrier = item2.fspCarrier,
						fspFreightShipmentID = item2.fspFreightShipmentID,
						fspCreatedBy = item2.fspCreatedBy,
						fspCreatedDate = item2.fspCreatedDate,
						fspDeclaredValue = item2.fspDeclaredValue,
						fspDistributeCostsOption = item2.fspDistributeCostsOption,
						fspUniqueID = item2.fspUniqueID,
						fspFdxAccessibility = item2.fspFdxAccessibility,
						fspFdxCodCollectionAmount = item2.fspFdxCodCollectionAmount,
						fspFdxCodCollectionType = item2.fspFdxCodCollectionType,
						fspFdxDropOffType = item2.fspFdxDropOffType,
						fspFdxHandlingCost = item2.fspFdxHandlingCost,
						fspFdxHomeDeliveryType = item2.fspFdxHomeDeliveryType,
						fspFdxLastLogID = item2.fspFdxLastLogID,
						fspFdxLastReplyErrorCode = item2.fspFdxLastReplyErrorCode,
						fspFdxLastReplyErrorMessage = item2.fspFdxLastReplyErrorMessage,
						fspFdxLastReplySoftErrorCode = item2.fspFdxLastReplySoftErrorCode,
						fspFdxLastReplySoftErrorMsg = item2.fspFdxLastReplySoftErrorMsg,
						fspFdxLastReplySoftErrorType = item2.fspFdxLastReplySoftErrorType,
						fspFdxLastRequestDate = item2.fspFdxLastRequestDate,
						fspFdxLastUTI = item2.fspFdxLastUTI,
						fspFdxPackagingCost = item2.fspFdxPackagingCost,
						fspFdxPayorAccountNumber = item2.fspFdxPayorAccountNumber,
						fspFdxPayorCountryCode = item2.fspFdxPayorCountryCode,
						fspFdxPayorType = item2.fspFdxPayorType,
						fspFdxRateRequestType = item2.fspFdxRateRequestType,
						fspFdxReturnShipIndicator = item2.fspFdxReturnShipIndicator,
						fspFdxService = item2.fspFdxService,
						fspFdxShipCostMarkupPct = item2.fspFdxShipCostMarkupPct,
						fspFdxSignatureOption = item2.fspFdxSignatureOption,
						fspFdxSignatureReleaseAuthNum = item2.fspFdxSignatureReleaseAuthNum,
						fspFdxStatus = item2.fspFdxStatus,
						fspFdxStatusText = item2.fspFdxStatusText,
						fspFdxVHCAmountOrPercentage = item2.fspFdxVHCAmountOrPercentage,
						fspFdxVHCLevel = item2.fspFdxVHCLevel,
						fspFdxVHCType = item2.fspFdxVHCType,
						fspFreightShipmentDate = item2.fspFreightShipmentDate,
						fspFdxCod = item2.fspFdxCod,
						fspFdxHoldAtLocation = item2.fspFdxHoldAtLocation,
						fspFdxInsideDelivery = item2.fspFdxInsideDelivery,
						fspFdxInsidePickup = item2.fspFdxInsidePickup,
						fspFdxOneItemPerShipment = item2.fspFdxOneItemPerShipment,
						fspFdxSaturdayDelivery = item2.fspFdxSaturdayDelivery,
						fspFdxSaturdayPickup = item2.fspFdxSaturdayPickup,
						fspUpsSaturdayDelivery = item2.fspUpsSaturdayDelivery,
						fspVoidOnUps = item2.fspVoidOnUps,
						fspNotesRTF = item2.fspNotesRTF,
						fspNotesText = item2.fspNotesText,
						fspRowVersion = item2.fspRowVersion,
						fspShipFromOrganizationID = item2.fspShipFromOrganizationID,
						fspShipLocationID = item2.fspShipLocationID,
						fspShipOrganizationID = item2.fspShipOrganizationID,
						fspShipperAcctNumber = item2.fspShipperAcctNumber,
						fspShippingMethodID = item2.fspShippingMethodID,
						fspTotalCharges = item2.fspTotalCharges,
						fspTotalPublishedCharges = item2.fspTotalPublishedCharges,
						fspUps3rdPartyLocationID = item2.fspUps3rdPartyLocationID,
						fspUps3rdPartyOrganizationID = item2.fspUps3rdPartyOrganizationID,
						fspUpsBillAcctNumber = item2.fspUpsBillAcctNumber,
						fspUpsBillingOption = item2.fspUpsBillingOption,
						fspUpsInterfaceStatus = item2.fspUpsInterfaceStatus,
						fspUpsServiceType = item2.fspUpsServiceType,
						CustomFields = item2.CustomFields
					};
					allFreightShipmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all FreightShipments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPFreightShipmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allFreightShipmentsDto,
				RecordCount = allFreightShipmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightShipmentDto>> Process_GetFreightShipment(Guid freightShipmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPFreightShipmentDto freightShipmentDto = null;
		ERPResponseMessageDto<ERPFreightShipmentDto> result;
		try
		{
			IERPFreightShipmentRepository iERPFreightShipmentRepository = (base.ERPFreightShipmentRepository = new ERPFreightShipmentRepository(base.ApiClientContext));
			using (iERPFreightShipmentRepository)
			{
				ERPFreightShipmentInformationDto eRPFreightShipmentInformationDto = await base.ERPFreightShipmentRepository.GetFreightShipment(freightShipmentId);
				freightShipmentDto = new ERPFreightShipmentDto
				{
					fspCarrier = eRPFreightShipmentInformationDto.fspCarrier,
					fspFreightShipmentID = eRPFreightShipmentInformationDto.fspFreightShipmentID,
					fspCreatedBy = eRPFreightShipmentInformationDto.fspCreatedBy,
					fspCreatedDate = eRPFreightShipmentInformationDto.fspCreatedDate,
					fspDeclaredValue = eRPFreightShipmentInformationDto.fspDeclaredValue,
					fspDistributeCostsOption = eRPFreightShipmentInformationDto.fspDistributeCostsOption,
					fspUniqueID = eRPFreightShipmentInformationDto.fspUniqueID,
					fspFdxAccessibility = eRPFreightShipmentInformationDto.fspFdxAccessibility,
					fspFdxCodCollectionAmount = eRPFreightShipmentInformationDto.fspFdxCodCollectionAmount,
					fspFdxCodCollectionType = eRPFreightShipmentInformationDto.fspFdxCodCollectionType,
					fspFdxDropOffType = eRPFreightShipmentInformationDto.fspFdxDropOffType,
					fspFdxHandlingCost = eRPFreightShipmentInformationDto.fspFdxHandlingCost,
					fspFdxHomeDeliveryType = eRPFreightShipmentInformationDto.fspFdxHomeDeliveryType,
					fspFdxLastLogID = eRPFreightShipmentInformationDto.fspFdxLastLogID,
					fspFdxLastReplyErrorCode = eRPFreightShipmentInformationDto.fspFdxLastReplyErrorCode,
					fspFdxLastReplyErrorMessage = eRPFreightShipmentInformationDto.fspFdxLastReplyErrorMessage,
					fspFdxLastReplySoftErrorCode = eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorCode,
					fspFdxLastReplySoftErrorMsg = eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorMsg,
					fspFdxLastReplySoftErrorType = eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorType,
					fspFdxLastRequestDate = eRPFreightShipmentInformationDto.fspFdxLastRequestDate,
					fspFdxLastUTI = eRPFreightShipmentInformationDto.fspFdxLastUTI,
					fspFdxPackagingCost = eRPFreightShipmentInformationDto.fspFdxPackagingCost,
					fspFdxPayorAccountNumber = eRPFreightShipmentInformationDto.fspFdxPayorAccountNumber,
					fspFdxPayorCountryCode = eRPFreightShipmentInformationDto.fspFdxPayorCountryCode,
					fspFdxPayorType = eRPFreightShipmentInformationDto.fspFdxPayorType,
					fspFdxRateRequestType = eRPFreightShipmentInformationDto.fspFdxRateRequestType,
					fspFdxReturnShipIndicator = eRPFreightShipmentInformationDto.fspFdxReturnShipIndicator,
					fspFdxService = eRPFreightShipmentInformationDto.fspFdxService,
					fspFdxShipCostMarkupPct = eRPFreightShipmentInformationDto.fspFdxShipCostMarkupPct,
					fspFdxSignatureOption = eRPFreightShipmentInformationDto.fspFdxSignatureOption,
					fspFdxSignatureReleaseAuthNum = eRPFreightShipmentInformationDto.fspFdxSignatureReleaseAuthNum,
					fspFdxStatus = eRPFreightShipmentInformationDto.fspFdxStatus,
					fspFdxStatusText = eRPFreightShipmentInformationDto.fspFdxStatusText,
					fspFdxVHCAmountOrPercentage = eRPFreightShipmentInformationDto.fspFdxVHCAmountOrPercentage,
					fspFdxVHCLevel = eRPFreightShipmentInformationDto.fspFdxVHCLevel,
					fspFdxVHCType = eRPFreightShipmentInformationDto.fspFdxVHCType,
					fspFreightShipmentDate = eRPFreightShipmentInformationDto.fspFreightShipmentDate,
					fspFdxCod = eRPFreightShipmentInformationDto.fspFdxCod,
					fspFdxHoldAtLocation = eRPFreightShipmentInformationDto.fspFdxHoldAtLocation,
					fspFdxInsideDelivery = eRPFreightShipmentInformationDto.fspFdxInsideDelivery,
					fspFdxInsidePickup = eRPFreightShipmentInformationDto.fspFdxInsidePickup,
					fspFdxOneItemPerShipment = eRPFreightShipmentInformationDto.fspFdxOneItemPerShipment,
					fspFdxSaturdayDelivery = eRPFreightShipmentInformationDto.fspFdxSaturdayDelivery,
					fspFdxSaturdayPickup = eRPFreightShipmentInformationDto.fspFdxSaturdayPickup,
					fspUpsSaturdayDelivery = eRPFreightShipmentInformationDto.fspUpsSaturdayDelivery,
					fspVoidOnUps = eRPFreightShipmentInformationDto.fspVoidOnUps,
					fspNotesRTF = eRPFreightShipmentInformationDto.fspNotesRTF,
					fspNotesText = eRPFreightShipmentInformationDto.fspNotesText,
					fspRowVersion = eRPFreightShipmentInformationDto.fspRowVersion,
					fspShipFromOrganizationID = eRPFreightShipmentInformationDto.fspShipFromOrganizationID,
					fspShipLocationID = eRPFreightShipmentInformationDto.fspShipLocationID,
					fspShipOrganizationID = eRPFreightShipmentInformationDto.fspShipOrganizationID,
					fspShipperAcctNumber = eRPFreightShipmentInformationDto.fspShipperAcctNumber,
					fspShippingMethodID = eRPFreightShipmentInformationDto.fspShippingMethodID,
					fspTotalCharges = eRPFreightShipmentInformationDto.fspTotalCharges,
					fspTotalPublishedCharges = eRPFreightShipmentInformationDto.fspTotalPublishedCharges,
					fspUps3rdPartyLocationID = eRPFreightShipmentInformationDto.fspUps3rdPartyLocationID,
					fspUps3rdPartyOrganizationID = eRPFreightShipmentInformationDto.fspUps3rdPartyOrganizationID,
					fspUpsBillAcctNumber = eRPFreightShipmentInformationDto.fspUpsBillAcctNumber,
					fspUpsBillingOption = eRPFreightShipmentInformationDto.fspUpsBillingOption,
					fspUpsInterfaceStatus = eRPFreightShipmentInformationDto.fspUpsInterfaceStatus,
					fspUpsServiceType = eRPFreightShipmentInformationDto.fspUpsServiceType,
					CustomFields = eRPFreightShipmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the FreightShipments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightShipmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = freightShipmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightShipmentDto>> Process_PutFreightShipment(ERPFreightShipmentDto freightShipment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPFreightShipmentDto createdObject = null;
		ERPResponseMessageDto<ERPFreightShipmentDto> result;
		try
		{
			IERPFreightShipmentRepository iERPFreightShipmentRepository = (base.ERPFreightShipmentRepository = new ERPFreightShipmentRepository(base.ApiClientContext));
			using (iERPFreightShipmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPFreightShipmentRepository.SaveFreightShipment(freightShipment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPFreightShipmentInformationDto eRPFreightShipmentInformationDto = await base.ERPFreightShipmentRepository.GetFreightShipment(freightShipment.fspUniqueID);
					createdObject = new ERPFreightShipmentDto
					{
						fspCarrier = eRPFreightShipmentInformationDto.fspCarrier,
						fspFreightShipmentID = eRPFreightShipmentInformationDto.fspFreightShipmentID,
						fspCreatedBy = eRPFreightShipmentInformationDto.fspCreatedBy,
						fspCreatedDate = eRPFreightShipmentInformationDto.fspCreatedDate,
						fspDeclaredValue = eRPFreightShipmentInformationDto.fspDeclaredValue,
						fspDistributeCostsOption = eRPFreightShipmentInformationDto.fspDistributeCostsOption,
						fspUniqueID = eRPFreightShipmentInformationDto.fspUniqueID,
						fspFdxAccessibility = eRPFreightShipmentInformationDto.fspFdxAccessibility,
						fspFdxCodCollectionAmount = eRPFreightShipmentInformationDto.fspFdxCodCollectionAmount,
						fspFdxCodCollectionType = eRPFreightShipmentInformationDto.fspFdxCodCollectionType,
						fspFdxDropOffType = eRPFreightShipmentInformationDto.fspFdxDropOffType,
						fspFdxHandlingCost = eRPFreightShipmentInformationDto.fspFdxHandlingCost,
						fspFdxHomeDeliveryType = eRPFreightShipmentInformationDto.fspFdxHomeDeliveryType,
						fspFdxLastLogID = eRPFreightShipmentInformationDto.fspFdxLastLogID,
						fspFdxLastReplyErrorCode = eRPFreightShipmentInformationDto.fspFdxLastReplyErrorCode,
						fspFdxLastReplyErrorMessage = eRPFreightShipmentInformationDto.fspFdxLastReplyErrorMessage,
						fspFdxLastReplySoftErrorCode = eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorCode,
						fspFdxLastReplySoftErrorMsg = eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorMsg,
						fspFdxLastReplySoftErrorType = eRPFreightShipmentInformationDto.fspFdxLastReplySoftErrorType,
						fspFdxLastRequestDate = eRPFreightShipmentInformationDto.fspFdxLastRequestDate,
						fspFdxLastUTI = eRPFreightShipmentInformationDto.fspFdxLastUTI,
						fspFdxPackagingCost = eRPFreightShipmentInformationDto.fspFdxPackagingCost,
						fspFdxPayorAccountNumber = eRPFreightShipmentInformationDto.fspFdxPayorAccountNumber,
						fspFdxPayorCountryCode = eRPFreightShipmentInformationDto.fspFdxPayorCountryCode,
						fspFdxPayorType = eRPFreightShipmentInformationDto.fspFdxPayorType,
						fspFdxRateRequestType = eRPFreightShipmentInformationDto.fspFdxRateRequestType,
						fspFdxReturnShipIndicator = eRPFreightShipmentInformationDto.fspFdxReturnShipIndicator,
						fspFdxService = eRPFreightShipmentInformationDto.fspFdxService,
						fspFdxShipCostMarkupPct = eRPFreightShipmentInformationDto.fspFdxShipCostMarkupPct,
						fspFdxSignatureOption = eRPFreightShipmentInformationDto.fspFdxSignatureOption,
						fspFdxSignatureReleaseAuthNum = eRPFreightShipmentInformationDto.fspFdxSignatureReleaseAuthNum,
						fspFdxStatus = eRPFreightShipmentInformationDto.fspFdxStatus,
						fspFdxStatusText = eRPFreightShipmentInformationDto.fspFdxStatusText,
						fspFdxVHCAmountOrPercentage = eRPFreightShipmentInformationDto.fspFdxVHCAmountOrPercentage,
						fspFdxVHCLevel = eRPFreightShipmentInformationDto.fspFdxVHCLevel,
						fspFdxVHCType = eRPFreightShipmentInformationDto.fspFdxVHCType,
						fspFreightShipmentDate = eRPFreightShipmentInformationDto.fspFreightShipmentDate,
						fspFdxCod = eRPFreightShipmentInformationDto.fspFdxCod,
						fspFdxHoldAtLocation = eRPFreightShipmentInformationDto.fspFdxHoldAtLocation,
						fspFdxInsideDelivery = eRPFreightShipmentInformationDto.fspFdxInsideDelivery,
						fspFdxInsidePickup = eRPFreightShipmentInformationDto.fspFdxInsidePickup,
						fspFdxOneItemPerShipment = eRPFreightShipmentInformationDto.fspFdxOneItemPerShipment,
						fspFdxSaturdayDelivery = eRPFreightShipmentInformationDto.fspFdxSaturdayDelivery,
						fspFdxSaturdayPickup = eRPFreightShipmentInformationDto.fspFdxSaturdayPickup,
						fspUpsSaturdayDelivery = eRPFreightShipmentInformationDto.fspUpsSaturdayDelivery,
						fspVoidOnUps = eRPFreightShipmentInformationDto.fspVoidOnUps,
						fspNotesRTF = eRPFreightShipmentInformationDto.fspNotesRTF,
						fspNotesText = eRPFreightShipmentInformationDto.fspNotesText,
						fspRowVersion = eRPFreightShipmentInformationDto.fspRowVersion,
						fspShipFromOrganizationID = eRPFreightShipmentInformationDto.fspShipFromOrganizationID,
						fspShipLocationID = eRPFreightShipmentInformationDto.fspShipLocationID,
						fspShipOrganizationID = eRPFreightShipmentInformationDto.fspShipOrganizationID,
						fspShipperAcctNumber = eRPFreightShipmentInformationDto.fspShipperAcctNumber,
						fspShippingMethodID = eRPFreightShipmentInformationDto.fspShippingMethodID,
						fspTotalCharges = eRPFreightShipmentInformationDto.fspTotalCharges,
						fspTotalPublishedCharges = eRPFreightShipmentInformationDto.fspTotalPublishedCharges,
						fspUps3rdPartyLocationID = eRPFreightShipmentInformationDto.fspUps3rdPartyLocationID,
						fspUps3rdPartyOrganizationID = eRPFreightShipmentInformationDto.fspUps3rdPartyOrganizationID,
						fspUpsBillAcctNumber = eRPFreightShipmentInformationDto.fspUpsBillAcctNumber,
						fspUpsBillingOption = eRPFreightShipmentInformationDto.fspUpsBillingOption,
						fspUpsInterfaceStatus = eRPFreightShipmentInformationDto.fspUpsInterfaceStatus,
						fspUpsServiceType = eRPFreightShipmentInformationDto.fspUpsServiceType,
						CustomFields = eRPFreightShipmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing FreightShipment [{freightShipment.fspUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightShipmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteFreightShipment(Guid freightShipmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightShipmentRepository iERPFreightShipmentRepository = (base.ERPFreightShipmentRepository = new ERPFreightShipmentRepository(base.ApiClientContext));
		using (iERPFreightShipmentRepository)
		{
			if (!(await base.ERPFreightShipmentRepository.DoesFreightShipmentExist(freightShipmentId)))
			{
				base.ErrorsList.Add($"FreightShipment [{freightShipmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPFreightShipmentInformationDto eRPFreightShipmentInformationDto = await base.ERPFreightShipmentRepository.GetFreightShipment(freightShipmentId);
				string text = await base.ERPFreightShipmentRepository.WhereUsed("FreightShipments", new object[1] { eRPFreightShipmentInformationDto.fspFreightShipmentID }, new object[1] { "fspFreightShipmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("FreightShipment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPFreightShipmentDto>> Process_DeleteFreightShipment(Guid freightShipmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPFreightShipmentDto> result;
		try
		{
			IERPFreightShipmentRepository iERPFreightShipmentRepository = (base.ERPFreightShipmentRepository = new ERPFreightShipmentRepository(base.ApiClientContext));
			using (iERPFreightShipmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPFreightShipmentRepository.DeleteRowFromTable("FreightShipments", "fsp", freightShipmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of FreightShipment [{freightShipmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightShipmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPFreightShipmentDto()
			};
		}
		return result;
	}
}
