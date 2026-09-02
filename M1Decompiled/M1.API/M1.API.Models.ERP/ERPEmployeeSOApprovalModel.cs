using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeeSOApprovalModel : ERPBaseModel, IERPEmployeeSOApprovalModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeSOApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeeSOApprovalRepository iERPEmployeeSOApprovalRepository = (base.ERPEmployeeSOApprovalRepository = new ERPEmployeeSOApprovalRepository(base.ApiClientContext));
		using (iERPEmployeeSOApprovalRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeeSOApprovalRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeeSOApprovalRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeeSOApprovalRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeeSOApprovalRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeeSOApproval(Guid employeeSOApprovalId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeSOApprovalRepository iERPEmployeeSOApprovalRepository = (base.ERPEmployeeSOApprovalRepository = new ERPEmployeeSOApprovalRepository(base.ApiClientContext));
		using (iERPEmployeeSOApprovalRepository)
		{
			if (!(await base.ERPEmployeeSOApprovalRepository.DoesEmployeeSOApprovalExist(employeeSOApprovalId)))
			{
				errorsList.Add($"EmployeeSOApproval [{employeeSOApprovalId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutEmployeeSOApproval(ERPEmployeeSOApprovalDto employeeSOApproval)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeSOApprovalRepository iERPEmployeeSOApprovalRepository = (base.ERPEmployeeSOApprovalRepository = new ERPEmployeeSOApprovalRepository(base.ApiClientContext));
		using (iERPEmployeeSOApprovalRepository)
		{
			if (!string.IsNullOrWhiteSpace(employeeSOApproval.lmoEmployeeID) && !(await base.ERPEmployeeSOApprovalRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { employeeSOApproval.lmoEmployeeID })))
			{
				errorsList.Add("lmoEmployeeID [" + employeeSOApproval.lmoEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employeeSOApproval.lmoApprovalEmployeeID) && !(await base.ERPEmployeeSOApprovalRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { employeeSOApproval.lmoApprovalEmployeeID })))
			{
				errorsList.Add("lmoApprovalEmployeeID [" + employeeSOApproval.lmoApprovalEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeeSOApprovalDto>>> Process_GetAllEmployeeSOApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeeSOApprovalDto> allEmployeeSOApprovalsDto = new List<ERPEmployeeSOApprovalDto>();
		ERPResponseMessageDto<IList<ERPEmployeeSOApprovalDto>> result;
		try
		{
			IERPEmployeeSOApprovalRepository iERPEmployeeSOApprovalRepository = (base.ERPEmployeeSOApprovalRepository = new ERPEmployeeSOApprovalRepository(base.ApiClientContext));
			using (iERPEmployeeSOApprovalRepository)
			{
				foreach (ERPEmployeeSOApprovalInformationDto item2 in await base.ERPEmployeeSOApprovalRepository.GetAllEmployeeSOApprovals(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeeSOApprovalDto item = new ERPEmployeeSOApprovalDto
					{
						lmoApprovalEmployeeID = item2.lmoApprovalEmployeeID,
						lmoCreatedBy = item2.lmoCreatedBy,
						lmoCreatedDate = item2.lmoCreatedDate,
						lmoEmployeeID = item2.lmoEmployeeID,
						lmoUniqueID = item2.lmoUniqueID,
						lmoRowVersion = item2.lmoRowVersion,
						lmoEmployeeSOApprovalID = item2.lmoEmployeeSOApprovalID,
						CustomFields = item2.CustomFields
					};
					allEmployeeSOApprovalsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeeSOApprovals]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeeSOApprovalDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeeSOApprovalsDto,
				RecordCount = allEmployeeSOApprovalsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeSOApprovalDto>> Process_GetEmployeeSOApproval(Guid employeeSOApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeeSOApprovalDto employeeSOApprovalDto = null;
		ERPResponseMessageDto<ERPEmployeeSOApprovalDto> result;
		try
		{
			IERPEmployeeSOApprovalRepository iERPEmployeeSOApprovalRepository = (base.ERPEmployeeSOApprovalRepository = new ERPEmployeeSOApprovalRepository(base.ApiClientContext));
			using (iERPEmployeeSOApprovalRepository)
			{
				ERPEmployeeSOApprovalInformationDto eRPEmployeeSOApprovalInformationDto = await base.ERPEmployeeSOApprovalRepository.GetEmployeeSOApproval(employeeSOApprovalId);
				employeeSOApprovalDto = new ERPEmployeeSOApprovalDto
				{
					lmoApprovalEmployeeID = eRPEmployeeSOApprovalInformationDto.lmoApprovalEmployeeID,
					lmoCreatedBy = eRPEmployeeSOApprovalInformationDto.lmoCreatedBy,
					lmoCreatedDate = eRPEmployeeSOApprovalInformationDto.lmoCreatedDate,
					lmoEmployeeID = eRPEmployeeSOApprovalInformationDto.lmoEmployeeID,
					lmoUniqueID = eRPEmployeeSOApprovalInformationDto.lmoUniqueID,
					lmoRowVersion = eRPEmployeeSOApprovalInformationDto.lmoRowVersion,
					lmoEmployeeSOApprovalID = eRPEmployeeSOApprovalInformationDto.lmoEmployeeSOApprovalID,
					CustomFields = eRPEmployeeSOApprovalInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeeSOApprovals []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeSOApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeeSOApprovalDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeSOApprovalDto>> Process_PutEmployeeSOApproval(ERPEmployeeSOApprovalDto employeeSOApproval)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPEmployeeSOApprovalDto createdObject = null;
		ERPResponseMessageDto<ERPEmployeeSOApprovalDto> result;
		try
		{
			IERPEmployeeSOApprovalRepository iERPEmployeeSOApprovalRepository = (base.ERPEmployeeSOApprovalRepository = new ERPEmployeeSOApprovalRepository(base.ApiClientContext));
			using (iERPEmployeeSOApprovalRepository)
			{
				APIValidationInfoDto postResult = await base.ERPEmployeeSOApprovalRepository.SaveEmployeeSOApproval(employeeSOApproval);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPEmployeeSOApprovalInformationDto eRPEmployeeSOApprovalInformationDto = await base.ERPEmployeeSOApprovalRepository.GetEmployeeSOApproval(employeeSOApproval.lmoUniqueID);
					createdObject = new ERPEmployeeSOApprovalDto
					{
						lmoApprovalEmployeeID = eRPEmployeeSOApprovalInformationDto.lmoApprovalEmployeeID,
						lmoCreatedBy = eRPEmployeeSOApprovalInformationDto.lmoCreatedBy,
						lmoCreatedDate = eRPEmployeeSOApprovalInformationDto.lmoCreatedDate,
						lmoEmployeeID = eRPEmployeeSOApprovalInformationDto.lmoEmployeeID,
						lmoUniqueID = eRPEmployeeSOApprovalInformationDto.lmoUniqueID,
						lmoRowVersion = eRPEmployeeSOApprovalInformationDto.lmoRowVersion,
						lmoEmployeeSOApprovalID = eRPEmployeeSOApprovalInformationDto.lmoEmployeeSOApprovalID,
						CustomFields = eRPEmployeeSOApprovalInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing EmployeeSOApproval [{employeeSOApproval.lmoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeSOApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeeSOApproval(Guid employeeSOApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeSOApprovalRepository iERPEmployeeSOApprovalRepository = (base.ERPEmployeeSOApprovalRepository = new ERPEmployeeSOApprovalRepository(base.ApiClientContext));
		using (iERPEmployeeSOApprovalRepository)
		{
			if (!(await base.ERPEmployeeSOApprovalRepository.DoesEmployeeSOApprovalExist(employeeSOApprovalId)))
			{
				base.ErrorsList.Add($"EmployeeSOApproval [{employeeSOApprovalId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPEmployeeSOApprovalInformationDto eRPEmployeeSOApprovalInformationDto = await base.ERPEmployeeSOApprovalRepository.GetEmployeeSOApproval(employeeSOApprovalId);
				string text = await base.ERPEmployeeSOApprovalRepository.WhereUsed("EmployeeSOApprovals", new object[2] { eRPEmployeeSOApprovalInformationDto.lmoEmployeeID, eRPEmployeeSOApprovalInformationDto.lmoApprovalEmployeeID }, new object[2] { "lmoEmployeeID", "lmoApprovalEmployeeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("EmployeeSOApproval cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeSOApprovalDto>> Process_DeleteEmployeeSOApproval(Guid employeeSOApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPEmployeeSOApprovalDto> result;
		try
		{
			IERPEmployeeSOApprovalRepository iERPEmployeeSOApprovalRepository = (base.ERPEmployeeSOApprovalRepository = new ERPEmployeeSOApprovalRepository(base.ApiClientContext));
			using (iERPEmployeeSOApprovalRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPEmployeeSOApprovalRepository.DeleteRowFromTable("EmployeeSOApprovals", "lmo", employeeSOApprovalId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of EmployeeSOApproval [{employeeSOApprovalId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeSOApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPEmployeeSOApprovalDto()
			};
		}
		return result;
	}
}
