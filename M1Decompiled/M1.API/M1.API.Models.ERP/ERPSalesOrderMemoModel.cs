using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderMemoModel : ERPBaseModel, IERPSalesOrderMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderMemoRepository iERPSalesOrderMemoRepository = (base.ERPSalesOrderMemoRepository = new ERPSalesOrderMemoRepository(base.ApiClientContext));
		using (iERPSalesOrderMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderMemo(Guid salesOrderMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderMemoRepository iERPSalesOrderMemoRepository = (base.ERPSalesOrderMemoRepository = new ERPSalesOrderMemoRepository(base.ApiClientContext));
		using (iERPSalesOrderMemoRepository)
		{
			if (!(await base.ERPSalesOrderMemoRepository.DoesSalesOrderMemoExist(salesOrderMemoId)))
			{
				errorsList.Add($"SalesOrderMemo [{salesOrderMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderMemo(ERPSalesOrderMemoDto salesOrderMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderMemoRepository iERPSalesOrderMemoRepository = (base.ERPSalesOrderMemoRepository = new ERPSalesOrderMemoRepository(base.ApiClientContext));
		using (iERPSalesOrderMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(salesOrderMemo.omkSalesOrderID) && !(await base.ERPSalesOrderMemoRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { salesOrderMemo.omkSalesOrderID })))
			{
				errorsList.Add("omkSalesOrderID [" + salesOrderMemo.omkSalesOrderID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderMemoDto>>> Process_GetAllSalesOrderMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderMemoDto> allSalesOrderMemosDto = new List<ERPSalesOrderMemoDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderMemoDto>> result;
		try
		{
			IERPSalesOrderMemoRepository iERPSalesOrderMemoRepository = (base.ERPSalesOrderMemoRepository = new ERPSalesOrderMemoRepository(base.ApiClientContext));
			using (iERPSalesOrderMemoRepository)
			{
				foreach (ERPSalesOrderMemoInformationDto item2 in await base.ERPSalesOrderMemoRepository.GetAllSalesOrderMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderMemoDto item = new ERPSalesOrderMemoDto
					{
						omkCreatedBy = item2.omkCreatedBy,
						omkCreatedDate = item2.omkCreatedDate,
						omkUniqueID = item2.omkUniqueID,
						omkClosed = item2.omkClosed,
						omkLongDescriptionRtf = item2.omkLongDescriptionRtf,
						omkLongDescriptionText = item2.omkLongDescriptionText,
						omkMemoDate = item2.omkMemoDate,
						omkRowVersion = item2.omkRowVersion,
						omkSalesOrderID = item2.omkSalesOrderID,
						omkSalesOrderMemoID = item2.omkSalesOrderMemoID,
						omkShortDescription = item2.omkShortDescription,
						omkShowInArInvoices = item2.omkShowInArInvoices,
						omkShowInSalesOrders = item2.omkShowInSalesOrders,
						omkShowInShipments = item2.omkShowInShipments,
						CustomFields = item2.CustomFields
					};
					allSalesOrderMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrderMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrderMemosDto,
				RecordCount = allSalesOrderMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderMemoDto>> Process_GetSalesOrderMemo(Guid salesOrderMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderMemoDto salesOrderMemoDto = null;
		ERPResponseMessageDto<ERPSalesOrderMemoDto> result;
		try
		{
			IERPSalesOrderMemoRepository iERPSalesOrderMemoRepository = (base.ERPSalesOrderMemoRepository = new ERPSalesOrderMemoRepository(base.ApiClientContext));
			using (iERPSalesOrderMemoRepository)
			{
				ERPSalesOrderMemoInformationDto eRPSalesOrderMemoInformationDto = await base.ERPSalesOrderMemoRepository.GetSalesOrderMemo(salesOrderMemoId);
				salesOrderMemoDto = new ERPSalesOrderMemoDto
				{
					omkCreatedBy = eRPSalesOrderMemoInformationDto.omkCreatedBy,
					omkCreatedDate = eRPSalesOrderMemoInformationDto.omkCreatedDate,
					omkUniqueID = eRPSalesOrderMemoInformationDto.omkUniqueID,
					omkClosed = eRPSalesOrderMemoInformationDto.omkClosed,
					omkLongDescriptionRtf = eRPSalesOrderMemoInformationDto.omkLongDescriptionRtf,
					omkLongDescriptionText = eRPSalesOrderMemoInformationDto.omkLongDescriptionText,
					omkMemoDate = eRPSalesOrderMemoInformationDto.omkMemoDate,
					omkRowVersion = eRPSalesOrderMemoInformationDto.omkRowVersion,
					omkSalesOrderID = eRPSalesOrderMemoInformationDto.omkSalesOrderID,
					omkSalesOrderMemoID = eRPSalesOrderMemoInformationDto.omkSalesOrderMemoID,
					omkShortDescription = eRPSalesOrderMemoInformationDto.omkShortDescription,
					omkShowInArInvoices = eRPSalesOrderMemoInformationDto.omkShowInArInvoices,
					omkShowInSalesOrders = eRPSalesOrderMemoInformationDto.omkShowInSalesOrders,
					omkShowInShipments = eRPSalesOrderMemoInformationDto.omkShowInShipments,
					CustomFields = eRPSalesOrderMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrderMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderMemoDto>> Process_PutSalesOrderMemo(ERPSalesOrderMemoDto salesOrderMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderMemoDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderMemoDto> result;
		try
		{
			IERPSalesOrderMemoRepository iERPSalesOrderMemoRepository = (base.ERPSalesOrderMemoRepository = new ERPSalesOrderMemoRepository(base.ApiClientContext));
			using (iERPSalesOrderMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderMemoRepository.SaveSalesOrderMemo(salesOrderMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderMemoInformationDto eRPSalesOrderMemoInformationDto = await base.ERPSalesOrderMemoRepository.GetSalesOrderMemo(salesOrderMemo.omkUniqueID);
					createdObject = new ERPSalesOrderMemoDto
					{
						omkCreatedBy = eRPSalesOrderMemoInformationDto.omkCreatedBy,
						omkCreatedDate = eRPSalesOrderMemoInformationDto.omkCreatedDate,
						omkUniqueID = eRPSalesOrderMemoInformationDto.omkUniqueID,
						omkClosed = eRPSalesOrderMemoInformationDto.omkClosed,
						omkLongDescriptionRtf = eRPSalesOrderMemoInformationDto.omkLongDescriptionRtf,
						omkLongDescriptionText = eRPSalesOrderMemoInformationDto.omkLongDescriptionText,
						omkMemoDate = eRPSalesOrderMemoInformationDto.omkMemoDate,
						omkRowVersion = eRPSalesOrderMemoInformationDto.omkRowVersion,
						omkSalesOrderID = eRPSalesOrderMemoInformationDto.omkSalesOrderID,
						omkSalesOrderMemoID = eRPSalesOrderMemoInformationDto.omkSalesOrderMemoID,
						omkShortDescription = eRPSalesOrderMemoInformationDto.omkShortDescription,
						omkShowInArInvoices = eRPSalesOrderMemoInformationDto.omkShowInArInvoices,
						omkShowInSalesOrders = eRPSalesOrderMemoInformationDto.omkShowInSalesOrders,
						omkShowInShipments = eRPSalesOrderMemoInformationDto.omkShowInShipments,
						CustomFields = eRPSalesOrderMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrderMemo [{salesOrderMemo.omkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderMemo(Guid salesOrderMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderMemoRepository iERPSalesOrderMemoRepository = (base.ERPSalesOrderMemoRepository = new ERPSalesOrderMemoRepository(base.ApiClientContext));
		using (iERPSalesOrderMemoRepository)
		{
			if (!(await base.ERPSalesOrderMemoRepository.DoesSalesOrderMemoExist(salesOrderMemoId)))
			{
				base.ErrorsList.Add($"SalesOrderMemo [{salesOrderMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderMemoInformationDto eRPSalesOrderMemoInformationDto = await base.ERPSalesOrderMemoRepository.GetSalesOrderMemo(salesOrderMemoId);
				string text = await base.ERPSalesOrderMemoRepository.WhereUsed("SalesOrderMemos", new object[2] { eRPSalesOrderMemoInformationDto.omkSalesOrderID, eRPSalesOrderMemoInformationDto.omkSalesOrderMemoID }, new object[2] { "omkSalesOrderID", "omkSalesOrderMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrderMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderMemoDto>> Process_DeleteSalesOrderMemo(Guid salesOrderMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderMemoDto> result;
		try
		{
			IERPSalesOrderMemoRepository iERPSalesOrderMemoRepository = (base.ERPSalesOrderMemoRepository = new ERPSalesOrderMemoRepository(base.ApiClientContext));
			using (iERPSalesOrderMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderMemoRepository.DeleteRowFromTable("SalesOrderMemos", "omk", salesOrderMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrderMemo [{salesOrderMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderMemoDto()
			};
		}
		return result;
	}
}
