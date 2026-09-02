using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseTransferModel : ERPBaseModel, IERPWarehouseTransferModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseTransfers(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseTransferRepository iERPWarehouseTransferRepository = (base.ERPWarehouseTransferRepository = new ERPWarehouseTransferRepository(base.ApiClientContext));
		using (iERPWarehouseTransferRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseTransferRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseTransferRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseTransferRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseTransferRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseTransfer(Guid warehouseTransferId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseTransferRepository iERPWarehouseTransferRepository = (base.ERPWarehouseTransferRepository = new ERPWarehouseTransferRepository(base.ApiClientContext));
		using (iERPWarehouseTransferRepository)
		{
			if (!(await base.ERPWarehouseTransferRepository.DoesWarehouseTransferExist(warehouseTransferId)))
			{
				errorsList.Add($"WarehouseTransfer [{warehouseTransferId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseTransfer(ERPWarehouseTransferDto warehouseTransfer)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseTransferRepository iERPWarehouseTransferRepository = (base.ERPWarehouseTransferRepository = new ERPWarehouseTransferRepository(base.ApiClientContext));
		using (iERPWarehouseTransferRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseTransfer.mwpSourceWarehouseID) && !(await base.ERPWarehouseTransferRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { warehouseTransfer.mwpSourceWarehouseID })))
			{
				errorsList.Add("mwpSourceWarehouseID [" + warehouseTransfer.mwpSourceWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransfer.mwpDestinationWarehouseID) && !(await base.ERPWarehouseTransferRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { warehouseTransfer.mwpDestinationWarehouseID })))
			{
				errorsList.Add("mwpDestinationWarehouseID [" + warehouseTransfer.mwpDestinationWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransfer.mwpShippingMethodID) && !(await base.ERPWarehouseTransferRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { warehouseTransfer.mwpShippingMethodID })))
			{
				errorsList.Add("mwpShippingMethodID [" + warehouseTransfer.mwpShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseTransfer.mwpShippingPaymentTypeID) && !(await base.ERPWarehouseTransferRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { warehouseTransfer.mwpShippingPaymentTypeID })))
			{
				errorsList.Add("mwpShippingPaymentTypeID [" + warehouseTransfer.mwpShippingPaymentTypeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseTransferDto>>> Process_GetAllWarehouseTransfers(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseTransferDto> allWarehouseTransfersDto = new List<ERPWarehouseTransferDto>();
		ERPResponseMessageDto<IList<ERPWarehouseTransferDto>> result;
		try
		{
			IERPWarehouseTransferRepository iERPWarehouseTransferRepository = (base.ERPWarehouseTransferRepository = new ERPWarehouseTransferRepository(base.ApiClientContext));
			using (iERPWarehouseTransferRepository)
			{
				foreach (ERPWarehouseTransferInformationDto item2 in await base.ERPWarehouseTransferRepository.GetAllWarehouseTransfers(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseTransferDto item = new ERPWarehouseTransferDto
					{
						mwpClosedDate = item2.mwpClosedDate,
						mwpWarehouseTransferID = item2.mwpWarehouseTransferID,
						mwpCreatedBy = item2.mwpCreatedBy,
						mwpCreatedDate = item2.mwpCreatedDate,
						mwpDestinationWarehouseID = item2.mwpDestinationWarehouseID,
						mwpUniqueID = item2.mwpUniqueID,
						mwpFreightCharge = item2.mwpFreightCharge,
						mwpClosed = item2.mwpClosed,
						mwpPosted = item2.mwpPosted,
						mwpPrintLabels = item2.mwpPrintLabels,
						mwpPrintPacker = item2.mwpPrintPacker,
						mwpReversalEntry = item2.mwpReversalEntry,
						mwpReversed = item2.mwpReversed,
						mwpNumberOfLabels = item2.mwpNumberOfLabels,
						mwpPostedDate = item2.mwpPostedDate,
						mwpRowVersion = item2.mwpRowVersion,
						mwpShipDate = item2.mwpShipDate,
						mwpShippingCommentsRTF = item2.mwpShippingCommentsRTF,
						mwpShippingCommentsText = item2.mwpShippingCommentsText,
						mwpShippingMethodID = item2.mwpShippingMethodID,
						mwpShippingPaymentTypeID = item2.mwpShippingPaymentTypeID,
						mwpSourceWarehouseID = item2.mwpSourceWarehouseID,
						mwpTrackingNumber = item2.mwpTrackingNumber,
						CustomFields = item2.CustomFields
					};
					allWarehouseTransfersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseTransfers]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseTransferDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseTransfersDto,
				RecordCount = allWarehouseTransfersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseTransferDto>> Process_GetWarehouseTransfer(Guid warehouseTransferId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseTransferDto warehouseTransferDto = null;
		ERPResponseMessageDto<ERPWarehouseTransferDto> result;
		try
		{
			IERPWarehouseTransferRepository iERPWarehouseTransferRepository = (base.ERPWarehouseTransferRepository = new ERPWarehouseTransferRepository(base.ApiClientContext));
			using (iERPWarehouseTransferRepository)
			{
				ERPWarehouseTransferInformationDto eRPWarehouseTransferInformationDto = await base.ERPWarehouseTransferRepository.GetWarehouseTransfer(warehouseTransferId);
				warehouseTransferDto = new ERPWarehouseTransferDto
				{
					mwpClosedDate = eRPWarehouseTransferInformationDto.mwpClosedDate,
					mwpWarehouseTransferID = eRPWarehouseTransferInformationDto.mwpWarehouseTransferID,
					mwpCreatedBy = eRPWarehouseTransferInformationDto.mwpCreatedBy,
					mwpCreatedDate = eRPWarehouseTransferInformationDto.mwpCreatedDate,
					mwpDestinationWarehouseID = eRPWarehouseTransferInformationDto.mwpDestinationWarehouseID,
					mwpUniqueID = eRPWarehouseTransferInformationDto.mwpUniqueID,
					mwpFreightCharge = eRPWarehouseTransferInformationDto.mwpFreightCharge,
					mwpClosed = eRPWarehouseTransferInformationDto.mwpClosed,
					mwpPosted = eRPWarehouseTransferInformationDto.mwpPosted,
					mwpPrintLabels = eRPWarehouseTransferInformationDto.mwpPrintLabels,
					mwpPrintPacker = eRPWarehouseTransferInformationDto.mwpPrintPacker,
					mwpReversalEntry = eRPWarehouseTransferInformationDto.mwpReversalEntry,
					mwpReversed = eRPWarehouseTransferInformationDto.mwpReversed,
					mwpNumberOfLabels = eRPWarehouseTransferInformationDto.mwpNumberOfLabels,
					mwpPostedDate = eRPWarehouseTransferInformationDto.mwpPostedDate,
					mwpRowVersion = eRPWarehouseTransferInformationDto.mwpRowVersion,
					mwpShipDate = eRPWarehouseTransferInformationDto.mwpShipDate,
					mwpShippingCommentsRTF = eRPWarehouseTransferInformationDto.mwpShippingCommentsRTF,
					mwpShippingCommentsText = eRPWarehouseTransferInformationDto.mwpShippingCommentsText,
					mwpShippingMethodID = eRPWarehouseTransferInformationDto.mwpShippingMethodID,
					mwpShippingPaymentTypeID = eRPWarehouseTransferInformationDto.mwpShippingPaymentTypeID,
					mwpSourceWarehouseID = eRPWarehouseTransferInformationDto.mwpSourceWarehouseID,
					mwpTrackingNumber = eRPWarehouseTransferInformationDto.mwpTrackingNumber,
					CustomFields = eRPWarehouseTransferInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseTransfers []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseTransferDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseTransferDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseTransferDto>> Process_PutWarehouseTransfer(ERPWarehouseTransferDto warehouseTransfer)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseTransferDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseTransferDto> result;
		try
		{
			IERPWarehouseTransferRepository iERPWarehouseTransferRepository = (base.ERPWarehouseTransferRepository = new ERPWarehouseTransferRepository(base.ApiClientContext));
			using (iERPWarehouseTransferRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseTransferRepository.SaveWarehouseTransfer(warehouseTransfer);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseTransferInformationDto eRPWarehouseTransferInformationDto = await base.ERPWarehouseTransferRepository.GetWarehouseTransfer(warehouseTransfer.mwpUniqueID);
					createdObject = new ERPWarehouseTransferDto
					{
						mwpClosedDate = eRPWarehouseTransferInformationDto.mwpClosedDate,
						mwpWarehouseTransferID = eRPWarehouseTransferInformationDto.mwpWarehouseTransferID,
						mwpCreatedBy = eRPWarehouseTransferInformationDto.mwpCreatedBy,
						mwpCreatedDate = eRPWarehouseTransferInformationDto.mwpCreatedDate,
						mwpDestinationWarehouseID = eRPWarehouseTransferInformationDto.mwpDestinationWarehouseID,
						mwpUniqueID = eRPWarehouseTransferInformationDto.mwpUniqueID,
						mwpFreightCharge = eRPWarehouseTransferInformationDto.mwpFreightCharge,
						mwpClosed = eRPWarehouseTransferInformationDto.mwpClosed,
						mwpPosted = eRPWarehouseTransferInformationDto.mwpPosted,
						mwpPrintLabels = eRPWarehouseTransferInformationDto.mwpPrintLabels,
						mwpPrintPacker = eRPWarehouseTransferInformationDto.mwpPrintPacker,
						mwpReversalEntry = eRPWarehouseTransferInformationDto.mwpReversalEntry,
						mwpReversed = eRPWarehouseTransferInformationDto.mwpReversed,
						mwpNumberOfLabels = eRPWarehouseTransferInformationDto.mwpNumberOfLabels,
						mwpPostedDate = eRPWarehouseTransferInformationDto.mwpPostedDate,
						mwpRowVersion = eRPWarehouseTransferInformationDto.mwpRowVersion,
						mwpShipDate = eRPWarehouseTransferInformationDto.mwpShipDate,
						mwpShippingCommentsRTF = eRPWarehouseTransferInformationDto.mwpShippingCommentsRTF,
						mwpShippingCommentsText = eRPWarehouseTransferInformationDto.mwpShippingCommentsText,
						mwpShippingMethodID = eRPWarehouseTransferInformationDto.mwpShippingMethodID,
						mwpShippingPaymentTypeID = eRPWarehouseTransferInformationDto.mwpShippingPaymentTypeID,
						mwpSourceWarehouseID = eRPWarehouseTransferInformationDto.mwpSourceWarehouseID,
						mwpTrackingNumber = eRPWarehouseTransferInformationDto.mwpTrackingNumber,
						CustomFields = eRPWarehouseTransferInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseTransfer [{warehouseTransfer.mwpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseTransferDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseTransfer(Guid warehouseTransferId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseTransferRepository iERPWarehouseTransferRepository = (base.ERPWarehouseTransferRepository = new ERPWarehouseTransferRepository(base.ApiClientContext));
		using (iERPWarehouseTransferRepository)
		{
			if (!(await base.ERPWarehouseTransferRepository.DoesWarehouseTransferExist(warehouseTransferId)))
			{
				base.ErrorsList.Add($"WarehouseTransfer [{warehouseTransferId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseTransferInformationDto eRPWarehouseTransferInformationDto = await base.ERPWarehouseTransferRepository.GetWarehouseTransfer(warehouseTransferId);
				string text = await base.ERPWarehouseTransferRepository.WhereUsed("WarehouseTransfers", new object[1] { eRPWarehouseTransferInformationDto.mwpWarehouseTransferID }, new object[1] { "mwpWarehouseTransferID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseTransfer cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseTransferDto>> Process_DeleteWarehouseTransfer(Guid warehouseTransferId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseTransferDto> result;
		try
		{
			IERPWarehouseTransferRepository iERPWarehouseTransferRepository = (base.ERPWarehouseTransferRepository = new ERPWarehouseTransferRepository(base.ApiClientContext));
			using (iERPWarehouseTransferRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseTransferRepository.DeleteRowFromTable("WarehouseTransfers", "mwp", warehouseTransferId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseTransfer [{warehouseTransferId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseTransferDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseTransferDto()
			};
		}
		return result;
	}
}
