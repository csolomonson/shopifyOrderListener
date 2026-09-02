using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPChangeRequestModel : ERPBaseModel, IERPChangeRequestModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllChangeRequests(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPChangeRequestRepository iERPChangeRequestRepository = (base.ERPChangeRequestRepository = new ERPChangeRequestRepository(base.ApiClientContext));
		using (iERPChangeRequestRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPChangeRequestRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPChangeRequestRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPChangeRequestRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPChangeRequestRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetChangeRequest(Guid changeRequestId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeRequestRepository iERPChangeRequestRepository = (base.ERPChangeRequestRepository = new ERPChangeRequestRepository(base.ApiClientContext));
		using (iERPChangeRequestRepository)
		{
			if (!(await base.ERPChangeRequestRepository.DoesChangeRequestExist(changeRequestId)))
			{
				errorsList.Add($"ChangeRequest [{changeRequestId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutChangeRequest(ERPChangeRequestDto changeRequest)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeRequestRepository iERPChangeRequestRepository = (base.ERPChangeRequestRepository = new ERPChangeRequestRepository(base.ApiClientContext));
		using (iERPChangeRequestRepository)
		{
			if (!string.IsNullOrWhiteSpace(changeRequest.chpChangeRequestTypeID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("ChangeRequestTypes", new object[1] { "CHTCHANGEREQUESTTYPEID" }, new object[1] { changeRequest.chpChangeRequestTypeID })))
			{
				errorsList.Add("chpChangeRequestTypeID [" + changeRequest.chpChangeRequestTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpJobID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { changeRequest.chpJobID })))
			{
				errorsList.Add("chpJobID [" + changeRequest.chpJobID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpPartID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { changeRequest.chpPartID })))
			{
				errorsList.Add("chpPartID [" + changeRequest.chpPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpPartRevisionID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { changeRequest.chpPartID, changeRequest.chpPartRevisionID })))
			{
				errorsList.Add("chpPartRevisionID [" + changeRequest.chpPartRevisionID + "] not found.");
			}
			if (changeRequest.chpPriorityID > 0 && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Priorities", new object[1] { "KBRPRIORITYID" }, new object[1] { changeRequest.chpPriorityID })))
			{
				errorsList.Add($"chpPriorityID [{changeRequest.chpPriorityID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpOpenedByEmployeeID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { changeRequest.chpOpenedByEmployeeID })))
			{
				errorsList.Add("chpOpenedByEmployeeID [" + changeRequest.chpOpenedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpAuthorizedByEmployeeID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { changeRequest.chpAuthorizedByEmployeeID })))
			{
				errorsList.Add("chpAuthorizedByEmployeeID [" + changeRequest.chpAuthorizedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpAssignedToEmployeeID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { changeRequest.chpAssignedToEmployeeID })))
			{
				errorsList.Add("chpAssignedToEmployeeID [" + changeRequest.chpAssignedToEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpClosedByEmployeeID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { changeRequest.chpClosedByEmployeeID })))
			{
				errorsList.Add("chpClosedByEmployeeID [" + changeRequest.chpClosedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpClosedReasonID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { changeRequest.chpClosedReasonID })))
			{
				errorsList.Add("chpClosedReasonID [" + changeRequest.chpClosedReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpResolvedPartID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { changeRequest.chpResolvedPartID })))
			{
				errorsList.Add("chpResolvedPartID [" + changeRequest.chpResolvedPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpResolvedPartRevisionID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { changeRequest.chpResolvedPartID, changeRequest.chpResolvedPartRevisionID })))
			{
				errorsList.Add("chpResolvedPartRevisionID [" + changeRequest.chpResolvedPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpProjectID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { changeRequest.chpProjectID })))
			{
				errorsList.Add("chpProjectID [" + changeRequest.chpProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpProjectAreaID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { changeRequest.chpProjectID, changeRequest.chpProjectAreaID })))
			{
				errorsList.Add("chpProjectAreaID [" + changeRequest.chpProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(changeRequest.chpNonConformanceID) && !(await base.ERPChangeRequestRepository.DoesRecordExistInTableUsingKeys("NonConformances", new object[1] { "QARNONCONFORMANCEID" }, new object[1] { changeRequest.chpNonConformanceID })))
			{
				errorsList.Add("chpNonConformanceID [" + changeRequest.chpNonConformanceID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPChangeRequestDto>>> Process_GetAllChangeRequests(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPChangeRequestDto> allChangeRequestsDto = new List<ERPChangeRequestDto>();
		ERPResponseMessageDto<IList<ERPChangeRequestDto>> result;
		try
		{
			IERPChangeRequestRepository iERPChangeRequestRepository = (base.ERPChangeRequestRepository = new ERPChangeRequestRepository(base.ApiClientContext));
			using (iERPChangeRequestRepository)
			{
				foreach (ERPChangeRequestInformationDto item2 in await base.ERPChangeRequestRepository.GetAllChangeRequests(pageSize, pageNumber, filter, orderBy))
				{
					ERPChangeRequestDto item = new ERPChangeRequestDto
					{
						chpActualHours = item2.chpActualHours,
						chpAssignedDate = item2.chpAssignedDate,
						chpAssignedToEmployeeID = item2.chpAssignedToEmployeeID,
						chpAuthorizedByEmployeeID = item2.chpAuthorizedByEmployeeID,
						chpAuthorizedDate = item2.chpAuthorizedDate,
						chpChangeRequestTypeID = item2.chpChangeRequestTypeID,
						chpClosedByEmployeeID = item2.chpClosedByEmployeeID,
						chpClosedDate = item2.chpClosedDate,
						chpClosedReasonID = item2.chpClosedReasonID,
						chpChangeRequestID = item2.chpChangeRequestID,
						chpCreatedBy = item2.chpCreatedBy,
						chpCreatedDate = item2.chpCreatedDate,
						chpDueDate = item2.chpDueDate,
						chpUniqueID = item2.chpUniqueID,
						chpEstimatedHours = item2.chpEstimatedHours,
						chpJobID = item2.chpJobID,
						chpLongDescriptionRtf = item2.chpLongDescriptionRtf,
						chpLongDescriptionText = item2.chpLongDescriptionText,
						chpNonConformanceID = item2.chpNonConformanceID,
						chpOpenedByEmployeeID = item2.chpOpenedByEmployeeID,
						chpOpenedDate = item2.chpOpenedDate,
						chpPartID = item2.chpPartID,
						chpPartRevisionID = item2.chpPartRevisionID,
						chpPriorityID = item2.chpPriorityID,
						chpProjectAreaID = item2.chpProjectAreaID,
						chpProjectID = item2.chpProjectID,
						chpResolvedPartID = item2.chpResolvedPartID,
						chpResolvedPartRevisionID = item2.chpResolvedPartRevisionID,
						chpRowVersion = item2.chpRowVersion,
						chpShortDescription = item2.chpShortDescription,
						chpStatus = item2.chpStatus,
						CustomFields = item2.CustomFields
					};
					allChangeRequestsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ChangeRequests]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPChangeRequestDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allChangeRequestsDto,
				RecordCount = allChangeRequestsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestDto>> Process_GetChangeRequest(Guid changeRequestId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPChangeRequestDto changeRequestDto = null;
		ERPResponseMessageDto<ERPChangeRequestDto> result;
		try
		{
			IERPChangeRequestRepository iERPChangeRequestRepository = (base.ERPChangeRequestRepository = new ERPChangeRequestRepository(base.ApiClientContext));
			using (iERPChangeRequestRepository)
			{
				ERPChangeRequestInformationDto eRPChangeRequestInformationDto = await base.ERPChangeRequestRepository.GetChangeRequest(changeRequestId);
				changeRequestDto = new ERPChangeRequestDto
				{
					chpActualHours = eRPChangeRequestInformationDto.chpActualHours,
					chpAssignedDate = eRPChangeRequestInformationDto.chpAssignedDate,
					chpAssignedToEmployeeID = eRPChangeRequestInformationDto.chpAssignedToEmployeeID,
					chpAuthorizedByEmployeeID = eRPChangeRequestInformationDto.chpAuthorizedByEmployeeID,
					chpAuthorizedDate = eRPChangeRequestInformationDto.chpAuthorizedDate,
					chpChangeRequestTypeID = eRPChangeRequestInformationDto.chpChangeRequestTypeID,
					chpClosedByEmployeeID = eRPChangeRequestInformationDto.chpClosedByEmployeeID,
					chpClosedDate = eRPChangeRequestInformationDto.chpClosedDate,
					chpClosedReasonID = eRPChangeRequestInformationDto.chpClosedReasonID,
					chpChangeRequestID = eRPChangeRequestInformationDto.chpChangeRequestID,
					chpCreatedBy = eRPChangeRequestInformationDto.chpCreatedBy,
					chpCreatedDate = eRPChangeRequestInformationDto.chpCreatedDate,
					chpDueDate = eRPChangeRequestInformationDto.chpDueDate,
					chpUniqueID = eRPChangeRequestInformationDto.chpUniqueID,
					chpEstimatedHours = eRPChangeRequestInformationDto.chpEstimatedHours,
					chpJobID = eRPChangeRequestInformationDto.chpJobID,
					chpLongDescriptionRtf = eRPChangeRequestInformationDto.chpLongDescriptionRtf,
					chpLongDescriptionText = eRPChangeRequestInformationDto.chpLongDescriptionText,
					chpNonConformanceID = eRPChangeRequestInformationDto.chpNonConformanceID,
					chpOpenedByEmployeeID = eRPChangeRequestInformationDto.chpOpenedByEmployeeID,
					chpOpenedDate = eRPChangeRequestInformationDto.chpOpenedDate,
					chpPartID = eRPChangeRequestInformationDto.chpPartID,
					chpPartRevisionID = eRPChangeRequestInformationDto.chpPartRevisionID,
					chpPriorityID = eRPChangeRequestInformationDto.chpPriorityID,
					chpProjectAreaID = eRPChangeRequestInformationDto.chpProjectAreaID,
					chpProjectID = eRPChangeRequestInformationDto.chpProjectID,
					chpResolvedPartID = eRPChangeRequestInformationDto.chpResolvedPartID,
					chpResolvedPartRevisionID = eRPChangeRequestInformationDto.chpResolvedPartRevisionID,
					chpRowVersion = eRPChangeRequestInformationDto.chpRowVersion,
					chpShortDescription = eRPChangeRequestInformationDto.chpShortDescription,
					chpStatus = eRPChangeRequestInformationDto.chpStatus,
					CustomFields = eRPChangeRequestInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ChangeRequests []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = changeRequestDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestDto>> Process_PutChangeRequest(ERPChangeRequestDto changeRequest)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPChangeRequestDto createdObject = null;
		ERPResponseMessageDto<ERPChangeRequestDto> result;
		try
		{
			IERPChangeRequestRepository iERPChangeRequestRepository = (base.ERPChangeRequestRepository = new ERPChangeRequestRepository(base.ApiClientContext));
			using (iERPChangeRequestRepository)
			{
				APIValidationInfoDto postResult = await base.ERPChangeRequestRepository.SaveChangeRequest(changeRequest);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPChangeRequestInformationDto eRPChangeRequestInformationDto = await base.ERPChangeRequestRepository.GetChangeRequest(changeRequest.chpUniqueID);
					createdObject = new ERPChangeRequestDto
					{
						chpActualHours = eRPChangeRequestInformationDto.chpActualHours,
						chpAssignedDate = eRPChangeRequestInformationDto.chpAssignedDate,
						chpAssignedToEmployeeID = eRPChangeRequestInformationDto.chpAssignedToEmployeeID,
						chpAuthorizedByEmployeeID = eRPChangeRequestInformationDto.chpAuthorizedByEmployeeID,
						chpAuthorizedDate = eRPChangeRequestInformationDto.chpAuthorizedDate,
						chpChangeRequestTypeID = eRPChangeRequestInformationDto.chpChangeRequestTypeID,
						chpClosedByEmployeeID = eRPChangeRequestInformationDto.chpClosedByEmployeeID,
						chpClosedDate = eRPChangeRequestInformationDto.chpClosedDate,
						chpClosedReasonID = eRPChangeRequestInformationDto.chpClosedReasonID,
						chpChangeRequestID = eRPChangeRequestInformationDto.chpChangeRequestID,
						chpCreatedBy = eRPChangeRequestInformationDto.chpCreatedBy,
						chpCreatedDate = eRPChangeRequestInformationDto.chpCreatedDate,
						chpDueDate = eRPChangeRequestInformationDto.chpDueDate,
						chpUniqueID = eRPChangeRequestInformationDto.chpUniqueID,
						chpEstimatedHours = eRPChangeRequestInformationDto.chpEstimatedHours,
						chpJobID = eRPChangeRequestInformationDto.chpJobID,
						chpLongDescriptionRtf = eRPChangeRequestInformationDto.chpLongDescriptionRtf,
						chpLongDescriptionText = eRPChangeRequestInformationDto.chpLongDescriptionText,
						chpNonConformanceID = eRPChangeRequestInformationDto.chpNonConformanceID,
						chpOpenedByEmployeeID = eRPChangeRequestInformationDto.chpOpenedByEmployeeID,
						chpOpenedDate = eRPChangeRequestInformationDto.chpOpenedDate,
						chpPartID = eRPChangeRequestInformationDto.chpPartID,
						chpPartRevisionID = eRPChangeRequestInformationDto.chpPartRevisionID,
						chpPriorityID = eRPChangeRequestInformationDto.chpPriorityID,
						chpProjectAreaID = eRPChangeRequestInformationDto.chpProjectAreaID,
						chpProjectID = eRPChangeRequestInformationDto.chpProjectID,
						chpResolvedPartID = eRPChangeRequestInformationDto.chpResolvedPartID,
						chpResolvedPartRevisionID = eRPChangeRequestInformationDto.chpResolvedPartRevisionID,
						chpRowVersion = eRPChangeRequestInformationDto.chpRowVersion,
						chpShortDescription = eRPChangeRequestInformationDto.chpShortDescription,
						chpStatus = eRPChangeRequestInformationDto.chpStatus,
						CustomFields = eRPChangeRequestInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ChangeRequest [{changeRequest.chpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteChangeRequest(Guid changeRequestId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeRequestRepository iERPChangeRequestRepository = (base.ERPChangeRequestRepository = new ERPChangeRequestRepository(base.ApiClientContext));
		using (iERPChangeRequestRepository)
		{
			if (!(await base.ERPChangeRequestRepository.DoesChangeRequestExist(changeRequestId)))
			{
				base.ErrorsList.Add($"ChangeRequest [{changeRequestId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPChangeRequestInformationDto eRPChangeRequestInformationDto = await base.ERPChangeRequestRepository.GetChangeRequest(changeRequestId);
				string text = await base.ERPChangeRequestRepository.WhereUsed("ChangeRequests", new object[1] { eRPChangeRequestInformationDto.chpChangeRequestID }, new object[1] { "chpChangeRequestID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ChangeRequest cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestDto>> Process_DeleteChangeRequest(Guid changeRequestId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPChangeRequestDto> result;
		try
		{
			IERPChangeRequestRepository iERPChangeRequestRepository = (base.ERPChangeRequestRepository = new ERPChangeRequestRepository(base.ApiClientContext));
			using (iERPChangeRequestRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPChangeRequestRepository.DeleteRowFromTable("ChangeRequests", "chp", changeRequestId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ChangeRequest [{changeRequestId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPChangeRequestDto()
			};
		}
		return result;
	}
}
