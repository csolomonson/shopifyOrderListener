using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPriorityModel : ERPBaseModel, IERPPriorityModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPriorities(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPriorityRepository iERPPriorityRepository = (base.ERPPriorityRepository = new ERPPriorityRepository(base.ApiClientContext));
		using (iERPPriorityRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPriorityRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPriorityRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPriorityRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPriorityRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPriority(Guid priorityId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPriorityRepository iERPPriorityRepository = (base.ERPPriorityRepository = new ERPPriorityRepository(base.ApiClientContext));
		using (iERPPriorityRepository)
		{
			if (!(await base.ERPPriorityRepository.DoesPriorityExist(priorityId)))
			{
				errorsList.Add($"Priority [{priorityId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPriority(ERPPriorityDto priority)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPriorityRepository iERPPriorityRepository = (base.ERPPriorityRepository = new ERPPriorityRepository(base.ApiClientContext));
		using (iERPPriorityRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPriorityDto>>> Process_GetAllPriorities(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPriorityDto> allPrioritiesDto = new List<ERPPriorityDto>();
		ERPResponseMessageDto<IList<ERPPriorityDto>> result;
		try
		{
			IERPPriorityRepository iERPPriorityRepository = (base.ERPPriorityRepository = new ERPPriorityRepository(base.ApiClientContext));
			using (iERPPriorityRepository)
			{
				foreach (ERPPriorityInformationDto item2 in await base.ERPPriorityRepository.GetAllPriorities(pageSize, pageNumber, filter, orderBy))
				{
					ERPPriorityDto item = new ERPPriorityDto
					{
						kbrCreatedBy = item2.kbrCreatedBy,
						kbrCreatedDate = item2.kbrCreatedDate,
						kbrDescription = item2.kbrDescription,
						kbrUniqueID = item2.kbrUniqueID,
						kbrRowVersion = item2.kbrRowVersion,
						kbrPriorityID = item2.kbrPriorityID,
						CustomFields = item2.CustomFields
					};
					allPrioritiesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Priorities]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPriorityDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPrioritiesDto,
				RecordCount = allPrioritiesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPriorityDto>> Process_GetPriority(Guid priorityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPriorityDto priorityDto = null;
		ERPResponseMessageDto<ERPPriorityDto> result;
		try
		{
			IERPPriorityRepository iERPPriorityRepository = (base.ERPPriorityRepository = new ERPPriorityRepository(base.ApiClientContext));
			using (iERPPriorityRepository)
			{
				ERPPriorityInformationDto eRPPriorityInformationDto = await base.ERPPriorityRepository.GetPriority(priorityId);
				priorityDto = new ERPPriorityDto
				{
					kbrCreatedBy = eRPPriorityInformationDto.kbrCreatedBy,
					kbrCreatedDate = eRPPriorityInformationDto.kbrCreatedDate,
					kbrDescription = eRPPriorityInformationDto.kbrDescription,
					kbrUniqueID = eRPPriorityInformationDto.kbrUniqueID,
					kbrRowVersion = eRPPriorityInformationDto.kbrRowVersion,
					kbrPriorityID = eRPPriorityInformationDto.kbrPriorityID,
					CustomFields = eRPPriorityInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Priorities []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPriorityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = priorityDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPriorityDto>> Process_PutPriority(ERPPriorityDto priority)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPriorityDto createdObject = null;
		ERPResponseMessageDto<ERPPriorityDto> result;
		try
		{
			IERPPriorityRepository iERPPriorityRepository = (base.ERPPriorityRepository = new ERPPriorityRepository(base.ApiClientContext));
			using (iERPPriorityRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPriorityRepository.SavePriority(priority);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPriorityInformationDto eRPPriorityInformationDto = await base.ERPPriorityRepository.GetPriority(priority.kbrUniqueID);
					createdObject = new ERPPriorityDto
					{
						kbrCreatedBy = eRPPriorityInformationDto.kbrCreatedBy,
						kbrCreatedDate = eRPPriorityInformationDto.kbrCreatedDate,
						kbrDescription = eRPPriorityInformationDto.kbrDescription,
						kbrUniqueID = eRPPriorityInformationDto.kbrUniqueID,
						kbrRowVersion = eRPPriorityInformationDto.kbrRowVersion,
						kbrPriorityID = eRPPriorityInformationDto.kbrPriorityID,
						CustomFields = eRPPriorityInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Priority [{priority.kbrUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPriorityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePriority(Guid priorityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPriorityRepository iERPPriorityRepository = (base.ERPPriorityRepository = new ERPPriorityRepository(base.ApiClientContext));
		using (iERPPriorityRepository)
		{
			if (!(await base.ERPPriorityRepository.DoesPriorityExist(priorityId)))
			{
				base.ErrorsList.Add($"Priority [{priorityId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPriorityInformationDto eRPPriorityInformationDto = await base.ERPPriorityRepository.GetPriority(priorityId);
				string text = await base.ERPPriorityRepository.WhereUsed("Priorities", new object[1] { eRPPriorityInformationDto.kbrPriorityID }, new object[1] { "kbrPriorityID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Priority cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPriorityDto>> Process_DeletePriority(Guid priorityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPriorityDto> result;
		try
		{
			IERPPriorityRepository iERPPriorityRepository = (base.ERPPriorityRepository = new ERPPriorityRepository(base.ApiClientContext));
			using (iERPPriorityRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPriorityRepository.DeleteRowFromTable("Priorities", "kbr", priorityId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Priority [{priorityId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPriorityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPriorityDto()
			};
		}
		return result;
	}
}
