using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCallMemoModel : ERPBaseModel, IERPCallMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCallMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCallMemoRepository iERPCallMemoRepository = (base.ERPCallMemoRepository = new ERPCallMemoRepository(base.ApiClientContext));
		using (iERPCallMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCallMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCallMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCallMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCallMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCallMemo(Guid callMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallMemoRepository iERPCallMemoRepository = (base.ERPCallMemoRepository = new ERPCallMemoRepository(base.ApiClientContext));
		using (iERPCallMemoRepository)
		{
			if (!(await base.ERPCallMemoRepository.DoesCallMemoExist(callMemoId)))
			{
				errorsList.Add($"CallMemo [{callMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCallMemo(ERPCallMemoDto callMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallMemoRepository iERPCallMemoRepository = (base.ERPCallMemoRepository = new ERPCallMemoRepository(base.ApiClientContext));
		using (iERPCallMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(callMemo.kbkCallID) && !(await base.ERPCallMemoRepository.DoesRecordExistInTableUsingKeys("Calls", new object[1] { "KBPCALLID" }, new object[1] { callMemo.kbkCallID })))
			{
				errorsList.Add("kbkCallID [" + callMemo.kbkCallID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCallMemoDto>>> Process_GetAllCallMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCallMemoDto> allCallMemosDto = new List<ERPCallMemoDto>();
		ERPResponseMessageDto<IList<ERPCallMemoDto>> result;
		try
		{
			IERPCallMemoRepository iERPCallMemoRepository = (base.ERPCallMemoRepository = new ERPCallMemoRepository(base.ApiClientContext));
			using (iERPCallMemoRepository)
			{
				foreach (ERPCallMemoInformationDto item2 in await base.ERPCallMemoRepository.GetAllCallMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPCallMemoDto item = new ERPCallMemoDto
					{
						kbkCallID = item2.kbkCallID,
						kbkCreatedBy = item2.kbkCreatedBy,
						kbkCreatedDate = item2.kbkCreatedDate,
						kbkUniqueID = item2.kbkUniqueID,
						kbkLongDescriptionRtf = item2.kbkLongDescriptionRtf,
						kbkLongDescriptionText = item2.kbkLongDescriptionText,
						kbkMemoDate = item2.kbkMemoDate,
						kbkRowVersion = item2.kbkRowVersion,
						kbkCallMemoID = item2.kbkCallMemoID,
						kbkShortDescription = item2.kbkShortDescription,
						kbkShowInCalls = item2.kbkShowInCalls,
						CustomFields = item2.CustomFields
					};
					allCallMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CallMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCallMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCallMemosDto,
				RecordCount = allCallMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCallMemoDto>> Process_GetCallMemo(Guid callMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCallMemoDto callMemoDto = null;
		ERPResponseMessageDto<ERPCallMemoDto> result;
		try
		{
			IERPCallMemoRepository iERPCallMemoRepository = (base.ERPCallMemoRepository = new ERPCallMemoRepository(base.ApiClientContext));
			using (iERPCallMemoRepository)
			{
				ERPCallMemoInformationDto eRPCallMemoInformationDto = await base.ERPCallMemoRepository.GetCallMemo(callMemoId);
				callMemoDto = new ERPCallMemoDto
				{
					kbkCallID = eRPCallMemoInformationDto.kbkCallID,
					kbkCreatedBy = eRPCallMemoInformationDto.kbkCreatedBy,
					kbkCreatedDate = eRPCallMemoInformationDto.kbkCreatedDate,
					kbkUniqueID = eRPCallMemoInformationDto.kbkUniqueID,
					kbkLongDescriptionRtf = eRPCallMemoInformationDto.kbkLongDescriptionRtf,
					kbkLongDescriptionText = eRPCallMemoInformationDto.kbkLongDescriptionText,
					kbkMemoDate = eRPCallMemoInformationDto.kbkMemoDate,
					kbkRowVersion = eRPCallMemoInformationDto.kbkRowVersion,
					kbkCallMemoID = eRPCallMemoInformationDto.kbkCallMemoID,
					kbkShortDescription = eRPCallMemoInformationDto.kbkShortDescription,
					kbkShowInCalls = eRPCallMemoInformationDto.kbkShowInCalls,
					CustomFields = eRPCallMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CallMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = callMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCallMemoDto>> Process_PutCallMemo(ERPCallMemoDto callMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCallMemoDto createdObject = null;
		ERPResponseMessageDto<ERPCallMemoDto> result;
		try
		{
			IERPCallMemoRepository iERPCallMemoRepository = (base.ERPCallMemoRepository = new ERPCallMemoRepository(base.ApiClientContext));
			using (iERPCallMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCallMemoRepository.SaveCallMemo(callMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCallMemoInformationDto eRPCallMemoInformationDto = await base.ERPCallMemoRepository.GetCallMemo(callMemo.kbkUniqueID);
					createdObject = new ERPCallMemoDto
					{
						kbkCallID = eRPCallMemoInformationDto.kbkCallID,
						kbkCreatedBy = eRPCallMemoInformationDto.kbkCreatedBy,
						kbkCreatedDate = eRPCallMemoInformationDto.kbkCreatedDate,
						kbkUniqueID = eRPCallMemoInformationDto.kbkUniqueID,
						kbkLongDescriptionRtf = eRPCallMemoInformationDto.kbkLongDescriptionRtf,
						kbkLongDescriptionText = eRPCallMemoInformationDto.kbkLongDescriptionText,
						kbkMemoDate = eRPCallMemoInformationDto.kbkMemoDate,
						kbkRowVersion = eRPCallMemoInformationDto.kbkRowVersion,
						kbkCallMemoID = eRPCallMemoInformationDto.kbkCallMemoID,
						kbkShortDescription = eRPCallMemoInformationDto.kbkShortDescription,
						kbkShowInCalls = eRPCallMemoInformationDto.kbkShowInCalls,
						CustomFields = eRPCallMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CallMemo [{callMemo.kbkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCallMemo(Guid callMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallMemoRepository iERPCallMemoRepository = (base.ERPCallMemoRepository = new ERPCallMemoRepository(base.ApiClientContext));
		using (iERPCallMemoRepository)
		{
			if (!(await base.ERPCallMemoRepository.DoesCallMemoExist(callMemoId)))
			{
				base.ErrorsList.Add($"CallMemo [{callMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCallMemoInformationDto eRPCallMemoInformationDto = await base.ERPCallMemoRepository.GetCallMemo(callMemoId);
				string text = await base.ERPCallMemoRepository.WhereUsed("CallMemos", new object[2] { eRPCallMemoInformationDto.kbkCallID, eRPCallMemoInformationDto.kbkCallMemoID }, new object[2] { "kbkCallID", "kbkCallMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CallMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCallMemoDto>> Process_DeleteCallMemo(Guid callMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCallMemoDto> result;
		try
		{
			IERPCallMemoRepository iERPCallMemoRepository = (base.ERPCallMemoRepository = new ERPCallMemoRepository(base.ApiClientContext));
			using (iERPCallMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCallMemoRepository.DeleteRowFromTable("CallMemos", "kbk", callMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CallMemo [{callMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCallMemoDto()
			};
		}
		return result;
	}
}
