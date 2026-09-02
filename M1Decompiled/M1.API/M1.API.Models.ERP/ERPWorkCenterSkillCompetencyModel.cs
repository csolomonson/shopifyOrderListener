using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWorkCenterSkillCompetencyModel : ERPBaseModel, IERPWorkCenterSkillCompetencyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenterSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWorkCenterSkillCompetencyRepository iERPWorkCenterSkillCompetencyRepository = (base.ERPWorkCenterSkillCompetencyRepository = new ERPWorkCenterSkillCompetencyRepository(base.ApiClientContext));
		using (iERPWorkCenterSkillCompetencyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWorkCenterSkillCompetencyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWorkCenterSkillCompetencyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWorkCenterSkillCompetencyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWorkCenterSkillCompetencyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWorkCenterSkillCompetency(Guid workCenterSkillCompetencyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterSkillCompetencyRepository iERPWorkCenterSkillCompetencyRepository = (base.ERPWorkCenterSkillCompetencyRepository = new ERPWorkCenterSkillCompetencyRepository(base.ApiClientContext));
		using (iERPWorkCenterSkillCompetencyRepository)
		{
			if (!(await base.ERPWorkCenterSkillCompetencyRepository.DoesWorkCenterSkillCompetencyExist(workCenterSkillCompetencyId)))
			{
				errorsList.Add($"WorkCenterSkillCompetency [{workCenterSkillCompetencyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWorkCenterSkillCompetencyDto>>> Process_GetAllWorkCenterSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWorkCenterSkillCompetencyDto> allWorkCenterSkillCompetenciesDto = new List<ERPWorkCenterSkillCompetencyDto>();
		ERPResponseMessageDto<IList<ERPWorkCenterSkillCompetencyDto>> result;
		try
		{
			IERPWorkCenterSkillCompetencyRepository iERPWorkCenterSkillCompetencyRepository = (base.ERPWorkCenterSkillCompetencyRepository = new ERPWorkCenterSkillCompetencyRepository(base.ApiClientContext));
			using (iERPWorkCenterSkillCompetencyRepository)
			{
				foreach (ERPWorkCenterSkillCompetencyInformationDto item2 in await base.ERPWorkCenterSkillCompetencyRepository.GetAllWorkCenterSkillCompetencies(pageSize, pageNumber, filter, orderBy))
				{
					ERPWorkCenterSkillCompetencyDto item = new ERPWorkCenterSkillCompetencyDto
					{
						xbbCommentsRTF = item2.xbbCommentsRTF,
						xbbCommentsText = item2.xbbCommentsText,
						xbbCompetencyID = item2.xbbCompetencyID,
						xbbCreatedBy = item2.xbbCreatedBy,
						xbbCreatedDate = item2.xbbCreatedDate,
						xbbDateAchieved = item2.xbbDateAchieved,
						xbbDateExpires = item2.xbbDateExpires,
						xbbUniqueID = item2.xbbUniqueID,
						xbbRowVersion = item2.xbbRowVersion,
						xbbWorkCenterSkillCompetencyID = item2.xbbWorkCenterSkillCompetencyID,
						xbbSkillID = item2.xbbSkillID,
						xbbWorkCenterID = item2.xbbWorkCenterID,
						xbbWorkCenterSkillID = item2.xbbWorkCenterSkillID,
						CustomFields = item2.CustomFields
					};
					allWorkCenterSkillCompetenciesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WorkCenterSkillCompetencies]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWorkCenterSkillCompetencyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWorkCenterSkillCompetenciesDto,
				RecordCount = allWorkCenterSkillCompetenciesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterSkillCompetencyDto>> Process_GetWorkCenterSkillCompetency(Guid workCenterSkillCompetencyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWorkCenterSkillCompetencyDto workCenterSkillCompetencyDto = null;
		ERPResponseMessageDto<ERPWorkCenterSkillCompetencyDto> result;
		try
		{
			IERPWorkCenterSkillCompetencyRepository iERPWorkCenterSkillCompetencyRepository = (base.ERPWorkCenterSkillCompetencyRepository = new ERPWorkCenterSkillCompetencyRepository(base.ApiClientContext));
			using (iERPWorkCenterSkillCompetencyRepository)
			{
				ERPWorkCenterSkillCompetencyInformationDto eRPWorkCenterSkillCompetencyInformationDto = await base.ERPWorkCenterSkillCompetencyRepository.GetWorkCenterSkillCompetency(workCenterSkillCompetencyId);
				workCenterSkillCompetencyDto = new ERPWorkCenterSkillCompetencyDto
				{
					xbbCommentsRTF = eRPWorkCenterSkillCompetencyInformationDto.xbbCommentsRTF,
					xbbCommentsText = eRPWorkCenterSkillCompetencyInformationDto.xbbCommentsText,
					xbbCompetencyID = eRPWorkCenterSkillCompetencyInformationDto.xbbCompetencyID,
					xbbCreatedBy = eRPWorkCenterSkillCompetencyInformationDto.xbbCreatedBy,
					xbbCreatedDate = eRPWorkCenterSkillCompetencyInformationDto.xbbCreatedDate,
					xbbDateAchieved = eRPWorkCenterSkillCompetencyInformationDto.xbbDateAchieved,
					xbbDateExpires = eRPWorkCenterSkillCompetencyInformationDto.xbbDateExpires,
					xbbUniqueID = eRPWorkCenterSkillCompetencyInformationDto.xbbUniqueID,
					xbbRowVersion = eRPWorkCenterSkillCompetencyInformationDto.xbbRowVersion,
					xbbWorkCenterSkillCompetencyID = eRPWorkCenterSkillCompetencyInformationDto.xbbWorkCenterSkillCompetencyID,
					xbbSkillID = eRPWorkCenterSkillCompetencyInformationDto.xbbSkillID,
					xbbWorkCenterID = eRPWorkCenterSkillCompetencyInformationDto.xbbWorkCenterID,
					xbbWorkCenterSkillID = eRPWorkCenterSkillCompetencyInformationDto.xbbWorkCenterSkillID,
					CustomFields = eRPWorkCenterSkillCompetencyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WorkCenterSkillCompetencies []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterSkillCompetencyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = workCenterSkillCompetencyDto
			};
		}
		return result;
	}
}
