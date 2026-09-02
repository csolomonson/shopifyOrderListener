using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLFiscalYearBudgetLineModel : ERPBaseModel, IERPGLFiscalYearBudgetLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearBudgetLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetLineRepository iERPGLFiscalYearBudgetLineRepository = (base.ERPGLFiscalYearBudgetLineRepository = new ERPGLFiscalYearBudgetLineRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLFiscalYearBudgetLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLFiscalYearBudgetLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLFiscalYearBudgetLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLFiscalYearBudgetLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetLineRepository iERPGLFiscalYearBudgetLineRepository = (base.ERPGLFiscalYearBudgetLineRepository = new ERPGLFiscalYearBudgetLineRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetLineRepository)
		{
			if (!(await base.ERPGLFiscalYearBudgetLineRepository.DoesGLFiscalYearBudgetLineExist(gLFiscalYearBudgetLineId)))
			{
				errorsList.Add($"GLFiscalYearBudgetLine [{gLFiscalYearBudgetLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLFiscalYearBudgetLine(ERPGLFiscalYearBudgetLineDto gLFiscalYearBudgetLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetLineRepository iERPGLFiscalYearBudgetLineRepository = (base.ERPGLFiscalYearBudgetLineRepository = new ERPGLFiscalYearBudgetLineRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetLineRepository)
		{
			if (gLFiscalYearBudgetLine.glgGlFiscalYearID > 0 && !(await base.ERPGLFiscalYearBudgetLineRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { gLFiscalYearBudgetLine.glgGlFiscalYearID })))
			{
				errorsList.Add($"glgGlFiscalYearID [{gLFiscalYearBudgetLine.glgGlFiscalYearID}] not found.");
			}
			if (gLFiscalYearBudgetLine.glgBudgetHeaderID > 0 && !(await base.ERPGLFiscalYearBudgetLineRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearBudgetHeaders", new object[2] { "GLKGLFISCALYEARID", "GLKBUDGETHEADERID" }, new object[2] { gLFiscalYearBudgetLine.glgGlFiscalYearID, gLFiscalYearBudgetLine.glgBudgetHeaderID })))
			{
				errorsList.Add($"glgBudgetHeaderID [{gLFiscalYearBudgetLine.glgBudgetHeaderID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetLineDto>>> Process_GetAllGLFiscalYearBudgetLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLFiscalYearBudgetLineDto> allGLFiscalYearBudgetLinesDto = new List<ERPGLFiscalYearBudgetLineDto>();
		ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetLineDto>> result;
		try
		{
			IERPGLFiscalYearBudgetLineRepository iERPGLFiscalYearBudgetLineRepository = (base.ERPGLFiscalYearBudgetLineRepository = new ERPGLFiscalYearBudgetLineRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetLineRepository)
			{
				foreach (ERPGLFiscalYearBudgetLineInformationDto item2 in await base.ERPGLFiscalYearBudgetLineRepository.GetAllGLFiscalYearBudgetLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLFiscalYearBudgetLineDto item = new ERPGLFiscalYearBudgetLineDto
					{
						glgAnnualAmount = item2.glgAnnualAmount,
						glgBudgetHeaderID = item2.glgBudgetHeaderID,
						glgBudgetLineID = item2.glgBudgetLineID,
						glgCreatedBy = item2.glgCreatedBy,
						glgCreatedDate = item2.glgCreatedDate,
						glgUniqueID = item2.glgUniqueID,
						glgGlFiscalYearID = item2.glgGlFiscalYearID,
						glgRowVersion = item2.glgRowVersion,
						CustomFields = item2.CustomFields
					};
					allGLFiscalYearBudgetLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLFiscalYearBudgetLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLFiscalYearBudgetLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLFiscalYearBudgetLinesDto,
				RecordCount = allGLFiscalYearBudgetLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto>> Process_GetGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLFiscalYearBudgetLineDto gLFiscalYearBudgetLineDto = null;
		ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto> result;
		try
		{
			IERPGLFiscalYearBudgetLineRepository iERPGLFiscalYearBudgetLineRepository = (base.ERPGLFiscalYearBudgetLineRepository = new ERPGLFiscalYearBudgetLineRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetLineRepository)
			{
				ERPGLFiscalYearBudgetLineInformationDto eRPGLFiscalYearBudgetLineInformationDto = await base.ERPGLFiscalYearBudgetLineRepository.GetGLFiscalYearBudgetLine(gLFiscalYearBudgetLineId);
				gLFiscalYearBudgetLineDto = new ERPGLFiscalYearBudgetLineDto
				{
					glgAnnualAmount = eRPGLFiscalYearBudgetLineInformationDto.glgAnnualAmount,
					glgBudgetHeaderID = eRPGLFiscalYearBudgetLineInformationDto.glgBudgetHeaderID,
					glgBudgetLineID = eRPGLFiscalYearBudgetLineInformationDto.glgBudgetLineID,
					glgCreatedBy = eRPGLFiscalYearBudgetLineInformationDto.glgCreatedBy,
					glgCreatedDate = eRPGLFiscalYearBudgetLineInformationDto.glgCreatedDate,
					glgUniqueID = eRPGLFiscalYearBudgetLineInformationDto.glgUniqueID,
					glgGlFiscalYearID = eRPGLFiscalYearBudgetLineInformationDto.glgGlFiscalYearID,
					glgRowVersion = eRPGLFiscalYearBudgetLineInformationDto.glgRowVersion,
					CustomFields = eRPGLFiscalYearBudgetLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLFiscalYearBudgetLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLFiscalYearBudgetLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto>> Process_PutGLFiscalYearBudgetLine(ERPGLFiscalYearBudgetLineDto gLFiscalYearBudgetLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLFiscalYearBudgetLineDto createdObject = null;
		ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto> result;
		try
		{
			IERPGLFiscalYearBudgetLineRepository iERPGLFiscalYearBudgetLineRepository = (base.ERPGLFiscalYearBudgetLineRepository = new ERPGLFiscalYearBudgetLineRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLFiscalYearBudgetLineRepository.SaveGLFiscalYearBudgetLine(gLFiscalYearBudgetLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLFiscalYearBudgetLineInformationDto eRPGLFiscalYearBudgetLineInformationDto = await base.ERPGLFiscalYearBudgetLineRepository.GetGLFiscalYearBudgetLine(gLFiscalYearBudgetLine.glgUniqueID);
					createdObject = new ERPGLFiscalYearBudgetLineDto
					{
						glgAnnualAmount = eRPGLFiscalYearBudgetLineInformationDto.glgAnnualAmount,
						glgBudgetHeaderID = eRPGLFiscalYearBudgetLineInformationDto.glgBudgetHeaderID,
						glgBudgetLineID = eRPGLFiscalYearBudgetLineInformationDto.glgBudgetLineID,
						glgCreatedBy = eRPGLFiscalYearBudgetLineInformationDto.glgCreatedBy,
						glgCreatedDate = eRPGLFiscalYearBudgetLineInformationDto.glgCreatedDate,
						glgUniqueID = eRPGLFiscalYearBudgetLineInformationDto.glgUniqueID,
						glgGlFiscalYearID = eRPGLFiscalYearBudgetLineInformationDto.glgGlFiscalYearID,
						glgRowVersion = eRPGLFiscalYearBudgetLineInformationDto.glgRowVersion,
						CustomFields = eRPGLFiscalYearBudgetLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLFiscalYearBudgetLine [{gLFiscalYearBudgetLine.glgUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearBudgetLineRepository iERPGLFiscalYearBudgetLineRepository = (base.ERPGLFiscalYearBudgetLineRepository = new ERPGLFiscalYearBudgetLineRepository(base.ApiClientContext));
		using (iERPGLFiscalYearBudgetLineRepository)
		{
			if (!(await base.ERPGLFiscalYearBudgetLineRepository.DoesGLFiscalYearBudgetLineExist(gLFiscalYearBudgetLineId)))
			{
				base.ErrorsList.Add($"GLFiscalYearBudgetLine [{gLFiscalYearBudgetLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLFiscalYearBudgetLineInformationDto eRPGLFiscalYearBudgetLineInformationDto = await base.ERPGLFiscalYearBudgetLineRepository.GetGLFiscalYearBudgetLine(gLFiscalYearBudgetLineId);
				string text = await base.ERPGLFiscalYearBudgetLineRepository.WhereUsed("GLFiscalYearBudgetLines", new object[3] { eRPGLFiscalYearBudgetLineInformationDto.glgGlFiscalYearID, eRPGLFiscalYearBudgetLineInformationDto.glgBudgetHeaderID, eRPGLFiscalYearBudgetLineInformationDto.glgBudgetLineID }, new object[3] { "glgGlFiscalYearID", "glgBudgetHeaderID", "glgBudgetLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLFiscalYearBudgetLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto>> Process_DeleteGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto> result;
		try
		{
			IERPGLFiscalYearBudgetLineRepository iERPGLFiscalYearBudgetLineRepository = (base.ERPGLFiscalYearBudgetLineRepository = new ERPGLFiscalYearBudgetLineRepository(base.ApiClientContext));
			using (iERPGLFiscalYearBudgetLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLFiscalYearBudgetLineRepository.DeleteRowFromTable("GLFiscalYearBudgetLines", "glg", gLFiscalYearBudgetLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLFiscalYearBudgetLine [{gLFiscalYearBudgetLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearBudgetLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLFiscalYearBudgetLineDto()
			};
		}
		return result;
	}
}
