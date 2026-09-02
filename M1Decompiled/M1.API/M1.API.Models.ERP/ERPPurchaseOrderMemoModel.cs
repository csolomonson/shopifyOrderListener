using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchaseOrderMemoModel : ERPBaseModel, IERPPurchaseOrderMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchaseOrderMemoRepository iERPPurchaseOrderMemoRepository = (base.ERPPurchaseOrderMemoRepository = new ERPPurchaseOrderMemoRepository(base.ApiClientContext));
		using (iERPPurchaseOrderMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchaseOrderMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchaseOrderMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchaseOrderMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchaseOrderMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderMemo(Guid purchaseOrderMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderMemoRepository iERPPurchaseOrderMemoRepository = (base.ERPPurchaseOrderMemoRepository = new ERPPurchaseOrderMemoRepository(base.ApiClientContext));
		using (iERPPurchaseOrderMemoRepository)
		{
			if (!(await base.ERPPurchaseOrderMemoRepository.DoesPurchaseOrderMemoExist(purchaseOrderMemoId)))
			{
				errorsList.Add($"PurchaseOrderMemo [{purchaseOrderMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderMemo(ERPPurchaseOrderMemoDto purchaseOrderMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderMemoRepository iERPPurchaseOrderMemoRepository = (base.ERPPurchaseOrderMemoRepository = new ERPPurchaseOrderMemoRepository(base.ApiClientContext));
		using (iERPPurchaseOrderMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchaseOrderMemo.pmkPurchaseOrderID) && !(await base.ERPPurchaseOrderMemoRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { purchaseOrderMemo.pmkPurchaseOrderID })))
			{
				errorsList.Add("pmkPurchaseOrderID [" + purchaseOrderMemo.pmkPurchaseOrderID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchaseOrderMemoDto>>> Process_GetAllPurchaseOrderMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchaseOrderMemoDto> allPurchaseOrderMemosDto = new List<ERPPurchaseOrderMemoDto>();
		ERPResponseMessageDto<IList<ERPPurchaseOrderMemoDto>> result;
		try
		{
			IERPPurchaseOrderMemoRepository iERPPurchaseOrderMemoRepository = (base.ERPPurchaseOrderMemoRepository = new ERPPurchaseOrderMemoRepository(base.ApiClientContext));
			using (iERPPurchaseOrderMemoRepository)
			{
				foreach (ERPPurchaseOrderMemoInformationDto item2 in await base.ERPPurchaseOrderMemoRepository.GetAllPurchaseOrderMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchaseOrderMemoDto item = new ERPPurchaseOrderMemoDto
					{
						pmkCreatedBy = item2.pmkCreatedBy,
						pmkCreatedDate = item2.pmkCreatedDate,
						pmkUniqueID = item2.pmkUniqueID,
						pmkClosed = item2.pmkClosed,
						pmkLongDescriptionRtf = item2.pmkLongDescriptionRtf,
						pmkLongDescriptionText = item2.pmkLongDescriptionText,
						pmkMemoDate = item2.pmkMemoDate,
						pmkPurchaseOrderID = item2.pmkPurchaseOrderID,
						pmkRowVersion = item2.pmkRowVersion,
						pmkPurchaseOrderMemoID = item2.pmkPurchaseOrderMemoID,
						pmkShortDescription = item2.pmkShortDescription,
						pmkShowInApInvoices = item2.pmkShowInApInvoices,
						pmkShowInPurchaseOrders = item2.pmkShowInPurchaseOrders,
						pmkShowInReceipts = item2.pmkShowInReceipts,
						CustomFields = item2.CustomFields
					};
					allPurchaseOrderMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchaseOrderMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchaseOrderMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchaseOrderMemosDto,
				RecordCount = allPurchaseOrderMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderMemoDto>> Process_GetPurchaseOrderMemo(Guid purchaseOrderMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchaseOrderMemoDto purchaseOrderMemoDto = null;
		ERPResponseMessageDto<ERPPurchaseOrderMemoDto> result;
		try
		{
			IERPPurchaseOrderMemoRepository iERPPurchaseOrderMemoRepository = (base.ERPPurchaseOrderMemoRepository = new ERPPurchaseOrderMemoRepository(base.ApiClientContext));
			using (iERPPurchaseOrderMemoRepository)
			{
				ERPPurchaseOrderMemoInformationDto eRPPurchaseOrderMemoInformationDto = await base.ERPPurchaseOrderMemoRepository.GetPurchaseOrderMemo(purchaseOrderMemoId);
				purchaseOrderMemoDto = new ERPPurchaseOrderMemoDto
				{
					pmkCreatedBy = eRPPurchaseOrderMemoInformationDto.pmkCreatedBy,
					pmkCreatedDate = eRPPurchaseOrderMemoInformationDto.pmkCreatedDate,
					pmkUniqueID = eRPPurchaseOrderMemoInformationDto.pmkUniqueID,
					pmkClosed = eRPPurchaseOrderMemoInformationDto.pmkClosed,
					pmkLongDescriptionRtf = eRPPurchaseOrderMemoInformationDto.pmkLongDescriptionRtf,
					pmkLongDescriptionText = eRPPurchaseOrderMemoInformationDto.pmkLongDescriptionText,
					pmkMemoDate = eRPPurchaseOrderMemoInformationDto.pmkMemoDate,
					pmkPurchaseOrderID = eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderID,
					pmkRowVersion = eRPPurchaseOrderMemoInformationDto.pmkRowVersion,
					pmkPurchaseOrderMemoID = eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderMemoID,
					pmkShortDescription = eRPPurchaseOrderMemoInformationDto.pmkShortDescription,
					pmkShowInApInvoices = eRPPurchaseOrderMemoInformationDto.pmkShowInApInvoices,
					pmkShowInPurchaseOrders = eRPPurchaseOrderMemoInformationDto.pmkShowInPurchaseOrders,
					pmkShowInReceipts = eRPPurchaseOrderMemoInformationDto.pmkShowInReceipts,
					CustomFields = eRPPurchaseOrderMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchaseOrderMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchaseOrderMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderMemoDto>> Process_PutPurchaseOrderMemo(ERPPurchaseOrderMemoDto purchaseOrderMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchaseOrderMemoDto createdObject = null;
		ERPResponseMessageDto<ERPPurchaseOrderMemoDto> result;
		try
		{
			IERPPurchaseOrderMemoRepository iERPPurchaseOrderMemoRepository = (base.ERPPurchaseOrderMemoRepository = new ERPPurchaseOrderMemoRepository(base.ApiClientContext));
			using (iERPPurchaseOrderMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchaseOrderMemoRepository.SavePurchaseOrderMemo(purchaseOrderMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchaseOrderMemoInformationDto eRPPurchaseOrderMemoInformationDto = await base.ERPPurchaseOrderMemoRepository.GetPurchaseOrderMemo(purchaseOrderMemo.pmkUniqueID);
					createdObject = new ERPPurchaseOrderMemoDto
					{
						pmkCreatedBy = eRPPurchaseOrderMemoInformationDto.pmkCreatedBy,
						pmkCreatedDate = eRPPurchaseOrderMemoInformationDto.pmkCreatedDate,
						pmkUniqueID = eRPPurchaseOrderMemoInformationDto.pmkUniqueID,
						pmkClosed = eRPPurchaseOrderMemoInformationDto.pmkClosed,
						pmkLongDescriptionRtf = eRPPurchaseOrderMemoInformationDto.pmkLongDescriptionRtf,
						pmkLongDescriptionText = eRPPurchaseOrderMemoInformationDto.pmkLongDescriptionText,
						pmkMemoDate = eRPPurchaseOrderMemoInformationDto.pmkMemoDate,
						pmkPurchaseOrderID = eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderID,
						pmkRowVersion = eRPPurchaseOrderMemoInformationDto.pmkRowVersion,
						pmkPurchaseOrderMemoID = eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderMemoID,
						pmkShortDescription = eRPPurchaseOrderMemoInformationDto.pmkShortDescription,
						pmkShowInApInvoices = eRPPurchaseOrderMemoInformationDto.pmkShowInApInvoices,
						pmkShowInPurchaseOrders = eRPPurchaseOrderMemoInformationDto.pmkShowInPurchaseOrders,
						pmkShowInReceipts = eRPPurchaseOrderMemoInformationDto.pmkShowInReceipts,
						CustomFields = eRPPurchaseOrderMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchaseOrderMemo [{purchaseOrderMemo.pmkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderMemo(Guid purchaseOrderMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderMemoRepository iERPPurchaseOrderMemoRepository = (base.ERPPurchaseOrderMemoRepository = new ERPPurchaseOrderMemoRepository(base.ApiClientContext));
		using (iERPPurchaseOrderMemoRepository)
		{
			if (!(await base.ERPPurchaseOrderMemoRepository.DoesPurchaseOrderMemoExist(purchaseOrderMemoId)))
			{
				base.ErrorsList.Add($"PurchaseOrderMemo [{purchaseOrderMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchaseOrderMemoInformationDto eRPPurchaseOrderMemoInformationDto = await base.ERPPurchaseOrderMemoRepository.GetPurchaseOrderMemo(purchaseOrderMemoId);
				string text = await base.ERPPurchaseOrderMemoRepository.WhereUsed("PurchaseOrderMemos", new object[2] { eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderID, eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderMemoID }, new object[2] { "pmkPurchaseOrderID", "pmkPurchaseOrderMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchaseOrderMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderMemoDto>> Process_DeletePurchaseOrderMemo(Guid purchaseOrderMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchaseOrderMemoDto> result;
		try
		{
			IERPPurchaseOrderMemoRepository iERPPurchaseOrderMemoRepository = (base.ERPPurchaseOrderMemoRepository = new ERPPurchaseOrderMemoRepository(base.ApiClientContext));
			using (iERPPurchaseOrderMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchaseOrderMemoRepository.DeleteRowFromTable("PurchaseOrderMemos", "pmk", purchaseOrderMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchaseOrderMemo [{purchaseOrderMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchaseOrderMemoDto()
			};
		}
		return result;
	}
}
