using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLFiscalYearPeriodModel : ERPBaseModel, IERPGLFiscalYearPeriodModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearPeriods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLFiscalYearPeriodRepository iERPGLFiscalYearPeriodRepository = (base.ERPGLFiscalYearPeriodRepository = new ERPGLFiscalYearPeriodRepository(base.ApiClientContext));
		using (iERPGLFiscalYearPeriodRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLFiscalYearPeriodRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLFiscalYearPeriodRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLFiscalYearPeriodRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLFiscalYearPeriodRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearPeriod(Guid gLFiscalYearPeriodId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearPeriodRepository iERPGLFiscalYearPeriodRepository = (base.ERPGLFiscalYearPeriodRepository = new ERPGLFiscalYearPeriodRepository(base.ApiClientContext));
		using (iERPGLFiscalYearPeriodRepository)
		{
			if (!(await base.ERPGLFiscalYearPeriodRepository.DoesGLFiscalYearPeriodExist(gLFiscalYearPeriodId)))
			{
				errorsList.Add($"GLFiscalYearPeriod [{gLFiscalYearPeriodId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYearPeriod(ERPGLFiscalYearPeriodDto gLFiscalYearPeriod)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearPeriodRepository iERPGLFiscalYearPeriodRepository = (base.ERPGLFiscalYearPeriodRepository = new ERPGLFiscalYearPeriodRepository(base.ApiClientContext));
		using (iERPGLFiscalYearPeriodRepository)
		{
			if (gLFiscalYearPeriod.glfGlFiscalYearID > 0 && !(await base.ERPGLFiscalYearPeriodRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { gLFiscalYearPeriod.glfGlFiscalYearID })))
			{
				errorsList.Add($"glfGlFiscalYearID [{gLFiscalYearPeriod.glfGlFiscalYearID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLFiscalYearPeriodDto>>> Process_GetAllGLFiscalYearPeriods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLFiscalYearPeriodDto> allGLFiscalYearPeriodsDto = new List<ERPGLFiscalYearPeriodDto>();
		ERPResponseMessageDto<IList<ERPGLFiscalYearPeriodDto>> result;
		try
		{
			IERPGLFiscalYearPeriodRepository iERPGLFiscalYearPeriodRepository = (base.ERPGLFiscalYearPeriodRepository = new ERPGLFiscalYearPeriodRepository(base.ApiClientContext));
			using (iERPGLFiscalYearPeriodRepository)
			{
				foreach (ERPGLFiscalYearPeriodInformationDto item2 in await base.ERPGLFiscalYearPeriodRepository.GetAllGLFiscalYearPeriods(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLFiscalYearPeriodDto item = new ERPGLFiscalYearPeriodDto
					{
						glfCreatedBy = item2.glfCreatedBy,
						glfCreatedDate = item2.glfCreatedDate,
						glfEndDate = item2.glfEndDate,
						glfUniqueID = item2.glfUniqueID,
						glfGlFiscalYearID = item2.glfGlFiscalYearID,
						glfApClosed = item2.glfApClosed,
						glfArClosed = item2.glfArClosed,
						glfGlClosed = item2.glfGlClosed,
						glfRowVersion = item2.glfRowVersion,
						glfGlFiscalYearPeriodID = item2.glfGlFiscalYearPeriodID,
						glfStartDate = item2.glfStartDate,
						CustomFields = item2.CustomFields
					};
					allGLFiscalYearPeriodsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLFiscalYearPeriods]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLFiscalYearPeriodDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLFiscalYearPeriodsDto,
				RecordCount = allGLFiscalYearPeriodsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearPeriodDto>> Process_GetGLFiscalYearPeriod(Guid gLFiscalYearPeriodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLFiscalYearPeriodDto gLFiscalYearPeriodDto = null;
		ERPResponseMessageDto<ERPGLFiscalYearPeriodDto> result;
		try
		{
			IERPGLFiscalYearPeriodRepository iERPGLFiscalYearPeriodRepository = (base.ERPGLFiscalYearPeriodRepository = new ERPGLFiscalYearPeriodRepository(base.ApiClientContext));
			using (iERPGLFiscalYearPeriodRepository)
			{
				ERPGLFiscalYearPeriodInformationDto eRPGLFiscalYearPeriodInformationDto = await base.ERPGLFiscalYearPeriodRepository.GetGLFiscalYearPeriod(gLFiscalYearPeriodId);
				gLFiscalYearPeriodDto = new ERPGLFiscalYearPeriodDto
				{
					glfCreatedBy = eRPGLFiscalYearPeriodInformationDto.glfCreatedBy,
					glfCreatedDate = eRPGLFiscalYearPeriodInformationDto.glfCreatedDate,
					glfEndDate = eRPGLFiscalYearPeriodInformationDto.glfEndDate,
					glfUniqueID = eRPGLFiscalYearPeriodInformationDto.glfUniqueID,
					glfGlFiscalYearID = eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearID,
					glfApClosed = eRPGLFiscalYearPeriodInformationDto.glfApClosed,
					glfArClosed = eRPGLFiscalYearPeriodInformationDto.glfArClosed,
					glfGlClosed = eRPGLFiscalYearPeriodInformationDto.glfGlClosed,
					glfRowVersion = eRPGLFiscalYearPeriodInformationDto.glfRowVersion,
					glfGlFiscalYearPeriodID = eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearPeriodID,
					glfStartDate = eRPGLFiscalYearPeriodInformationDto.glfStartDate,
					CustomFields = eRPGLFiscalYearPeriodInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLFiscalYearPeriods []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearPeriodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLFiscalYearPeriodDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearPeriodDto>> Process_PutGLFiscalYearPeriod(ERPGLFiscalYearPeriodDto gLFiscalYearPeriod)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLFiscalYearPeriodDto createdObject = null;
		ERPResponseMessageDto<ERPGLFiscalYearPeriodDto> result;
		try
		{
			IERPGLFiscalYearPeriodRepository iERPGLFiscalYearPeriodRepository = (base.ERPGLFiscalYearPeriodRepository = new ERPGLFiscalYearPeriodRepository(base.ApiClientContext));
			using (iERPGLFiscalYearPeriodRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLFiscalYearPeriodRepository.SaveGLFiscalYearPeriod(gLFiscalYearPeriod);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLFiscalYearPeriodInformationDto eRPGLFiscalYearPeriodInformationDto = await base.ERPGLFiscalYearPeriodRepository.GetGLFiscalYearPeriod(gLFiscalYearPeriod.glfUniqueID);
					createdObject = new ERPGLFiscalYearPeriodDto
					{
						glfCreatedBy = eRPGLFiscalYearPeriodInformationDto.glfCreatedBy,
						glfCreatedDate = eRPGLFiscalYearPeriodInformationDto.glfCreatedDate,
						glfEndDate = eRPGLFiscalYearPeriodInformationDto.glfEndDate,
						glfUniqueID = eRPGLFiscalYearPeriodInformationDto.glfUniqueID,
						glfGlFiscalYearID = eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearID,
						glfApClosed = eRPGLFiscalYearPeriodInformationDto.glfApClosed,
						glfArClosed = eRPGLFiscalYearPeriodInformationDto.glfArClosed,
						glfGlClosed = eRPGLFiscalYearPeriodInformationDto.glfGlClosed,
						glfRowVersion = eRPGLFiscalYearPeriodInformationDto.glfRowVersion,
						glfGlFiscalYearPeriodID = eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearPeriodID,
						glfStartDate = eRPGLFiscalYearPeriodInformationDto.glfStartDate,
						CustomFields = eRPGLFiscalYearPeriodInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLFiscalYearPeriod [{gLFiscalYearPeriod.glfUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearPeriodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYearPeriod(Guid gLFiscalYearPeriodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearPeriodRepository iERPGLFiscalYearPeriodRepository = (base.ERPGLFiscalYearPeriodRepository = new ERPGLFiscalYearPeriodRepository(base.ApiClientContext));
		using (iERPGLFiscalYearPeriodRepository)
		{
			if (!(await base.ERPGLFiscalYearPeriodRepository.DoesGLFiscalYearPeriodExist(gLFiscalYearPeriodId)))
			{
				base.ErrorsList.Add($"GLFiscalYearPeriod [{gLFiscalYearPeriodId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLFiscalYearPeriodInformationDto eRPGLFiscalYearPeriodInformationDto = await base.ERPGLFiscalYearPeriodRepository.GetGLFiscalYearPeriod(gLFiscalYearPeriodId);
				string text = await base.ERPGLFiscalYearPeriodRepository.WhereUsed("GLFiscalYearPeriods", new object[2] { eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearID, eRPGLFiscalYearPeriodInformationDto.glfGlFiscalYearPeriodID }, new object[2] { "glfGlFiscalYearID", "glfGlFiscalYearPeriodID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLFiscalYearPeriod cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearPeriodDto>> Process_DeleteGLFiscalYearPeriod(Guid gLFiscalYearPeriodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLFiscalYearPeriodDto> result;
		try
		{
			IERPGLFiscalYearPeriodRepository iERPGLFiscalYearPeriodRepository = (base.ERPGLFiscalYearPeriodRepository = new ERPGLFiscalYearPeriodRepository(base.ApiClientContext));
			using (iERPGLFiscalYearPeriodRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLFiscalYearPeriodRepository.DeleteRowFromTable("GLFiscalYearPeriods", "glf", gLFiscalYearPeriodId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLFiscalYearPeriod [{gLFiscalYearPeriodId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearPeriodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLFiscalYearPeriodDto()
			};
		}
		return result;
	}
}
