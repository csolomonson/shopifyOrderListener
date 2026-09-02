using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMilestoneModel : ERPBaseModel, IERPMilestoneModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMilestones(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMilestoneRepository iERPMilestoneRepository = (base.ERPMilestoneRepository = new ERPMilestoneRepository(base.ApiClientContext));
		using (iERPMilestoneRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMilestoneRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMilestoneRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMilestoneRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMilestoneRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMilestone(Guid milestoneId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMilestoneRepository iERPMilestoneRepository = (base.ERPMilestoneRepository = new ERPMilestoneRepository(base.ApiClientContext));
		using (iERPMilestoneRepository)
		{
			if (!(await base.ERPMilestoneRepository.DoesMilestoneExist(milestoneId)))
			{
				errorsList.Add($"Milestone [{milestoneId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMilestone(ERPMilestoneDto milestone)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMilestoneRepository iERPMilestoneRepository = (base.ERPMilestoneRepository = new ERPMilestoneRepository(base.ApiClientContext));
		using (iERPMilestoneRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMilestoneDto>>> Process_GetAllMilestones(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMilestoneDto> allMilestonesDto = new List<ERPMilestoneDto>();
		ERPResponseMessageDto<IList<ERPMilestoneDto>> result;
		try
		{
			IERPMilestoneRepository iERPMilestoneRepository = (base.ERPMilestoneRepository = new ERPMilestoneRepository(base.ApiClientContext));
			using (iERPMilestoneRepository)
			{
				foreach (ERPMilestoneInformationDto item2 in await base.ERPMilestoneRepository.GetAllMilestones(pageSize, pageNumber, filter, orderBy))
				{
					ERPMilestoneDto item = new ERPMilestoneDto
					{
						losMilestoneID = item2.losMilestoneID,
						losConfidenceFactor = item2.losConfidenceFactor,
						losCreatedBy = item2.losCreatedBy,
						losCreatedDate = item2.losCreatedDate,
						losUniqueID = item2.losUniqueID,
						losLongDescriptionRtf = item2.losLongDescriptionRtf,
						losLongDescriptionText = item2.losLongDescriptionText,
						losRowVersion = item2.losRowVersion,
						losShortDescription = item2.losShortDescription,
						CustomFields = item2.CustomFields
					};
					allMilestonesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Milestones]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMilestoneDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMilestonesDto,
				RecordCount = allMilestonesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMilestoneDto>> Process_GetMilestone(Guid milestoneId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMilestoneDto milestoneDto = null;
		ERPResponseMessageDto<ERPMilestoneDto> result;
		try
		{
			IERPMilestoneRepository iERPMilestoneRepository = (base.ERPMilestoneRepository = new ERPMilestoneRepository(base.ApiClientContext));
			using (iERPMilestoneRepository)
			{
				ERPMilestoneInformationDto eRPMilestoneInformationDto = await base.ERPMilestoneRepository.GetMilestone(milestoneId);
				milestoneDto = new ERPMilestoneDto
				{
					losMilestoneID = eRPMilestoneInformationDto.losMilestoneID,
					losConfidenceFactor = eRPMilestoneInformationDto.losConfidenceFactor,
					losCreatedBy = eRPMilestoneInformationDto.losCreatedBy,
					losCreatedDate = eRPMilestoneInformationDto.losCreatedDate,
					losUniqueID = eRPMilestoneInformationDto.losUniqueID,
					losLongDescriptionRtf = eRPMilestoneInformationDto.losLongDescriptionRtf,
					losLongDescriptionText = eRPMilestoneInformationDto.losLongDescriptionText,
					losRowVersion = eRPMilestoneInformationDto.losRowVersion,
					losShortDescription = eRPMilestoneInformationDto.losShortDescription,
					CustomFields = eRPMilestoneInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Milestones []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMilestoneDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = milestoneDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMilestoneDto>> Process_PutMilestone(ERPMilestoneDto milestone)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMilestoneDto createdObject = null;
		ERPResponseMessageDto<ERPMilestoneDto> result;
		try
		{
			IERPMilestoneRepository iERPMilestoneRepository = (base.ERPMilestoneRepository = new ERPMilestoneRepository(base.ApiClientContext));
			using (iERPMilestoneRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMilestoneRepository.SaveMilestone(milestone);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMilestoneInformationDto eRPMilestoneInformationDto = await base.ERPMilestoneRepository.GetMilestone(milestone.losUniqueID);
					createdObject = new ERPMilestoneDto
					{
						losMilestoneID = eRPMilestoneInformationDto.losMilestoneID,
						losConfidenceFactor = eRPMilestoneInformationDto.losConfidenceFactor,
						losCreatedBy = eRPMilestoneInformationDto.losCreatedBy,
						losCreatedDate = eRPMilestoneInformationDto.losCreatedDate,
						losUniqueID = eRPMilestoneInformationDto.losUniqueID,
						losLongDescriptionRtf = eRPMilestoneInformationDto.losLongDescriptionRtf,
						losLongDescriptionText = eRPMilestoneInformationDto.losLongDescriptionText,
						losRowVersion = eRPMilestoneInformationDto.losRowVersion,
						losShortDescription = eRPMilestoneInformationDto.losShortDescription,
						CustomFields = eRPMilestoneInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Milestone [{milestone.losUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMilestoneDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMilestone(Guid milestoneId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMilestoneRepository iERPMilestoneRepository = (base.ERPMilestoneRepository = new ERPMilestoneRepository(base.ApiClientContext));
		using (iERPMilestoneRepository)
		{
			if (!(await base.ERPMilestoneRepository.DoesMilestoneExist(milestoneId)))
			{
				base.ErrorsList.Add($"Milestone [{milestoneId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMilestoneInformationDto eRPMilestoneInformationDto = await base.ERPMilestoneRepository.GetMilestone(milestoneId);
				string text = await base.ERPMilestoneRepository.WhereUsed("Milestones", new object[1] { eRPMilestoneInformationDto.losMilestoneID }, new object[1] { "losMilestoneID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Milestone cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMilestoneDto>> Process_DeleteMilestone(Guid milestoneId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMilestoneDto> result;
		try
		{
			IERPMilestoneRepository iERPMilestoneRepository = (base.ERPMilestoneRepository = new ERPMilestoneRepository(base.ApiClientContext));
			using (iERPMilestoneRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMilestoneRepository.DeleteRowFromTable("Milestones", "los", milestoneId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Milestone [{milestoneId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMilestoneDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMilestoneDto()
			};
		}
		return result;
	}
}
