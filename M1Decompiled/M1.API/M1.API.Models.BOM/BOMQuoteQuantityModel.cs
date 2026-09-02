using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Sales;

namespace M1.API.Models.BOM;

public class BOMQuoteQuantityModel : BOMBaseModel, IBOMQuoteQuantityModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteQuantity(string quoteId, string quoteLineId = "")
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (QuoteRepository quoteRepository = new QuoteRepository(base.ApiClientContext))
		{
			if (!quoteRepository.DoesQuoteExistsAsync(quoteId).Result)
			{
				list.Add("Quote [" + quoteId + "] is invalid");
			}
		}
		if (!string.IsNullOrEmpty(quoteLineId))
		{
			using QuoteLineRepository quoteLineRepository = new QuoteLineRepository(base.ApiClientContext);
			if (!quoteLineRepository.DoesQuoteLineExists(quoteId, quoteLineId).Result)
			{
				list.Add("Quote [" + quoteId + "], containing Quote Line [" + quoteLineId + "] is invalid");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<IList<BOMQuoteQuantityDto>>> Process_GetAllQuoteQuantities(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteQuantityDto> allQuoteQuantitiesDto = new List<BOMQuoteQuantityDto>();
		BOMResponseMessageDto<IList<BOMQuoteQuantityDto>> result;
		try
		{
			using QuoteQuantityRepository quoteQuantityRepository = new QuoteQuantityRepository(base.ApiClientContext);
			foreach (BOMQuoteQuantityDto item2 in await quoteQuantityRepository.GetAllQuoteQuantities(pageSize, pageNumber))
			{
				BOMQuoteQuantityDto item = new BOMQuoteQuantityDto
				{
					QuoteID = item2.QuoteID,
					QuoteLineID = item2.QuoteLineID,
					QuoteQuantityID = item2.QuoteQuantityID,
					QuoteQuantity = item2.QuoteQuantity,
					ScrapPercent = item2.ScrapPercent,
					TotalRunQuantity = item2.TotalRunQuantity,
					QuoteMarkupType = item2.QuoteMarkupType,
					PurchaseToOrder = item2.PurchaseToOrder,
					SetupHours = item2.SetupHours,
					ProductionHours = item2.ProductionHours,
					MaterialCost = item2.MaterialCost,
					MaterialMarkupPercent = item2.MaterialMarkupPercent,
					MaterialPrice = item2.MaterialPrice,
					SubcontractPrice = item2.SubcontractPrice,
					LaborCost = item2.LaborCost,
					LaborMarkupPercent = item2.LaborMarkupPercent,
					LaborPrice = item2.LaborPrice,
					OverheadCost = item2.OverheadCost,
					OverheadMarkupPercent = item2.OverheadMarkupPercent,
					OverheadPrice = item2.OverheadPrice,
					QuotingPrice = item2.QuotingPrice,
					PurchaseUnitCostBase = item2.PurchaseUnitCostBase,
					PurchaseToOrderCost = item2.PurchaseToOrderCost,
					PurToOrderMarkupPercent = item2.PurToOrderMarkupPercent,
					PurchaseToOrderPrice = item2.PurchaseToOrderPrice,
					AdditionalCostAmount = item2.AdditionalCostAmount,
					AdditionalMarkupPercent = item2.AdditionalMarkupPercent,
					AdditionalCostPrice = item2.AdditionalCostPrice,
					TotalCost = item2.TotalCost,
					TotalPrice = item2.TotalPrice,
					TotalUnitCost = item2.TotalUnitCost,
					TotalMarkupPercent = item2.TotalMarkupPercent,
					TotalUnitPrice = item2.TotalUnitPrice,
					CalculatedUnitPrice = item2.CalculatedUnitPrice,
					FullRevisedUnitPriceForeign = item2.FullRevisedUnitPriceForeign,
					DiscountPercent = item2.DiscountPercent,
					UnitDiscountForeign = item2.UnitDiscountForeign,
					RevisedUnitPriceForeign = item2.RevisedUnitPriceForeign,
					AdditionalChargeForeign = item2.AdditionalChargeForeign,
					AdditionalChargeDescription = item2.AdditionalChargeDescription,
					LeadTime = item2.LeadTime,
					Closed = item2.Closed,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				allQuoteQuantitiesDto.Add(item);
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
			result = new BOMResponseMessageDto<IList<BOMQuoteQuantityDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteQuantitiesDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<IList<BOMQuoteQuantityDto>>> Process_GetQuoteQuantities(string quoteId, string quoteLineId = "")
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteQuantityDto> quoteQuantitiesDto = new List<BOMQuoteQuantityDto>();
		BOMResponseMessageDto<IList<BOMQuoteQuantityDto>> result;
		try
		{
			using QuoteQuantityRepository quoteQuantityRepository = new QuoteQuantityRepository(base.ApiClientContext);
			foreach (BOMQuoteQuantityDto item2 in await quoteQuantityRepository.GetQuoteQuantitiesInfo(quoteId, quoteLineId))
			{
				BOMQuoteQuantityDto item = new BOMQuoteQuantityDto
				{
					QuoteID = item2.QuoteID,
					QuoteLineID = item2.QuoteLineID,
					QuoteQuantityID = item2.QuoteQuantityID,
					QuoteQuantity = item2.QuoteQuantity,
					ScrapPercent = item2.ScrapPercent,
					TotalRunQuantity = item2.TotalRunQuantity,
					QuoteMarkupType = item2.QuoteMarkupType,
					PurchaseToOrder = item2.PurchaseToOrder,
					SetupHours = item2.SetupHours,
					ProductionHours = item2.ProductionHours,
					MaterialCost = item2.MaterialCost,
					MaterialMarkupPercent = item2.MaterialMarkupPercent,
					MaterialPrice = item2.MaterialPrice,
					SubcontractPrice = item2.SubcontractPrice,
					LaborCost = item2.LaborCost,
					LaborMarkupPercent = item2.LaborMarkupPercent,
					LaborPrice = item2.LaborPrice,
					OverheadCost = item2.OverheadCost,
					OverheadMarkupPercent = item2.OverheadMarkupPercent,
					OverheadPrice = item2.OverheadPrice,
					QuotingPrice = item2.QuotingPrice,
					PurchaseUnitCostBase = item2.PurchaseUnitCostBase,
					PurchaseToOrderCost = item2.PurchaseToOrderCost,
					PurToOrderMarkupPercent = item2.PurToOrderMarkupPercent,
					PurchaseToOrderPrice = item2.PurchaseToOrderPrice,
					AdditionalCostAmount = item2.AdditionalCostAmount,
					AdditionalMarkupPercent = item2.AdditionalMarkupPercent,
					AdditionalCostPrice = item2.AdditionalCostPrice,
					TotalCost = item2.TotalCost,
					TotalPrice = item2.TotalPrice,
					TotalUnitCost = item2.TotalUnitCost,
					TotalMarkupPercent = item2.TotalMarkupPercent,
					TotalUnitPrice = item2.TotalUnitPrice,
					CalculatedUnitPrice = item2.CalculatedUnitPrice,
					FullRevisedUnitPriceForeign = item2.FullRevisedUnitPriceForeign,
					DiscountPercent = item2.DiscountPercent,
					UnitDiscountForeign = item2.UnitDiscountForeign,
					RevisedUnitPriceForeign = item2.RevisedUnitPriceForeign,
					AdditionalChargeForeign = item2.AdditionalChargeForeign,
					AdditionalChargeDescription = item2.AdditionalChargeDescription,
					LeadTime = item2.LeadTime,
					Closed = item2.Closed,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				quoteQuantitiesDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering QuoteQuantities based on quoteId [" + quoteId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMQuoteQuantityDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteQuantitiesDto
			};
		}
		return result;
	}
}
