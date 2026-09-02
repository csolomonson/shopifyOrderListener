using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProductCategoryLineModel : ERPBaseModel, IERPProductCategoryLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProductCategoryLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProductCategoryLineRepository iERPProductCategoryLineRepository = (base.ERPProductCategoryLineRepository = new ERPProductCategoryLineRepository(base.ApiClientContext));
		using (iERPProductCategoryLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProductCategoryLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProductCategoryLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProductCategoryLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProductCategoryLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProductCategoryLine(Guid productCategoryLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProductCategoryLineRepository iERPProductCategoryLineRepository = (base.ERPProductCategoryLineRepository = new ERPProductCategoryLineRepository(base.ApiClientContext));
		using (iERPProductCategoryLineRepository)
		{
			if (!(await base.ERPProductCategoryLineRepository.DoesProductCategoryLineExist(productCategoryLineId)))
			{
				errorsList.Add($"ProductCategoryLine [{productCategoryLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProductCategoryLineDto>>> Process_GetAllProductCategoryLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProductCategoryLineDto> allProductCategoryLinesDto = new List<ERPProductCategoryLineDto>();
		ERPResponseMessageDto<IList<ERPProductCategoryLineDto>> result;
		try
		{
			IERPProductCategoryLineRepository iERPProductCategoryLineRepository = (base.ERPProductCategoryLineRepository = new ERPProductCategoryLineRepository(base.ApiClientContext));
			using (iERPProductCategoryLineRepository)
			{
				foreach (ERPProductCategoryLineInformationDto item2 in await base.ERPProductCategoryLineRepository.GetAllProductCategoryLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPProductCategoryLineDto item = new ERPProductCategoryLineDto
					{
						insCreatedBy = item2.insCreatedBy,
						insCreatedDate = item2.insCreatedDate,
						insDescription = item2.insDescription,
						insUniqueID = item2.insUniqueID,
						insImageFilePath = item2.insImageFilePath,
						insInactiveDate = item2.insInactiveDate,
						insInactive = item2.insInactive,
						insLevel = item2.insLevel,
						insParentLineID = item2.insParentLineID,
						insProductCategoryID = item2.insProductCategoryID,
						INSRowVersion = item2.INSRowVersion,
						insProductCategoryLineID = item2.insProductCategoryLineID,
						insStructureCode = item2.insStructureCode,
						insStructureID = item2.insStructureID,
						CustomFields = item2.CustomFields
					};
					allProductCategoryLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProductCategoryLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProductCategoryLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProductCategoryLinesDto,
				RecordCount = allProductCategoryLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProductCategoryLineDto>> Process_GetProductCategoryLine(Guid productCategoryLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProductCategoryLineDto productCategoryLineDto = null;
		ERPResponseMessageDto<ERPProductCategoryLineDto> result;
		try
		{
			IERPProductCategoryLineRepository iERPProductCategoryLineRepository = (base.ERPProductCategoryLineRepository = new ERPProductCategoryLineRepository(base.ApiClientContext));
			using (iERPProductCategoryLineRepository)
			{
				ERPProductCategoryLineInformationDto eRPProductCategoryLineInformationDto = await base.ERPProductCategoryLineRepository.GetProductCategoryLine(productCategoryLineId);
				productCategoryLineDto = new ERPProductCategoryLineDto
				{
					insCreatedBy = eRPProductCategoryLineInformationDto.insCreatedBy,
					insCreatedDate = eRPProductCategoryLineInformationDto.insCreatedDate,
					insDescription = eRPProductCategoryLineInformationDto.insDescription,
					insUniqueID = eRPProductCategoryLineInformationDto.insUniqueID,
					insImageFilePath = eRPProductCategoryLineInformationDto.insImageFilePath,
					insInactiveDate = eRPProductCategoryLineInformationDto.insInactiveDate,
					insInactive = eRPProductCategoryLineInformationDto.insInactive,
					insLevel = eRPProductCategoryLineInformationDto.insLevel,
					insParentLineID = eRPProductCategoryLineInformationDto.insParentLineID,
					insProductCategoryID = eRPProductCategoryLineInformationDto.insProductCategoryID,
					INSRowVersion = eRPProductCategoryLineInformationDto.INSRowVersion,
					insProductCategoryLineID = eRPProductCategoryLineInformationDto.insProductCategoryLineID,
					insStructureCode = eRPProductCategoryLineInformationDto.insStructureCode,
					insStructureID = eRPProductCategoryLineInformationDto.insStructureID,
					CustomFields = eRPProductCategoryLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProductCategoryLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProductCategoryLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = productCategoryLineDto
			};
		}
		return result;
	}
}
