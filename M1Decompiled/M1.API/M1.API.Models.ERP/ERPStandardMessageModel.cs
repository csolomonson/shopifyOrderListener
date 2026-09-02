using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPStandardMessageModel : ERPBaseModel, IERPStandardMessageModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllStandardMessages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPStandardMessageRepository iERPStandardMessageRepository = (base.ERPStandardMessageRepository = new ERPStandardMessageRepository(base.ApiClientContext));
		using (iERPStandardMessageRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPStandardMessageRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPStandardMessageRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPStandardMessageRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPStandardMessageRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetStandardMessage(Guid standardMessageId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPStandardMessageRepository iERPStandardMessageRepository = (base.ERPStandardMessageRepository = new ERPStandardMessageRepository(base.ApiClientContext));
		using (iERPStandardMessageRepository)
		{
			if (!(await base.ERPStandardMessageRepository.DoesStandardMessageExist(standardMessageId)))
			{
				errorsList.Add($"StandardMessage [{standardMessageId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutStandardMessage(ERPStandardMessageDto standardMessage)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPStandardMessageRepository iERPStandardMessageRepository = (base.ERPStandardMessageRepository = new ERPStandardMessageRepository(base.ApiClientContext));
		using (iERPStandardMessageRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPStandardMessageDto>>> Process_GetAllStandardMessages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPStandardMessageDto> allStandardMessagesDto = new List<ERPStandardMessageDto>();
		ERPResponseMessageDto<IList<ERPStandardMessageDto>> result;
		try
		{
			IERPStandardMessageRepository iERPStandardMessageRepository = (base.ERPStandardMessageRepository = new ERPStandardMessageRepository(base.ApiClientContext));
			using (iERPStandardMessageRepository)
			{
				foreach (ERPStandardMessageInformationDto item2 in await base.ERPStandardMessageRepository.GetAllStandardMessages(pageSize, pageNumber, filter, orderBy))
				{
					ERPStandardMessageDto item = new ERPStandardMessageDto
					{
						xamStandardMessageID = item2.xamStandardMessageID,
						xamCreatedBy = item2.xamCreatedBy,
						xamCreatedDate = item2.xamCreatedDate,
						xamUniqueID = item2.xamUniqueID,
						xamInactiveDate = item2.xamInactiveDate,
						xamInactive = item2.xamInactive,
						xamLongDescriptionRtf = item2.xamLongDescriptionRtf,
						xamLongDescriptionText = item2.xamLongDescriptionText,
						xamMessageType = item2.xamMessageType,
						xamRowVersion = item2.xamRowVersion,
						xamShortDescription = item2.xamShortDescription,
						CustomFields = item2.CustomFields
					};
					allStandardMessagesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all StandardMessages]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPStandardMessageDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allStandardMessagesDto,
				RecordCount = allStandardMessagesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPStandardMessageDto>> Process_GetStandardMessage(Guid standardMessageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPStandardMessageDto standardMessageDto = null;
		ERPResponseMessageDto<ERPStandardMessageDto> result;
		try
		{
			IERPStandardMessageRepository iERPStandardMessageRepository = (base.ERPStandardMessageRepository = new ERPStandardMessageRepository(base.ApiClientContext));
			using (iERPStandardMessageRepository)
			{
				ERPStandardMessageInformationDto eRPStandardMessageInformationDto = await base.ERPStandardMessageRepository.GetStandardMessage(standardMessageId);
				standardMessageDto = new ERPStandardMessageDto
				{
					xamStandardMessageID = eRPStandardMessageInformationDto.xamStandardMessageID,
					xamCreatedBy = eRPStandardMessageInformationDto.xamCreatedBy,
					xamCreatedDate = eRPStandardMessageInformationDto.xamCreatedDate,
					xamUniqueID = eRPStandardMessageInformationDto.xamUniqueID,
					xamInactiveDate = eRPStandardMessageInformationDto.xamInactiveDate,
					xamInactive = eRPStandardMessageInformationDto.xamInactive,
					xamLongDescriptionRtf = eRPStandardMessageInformationDto.xamLongDescriptionRtf,
					xamLongDescriptionText = eRPStandardMessageInformationDto.xamLongDescriptionText,
					xamMessageType = eRPStandardMessageInformationDto.xamMessageType,
					xamRowVersion = eRPStandardMessageInformationDto.xamRowVersion,
					xamShortDescription = eRPStandardMessageInformationDto.xamShortDescription,
					CustomFields = eRPStandardMessageInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the StandardMessages []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPStandardMessageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = standardMessageDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPStandardMessageDto>> Process_PutStandardMessage(ERPStandardMessageDto standardMessage)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPStandardMessageDto createdObject = null;
		ERPResponseMessageDto<ERPStandardMessageDto> result;
		try
		{
			IERPStandardMessageRepository iERPStandardMessageRepository = (base.ERPStandardMessageRepository = new ERPStandardMessageRepository(base.ApiClientContext));
			using (iERPStandardMessageRepository)
			{
				APIValidationInfoDto postResult = await base.ERPStandardMessageRepository.SaveStandardMessage(standardMessage);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPStandardMessageInformationDto eRPStandardMessageInformationDto = await base.ERPStandardMessageRepository.GetStandardMessage(standardMessage.xamUniqueID);
					createdObject = new ERPStandardMessageDto
					{
						xamStandardMessageID = eRPStandardMessageInformationDto.xamStandardMessageID,
						xamCreatedBy = eRPStandardMessageInformationDto.xamCreatedBy,
						xamCreatedDate = eRPStandardMessageInformationDto.xamCreatedDate,
						xamUniqueID = eRPStandardMessageInformationDto.xamUniqueID,
						xamInactiveDate = eRPStandardMessageInformationDto.xamInactiveDate,
						xamInactive = eRPStandardMessageInformationDto.xamInactive,
						xamLongDescriptionRtf = eRPStandardMessageInformationDto.xamLongDescriptionRtf,
						xamLongDescriptionText = eRPStandardMessageInformationDto.xamLongDescriptionText,
						xamMessageType = eRPStandardMessageInformationDto.xamMessageType,
						xamRowVersion = eRPStandardMessageInformationDto.xamRowVersion,
						xamShortDescription = eRPStandardMessageInformationDto.xamShortDescription,
						CustomFields = eRPStandardMessageInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing StandardMessage [{standardMessage.xamUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPStandardMessageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteStandardMessage(Guid standardMessageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPStandardMessageRepository iERPStandardMessageRepository = (base.ERPStandardMessageRepository = new ERPStandardMessageRepository(base.ApiClientContext));
		using (iERPStandardMessageRepository)
		{
			if (!(await base.ERPStandardMessageRepository.DoesStandardMessageExist(standardMessageId)))
			{
				base.ErrorsList.Add($"StandardMessage [{standardMessageId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPStandardMessageInformationDto eRPStandardMessageInformationDto = await base.ERPStandardMessageRepository.GetStandardMessage(standardMessageId);
				string text = await base.ERPStandardMessageRepository.WhereUsed("StandardMessages", new object[1] { eRPStandardMessageInformationDto.xamStandardMessageID }, new object[1] { "xamStandardMessageID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("StandardMessage cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPStandardMessageDto>> Process_DeleteStandardMessage(Guid standardMessageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPStandardMessageDto> result;
		try
		{
			IERPStandardMessageRepository iERPStandardMessageRepository = (base.ERPStandardMessageRepository = new ERPStandardMessageRepository(base.ApiClientContext));
			using (iERPStandardMessageRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPStandardMessageRepository.DeleteRowFromTable("StandardMessages", "xam", standardMessageId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of StandardMessage [{standardMessageId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPStandardMessageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPStandardMessageDto()
			};
		}
		return result;
	}
}
