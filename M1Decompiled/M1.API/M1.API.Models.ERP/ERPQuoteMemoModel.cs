using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPQuoteMemoModel : ERPBaseModel, IERPQuoteMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPQuoteMemoRepository iERPQuoteMemoRepository = (base.ERPQuoteMemoRepository = new ERPQuoteMemoRepository(base.ApiClientContext));
		using (iERPQuoteMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPQuoteMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPQuoteMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPQuoteMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPQuoteMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteMemo(Guid quoteMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteMemoRepository iERPQuoteMemoRepository = (base.ERPQuoteMemoRepository = new ERPQuoteMemoRepository(base.ApiClientContext));
		using (iERPQuoteMemoRepository)
		{
			if (!(await base.ERPQuoteMemoRepository.DoesQuoteMemoExist(quoteMemoId)))
			{
				errorsList.Add($"QuoteMemo [{quoteMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutQuoteMemo(ERPQuoteMemoDto quoteMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteMemoRepository iERPQuoteMemoRepository = (base.ERPQuoteMemoRepository = new ERPQuoteMemoRepository(base.ApiClientContext));
		using (iERPQuoteMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(quoteMemo.qmkQuoteID) && !(await base.ERPQuoteMemoRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { quoteMemo.qmkQuoteID })))
			{
				errorsList.Add("qmkQuoteID [" + quoteMemo.qmkQuoteID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPQuoteMemoDto>>> Process_GetAllQuoteMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPQuoteMemoDto> allQuoteMemosDto = new List<ERPQuoteMemoDto>();
		ERPResponseMessageDto<IList<ERPQuoteMemoDto>> result;
		try
		{
			IERPQuoteMemoRepository iERPQuoteMemoRepository = (base.ERPQuoteMemoRepository = new ERPQuoteMemoRepository(base.ApiClientContext));
			using (iERPQuoteMemoRepository)
			{
				foreach (ERPQuoteMemoInformationDto item2 in await base.ERPQuoteMemoRepository.GetAllQuoteMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPQuoteMemoDto item = new ERPQuoteMemoDto
					{
						qmkCreatedBy = item2.qmkCreatedBy,
						qmkCreatedDate = item2.qmkCreatedDate,
						qmkUniqueID = item2.qmkUniqueID,
						qmkClosed = item2.qmkClosed,
						qmkLongDescriptionRtf = item2.qmkLongDescriptionRtf,
						qmkLongDescriptionText = item2.qmkLongDescriptionText,
						qmkMemoDate = item2.qmkMemoDate,
						qmkQuoteID = item2.qmkQuoteID,
						qmkRowVersion = item2.qmkRowVersion,
						qmkQuoteMemoID = item2.qmkQuoteMemoID,
						qmkShortDescription = item2.qmkShortDescription,
						qmkShowInQuotes = item2.qmkShowInQuotes,
						qmkShowInSalesOrders = item2.qmkShowInSalesOrders,
						CustomFields = item2.CustomFields
					};
					allQuoteMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPQuoteMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteMemosDto,
				RecordCount = allQuoteMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteMemoDto>> Process_GetQuoteMemo(Guid quoteMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPQuoteMemoDto quoteMemoDto = null;
		ERPResponseMessageDto<ERPQuoteMemoDto> result;
		try
		{
			IERPQuoteMemoRepository iERPQuoteMemoRepository = (base.ERPQuoteMemoRepository = new ERPQuoteMemoRepository(base.ApiClientContext));
			using (iERPQuoteMemoRepository)
			{
				ERPQuoteMemoInformationDto eRPQuoteMemoInformationDto = await base.ERPQuoteMemoRepository.GetQuoteMemo(quoteMemoId);
				quoteMemoDto = new ERPQuoteMemoDto
				{
					qmkCreatedBy = eRPQuoteMemoInformationDto.qmkCreatedBy,
					qmkCreatedDate = eRPQuoteMemoInformationDto.qmkCreatedDate,
					qmkUniqueID = eRPQuoteMemoInformationDto.qmkUniqueID,
					qmkClosed = eRPQuoteMemoInformationDto.qmkClosed,
					qmkLongDescriptionRtf = eRPQuoteMemoInformationDto.qmkLongDescriptionRtf,
					qmkLongDescriptionText = eRPQuoteMemoInformationDto.qmkLongDescriptionText,
					qmkMemoDate = eRPQuoteMemoInformationDto.qmkMemoDate,
					qmkQuoteID = eRPQuoteMemoInformationDto.qmkQuoteID,
					qmkRowVersion = eRPQuoteMemoInformationDto.qmkRowVersion,
					qmkQuoteMemoID = eRPQuoteMemoInformationDto.qmkQuoteMemoID,
					qmkShortDescription = eRPQuoteMemoInformationDto.qmkShortDescription,
					qmkShowInQuotes = eRPQuoteMemoInformationDto.qmkShowInQuotes,
					qmkShowInSalesOrders = eRPQuoteMemoInformationDto.qmkShowInSalesOrders,
					CustomFields = eRPQuoteMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteMemoDto>> Process_PutQuoteMemo(ERPQuoteMemoDto quoteMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPQuoteMemoDto createdObject = null;
		ERPResponseMessageDto<ERPQuoteMemoDto> result;
		try
		{
			IERPQuoteMemoRepository iERPQuoteMemoRepository = (base.ERPQuoteMemoRepository = new ERPQuoteMemoRepository(base.ApiClientContext));
			using (iERPQuoteMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPQuoteMemoRepository.SaveQuoteMemo(quoteMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPQuoteMemoInformationDto eRPQuoteMemoInformationDto = await base.ERPQuoteMemoRepository.GetQuoteMemo(quoteMemo.qmkUniqueID);
					createdObject = new ERPQuoteMemoDto
					{
						qmkCreatedBy = eRPQuoteMemoInformationDto.qmkCreatedBy,
						qmkCreatedDate = eRPQuoteMemoInformationDto.qmkCreatedDate,
						qmkUniqueID = eRPQuoteMemoInformationDto.qmkUniqueID,
						qmkClosed = eRPQuoteMemoInformationDto.qmkClosed,
						qmkLongDescriptionRtf = eRPQuoteMemoInformationDto.qmkLongDescriptionRtf,
						qmkLongDescriptionText = eRPQuoteMemoInformationDto.qmkLongDescriptionText,
						qmkMemoDate = eRPQuoteMemoInformationDto.qmkMemoDate,
						qmkQuoteID = eRPQuoteMemoInformationDto.qmkQuoteID,
						qmkRowVersion = eRPQuoteMemoInformationDto.qmkRowVersion,
						qmkQuoteMemoID = eRPQuoteMemoInformationDto.qmkQuoteMemoID,
						qmkShortDescription = eRPQuoteMemoInformationDto.qmkShortDescription,
						qmkShowInQuotes = eRPQuoteMemoInformationDto.qmkShowInQuotes,
						qmkShowInSalesOrders = eRPQuoteMemoInformationDto.qmkShowInSalesOrders,
						CustomFields = eRPQuoteMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing QuoteMemo [{quoteMemo.qmkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteMemo(Guid quoteMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteMemoRepository iERPQuoteMemoRepository = (base.ERPQuoteMemoRepository = new ERPQuoteMemoRepository(base.ApiClientContext));
		using (iERPQuoteMemoRepository)
		{
			if (!(await base.ERPQuoteMemoRepository.DoesQuoteMemoExist(quoteMemoId)))
			{
				base.ErrorsList.Add($"QuoteMemo [{quoteMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPQuoteMemoInformationDto eRPQuoteMemoInformationDto = await base.ERPQuoteMemoRepository.GetQuoteMemo(quoteMemoId);
				string text = await base.ERPQuoteMemoRepository.WhereUsed("QuoteMemos", new object[2] { eRPQuoteMemoInformationDto.qmkQuoteID, eRPQuoteMemoInformationDto.qmkQuoteMemoID }, new object[2] { "qmkQuoteID", "qmkQuoteMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("QuoteMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPQuoteMemoDto>> Process_DeleteQuoteMemo(Guid quoteMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPQuoteMemoDto> result;
		try
		{
			IERPQuoteMemoRepository iERPQuoteMemoRepository = (base.ERPQuoteMemoRepository = new ERPQuoteMemoRepository(base.ApiClientContext));
			using (iERPQuoteMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPQuoteMemoRepository.DeleteRowFromTable("QuoteMemos", "qmk", quoteMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of QuoteMemo [{quoteMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPQuoteMemoDto()
			};
		}
		return result;
	}
}
