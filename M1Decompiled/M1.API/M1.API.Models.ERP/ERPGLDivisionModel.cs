using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLDivisionModel : ERPBaseModel, IERPGLDivisionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLDivisions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLDivisionRepository iERPGLDivisionRepository = (base.ERPGLDivisionRepository = new ERPGLDivisionRepository(base.ApiClientContext));
		using (iERPGLDivisionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLDivisionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLDivisionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLDivisionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLDivisionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLDivision(Guid gLDivisionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLDivisionRepository iERPGLDivisionRepository = (base.ERPGLDivisionRepository = new ERPGLDivisionRepository(base.ApiClientContext));
		using (iERPGLDivisionRepository)
		{
			if (!(await base.ERPGLDivisionRepository.DoesGLDivisionExist(gLDivisionId)))
			{
				errorsList.Add($"GLDivision [{gLDivisionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLDivision(ERPGLDivisionDto gLDivision)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLDivisionRepository iERPGLDivisionRepository = (base.ERPGLDivisionRepository = new ERPGLDivisionRepository(base.ApiClientContext));
		using (iERPGLDivisionRepository)
		{
			if (!string.IsNullOrWhiteSpace(gLDivision.glvRetainedEarningsAccountID) && !(await base.ERPGLDivisionRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { gLDivision.glvRetainedEarningsAccountID })))
			{
				errorsList.Add("glvRetainedEarningsAccountID [" + gLDivision.glvRetainedEarningsAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLDivisionDto>>> Process_GetAllGLDivisions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLDivisionDto> allGLDivisionsDto = new List<ERPGLDivisionDto>();
		ERPResponseMessageDto<IList<ERPGLDivisionDto>> result;
		try
		{
			IERPGLDivisionRepository iERPGLDivisionRepository = (base.ERPGLDivisionRepository = new ERPGLDivisionRepository(base.ApiClientContext));
			using (iERPGLDivisionRepository)
			{
				foreach (ERPGLDivisionInformationDto item2 in await base.ERPGLDivisionRepository.GetAllGLDivisions(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLDivisionDto item = new ERPGLDivisionDto
					{
						glvGlDivisionID = item2.glvGlDivisionID,
						glvCreatedBy = item2.glvCreatedBy,
						glvCreatedDate = item2.glvCreatedDate,
						glvDescription = item2.glvDescription,
						glvUniqueID = item2.glvUniqueID,
						glvRetainedEarningsAccountID = item2.glvRetainedEarningsAccountID,
						glvRowVersion = item2.glvRowVersion,
						CustomFields = item2.CustomFields
					};
					allGLDivisionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLDivisions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLDivisionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLDivisionsDto,
				RecordCount = allGLDivisionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLDivisionDto>> Process_GetGLDivision(Guid gLDivisionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLDivisionDto gLDivisionDto = null;
		ERPResponseMessageDto<ERPGLDivisionDto> result;
		try
		{
			IERPGLDivisionRepository iERPGLDivisionRepository = (base.ERPGLDivisionRepository = new ERPGLDivisionRepository(base.ApiClientContext));
			using (iERPGLDivisionRepository)
			{
				ERPGLDivisionInformationDto eRPGLDivisionInformationDto = await base.ERPGLDivisionRepository.GetGLDivision(gLDivisionId);
				gLDivisionDto = new ERPGLDivisionDto
				{
					glvGlDivisionID = eRPGLDivisionInformationDto.glvGlDivisionID,
					glvCreatedBy = eRPGLDivisionInformationDto.glvCreatedBy,
					glvCreatedDate = eRPGLDivisionInformationDto.glvCreatedDate,
					glvDescription = eRPGLDivisionInformationDto.glvDescription,
					glvUniqueID = eRPGLDivisionInformationDto.glvUniqueID,
					glvRetainedEarningsAccountID = eRPGLDivisionInformationDto.glvRetainedEarningsAccountID,
					glvRowVersion = eRPGLDivisionInformationDto.glvRowVersion,
					CustomFields = eRPGLDivisionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLDivisions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLDivisionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLDivisionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLDivisionDto>> Process_PutGLDivision(ERPGLDivisionDto gLDivision)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLDivisionDto createdObject = null;
		ERPResponseMessageDto<ERPGLDivisionDto> result;
		try
		{
			IERPGLDivisionRepository iERPGLDivisionRepository = (base.ERPGLDivisionRepository = new ERPGLDivisionRepository(base.ApiClientContext));
			using (iERPGLDivisionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLDivisionRepository.SaveGLDivision(gLDivision);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLDivisionInformationDto eRPGLDivisionInformationDto = await base.ERPGLDivisionRepository.GetGLDivision(gLDivision.glvUniqueID);
					createdObject = new ERPGLDivisionDto
					{
						glvGlDivisionID = eRPGLDivisionInformationDto.glvGlDivisionID,
						glvCreatedBy = eRPGLDivisionInformationDto.glvCreatedBy,
						glvCreatedDate = eRPGLDivisionInformationDto.glvCreatedDate,
						glvDescription = eRPGLDivisionInformationDto.glvDescription,
						glvUniqueID = eRPGLDivisionInformationDto.glvUniqueID,
						glvRetainedEarningsAccountID = eRPGLDivisionInformationDto.glvRetainedEarningsAccountID,
						glvRowVersion = eRPGLDivisionInformationDto.glvRowVersion,
						CustomFields = eRPGLDivisionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLDivision [{gLDivision.glvUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLDivisionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLDivision(Guid gLDivisionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLDivisionRepository iERPGLDivisionRepository = (base.ERPGLDivisionRepository = new ERPGLDivisionRepository(base.ApiClientContext));
		using (iERPGLDivisionRepository)
		{
			if (!(await base.ERPGLDivisionRepository.DoesGLDivisionExist(gLDivisionId)))
			{
				base.ErrorsList.Add($"GLDivision [{gLDivisionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLDivisionInformationDto eRPGLDivisionInformationDto = await base.ERPGLDivisionRepository.GetGLDivision(gLDivisionId);
				string text = await base.ERPGLDivisionRepository.WhereUsed("GLDivisions", new object[1] { eRPGLDivisionInformationDto.glvGlDivisionID }, new object[1] { "glvGlDivisionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLDivision cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLDivisionDto>> Process_DeleteGLDivision(Guid gLDivisionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLDivisionDto> result;
		try
		{
			IERPGLDivisionRepository iERPGLDivisionRepository = (base.ERPGLDivisionRepository = new ERPGLDivisionRepository(base.ApiClientContext));
			using (iERPGLDivisionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLDivisionRepository.DeleteRowFromTable("GLDivisions", "glv", gLDivisionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLDivision [{gLDivisionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLDivisionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLDivisionDto()
			};
		}
		return result;
	}
}
