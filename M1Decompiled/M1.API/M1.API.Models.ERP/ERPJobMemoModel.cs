using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPJobMemoModel : ERPBaseModel, IERPJobMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllJobMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobMemoRepository iERPJobMemoRepository = (base.ERPJobMemoRepository = new ERPJobMemoRepository(base.ApiClientContext));
		using (iERPJobMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPJobMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPJobMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPJobMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPJobMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJobMemo(Guid jobMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobMemoRepository iERPJobMemoRepository = (base.ERPJobMemoRepository = new ERPJobMemoRepository(base.ApiClientContext));
		using (iERPJobMemoRepository)
		{
			if (!(await base.ERPJobMemoRepository.DoesJobMemoExist(jobMemoId)))
			{
				errorsList.Add($"JobMemo [{jobMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutJobMemo(ERPJobMemoDto jobMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobMemoRepository iERPJobMemoRepository = (base.ERPJobMemoRepository = new ERPJobMemoRepository(base.ApiClientContext));
		using (iERPJobMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(jobMemo.jmkJobID) && !(await base.ERPJobMemoRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { jobMemo.jmkJobID })))
			{
				errorsList.Add("jmkJobID [" + jobMemo.jmkJobID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPJobMemoDto>>> Process_GetAllJobMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPJobMemoDto> allJobMemosDto = new List<ERPJobMemoDto>();
		ERPResponseMessageDto<IList<ERPJobMemoDto>> result;
		try
		{
			IERPJobMemoRepository iERPJobMemoRepository = (base.ERPJobMemoRepository = new ERPJobMemoRepository(base.ApiClientContext));
			using (iERPJobMemoRepository)
			{
				foreach (ERPJobMemoInformationDto item2 in await base.ERPJobMemoRepository.GetAllJobMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPJobMemoDto item = new ERPJobMemoDto
					{
						jmkCreatedBy = item2.jmkCreatedBy,
						jmkCreatedDate = item2.jmkCreatedDate,
						jmkUniqueID = item2.jmkUniqueID,
						jmkClosed = item2.jmkClosed,
						jmkJobID = item2.jmkJobID,
						jmkLongDescriptionRtf = item2.jmkLongDescriptionRtf,
						jmkLongDescriptionText = item2.jmkLongDescriptionText,
						jmkMemoDate = item2.jmkMemoDate,
						jmkRowVersion = item2.jmkRowVersion,
						jmkJobMemoID = item2.jmkJobMemoID,
						jmkShortDescription = item2.jmkShortDescription,
						jmkShowInJobs = item2.jmkShowInJobs,
						CustomFields = item2.CustomFields
					};
					allJobMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPJobMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobMemosDto,
				RecordCount = allJobMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobMemoDto>> Process_GetJobMemo(Guid jobMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPJobMemoDto jobMemoDto = null;
		ERPResponseMessageDto<ERPJobMemoDto> result;
		try
		{
			IERPJobMemoRepository iERPJobMemoRepository = (base.ERPJobMemoRepository = new ERPJobMemoRepository(base.ApiClientContext));
			using (iERPJobMemoRepository)
			{
				ERPJobMemoInformationDto eRPJobMemoInformationDto = await base.ERPJobMemoRepository.GetJobMemo(jobMemoId);
				jobMemoDto = new ERPJobMemoDto
				{
					jmkCreatedBy = eRPJobMemoInformationDto.jmkCreatedBy,
					jmkCreatedDate = eRPJobMemoInformationDto.jmkCreatedDate,
					jmkUniqueID = eRPJobMemoInformationDto.jmkUniqueID,
					jmkClosed = eRPJobMemoInformationDto.jmkClosed,
					jmkJobID = eRPJobMemoInformationDto.jmkJobID,
					jmkLongDescriptionRtf = eRPJobMemoInformationDto.jmkLongDescriptionRtf,
					jmkLongDescriptionText = eRPJobMemoInformationDto.jmkLongDescriptionText,
					jmkMemoDate = eRPJobMemoInformationDto.jmkMemoDate,
					jmkRowVersion = eRPJobMemoInformationDto.jmkRowVersion,
					jmkJobMemoID = eRPJobMemoInformationDto.jmkJobMemoID,
					jmkShortDescription = eRPJobMemoInformationDto.jmkShortDescription,
					jmkShowInJobs = eRPJobMemoInformationDto.jmkShowInJobs,
					CustomFields = eRPJobMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the JobMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobMemoDto>> Process_PutJobMemo(ERPJobMemoDto jobMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPJobMemoDto createdObject = null;
		ERPResponseMessageDto<ERPJobMemoDto> result;
		try
		{
			IERPJobMemoRepository iERPJobMemoRepository = (base.ERPJobMemoRepository = new ERPJobMemoRepository(base.ApiClientContext));
			using (iERPJobMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPJobMemoRepository.SaveJobMemo(jobMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPJobMemoInformationDto eRPJobMemoInformationDto = await base.ERPJobMemoRepository.GetJobMemo(jobMemo.jmkUniqueID);
					createdObject = new ERPJobMemoDto
					{
						jmkCreatedBy = eRPJobMemoInformationDto.jmkCreatedBy,
						jmkCreatedDate = eRPJobMemoInformationDto.jmkCreatedDate,
						jmkUniqueID = eRPJobMemoInformationDto.jmkUniqueID,
						jmkClosed = eRPJobMemoInformationDto.jmkClosed,
						jmkJobID = eRPJobMemoInformationDto.jmkJobID,
						jmkLongDescriptionRtf = eRPJobMemoInformationDto.jmkLongDescriptionRtf,
						jmkLongDescriptionText = eRPJobMemoInformationDto.jmkLongDescriptionText,
						jmkMemoDate = eRPJobMemoInformationDto.jmkMemoDate,
						jmkRowVersion = eRPJobMemoInformationDto.jmkRowVersion,
						jmkJobMemoID = eRPJobMemoInformationDto.jmkJobMemoID,
						jmkShortDescription = eRPJobMemoInformationDto.jmkShortDescription,
						jmkShowInJobs = eRPJobMemoInformationDto.jmkShowInJobs,
						CustomFields = eRPJobMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing JobMemo [{jobMemo.jmkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteJobMemo(Guid jobMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobMemoRepository iERPJobMemoRepository = (base.ERPJobMemoRepository = new ERPJobMemoRepository(base.ApiClientContext));
		using (iERPJobMemoRepository)
		{
			if (!(await base.ERPJobMemoRepository.DoesJobMemoExist(jobMemoId)))
			{
				base.ErrorsList.Add($"JobMemo [{jobMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPJobMemoInformationDto eRPJobMemoInformationDto = await base.ERPJobMemoRepository.GetJobMemo(jobMemoId);
				string text = await base.ERPJobMemoRepository.WhereUsed("JobMemos", new object[2] { eRPJobMemoInformationDto.jmkJobID, eRPJobMemoInformationDto.jmkJobMemoID }, new object[2] { "jmkJobID", "jmkJobMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("JobMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPJobMemoDto>> Process_DeleteJobMemo(Guid jobMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPJobMemoDto> result;
		try
		{
			IERPJobMemoRepository iERPJobMemoRepository = (base.ERPJobMemoRepository = new ERPJobMemoRepository(base.ApiClientContext));
			using (iERPJobMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPJobMemoRepository.DeleteRowFromTable("JobMemos", "jmk", jobMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of JobMemo [{jobMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPJobMemoDto()
			};
		}
		return result;
	}
}
