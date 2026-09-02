using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLeadMemoModel : ERPBaseModel, IERPLeadMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLeadMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLeadMemoRepository iERPLeadMemoRepository = (base.ERPLeadMemoRepository = new ERPLeadMemoRepository(base.ApiClientContext));
		using (iERPLeadMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLeadMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLeadMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLeadMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLeadMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLeadMemo(Guid leadMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadMemoRepository iERPLeadMemoRepository = (base.ERPLeadMemoRepository = new ERPLeadMemoRepository(base.ApiClientContext));
		using (iERPLeadMemoRepository)
		{
			if (!(await base.ERPLeadMemoRepository.DoesLeadMemoExist(leadMemoId)))
			{
				errorsList.Add($"LeadMemo [{leadMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLeadMemo(ERPLeadMemoDto leadMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadMemoRepository iERPLeadMemoRepository = (base.ERPLeadMemoRepository = new ERPLeadMemoRepository(base.ApiClientContext));
		using (iERPLeadMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(leadMemo.lokLeadID) && !(await base.ERPLeadMemoRepository.DoesRecordExistInTableUsingKeys("Leads", new object[1] { "LOPLEADID" }, new object[1] { leadMemo.lokLeadID })))
			{
				errorsList.Add("lokLeadID [" + leadMemo.lokLeadID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLeadMemoDto>>> Process_GetAllLeadMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLeadMemoDto> allLeadMemosDto = new List<ERPLeadMemoDto>();
		ERPResponseMessageDto<IList<ERPLeadMemoDto>> result;
		try
		{
			IERPLeadMemoRepository iERPLeadMemoRepository = (base.ERPLeadMemoRepository = new ERPLeadMemoRepository(base.ApiClientContext));
			using (iERPLeadMemoRepository)
			{
				foreach (ERPLeadMemoInformationDto item2 in await base.ERPLeadMemoRepository.GetAllLeadMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPLeadMemoDto item = new ERPLeadMemoDto
					{
						lokCreatedBy = item2.lokCreatedBy,
						lokCreatedDate = item2.lokCreatedDate,
						lokUniqueID = item2.lokUniqueID,
						lokLeadID = item2.lokLeadID,
						lokLongDescriptionRtf = item2.lokLongDescriptionRtf,
						lokLongDescriptionText = item2.lokLongDescriptionText,
						lokMemoDate = item2.lokMemoDate,
						lokRowVersion = item2.lokRowVersion,
						lokLeadMemoID = item2.lokLeadMemoID,
						lokShortDescription = item2.lokShortDescription,
						CustomFields = item2.CustomFields
					};
					allLeadMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LeadMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLeadMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLeadMemosDto,
				RecordCount = allLeadMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadMemoDto>> Process_GetLeadMemo(Guid leadMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLeadMemoDto leadMemoDto = null;
		ERPResponseMessageDto<ERPLeadMemoDto> result;
		try
		{
			IERPLeadMemoRepository iERPLeadMemoRepository = (base.ERPLeadMemoRepository = new ERPLeadMemoRepository(base.ApiClientContext));
			using (iERPLeadMemoRepository)
			{
				ERPLeadMemoInformationDto eRPLeadMemoInformationDto = await base.ERPLeadMemoRepository.GetLeadMemo(leadMemoId);
				leadMemoDto = new ERPLeadMemoDto
				{
					lokCreatedBy = eRPLeadMemoInformationDto.lokCreatedBy,
					lokCreatedDate = eRPLeadMemoInformationDto.lokCreatedDate,
					lokUniqueID = eRPLeadMemoInformationDto.lokUniqueID,
					lokLeadID = eRPLeadMemoInformationDto.lokLeadID,
					lokLongDescriptionRtf = eRPLeadMemoInformationDto.lokLongDescriptionRtf,
					lokLongDescriptionText = eRPLeadMemoInformationDto.lokLongDescriptionText,
					lokMemoDate = eRPLeadMemoInformationDto.lokMemoDate,
					lokRowVersion = eRPLeadMemoInformationDto.lokRowVersion,
					lokLeadMemoID = eRPLeadMemoInformationDto.lokLeadMemoID,
					lokShortDescription = eRPLeadMemoInformationDto.lokShortDescription,
					CustomFields = eRPLeadMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LeadMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = leadMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadMemoDto>> Process_PutLeadMemo(ERPLeadMemoDto leadMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLeadMemoDto createdObject = null;
		ERPResponseMessageDto<ERPLeadMemoDto> result;
		try
		{
			IERPLeadMemoRepository iERPLeadMemoRepository = (base.ERPLeadMemoRepository = new ERPLeadMemoRepository(base.ApiClientContext));
			using (iERPLeadMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLeadMemoRepository.SaveLeadMemo(leadMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLeadMemoInformationDto eRPLeadMemoInformationDto = await base.ERPLeadMemoRepository.GetLeadMemo(leadMemo.lokUniqueID);
					createdObject = new ERPLeadMemoDto
					{
						lokCreatedBy = eRPLeadMemoInformationDto.lokCreatedBy,
						lokCreatedDate = eRPLeadMemoInformationDto.lokCreatedDate,
						lokUniqueID = eRPLeadMemoInformationDto.lokUniqueID,
						lokLeadID = eRPLeadMemoInformationDto.lokLeadID,
						lokLongDescriptionRtf = eRPLeadMemoInformationDto.lokLongDescriptionRtf,
						lokLongDescriptionText = eRPLeadMemoInformationDto.lokLongDescriptionText,
						lokMemoDate = eRPLeadMemoInformationDto.lokMemoDate,
						lokRowVersion = eRPLeadMemoInformationDto.lokRowVersion,
						lokLeadMemoID = eRPLeadMemoInformationDto.lokLeadMemoID,
						lokShortDescription = eRPLeadMemoInformationDto.lokShortDescription,
						CustomFields = eRPLeadMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LeadMemo [{leadMemo.lokUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLeadMemo(Guid leadMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadMemoRepository iERPLeadMemoRepository = (base.ERPLeadMemoRepository = new ERPLeadMemoRepository(base.ApiClientContext));
		using (iERPLeadMemoRepository)
		{
			if (!(await base.ERPLeadMemoRepository.DoesLeadMemoExist(leadMemoId)))
			{
				base.ErrorsList.Add($"LeadMemo [{leadMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLeadMemoInformationDto eRPLeadMemoInformationDto = await base.ERPLeadMemoRepository.GetLeadMemo(leadMemoId);
				string text = await base.ERPLeadMemoRepository.WhereUsed("LeadMemos", new object[2] { eRPLeadMemoInformationDto.lokLeadID, eRPLeadMemoInformationDto.lokLeadMemoID }, new object[2] { "lokLeadID", "lokLeadMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LeadMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLeadMemoDto>> Process_DeleteLeadMemo(Guid leadMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLeadMemoDto> result;
		try
		{
			IERPLeadMemoRepository iERPLeadMemoRepository = (base.ERPLeadMemoRepository = new ERPLeadMemoRepository(base.ApiClientContext));
			using (iERPLeadMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLeadMemoRepository.DeleteRowFromTable("LeadMemos", "lok", leadMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LeadMemo [{leadMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLeadMemoDto()
			};
		}
		return result;
	}
}
