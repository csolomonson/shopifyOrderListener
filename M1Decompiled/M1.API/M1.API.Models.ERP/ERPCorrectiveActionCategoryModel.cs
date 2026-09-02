using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCorrectiveActionCategoryModel : ERPBaseModel, IERPCorrectiveActionCategoryModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCorrectiveActionCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCorrectiveActionCategoryRepository iERPCorrectiveActionCategoryRepository = (base.ERPCorrectiveActionCategoryRepository = new ERPCorrectiveActionCategoryRepository(base.ApiClientContext));
		using (iERPCorrectiveActionCategoryRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCorrectiveActionCategoryRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCorrectiveActionCategoryRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCorrectiveActionCategoryRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCorrectiveActionCategoryRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCorrectiveActionCategory(Guid correctiveActionCategoryId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCorrectiveActionCategoryRepository iERPCorrectiveActionCategoryRepository = (base.ERPCorrectiveActionCategoryRepository = new ERPCorrectiveActionCategoryRepository(base.ApiClientContext));
		using (iERPCorrectiveActionCategoryRepository)
		{
			if (!(await base.ERPCorrectiveActionCategoryRepository.DoesCorrectiveActionCategoryExist(correctiveActionCategoryId)))
			{
				errorsList.Add($"CorrectiveActionCategory [{correctiveActionCategoryId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCorrectiveActionCategory(ERPCorrectiveActionCategoryDto correctiveActionCategory)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCorrectiveActionCategoryRepository iERPCorrectiveActionCategoryRepository = (base.ERPCorrectiveActionCategoryRepository = new ERPCorrectiveActionCategoryRepository(base.ApiClientContext));
		using (iERPCorrectiveActionCategoryRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCorrectiveActionCategoryDto>>> Process_GetAllCorrectiveActionCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCorrectiveActionCategoryDto> allCorrectiveActionCategoriesDto = new List<ERPCorrectiveActionCategoryDto>();
		ERPResponseMessageDto<IList<ERPCorrectiveActionCategoryDto>> result;
		try
		{
			IERPCorrectiveActionCategoryRepository iERPCorrectiveActionCategoryRepository = (base.ERPCorrectiveActionCategoryRepository = new ERPCorrectiveActionCategoryRepository(base.ApiClientContext));
			using (iERPCorrectiveActionCategoryRepository)
			{
				foreach (ERPCorrectiveActionCategoryInformationDto item2 in await base.ERPCorrectiveActionCategoryRepository.GetAllCorrectiveActionCategories(pageSize, pageNumber, filter, orderBy))
				{
					ERPCorrectiveActionCategoryDto item = new ERPCorrectiveActionCategoryDto
					{
						qatCorrectiveActionCategoryID = item2.qatCorrectiveActionCategoryID,
						qatCreatedBy = item2.qatCreatedBy,
						qatCreatedDate = item2.qatCreatedDate,
						qatDescription = item2.qatDescription,
						qatUniqueID = item2.qatUniqueID,
						qatRowVersion = item2.qatRowVersion,
						CustomFields = item2.CustomFields
					};
					allCorrectiveActionCategoriesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CorrectiveActionCategories]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCorrectiveActionCategoryDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCorrectiveActionCategoriesDto,
				RecordCount = allCorrectiveActionCategoriesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCorrectiveActionCategoryDto>> Process_GetCorrectiveActionCategory(Guid correctiveActionCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCorrectiveActionCategoryDto correctiveActionCategoryDto = null;
		ERPResponseMessageDto<ERPCorrectiveActionCategoryDto> result;
		try
		{
			IERPCorrectiveActionCategoryRepository iERPCorrectiveActionCategoryRepository = (base.ERPCorrectiveActionCategoryRepository = new ERPCorrectiveActionCategoryRepository(base.ApiClientContext));
			using (iERPCorrectiveActionCategoryRepository)
			{
				ERPCorrectiveActionCategoryInformationDto eRPCorrectiveActionCategoryInformationDto = await base.ERPCorrectiveActionCategoryRepository.GetCorrectiveActionCategory(correctiveActionCategoryId);
				correctiveActionCategoryDto = new ERPCorrectiveActionCategoryDto
				{
					qatCorrectiveActionCategoryID = eRPCorrectiveActionCategoryInformationDto.qatCorrectiveActionCategoryID,
					qatCreatedBy = eRPCorrectiveActionCategoryInformationDto.qatCreatedBy,
					qatCreatedDate = eRPCorrectiveActionCategoryInformationDto.qatCreatedDate,
					qatDescription = eRPCorrectiveActionCategoryInformationDto.qatDescription,
					qatUniqueID = eRPCorrectiveActionCategoryInformationDto.qatUniqueID,
					qatRowVersion = eRPCorrectiveActionCategoryInformationDto.qatRowVersion,
					CustomFields = eRPCorrectiveActionCategoryInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CorrectiveActionCategories []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCorrectiveActionCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = correctiveActionCategoryDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCorrectiveActionCategoryDto>> Process_PutCorrectiveActionCategory(ERPCorrectiveActionCategoryDto correctiveActionCategory)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCorrectiveActionCategoryDto createdObject = null;
		ERPResponseMessageDto<ERPCorrectiveActionCategoryDto> result;
		try
		{
			IERPCorrectiveActionCategoryRepository iERPCorrectiveActionCategoryRepository = (base.ERPCorrectiveActionCategoryRepository = new ERPCorrectiveActionCategoryRepository(base.ApiClientContext));
			using (iERPCorrectiveActionCategoryRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCorrectiveActionCategoryRepository.SaveCorrectiveActionCategory(correctiveActionCategory);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCorrectiveActionCategoryInformationDto eRPCorrectiveActionCategoryInformationDto = await base.ERPCorrectiveActionCategoryRepository.GetCorrectiveActionCategory(correctiveActionCategory.qatUniqueID);
					createdObject = new ERPCorrectiveActionCategoryDto
					{
						qatCorrectiveActionCategoryID = eRPCorrectiveActionCategoryInformationDto.qatCorrectiveActionCategoryID,
						qatCreatedBy = eRPCorrectiveActionCategoryInformationDto.qatCreatedBy,
						qatCreatedDate = eRPCorrectiveActionCategoryInformationDto.qatCreatedDate,
						qatDescription = eRPCorrectiveActionCategoryInformationDto.qatDescription,
						qatUniqueID = eRPCorrectiveActionCategoryInformationDto.qatUniqueID,
						qatRowVersion = eRPCorrectiveActionCategoryInformationDto.qatRowVersion,
						CustomFields = eRPCorrectiveActionCategoryInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CorrectiveActionCategory [{correctiveActionCategory.qatUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCorrectiveActionCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCorrectiveActionCategory(Guid correctiveActionCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCorrectiveActionCategoryRepository iERPCorrectiveActionCategoryRepository = (base.ERPCorrectiveActionCategoryRepository = new ERPCorrectiveActionCategoryRepository(base.ApiClientContext));
		using (iERPCorrectiveActionCategoryRepository)
		{
			if (!(await base.ERPCorrectiveActionCategoryRepository.DoesCorrectiveActionCategoryExist(correctiveActionCategoryId)))
			{
				base.ErrorsList.Add($"CorrectiveActionCategory [{correctiveActionCategoryId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCorrectiveActionCategoryInformationDto eRPCorrectiveActionCategoryInformationDto = await base.ERPCorrectiveActionCategoryRepository.GetCorrectiveActionCategory(correctiveActionCategoryId);
				string text = await base.ERPCorrectiveActionCategoryRepository.WhereUsed("CorrectiveActionCategories", new object[1] { eRPCorrectiveActionCategoryInformationDto.qatCorrectiveActionCategoryID }, new object[1] { "qatCorrectiveActionCategoryID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CorrectiveActionCategory cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCorrectiveActionCategoryDto>> Process_DeleteCorrectiveActionCategory(Guid correctiveActionCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCorrectiveActionCategoryDto> result;
		try
		{
			IERPCorrectiveActionCategoryRepository iERPCorrectiveActionCategoryRepository = (base.ERPCorrectiveActionCategoryRepository = new ERPCorrectiveActionCategoryRepository(base.ApiClientContext));
			using (iERPCorrectiveActionCategoryRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCorrectiveActionCategoryRepository.DeleteRowFromTable("CorrectiveActionCategories", "qat", correctiveActionCategoryId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CorrectiveActionCategory [{correctiveActionCategoryId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCorrectiveActionCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCorrectiveActionCategoryDto()
			};
		}
		return result;
	}
}
