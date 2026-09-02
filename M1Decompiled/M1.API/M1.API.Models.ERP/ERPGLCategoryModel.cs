using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLCategoryModel : ERPBaseModel, IERPGLCategoryModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLCategoryRepository iERPGLCategoryRepository = (base.ERPGLCategoryRepository = new ERPGLCategoryRepository(base.ApiClientContext));
		using (iERPGLCategoryRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLCategoryRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLCategoryRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLCategoryRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLCategoryRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLCategory(Guid gLCategoryId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLCategoryRepository iERPGLCategoryRepository = (base.ERPGLCategoryRepository = new ERPGLCategoryRepository(base.ApiClientContext));
		using (iERPGLCategoryRepository)
		{
			if (!(await base.ERPGLCategoryRepository.DoesGLCategoryExist(gLCategoryId)))
			{
				errorsList.Add($"GLCategory [{gLCategoryId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLCategory(ERPGLCategoryDto gLCategory)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLCategoryRepository iERPGLCategoryRepository = (base.ERPGLCategoryRepository = new ERPGLCategoryRepository(base.ApiClientContext));
		using (iERPGLCategoryRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLCategoryDto>>> Process_GetAllGLCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLCategoryDto> allGLCategoriesDto = new List<ERPGLCategoryDto>();
		ERPResponseMessageDto<IList<ERPGLCategoryDto>> result;
		try
		{
			IERPGLCategoryRepository iERPGLCategoryRepository = (base.ERPGLCategoryRepository = new ERPGLCategoryRepository(base.ApiClientContext));
			using (iERPGLCategoryRepository)
			{
				foreach (ERPGLCategoryInformationDto item2 in await base.ERPGLCategoryRepository.GetAllGLCategories(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLCategoryDto item = new ERPGLCategoryDto
					{
						gltCategoryType = item2.gltCategoryType,
						gltGlCategoryID = item2.gltGlCategoryID,
						gltCreatedBy = item2.gltCreatedBy,
						gltCreatedDate = item2.gltCreatedDate,
						gltDescription = item2.gltDescription,
						gltUniqueID = item2.gltUniqueID,
						gltReportSequence = item2.gltReportSequence,
						gltRowVersion = item2.gltRowVersion,
						CustomFields = item2.CustomFields
					};
					allGLCategoriesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLCategories]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLCategoryDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLCategoriesDto,
				RecordCount = allGLCategoriesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLCategoryDto>> Process_GetGLCategory(Guid gLCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLCategoryDto gLCategoryDto = null;
		ERPResponseMessageDto<ERPGLCategoryDto> result;
		try
		{
			IERPGLCategoryRepository iERPGLCategoryRepository = (base.ERPGLCategoryRepository = new ERPGLCategoryRepository(base.ApiClientContext));
			using (iERPGLCategoryRepository)
			{
				ERPGLCategoryInformationDto eRPGLCategoryInformationDto = await base.ERPGLCategoryRepository.GetGLCategory(gLCategoryId);
				gLCategoryDto = new ERPGLCategoryDto
				{
					gltCategoryType = eRPGLCategoryInformationDto.gltCategoryType,
					gltGlCategoryID = eRPGLCategoryInformationDto.gltGlCategoryID,
					gltCreatedBy = eRPGLCategoryInformationDto.gltCreatedBy,
					gltCreatedDate = eRPGLCategoryInformationDto.gltCreatedDate,
					gltDescription = eRPGLCategoryInformationDto.gltDescription,
					gltUniqueID = eRPGLCategoryInformationDto.gltUniqueID,
					gltReportSequence = eRPGLCategoryInformationDto.gltReportSequence,
					gltRowVersion = eRPGLCategoryInformationDto.gltRowVersion,
					CustomFields = eRPGLCategoryInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLCategories []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLCategoryDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLCategoryDto>> Process_PutGLCategory(ERPGLCategoryDto gLCategory)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLCategoryDto createdObject = null;
		ERPResponseMessageDto<ERPGLCategoryDto> result;
		try
		{
			IERPGLCategoryRepository iERPGLCategoryRepository = (base.ERPGLCategoryRepository = new ERPGLCategoryRepository(base.ApiClientContext));
			using (iERPGLCategoryRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLCategoryRepository.SaveGLCategory(gLCategory);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLCategoryInformationDto eRPGLCategoryInformationDto = await base.ERPGLCategoryRepository.GetGLCategory(gLCategory.gltUniqueID);
					createdObject = new ERPGLCategoryDto
					{
						gltCategoryType = eRPGLCategoryInformationDto.gltCategoryType,
						gltGlCategoryID = eRPGLCategoryInformationDto.gltGlCategoryID,
						gltCreatedBy = eRPGLCategoryInformationDto.gltCreatedBy,
						gltCreatedDate = eRPGLCategoryInformationDto.gltCreatedDate,
						gltDescription = eRPGLCategoryInformationDto.gltDescription,
						gltUniqueID = eRPGLCategoryInformationDto.gltUniqueID,
						gltReportSequence = eRPGLCategoryInformationDto.gltReportSequence,
						gltRowVersion = eRPGLCategoryInformationDto.gltRowVersion,
						CustomFields = eRPGLCategoryInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLCategory [{gLCategory.gltUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLCategory(Guid gLCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLCategoryRepository iERPGLCategoryRepository = (base.ERPGLCategoryRepository = new ERPGLCategoryRepository(base.ApiClientContext));
		using (iERPGLCategoryRepository)
		{
			if (!(await base.ERPGLCategoryRepository.DoesGLCategoryExist(gLCategoryId)))
			{
				base.ErrorsList.Add($"GLCategory [{gLCategoryId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLCategoryInformationDto eRPGLCategoryInformationDto = await base.ERPGLCategoryRepository.GetGLCategory(gLCategoryId);
				string text = await base.ERPGLCategoryRepository.WhereUsed("GLCategories", new object[1] { eRPGLCategoryInformationDto.gltGlCategoryID }, new object[1] { "gltGlCategoryID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLCategory cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLCategoryDto>> Process_DeleteGLCategory(Guid gLCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLCategoryDto> result;
		try
		{
			IERPGLCategoryRepository iERPGLCategoryRepository = (base.ERPGLCategoryRepository = new ERPGLCategoryRepository(base.ApiClientContext));
			using (iERPGLCategoryRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLCategoryRepository.DeleteRowFromTable("GLCategories", "glt", gLCategoryId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLCategory [{gLCategoryId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLCategoryDto()
			};
		}
		return result;
	}
}
