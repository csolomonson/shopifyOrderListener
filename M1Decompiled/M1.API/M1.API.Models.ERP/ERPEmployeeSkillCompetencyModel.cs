using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeeSkillCompetencyModel : ERPBaseModel, IERPEmployeeSkillCompetencyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeeSkillCompetencyRepository iERPEmployeeSkillCompetencyRepository = (base.ERPEmployeeSkillCompetencyRepository = new ERPEmployeeSkillCompetencyRepository(base.ApiClientContext));
		using (iERPEmployeeSkillCompetencyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeeSkillCompetencyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeeSkillCompetencyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeeSkillCompetencyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeeSkillCompetencyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeeSkillCompetency(Guid employeeSkillCompetencyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeSkillCompetencyRepository iERPEmployeeSkillCompetencyRepository = (base.ERPEmployeeSkillCompetencyRepository = new ERPEmployeeSkillCompetencyRepository(base.ApiClientContext));
		using (iERPEmployeeSkillCompetencyRepository)
		{
			if (!(await base.ERPEmployeeSkillCompetencyRepository.DoesEmployeeSkillCompetencyExist(employeeSkillCompetencyId)))
			{
				errorsList.Add($"EmployeeSkillCompetency [{employeeSkillCompetencyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeeSkillCompetencyDto>>> Process_GetAllEmployeeSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeeSkillCompetencyDto> allEmployeeSkillCompetenciesDto = new List<ERPEmployeeSkillCompetencyDto>();
		ERPResponseMessageDto<IList<ERPEmployeeSkillCompetencyDto>> result;
		try
		{
			IERPEmployeeSkillCompetencyRepository iERPEmployeeSkillCompetencyRepository = (base.ERPEmployeeSkillCompetencyRepository = new ERPEmployeeSkillCompetencyRepository(base.ApiClientContext));
			using (iERPEmployeeSkillCompetencyRepository)
			{
				foreach (ERPEmployeeSkillCompetencyInformationDto item2 in await base.ERPEmployeeSkillCompetencyRepository.GetAllEmployeeSkillCompetencies(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeeSkillCompetencyDto item = new ERPEmployeeSkillCompetencyDto
					{
						lnpCommentsRTF = item2.lnpCommentsRTF,
						lnpCommentsText = item2.lnpCommentsText,
						lnpCompetencyID = item2.lnpCompetencyID,
						lnpCreatedBy = item2.lnpCreatedBy,
						lnpCreatedDate = item2.lnpCreatedDate,
						lnpDateAchieved = item2.lnpDateAchieved,
						lnpDateExpires = item2.lnpDateExpires,
						lnpEmployeeID = item2.lnpEmployeeID,
						lnpEmployeeSkillID = item2.lnpEmployeeSkillID,
						lnpUniqueID = item2.lnpUniqueID,
						lnpRowVersion = item2.lnpRowVersion,
						lnpEmployeeSkillCompetencyID = item2.lnpEmployeeSkillCompetencyID,
						lnpSkillID = item2.lnpSkillID,
						CustomFields = item2.CustomFields
					};
					allEmployeeSkillCompetenciesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeeSkillCompetencies]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeeSkillCompetencyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeeSkillCompetenciesDto,
				RecordCount = allEmployeeSkillCompetenciesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeSkillCompetencyDto>> Process_GetEmployeeSkillCompetency(Guid employeeSkillCompetencyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeeSkillCompetencyDto employeeSkillCompetencyDto = null;
		ERPResponseMessageDto<ERPEmployeeSkillCompetencyDto> result;
		try
		{
			IERPEmployeeSkillCompetencyRepository iERPEmployeeSkillCompetencyRepository = (base.ERPEmployeeSkillCompetencyRepository = new ERPEmployeeSkillCompetencyRepository(base.ApiClientContext));
			using (iERPEmployeeSkillCompetencyRepository)
			{
				ERPEmployeeSkillCompetencyInformationDto eRPEmployeeSkillCompetencyInformationDto = await base.ERPEmployeeSkillCompetencyRepository.GetEmployeeSkillCompetency(employeeSkillCompetencyId);
				employeeSkillCompetencyDto = new ERPEmployeeSkillCompetencyDto
				{
					lnpCommentsRTF = eRPEmployeeSkillCompetencyInformationDto.lnpCommentsRTF,
					lnpCommentsText = eRPEmployeeSkillCompetencyInformationDto.lnpCommentsText,
					lnpCompetencyID = eRPEmployeeSkillCompetencyInformationDto.lnpCompetencyID,
					lnpCreatedBy = eRPEmployeeSkillCompetencyInformationDto.lnpCreatedBy,
					lnpCreatedDate = eRPEmployeeSkillCompetencyInformationDto.lnpCreatedDate,
					lnpDateAchieved = eRPEmployeeSkillCompetencyInformationDto.lnpDateAchieved,
					lnpDateExpires = eRPEmployeeSkillCompetencyInformationDto.lnpDateExpires,
					lnpEmployeeID = eRPEmployeeSkillCompetencyInformationDto.lnpEmployeeID,
					lnpEmployeeSkillID = eRPEmployeeSkillCompetencyInformationDto.lnpEmployeeSkillID,
					lnpUniqueID = eRPEmployeeSkillCompetencyInformationDto.lnpUniqueID,
					lnpRowVersion = eRPEmployeeSkillCompetencyInformationDto.lnpRowVersion,
					lnpEmployeeSkillCompetencyID = eRPEmployeeSkillCompetencyInformationDto.lnpEmployeeSkillCompetencyID,
					lnpSkillID = eRPEmployeeSkillCompetencyInformationDto.lnpSkillID,
					CustomFields = eRPEmployeeSkillCompetencyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeeSkillCompetencies []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeSkillCompetencyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeeSkillCompetencyDto
			};
		}
		return result;
	}
}
