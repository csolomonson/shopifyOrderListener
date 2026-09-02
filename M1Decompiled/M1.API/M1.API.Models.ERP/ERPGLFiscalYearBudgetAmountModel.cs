using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLFiscalYearBudgetAmountModel : ERPBaseModel, IERPGLFiscalYearBudgetAmountModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearBudgetAmounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetAmountRepository iERPGLFiscalYearBudgetAmountRepository = (base.ERPGLFiscalYearBudgetAmountRepository = new ERPGLFiscalYearBudgetAmountRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetAmountRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLFiscalYearBudgetAmountRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLFiscalYearBudgetAmountRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLFiscalYearBudgetAmountRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLFiscalYearBudgetAmountRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetAmountRepository iERPGLFiscalYearBudgetAmountRepository = (base.ERPGLFiscalYearBudgetAmountRepository = new ERPGLFiscalYearBudgetAmountRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetAmountRepository)
		{
			if (!(await base.ERPGLFiscalYearBudgetAmountRepository.DoesGLFiscalYearBudgetAmountExist(gLFiscalYearBudgetAmountId)))
			{
				errorsList.Add($"GLFiscalYearBudgetAmount [{gLFiscalYearBudgetAmountId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYearBudgetAmount(ERPGLFiscalYearBudgetAmountDto gLFiscalYearBudgetAmount)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetAmountRepository iERPGLFiscalYearBudgetAmountRepository = (base.ERPGLFiscalYearBudgetAmountRepository = new ERPGLFiscalYearBudgetAmountRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetAmountRepository)
		{
			if (gLFiscalYearBudgetAmount.glbGlFiscalYearID > 0 && !(await base.ERPGLFiscalYearBudgetAmountRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { gLFiscalYearBudgetAmount.glbGlFiscalYearID })))
			{
				errorsList.Add($"glbGlFiscalYearID [{gLFiscalYearBudgetAmount.glbGlFiscalYearID}] not found.");
			}
			if (gLFiscalYearBudgetAmount.glbBudgetHeaderID > 0 && !(await base.ERPGLFiscalYearBudgetAmountRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearBudgetHeaders", new object[2] { "GLKGLFISCALYEARID", "GLKBUDGETHEADERID" }, new object[2] { gLFiscalYearBudgetAmount.glbGlFiscalYearID, gLFiscalYearBudgetAmount.glbBudgetHeaderID })))
			{
				errorsList.Add($"glbBudgetHeaderID [{gLFiscalYearBudgetAmount.glbBudgetHeaderID}] not found.");
			}
			if (gLFiscalYearBudgetAmount.glbBudgetLineID > 0 && !(await base.ERPGLFiscalYearBudgetAmountRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearBudgetLines", new object[3] { "GLGGLFISCALYEARID", "GLGBUDGETHEADERID", "GLGBUDGETLINEID" }, new object[3] { gLFiscalYearBudgetAmount.glbGlFiscalYearID, gLFiscalYearBudgetAmount.glbBudgetHeaderID, gLFiscalYearBudgetAmount.glbBudgetLineID })))
			{
				errorsList.Add($"glbBudgetLineID [{gLFiscalYearBudgetAmount.glbBudgetLineID}] not found.");
			}
			if (gLFiscalYearBudgetAmount.glbGlFiscalYearPeriodID > 0 && !(await base.ERPGLFiscalYearBudgetAmountRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { gLFiscalYearBudgetAmount.glbGlFiscalYearID, gLFiscalYearBudgetAmount.glbGlFiscalYearPeriodID })))
			{
				errorsList.Add($"glbGlFiscalYearPeriodID [{gLFiscalYearBudgetAmount.glbGlFiscalYearPeriodID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetAmountDto>>> Process_GetAllGLFiscalYearBudgetAmounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLFiscalYearBudgetAmountDto> allGLFiscalYearBudgetAmountsDto = new List<ERPGLFiscalYearBudgetAmountDto>();
		ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetAmountDto>> result;
		try
		{
			IERPGLFiscalYearBudgetAmountRepository iERPGLFiscalYearBudgetAmountRepository = (base.ERPGLFiscalYearBudgetAmountRepository = new ERPGLFiscalYearBudgetAmountRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetAmountRepository)
			{
				foreach (ERPGLFiscalYearBudgetAmountInformationDto item2 in await base.ERPGLFiscalYearBudgetAmountRepository.GetAllGLFiscalYearBudgetAmounts(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLFiscalYearBudgetAmountDto item = new ERPGLFiscalYearBudgetAmountDto
					{
						glbBudgetAmount = item2.glbBudgetAmount,
						glbBudgetHeaderID = item2.glbBudgetHeaderID,
						glbBudgetLineID = item2.glbBudgetLineID,
						glbCreatedBy = item2.glbCreatedBy,
						glbCreatedDate = item2.glbCreatedDate,
						glbUniqueID = item2.glbUniqueID,
						glbGlFiscalYearID = item2.glbGlFiscalYearID,
						glbGlFiscalYearPeriodID = item2.glbGlFiscalYearPeriodID,
						glbRowVersion = item2.glbRowVersion,
						CustomFields = item2.CustomFields
					};
					allGLFiscalYearBudgetAmountsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLFiscalYearBudgetAmounts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetAmountDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLFiscalYearBudgetAmountsDto,
				RecordCount = allGLFiscalYearBudgetAmountsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto>> Process_GetGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLFiscalYearBudgetAmountDto gLFiscalYearBudgetAmountDto = null;
		ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto> result;
		try
		{
			IERPGLFiscalYearBudgetAmountRepository iERPGLFiscalYearBudgetAmountRepository = (base.ERPGLFiscalYearBudgetAmountRepository = new ERPGLFiscalYearBudgetAmountRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetAmountRepository)
			{
				ERPGLFiscalYearBudgetAmountInformationDto eRPGLFiscalYearBudgetAmountInformationDto = await base.ERPGLFiscalYearBudgetAmountRepository.GetGLFiscalYearBudgetAmount(gLFiscalYearBudgetAmountId);
				gLFiscalYearBudgetAmountDto = new ERPGLFiscalYearBudgetAmountDto
				{
					glbBudgetAmount = eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetAmount,
					glbBudgetHeaderID = eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetHeaderID,
					glbBudgetLineID = eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetLineID,
					glbCreatedBy = eRPGLFiscalYearBudgetAmountInformationDto.glbCreatedBy,
					glbCreatedDate = eRPGLFiscalYearBudgetAmountInformationDto.glbCreatedDate,
					glbUniqueID = eRPGLFiscalYearBudgetAmountInformationDto.glbUniqueID,
					glbGlFiscalYearID = eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearID,
					glbGlFiscalYearPeriodID = eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearPeriodID,
					glbRowVersion = eRPGLFiscalYearBudgetAmountInformationDto.glbRowVersion,
					CustomFields = eRPGLFiscalYearBudgetAmountInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLFiscalYearBudgetAmounts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLFiscalYearBudgetAmountDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto>> Process_PutGLFiscalYearBudgetAmount(ERPGLFiscalYearBudgetAmountDto gLFiscalYearBudgetAmount)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLFiscalYearBudgetAmountDto createdObject = null;
		ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto> result;
		try
		{
			IERPGLFiscalYearBudgetAmountRepository iERPGLFiscalYearBudgetAmountRepository = (base.ERPGLFiscalYearBudgetAmountRepository = new ERPGLFiscalYearBudgetAmountRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetAmountRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLFiscalYearBudgetAmountRepository.SaveGLFiscalYearBudgetAmount(gLFiscalYearBudgetAmount);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLFiscalYearBudgetAmountInformationDto eRPGLFiscalYearBudgetAmountInformationDto = await base.ERPGLFiscalYearBudgetAmountRepository.GetGLFiscalYearBudgetAmount(gLFiscalYearBudgetAmount.glbUniqueID);
					createdObject = new ERPGLFiscalYearBudgetAmountDto
					{
						glbBudgetAmount = eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetAmount,
						glbBudgetHeaderID = eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetHeaderID,
						glbBudgetLineID = eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetLineID,
						glbCreatedBy = eRPGLFiscalYearBudgetAmountInformationDto.glbCreatedBy,
						glbCreatedDate = eRPGLFiscalYearBudgetAmountInformationDto.glbCreatedDate,
						glbUniqueID = eRPGLFiscalYearBudgetAmountInformationDto.glbUniqueID,
						glbGlFiscalYearID = eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearID,
						glbGlFiscalYearPeriodID = eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearPeriodID,
						glbRowVersion = eRPGLFiscalYearBudgetAmountInformationDto.glbRowVersion,
						CustomFields = eRPGLFiscalYearBudgetAmountInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLFiscalYearBudgetAmount [{gLFiscalYearBudgetAmount.glbUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetAmountRepository iERPGLFiscalYearBudgetAmountRepository = (base.ERPGLFiscalYearBudgetAmountRepository = new ERPGLFiscalYearBudgetAmountRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetAmountRepository)
		{
			if (!(await base.ERPGLFiscalYearBudgetAmountRepository.DoesGLFiscalYearBudgetAmountExist(gLFiscalYearBudgetAmountId)))
			{
				base.ErrorsList.Add($"GLFiscalYearBudgetAmount [{gLFiscalYearBudgetAmountId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLFiscalYearBudgetAmountInformationDto eRPGLFiscalYearBudgetAmountInformationDto = await base.ERPGLFiscalYearBudgetAmountRepository.GetGLFiscalYearBudgetAmount(gLFiscalYearBudgetAmountId);
				string text = await base.ERPGLFiscalYearBudgetAmountRepository.WhereUsed("GLFiscalYearBudgetAmounts", new object[4] { eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearID, eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetHeaderID, eRPGLFiscalYearBudgetAmountInformationDto.glbBudgetLineID, eRPGLFiscalYearBudgetAmountInformationDto.glbGlFiscalYearPeriodID }, new object[4] { "glbGlFiscalYearID", "glbBudgetHeaderID", "glbBudgetLineID", "glbGlFiscalYearPeriodID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLFiscalYearBudgetAmount cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto>> Process_DeleteGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto> result;
		try
		{
			IERPGLFiscalYearBudgetAmountRepository iERPGLFiscalYearBudgetAmountRepository = (base.ERPGLFiscalYearBudgetAmountRepository = new ERPGLFiscalYearBudgetAmountRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetAmountRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLFiscalYearBudgetAmountRepository.DeleteRowFromTable("GLFiscalYearBudgetAmounts", "glb", gLFiscalYearBudgetAmountId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLFiscalYearBudgetAmount [{gLFiscalYearBudgetAmountId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearBudgetAmountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLFiscalYearBudgetAmountDto()
			};
		}
		return result;
	}
}
