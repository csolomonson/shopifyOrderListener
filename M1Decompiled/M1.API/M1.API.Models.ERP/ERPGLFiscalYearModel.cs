using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLFiscalYearModel : ERPBaseModel, IERPGLFiscalYearModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYears(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLFiscalYearRepository iERPGLFiscalYearRepository = (base.ERPGLFiscalYearRepository = new ERPGLFiscalYearRepository(base.ApiClientContext));
		using (iERPGLFiscalYearRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLFiscalYearRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLFiscalYearRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLFiscalYearRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLFiscalYearRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYear(Guid gLFiscalYearId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearRepository iERPGLFiscalYearRepository = (base.ERPGLFiscalYearRepository = new ERPGLFiscalYearRepository(base.ApiClientContext));
		using (iERPGLFiscalYearRepository)
		{
			if (!(await base.ERPGLFiscalYearRepository.DoesGLFiscalYearExist(gLFiscalYearId)))
			{
				errorsList.Add($"GLFiscalYear [{gLFiscalYearId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYear(ERPGLFiscalYearDto gLFiscalYear)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLFiscalYearRepository iERPGLFiscalYearRepository = (base.ERPGLFiscalYearRepository = new ERPGLFiscalYearRepository(base.ApiClientContext));
		using (iERPGLFiscalYearRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLFiscalYearDto>>> Process_GetAllGLFiscalYears(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLFiscalYearDto> allGLFiscalYearsDto = new List<ERPGLFiscalYearDto>();
		ERPResponseMessageDto<IList<ERPGLFiscalYearDto>> result;
		try
		{
			IERPGLFiscalYearRepository iERPGLFiscalYearRepository = (base.ERPGLFiscalYearRepository = new ERPGLFiscalYearRepository(base.ApiClientContext));
			using (iERPGLFiscalYearRepository)
			{
				foreach (ERPGLFiscalYearInformationDto item2 in await base.ERPGLFiscalYearRepository.GetAllGLFiscalYears(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLFiscalYearDto item = new ERPGLFiscalYearDto
					{
						glzCreatedBy = item2.glzCreatedBy,
						glzCreatedDate = item2.glzCreatedDate,
						glzEndDate = item2.glzEndDate,
						glzUniqueID = item2.glzUniqueID,
						glzRowVersion = item2.glzRowVersion,
						glzGlFiscalYearID = item2.glzGlFiscalYearID,
						glzStartDate = item2.glzStartDate,
						CustomFields = item2.CustomFields
					};
					allGLFiscalYearsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLFiscalYears]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLFiscalYearDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLFiscalYearsDto,
				RecordCount = allGLFiscalYearsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearDto>> Process_GetGLFiscalYear(Guid gLFiscalYearId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLFiscalYearDto gLFiscalYearDto = null;
		ERPResponseMessageDto<ERPGLFiscalYearDto> result;
		try
		{
			IERPGLFiscalYearRepository iERPGLFiscalYearRepository = (base.ERPGLFiscalYearRepository = new ERPGLFiscalYearRepository(base.ApiClientContext));
			using (iERPGLFiscalYearRepository)
			{
				ERPGLFiscalYearInformationDto eRPGLFiscalYearInformationDto = await base.ERPGLFiscalYearRepository.GetGLFiscalYear(gLFiscalYearId);
				gLFiscalYearDto = new ERPGLFiscalYearDto
				{
					glzCreatedBy = eRPGLFiscalYearInformationDto.glzCreatedBy,
					glzCreatedDate = eRPGLFiscalYearInformationDto.glzCreatedDate,
					glzEndDate = eRPGLFiscalYearInformationDto.glzEndDate,
					glzUniqueID = eRPGLFiscalYearInformationDto.glzUniqueID,
					glzRowVersion = eRPGLFiscalYearInformationDto.glzRowVersion,
					glzGlFiscalYearID = eRPGLFiscalYearInformationDto.glzGlFiscalYearID,
					glzStartDate = eRPGLFiscalYearInformationDto.glzStartDate,
					CustomFields = eRPGLFiscalYearInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLFiscalYears []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLFiscalYearDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearDto>> Process_PutGLFiscalYear(ERPGLFiscalYearDto gLFiscalYear)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLFiscalYearDto createdObject = null;
		ERPResponseMessageDto<ERPGLFiscalYearDto> result;
		try
		{
			IERPGLFiscalYearRepository iERPGLFiscalYearRepository = (base.ERPGLFiscalYearRepository = new ERPGLFiscalYearRepository(base.ApiClientContext));
			using (iERPGLFiscalYearRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLFiscalYearRepository.SaveGLFiscalYear(gLFiscalYear);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLFiscalYearInformationDto eRPGLFiscalYearInformationDto = await base.ERPGLFiscalYearRepository.GetGLFiscalYear(gLFiscalYear.glzUniqueID);
					createdObject = new ERPGLFiscalYearDto
					{
						glzCreatedBy = eRPGLFiscalYearInformationDto.glzCreatedBy,
						glzCreatedDate = eRPGLFiscalYearInformationDto.glzCreatedDate,
						glzEndDate = eRPGLFiscalYearInformationDto.glzEndDate,
						glzUniqueID = eRPGLFiscalYearInformationDto.glzUniqueID,
						glzRowVersion = eRPGLFiscalYearInformationDto.glzRowVersion,
						glzGlFiscalYearID = eRPGLFiscalYearInformationDto.glzGlFiscalYearID,
						glzStartDate = eRPGLFiscalYearInformationDto.glzStartDate,
						CustomFields = eRPGLFiscalYearInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLFiscalYear [{gLFiscalYear.glzUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYear(Guid gLFiscalYearId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearRepository iERPGLFiscalYearRepository = (base.ERPGLFiscalYearRepository = new ERPGLFiscalYearRepository(base.ApiClientContext));
		using (iERPGLFiscalYearRepository)
		{
			if (!(await base.ERPGLFiscalYearRepository.DoesGLFiscalYearExist(gLFiscalYearId)))
			{
				base.ErrorsList.Add($"GLFiscalYear [{gLFiscalYearId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLFiscalYearInformationDto eRPGLFiscalYearInformationDto = await base.ERPGLFiscalYearRepository.GetGLFiscalYear(gLFiscalYearId);
				string text = await base.ERPGLFiscalYearRepository.WhereUsed("GLFiscalYears", new object[1] { eRPGLFiscalYearInformationDto.glzGlFiscalYearID }, new object[1] { "glzGlFiscalYearID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLFiscalYear cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearDto>> Process_DeleteGLFiscalYear(Guid gLFiscalYearId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLFiscalYearDto> result;
		try
		{
			IERPGLFiscalYearRepository iERPGLFiscalYearRepository = (base.ERPGLFiscalYearRepository = new ERPGLFiscalYearRepository(base.ApiClientContext));
			using (iERPGLFiscalYearRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLFiscalYearRepository.DeleteRowFromTable("GLFiscalYears", "glz", gLFiscalYearId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLFiscalYear [{gLFiscalYearId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLFiscalYearDto()
			};
		}
		return result;
	}
}
