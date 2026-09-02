using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseReceiptModel : ERPBaseModel, IERPWarehouseReceiptModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseReceipts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseReceiptRepository iERPWarehouseReceiptRepository = (base.ERPWarehouseReceiptRepository = new ERPWarehouseReceiptRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseReceiptRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseReceiptRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseReceiptRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseReceiptRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseReceipt(Guid warehouseReceiptId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseReceiptRepository iERPWarehouseReceiptRepository = (base.ERPWarehouseReceiptRepository = new ERPWarehouseReceiptRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptRepository)
		{
			if (!(await base.ERPWarehouseReceiptRepository.DoesWarehouseReceiptExist(warehouseReceiptId)))
			{
				errorsList.Add($"WarehouseReceipt [{warehouseReceiptId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseReceipt(ERPWarehouseReceiptDto warehouseReceipt)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseReceiptRepository iERPWarehouseReceiptRepository = (base.ERPWarehouseReceiptRepository = new ERPWarehouseReceiptRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseReceipt.wrpSourceWarehouseID) && !(await base.ERPWarehouseReceiptRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { warehouseReceipt.wrpSourceWarehouseID })))
			{
				errorsList.Add("wrpSourceWarehouseID [" + warehouseReceipt.wrpSourceWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceipt.wrpDestinationWarehouseID) && !(await base.ERPWarehouseReceiptRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { warehouseReceipt.wrpDestinationWarehouseID })))
			{
				errorsList.Add("wrpDestinationWarehouseID [" + warehouseReceipt.wrpDestinationWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceipt.wrpShippingMethodID) && !(await base.ERPWarehouseReceiptRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { warehouseReceipt.wrpShippingMethodID })))
			{
				errorsList.Add("wrpShippingMethodID [" + warehouseReceipt.wrpShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceipt.wrpShippingPaymentTypeID) && !(await base.ERPWarehouseReceiptRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { warehouseReceipt.wrpShippingPaymentTypeID })))
			{
				errorsList.Add("wrpShippingPaymentTypeID [" + warehouseReceipt.wrpShippingPaymentTypeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseReceiptDto>>> Process_GetAllWarehouseReceipts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseReceiptDto> allWarehouseReceiptsDto = new List<ERPWarehouseReceiptDto>();
		ERPResponseMessageDto<IList<ERPWarehouseReceiptDto>> result;
		try
		{
			IERPWarehouseReceiptRepository iERPWarehouseReceiptRepository = (base.ERPWarehouseReceiptRepository = new ERPWarehouseReceiptRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptRepository)
			{
				foreach (ERPWarehouseReceiptInformationDto item2 in await base.ERPWarehouseReceiptRepository.GetAllWarehouseReceipts(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseReceiptDto item = new ERPWarehouseReceiptDto
					{
						wrpClosedDate = item2.wrpClosedDate,
						wrpWarehouseReceiptID = item2.wrpWarehouseReceiptID,
						wrpCreatedBy = item2.wrpCreatedBy,
						wrpCreatedDate = item2.wrpCreatedDate,
						wrpDestinationWarehouseID = item2.wrpDestinationWarehouseID,
						wrpUniqueID = item2.wrpUniqueID,
						wrpFreightCharge = item2.wrpFreightCharge,
						wrpClosed = item2.wrpClosed,
						wrpPosted = item2.wrpPosted,
						wrpReversalEntry = item2.wrpReversalEntry,
						wrpReversed = item2.wrpReversed,
						wrpPostedDate = item2.wrpPostedDate,
						wrpReceiptDate = item2.wrpReceiptDate,
						wrpRowVersion = item2.wrpRowVersion,
						wrpShippingMethodID = item2.wrpShippingMethodID,
						wrpShippingPaymentTypeID = item2.wrpShippingPaymentTypeID,
						wrpSourceWarehouseID = item2.wrpSourceWarehouseID,
						CustomFields = item2.CustomFields
					};
					allWarehouseReceiptsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseReceipts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseReceiptDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseReceiptsDto,
				RecordCount = allWarehouseReceiptsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseReceiptDto>> Process_GetWarehouseReceipt(Guid warehouseReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseReceiptDto warehouseReceiptDto = null;
		ERPResponseMessageDto<ERPWarehouseReceiptDto> result;
		try
		{
			IERPWarehouseReceiptRepository iERPWarehouseReceiptRepository = (base.ERPWarehouseReceiptRepository = new ERPWarehouseReceiptRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptRepository)
			{
				ERPWarehouseReceiptInformationDto eRPWarehouseReceiptInformationDto = await base.ERPWarehouseReceiptRepository.GetWarehouseReceipt(warehouseReceiptId);
				warehouseReceiptDto = new ERPWarehouseReceiptDto
				{
					wrpClosedDate = eRPWarehouseReceiptInformationDto.wrpClosedDate,
					wrpWarehouseReceiptID = eRPWarehouseReceiptInformationDto.wrpWarehouseReceiptID,
					wrpCreatedBy = eRPWarehouseReceiptInformationDto.wrpCreatedBy,
					wrpCreatedDate = eRPWarehouseReceiptInformationDto.wrpCreatedDate,
					wrpDestinationWarehouseID = eRPWarehouseReceiptInformationDto.wrpDestinationWarehouseID,
					wrpUniqueID = eRPWarehouseReceiptInformationDto.wrpUniqueID,
					wrpFreightCharge = eRPWarehouseReceiptInformationDto.wrpFreightCharge,
					wrpClosed = eRPWarehouseReceiptInformationDto.wrpClosed,
					wrpPosted = eRPWarehouseReceiptInformationDto.wrpPosted,
					wrpReversalEntry = eRPWarehouseReceiptInformationDto.wrpReversalEntry,
					wrpReversed = eRPWarehouseReceiptInformationDto.wrpReversed,
					wrpPostedDate = eRPWarehouseReceiptInformationDto.wrpPostedDate,
					wrpReceiptDate = eRPWarehouseReceiptInformationDto.wrpReceiptDate,
					wrpRowVersion = eRPWarehouseReceiptInformationDto.wrpRowVersion,
					wrpShippingMethodID = eRPWarehouseReceiptInformationDto.wrpShippingMethodID,
					wrpShippingPaymentTypeID = eRPWarehouseReceiptInformationDto.wrpShippingPaymentTypeID,
					wrpSourceWarehouseID = eRPWarehouseReceiptInformationDto.wrpSourceWarehouseID,
					CustomFields = eRPWarehouseReceiptInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseReceipts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseReceiptDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseReceiptDto>> Process_PutWarehouseReceipt(ERPWarehouseReceiptDto warehouseReceipt)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseReceiptDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseReceiptDto> result;
		try
		{
			IERPWarehouseReceiptRepository iERPWarehouseReceiptRepository = (base.ERPWarehouseReceiptRepository = new ERPWarehouseReceiptRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseReceiptRepository.SaveWarehouseReceipt(warehouseReceipt);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseReceiptInformationDto eRPWarehouseReceiptInformationDto = await base.ERPWarehouseReceiptRepository.GetWarehouseReceipt(warehouseReceipt.wrpUniqueID);
					createdObject = new ERPWarehouseReceiptDto
					{
						wrpClosedDate = eRPWarehouseReceiptInformationDto.wrpClosedDate,
						wrpWarehouseReceiptID = eRPWarehouseReceiptInformationDto.wrpWarehouseReceiptID,
						wrpCreatedBy = eRPWarehouseReceiptInformationDto.wrpCreatedBy,
						wrpCreatedDate = eRPWarehouseReceiptInformationDto.wrpCreatedDate,
						wrpDestinationWarehouseID = eRPWarehouseReceiptInformationDto.wrpDestinationWarehouseID,
						wrpUniqueID = eRPWarehouseReceiptInformationDto.wrpUniqueID,
						wrpFreightCharge = eRPWarehouseReceiptInformationDto.wrpFreightCharge,
						wrpClosed = eRPWarehouseReceiptInformationDto.wrpClosed,
						wrpPosted = eRPWarehouseReceiptInformationDto.wrpPosted,
						wrpReversalEntry = eRPWarehouseReceiptInformationDto.wrpReversalEntry,
						wrpReversed = eRPWarehouseReceiptInformationDto.wrpReversed,
						wrpPostedDate = eRPWarehouseReceiptInformationDto.wrpPostedDate,
						wrpReceiptDate = eRPWarehouseReceiptInformationDto.wrpReceiptDate,
						wrpRowVersion = eRPWarehouseReceiptInformationDto.wrpRowVersion,
						wrpShippingMethodID = eRPWarehouseReceiptInformationDto.wrpShippingMethodID,
						wrpShippingPaymentTypeID = eRPWarehouseReceiptInformationDto.wrpShippingPaymentTypeID,
						wrpSourceWarehouseID = eRPWarehouseReceiptInformationDto.wrpSourceWarehouseID,
						CustomFields = eRPWarehouseReceiptInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseReceipt [{warehouseReceipt.wrpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseReceipt(Guid warehouseReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseReceiptRepository iERPWarehouseReceiptRepository = (base.ERPWarehouseReceiptRepository = new ERPWarehouseReceiptRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptRepository)
		{
			if (!(await base.ERPWarehouseReceiptRepository.DoesWarehouseReceiptExist(warehouseReceiptId)))
			{
				base.ErrorsList.Add($"WarehouseReceipt [{warehouseReceiptId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseReceiptInformationDto eRPWarehouseReceiptInformationDto = await base.ERPWarehouseReceiptRepository.GetWarehouseReceipt(warehouseReceiptId);
				string text = await base.ERPWarehouseReceiptRepository.WhereUsed("WarehouseReceipts", new object[1] { eRPWarehouseReceiptInformationDto.wrpWarehouseReceiptID }, new object[1] { "wrpWarehouseReceiptID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseReceipt cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseReceiptDto>> Process_DeleteWarehouseReceipt(Guid warehouseReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseReceiptDto> result;
		try
		{
			IERPWarehouseReceiptRepository iERPWarehouseReceiptRepository = (base.ERPWarehouseReceiptRepository = new ERPWarehouseReceiptRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseReceiptRepository.DeleteRowFromTable("WarehouseReceipts", "wrp", warehouseReceiptId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseReceipt [{warehouseReceiptId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseReceiptDto()
			};
		}
		return result;
	}
}
