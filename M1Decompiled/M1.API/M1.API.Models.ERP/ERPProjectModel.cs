using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProjectModel : ERPBaseModel, IERPProjectModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProjects(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProjectRepository iERPProjectRepository = (base.ERPProjectRepository = new ERPProjectRepository(base.ApiClientContext));
		using (iERPProjectRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProjectRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProjectRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProjectRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProjectRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProject(Guid projectId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectRepository iERPProjectRepository = (base.ERPProjectRepository = new ERPProjectRepository(base.ApiClientContext));
		using (iERPProjectRepository)
		{
			if (!(await base.ERPProjectRepository.DoesProjectExist(projectId)))
			{
				errorsList.Add($"Project [{projectId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutProject(ERPProjectDto project)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectRepository iERPProjectRepository = (base.ERPProjectRepository = new ERPProjectRepository(base.ApiClientContext));
		using (iERPProjectRepository)
		{
			if (!string.IsNullOrWhiteSpace(project.prpOrganizationID) && !(await base.ERPProjectRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { project.prpOrganizationID })))
			{
				errorsList.Add("prpOrganizationID [" + project.prpOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(project.prpLocationID) && !(await base.ERPProjectRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { project.prpOrganizationID, project.prpLocationID })))
			{
				errorsList.Add("prpLocationID [" + project.prpLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(project.prpContactID) && !(await base.ERPProjectRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { project.prpOrganizationID, project.prpLocationID, project.prpContactID })))
			{
				errorsList.Add("prpContactID [" + project.prpContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(project.prpProjectManagerEmployeeID) && !(await base.ERPProjectRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { project.prpProjectManagerEmployeeID })))
			{
				errorsList.Add("prpProjectManagerEmployeeID [" + project.prpProjectManagerEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(project.prpProjectTypeID) && !(await base.ERPProjectRepository.DoesRecordExistInTableUsingKeys("ProjectTypes", new object[1] { "PRTPROJECTTYPEID" }, new object[1] { project.prpProjectTypeID })))
			{
				errorsList.Add("prpProjectTypeID [" + project.prpProjectTypeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProjectDto>>> Process_GetAllProjects(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProjectDto> allProjectsDto = new List<ERPProjectDto>();
		ERPResponseMessageDto<IList<ERPProjectDto>> result;
		try
		{
			IERPProjectRepository iERPProjectRepository = (base.ERPProjectRepository = new ERPProjectRepository(base.ApiClientContext));
			using (iERPProjectRepository)
			{
				foreach (ERPProjectInformationDto item2 in await base.ERPProjectRepository.GetAllProjects(pageSize, pageNumber, filter, orderBy))
				{
					ERPProjectDto item = new ERPProjectDto
					{
						prpClosedDate = item2.prpClosedDate,
						prpProjectID = item2.prpProjectID,
						prpContactID = item2.prpContactID,
						prpCreatedBy = item2.prpCreatedBy,
						prpCreatedDate = item2.prpCreatedDate,
						prpDueDate = item2.prpDueDate,
						prpUniqueID = item2.prpUniqueID,
						prpClosed = item2.prpClosed,
						prpLocationID = item2.prpLocationID,
						prpLongDescriptionRtf = item2.prpLongDescriptionRtf,
						prpLongDescriptionText = item2.prpLongDescriptionText,
						prpOrganizationID = item2.prpOrganizationID,
						prpProjectDate = item2.prpProjectDate,
						prpProjectManagerEmployeeID = item2.prpProjectManagerEmployeeID,
						prpProjectTypeID = item2.prpProjectTypeID,
						prpRowVersion = item2.prpRowVersion,
						prpShortDescription = item2.prpShortDescription,
						prpStatus = item2.prpStatus,
						CustomFields = item2.CustomFields
					};
					allProjectsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Projects]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProjectDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProjectsDto,
				RecordCount = allProjectsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectDto>> Process_GetProject(Guid projectId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProjectDto projectDto = null;
		ERPResponseMessageDto<ERPProjectDto> result;
		try
		{
			IERPProjectRepository iERPProjectRepository = (base.ERPProjectRepository = new ERPProjectRepository(base.ApiClientContext));
			using (iERPProjectRepository)
			{
				ERPProjectInformationDto eRPProjectInformationDto = await base.ERPProjectRepository.GetProject(projectId);
				projectDto = new ERPProjectDto
				{
					prpClosedDate = eRPProjectInformationDto.prpClosedDate,
					prpProjectID = eRPProjectInformationDto.prpProjectID,
					prpContactID = eRPProjectInformationDto.prpContactID,
					prpCreatedBy = eRPProjectInformationDto.prpCreatedBy,
					prpCreatedDate = eRPProjectInformationDto.prpCreatedDate,
					prpDueDate = eRPProjectInformationDto.prpDueDate,
					prpUniqueID = eRPProjectInformationDto.prpUniqueID,
					prpClosed = eRPProjectInformationDto.prpClosed,
					prpLocationID = eRPProjectInformationDto.prpLocationID,
					prpLongDescriptionRtf = eRPProjectInformationDto.prpLongDescriptionRtf,
					prpLongDescriptionText = eRPProjectInformationDto.prpLongDescriptionText,
					prpOrganizationID = eRPProjectInformationDto.prpOrganizationID,
					prpProjectDate = eRPProjectInformationDto.prpProjectDate,
					prpProjectManagerEmployeeID = eRPProjectInformationDto.prpProjectManagerEmployeeID,
					prpProjectTypeID = eRPProjectInformationDto.prpProjectTypeID,
					prpRowVersion = eRPProjectInformationDto.prpRowVersion,
					prpShortDescription = eRPProjectInformationDto.prpShortDescription,
					prpStatus = eRPProjectInformationDto.prpStatus,
					CustomFields = eRPProjectInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Projects []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = projectDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectDto>> Process_PutProject(ERPProjectDto project)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPProjectDto createdObject = null;
		ERPResponseMessageDto<ERPProjectDto> result;
		try
		{
			IERPProjectRepository iERPProjectRepository = (base.ERPProjectRepository = new ERPProjectRepository(base.ApiClientContext));
			using (iERPProjectRepository)
			{
				APIValidationInfoDto postResult = await base.ERPProjectRepository.SaveProject(project);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPProjectInformationDto eRPProjectInformationDto = await base.ERPProjectRepository.GetProject(project.prpUniqueID);
					createdObject = new ERPProjectDto
					{
						prpClosedDate = eRPProjectInformationDto.prpClosedDate,
						prpProjectID = eRPProjectInformationDto.prpProjectID,
						prpContactID = eRPProjectInformationDto.prpContactID,
						prpCreatedBy = eRPProjectInformationDto.prpCreatedBy,
						prpCreatedDate = eRPProjectInformationDto.prpCreatedDate,
						prpDueDate = eRPProjectInformationDto.prpDueDate,
						prpUniqueID = eRPProjectInformationDto.prpUniqueID,
						prpClosed = eRPProjectInformationDto.prpClosed,
						prpLocationID = eRPProjectInformationDto.prpLocationID,
						prpLongDescriptionRtf = eRPProjectInformationDto.prpLongDescriptionRtf,
						prpLongDescriptionText = eRPProjectInformationDto.prpLongDescriptionText,
						prpOrganizationID = eRPProjectInformationDto.prpOrganizationID,
						prpProjectDate = eRPProjectInformationDto.prpProjectDate,
						prpProjectManagerEmployeeID = eRPProjectInformationDto.prpProjectManagerEmployeeID,
						prpProjectTypeID = eRPProjectInformationDto.prpProjectTypeID,
						prpRowVersion = eRPProjectInformationDto.prpRowVersion,
						prpShortDescription = eRPProjectInformationDto.prpShortDescription,
						prpStatus = eRPProjectInformationDto.prpStatus,
						CustomFields = eRPProjectInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Project [{project.prpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteProject(Guid projectId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectRepository iERPProjectRepository = (base.ERPProjectRepository = new ERPProjectRepository(base.ApiClientContext));
		using (iERPProjectRepository)
		{
			if (!(await base.ERPProjectRepository.DoesProjectExist(projectId)))
			{
				base.ErrorsList.Add($"Project [{projectId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPProjectInformationDto eRPProjectInformationDto = await base.ERPProjectRepository.GetProject(projectId);
				string text = await base.ERPProjectRepository.WhereUsed("Projects", new object[1] { eRPProjectInformationDto.prpProjectID }, new object[1] { "prpProjectID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Project cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPProjectDto>> Process_DeleteProject(Guid projectId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPProjectDto> result;
		try
		{
			IERPProjectRepository iERPProjectRepository = (base.ERPProjectRepository = new ERPProjectRepository(base.ApiClientContext));
			using (iERPProjectRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPProjectRepository.DeleteRowFromTable("Projects", "prp", projectId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Project [{projectId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPProjectDto()
			};
		}
		return result;
	}
}
