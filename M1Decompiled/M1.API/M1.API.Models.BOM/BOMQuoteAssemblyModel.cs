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

public class BOMQuoteAssemblyModel : BOMBaseModel, IBOMQuoteAssemblyModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteAssembly(string quoteId, string quoteLineId = "")
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

	public async Task<APIValidationInfoDto> ValidateRequest_PostQuoteAssemblyAsync(BOMCreateQuoteAssemblyDto quoteAssembly)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using (QuoteLineRepository quoteLineRepository = new QuoteLineRepository(base.ApiClientContext))
			{
				if (!(await quoteLineRepository.DoesQuoteLineExists(quoteAssembly.QuoteID, quoteAssembly.QuoteLineID.ToString())))
				{
					base.ErrorsList.Add($"Quote with ID [{quoteAssembly.QuoteID}], containing QuoteLine with ID [{quoteAssembly.QuoteLineID}], is invalid");
				}
			}
			using (QuoteAssemblyRepository quoteAssemblyRepository = new QuoteAssemblyRepository(base.ApiClientContext))
			{
				if (!(await quoteAssemblyRepository.DoesQuoteAssemblyExist(quoteAssembly.QuoteID, quoteAssembly.QuoteLineID.ToString(), quoteAssembly.ParentAssemblyID.ToString())))
				{
					base.ErrorsList.Add($"Quote parent assembly with ID [{quoteAssembly.ParentAssemblyID}], with Quote ID [{quoteAssembly.QuoteID}], containing QuoteLine ID [{quoteAssembly.QuoteLineID}], is invalid");
				}
			}
			using (PartRepository partRepository = new PartRepository(base.ApiClientContext))
			{
				if (!(await partRepository.DoesPartRevisionExists(quoteAssembly.PartID, quoteAssembly.PartRevisionID)))
				{
					base.ErrorsList.Add("Part with ID [" + quoteAssembly.PartID + "], containing PartRevision with ID [" + quoteAssembly.PartRevisionID + "], is invalid");
				}
			}
			if (quoteAssembly.Level <= 1 && quoteAssembly.Level > 10)
			{
				base.ErrorsList.Add($"Quote assembly level [{quoteAssembly.Level}] exceeds the allowable range");
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while validating QuoteAssembly [{quoteAssembly.QuoteAssemblyID}]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public async Task<BOMResponseMessageDto<IList<BOMQuoteAssemblyDto>>> Process_GetAllQuoteAssemblies(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteAssemblyDto> allQuoteAssembliesDto = new List<BOMQuoteAssemblyDto>();
		BOMResponseMessageDto<IList<BOMQuoteAssemblyDto>> result;
		try
		{
			using QuoteAssemblyRepository quoteAssemblyRepository = new QuoteAssemblyRepository(base.ApiClientContext);
			foreach (BOMQuoteAssemblyDto item2 in await quoteAssemblyRepository.GetAllQuoteAssemblies(pageSize, pageNumber))
			{
				BOMQuoteAssemblyDto item = new BOMQuoteAssemblyDto
				{
					QuoteID = item2.QuoteID,
					QuoteLineID = item2.QuoteLineID,
					QuoteAssemblyID = item2.QuoteAssemblyID,
					ParentAssemblyID = item2.ParentAssemblyID,
					Level = item2.Level,
					SourceMethodID = item2.SourceMethodID,
					SourceRevisionID = item2.SourceRevisionID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					UnitOfMeasure = item2.UnitOfMeasure,
					PartShortDescription = item2.PartShortDescription,
					QuantityPerParent = item2.QuantityPerParent,
					Closed = item2.Closed,
					PullAllFromStock = item2.PullAllFromStock,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				allQuoteAssembliesDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteAssemblies]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMQuoteAssemblyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteAssembliesDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<IList<BOMQuoteAssemblyDto>>> Process_GetQuoteAssemblies(string quoteId, string quoteLineId = "")
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMQuoteAssemblyDto> quoteAssembliesDto = new List<BOMQuoteAssemblyDto>();
		BOMResponseMessageDto<IList<BOMQuoteAssemblyDto>> result;
		try
		{
			using QuoteAssemblyRepository quoteAssemblyRepository = new QuoteAssemblyRepository(base.ApiClientContext);
			foreach (BOMQuoteAssemblyDto item2 in await quoteAssemblyRepository.GetQuoteAssemblies(quoteId, quoteLineId))
			{
				BOMQuoteAssemblyDto item = new BOMQuoteAssemblyDto
				{
					QuoteID = item2.QuoteID,
					QuoteLineID = item2.QuoteLineID,
					QuoteAssemblyID = item2.QuoteAssemblyID,
					ParentAssemblyID = item2.ParentAssemblyID,
					Level = item2.Level,
					SourceMethodID = item2.SourceMethodID,
					SourceRevisionID = item2.SourceRevisionID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					UnitOfMeasure = item2.UnitOfMeasure,
					PartShortDescription = item2.PartShortDescription,
					QuantityPerParent = item2.QuantityPerParent,
					Closed = item2.Closed,
					PullAllFromStock = item2.PullAllFromStock,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				quoteAssembliesDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteAssemblies []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMQuoteAssemblyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteAssembliesDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMCreateQuoteAssemblyDto>> Process_PostQuoteAssemblyAsync(BOMCreateQuoteAssemblyDto quoteAssembly)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		BOMResponseMessageDto<BOMCreateQuoteAssemblyDto> result;
		try
		{
			using QuoteAssemblyRepository quoteAssemblyRepository = new QuoteAssemblyRepository(base.ApiClientContext);
			APIValidationInfoDto aPIValidationInfoDto = await quoteAssemblyRepository.SaveQuoteAssemblyAsync(quoteAssembly);
			((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Quote Assembly [{quoteAssembly.QuoteAssemblyID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMCreateQuoteAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteAssembly
			};
		}
		return result;
	}
}
