using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPQuoteQuantityModel : ERPBaseModel, IERPQuoteQuantityModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteQuantities(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPQuoteQuantityRepository iERPQuoteQuantityRepository = (base.ERPQuoteQuantityRepository = new ERPQuoteQuantityRepository(base.ApiClientContext));
		using (iERPQuoteQuantityRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPQuoteQuantityRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPQuoteQuantityRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPQuoteQuantityRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPQuoteQuantityRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteQuantity(Guid quoteQuantityId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteQuantityRepository iERPQuoteQuantityRepository = (base.ERPQuoteQuantityRepository = new ERPQuoteQuantityRepository(base.ApiClientContext));
		using (iERPQuoteQuantityRepository)
		{
			if (!(await base.ERPQuoteQuantityRepository.DoesQuoteQuantityExist(quoteQuantityId)))
			{
				errorsList.Add($"QuoteQuantity [{quoteQuantityId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutQuoteQuantity(ERPQuoteQuantityDto quoteQuantity)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteQuantityRepository iERPQuoteQuantityRepository = (base.ERPQuoteQuantityRepository = new ERPQuoteQuantityRepository(base.ApiClientContext));
		using (iERPQuoteQuantityRepository)
		{
			if (!string.IsNullOrWhiteSpace(quoteQuantity.qmqQuoteID) && !(await base.ERPQuoteQuantityRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { quoteQuantity.qmqQuoteID })))
			{
				errorsList.Add("qmqQuoteID [" + quoteQuantity.qmqQuoteID + "] not found.");
			}
			if (quoteQuantity.qmqQuoteLineID > 0 && !(await base.ERPQuoteQuantityRepository.DoesRecordExistInTableUsingKeys("QuoteLines", new object[2] { "QMLQUOTEID", "QMLQUOTELINEID" }, new object[2] { quoteQuantity.qmqQuoteID, quoteQuantity.qmqQuoteLineID })))
			{
				errorsList.Add($"qmqQuoteLineID [{quoteQuantity.qmqQuoteLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteQuantity.qmqSecondTaxCodeID) && !(await base.ERPQuoteQuantityRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { quoteQuantity.qmqSecondTaxCodeID })))
			{
				errorsList.Add("qmqSecondTaxCodeID [" + quoteQuantity.qmqSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteQuantity.qmqTaxCodeID) && !(await base.ERPQuoteQuantityRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { quoteQuantity.qmqTaxCodeID })))
			{
				errorsList.Add("qmqTaxCodeID [" + quoteQuantity.qmqTaxCodeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPQuoteQuantityDto>>> Process_GetAllQuoteQuantities(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPQuoteQuantityDto> allQuoteQuantitiesDto = new List<ERPQuoteQuantityDto>();
		ERPResponseMessageDto<IList<ERPQuoteQuantityDto>> result;
		try
		{
			IERPQuoteQuantityRepository iERPQuoteQuantityRepository = (base.ERPQuoteQuantityRepository = new ERPQuoteQuantityRepository(base.ApiClientContext));
			using (iERPQuoteQuantityRepository)
			{
				foreach (ERPQuoteQuantityInformationDto item2 in await base.ERPQuoteQuantityRepository.GetAllQuoteQuantities(pageSize, pageNumber, filter, orderBy))
				{
					ERPQuoteQuantityDto item = new ERPQuoteQuantityDto
					{
						qmqAdditionalChargeBase = item2.qmqAdditionalChargeBase,
						qmqAdditionalChargeDescription = item2.qmqAdditionalChargeDescription,
						qmqAdditionalChargeForeign = item2.qmqAdditionalChargeForeign,
						qmqAdditionalCostAmount = item2.qmqAdditionalCostAmount,
						qmqAdditionalCostDescription = item2.qmqAdditionalCostDescription,
						qmqAdditionalCostPrice = item2.qmqAdditionalCostPrice,
						qmqAdditionalMarkupPercent = item2.qmqAdditionalMarkupPercent,
						qmqAddSecondTaxAmountBase = item2.qmqAddSecondTaxAmountBase,
						qmqAddSecondTaxAmountForeign = item2.qmqAddSecondTaxAmountForeign,
						qmqAddTaxAmountBase = item2.qmqAddTaxAmountBase,
						qmqAddTaxAmountForeign = item2.qmqAddTaxAmountForeign,
						qmqCalculatedUnitPrice = item2.qmqCalculatedUnitPrice,
						qmqCommissionPercent = item2.qmqCommissionPercent,
						qmqCreatedBy = item2.qmqCreatedBy,
						qmqCreatedDate = item2.qmqCreatedDate,
						qmqDiscountPercent = item2.qmqDiscountPercent,
						qmqDueDate = item2.qmqDueDate,
						qmqUniqueID = item2.qmqUniqueID,
						qmqFullRevisedUnitPriceBase = item2.qmqFullRevisedUnitPriceBase,
						qmqFullRevisedUnitPriceForeign = item2.qmqFullRevisedUnitPriceForeign,
						qmqClosed = item2.qmqClosed,
						qmqCreatedFromMobile = item2.qmqCreatedFromMobile,
						qmqPurchaseToOrder = item2.qmqPurchaseToOrder,
						qmqLaborCost = item2.qmqLaborCost,
						qmqLaborMarkupPercent = item2.qmqLaborMarkupPercent,
						qmqLaborPrice = item2.qmqLaborPrice,
						qmqLeadTime = item2.qmqLeadTime,
						qmqMaterialCost = item2.qmqMaterialCost,
						qmqMaterialMarkupPercent = item2.qmqMaterialMarkupPercent,
						qmqMaterialPrice = item2.qmqMaterialPrice,
						qmqOverheadCost = item2.qmqOverheadCost,
						qmqOverheadMarkupPercent = item2.qmqOverheadMarkupPercent,
						qmqOverheadPrice = item2.qmqOverheadPrice,
						qmqProductionHours = item2.qmqProductionHours,
						qmqPurchaseToOrderCost = item2.qmqPurchaseToOrderCost,
						qmqPurchaseToOrderPrice = item2.qmqPurchaseToOrderPrice,
						qmqPurchaseUnitCostBase = item2.qmqPurchaseUnitCostBase,
						qmqPurToOrderMarkupPercent = item2.qmqPurToOrderMarkupPercent,
						qmqQuoteID = item2.qmqQuoteID,
						qmqQuoteLineID = item2.qmqQuoteLineID,
						qmqQuoteMarkupType = item2.qmqQuoteMarkupType,
						qmqQuoteQuantity = item2.qmqQuoteQuantity,
						qmqQuotingCost = item2.qmqQuotingCost,
						qmqQuotingMarkupPercent = item2.qmqQuotingMarkupPercent,
						qmqQuotingPrice = item2.qmqQuotingPrice,
						qmqRevisedUnitPriceBase = item2.qmqRevisedUnitPriceBase,
						qmqRevisedUnitPriceForeign = item2.qmqRevisedUnitPriceForeign,
						qmqRowVersion = item2.qmqRowVersion,
						qmqScrapPercent = item2.qmqScrapPercent,
						qmqSecondTaxCodeID = item2.qmqSecondTaxCodeID,
						qmqQuoteQuantityID = item2.qmqQuoteQuantityID,
						qmqSetupHours = item2.qmqSetupHours,
						qmqStartDate = item2.qmqStartDate,
						qmqSubcontractCost = item2.qmqSubcontractCost,
						qmqSubcontractMarkupPercent = item2.qmqSubcontractMarkupPercent,
						qmqSubcontractPrice = item2.qmqSubcontractPrice,
						qmqTaxCodeID = item2.qmqTaxCodeID,
						qmqTaxDate = item2.qmqTaxDate,
						qmqTotalCost = item2.qmqTotalCost,
						qmqTotalMarkupPercent = item2.qmqTotalMarkupPercent,
						qmqTotalPrice = item2.qmqTotalPrice,
						qmqTotalRunQuantity = item2.qmqTotalRunQuantity,
						qmqTotalUnitCost = item2.qmqTotalUnitCost,
						qmqTotalUnitPrice = item2.qmqTotalUnitPrice,
						qmqUnitDiscountBase = item2.qmqUnitDiscountBase,
						qmqUnitDiscountForeign = item2.qmqUnitDiscountForeign,
						qmqUnitSecondTaxAmountBase = item2.qmqUnitSecondTaxAmountBase,
						qmqUnitSecondTaxAmountForeign = item2.qmqUnitSecondTaxAmountForeign,
						qmqUnitTaxAmountBase = item2.qmqUnitTaxAmountBase,
						qmqUnitTaxAmountForeign = item2.qmqUnitTaxAmountForeign,
						CustomFields = item2.CustomFields
					};
					allQuoteQuantitiesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteQuantities]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPQuoteQuantityDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteQuantitiesDto,
				RecordCount = allQuoteQuantitiesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteQuantityDto>> Process_GetQuoteQuantity(Guid quoteQuantityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPQuoteQuantityDto quoteQuantityDto = null;
		ERPResponseMessageDto<ERPQuoteQuantityDto> result;
		try
		{
			IERPQuoteQuantityRepository iERPQuoteQuantityRepository = (base.ERPQuoteQuantityRepository = new ERPQuoteQuantityRepository(base.ApiClientContext));
			using (iERPQuoteQuantityRepository)
			{
				ERPQuoteQuantityInformationDto eRPQuoteQuantityInformationDto = await base.ERPQuoteQuantityRepository.GetQuoteQuantity(quoteQuantityId);
				quoteQuantityDto = new ERPQuoteQuantityDto
				{
					qmqAdditionalChargeBase = eRPQuoteQuantityInformationDto.qmqAdditionalChargeBase,
					qmqAdditionalChargeDescription = eRPQuoteQuantityInformationDto.qmqAdditionalChargeDescription,
					qmqAdditionalChargeForeign = eRPQuoteQuantityInformationDto.qmqAdditionalChargeForeign,
					qmqAdditionalCostAmount = eRPQuoteQuantityInformationDto.qmqAdditionalCostAmount,
					qmqAdditionalCostDescription = eRPQuoteQuantityInformationDto.qmqAdditionalCostDescription,
					qmqAdditionalCostPrice = eRPQuoteQuantityInformationDto.qmqAdditionalCostPrice,
					qmqAdditionalMarkupPercent = eRPQuoteQuantityInformationDto.qmqAdditionalMarkupPercent,
					qmqAddSecondTaxAmountBase = eRPQuoteQuantityInformationDto.qmqAddSecondTaxAmountBase,
					qmqAddSecondTaxAmountForeign = eRPQuoteQuantityInformationDto.qmqAddSecondTaxAmountForeign,
					qmqAddTaxAmountBase = eRPQuoteQuantityInformationDto.qmqAddTaxAmountBase,
					qmqAddTaxAmountForeign = eRPQuoteQuantityInformationDto.qmqAddTaxAmountForeign,
					qmqCalculatedUnitPrice = eRPQuoteQuantityInformationDto.qmqCalculatedUnitPrice,
					qmqCommissionPercent = eRPQuoteQuantityInformationDto.qmqCommissionPercent,
					qmqCreatedBy = eRPQuoteQuantityInformationDto.qmqCreatedBy,
					qmqCreatedDate = eRPQuoteQuantityInformationDto.qmqCreatedDate,
					qmqDiscountPercent = eRPQuoteQuantityInformationDto.qmqDiscountPercent,
					qmqDueDate = eRPQuoteQuantityInformationDto.qmqDueDate,
					qmqUniqueID = eRPQuoteQuantityInformationDto.qmqUniqueID,
					qmqFullRevisedUnitPriceBase = eRPQuoteQuantityInformationDto.qmqFullRevisedUnitPriceBase,
					qmqFullRevisedUnitPriceForeign = eRPQuoteQuantityInformationDto.qmqFullRevisedUnitPriceForeign,
					qmqClosed = eRPQuoteQuantityInformationDto.qmqClosed,
					qmqCreatedFromMobile = eRPQuoteQuantityInformationDto.qmqCreatedFromMobile,
					qmqPurchaseToOrder = eRPQuoteQuantityInformationDto.qmqPurchaseToOrder,
					qmqLaborCost = eRPQuoteQuantityInformationDto.qmqLaborCost,
					qmqLaborMarkupPercent = eRPQuoteQuantityInformationDto.qmqLaborMarkupPercent,
					qmqLaborPrice = eRPQuoteQuantityInformationDto.qmqLaborPrice,
					qmqLeadTime = eRPQuoteQuantityInformationDto.qmqLeadTime,
					qmqMaterialCost = eRPQuoteQuantityInformationDto.qmqMaterialCost,
					qmqMaterialMarkupPercent = eRPQuoteQuantityInformationDto.qmqMaterialMarkupPercent,
					qmqMaterialPrice = eRPQuoteQuantityInformationDto.qmqMaterialPrice,
					qmqOverheadCost = eRPQuoteQuantityInformationDto.qmqOverheadCost,
					qmqOverheadMarkupPercent = eRPQuoteQuantityInformationDto.qmqOverheadMarkupPercent,
					qmqOverheadPrice = eRPQuoteQuantityInformationDto.qmqOverheadPrice,
					qmqProductionHours = eRPQuoteQuantityInformationDto.qmqProductionHours,
					qmqPurchaseToOrderCost = eRPQuoteQuantityInformationDto.qmqPurchaseToOrderCost,
					qmqPurchaseToOrderPrice = eRPQuoteQuantityInformationDto.qmqPurchaseToOrderPrice,
					qmqPurchaseUnitCostBase = eRPQuoteQuantityInformationDto.qmqPurchaseUnitCostBase,
					qmqPurToOrderMarkupPercent = eRPQuoteQuantityInformationDto.qmqPurToOrderMarkupPercent,
					qmqQuoteID = eRPQuoteQuantityInformationDto.qmqQuoteID,
					qmqQuoteLineID = eRPQuoteQuantityInformationDto.qmqQuoteLineID,
					qmqQuoteMarkupType = eRPQuoteQuantityInformationDto.qmqQuoteMarkupType,
					qmqQuoteQuantity = eRPQuoteQuantityInformationDto.qmqQuoteQuantity,
					qmqQuotingCost = eRPQuoteQuantityInformationDto.qmqQuotingCost,
					qmqQuotingMarkupPercent = eRPQuoteQuantityInformationDto.qmqQuotingMarkupPercent,
					qmqQuotingPrice = eRPQuoteQuantityInformationDto.qmqQuotingPrice,
					qmqRevisedUnitPriceBase = eRPQuoteQuantityInformationDto.qmqRevisedUnitPriceBase,
					qmqRevisedUnitPriceForeign = eRPQuoteQuantityInformationDto.qmqRevisedUnitPriceForeign,
					qmqRowVersion = eRPQuoteQuantityInformationDto.qmqRowVersion,
					qmqScrapPercent = eRPQuoteQuantityInformationDto.qmqScrapPercent,
					qmqSecondTaxCodeID = eRPQuoteQuantityInformationDto.qmqSecondTaxCodeID,
					qmqQuoteQuantityID = eRPQuoteQuantityInformationDto.qmqQuoteQuantityID,
					qmqSetupHours = eRPQuoteQuantityInformationDto.qmqSetupHours,
					qmqStartDate = eRPQuoteQuantityInformationDto.qmqStartDate,
					qmqSubcontractCost = eRPQuoteQuantityInformationDto.qmqSubcontractCost,
					qmqSubcontractMarkupPercent = eRPQuoteQuantityInformationDto.qmqSubcontractMarkupPercent,
					qmqSubcontractPrice = eRPQuoteQuantityInformationDto.qmqSubcontractPrice,
					qmqTaxCodeID = eRPQuoteQuantityInformationDto.qmqTaxCodeID,
					qmqTaxDate = eRPQuoteQuantityInformationDto.qmqTaxDate,
					qmqTotalCost = eRPQuoteQuantityInformationDto.qmqTotalCost,
					qmqTotalMarkupPercent = eRPQuoteQuantityInformationDto.qmqTotalMarkupPercent,
					qmqTotalPrice = eRPQuoteQuantityInformationDto.qmqTotalPrice,
					qmqTotalRunQuantity = eRPQuoteQuantityInformationDto.qmqTotalRunQuantity,
					qmqTotalUnitCost = eRPQuoteQuantityInformationDto.qmqTotalUnitCost,
					qmqTotalUnitPrice = eRPQuoteQuantityInformationDto.qmqTotalUnitPrice,
					qmqUnitDiscountBase = eRPQuoteQuantityInformationDto.qmqUnitDiscountBase,
					qmqUnitDiscountForeign = eRPQuoteQuantityInformationDto.qmqUnitDiscountForeign,
					qmqUnitSecondTaxAmountBase = eRPQuoteQuantityInformationDto.qmqUnitSecondTaxAmountBase,
					qmqUnitSecondTaxAmountForeign = eRPQuoteQuantityInformationDto.qmqUnitSecondTaxAmountForeign,
					qmqUnitTaxAmountBase = eRPQuoteQuantityInformationDto.qmqUnitTaxAmountBase,
					qmqUnitTaxAmountForeign = eRPQuoteQuantityInformationDto.qmqUnitTaxAmountForeign,
					CustomFields = eRPQuoteQuantityInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteQuantities []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteQuantityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteQuantityDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteQuantityDto>> Process_PutQuoteQuantity(ERPQuoteQuantityDto quoteQuantity)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPQuoteQuantityDto createdObject = null;
		ERPResponseMessageDto<ERPQuoteQuantityDto> result;
		try
		{
			IERPQuoteQuantityRepository iERPQuoteQuantityRepository = (base.ERPQuoteQuantityRepository = new ERPQuoteQuantityRepository(base.ApiClientContext));
			using (iERPQuoteQuantityRepository)
			{
				APIValidationInfoDto postResult = await base.ERPQuoteQuantityRepository.SaveQuoteQuantity(quoteQuantity);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPQuoteQuantityInformationDto eRPQuoteQuantityInformationDto = await base.ERPQuoteQuantityRepository.GetQuoteQuantity(quoteQuantity.qmqUniqueID);
					createdObject = new ERPQuoteQuantityDto
					{
						qmqAdditionalChargeBase = eRPQuoteQuantityInformationDto.qmqAdditionalChargeBase,
						qmqAdditionalChargeDescription = eRPQuoteQuantityInformationDto.qmqAdditionalChargeDescription,
						qmqAdditionalChargeForeign = eRPQuoteQuantityInformationDto.qmqAdditionalChargeForeign,
						qmqAdditionalCostAmount = eRPQuoteQuantityInformationDto.qmqAdditionalCostAmount,
						qmqAdditionalCostDescription = eRPQuoteQuantityInformationDto.qmqAdditionalCostDescription,
						qmqAdditionalCostPrice = eRPQuoteQuantityInformationDto.qmqAdditionalCostPrice,
						qmqAdditionalMarkupPercent = eRPQuoteQuantityInformationDto.qmqAdditionalMarkupPercent,
						qmqAddSecondTaxAmountBase = eRPQuoteQuantityInformationDto.qmqAddSecondTaxAmountBase,
						qmqAddSecondTaxAmountForeign = eRPQuoteQuantityInformationDto.qmqAddSecondTaxAmountForeign,
						qmqAddTaxAmountBase = eRPQuoteQuantityInformationDto.qmqAddTaxAmountBase,
						qmqAddTaxAmountForeign = eRPQuoteQuantityInformationDto.qmqAddTaxAmountForeign,
						qmqCalculatedUnitPrice = eRPQuoteQuantityInformationDto.qmqCalculatedUnitPrice,
						qmqCommissionPercent = eRPQuoteQuantityInformationDto.qmqCommissionPercent,
						qmqCreatedBy = eRPQuoteQuantityInformationDto.qmqCreatedBy,
						qmqCreatedDate = eRPQuoteQuantityInformationDto.qmqCreatedDate,
						qmqDiscountPercent = eRPQuoteQuantityInformationDto.qmqDiscountPercent,
						qmqDueDate = eRPQuoteQuantityInformationDto.qmqDueDate,
						qmqUniqueID = eRPQuoteQuantityInformationDto.qmqUniqueID,
						qmqFullRevisedUnitPriceBase = eRPQuoteQuantityInformationDto.qmqFullRevisedUnitPriceBase,
						qmqFullRevisedUnitPriceForeign = eRPQuoteQuantityInformationDto.qmqFullRevisedUnitPriceForeign,
						qmqClosed = eRPQuoteQuantityInformationDto.qmqClosed,
						qmqCreatedFromMobile = eRPQuoteQuantityInformationDto.qmqCreatedFromMobile,
						qmqPurchaseToOrder = eRPQuoteQuantityInformationDto.qmqPurchaseToOrder,
						qmqLaborCost = eRPQuoteQuantityInformationDto.qmqLaborCost,
						qmqLaborMarkupPercent = eRPQuoteQuantityInformationDto.qmqLaborMarkupPercent,
						qmqLaborPrice = eRPQuoteQuantityInformationDto.qmqLaborPrice,
						qmqLeadTime = eRPQuoteQuantityInformationDto.qmqLeadTime,
						qmqMaterialCost = eRPQuoteQuantityInformationDto.qmqMaterialCost,
						qmqMaterialMarkupPercent = eRPQuoteQuantityInformationDto.qmqMaterialMarkupPercent,
						qmqMaterialPrice = eRPQuoteQuantityInformationDto.qmqMaterialPrice,
						qmqOverheadCost = eRPQuoteQuantityInformationDto.qmqOverheadCost,
						qmqOverheadMarkupPercent = eRPQuoteQuantityInformationDto.qmqOverheadMarkupPercent,
						qmqOverheadPrice = eRPQuoteQuantityInformationDto.qmqOverheadPrice,
						qmqProductionHours = eRPQuoteQuantityInformationDto.qmqProductionHours,
						qmqPurchaseToOrderCost = eRPQuoteQuantityInformationDto.qmqPurchaseToOrderCost,
						qmqPurchaseToOrderPrice = eRPQuoteQuantityInformationDto.qmqPurchaseToOrderPrice,
						qmqPurchaseUnitCostBase = eRPQuoteQuantityInformationDto.qmqPurchaseUnitCostBase,
						qmqPurToOrderMarkupPercent = eRPQuoteQuantityInformationDto.qmqPurToOrderMarkupPercent,
						qmqQuoteID = eRPQuoteQuantityInformationDto.qmqQuoteID,
						qmqQuoteLineID = eRPQuoteQuantityInformationDto.qmqQuoteLineID,
						qmqQuoteMarkupType = eRPQuoteQuantityInformationDto.qmqQuoteMarkupType,
						qmqQuoteQuantity = eRPQuoteQuantityInformationDto.qmqQuoteQuantity,
						qmqQuotingCost = eRPQuoteQuantityInformationDto.qmqQuotingCost,
						qmqQuotingMarkupPercent = eRPQuoteQuantityInformationDto.qmqQuotingMarkupPercent,
						qmqQuotingPrice = eRPQuoteQuantityInformationDto.qmqQuotingPrice,
						qmqRevisedUnitPriceBase = eRPQuoteQuantityInformationDto.qmqRevisedUnitPriceBase,
						qmqRevisedUnitPriceForeign = eRPQuoteQuantityInformationDto.qmqRevisedUnitPriceForeign,
						qmqRowVersion = eRPQuoteQuantityInformationDto.qmqRowVersion,
						qmqScrapPercent = eRPQuoteQuantityInformationDto.qmqScrapPercent,
						qmqSecondTaxCodeID = eRPQuoteQuantityInformationDto.qmqSecondTaxCodeID,
						qmqQuoteQuantityID = eRPQuoteQuantityInformationDto.qmqQuoteQuantityID,
						qmqSetupHours = eRPQuoteQuantityInformationDto.qmqSetupHours,
						qmqStartDate = eRPQuoteQuantityInformationDto.qmqStartDate,
						qmqSubcontractCost = eRPQuoteQuantityInformationDto.qmqSubcontractCost,
						qmqSubcontractMarkupPercent = eRPQuoteQuantityInformationDto.qmqSubcontractMarkupPercent,
						qmqSubcontractPrice = eRPQuoteQuantityInformationDto.qmqSubcontractPrice,
						qmqTaxCodeID = eRPQuoteQuantityInformationDto.qmqTaxCodeID,
						qmqTaxDate = eRPQuoteQuantityInformationDto.qmqTaxDate,
						qmqTotalCost = eRPQuoteQuantityInformationDto.qmqTotalCost,
						qmqTotalMarkupPercent = eRPQuoteQuantityInformationDto.qmqTotalMarkupPercent,
						qmqTotalPrice = eRPQuoteQuantityInformationDto.qmqTotalPrice,
						qmqTotalRunQuantity = eRPQuoteQuantityInformationDto.qmqTotalRunQuantity,
						qmqTotalUnitCost = eRPQuoteQuantityInformationDto.qmqTotalUnitCost,
						qmqTotalUnitPrice = eRPQuoteQuantityInformationDto.qmqTotalUnitPrice,
						qmqUnitDiscountBase = eRPQuoteQuantityInformationDto.qmqUnitDiscountBase,
						qmqUnitDiscountForeign = eRPQuoteQuantityInformationDto.qmqUnitDiscountForeign,
						qmqUnitSecondTaxAmountBase = eRPQuoteQuantityInformationDto.qmqUnitSecondTaxAmountBase,
						qmqUnitSecondTaxAmountForeign = eRPQuoteQuantityInformationDto.qmqUnitSecondTaxAmountForeign,
						qmqUnitTaxAmountBase = eRPQuoteQuantityInformationDto.qmqUnitTaxAmountBase,
						qmqUnitTaxAmountForeign = eRPQuoteQuantityInformationDto.qmqUnitTaxAmountForeign,
						CustomFields = eRPQuoteQuantityInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing QuoteQuantity [{quoteQuantity.qmqUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteQuantityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteQuantity(Guid quoteQuantityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteQuantityRepository iERPQuoteQuantityRepository = (base.ERPQuoteQuantityRepository = new ERPQuoteQuantityRepository(base.ApiClientContext));
		using (iERPQuoteQuantityRepository)
		{
			if (!(await base.ERPQuoteQuantityRepository.DoesQuoteQuantityExist(quoteQuantityId)))
			{
				base.ErrorsList.Add($"QuoteQuantity [{quoteQuantityId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPQuoteQuantityInformationDto eRPQuoteQuantityInformationDto = await base.ERPQuoteQuantityRepository.GetQuoteQuantity(quoteQuantityId);
				string text = await base.ERPQuoteQuantityRepository.WhereUsed("QuoteQuantities", new object[3] { eRPQuoteQuantityInformationDto.qmqQuoteID, eRPQuoteQuantityInformationDto.qmqQuoteLineID, eRPQuoteQuantityInformationDto.qmqQuoteQuantityID }, new object[3] { "qmqQuoteID", "qmqQuoteLineID", "qmqQuoteQuantityID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("QuoteQuantity cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPQuoteQuantityDto>> Process_DeleteQuoteQuantity(Guid quoteQuantityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPQuoteQuantityDto> result;
		try
		{
			IERPQuoteQuantityRepository iERPQuoteQuantityRepository = (base.ERPQuoteQuantityRepository = new ERPQuoteQuantityRepository(base.ApiClientContext));
			using (iERPQuoteQuantityRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPQuoteQuantityRepository.DeleteRowFromTable("QuoteQuantities", "qmq", quoteQuantityId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of QuoteQuantity [{quoteQuantityId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteQuantityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPQuoteQuantityDto()
			};
		}
		return result;
	}
}
