using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeeMemoModel : ERPBaseModel, IERPEmployeeMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeeMemoRepository iERPEmployeeMemoRepository = (base.ERPEmployeeMemoRepository = new ERPEmployeeMemoRepository(base.ApiClientContext));
		using (iERPEmployeeMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeeMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeeMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeeMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeeMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeeMemo(Guid employeeMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeMemoRepository iERPEmployeeMemoRepository = (base.ERPEmployeeMemoRepository = new ERPEmployeeMemoRepository(base.ApiClientContext));
		using (iERPEmployeeMemoRepository)
		{
			if (!(await base.ERPEmployeeMemoRepository.DoesEmployeeMemoExist(employeeMemoId)))
			{
				errorsList.Add($"EmployeeMemo [{employeeMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutEmployeeMemo(ERPEmployeeMemoDto employeeMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeMemoRepository iERPEmployeeMemoRepository = (base.ERPEmployeeMemoRepository = new ERPEmployeeMemoRepository(base.ApiClientContext));
		using (iERPEmployeeMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(employeeMemo.lmkEmployeeID) && !(await base.ERPEmployeeMemoRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { employeeMemo.lmkEmployeeID })))
			{
				errorsList.Add("lmkEmployeeID [" + employeeMemo.lmkEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeeMemoDto>>> Process_GetAllEmployeeMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeeMemoDto> allEmployeeMemosDto = new List<ERPEmployeeMemoDto>();
		ERPResponseMessageDto<IList<ERPEmployeeMemoDto>> result;
		try
		{
			IERPEmployeeMemoRepository iERPEmployeeMemoRepository = (base.ERPEmployeeMemoRepository = new ERPEmployeeMemoRepository(base.ApiClientContext));
			using (iERPEmployeeMemoRepository)
			{
				foreach (ERPEmployeeMemoInformationDto item2 in await base.ERPEmployeeMemoRepository.GetAllEmployeeMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeeMemoDto item = new ERPEmployeeMemoDto
					{
						lmkCreatedBy = item2.lmkCreatedBy,
						lmkCreatedDate = item2.lmkCreatedDate,
						lmkEmployeeID = item2.lmkEmployeeID,
						lmkUniqueID = item2.lmkUniqueID,
						lmkLongDescriptionRtf = item2.lmkLongDescriptionRtf,
						lmkLongDescriptionText = item2.lmkLongDescriptionText,
						lmkMemoDate = item2.lmkMemoDate,
						lmkRowVersion = item2.lmkRowVersion,
						lmkEmployeeMemoID = item2.lmkEmployeeMemoID,
						lmkShortDescription = item2.lmkShortDescription,
						lmkShowInEmployees = item2.lmkShowInEmployees,
						CustomFields = item2.CustomFields
					};
					allEmployeeMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeeMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeeMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeeMemosDto,
				RecordCount = allEmployeeMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeMemoDto>> Process_GetEmployeeMemo(Guid employeeMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeeMemoDto employeeMemoDto = null;
		ERPResponseMessageDto<ERPEmployeeMemoDto> result;
		try
		{
			IERPEmployeeMemoRepository iERPEmployeeMemoRepository = (base.ERPEmployeeMemoRepository = new ERPEmployeeMemoRepository(base.ApiClientContext));
			using (iERPEmployeeMemoRepository)
			{
				ERPEmployeeMemoInformationDto eRPEmployeeMemoInformationDto = await base.ERPEmployeeMemoRepository.GetEmployeeMemo(employeeMemoId);
				employeeMemoDto = new ERPEmployeeMemoDto
				{
					lmkCreatedBy = eRPEmployeeMemoInformationDto.lmkCreatedBy,
					lmkCreatedDate = eRPEmployeeMemoInformationDto.lmkCreatedDate,
					lmkEmployeeID = eRPEmployeeMemoInformationDto.lmkEmployeeID,
					lmkUniqueID = eRPEmployeeMemoInformationDto.lmkUniqueID,
					lmkLongDescriptionRtf = eRPEmployeeMemoInformationDto.lmkLongDescriptionRtf,
					lmkLongDescriptionText = eRPEmployeeMemoInformationDto.lmkLongDescriptionText,
					lmkMemoDate = eRPEmployeeMemoInformationDto.lmkMemoDate,
					lmkRowVersion = eRPEmployeeMemoInformationDto.lmkRowVersion,
					lmkEmployeeMemoID = eRPEmployeeMemoInformationDto.lmkEmployeeMemoID,
					lmkShortDescription = eRPEmployeeMemoInformationDto.lmkShortDescription,
					lmkShowInEmployees = eRPEmployeeMemoInformationDto.lmkShowInEmployees,
					CustomFields = eRPEmployeeMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeeMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeeMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeMemoDto>> Process_PutEmployeeMemo(ERPEmployeeMemoDto employeeMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPEmployeeMemoDto createdObject = null;
		ERPResponseMessageDto<ERPEmployeeMemoDto> result;
		try
		{
			IERPEmployeeMemoRepository iERPEmployeeMemoRepository = (base.ERPEmployeeMemoRepository = new ERPEmployeeMemoRepository(base.ApiClientContext));
			using (iERPEmployeeMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPEmployeeMemoRepository.SaveEmployeeMemo(employeeMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPEmployeeMemoInformationDto eRPEmployeeMemoInformationDto = await base.ERPEmployeeMemoRepository.GetEmployeeMemo(employeeMemo.lmkUniqueID);
					createdObject = new ERPEmployeeMemoDto
					{
						lmkCreatedBy = eRPEmployeeMemoInformationDto.lmkCreatedBy,
						lmkCreatedDate = eRPEmployeeMemoInformationDto.lmkCreatedDate,
						lmkEmployeeID = eRPEmployeeMemoInformationDto.lmkEmployeeID,
						lmkUniqueID = eRPEmployeeMemoInformationDto.lmkUniqueID,
						lmkLongDescriptionRtf = eRPEmployeeMemoInformationDto.lmkLongDescriptionRtf,
						lmkLongDescriptionText = eRPEmployeeMemoInformationDto.lmkLongDescriptionText,
						lmkMemoDate = eRPEmployeeMemoInformationDto.lmkMemoDate,
						lmkRowVersion = eRPEmployeeMemoInformationDto.lmkRowVersion,
						lmkEmployeeMemoID = eRPEmployeeMemoInformationDto.lmkEmployeeMemoID,
						lmkShortDescription = eRPEmployeeMemoInformationDto.lmkShortDescription,
						lmkShowInEmployees = eRPEmployeeMemoInformationDto.lmkShowInEmployees,
						CustomFields = eRPEmployeeMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing EmployeeMemo [{employeeMemo.lmkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeeMemo(Guid employeeMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeMemoRepository iERPEmployeeMemoRepository = (base.ERPEmployeeMemoRepository = new ERPEmployeeMemoRepository(base.ApiClientContext));
		using (iERPEmployeeMemoRepository)
		{
			if (!(await base.ERPEmployeeMemoRepository.DoesEmployeeMemoExist(employeeMemoId)))
			{
				base.ErrorsList.Add($"EmployeeMemo [{employeeMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPEmployeeMemoInformationDto eRPEmployeeMemoInformationDto = await base.ERPEmployeeMemoRepository.GetEmployeeMemo(employeeMemoId);
				string text = await base.ERPEmployeeMemoRepository.WhereUsed("EmployeeMemos", new object[2] { eRPEmployeeMemoInformationDto.lmkEmployeeID, eRPEmployeeMemoInformationDto.lmkEmployeeMemoID }, new object[2] { "lmkEmployeeID", "lmkEmployeeMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("EmployeeMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeMemoDto>> Process_DeleteEmployeeMemo(Guid employeeMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPEmployeeMemoDto> result;
		try
		{
			IERPEmployeeMemoRepository iERPEmployeeMemoRepository = (base.ERPEmployeeMemoRepository = new ERPEmployeeMemoRepository(base.ApiClientContext));
			using (iERPEmployeeMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPEmployeeMemoRepository.DeleteRowFromTable("EmployeeMemos", "lmk", employeeMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of EmployeeMemo [{employeeMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPEmployeeMemoDto()
			};
		}
		return result;
	}
}
