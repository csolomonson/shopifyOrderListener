using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPJobPriorityModel : ERPBaseModel, IERPJobPriorityModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllJobPriorities(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobPriorityRepository iERPJobPriorityRepository = (base.ERPJobPriorityRepository = new ERPJobPriorityRepository(base.ApiClientContext));
		using (iERPJobPriorityRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPJobPriorityRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPJobPriorityRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPJobPriorityRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPJobPriorityRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJobPriority(Guid jobPriorityId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobPriorityRepository iERPJobPriorityRepository = (base.ERPJobPriorityRepository = new ERPJobPriorityRepository(base.ApiClientContext));
		using (iERPJobPriorityRepository)
		{
			if (!(await base.ERPJobPriorityRepository.DoesJobPriorityExist(jobPriorityId)))
			{
				errorsList.Add($"JobPriority [{jobPriorityId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutJobPriority(ERPJobPriorityDto jobPriority)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobPriorityRepository iERPJobPriorityRepository = (base.ERPJobPriorityRepository = new ERPJobPriorityRepository(base.ApiClientContext));
		using (iERPJobPriorityRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPJobPriorityDto>>> Process_GetAllJobPriorities(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPJobPriorityDto> allJobPrioritiesDto = new List<ERPJobPriorityDto>();
		ERPResponseMessageDto<IList<ERPJobPriorityDto>> result;
		try
		{
			IERPJobPriorityRepository iERPJobPriorityRepository = (base.ERPJobPriorityRepository = new ERPJobPriorityRepository(base.ApiClientContext));
			using (iERPJobPriorityRepository)
			{
				foreach (ERPJobPriorityInformationDto item2 in await base.ERPJobPriorityRepository.GetAllJobPriorities(pageSize, pageNumber, filter, orderBy))
				{
					ERPJobPriorityDto item = new ERPJobPriorityDto
					{
						jmjDescription = item2.jmjDescription,
						jmjUniqueID = item2.jmjUniqueID,
						jmjRowVersion = item2.jmjRowVersion,
						jmjJobPriorityID = item2.jmjJobPriorityID,
						CustomFields = item2.CustomFields
					};
					allJobPrioritiesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobPriorities]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPJobPriorityDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobPrioritiesDto,
				RecordCount = allJobPrioritiesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobPriorityDto>> Process_GetJobPriority(Guid jobPriorityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPJobPriorityDto jobPriorityDto = null;
		ERPResponseMessageDto<ERPJobPriorityDto> result;
		try
		{
			IERPJobPriorityRepository iERPJobPriorityRepository = (base.ERPJobPriorityRepository = new ERPJobPriorityRepository(base.ApiClientContext));
			using (iERPJobPriorityRepository)
			{
				ERPJobPriorityInformationDto eRPJobPriorityInformationDto = await base.ERPJobPriorityRepository.GetJobPriority(jobPriorityId);
				jobPriorityDto = new ERPJobPriorityDto
				{
					jmjDescription = eRPJobPriorityInformationDto.jmjDescription,
					jmjUniqueID = eRPJobPriorityInformationDto.jmjUniqueID,
					jmjRowVersion = eRPJobPriorityInformationDto.jmjRowVersion,
					jmjJobPriorityID = eRPJobPriorityInformationDto.jmjJobPriorityID,
					CustomFields = eRPJobPriorityInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the JobPriorities []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobPriorityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobPriorityDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobPriorityDto>> Process_PutJobPriority(ERPJobPriorityDto jobPriority)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPJobPriorityDto createdObject = null;
		ERPResponseMessageDto<ERPJobPriorityDto> result;
		try
		{
			IERPJobPriorityRepository iERPJobPriorityRepository = (base.ERPJobPriorityRepository = new ERPJobPriorityRepository(base.ApiClientContext));
			using (iERPJobPriorityRepository)
			{
				APIValidationInfoDto postResult = await base.ERPJobPriorityRepository.SaveJobPriority(jobPriority);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPJobPriorityInformationDto eRPJobPriorityInformationDto = await base.ERPJobPriorityRepository.GetJobPriority(jobPriority.jmjUniqueID);
					createdObject = new ERPJobPriorityDto
					{
						jmjDescription = eRPJobPriorityInformationDto.jmjDescription,
						jmjUniqueID = eRPJobPriorityInformationDto.jmjUniqueID,
						jmjRowVersion = eRPJobPriorityInformationDto.jmjRowVersion,
						jmjJobPriorityID = eRPJobPriorityInformationDto.jmjJobPriorityID,
						CustomFields = eRPJobPriorityInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing JobPriority [{jobPriority.jmjUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobPriorityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteJobPriority(Guid jobPriorityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobPriorityRepository iERPJobPriorityRepository = (base.ERPJobPriorityRepository = new ERPJobPriorityRepository(base.ApiClientContext));
		using (iERPJobPriorityRepository)
		{
			if (!(await base.ERPJobPriorityRepository.DoesJobPriorityExist(jobPriorityId)))
			{
				base.ErrorsList.Add($"JobPriority [{jobPriorityId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPJobPriorityInformationDto eRPJobPriorityInformationDto = await base.ERPJobPriorityRepository.GetJobPriority(jobPriorityId);
				string text = await base.ERPJobPriorityRepository.WhereUsed("JobPriorities", new object[1] { eRPJobPriorityInformationDto.jmjJobPriorityID }, new object[1] { "jmjJobPriorityID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("JobPriority cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPJobPriorityDto>> Process_DeleteJobPriority(Guid jobPriorityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPJobPriorityDto> result;
		try
		{
			IERPJobPriorityRepository iERPJobPriorityRepository = (base.ERPJobPriorityRepository = new ERPJobPriorityRepository(base.ApiClientContext));
			using (iERPJobPriorityRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPJobPriorityRepository.DeleteRowFromTable("JobPriorities", "jmj", jobPriorityId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of JobPriority [{jobPriorityId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobPriorityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPJobPriorityDto()
			};
		}
		return result;
	}
}
