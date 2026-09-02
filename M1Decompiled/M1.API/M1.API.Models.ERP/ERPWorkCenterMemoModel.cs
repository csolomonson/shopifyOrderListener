using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWorkCenterMemoModel : ERPBaseModel, IERPWorkCenterMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenterMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWorkCenterMemoRepository iERPWorkCenterMemoRepository = (base.ERPWorkCenterMemoRepository = new ERPWorkCenterMemoRepository(base.ApiClientContext));
		using (iERPWorkCenterMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWorkCenterMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWorkCenterMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWorkCenterMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWorkCenterMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWorkCenterMemo(Guid workCenterMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterMemoRepository iERPWorkCenterMemoRepository = (base.ERPWorkCenterMemoRepository = new ERPWorkCenterMemoRepository(base.ApiClientContext));
		using (iERPWorkCenterMemoRepository)
		{
			if (!(await base.ERPWorkCenterMemoRepository.DoesWorkCenterMemoExist(workCenterMemoId)))
			{
				errorsList.Add($"WorkCenterMemo [{workCenterMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWorkCenterMemo(ERPWorkCenterMemoDto workCenterMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterMemoRepository iERPWorkCenterMemoRepository = (base.ERPWorkCenterMemoRepository = new ERPWorkCenterMemoRepository(base.ApiClientContext));
		using (iERPWorkCenterMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(workCenterMemo.xakWorkCenterID) && !(await base.ERPWorkCenterMemoRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { workCenterMemo.xakWorkCenterID })))
			{
				errorsList.Add("xakWorkCenterID [" + workCenterMemo.xakWorkCenterID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWorkCenterMemoDto>>> Process_GetAllWorkCenterMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWorkCenterMemoDto> allWorkCenterMemosDto = new List<ERPWorkCenterMemoDto>();
		ERPResponseMessageDto<IList<ERPWorkCenterMemoDto>> result;
		try
		{
			IERPWorkCenterMemoRepository iERPWorkCenterMemoRepository = (base.ERPWorkCenterMemoRepository = new ERPWorkCenterMemoRepository(base.ApiClientContext));
			using (iERPWorkCenterMemoRepository)
			{
				foreach (ERPWorkCenterMemoInformationDto item2 in await base.ERPWorkCenterMemoRepository.GetAllWorkCenterMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPWorkCenterMemoDto item = new ERPWorkCenterMemoDto
					{
						xakCreatedBy = item2.xakCreatedBy,
						xakCreatedDate = item2.xakCreatedDate,
						xakUniqueID = item2.xakUniqueID,
						xakLongDescriptionRtf = item2.xakLongDescriptionRtf,
						xakLongDescriptionText = item2.xakLongDescriptionText,
						xakMemoDate = item2.xakMemoDate,
						xakRowVersion = item2.xakRowVersion,
						xakWorkCenterMemoID = item2.xakWorkCenterMemoID,
						xakShortDescription = item2.xakShortDescription,
						xakShowInJobs = item2.xakShowInJobs,
						xakShowInParts = item2.xakShowInParts,
						xakShowInQuotes = item2.xakShowInQuotes,
						xakShowInWorkCenters = item2.xakShowInWorkCenters,
						xakWorkCenterID = item2.xakWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allWorkCenterMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WorkCenterMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWorkCenterMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWorkCenterMemosDto,
				RecordCount = allWorkCenterMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterMemoDto>> Process_GetWorkCenterMemo(Guid workCenterMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWorkCenterMemoDto workCenterMemoDto = null;
		ERPResponseMessageDto<ERPWorkCenterMemoDto> result;
		try
		{
			IERPWorkCenterMemoRepository iERPWorkCenterMemoRepository = (base.ERPWorkCenterMemoRepository = new ERPWorkCenterMemoRepository(base.ApiClientContext));
			using (iERPWorkCenterMemoRepository)
			{
				ERPWorkCenterMemoInformationDto eRPWorkCenterMemoInformationDto = await base.ERPWorkCenterMemoRepository.GetWorkCenterMemo(workCenterMemoId);
				workCenterMemoDto = new ERPWorkCenterMemoDto
				{
					xakCreatedBy = eRPWorkCenterMemoInformationDto.xakCreatedBy,
					xakCreatedDate = eRPWorkCenterMemoInformationDto.xakCreatedDate,
					xakUniqueID = eRPWorkCenterMemoInformationDto.xakUniqueID,
					xakLongDescriptionRtf = eRPWorkCenterMemoInformationDto.xakLongDescriptionRtf,
					xakLongDescriptionText = eRPWorkCenterMemoInformationDto.xakLongDescriptionText,
					xakMemoDate = eRPWorkCenterMemoInformationDto.xakMemoDate,
					xakRowVersion = eRPWorkCenterMemoInformationDto.xakRowVersion,
					xakWorkCenterMemoID = eRPWorkCenterMemoInformationDto.xakWorkCenterMemoID,
					xakShortDescription = eRPWorkCenterMemoInformationDto.xakShortDescription,
					xakShowInJobs = eRPWorkCenterMemoInformationDto.xakShowInJobs,
					xakShowInParts = eRPWorkCenterMemoInformationDto.xakShowInParts,
					xakShowInQuotes = eRPWorkCenterMemoInformationDto.xakShowInQuotes,
					xakShowInWorkCenters = eRPWorkCenterMemoInformationDto.xakShowInWorkCenters,
					xakWorkCenterID = eRPWorkCenterMemoInformationDto.xakWorkCenterID,
					CustomFields = eRPWorkCenterMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WorkCenterMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = workCenterMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterMemoDto>> Process_PutWorkCenterMemo(ERPWorkCenterMemoDto workCenterMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWorkCenterMemoDto createdObject = null;
		ERPResponseMessageDto<ERPWorkCenterMemoDto> result;
		try
		{
			IERPWorkCenterMemoRepository iERPWorkCenterMemoRepository = (base.ERPWorkCenterMemoRepository = new ERPWorkCenterMemoRepository(base.ApiClientContext));
			using (iERPWorkCenterMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWorkCenterMemoRepository.SaveWorkCenterMemo(workCenterMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWorkCenterMemoInformationDto eRPWorkCenterMemoInformationDto = await base.ERPWorkCenterMemoRepository.GetWorkCenterMemo(workCenterMemo.xakUniqueID);
					createdObject = new ERPWorkCenterMemoDto
					{
						xakCreatedBy = eRPWorkCenterMemoInformationDto.xakCreatedBy,
						xakCreatedDate = eRPWorkCenterMemoInformationDto.xakCreatedDate,
						xakUniqueID = eRPWorkCenterMemoInformationDto.xakUniqueID,
						xakLongDescriptionRtf = eRPWorkCenterMemoInformationDto.xakLongDescriptionRtf,
						xakLongDescriptionText = eRPWorkCenterMemoInformationDto.xakLongDescriptionText,
						xakMemoDate = eRPWorkCenterMemoInformationDto.xakMemoDate,
						xakRowVersion = eRPWorkCenterMemoInformationDto.xakRowVersion,
						xakWorkCenterMemoID = eRPWorkCenterMemoInformationDto.xakWorkCenterMemoID,
						xakShortDescription = eRPWorkCenterMemoInformationDto.xakShortDescription,
						xakShowInJobs = eRPWorkCenterMemoInformationDto.xakShowInJobs,
						xakShowInParts = eRPWorkCenterMemoInformationDto.xakShowInParts,
						xakShowInQuotes = eRPWorkCenterMemoInformationDto.xakShowInQuotes,
						xakShowInWorkCenters = eRPWorkCenterMemoInformationDto.xakShowInWorkCenters,
						xakWorkCenterID = eRPWorkCenterMemoInformationDto.xakWorkCenterID,
						CustomFields = eRPWorkCenterMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WorkCenterMemo [{workCenterMemo.xakUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWorkCenterMemo(Guid workCenterMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterMemoRepository iERPWorkCenterMemoRepository = (base.ERPWorkCenterMemoRepository = new ERPWorkCenterMemoRepository(base.ApiClientContext));
		using (iERPWorkCenterMemoRepository)
		{
			if (!(await base.ERPWorkCenterMemoRepository.DoesWorkCenterMemoExist(workCenterMemoId)))
			{
				base.ErrorsList.Add($"WorkCenterMemo [{workCenterMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWorkCenterMemoInformationDto eRPWorkCenterMemoInformationDto = await base.ERPWorkCenterMemoRepository.GetWorkCenterMemo(workCenterMemoId);
				string text = await base.ERPWorkCenterMemoRepository.WhereUsed("WorkCenterMemos", new object[2] { eRPWorkCenterMemoInformationDto.xakWorkCenterID, eRPWorkCenterMemoInformationDto.xakWorkCenterMemoID }, new object[2] { "xakWorkCenterID", "xakWorkCenterMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WorkCenterMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterMemoDto>> Process_DeleteWorkCenterMemo(Guid workCenterMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWorkCenterMemoDto> result;
		try
		{
			IERPWorkCenterMemoRepository iERPWorkCenterMemoRepository = (base.ERPWorkCenterMemoRepository = new ERPWorkCenterMemoRepository(base.ApiClientContext));
			using (iERPWorkCenterMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWorkCenterMemoRepository.DeleteRowFromTable("WorkCenterMemos", "xak", workCenterMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WorkCenterMemo [{workCenterMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWorkCenterMemoDto()
			};
		}
		return result;
	}
}
