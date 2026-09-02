using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPInspectionLineApprovalModel : ERPBaseModel, IERPInspectionLineApprovalModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllInspectionLineApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPInspectionLineApprovalRepository iERPInspectionLineApprovalRepository = (base.ERPInspectionLineApprovalRepository = new ERPInspectionLineApprovalRepository(base.ApiClientContext));
		using (iERPInspectionLineApprovalRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPInspectionLineApprovalRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPInspectionLineApprovalRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPInspectionLineApprovalRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPInspectionLineApprovalRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetInspectionLineApproval(Guid inspectionLineApprovalId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionLineApprovalRepository iERPInspectionLineApprovalRepository = (base.ERPInspectionLineApprovalRepository = new ERPInspectionLineApprovalRepository(base.ApiClientContext));
		using (iERPInspectionLineApprovalRepository)
		{
			if (!(await base.ERPInspectionLineApprovalRepository.DoesInspectionLineApprovalExist(inspectionLineApprovalId)))
			{
				errorsList.Add($"InspectionLineApproval [{inspectionLineApprovalId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutInspectionLineApproval(ERPInspectionLineApprovalDto inspectionLineApproval)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionLineApprovalRepository iERPInspectionLineApprovalRepository = (base.ERPInspectionLineApprovalRepository = new ERPInspectionLineApprovalRepository(base.ApiClientContext));
		using (iERPInspectionLineApprovalRepository)
		{
			if (!string.IsNullOrWhiteSpace(inspectionLineApproval.qaaInspectionID) && !(await base.ERPInspectionLineApprovalRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { inspectionLineApproval.qaaInspectionID })))
			{
				errorsList.Add("qaaInspectionID [" + inspectionLineApproval.qaaInspectionID + "] not found.");
			}
			if (inspectionLineApproval.qaaInspectionLineID > 0 && !(await base.ERPInspectionLineApprovalRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { inspectionLineApproval.qaaInspectionID, inspectionLineApproval.qaaInspectionLineID })))
			{
				errorsList.Add($"qaaInspectionLineID [{inspectionLineApproval.qaaInspectionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLineApproval.qaaApprovalEmployeeID) && !(await base.ERPInspectionLineApprovalRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { inspectionLineApproval.qaaApprovalEmployeeID })))
			{
				errorsList.Add("qaaApprovalEmployeeID [" + inspectionLineApproval.qaaApprovalEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPInspectionLineApprovalDto>>> Process_GetAllInspectionLineApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPInspectionLineApprovalDto> allInspectionLineApprovalsDto = new List<ERPInspectionLineApprovalDto>();
		ERPResponseMessageDto<IList<ERPInspectionLineApprovalDto>> result;
		try
		{
			IERPInspectionLineApprovalRepository iERPInspectionLineApprovalRepository = (base.ERPInspectionLineApprovalRepository = new ERPInspectionLineApprovalRepository(base.ApiClientContext));
			using (iERPInspectionLineApprovalRepository)
			{
				foreach (ERPInspectionLineApprovalInformationDto item2 in await base.ERPInspectionLineApprovalRepository.GetAllInspectionLineApprovals(pageSize, pageNumber, filter, orderBy))
				{
					ERPInspectionLineApprovalDto item = new ERPInspectionLineApprovalDto
					{
						qaaApprovalEmployeeID = item2.qaaApprovalEmployeeID,
						qaaCreatedBy = item2.qaaCreatedBy,
						qaaCreatedDate = item2.qaaCreatedDate,
						qaaDescription = item2.qaaDescription,
						qaaUniqueID = item2.qaaUniqueID,
						qaaInspectionID = item2.qaaInspectionID,
						qaaInspectionLineID = item2.qaaInspectionLineID,
						qaaInspectionLineApprovalID = item2.qaaInspectionLineApprovalID,
						qaaStatus = item2.qaaStatus,
						qaaStatusDate = item2.qaaStatusDate,
						CustomFields = item2.CustomFields
					};
					allInspectionLineApprovalsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all InspectionLineApprovals]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPInspectionLineApprovalDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allInspectionLineApprovalsDto,
				RecordCount = allInspectionLineApprovalsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInspectionLineApprovalDto>> Process_GetInspectionLineApproval(Guid inspectionLineApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPInspectionLineApprovalDto inspectionLineApprovalDto = null;
		ERPResponseMessageDto<ERPInspectionLineApprovalDto> result;
		try
		{
			IERPInspectionLineApprovalRepository iERPInspectionLineApprovalRepository = (base.ERPInspectionLineApprovalRepository = new ERPInspectionLineApprovalRepository(base.ApiClientContext));
			using (iERPInspectionLineApprovalRepository)
			{
				ERPInspectionLineApprovalInformationDto eRPInspectionLineApprovalInformationDto = await base.ERPInspectionLineApprovalRepository.GetInspectionLineApproval(inspectionLineApprovalId);
				inspectionLineApprovalDto = new ERPInspectionLineApprovalDto
				{
					qaaApprovalEmployeeID = eRPInspectionLineApprovalInformationDto.qaaApprovalEmployeeID,
					qaaCreatedBy = eRPInspectionLineApprovalInformationDto.qaaCreatedBy,
					qaaCreatedDate = eRPInspectionLineApprovalInformationDto.qaaCreatedDate,
					qaaDescription = eRPInspectionLineApprovalInformationDto.qaaDescription,
					qaaUniqueID = eRPInspectionLineApprovalInformationDto.qaaUniqueID,
					qaaInspectionID = eRPInspectionLineApprovalInformationDto.qaaInspectionID,
					qaaInspectionLineID = eRPInspectionLineApprovalInformationDto.qaaInspectionLineID,
					qaaInspectionLineApprovalID = eRPInspectionLineApprovalInformationDto.qaaInspectionLineApprovalID,
					qaaStatus = eRPInspectionLineApprovalInformationDto.qaaStatus,
					qaaStatusDate = eRPInspectionLineApprovalInformationDto.qaaStatusDate,
					CustomFields = eRPInspectionLineApprovalInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the InspectionLineApprovals []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionLineApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = inspectionLineApprovalDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInspectionLineApprovalDto>> Process_PutInspectionLineApproval(ERPInspectionLineApprovalDto inspectionLineApproval)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPInspectionLineApprovalDto createdObject = null;
		ERPResponseMessageDto<ERPInspectionLineApprovalDto> result;
		try
		{
			IERPInspectionLineApprovalRepository iERPInspectionLineApprovalRepository = (base.ERPInspectionLineApprovalRepository = new ERPInspectionLineApprovalRepository(base.ApiClientContext));
			using (iERPInspectionLineApprovalRepository)
			{
				APIValidationInfoDto postResult = await base.ERPInspectionLineApprovalRepository.SaveInspectionLineApproval(inspectionLineApproval);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPInspectionLineApprovalInformationDto eRPInspectionLineApprovalInformationDto = await base.ERPInspectionLineApprovalRepository.GetInspectionLineApproval(inspectionLineApproval.qaaUniqueID);
					createdObject = new ERPInspectionLineApprovalDto
					{
						qaaApprovalEmployeeID = eRPInspectionLineApprovalInformationDto.qaaApprovalEmployeeID,
						qaaCreatedBy = eRPInspectionLineApprovalInformationDto.qaaCreatedBy,
						qaaCreatedDate = eRPInspectionLineApprovalInformationDto.qaaCreatedDate,
						qaaDescription = eRPInspectionLineApprovalInformationDto.qaaDescription,
						qaaUniqueID = eRPInspectionLineApprovalInformationDto.qaaUniqueID,
						qaaInspectionID = eRPInspectionLineApprovalInformationDto.qaaInspectionID,
						qaaInspectionLineID = eRPInspectionLineApprovalInformationDto.qaaInspectionLineID,
						qaaInspectionLineApprovalID = eRPInspectionLineApprovalInformationDto.qaaInspectionLineApprovalID,
						qaaStatus = eRPInspectionLineApprovalInformationDto.qaaStatus,
						qaaStatusDate = eRPInspectionLineApprovalInformationDto.qaaStatusDate,
						CustomFields = eRPInspectionLineApprovalInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing InspectionLineApproval [{inspectionLineApproval.qaaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionLineApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteInspectionLineApproval(Guid inspectionLineApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionLineApprovalRepository iERPInspectionLineApprovalRepository = (base.ERPInspectionLineApprovalRepository = new ERPInspectionLineApprovalRepository(base.ApiClientContext));
		using (iERPInspectionLineApprovalRepository)
		{
			if (!(await base.ERPInspectionLineApprovalRepository.DoesInspectionLineApprovalExist(inspectionLineApprovalId)))
			{
				base.ErrorsList.Add($"InspectionLineApproval [{inspectionLineApprovalId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPInspectionLineApprovalInformationDto eRPInspectionLineApprovalInformationDto = await base.ERPInspectionLineApprovalRepository.GetInspectionLineApproval(inspectionLineApprovalId);
				string text = await base.ERPInspectionLineApprovalRepository.WhereUsed("InspectionLineApprovals", new object[4] { eRPInspectionLineApprovalInformationDto.qaaInspectionID, eRPInspectionLineApprovalInformationDto.qaaInspectionLineID, eRPInspectionLineApprovalInformationDto.qaaApprovalEmployeeID, eRPInspectionLineApprovalInformationDto.qaaInspectionLineApprovalID }, new object[4] { "qaaInspectionID", "qaaInspectionLineID", "qaaApprovalEmployeeID", "qaaInspectionLineApprovalID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("InspectionLineApproval cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPInspectionLineApprovalDto>> Process_DeleteInspectionLineApproval(Guid inspectionLineApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPInspectionLineApprovalDto> result;
		try
		{
			IERPInspectionLineApprovalRepository iERPInspectionLineApprovalRepository = (base.ERPInspectionLineApprovalRepository = new ERPInspectionLineApprovalRepository(base.ApiClientContext));
			using (iERPInspectionLineApprovalRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPInspectionLineApprovalRepository.DeleteRowFromTable("InspectionLineApprovals", "qaa", inspectionLineApprovalId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of InspectionLineApproval [{inspectionLineApprovalId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionLineApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPInspectionLineApprovalDto()
			};
		}
		return result;
	}
}
