using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCallLineModel : ERPBaseModel, IERPCallLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCallLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCallLineRepository iERPCallLineRepository = (base.ERPCallLineRepository = new ERPCallLineRepository(base.ApiClientContext));
		using (iERPCallLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCallLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCallLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCallLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCallLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCallLine(Guid callLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallLineRepository iERPCallLineRepository = (base.ERPCallLineRepository = new ERPCallLineRepository(base.ApiClientContext));
		using (iERPCallLineRepository)
		{
			if (!(await base.ERPCallLineRepository.DoesCallLineExist(callLineId)))
			{
				errorsList.Add($"CallLine [{callLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCallLine(ERPCallLineDto callLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallLineRepository iERPCallLineRepository = (base.ERPCallLineRepository = new ERPCallLineRepository(base.ApiClientContext));
		using (iERPCallLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(callLine.kblCallID) && !(await base.ERPCallLineRepository.DoesRecordExistInTableUsingKeys("Calls", new object[1] { "KBPCALLID" }, new object[1] { callLine.kblCallID })))
			{
				errorsList.Add("kblCallID [" + callLine.kblCallID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(callLine.kblContactMethodID) && !(await base.ERPCallLineRepository.DoesRecordExistInTableUsingKeys("ContactMethods", new object[1] { "KBCCONTACTMETHODID" }, new object[1] { callLine.kblContactMethodID })))
			{
				errorsList.Add("kblContactMethodID [" + callLine.kblContactMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(callLine.kblAddedByEmployeeID) && !(await base.ERPCallLineRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { callLine.kblAddedByEmployeeID })))
			{
				errorsList.Add("kblAddedByEmployeeID [" + callLine.kblAddedByEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCallLineDto>>> Process_GetAllCallLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCallLineDto> allCallLinesDto = new List<ERPCallLineDto>();
		ERPResponseMessageDto<IList<ERPCallLineDto>> result;
		try
		{
			IERPCallLineRepository iERPCallLineRepository = (base.ERPCallLineRepository = new ERPCallLineRepository(base.ApiClientContext));
			using (iERPCallLineRepository)
			{
				foreach (ERPCallLineInformationDto item2 in await base.ERPCallLineRepository.GetAllCallLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPCallLineDto item = new ERPCallLineDto
					{
						kblAddedByEmployeeID = item2.kblAddedByEmployeeID,
						kblAddedDate = item2.kblAddedDate,
						kblCallID = item2.kblCallID,
						kblContactMethodID = item2.kblContactMethodID,
						kblCreatedBy = item2.kblCreatedBy,
						kblCreatedDate = item2.kblCreatedDate,
						kblUniqueID = item2.kblUniqueID,
						kblExtraTime = item2.kblExtraTime,
						kblBillable = item2.kblBillable,
						kblCreatedFromMobile = item2.kblCreatedFromMobile,
						kblInbound = item2.kblInbound,
						kblInternalOnly = item2.kblInternalOnly,
						kblLongDescriptionRtf = item2.kblLongDescriptionRtf,
						kblLongDescriptionText = item2.kblLongDescriptionText,
						kblRowVersion = item2.kblRowVersion,
						kblCallLineID = item2.kblCallLineID,
						kblShortDescription = item2.kblShortDescription,
						kblTimeSpent = item2.kblTimeSpent,
						kblTotalTime = item2.kblTotalTime,
						CustomFields = item2.CustomFields
					};
					allCallLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CallLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCallLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCallLinesDto,
				RecordCount = allCallLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCallLineDto>> Process_GetCallLine(Guid callLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCallLineDto callLineDto = null;
		ERPResponseMessageDto<ERPCallLineDto> result;
		try
		{
			IERPCallLineRepository iERPCallLineRepository = (base.ERPCallLineRepository = new ERPCallLineRepository(base.ApiClientContext));
			using (iERPCallLineRepository)
			{
				ERPCallLineInformationDto eRPCallLineInformationDto = await base.ERPCallLineRepository.GetCallLine(callLineId);
				callLineDto = new ERPCallLineDto
				{
					kblAddedByEmployeeID = eRPCallLineInformationDto.kblAddedByEmployeeID,
					kblAddedDate = eRPCallLineInformationDto.kblAddedDate,
					kblCallID = eRPCallLineInformationDto.kblCallID,
					kblContactMethodID = eRPCallLineInformationDto.kblContactMethodID,
					kblCreatedBy = eRPCallLineInformationDto.kblCreatedBy,
					kblCreatedDate = eRPCallLineInformationDto.kblCreatedDate,
					kblUniqueID = eRPCallLineInformationDto.kblUniqueID,
					kblExtraTime = eRPCallLineInformationDto.kblExtraTime,
					kblBillable = eRPCallLineInformationDto.kblBillable,
					kblCreatedFromMobile = eRPCallLineInformationDto.kblCreatedFromMobile,
					kblInbound = eRPCallLineInformationDto.kblInbound,
					kblInternalOnly = eRPCallLineInformationDto.kblInternalOnly,
					kblLongDescriptionRtf = eRPCallLineInformationDto.kblLongDescriptionRtf,
					kblLongDescriptionText = eRPCallLineInformationDto.kblLongDescriptionText,
					kblRowVersion = eRPCallLineInformationDto.kblRowVersion,
					kblCallLineID = eRPCallLineInformationDto.kblCallLineID,
					kblShortDescription = eRPCallLineInformationDto.kblShortDescription,
					kblTimeSpent = eRPCallLineInformationDto.kblTimeSpent,
					kblTotalTime = eRPCallLineInformationDto.kblTotalTime,
					CustomFields = eRPCallLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CallLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = callLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCallLineDto>> Process_PutCallLine(ERPCallLineDto callLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCallLineDto createdObject = null;
		ERPResponseMessageDto<ERPCallLineDto> result;
		try
		{
			IERPCallLineRepository iERPCallLineRepository = (base.ERPCallLineRepository = new ERPCallLineRepository(base.ApiClientContext));
			using (iERPCallLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCallLineRepository.SaveCallLine(callLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCallLineInformationDto eRPCallLineInformationDto = await base.ERPCallLineRepository.GetCallLine(callLine.kblUniqueID);
					createdObject = new ERPCallLineDto
					{
						kblAddedByEmployeeID = eRPCallLineInformationDto.kblAddedByEmployeeID,
						kblAddedDate = eRPCallLineInformationDto.kblAddedDate,
						kblCallID = eRPCallLineInformationDto.kblCallID,
						kblContactMethodID = eRPCallLineInformationDto.kblContactMethodID,
						kblCreatedBy = eRPCallLineInformationDto.kblCreatedBy,
						kblCreatedDate = eRPCallLineInformationDto.kblCreatedDate,
						kblUniqueID = eRPCallLineInformationDto.kblUniqueID,
						kblExtraTime = eRPCallLineInformationDto.kblExtraTime,
						kblBillable = eRPCallLineInformationDto.kblBillable,
						kblCreatedFromMobile = eRPCallLineInformationDto.kblCreatedFromMobile,
						kblInbound = eRPCallLineInformationDto.kblInbound,
						kblInternalOnly = eRPCallLineInformationDto.kblInternalOnly,
						kblLongDescriptionRtf = eRPCallLineInformationDto.kblLongDescriptionRtf,
						kblLongDescriptionText = eRPCallLineInformationDto.kblLongDescriptionText,
						kblRowVersion = eRPCallLineInformationDto.kblRowVersion,
						kblCallLineID = eRPCallLineInformationDto.kblCallLineID,
						kblShortDescription = eRPCallLineInformationDto.kblShortDescription,
						kblTimeSpent = eRPCallLineInformationDto.kblTimeSpent,
						kblTotalTime = eRPCallLineInformationDto.kblTotalTime,
						CustomFields = eRPCallLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CallLine [{callLine.kblUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCallLine(Guid callLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallLineRepository iERPCallLineRepository = (base.ERPCallLineRepository = new ERPCallLineRepository(base.ApiClientContext));
		using (iERPCallLineRepository)
		{
			if (!(await base.ERPCallLineRepository.DoesCallLineExist(callLineId)))
			{
				base.ErrorsList.Add($"CallLine [{callLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCallLineInformationDto eRPCallLineInformationDto = await base.ERPCallLineRepository.GetCallLine(callLineId);
				string text = await base.ERPCallLineRepository.WhereUsed("CallLines", new object[2] { eRPCallLineInformationDto.kblCallID, eRPCallLineInformationDto.kblCallLineID }, new object[2] { "kblCallID", "kblCallLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CallLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCallLineDto>> Process_DeleteCallLine(Guid callLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCallLineDto> result;
		try
		{
			IERPCallLineRepository iERPCallLineRepository = (base.ERPCallLineRepository = new ERPCallLineRepository(base.ApiClientContext));
			using (iERPCallLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCallLineRepository.DeleteRowFromTable("CallLines", "kbl", callLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CallLine [{callLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCallLineDto()
			};
		}
		return result;
	}
}
