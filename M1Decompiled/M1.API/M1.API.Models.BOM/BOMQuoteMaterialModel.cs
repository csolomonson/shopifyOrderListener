using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Sales;

namespace M1.API.Models.BOM;

public class BOMQuoteMaterialModel : BOMBaseModel, IBOMQuoteMaterialModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteMaterialsAsync(string quoteId, string quoteLineId = "", string quoteAssemblyId = "")
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		if (!string.IsNullOrEmpty(quoteLineId) && string.IsNullOrEmpty(quoteAssemblyId))
		{
			using QuoteLineRepository quoteLineRepository = new QuoteLineRepository(base.ApiClientContext);
			if (!quoteLineRepository.DoesQuoteLineExists(quoteId, quoteLineId).Result)
			{
				list.Add("Quote ID [" + quoteId + "] contains an invalid QuoteLine ID [" + quoteLineId + "].");
			}
		}
		if (!string.IsNullOrEmpty(quoteAssemblyId))
		{
			using QuoteAssemblyRepository quoteAssemblyRepository = new QuoteAssemblyRepository(base.ApiClientContext);
			if (!quoteAssemblyRepository.DoesQuoteAssemblyExist(quoteId, quoteLineId, quoteAssemblyId).Result)
			{
				list.Add("Quote ID [" + quoteId + "] contains an invalid QuoteLine ID [" + quoteLineId + "] within QuoteAssembly ID [" + quoteAssemblyId + "].");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PostQuoteMaterialAsync(BOMCreateQuoteMaterialDto quoteMaterial)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			if (quoteMaterial.QuoteMaterialID == 0)
			{
				base.ErrorsList.Add("QuoteMaterialID is invalid or empty.");
			}
			using (QuoteAssemblyRepository quoteAssemblyRepository = new QuoteAssemblyRepository(base.ApiClientContext))
			{
				if (!(await quoteAssemblyRepository.DoesQuoteAssemblyExist(quoteMaterial.QuoteID, quoteMaterial.QuoteLineID.ToString(), quoteMaterial.QuoteAssemblyID.ToString())))
				{
					base.ErrorsList.Add($"Quote ID [{quoteMaterial.QuoteID}] contains an invalid QuoteLine ID [{quoteMaterial.QuoteLineID}] within QuoteAssembly ID [{quoteMaterial.QuoteAssemblyID}].");
				}
			}
			using (PartRepository partRevisionRepository = new PartRepository(base.ApiClientContext))
			{
				if (!(await partRevisionRepository.DoesPartRevisionExists(quoteMaterial.PartID, quoteMaterial.PartRevisionID)))
				{
					base.ErrorsList.Add("Part with ID [" + quoteMaterial.PartID + "], containing PartRevision with ID [" + quoteMaterial.PartRevisionID + "], is invalid");
				}
			}
			using OrganizationRepository organizationRepository = new OrganizationRepository(base.ApiClientContext);
			if (!string.IsNullOrWhiteSpace(quoteMaterial.SupplierOrganizationID) && !(await organizationRepository.DoesOrganizationExists(quoteMaterial.SupplierOrganizationID)))
			{
				base.ErrorsList.Add("Supplier Organization with ID [" + quoteMaterial.SupplierOrganizationID + "] is not valid.");
			}
			if (!string.IsNullOrWhiteSpace(quoteMaterial.PurchaseLocationID) && !(await organizationRepository.DoesSupplierPurchaseLocationExists(quoteMaterial.SupplierOrganizationID, quoteMaterial.PurchaseLocationID)))
			{
				base.ErrorsList.Add("Supplier with ID [" + quoteMaterial.SupplierOrganizationID + "] or Purchase Location with ID [" + quoteMaterial.PurchaseLocationID + "] is invalid");
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while validating QuoteMaterial with QuoteID [{quoteMaterial.QuoteID}], QuoteLine ID  [{quoteMaterial.QuoteLineID}] and QuoteAssembly ID [{quoteMaterial.QuoteAssemblyID}]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public async Task<BOMResponseMessageDto<IList<BOMQuoteMaterialDto>>> Process_GetAllQuoteMaterials(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteMaterialDto> allQuoteMaterialsDto = new List<BOMQuoteMaterialDto>();
		BOMResponseMessageDto<IList<BOMQuoteMaterialDto>> result;
		try
		{
			using QuoteMaterialRepository quoteMaterialRepository = new QuoteMaterialRepository(base.ApiClientContext);
			foreach (BOMQuoteMaterialDto item2 in await quoteMaterialRepository.GetAllQuoteMaterials(pageSize, pageNumber))
			{
				BOMQuoteMaterialDto item = new BOMQuoteMaterialDto
				{
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					EstimatedUnitCost = item2.EstimatedUnitCost,
					Closed = item2.Closed,
					LeadTime = item2.LeadTime,
					MinimumCharge = item2.MinimumCharge,
					PartBinID = item2.PartBinID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					PartShortDescription = item2.PartShortDescription,
					PartWarehouseLocationID = item2.PartWarehouseLocationID,
					PurchaseLocationID = item2.PurchaseLocationID,
					QuantityPerAssembly = item2.QuantityPerAssembly,
					QuoteAssemblyID = item2.QuoteAssemblyID,
					QuoteID = item2.QuoteID,
					QuoteLineID = item2.QuoteLineID,
					ScrapPercent = item2.ScrapPercent,
					ScrapQuantity = item2.ScrapQuantity,
					QuoteMaterialID = item2.QuoteMaterialID,
					SupplierOrganizationID = item2.SupplierOrganizationID,
					UnitOfMeasure = item2.UnitOfMeasure,
					RowVersion = item2.RowVersion
				};
				allQuoteMaterialsDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteMaterials]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMQuoteMaterialDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteMaterialsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<IList<BOMQuoteMaterialDto>>> Process_GetQuoteMaterialsAsync(string quoteId, string quoteLineId = "", string quoteAssemblyId = "")
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteMaterialDto> quoteMaterialsDto = new List<BOMQuoteMaterialDto>();
		BOMResponseMessageDto<IList<BOMQuoteMaterialDto>> result;
		try
		{
			using QuoteMaterialRepository quoteMaterialRepository = new QuoteMaterialRepository(base.ApiClientContext);
			foreach (BOMQuoteMaterialDto item2 in await quoteMaterialRepository.GetQuoteMaterialsAsync(quoteId, quoteLineId, quoteAssemblyId))
			{
				BOMQuoteMaterialDto item = new BOMQuoteMaterialDto
				{
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					EstimatedUnitCost = item2.EstimatedUnitCost,
					Closed = item2.Closed,
					LeadTime = item2.LeadTime,
					MinimumCharge = item2.MinimumCharge,
					PartBinID = item2.PartBinID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					PartShortDescription = item2.PartShortDescription,
					PartWarehouseLocationID = item2.PartWarehouseLocationID,
					PurchaseLocationID = item2.PurchaseLocationID,
					QuantityPerAssembly = item2.QuantityPerAssembly,
					QuoteAssemblyID = item2.QuoteAssemblyID,
					QuoteID = item2.QuoteID,
					QuoteLineID = item2.QuoteLineID,
					ScrapPercent = item2.ScrapPercent,
					ScrapQuantity = item2.ScrapQuantity,
					QuoteMaterialID = item2.QuoteMaterialID,
					SupplierOrganizationID = item2.SupplierOrganizationID,
					UnitOfMeasure = item2.UnitOfMeasure,
					RowVersion = item2.RowVersion
				};
				quoteMaterialsDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteMaterials []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMQuoteMaterialDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteMaterialsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMCreateQuoteMaterialDto>> Process_PostQuoteMaterialAsync(BOMCreateQuoteMaterialDto quoteMaterial)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		BOMResponseMessageDto<BOMCreateQuoteMaterialDto> result;
		try
		{
			using QuoteMaterialRepository quoteMaterialRepository = new QuoteMaterialRepository(base.ApiClientContext);
			APIValidationInfoDto aPIValidationInfoDto = await quoteMaterialRepository.SaveQuoteMaterialAsync(quoteMaterial);
			((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Quote Material [{quoteMaterial.QuoteMaterialID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMCreateQuoteMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteMaterial
			};
		}
		return result;
	}
}
