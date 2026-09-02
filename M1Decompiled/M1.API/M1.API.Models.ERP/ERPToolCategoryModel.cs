using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPToolCategoryModel : ERPBaseModel, IERPToolCategoryModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllToolCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPToolCategoryRepository iERPToolCategoryRepository = (base.ERPToolCategoryRepository = new ERPToolCategoryRepository(base.ApiClientContext));
		using (iERPToolCategoryRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPToolCategoryRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPToolCategoryRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPToolCategoryRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPToolCategoryRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetToolCategory(Guid toolCategoryId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolCategoryRepository iERPToolCategoryRepository = (base.ERPToolCategoryRepository = new ERPToolCategoryRepository(base.ApiClientContext));
		using (iERPToolCategoryRepository)
		{
			if (!(await base.ERPToolCategoryRepository.DoesToolCategoryExist(toolCategoryId)))
			{
				errorsList.Add($"ToolCategory [{toolCategoryId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPToolCategoryDto>>> Process_GetAllToolCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPToolCategoryDto> allToolCategoriesDto = new List<ERPToolCategoryDto>();
		ERPResponseMessageDto<IList<ERPToolCategoryDto>> result;
		try
		{
			IERPToolCategoryRepository iERPToolCategoryRepository = (base.ERPToolCategoryRepository = new ERPToolCategoryRepository(base.ApiClientContext));
			using (iERPToolCategoryRepository)
			{
				foreach (ERPToolCategoryInformationDto item2 in await base.ERPToolCategoryRepository.GetAllToolCategories(pageSize, pageNumber, filter, orderBy))
				{
					ERPToolCategoryDto item = new ERPToolCategoryDto
					{
						xtcToolCategoryID = item2.xtcToolCategoryID,
						xtcCreatedBy = item2.xtcCreatedBy,
						xtcCreatedDate = item2.xtcCreatedDate,
						xtcDescription = item2.xtcDescription,
						xtcUniqueID = item2.xtcUniqueID,
						xtcInactiveDate = item2.xtcInactiveDate,
						xtcInactive = item2.xtcInactive,
						xtcRowVersion = item2.xtcRowVersion,
						CustomFields = item2.CustomFields
					};
					allToolCategoriesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ToolCategories]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPToolCategoryDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allToolCategoriesDto,
				RecordCount = allToolCategoriesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPToolCategoryDto>> Process_GetToolCategory(Guid toolCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPToolCategoryDto toolCategoryDto = null;
		ERPResponseMessageDto<ERPToolCategoryDto> result;
		try
		{
			IERPToolCategoryRepository iERPToolCategoryRepository = (base.ERPToolCategoryRepository = new ERPToolCategoryRepository(base.ApiClientContext));
			using (iERPToolCategoryRepository)
			{
				ERPToolCategoryInformationDto eRPToolCategoryInformationDto = await base.ERPToolCategoryRepository.GetToolCategory(toolCategoryId);
				toolCategoryDto = new ERPToolCategoryDto
				{
					xtcToolCategoryID = eRPToolCategoryInformationDto.xtcToolCategoryID,
					xtcCreatedBy = eRPToolCategoryInformationDto.xtcCreatedBy,
					xtcCreatedDate = eRPToolCategoryInformationDto.xtcCreatedDate,
					xtcDescription = eRPToolCategoryInformationDto.xtcDescription,
					xtcUniqueID = eRPToolCategoryInformationDto.xtcUniqueID,
					xtcInactiveDate = eRPToolCategoryInformationDto.xtcInactiveDate,
					xtcInactive = eRPToolCategoryInformationDto.xtcInactive,
					xtcRowVersion = eRPToolCategoryInformationDto.xtcRowVersion,
					CustomFields = eRPToolCategoryInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ToolCategories []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = toolCategoryDto
			};
		}
		return result;
	}
}
