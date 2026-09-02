using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAttachmentMemoModel : ERPBaseModel, IERPAttachmentMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAttachmentMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAttachmentMemoRepository iERPAttachmentMemoRepository = (base.ERPAttachmentMemoRepository = new ERPAttachmentMemoRepository(base.ApiClientContext));
		using (iERPAttachmentMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAttachmentMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAttachmentMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAttachmentMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAttachmentMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAttachmentMemo(Guid attachmentMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAttachmentMemoRepository iERPAttachmentMemoRepository = (base.ERPAttachmentMemoRepository = new ERPAttachmentMemoRepository(base.ApiClientContext));
		using (iERPAttachmentMemoRepository)
		{
			if (!(await base.ERPAttachmentMemoRepository.DoesAttachmentMemoExist(attachmentMemoId)))
			{
				errorsList.Add($"AttachmentMemo [{attachmentMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAttachmentMemo(ERPAttachmentMemoDto attachmentMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAttachmentMemoRepository iERPAttachmentMemoRepository = (base.ERPAttachmentMemoRepository = new ERPAttachmentMemoRepository(base.ApiClientContext));
		using (iERPAttachmentMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(attachmentMemo.cmqAttachmentID) && !(await base.ERPAttachmentMemoRepository.DoesRecordExistInTableUsingKeys("Attachments", new object[1] { "CMAATTACHMENTID" }, new object[1] { attachmentMemo.cmqAttachmentID })))
			{
				errorsList.Add("cmqAttachmentID [" + attachmentMemo.cmqAttachmentID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAttachmentMemoDto>>> Process_GetAllAttachmentMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAttachmentMemoDto> allAttachmentMemosDto = new List<ERPAttachmentMemoDto>();
		ERPResponseMessageDto<IList<ERPAttachmentMemoDto>> result;
		try
		{
			IERPAttachmentMemoRepository iERPAttachmentMemoRepository = (base.ERPAttachmentMemoRepository = new ERPAttachmentMemoRepository(base.ApiClientContext));
			using (iERPAttachmentMemoRepository)
			{
				foreach (ERPAttachmentMemoInformationDto item2 in await base.ERPAttachmentMemoRepository.GetAllAttachmentMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPAttachmentMemoDto item = new ERPAttachmentMemoDto
					{
						cmqAttachmentID = item2.cmqAttachmentID,
						cmqCreatedBy = item2.cmqCreatedBy,
						cmqCreatedDate = item2.cmqCreatedDate,
						cmqUniqueID = item2.cmqUniqueID,
						cmqLongDescriptionRtf = item2.cmqLongDescriptionRtf,
						cmqLongDescriptionText = item2.cmqLongDescriptionText,
						cmqMemoDate = item2.cmqMemoDate,
						cmqRowVersion = item2.cmqRowVersion,
						cmqAttachmentMemoID = item2.cmqAttachmentMemoID,
						cmqShortDescription = item2.cmqShortDescription,
						CustomFields = item2.CustomFields
					};
					allAttachmentMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AttachmentMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAttachmentMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAttachmentMemosDto,
				RecordCount = allAttachmentMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAttachmentMemoDto>> Process_GetAttachmentMemo(Guid attachmentMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAttachmentMemoDto attachmentMemoDto = null;
		ERPResponseMessageDto<ERPAttachmentMemoDto> result;
		try
		{
			IERPAttachmentMemoRepository iERPAttachmentMemoRepository = (base.ERPAttachmentMemoRepository = new ERPAttachmentMemoRepository(base.ApiClientContext));
			using (iERPAttachmentMemoRepository)
			{
				ERPAttachmentMemoInformationDto eRPAttachmentMemoInformationDto = await base.ERPAttachmentMemoRepository.GetAttachmentMemo(attachmentMemoId);
				attachmentMemoDto = new ERPAttachmentMemoDto
				{
					cmqAttachmentID = eRPAttachmentMemoInformationDto.cmqAttachmentID,
					cmqCreatedBy = eRPAttachmentMemoInformationDto.cmqCreatedBy,
					cmqCreatedDate = eRPAttachmentMemoInformationDto.cmqCreatedDate,
					cmqUniqueID = eRPAttachmentMemoInformationDto.cmqUniqueID,
					cmqLongDescriptionRtf = eRPAttachmentMemoInformationDto.cmqLongDescriptionRtf,
					cmqLongDescriptionText = eRPAttachmentMemoInformationDto.cmqLongDescriptionText,
					cmqMemoDate = eRPAttachmentMemoInformationDto.cmqMemoDate,
					cmqRowVersion = eRPAttachmentMemoInformationDto.cmqRowVersion,
					cmqAttachmentMemoID = eRPAttachmentMemoInformationDto.cmqAttachmentMemoID,
					cmqShortDescription = eRPAttachmentMemoInformationDto.cmqShortDescription,
					CustomFields = eRPAttachmentMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AttachmentMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAttachmentMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = attachmentMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAttachmentMemoDto>> Process_PutAttachmentMemo(ERPAttachmentMemoDto attachmentMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAttachmentMemoDto createdObject = null;
		ERPResponseMessageDto<ERPAttachmentMemoDto> result;
		try
		{
			IERPAttachmentMemoRepository iERPAttachmentMemoRepository = (base.ERPAttachmentMemoRepository = new ERPAttachmentMemoRepository(base.ApiClientContext));
			using (iERPAttachmentMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAttachmentMemoRepository.SaveAttachmentMemo(attachmentMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAttachmentMemoInformationDto eRPAttachmentMemoInformationDto = await base.ERPAttachmentMemoRepository.GetAttachmentMemo(attachmentMemo.cmqUniqueID);
					createdObject = new ERPAttachmentMemoDto
					{
						cmqAttachmentID = eRPAttachmentMemoInformationDto.cmqAttachmentID,
						cmqCreatedBy = eRPAttachmentMemoInformationDto.cmqCreatedBy,
						cmqCreatedDate = eRPAttachmentMemoInformationDto.cmqCreatedDate,
						cmqUniqueID = eRPAttachmentMemoInformationDto.cmqUniqueID,
						cmqLongDescriptionRtf = eRPAttachmentMemoInformationDto.cmqLongDescriptionRtf,
						cmqLongDescriptionText = eRPAttachmentMemoInformationDto.cmqLongDescriptionText,
						cmqMemoDate = eRPAttachmentMemoInformationDto.cmqMemoDate,
						cmqRowVersion = eRPAttachmentMemoInformationDto.cmqRowVersion,
						cmqAttachmentMemoID = eRPAttachmentMemoInformationDto.cmqAttachmentMemoID,
						cmqShortDescription = eRPAttachmentMemoInformationDto.cmqShortDescription,
						CustomFields = eRPAttachmentMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing AttachmentMemo [{attachmentMemo.cmqUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAttachmentMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAttachmentMemo(Guid attachmentMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAttachmentMemoRepository iERPAttachmentMemoRepository = (base.ERPAttachmentMemoRepository = new ERPAttachmentMemoRepository(base.ApiClientContext));
		using (iERPAttachmentMemoRepository)
		{
			if (!(await base.ERPAttachmentMemoRepository.DoesAttachmentMemoExist(attachmentMemoId)))
			{
				base.ErrorsList.Add($"AttachmentMemo [{attachmentMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAttachmentMemoInformationDto eRPAttachmentMemoInformationDto = await base.ERPAttachmentMemoRepository.GetAttachmentMemo(attachmentMemoId);
				string text = await base.ERPAttachmentMemoRepository.WhereUsed("AttachmentMemos", new object[2] { eRPAttachmentMemoInformationDto.cmqAttachmentID, eRPAttachmentMemoInformationDto.cmqAttachmentMemoID }, new object[2] { "cmqAttachmentID", "cmqAttachmentMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("AttachmentMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAttachmentMemoDto>> Process_DeleteAttachmentMemo(Guid attachmentMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAttachmentMemoDto> result;
		try
		{
			IERPAttachmentMemoRepository iERPAttachmentMemoRepository = (base.ERPAttachmentMemoRepository = new ERPAttachmentMemoRepository(base.ApiClientContext));
			using (iERPAttachmentMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAttachmentMemoRepository.DeleteRowFromTable("AttachmentMemos", "cmq", attachmentMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of AttachmentMemo [{attachmentMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAttachmentMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAttachmentMemoDto()
			};
		}
		return result;
	}
}
