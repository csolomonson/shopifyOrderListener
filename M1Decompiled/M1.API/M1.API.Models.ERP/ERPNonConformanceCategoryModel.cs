using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPNonConformanceCategoryModel : ERPBaseModel, IERPNonConformanceCategoryModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllNonConformanceCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPNonConformanceCategoryRepository iERPNonConformanceCategoryRepository = (base.ERPNonConformanceCategoryRepository = new ERPNonConformanceCategoryRepository(base.ApiClientContext));
		using (iERPNonConformanceCategoryRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPNonConformanceCategoryRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPNonConformanceCategoryRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPNonConformanceCategoryRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPNonConformanceCategoryRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetNonConformanceCategory(Guid nonConformanceCategoryId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceCategoryRepository iERPNonConformanceCategoryRepository = (base.ERPNonConformanceCategoryRepository = new ERPNonConformanceCategoryRepository(base.ApiClientContext));
		using (iERPNonConformanceCategoryRepository)
		{
			if (!(await base.ERPNonConformanceCategoryRepository.DoesNonConformanceCategoryExist(nonConformanceCategoryId)))
			{
				errorsList.Add($"NonConformanceCategory [{nonConformanceCategoryId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutNonConformanceCategory(ERPNonConformanceCategoryDto nonConformanceCategory)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPNonConformanceCategoryRepository iERPNonConformanceCategoryRepository = (base.ERPNonConformanceCategoryRepository = new ERPNonConformanceCategoryRepository(base.ApiClientContext));
		using (iERPNonConformanceCategoryRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPNonConformanceCategoryDto>>> Process_GetAllNonConformanceCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPNonConformanceCategoryDto> allNonConformanceCategoriesDto = new List<ERPNonConformanceCategoryDto>();
		ERPResponseMessageDto<IList<ERPNonConformanceCategoryDto>> result;
		try
		{
			IERPNonConformanceCategoryRepository iERPNonConformanceCategoryRepository = (base.ERPNonConformanceCategoryRepository = new ERPNonConformanceCategoryRepository(base.ApiClientContext));
			using (iERPNonConformanceCategoryRepository)
			{
				foreach (ERPNonConformanceCategoryInformationDto item2 in await base.ERPNonConformanceCategoryRepository.GetAllNonConformanceCategories(pageSize, pageNumber, filter, orderBy))
				{
					ERPNonConformanceCategoryDto item = new ERPNonConformanceCategoryDto
					{
						qagNonConformanceCategoryID = item2.qagNonConformanceCategoryID,
						qagCreatedBy = item2.qagCreatedBy,
						qagCreatedDate = item2.qagCreatedDate,
						qagDescription = item2.qagDescription,
						qagUniqueID = item2.qagUniqueID,
						qagRowVersion = item2.qagRowVersion,
						CustomFields = item2.CustomFields
					};
					allNonConformanceCategoriesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all NonConformanceCategories]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPNonConformanceCategoryDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allNonConformanceCategoriesDto,
				RecordCount = allNonConformanceCategoriesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceCategoryDto>> Process_GetNonConformanceCategory(Guid nonConformanceCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPNonConformanceCategoryDto nonConformanceCategoryDto = null;
		ERPResponseMessageDto<ERPNonConformanceCategoryDto> result;
		try
		{
			IERPNonConformanceCategoryRepository iERPNonConformanceCategoryRepository = (base.ERPNonConformanceCategoryRepository = new ERPNonConformanceCategoryRepository(base.ApiClientContext));
			using (iERPNonConformanceCategoryRepository)
			{
				ERPNonConformanceCategoryInformationDto eRPNonConformanceCategoryInformationDto = await base.ERPNonConformanceCategoryRepository.GetNonConformanceCategory(nonConformanceCategoryId);
				nonConformanceCategoryDto = new ERPNonConformanceCategoryDto
				{
					qagNonConformanceCategoryID = eRPNonConformanceCategoryInformationDto.qagNonConformanceCategoryID,
					qagCreatedBy = eRPNonConformanceCategoryInformationDto.qagCreatedBy,
					qagCreatedDate = eRPNonConformanceCategoryInformationDto.qagCreatedDate,
					qagDescription = eRPNonConformanceCategoryInformationDto.qagDescription,
					qagUniqueID = eRPNonConformanceCategoryInformationDto.qagUniqueID,
					qagRowVersion = eRPNonConformanceCategoryInformationDto.qagRowVersion,
					CustomFields = eRPNonConformanceCategoryInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the NonConformanceCategories []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = nonConformanceCategoryDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceCategoryDto>> Process_PutNonConformanceCategory(ERPNonConformanceCategoryDto nonConformanceCategory)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPNonConformanceCategoryDto createdObject = null;
		ERPResponseMessageDto<ERPNonConformanceCategoryDto> result;
		try
		{
			IERPNonConformanceCategoryRepository iERPNonConformanceCategoryRepository = (base.ERPNonConformanceCategoryRepository = new ERPNonConformanceCategoryRepository(base.ApiClientContext));
			using (iERPNonConformanceCategoryRepository)
			{
				APIValidationInfoDto postResult = await base.ERPNonConformanceCategoryRepository.SaveNonConformanceCategory(nonConformanceCategory);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPNonConformanceCategoryInformationDto eRPNonConformanceCategoryInformationDto = await base.ERPNonConformanceCategoryRepository.GetNonConformanceCategory(nonConformanceCategory.qagUniqueID);
					createdObject = new ERPNonConformanceCategoryDto
					{
						qagNonConformanceCategoryID = eRPNonConformanceCategoryInformationDto.qagNonConformanceCategoryID,
						qagCreatedBy = eRPNonConformanceCategoryInformationDto.qagCreatedBy,
						qagCreatedDate = eRPNonConformanceCategoryInformationDto.qagCreatedDate,
						qagDescription = eRPNonConformanceCategoryInformationDto.qagDescription,
						qagUniqueID = eRPNonConformanceCategoryInformationDto.qagUniqueID,
						qagRowVersion = eRPNonConformanceCategoryInformationDto.qagRowVersion,
						CustomFields = eRPNonConformanceCategoryInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing NonConformanceCategory [{nonConformanceCategory.qagUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteNonConformanceCategory(Guid nonConformanceCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceCategoryRepository iERPNonConformanceCategoryRepository = (base.ERPNonConformanceCategoryRepository = new ERPNonConformanceCategoryRepository(base.ApiClientContext));
		using (iERPNonConformanceCategoryRepository)
		{
			if (!(await base.ERPNonConformanceCategoryRepository.DoesNonConformanceCategoryExist(nonConformanceCategoryId)))
			{
				base.ErrorsList.Add($"NonConformanceCategory [{nonConformanceCategoryId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPNonConformanceCategoryInformationDto eRPNonConformanceCategoryInformationDto = await base.ERPNonConformanceCategoryRepository.GetNonConformanceCategory(nonConformanceCategoryId);
				string text = await base.ERPNonConformanceCategoryRepository.WhereUsed("NonConformanceCategories", new object[1] { eRPNonConformanceCategoryInformationDto.qagNonConformanceCategoryID }, new object[1] { "qagNonConformanceCategoryID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("NonConformanceCategory cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceCategoryDto>> Process_DeleteNonConformanceCategory(Guid nonConformanceCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPNonConformanceCategoryDto> result;
		try
		{
			IERPNonConformanceCategoryRepository iERPNonConformanceCategoryRepository = (base.ERPNonConformanceCategoryRepository = new ERPNonConformanceCategoryRepository(base.ApiClientContext));
			using (iERPNonConformanceCategoryRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPNonConformanceCategoryRepository.DeleteRowFromTable("NonConformanceCategories", "qag", nonConformanceCategoryId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of NonConformanceCategory [{nonConformanceCategoryId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPNonConformanceCategoryDto()
			};
		}
		return result;
	}
}
