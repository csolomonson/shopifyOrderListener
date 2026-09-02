using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSkillCompetencyModel : ERPBaseModel, IERPSkillCompetencyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSkillCompetencyRepository iERPSkillCompetencyRepository = (base.ERPSkillCompetencyRepository = new ERPSkillCompetencyRepository(base.ApiClientContext));
		using (iERPSkillCompetencyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSkillCompetencyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSkillCompetencyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSkillCompetencyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSkillCompetencyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSkillCompetency(Guid skillCompetencyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSkillCompetencyRepository iERPSkillCompetencyRepository = (base.ERPSkillCompetencyRepository = new ERPSkillCompetencyRepository(base.ApiClientContext));
		using (iERPSkillCompetencyRepository)
		{
			if (!(await base.ERPSkillCompetencyRepository.DoesSkillCompetencyExist(skillCompetencyId)))
			{
				errorsList.Add($"SkillCompetency [{skillCompetencyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSkillCompetencyDto>>> Process_GetAllSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSkillCompetencyDto> allSkillCompetenciesDto = new List<ERPSkillCompetencyDto>();
		ERPResponseMessageDto<IList<ERPSkillCompetencyDto>> result;
		try
		{
			IERPSkillCompetencyRepository iERPSkillCompetencyRepository = (base.ERPSkillCompetencyRepository = new ERPSkillCompetencyRepository(base.ApiClientContext));
			using (iERPSkillCompetencyRepository)
			{
				foreach (ERPSkillCompetencyInformationDto item2 in await base.ERPSkillCompetencyRepository.GetAllSkillCompetencies(pageSize, pageNumber, filter, orderBy))
				{
					ERPSkillCompetencyDto item = new ERPSkillCompetencyDto
					{
						lecColor = item2.lecColor,
						lecCompetencyID = item2.lecCompetencyID,
						lecCreatedBy = item2.lecCreatedBy,
						lecCreatedDate = item2.lecCreatedDate,
						lecDescription = item2.lecDescription,
						lecUniqueID = item2.lecUniqueID,
						lecInactiveDate = item2.lecInactiveDate,
						lecInactive = item2.lecInactive,
						lecLevel = item2.lecLevel,
						lecLongDescriptionRtf = item2.lecLongDescriptionRtf,
						lecLongDescriptionText = item2.lecLongDescriptionText,
						lecRowVersion = item2.lecRowVersion,
						CustomFields = item2.CustomFields
					};
					allSkillCompetenciesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SkillCompetencies]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSkillCompetencyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSkillCompetenciesDto,
				RecordCount = allSkillCompetenciesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSkillCompetencyDto>> Process_GetSkillCompetency(Guid skillCompetencyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSkillCompetencyDto skillCompetencyDto = null;
		ERPResponseMessageDto<ERPSkillCompetencyDto> result;
		try
		{
			IERPSkillCompetencyRepository iERPSkillCompetencyRepository = (base.ERPSkillCompetencyRepository = new ERPSkillCompetencyRepository(base.ApiClientContext));
			using (iERPSkillCompetencyRepository)
			{
				ERPSkillCompetencyInformationDto eRPSkillCompetencyInformationDto = await base.ERPSkillCompetencyRepository.GetSkillCompetency(skillCompetencyId);
				skillCompetencyDto = new ERPSkillCompetencyDto
				{
					lecColor = eRPSkillCompetencyInformationDto.lecColor,
					lecCompetencyID = eRPSkillCompetencyInformationDto.lecCompetencyID,
					lecCreatedBy = eRPSkillCompetencyInformationDto.lecCreatedBy,
					lecCreatedDate = eRPSkillCompetencyInformationDto.lecCreatedDate,
					lecDescription = eRPSkillCompetencyInformationDto.lecDescription,
					lecUniqueID = eRPSkillCompetencyInformationDto.lecUniqueID,
					lecInactiveDate = eRPSkillCompetencyInformationDto.lecInactiveDate,
					lecInactive = eRPSkillCompetencyInformationDto.lecInactive,
					lecLevel = eRPSkillCompetencyInformationDto.lecLevel,
					lecLongDescriptionRtf = eRPSkillCompetencyInformationDto.lecLongDescriptionRtf,
					lecLongDescriptionText = eRPSkillCompetencyInformationDto.lecLongDescriptionText,
					lecRowVersion = eRPSkillCompetencyInformationDto.lecRowVersion,
					CustomFields = eRPSkillCompetencyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SkillCompetencies []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSkillCompetencyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = skillCompetencyDto
			};
		}
		return result;
	}
}
