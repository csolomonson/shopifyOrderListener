using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeePOApprovalModel : ERPBaseModel, IERPEmployeePOApprovalModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeePOApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeePOApprovalRepository iERPEmployeePOApprovalRepository = (base.ERPEmployeePOApprovalRepository = new ERPEmployeePOApprovalRepository(base.ApiClientContext));
		using (iERPEmployeePOApprovalRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeePOApprovalRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeePOApprovalRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeePOApprovalRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeePOApprovalRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeePOApproval(Guid employeePOApprovalId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeePOApprovalRepository iERPEmployeePOApprovalRepository = (base.ERPEmployeePOApprovalRepository = new ERPEmployeePOApprovalRepository(base.ApiClientContext));
		using (iERPEmployeePOApprovalRepository)
		{
			if (!(await base.ERPEmployeePOApprovalRepository.DoesEmployeePOApprovalExist(employeePOApprovalId)))
			{
				errorsList.Add($"EmployeePOApproval [{employeePOApprovalId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutEmployeePOApproval(ERPEmployeePOApprovalDto employeePOApproval)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeePOApprovalRepository iERPEmployeePOApprovalRepository = (base.ERPEmployeePOApprovalRepository = new ERPEmployeePOApprovalRepository(base.ApiClientContext));
		using (iERPEmployeePOApprovalRepository)
		{
			if (!string.IsNullOrWhiteSpace(employeePOApproval.lmhEmployeeID) && !(await base.ERPEmployeePOApprovalRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { employeePOApproval.lmhEmployeeID })))
			{
				errorsList.Add("lmhEmployeeID [" + employeePOApproval.lmhEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employeePOApproval.lmhApprovalEmployeeID) && !(await base.ERPEmployeePOApprovalRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { employeePOApproval.lmhApprovalEmployeeID })))
			{
				errorsList.Add("lmhApprovalEmployeeID [" + employeePOApproval.lmhApprovalEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeePOApprovalDto>>> Process_GetAllEmployeePOApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeePOApprovalDto> allEmployeePOApprovalsDto = new List<ERPEmployeePOApprovalDto>();
		ERPResponseMessageDto<IList<ERPEmployeePOApprovalDto>> result;
		try
		{
			IERPEmployeePOApprovalRepository iERPEmployeePOApprovalRepository = (base.ERPEmployeePOApprovalRepository = new ERPEmployeePOApprovalRepository(base.ApiClientContext));
			using (iERPEmployeePOApprovalRepository)
			{
				foreach (ERPEmployeePOApprovalInformationDto item2 in await base.ERPEmployeePOApprovalRepository.GetAllEmployeePOApprovals(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeePOApprovalDto item = new ERPEmployeePOApprovalDto
					{
						lmhApprovalEmployeeID = item2.lmhApprovalEmployeeID,
						lmhCreatedBy = item2.lmhCreatedBy,
						lmhCreatedDate = item2.lmhCreatedDate,
						lmhEmployeeID = item2.lmhEmployeeID,
						lmhUniqueID = item2.lmhUniqueID,
						lmhRowVersion = item2.lmhRowVersion,
						lmhEmployeePoApprovalID = item2.lmhEmployeePoApprovalID,
						CustomFields = item2.CustomFields
					};
					allEmployeePOApprovalsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeePOApprovals]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeePOApprovalDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeePOApprovalsDto,
				RecordCount = allEmployeePOApprovalsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeePOApprovalDto>> Process_GetEmployeePOApproval(Guid employeePOApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeePOApprovalDto employeePOApprovalDto = null;
		ERPResponseMessageDto<ERPEmployeePOApprovalDto> result;
		try
		{
			IERPEmployeePOApprovalRepository iERPEmployeePOApprovalRepository = (base.ERPEmployeePOApprovalRepository = new ERPEmployeePOApprovalRepository(base.ApiClientContext));
			using (iERPEmployeePOApprovalRepository)
			{
				ERPEmployeePOApprovalInformationDto eRPEmployeePOApprovalInformationDto = await base.ERPEmployeePOApprovalRepository.GetEmployeePOApproval(employeePOApprovalId);
				employeePOApprovalDto = new ERPEmployeePOApprovalDto
				{
					lmhApprovalEmployeeID = eRPEmployeePOApprovalInformationDto.lmhApprovalEmployeeID,
					lmhCreatedBy = eRPEmployeePOApprovalInformationDto.lmhCreatedBy,
					lmhCreatedDate = eRPEmployeePOApprovalInformationDto.lmhCreatedDate,
					lmhEmployeeID = eRPEmployeePOApprovalInformationDto.lmhEmployeeID,
					lmhUniqueID = eRPEmployeePOApprovalInformationDto.lmhUniqueID,
					lmhRowVersion = eRPEmployeePOApprovalInformationDto.lmhRowVersion,
					lmhEmployeePoApprovalID = eRPEmployeePOApprovalInformationDto.lmhEmployeePoApprovalID,
					CustomFields = eRPEmployeePOApprovalInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeePOApprovals []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeePOApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeePOApprovalDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeePOApprovalDto>> Process_PutEmployeePOApproval(ERPEmployeePOApprovalDto employeePOApproval)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPEmployeePOApprovalDto createdObject = null;
		ERPResponseMessageDto<ERPEmployeePOApprovalDto> result;
		try
		{
			IERPEmployeePOApprovalRepository iERPEmployeePOApprovalRepository = (base.ERPEmployeePOApprovalRepository = new ERPEmployeePOApprovalRepository(base.ApiClientContext));
			using (iERPEmployeePOApprovalRepository)
			{
				APIValidationInfoDto postResult = await base.ERPEmployeePOApprovalRepository.SaveEmployeePOApproval(employeePOApproval);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPEmployeePOApprovalInformationDto eRPEmployeePOApprovalInformationDto = await base.ERPEmployeePOApprovalRepository.GetEmployeePOApproval(employeePOApproval.lmhUniqueID);
					createdObject = new ERPEmployeePOApprovalDto
					{
						lmhApprovalEmployeeID = eRPEmployeePOApprovalInformationDto.lmhApprovalEmployeeID,
						lmhCreatedBy = eRPEmployeePOApprovalInformationDto.lmhCreatedBy,
						lmhCreatedDate = eRPEmployeePOApprovalInformationDto.lmhCreatedDate,
						lmhEmployeeID = eRPEmployeePOApprovalInformationDto.lmhEmployeeID,
						lmhUniqueID = eRPEmployeePOApprovalInformationDto.lmhUniqueID,
						lmhRowVersion = eRPEmployeePOApprovalInformationDto.lmhRowVersion,
						lmhEmployeePoApprovalID = eRPEmployeePOApprovalInformationDto.lmhEmployeePoApprovalID,
						CustomFields = eRPEmployeePOApprovalInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing EmployeePOApproval [{employeePOApproval.lmhUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeePOApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeePOApproval(Guid employeePOApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeePOApprovalRepository iERPEmployeePOApprovalRepository = (base.ERPEmployeePOApprovalRepository = new ERPEmployeePOApprovalRepository(base.ApiClientContext));
		using (iERPEmployeePOApprovalRepository)
		{
			if (!(await base.ERPEmployeePOApprovalRepository.DoesEmployeePOApprovalExist(employeePOApprovalId)))
			{
				base.ErrorsList.Add($"EmployeePOApproval [{employeePOApprovalId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPEmployeePOApprovalInformationDto eRPEmployeePOApprovalInformationDto = await base.ERPEmployeePOApprovalRepository.GetEmployeePOApproval(employeePOApprovalId);
				string text = await base.ERPEmployeePOApprovalRepository.WhereUsed("EmployeePOApprovals", new object[2] { eRPEmployeePOApprovalInformationDto.lmhEmployeeID, eRPEmployeePOApprovalInformationDto.lmhApprovalEmployeeID }, new object[2] { "lmhEmployeeID", "lmhApprovalEmployeeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("EmployeePOApproval cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPEmployeePOApprovalDto>> Process_DeleteEmployeePOApproval(Guid employeePOApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPEmployeePOApprovalDto> result;
		try
		{
			IERPEmployeePOApprovalRepository iERPEmployeePOApprovalRepository = (base.ERPEmployeePOApprovalRepository = new ERPEmployeePOApprovalRepository(base.ApiClientContext));
			using (iERPEmployeePOApprovalRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPEmployeePOApprovalRepository.DeleteRowFromTable("EmployeePOApprovals", "lmh", employeePOApprovalId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of EmployeePOApproval [{employeePOApprovalId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeePOApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPEmployeePOApprovalDto()
			};
		}
		return result;
	}
}
