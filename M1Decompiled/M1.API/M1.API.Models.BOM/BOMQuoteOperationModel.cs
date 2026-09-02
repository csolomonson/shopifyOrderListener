using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Sales;

namespace M1.API.Models.BOM;

public class BOMQuoteOperationModel : BOMBaseModel, IBOMQuoteOperationModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteOperation(string quoteId, string quoteLineId = "", string quoteAssemblyId = "")
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

	public async Task<BOMResponseMessageDto<IList<BOMQuoteOperationDto>>> Process_GetAllQuoteOperations(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteOperationDto> allQuoteOperationsDto = new List<BOMQuoteOperationDto>();
		BOMResponseMessageDto<IList<BOMQuoteOperationDto>> result;
		try
		{
			using QuoteOperationRepository quoteOperationRepository = new QuoteOperationRepository(base.ApiClientContext);
			foreach (BOMQuoteOperationDto item2 in await quoteOperationRepository.GetAllQuoteOperations(pageSize, pageNumber))
			{
				BOMQuoteOperationDto item = new BOMQuoteOperationDto
				{
					QuoteID = item2.QuoteID,
					QuoteLineID = item2.QuoteLineID,
					QuoteAssemblyID = item2.QuoteAssemblyID,
					QuoteOperationID = item2.QuoteOperationID,
					OperationType = item2.OperationType,
					WorkCenterID = item2.WorkCenterID,
					ProcessID = item2.ProcessID,
					ProcessShortDescription = item2.ProcessShortDescription,
					ProcessLongDescriptionRtf = item2.ProcessLongDescriptionRtf,
					ProcessLongDescriptionText = item2.ProcessLongDescriptionText,
					QuantityPerAssembly = item2.QuantityPerAssembly,
					QueueTime = item2.QueueTime,
					SetupHours = item2.SetupHours,
					MoveTime = item2.MoveTime,
					QuotingRate = item2.QuotingRate,
					SetupRate = item2.SetupRate,
					ProductionRate = item2.ProductionRate,
					OverheadRate = item2.OverheadRate,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					UnitOfMeasure = item2.UnitOfMeasure,
					SupplierOrganizationID = item2.SupplierOrganizationID,
					StandardFactor = item2.StandardFactor,
					ProductionStandard = item2.ProductionStandard,
					Closed = item2.Closed,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				allQuoteOperationsDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteOperations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMQuoteOperationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteOperationsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<IList<BOMQuoteOperationDto>>> Process_GetQuoteOperations(string quoteId, string quoteLineId = "", string quoteAssemblyId = "")
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteOperationDto> quoteOperationsDto = new List<BOMQuoteOperationDto>();
		BOMResponseMessageDto<IList<BOMQuoteOperationDto>> result;
		try
		{
			using QuoteOperationRepository quoteOperationRepository = new QuoteOperationRepository(base.ApiClientContext);
			foreach (BOMQuoteOperationDto item2 in await quoteOperationRepository.GetQuoteOperationsAsync(quoteId, quoteLineId, quoteAssemblyId))
			{
				BOMQuoteOperationDto item = new BOMQuoteOperationDto
				{
					QuoteID = item2.QuoteID,
					QuoteLineID = item2.QuoteLineID,
					QuoteAssemblyID = item2.QuoteAssemblyID,
					QuoteOperationID = item2.QuoteOperationID,
					OperationType = item2.OperationType,
					WorkCenterID = item2.WorkCenterID,
					ProcessID = item2.ProcessID,
					ProcessShortDescription = item2.ProcessShortDescription,
					ProcessLongDescriptionRtf = item2.ProcessLongDescriptionRtf,
					ProcessLongDescriptionText = item2.ProcessLongDescriptionText,
					QuantityPerAssembly = item2.QuantityPerAssembly,
					QueueTime = item2.QueueTime,
					SetupHours = item2.SetupHours,
					MoveTime = item2.MoveTime,
					QuotingRate = item2.QuotingRate,
					SetupRate = item2.SetupRate,
					ProductionRate = item2.ProductionRate,
					OverheadRate = item2.OverheadRate,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					UnitOfMeasure = item2.UnitOfMeasure,
					SupplierOrganizationID = item2.SupplierOrganizationID,
					StandardFactor = item2.StandardFactor,
					ProductionStandard = item2.ProductionStandard,
					Closed = item2.Closed,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				quoteOperationsDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteOperations []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMQuoteOperationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteOperationsDto
			};
		}
		return result;
	}
}
