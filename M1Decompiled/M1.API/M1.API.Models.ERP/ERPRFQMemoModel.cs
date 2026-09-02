using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRFQMemoModel : ERPBaseModel, IERPRFQMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRFQMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRFQMemoRepository iERPRFQMemoRepository = (base.ERPRFQMemoRepository = new ERPRFQMemoRepository(base.ApiClientContext));
		using (iERPRFQMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRFQMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRFQMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRFQMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRFQMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRFQMemo(Guid rFQMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQMemoRepository iERPRFQMemoRepository = (base.ERPRFQMemoRepository = new ERPRFQMemoRepository(base.ApiClientContext));
		using (iERPRFQMemoRepository)
		{
			if (!(await base.ERPRFQMemoRepository.DoesRFQMemoExist(rFQMemoId)))
			{
				errorsList.Add($"RFQMemo [{rFQMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRFQMemo(ERPRFQMemoDto rFQMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQMemoRepository iERPRFQMemoRepository = (base.ERPRFQMemoRepository = new ERPRFQMemoRepository(base.ApiClientContext));
		using (iERPRFQMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(rFQMemo.rqkRfqID) && !(await base.ERPRFQMemoRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { rFQMemo.rqkRfqID })))
			{
				errorsList.Add("rqkRfqID [" + rFQMemo.rqkRfqID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRFQMemoDto>>> Process_GetAllRFQMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRFQMemoDto> allRFQMemosDto = new List<ERPRFQMemoDto>();
		ERPResponseMessageDto<IList<ERPRFQMemoDto>> result;
		try
		{
			IERPRFQMemoRepository iERPRFQMemoRepository = (base.ERPRFQMemoRepository = new ERPRFQMemoRepository(base.ApiClientContext));
			using (iERPRFQMemoRepository)
			{
				foreach (ERPRFQMemoInformationDto item2 in await base.ERPRFQMemoRepository.GetAllRFQMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPRFQMemoDto item = new ERPRFQMemoDto
					{
						rqkCreatedBy = item2.rqkCreatedBy,
						rqkCreatedDate = item2.rqkCreatedDate,
						rqkUniqueID = item2.rqkUniqueID,
						rqkClosed = item2.rqkClosed,
						rqkLongDescriptionRtf = item2.rqkLongDescriptionRtf,
						rqkLongDescriptionText = item2.rqkLongDescriptionText,
						rqkMemoDate = item2.rqkMemoDate,
						rqkRfqID = item2.rqkRfqID,
						rqkRowVersion = item2.rqkRowVersion,
						rqkRfqMemoID = item2.rqkRfqMemoID,
						rqkShortDescription = item2.rqkShortDescription,
						CustomFields = item2.CustomFields
					};
					allRFQMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RFQMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRFQMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRFQMemosDto,
				RecordCount = allRFQMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQMemoDto>> Process_GetRFQMemo(Guid rFQMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRFQMemoDto rFQMemoDto = null;
		ERPResponseMessageDto<ERPRFQMemoDto> result;
		try
		{
			IERPRFQMemoRepository iERPRFQMemoRepository = (base.ERPRFQMemoRepository = new ERPRFQMemoRepository(base.ApiClientContext));
			using (iERPRFQMemoRepository)
			{
				ERPRFQMemoInformationDto eRPRFQMemoInformationDto = await base.ERPRFQMemoRepository.GetRFQMemo(rFQMemoId);
				rFQMemoDto = new ERPRFQMemoDto
				{
					rqkCreatedBy = eRPRFQMemoInformationDto.rqkCreatedBy,
					rqkCreatedDate = eRPRFQMemoInformationDto.rqkCreatedDate,
					rqkUniqueID = eRPRFQMemoInformationDto.rqkUniqueID,
					rqkClosed = eRPRFQMemoInformationDto.rqkClosed,
					rqkLongDescriptionRtf = eRPRFQMemoInformationDto.rqkLongDescriptionRtf,
					rqkLongDescriptionText = eRPRFQMemoInformationDto.rqkLongDescriptionText,
					rqkMemoDate = eRPRFQMemoInformationDto.rqkMemoDate,
					rqkRfqID = eRPRFQMemoInformationDto.rqkRfqID,
					rqkRowVersion = eRPRFQMemoInformationDto.rqkRowVersion,
					rqkRfqMemoID = eRPRFQMemoInformationDto.rqkRfqMemoID,
					rqkShortDescription = eRPRFQMemoInformationDto.rqkShortDescription,
					CustomFields = eRPRFQMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RFQMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rFQMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQMemoDto>> Process_PutRFQMemo(ERPRFQMemoDto rFQMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRFQMemoDto createdObject = null;
		ERPResponseMessageDto<ERPRFQMemoDto> result;
		try
		{
			IERPRFQMemoRepository iERPRFQMemoRepository = (base.ERPRFQMemoRepository = new ERPRFQMemoRepository(base.ApiClientContext));
			using (iERPRFQMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRFQMemoRepository.SaveRFQMemo(rFQMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRFQMemoInformationDto eRPRFQMemoInformationDto = await base.ERPRFQMemoRepository.GetRFQMemo(rFQMemo.rqkUniqueID);
					createdObject = new ERPRFQMemoDto
					{
						rqkCreatedBy = eRPRFQMemoInformationDto.rqkCreatedBy,
						rqkCreatedDate = eRPRFQMemoInformationDto.rqkCreatedDate,
						rqkUniqueID = eRPRFQMemoInformationDto.rqkUniqueID,
						rqkClosed = eRPRFQMemoInformationDto.rqkClosed,
						rqkLongDescriptionRtf = eRPRFQMemoInformationDto.rqkLongDescriptionRtf,
						rqkLongDescriptionText = eRPRFQMemoInformationDto.rqkLongDescriptionText,
						rqkMemoDate = eRPRFQMemoInformationDto.rqkMemoDate,
						rqkRfqID = eRPRFQMemoInformationDto.rqkRfqID,
						rqkRowVersion = eRPRFQMemoInformationDto.rqkRowVersion,
						rqkRfqMemoID = eRPRFQMemoInformationDto.rqkRfqMemoID,
						rqkShortDescription = eRPRFQMemoInformationDto.rqkShortDescription,
						CustomFields = eRPRFQMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RFQMemo [{rFQMemo.rqkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRFQMemo(Guid rFQMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQMemoRepository iERPRFQMemoRepository = (base.ERPRFQMemoRepository = new ERPRFQMemoRepository(base.ApiClientContext));
		using (iERPRFQMemoRepository)
		{
			if (!(await base.ERPRFQMemoRepository.DoesRFQMemoExist(rFQMemoId)))
			{
				base.ErrorsList.Add($"RFQMemo [{rFQMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRFQMemoInformationDto eRPRFQMemoInformationDto = await base.ERPRFQMemoRepository.GetRFQMemo(rFQMemoId);
				string text = await base.ERPRFQMemoRepository.WhereUsed("RFQMemos", new object[2] { eRPRFQMemoInformationDto.rqkRfqID, eRPRFQMemoInformationDto.rqkRfqMemoID }, new object[2] { "rqkRfqID", "rqkRfqMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RFQMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRFQMemoDto>> Process_DeleteRFQMemo(Guid rFQMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRFQMemoDto> result;
		try
		{
			IERPRFQMemoRepository iERPRFQMemoRepository = (base.ERPRFQMemoRepository = new ERPRFQMemoRepository(base.ApiClientContext));
			using (iERPRFQMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRFQMemoRepository.DeleteRowFromTable("RFQMemos", "rqk", rFQMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RFQMemo [{rFQMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRFQMemoDto()
			};
		}
		return result;
	}
}
