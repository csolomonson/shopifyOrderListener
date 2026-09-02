using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProjectAreaModel : ERPBaseModel, IERPProjectAreaModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProjectAreas(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProjectAreaRepository iERPProjectAreaRepository = (base.ERPProjectAreaRepository = new ERPProjectAreaRepository(base.ApiClientContext));
		using (iERPProjectAreaRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProjectAreaRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProjectAreaRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProjectAreaRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProjectAreaRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProjectArea(Guid projectAreaId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectAreaRepository iERPProjectAreaRepository = (base.ERPProjectAreaRepository = new ERPProjectAreaRepository(base.ApiClientContext));
		using (iERPProjectAreaRepository)
		{
			if (!(await base.ERPProjectAreaRepository.DoesProjectAreaExist(projectAreaId)))
			{
				errorsList.Add($"ProjectArea [{projectAreaId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutProjectArea(ERPProjectAreaDto projectArea)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectAreaRepository iERPProjectAreaRepository = (base.ERPProjectAreaRepository = new ERPProjectAreaRepository(base.ApiClientContext));
		using (iERPProjectAreaRepository)
		{
			if (!string.IsNullOrWhiteSpace(projectArea.praProjectID) && !(await base.ERPProjectAreaRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { projectArea.praProjectID })))
			{
				errorsList.Add("praProjectID [" + projectArea.praProjectID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProjectAreaDto>>> Process_GetAllProjectAreas(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProjectAreaDto> allProjectAreasDto = new List<ERPProjectAreaDto>();
		ERPResponseMessageDto<IList<ERPProjectAreaDto>> result;
		try
		{
			IERPProjectAreaRepository iERPProjectAreaRepository = (base.ERPProjectAreaRepository = new ERPProjectAreaRepository(base.ApiClientContext));
			using (iERPProjectAreaRepository)
			{
				foreach (ERPProjectAreaInformationDto item2 in await base.ERPProjectAreaRepository.GetAllProjectAreas(pageSize, pageNumber, filter, orderBy))
				{
					ERPProjectAreaDto item = new ERPProjectAreaDto
					{
						praProjectAreaID = item2.praProjectAreaID,
						praCreatedBy = item2.praCreatedBy,
						praCreatedDate = item2.praCreatedDate,
						praDescription = item2.praDescription,
						praUniqueID = item2.praUniqueID,
						praProjectID = item2.praProjectID,
						praRowVersion = item2.praRowVersion,
						CustomFields = item2.CustomFields
					};
					allProjectAreasDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProjectAreas]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProjectAreaDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProjectAreasDto,
				RecordCount = allProjectAreasDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectAreaDto>> Process_GetProjectArea(Guid projectAreaId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProjectAreaDto projectAreaDto = null;
		ERPResponseMessageDto<ERPProjectAreaDto> result;
		try
		{
			IERPProjectAreaRepository iERPProjectAreaRepository = (base.ERPProjectAreaRepository = new ERPProjectAreaRepository(base.ApiClientContext));
			using (iERPProjectAreaRepository)
			{
				ERPProjectAreaInformationDto eRPProjectAreaInformationDto = await base.ERPProjectAreaRepository.GetProjectArea(projectAreaId);
				projectAreaDto = new ERPProjectAreaDto
				{
					praProjectAreaID = eRPProjectAreaInformationDto.praProjectAreaID,
					praCreatedBy = eRPProjectAreaInformationDto.praCreatedBy,
					praCreatedDate = eRPProjectAreaInformationDto.praCreatedDate,
					praDescription = eRPProjectAreaInformationDto.praDescription,
					praUniqueID = eRPProjectAreaInformationDto.praUniqueID,
					praProjectID = eRPProjectAreaInformationDto.praProjectID,
					praRowVersion = eRPProjectAreaInformationDto.praRowVersion,
					CustomFields = eRPProjectAreaInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProjectAreas []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectAreaDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = projectAreaDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectAreaDto>> Process_PutProjectArea(ERPProjectAreaDto projectArea)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPProjectAreaDto createdObject = null;
		ERPResponseMessageDto<ERPProjectAreaDto> result;
		try
		{
			IERPProjectAreaRepository iERPProjectAreaRepository = (base.ERPProjectAreaRepository = new ERPProjectAreaRepository(base.ApiClientContext));
			using (iERPProjectAreaRepository)
			{
				APIValidationInfoDto postResult = await base.ERPProjectAreaRepository.SaveProjectArea(projectArea);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPProjectAreaInformationDto eRPProjectAreaInformationDto = await base.ERPProjectAreaRepository.GetProjectArea(projectArea.praUniqueID);
					createdObject = new ERPProjectAreaDto
					{
						praProjectAreaID = eRPProjectAreaInformationDto.praProjectAreaID,
						praCreatedBy = eRPProjectAreaInformationDto.praCreatedBy,
						praCreatedDate = eRPProjectAreaInformationDto.praCreatedDate,
						praDescription = eRPProjectAreaInformationDto.praDescription,
						praUniqueID = eRPProjectAreaInformationDto.praUniqueID,
						praProjectID = eRPProjectAreaInformationDto.praProjectID,
						praRowVersion = eRPProjectAreaInformationDto.praRowVersion,
						CustomFields = eRPProjectAreaInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ProjectArea [{projectArea.praUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectAreaDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteProjectArea(Guid projectAreaId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectAreaRepository iERPProjectAreaRepository = (base.ERPProjectAreaRepository = new ERPProjectAreaRepository(base.ApiClientContext));
		using (iERPProjectAreaRepository)
		{
			if (!(await base.ERPProjectAreaRepository.DoesProjectAreaExist(projectAreaId)))
			{
				base.ErrorsList.Add($"ProjectArea [{projectAreaId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPProjectAreaInformationDto eRPProjectAreaInformationDto = await base.ERPProjectAreaRepository.GetProjectArea(projectAreaId);
				string text = await base.ERPProjectAreaRepository.WhereUsed("ProjectAreas", new object[2] { eRPProjectAreaInformationDto.praProjectID, eRPProjectAreaInformationDto.praProjectAreaID }, new object[2] { "praProjectID", "praProjectAreaID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ProjectArea cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPProjectAreaDto>> Process_DeleteProjectArea(Guid projectAreaId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPProjectAreaDto> result;
		try
		{
			IERPProjectAreaRepository iERPProjectAreaRepository = (base.ERPProjectAreaRepository = new ERPProjectAreaRepository(base.ApiClientContext));
			using (iERPProjectAreaRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPProjectAreaRepository.DeleteRowFromTable("ProjectAreas", "pra", projectAreaId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ProjectArea [{projectAreaId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectAreaDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPProjectAreaDto()
			};
		}
		return result;
	}
}
