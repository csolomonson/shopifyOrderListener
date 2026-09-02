using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeeQAApprovalModel : ERPBaseModel, IERPEmployeeQAApprovalModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeQAApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeeQAApprovalRepository iERPEmployeeQAApprovalRepository = (base.ERPEmployeeQAApprovalRepository = new ERPEmployeeQAApprovalRepository(base.ApiClientContext));
		using (iERPEmployeeQAApprovalRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeeQAApprovalRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeeQAApprovalRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeeQAApprovalRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeeQAApprovalRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeeQAApproval(Guid employeeQAApprovalId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeQAApprovalRepository iERPEmployeeQAApprovalRepository = (base.ERPEmployeeQAApprovalRepository = new ERPEmployeeQAApprovalRepository(base.ApiClientContext));
		using (iERPEmployeeQAApprovalRepository)
		{
			if (!(await base.ERPEmployeeQAApprovalRepository.DoesEmployeeQAApprovalExist(employeeQAApprovalId)))
			{
				errorsList.Add($"EmployeeQAApproval [{employeeQAApprovalId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutEmployeeQAApproval(ERPEmployeeQAApprovalDto employeeQAApproval)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeQAApprovalRepository iERPEmployeeQAApprovalRepository = (base.ERPEmployeeQAApprovalRepository = new ERPEmployeeQAApprovalRepository(base.ApiClientContext));
		using (iERPEmployeeQAApprovalRepository)
		{
			if (!string.IsNullOrWhiteSpace(employeeQAApproval.lmbEmployeeID) && !(await base.ERPEmployeeQAApprovalRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { employeeQAApproval.lmbEmployeeID })))
			{
				errorsList.Add("lmbEmployeeID [" + employeeQAApproval.lmbEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employeeQAApproval.lmbApprovalEmployeeID) && !(await base.ERPEmployeeQAApprovalRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { employeeQAApproval.lmbApprovalEmployeeID })))
			{
				errorsList.Add("lmbApprovalEmployeeID [" + employeeQAApproval.lmbApprovalEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeeQAApprovalDto>>> Process_GetAllEmployeeQAApprovals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeeQAApprovalDto> allEmployeeQAApprovalsDto = new List<ERPEmployeeQAApprovalDto>();
		ERPResponseMessageDto<IList<ERPEmployeeQAApprovalDto>> result;
		try
		{
			IERPEmployeeQAApprovalRepository iERPEmployeeQAApprovalRepository = (base.ERPEmployeeQAApprovalRepository = new ERPEmployeeQAApprovalRepository(base.ApiClientContext));
			using (iERPEmployeeQAApprovalRepository)
			{
				foreach (ERPEmployeeQAApprovalInformationDto item2 in await base.ERPEmployeeQAApprovalRepository.GetAllEmployeeQAApprovals(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeeQAApprovalDto item = new ERPEmployeeQAApprovalDto
					{
						lmbApprovalEmployeeID = item2.lmbApprovalEmployeeID,
						lmbCreatedBy = item2.lmbCreatedBy,
						lmbCreatedDate = item2.lmbCreatedDate,
						lmbEmployeeID = item2.lmbEmployeeID,
						lmbUniqueID = item2.lmbUniqueID,
						lmbRowVersion = item2.lmbRowVersion,
						lmbEmployeeQAApprovalID = item2.lmbEmployeeQAApprovalID,
						CustomFields = item2.CustomFields
					};
					allEmployeeQAApprovalsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeeQAApprovals]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeeQAApprovalDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeeQAApprovalsDto,
				RecordCount = allEmployeeQAApprovalsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeQAApprovalDto>> Process_GetEmployeeQAApproval(Guid employeeQAApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeeQAApprovalDto employeeQAApprovalDto = null;
		ERPResponseMessageDto<ERPEmployeeQAApprovalDto> result;
		try
		{
			IERPEmployeeQAApprovalRepository iERPEmployeeQAApprovalRepository = (base.ERPEmployeeQAApprovalRepository = new ERPEmployeeQAApprovalRepository(base.ApiClientContext));
			using (iERPEmployeeQAApprovalRepository)
			{
				ERPEmployeeQAApprovalInformationDto eRPEmployeeQAApprovalInformationDto = await base.ERPEmployeeQAApprovalRepository.GetEmployeeQAApproval(employeeQAApprovalId);
				employeeQAApprovalDto = new ERPEmployeeQAApprovalDto
				{
					lmbApprovalEmployeeID = eRPEmployeeQAApprovalInformationDto.lmbApprovalEmployeeID,
					lmbCreatedBy = eRPEmployeeQAApprovalInformationDto.lmbCreatedBy,
					lmbCreatedDate = eRPEmployeeQAApprovalInformationDto.lmbCreatedDate,
					lmbEmployeeID = eRPEmployeeQAApprovalInformationDto.lmbEmployeeID,
					lmbUniqueID = eRPEmployeeQAApprovalInformationDto.lmbUniqueID,
					lmbRowVersion = eRPEmployeeQAApprovalInformationDto.lmbRowVersion,
					lmbEmployeeQAApprovalID = eRPEmployeeQAApprovalInformationDto.lmbEmployeeQAApprovalID,
					CustomFields = eRPEmployeeQAApprovalInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeeQAApprovals []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeQAApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeeQAApprovalDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeQAApprovalDto>> Process_PutEmployeeQAApproval(ERPEmployeeQAApprovalDto employeeQAApproval)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPEmployeeQAApprovalDto createdObject = null;
		ERPResponseMessageDto<ERPEmployeeQAApprovalDto> result;
		try
		{
			IERPEmployeeQAApprovalRepository iERPEmployeeQAApprovalRepository = (base.ERPEmployeeQAApprovalRepository = new ERPEmployeeQAApprovalRepository(base.ApiClientContext));
			using (iERPEmployeeQAApprovalRepository)
			{
				APIValidationInfoDto postResult = await base.ERPEmployeeQAApprovalRepository.SaveEmployeeQAApproval(employeeQAApproval);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPEmployeeQAApprovalInformationDto eRPEmployeeQAApprovalInformationDto = await base.ERPEmployeeQAApprovalRepository.GetEmployeeQAApproval(employeeQAApproval.lmbUniqueID);
					createdObject = new ERPEmployeeQAApprovalDto
					{
						lmbApprovalEmployeeID = eRPEmployeeQAApprovalInformationDto.lmbApprovalEmployeeID,
						lmbCreatedBy = eRPEmployeeQAApprovalInformationDto.lmbCreatedBy,
						lmbCreatedDate = eRPEmployeeQAApprovalInformationDto.lmbCreatedDate,
						lmbEmployeeID = eRPEmployeeQAApprovalInformationDto.lmbEmployeeID,
						lmbUniqueID = eRPEmployeeQAApprovalInformationDto.lmbUniqueID,
						lmbRowVersion = eRPEmployeeQAApprovalInformationDto.lmbRowVersion,
						lmbEmployeeQAApprovalID = eRPEmployeeQAApprovalInformationDto.lmbEmployeeQAApprovalID,
						CustomFields = eRPEmployeeQAApprovalInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing EmployeeQAApproval [{employeeQAApproval.lmbUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeQAApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeeQAApproval(Guid employeeQAApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeQAApprovalRepository iERPEmployeeQAApprovalRepository = (base.ERPEmployeeQAApprovalRepository = new ERPEmployeeQAApprovalRepository(base.ApiClientContext));
		using (iERPEmployeeQAApprovalRepository)
		{
			if (!(await base.ERPEmployeeQAApprovalRepository.DoesEmployeeQAApprovalExist(employeeQAApprovalId)))
			{
				base.ErrorsList.Add($"EmployeeQAApproval [{employeeQAApprovalId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPEmployeeQAApprovalInformationDto eRPEmployeeQAApprovalInformationDto = await base.ERPEmployeeQAApprovalRepository.GetEmployeeQAApproval(employeeQAApprovalId);
				string text = await base.ERPEmployeeQAApprovalRepository.WhereUsed("EmployeeQAApprovals", new object[2] { eRPEmployeeQAApprovalInformationDto.lmbEmployeeID, eRPEmployeeQAApprovalInformationDto.lmbApprovalEmployeeID }, new object[2] { "lmbEmployeeID", "lmbApprovalEmployeeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("EmployeeQAApproval cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeQAApprovalDto>> Process_DeleteEmployeeQAApproval(Guid employeeQAApprovalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPEmployeeQAApprovalDto> result;
		try
		{
			IERPEmployeeQAApprovalRepository iERPEmployeeQAApprovalRepository = (base.ERPEmployeeQAApprovalRepository = new ERPEmployeeQAApprovalRepository(base.ApiClientContext));
			using (iERPEmployeeQAApprovalRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPEmployeeQAApprovalRepository.DeleteRowFromTable("EmployeeQAApprovals", "lmb", employeeQAApprovalId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of EmployeeQAApproval [{employeeQAApprovalId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeQAApprovalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPEmployeeQAApprovalDto()
			};
		}
		return result;
	}
}
