using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLFiscalYearBudgetHeaderModel : ERPBaseModel, IERPGLFiscalYearBudgetHeaderModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearBudgetHeaders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetHeaderRepository iERPGLFiscalYearBudgetHeaderRepository = (base.ERPGLFiscalYearBudgetHeaderRepository = new ERPGLFiscalYearBudgetHeaderRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetHeaderRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLFiscalYearBudgetHeaderRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLFiscalYearBudgetHeaderRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLFiscalYearBudgetHeaderRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLFiscalYearBudgetHeaderRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearBudgetHeader(Guid gLFiscalYearBudgetHeaderId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetHeaderRepository iERPGLFiscalYearBudgetHeaderRepository = (base.ERPGLFiscalYearBudgetHeaderRepository = new ERPGLFiscalYearBudgetHeaderRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetHeaderRepository)
		{
			if (!(await base.ERPGLFiscalYearBudgetHeaderRepository.DoesGLFiscalYearBudgetHeaderExist(gLFiscalYearBudgetHeaderId)))
			{
				errorsList.Add($"GLFiscalYearBudgetHeader [{gLFiscalYearBudgetHeaderId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYearBudgetHeader(ERPGLFiscalYearBudgetHeaderDto gLFiscalYearBudgetHeader)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetHeaderRepository iERPGLFiscalYearBudgetHeaderRepository = (base.ERPGLFiscalYearBudgetHeaderRepository = new ERPGLFiscalYearBudgetHeaderRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetHeaderRepository)
		{
			if (gLFiscalYearBudgetHeader.glkGlFiscalYearID > 0 && !(await base.ERPGLFiscalYearBudgetHeaderRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { gLFiscalYearBudgetHeader.glkGlFiscalYearID })))
			{
				errorsList.Add($"glkGlFiscalYearID [{gLFiscalYearBudgetHeader.glkGlFiscalYearID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLFiscalYearBudgetHeader.glkGlAccountID) && !(await base.ERPGLFiscalYearBudgetHeaderRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { gLFiscalYearBudgetHeader.glkGlAccountID })))
			{
				errorsList.Add("glkGlAccountID [" + gLFiscalYearBudgetHeader.glkGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetHeaderDto>>> Process_GetAllGLFiscalYearBudgetHeaders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLFiscalYearBudgetHeaderDto> allGLFiscalYearBudgetHeadersDto = new List<ERPGLFiscalYearBudgetHeaderDto>();
		ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetHeaderDto>> result;
		try
		{
			IERPGLFiscalYearBudgetHeaderRepository iERPGLFiscalYearBudgetHeaderRepository = (base.ERPGLFiscalYearBudgetHeaderRepository = new ERPGLFiscalYearBudgetHeaderRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetHeaderRepository)
			{
				foreach (ERPGLFiscalYearBudgetHeaderInformationDto item2 in await base.ERPGLFiscalYearBudgetHeaderRepository.GetAllGLFiscalYearBudgetHeaders(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLFiscalYearBudgetHeaderDto item = new ERPGLFiscalYearBudgetHeaderDto
					{
						glkAnnualAmount = item2.glkAnnualAmount,
						glkBudgetHeaderID = item2.glkBudgetHeaderID,
						glkCreatedBy = item2.glkCreatedBy,
						glkCreatedDate = item2.glkCreatedDate,
						glkUniqueID = item2.glkUniqueID,
						glkGlAccountID = item2.glkGlAccountID,
						glkGlFiscalYearID = item2.glkGlFiscalYearID,
						glkRowVersion = item2.glkRowVersion,
						CustomFields = item2.CustomFields
					};
					allGLFiscalYearBudgetHeadersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLFiscalYearBudgetHeaders]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetHeaderDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLFiscalYearBudgetHeadersDto,
				RecordCount = allGLFiscalYearBudgetHeadersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto>> Process_GetGLFiscalYearBudgetHeader(Guid gLFiscalYearBudgetHeaderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLFiscalYearBudgetHeaderDto gLFiscalYearBudgetHeaderDto = null;
		ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto> result;
		try
		{
			IERPGLFiscalYearBudgetHeaderRepository iERPGLFiscalYearBudgetHeaderRepository = (base.ERPGLFiscalYearBudgetHeaderRepository = new ERPGLFiscalYearBudgetHeaderRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetHeaderRepository)
			{
				ERPGLFiscalYearBudgetHeaderInformationDto eRPGLFiscalYearBudgetHeaderInformationDto = await base.ERPGLFiscalYearBudgetHeaderRepository.GetGLFiscalYearBudgetHeader(gLFiscalYearBudgetHeaderId);
				gLFiscalYearBudgetHeaderDto = new ERPGLFiscalYearBudgetHeaderDto
				{
					glkAnnualAmount = eRPGLFiscalYearBudgetHeaderInformationDto.glkAnnualAmount,
					glkBudgetHeaderID = eRPGLFiscalYearBudgetHeaderInformationDto.glkBudgetHeaderID,
					glkCreatedBy = eRPGLFiscalYearBudgetHeaderInformationDto.glkCreatedBy,
					glkCreatedDate = eRPGLFiscalYearBudgetHeaderInformationDto.glkCreatedDate,
					glkUniqueID = eRPGLFiscalYearBudgetHeaderInformationDto.glkUniqueID,
					glkGlAccountID = eRPGLFiscalYearBudgetHeaderInformationDto.glkGlAccountID,
					glkGlFiscalYearID = eRPGLFiscalYearBudgetHeaderInformationDto.glkGlFiscalYearID,
					glkRowVersion = eRPGLFiscalYearBudgetHeaderInformationDto.glkRowVersion,
					CustomFields = eRPGLFiscalYearBudgetHeaderInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLFiscalYearBudgetHeaders []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLFiscalYearBudgetHeaderDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto>> Process_PutGLFiscalYearBudgetHeader(ERPGLFiscalYearBudgetHeaderDto gLFiscalYearBudgetHeader)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLFiscalYearBudgetHeaderDto createdObject = null;
		ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto> result;
		try
		{
			IERPGLFiscalYearBudgetHeaderRepository iERPGLFiscalYearBudgetHeaderRepository = (base.ERPGLFiscalYearBudgetHeaderRepository = new ERPGLFiscalYearBudgetHeaderRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetHeaderRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLFiscalYearBudgetHeaderRepository.SaveGLFiscalYearBudgetHeader(gLFiscalYearBudgetHeader);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLFiscalYearBudgetHeaderInformationDto eRPGLFiscalYearBudgetHeaderInformationDto = await base.ERPGLFiscalYearBudgetHeaderRepository.GetGLFiscalYearBudgetHeader(gLFiscalYearBudgetHeader.glkUniqueID);
					createdObject = new ERPGLFiscalYearBudgetHeaderDto
					{
						glkAnnualAmount = eRPGLFiscalYearBudgetHeaderInformationDto.glkAnnualAmount,
						glkBudgetHeaderID = eRPGLFiscalYearBudgetHeaderInformationDto.glkBudgetHeaderID,
						glkCreatedBy = eRPGLFiscalYearBudgetHeaderInformationDto.glkCreatedBy,
						glkCreatedDate = eRPGLFiscalYearBudgetHeaderInformationDto.glkCreatedDate,
						glkUniqueID = eRPGLFiscalYearBudgetHeaderInformationDto.glkUniqueID,
						glkGlAccountID = eRPGLFiscalYearBudgetHeaderInformationDto.glkGlAccountID,
						glkGlFiscalYearID = eRPGLFiscalYearBudgetHeaderInformationDto.glkGlFiscalYearID,
						glkRowVersion = eRPGLFiscalYearBudgetHeaderInformationDto.glkRowVersion,
						CustomFields = eRPGLFiscalYearBudgetHeaderInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLFiscalYearBudgetHeader [{gLFiscalYearBudgetHeader.glkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYearBudgetHeader(Guid gLFiscalYearBudgetHeaderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetHeaderRepository iERPGLFiscalYearBudgetHeaderRepository = (base.ERPGLFiscalYearBudgetHeaderRepository = new ERPGLFiscalYearBudgetHeaderRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetHeaderRepository)
		{
			if (!(await base.ERPGLFiscalYearBudgetHeaderRepository.DoesGLFiscalYearBudgetHeaderExist(gLFiscalYearBudgetHeaderId)))
			{
				base.ErrorsList.Add($"GLFiscalYearBudgetHeader [{gLFiscalYearBudgetHeaderId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLFiscalYearBudgetHeaderInformationDto eRPGLFiscalYearBudgetHeaderInformationDto = await base.ERPGLFiscalYearBudgetHeaderRepository.GetGLFiscalYearBudgetHeader(gLFiscalYearBudgetHeaderId);
				string text = await base.ERPGLFiscalYearBudgetHeaderRepository.WhereUsed("GLFiscalYearBudgetHeaders", new object[2] { eRPGLFiscalYearBudgetHeaderInformationDto.glkGlFiscalYearID, eRPGLFiscalYearBudgetHeaderInformationDto.glkBudgetHeaderID }, new object[2] { "glkGlFiscalYearID", "glkBudgetHeaderID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLFiscalYearBudgetHeader cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto>> Process_DeleteGLFiscalYearBudgetHeader(Guid gLFiscalYearBudgetHeaderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto> result;
		try
		{
			IERPGLFiscalYearBudgetHeaderRepository iERPGLFiscalYearBudgetHeaderRepository = (base.ERPGLFiscalYearBudgetHeaderRepository = new ERPGLFiscalYearBudgetHeaderRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetHeaderRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLFiscalYearBudgetHeaderRepository.DeleteRowFromTable("GLFiscalYearBudgetHeaders", "glk", gLFiscalYearBudgetHeaderId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLFiscalYearBudgetHeader [{gLFiscalYearBudgetHeaderId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearBudgetHeaderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLFiscalYearBudgetHeaderDto()
			};
		}
		return result;
	}
}
