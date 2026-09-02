using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSkillModel : ERPBaseModel, IERPSkillModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSkills(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSkillRepository iERPSkillRepository = (base.ERPSkillRepository = new ERPSkillRepository(base.ApiClientContext));
		using (iERPSkillRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSkillRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSkillRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSkillRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSkillRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSkill(Guid skillId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSkillRepository iERPSkillRepository = (base.ERPSkillRepository = new ERPSkillRepository(base.ApiClientContext));
		using (iERPSkillRepository)
		{
			if (!(await base.ERPSkillRepository.DoesSkillExist(skillId)))
			{
				errorsList.Add($"Skill [{skillId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSkillDto>>> Process_GetAllSkills(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSkillDto> allSkillsDto = new List<ERPSkillDto>();
		ERPResponseMessageDto<IList<ERPSkillDto>> result;
		try
		{
			IERPSkillRepository iERPSkillRepository = (base.ERPSkillRepository = new ERPSkillRepository(base.ApiClientContext));
			using (iERPSkillRepository)
			{
				foreach (ERPSkillInformationDto item2 in await base.ERPSkillRepository.GetAllSkills(pageSize, pageNumber, filter, orderBy))
				{
					ERPSkillDto item = new ERPSkillDto
					{
						lesSkillID = item2.lesSkillID,
						lesCreatedBy = item2.lesCreatedBy,
						lesCreatedDate = item2.lesCreatedDate,
						lesDescription = item2.lesDescription,
						lesUniqueID = item2.lesUniqueID,
						lesInactiveDate = item2.lesInactiveDate,
						lesInactive = item2.lesInactive,
						lesLongDescriptionRtf = item2.lesLongDescriptionRtf,
						lesLongDescriptionText = item2.lesLongDescriptionText,
						lesRowVersion = item2.lesRowVersion,
						CustomFields = item2.CustomFields
					};
					allSkillsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Skills]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSkillDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSkillsDto,
				RecordCount = allSkillsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSkillDto>> Process_GetSkill(Guid skillId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSkillDto skillDto = null;
		ERPResponseMessageDto<ERPSkillDto> result;
		try
		{
			IERPSkillRepository iERPSkillRepository = (base.ERPSkillRepository = new ERPSkillRepository(base.ApiClientContext));
			using (iERPSkillRepository)
			{
				ERPSkillInformationDto eRPSkillInformationDto = await base.ERPSkillRepository.GetSkill(skillId);
				skillDto = new ERPSkillDto
				{
					lesSkillID = eRPSkillInformationDto.lesSkillID,
					lesCreatedBy = eRPSkillInformationDto.lesCreatedBy,
					lesCreatedDate = eRPSkillInformationDto.lesCreatedDate,
					lesDescription = eRPSkillInformationDto.lesDescription,
					lesUniqueID = eRPSkillInformationDto.lesUniqueID,
					lesInactiveDate = eRPSkillInformationDto.lesInactiveDate,
					lesInactive = eRPSkillInformationDto.lesInactive,
					lesLongDescriptionRtf = eRPSkillInformationDto.lesLongDescriptionRtf,
					lesLongDescriptionText = eRPSkillInformationDto.lesLongDescriptionText,
					lesRowVersion = eRPSkillInformationDto.lesRowVersion,
					CustomFields = eRPSkillInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Skills []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSkillDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = skillDto
			};
		}
		return result;
	}
}
