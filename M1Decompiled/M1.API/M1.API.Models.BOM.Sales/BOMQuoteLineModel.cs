using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Sales;

namespace M1.API.Models.BOM.Sales;

public class BOMQuoteLineModel : BOMBaseModel, IBOMQuoteLineModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteLine(string quoteId, string quoteLineId)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (QuoteLineRepository quoteLineRepository = new QuoteLineRepository(base.ApiClientContext))
		{
			if (!quoteLineRepository.DoesQuoteLineExists(quoteId, quoteLineId).Result)
			{
				list.Add("Quote [" + quoteId + "], containing QuoteLine [" + quoteLineId + "], is invalid");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PostQuoteLineAsync(BOMCreateQuoteLineDto quoteLine)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using (QuoteRepository quoteRepository = new QuoteRepository(base.ApiClientContext))
			{
				if (!quoteRepository.DoesQuoteExistsAsync(quoteLine.QuoteID).Result)
				{
					base.ErrorsList.Add("Quote [" + quoteLine.QuoteID + "] is invalid");
				}
			}
			using (PartRepository partRevisionRepository = new PartRepository(base.ApiClientContext))
			{
				if (!(await partRevisionRepository.DoesPartRevisionExists(quoteLine.PartID, quoteLine.PartRevisionID)))
				{
					base.ErrorsList.Add("Part with ID [" + quoteLine.PartID + "], containing PartRevision with ID [" + quoteLine.PartRevisionID + "], is invalid");
				}
			}
			using OrganizationRepository organizationRepository = new OrganizationRepository(base.ApiClientContext);
			if (!string.IsNullOrWhiteSpace(quoteLine.SupplierOrganizationID) && !(await organizationRepository.DoesOrganizationExists(quoteLine.SupplierOrganizationID)))
			{
				base.ErrorsList.Add("Supplier Organization with ID [" + quoteLine.SupplierOrganizationID + "] is not valid.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.PurchaseLocationID) && !(await organizationRepository.DoesSupplierPurchaseLocationExists(quoteLine.SupplierOrganizationID, quoteLine.PurchaseLocationID)))
			{
				base.ErrorsList.Add("Purchase Location with ID [" + quoteLine.PurchaseLocationID + "] is not valid.");
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating Quote [" + quoteLine.QuoteID + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public async Task<BOMResponseMessageDto<IList<BOMQuoteLineDto>>> Process_GetAllQuoteLines(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteLineDto> allQuoteLinesDto = new List<BOMQuoteLineDto>();
		BOMResponseMessageDto<IList<BOMQuoteLineDto>> result;
		try
		{
			using QuoteLineRepository quoteLineRepository = new QuoteLineRepository(base.ApiClientContext);
			foreach (BOMQuoteLineDto item2 in await quoteLineRepository.GetAllQuoteLines(pageSize, pageNumber))
			{
				BOMQuoteLineDto item = new BOMQuoteLineDto
				{
					QuoteID = item2.QuoteID,
					QuoteLineID = item2.QuoteLineID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					UnitOfMeasure = item2.UnitOfMeasure,
					PartGroupID = item2.PartGroupID,
					PartShortDescription = item2.PartShortDescription,
					OrgPartShortDescription = item2.OrgPartShortDescription,
					ResolutionReasonID = item2.ResolutionReasonID,
					QuoteMarkupType = item2.QuoteMarkupType,
					PurchaseToOrder = item2.PurchaseToOrder,
					PurchaseUnitCostForeign = item2.PurchaseUnitCostForeign,
					SupplierOrganizationID = item2.SupplierOrganizationID,
					PurchaseLocationID = item2.PurchaseLocationID,
					Firm = item2.Firm,
					ProjectID = item2.ProjectID,
					ProjectAreaID = item2.ProjectAreaID,
					Closed = item2.Closed,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				allQuoteLinesDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMQuoteLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteLinesDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMQuoteLineDto>> Process_GetQuoteLine(string quoteId, string quoteLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMQuoteLineDto quoteLineDto = null;
		BOMResponseMessageDto<BOMQuoteLineDto> result;
		try
		{
			using QuoteLineRepository quoteLineRepository = new QuoteLineRepository(base.ApiClientContext);
			BOMQuoteLineDto bOMQuoteLineDto = await quoteLineRepository.GetQuoteLine(quoteId, quoteLineId);
			quoteLineDto = new BOMQuoteLineDto
			{
				QuoteID = bOMQuoteLineDto.QuoteID,
				QuoteLineID = bOMQuoteLineDto.QuoteLineID,
				PartID = bOMQuoteLineDto.PartID,
				PartRevisionID = bOMQuoteLineDto.PartRevisionID,
				UnitOfMeasure = bOMQuoteLineDto.UnitOfMeasure,
				PartGroupID = bOMQuoteLineDto.PartGroupID,
				PartShortDescription = bOMQuoteLineDto.PartShortDescription,
				OrgPartShortDescription = bOMQuoteLineDto.OrgPartShortDescription,
				ResolutionReasonID = bOMQuoteLineDto.ResolutionReasonID,
				QuoteMarkupType = bOMQuoteLineDto.QuoteMarkupType,
				PurchaseToOrder = bOMQuoteLineDto.PurchaseToOrder,
				PurchaseUnitCostForeign = bOMQuoteLineDto.PurchaseUnitCostForeign,
				SupplierOrganizationID = bOMQuoteLineDto.SupplierOrganizationID,
				PurchaseLocationID = bOMQuoteLineDto.PurchaseLocationID,
				Firm = bOMQuoteLineDto.Firm,
				ProjectID = bOMQuoteLineDto.ProjectID,
				ProjectAreaID = bOMQuoteLineDto.ProjectAreaID,
				Closed = bOMQuoteLineDto.Closed,
				CreatedBy = bOMQuoteLineDto.CreatedBy,
				CreatedDate = bOMQuoteLineDto.CreatedDate,
				UniqueID = bOMQuoteLineDto.UniqueID,
				RowVersion = bOMQuoteLineDto.RowVersion
			};
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMQuoteLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteLineDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMCreateQuoteLineDto>> Process_PostQuoteLineAsync(BOMCreateQuoteLineDto quoteLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		BOMResponseMessageDto<BOMCreateQuoteLineDto> result;
		try
		{
			using QuoteLineRepository quoteLineRepository = new QuoteLineRepository(base.ApiClientContext);
			APIValidationInfoDto aPIValidationInfoDto = await quoteLineRepository.SaveQuoteLineAsync(quoteLine);
			((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Quote Line [{quoteLine.QuoteLineID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMCreateQuoteLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteLine
			};
		}
		return result;
	}
}
