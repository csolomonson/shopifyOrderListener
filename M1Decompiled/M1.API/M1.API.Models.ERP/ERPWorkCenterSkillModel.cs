using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWorkCenterSkillModel : ERPBaseModel, IERPWorkCenterSkillModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenterSkills(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWorkCenterSkillRepository iERPWorkCenterSkillRepository = (base.ERPWorkCenterSkillRepository = new ERPWorkCenterSkillRepository(base.ApiClientContext));
		using (iERPWorkCenterSkillRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWorkCenterSkillRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWorkCenterSkillRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWorkCenterSkillRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWorkCenterSkillRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWorkCenterSkill(Guid workCenterSkillId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterSkillRepository iERPWorkCenterSkillRepository = (base.ERPWorkCenterSkillRepository = new ERPWorkCenterSkillRepository(base.ApiClientContext));
		using (iERPWorkCenterSkillRepository)
		{
			if (!(await base.ERPWorkCenterSkillRepository.DoesWorkCenterSkillExist(workCenterSkillId)))
			{
				errorsList.Add($"WorkCenterSkill [{workCenterSkillId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWorkCenterSkillDto>>> Process_GetAllWorkCenterSkills(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWorkCenterSkillDto> allWorkCenterSkillsDto = new List<ERPWorkCenterSkillDto>();
		ERPResponseMessageDto<IList<ERPWorkCenterSkillDto>> result;
		try
		{
			IERPWorkCenterSkillRepository iERPWorkCenterSkillRepository = (base.ERPWorkCenterSkillRepository = new ERPWorkCenterSkillRepository(base.ApiClientContext));
			using (iERPWorkCenterSkillRepository)
			{
				foreach (ERPWorkCenterSkillInformationDto item2 in await base.ERPWorkCenterSkillRepository.GetAllWorkCenterSkills(pageSize, pageNumber, filter, orderBy))
				{
					ERPWorkCenterSkillDto item = new ERPWorkCenterSkillDto
					{
						xbaCreatedBy = item2.xbaCreatedBy,
						xbaCreatedDate = item2.xbaCreatedDate,
						xbaDocuments = item2.xbaDocuments,
						xbaUniqueID = item2.xbaUniqueID,
						xbaNotesRTF = item2.xbaNotesRTF,
						xbaNotesText = item2.xbaNotesText,
						xbaRowVersion = item2.xbaRowVersion,
						xbaWorkCenterSkillID = item2.xbaWorkCenterSkillID,
						xbaSkillID = item2.xbaSkillID,
						xbaWorkCenterID = item2.xbaWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allWorkCenterSkillsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WorkCenterSkills]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWorkCenterSkillDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWorkCenterSkillsDto,
				RecordCount = allWorkCenterSkillsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterSkillDto>> Process_GetWorkCenterSkill(Guid workCenterSkillId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWorkCenterSkillDto workCenterSkillDto = null;
		ERPResponseMessageDto<ERPWorkCenterSkillDto> result;
		try
		{
			IERPWorkCenterSkillRepository iERPWorkCenterSkillRepository = (base.ERPWorkCenterSkillRepository = new ERPWorkCenterSkillRepository(base.ApiClientContext));
			using (iERPWorkCenterSkillRepository)
			{
				ERPWorkCenterSkillInformationDto eRPWorkCenterSkillInformationDto = await base.ERPWorkCenterSkillRepository.GetWorkCenterSkill(workCenterSkillId);
				workCenterSkillDto = new ERPWorkCenterSkillDto
				{
					xbaCreatedBy = eRPWorkCenterSkillInformationDto.xbaCreatedBy,
					xbaCreatedDate = eRPWorkCenterSkillInformationDto.xbaCreatedDate,
					xbaDocuments = eRPWorkCenterSkillInformationDto.xbaDocuments,
					xbaUniqueID = eRPWorkCenterSkillInformationDto.xbaUniqueID,
					xbaNotesRTF = eRPWorkCenterSkillInformationDto.xbaNotesRTF,
					xbaNotesText = eRPWorkCenterSkillInformationDto.xbaNotesText,
					xbaRowVersion = eRPWorkCenterSkillInformationDto.xbaRowVersion,
					xbaWorkCenterSkillID = eRPWorkCenterSkillInformationDto.xbaWorkCenterSkillID,
					xbaSkillID = eRPWorkCenterSkillInformationDto.xbaSkillID,
					xbaWorkCenterID = eRPWorkCenterSkillInformationDto.xbaWorkCenterID,
					CustomFields = eRPWorkCenterSkillInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WorkCenterSkills []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterSkillDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = workCenterSkillDto
			};
		}
		return result;
	}
}
