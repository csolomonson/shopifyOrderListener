using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeeSkillModel : ERPBaseModel, IERPEmployeeSkillModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeSkills(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeeSkillRepository iERPEmployeeSkillRepository = (base.ERPEmployeeSkillRepository = new ERPEmployeeSkillRepository(base.ApiClientContext));
		using (iERPEmployeeSkillRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeeSkillRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeeSkillRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeeSkillRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeeSkillRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeeSkill(Guid employeeSkillId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeSkillRepository iERPEmployeeSkillRepository = (base.ERPEmployeeSkillRepository = new ERPEmployeeSkillRepository(base.ApiClientContext));
		using (iERPEmployeeSkillRepository)
		{
			if (!(await base.ERPEmployeeSkillRepository.DoesEmployeeSkillExist(employeeSkillId)))
			{
				errorsList.Add($"EmployeeSkill [{employeeSkillId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeeSkillDto>>> Process_GetAllEmployeeSkills(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeeSkillDto> allEmployeeSkillsDto = new List<ERPEmployeeSkillDto>();
		ERPResponseMessageDto<IList<ERPEmployeeSkillDto>> result;
		try
		{
			IERPEmployeeSkillRepository iERPEmployeeSkillRepository = (base.ERPEmployeeSkillRepository = new ERPEmployeeSkillRepository(base.ApiClientContext));
			using (iERPEmployeeSkillRepository)
			{
				foreach (ERPEmployeeSkillInformationDto item2 in await base.ERPEmployeeSkillRepository.GetAllEmployeeSkills(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeeSkillDto item = new ERPEmployeeSkillDto
					{
						lnkCreatedBy = item2.lnkCreatedBy,
						lnkCreatedDate = item2.lnkCreatedDate,
						lnkDocuments = item2.lnkDocuments,
						lnkEmployeeID = item2.lnkEmployeeID,
						lnkUniqueID = item2.lnkUniqueID,
						lnkNotesRTF = item2.lnkNotesRTF,
						lnkNotesText = item2.lnkNotesText,
						lnkRowVersion = item2.lnkRowVersion,
						lnkEmployeeSkillID = item2.lnkEmployeeSkillID,
						lnkSkillID = item2.lnkSkillID,
						CustomFields = item2.CustomFields
					};
					allEmployeeSkillsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeeSkills]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeeSkillDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeeSkillsDto,
				RecordCount = allEmployeeSkillsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeSkillDto>> Process_GetEmployeeSkill(Guid employeeSkillId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeeSkillDto employeeSkillDto = null;
		ERPResponseMessageDto<ERPEmployeeSkillDto> result;
		try
		{
			IERPEmployeeSkillRepository iERPEmployeeSkillRepository = (base.ERPEmployeeSkillRepository = new ERPEmployeeSkillRepository(base.ApiClientContext));
			using (iERPEmployeeSkillRepository)
			{
				ERPEmployeeSkillInformationDto eRPEmployeeSkillInformationDto = await base.ERPEmployeeSkillRepository.GetEmployeeSkill(employeeSkillId);
				employeeSkillDto = new ERPEmployeeSkillDto
				{
					lnkCreatedBy = eRPEmployeeSkillInformationDto.lnkCreatedBy,
					lnkCreatedDate = eRPEmployeeSkillInformationDto.lnkCreatedDate,
					lnkDocuments = eRPEmployeeSkillInformationDto.lnkDocuments,
					lnkEmployeeID = eRPEmployeeSkillInformationDto.lnkEmployeeID,
					lnkUniqueID = eRPEmployeeSkillInformationDto.lnkUniqueID,
					lnkNotesRTF = eRPEmployeeSkillInformationDto.lnkNotesRTF,
					lnkNotesText = eRPEmployeeSkillInformationDto.lnkNotesText,
					lnkRowVersion = eRPEmployeeSkillInformationDto.lnkRowVersion,
					lnkEmployeeSkillID = eRPEmployeeSkillInformationDto.lnkEmployeeSkillID,
					lnkSkillID = eRPEmployeeSkillInformationDto.lnkSkillID,
					CustomFields = eRPEmployeeSkillInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeeSkills []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeSkillDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeeSkillDto
			};
		}
		return result;
	}
}
