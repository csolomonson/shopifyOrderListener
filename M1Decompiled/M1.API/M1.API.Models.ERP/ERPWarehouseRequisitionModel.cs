using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseRequisitionModel : ERPBaseModel, IERPWarehouseRequisitionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseRequisitions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseRequisitionRepository iERPWarehouseRequisitionRepository = (base.ERPWarehouseRequisitionRepository = new ERPWarehouseRequisitionRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseRequisitionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseRequisitionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseRequisitionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseRequisitionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseRequisition(Guid warehouseRequisitionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRequisitionRepository iERPWarehouseRequisitionRepository = (base.ERPWarehouseRequisitionRepository = new ERPWarehouseRequisitionRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionRepository)
		{
			if (!(await base.ERPWarehouseRequisitionRepository.DoesWarehouseRequisitionExist(warehouseRequisitionId)))
			{
				errorsList.Add($"WarehouseRequisition [{warehouseRequisitionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseRequisition(ERPWarehouseRequisitionDto warehouseRequisition)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRequisitionRepository iERPWarehouseRequisitionRepository = (base.ERPWarehouseRequisitionRepository = new ERPWarehouseRequisitionRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseRequisition.wqpSourceWarehouseID) && !(await base.ERPWarehouseRequisitionRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { warehouseRequisition.wqpSourceWarehouseID })))
			{
				errorsList.Add("wqpSourceWarehouseID [" + warehouseRequisition.wqpSourceWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseRequisition.wqpDestinationWarehouseID) && !(await base.ERPWarehouseRequisitionRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { warehouseRequisition.wqpDestinationWarehouseID })))
			{
				errorsList.Add("wqpDestinationWarehouseID [" + warehouseRequisition.wqpDestinationWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseRequisition.wqpShippingMethodID) && !(await base.ERPWarehouseRequisitionRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { warehouseRequisition.wqpShippingMethodID })))
			{
				errorsList.Add("wqpShippingMethodID [" + warehouseRequisition.wqpShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseRequisition.wqpShippingPaymentTypeID) && !(await base.ERPWarehouseRequisitionRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { warehouseRequisition.wqpShippingPaymentTypeID })))
			{
				errorsList.Add("wqpShippingPaymentTypeID [" + warehouseRequisition.wqpShippingPaymentTypeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseRequisitionDto>>> Process_GetAllWarehouseRequisitions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseRequisitionDto> allWarehouseRequisitionsDto = new List<ERPWarehouseRequisitionDto>();
		ERPResponseMessageDto<IList<ERPWarehouseRequisitionDto>> result;
		try
		{
			IERPWarehouseRequisitionRepository iERPWarehouseRequisitionRepository = (base.ERPWarehouseRequisitionRepository = new ERPWarehouseRequisitionRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionRepository)
			{
				foreach (ERPWarehouseRequisitionInformationDto item2 in await base.ERPWarehouseRequisitionRepository.GetAllWarehouseRequisitions(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseRequisitionDto item = new ERPWarehouseRequisitionDto
					{
						wqpClosedDate = item2.wqpClosedDate,
						wqpWarehouseRequisitionID = item2.wqpWarehouseRequisitionID,
						wqpCreatedBy = item2.wqpCreatedBy,
						wqpCreatedDate = item2.wqpCreatedDate,
						wqpDestinationWarehouseID = item2.wqpDestinationWarehouseID,
						wqpUniqueID = item2.wqpUniqueID,
						wqpClosed = item2.wqpClosed,
						wqpReadyToPrint = item2.wqpReadyToPrint,
						wqpRequestedShipDate = item2.wqpRequestedShipDate,
						wqpRequisitionCommentsRTF = item2.wqpRequisitionCommentsRTF,
						wqpRequisitionCommentsText = item2.wqpRequisitionCommentsText,
						wqpRequisitionDate = item2.wqpRequisitionDate,
						wqpRowVersion = item2.wqpRowVersion,
						wqpShippingMethodID = item2.wqpShippingMethodID,
						wqpShippingPaymentTypeID = item2.wqpShippingPaymentTypeID,
						wqpSourceWarehouseID = item2.wqpSourceWarehouseID,
						wqpStatus = item2.wqpStatus,
						CustomFields = item2.CustomFields
					};
					allWarehouseRequisitionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseRequisitions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseRequisitionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseRequisitionsDto,
				RecordCount = allWarehouseRequisitionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseRequisitionDto>> Process_GetWarehouseRequisition(Guid warehouseRequisitionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseRequisitionDto warehouseRequisitionDto = null;
		ERPResponseMessageDto<ERPWarehouseRequisitionDto> result;
		try
		{
			IERPWarehouseRequisitionRepository iERPWarehouseRequisitionRepository = (base.ERPWarehouseRequisitionRepository = new ERPWarehouseRequisitionRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionRepository)
			{
				ERPWarehouseRequisitionInformationDto eRPWarehouseRequisitionInformationDto = await base.ERPWarehouseRequisitionRepository.GetWarehouseRequisition(warehouseRequisitionId);
				warehouseRequisitionDto = new ERPWarehouseRequisitionDto
				{
					wqpClosedDate = eRPWarehouseRequisitionInformationDto.wqpClosedDate,
					wqpWarehouseRequisitionID = eRPWarehouseRequisitionInformationDto.wqpWarehouseRequisitionID,
					wqpCreatedBy = eRPWarehouseRequisitionInformationDto.wqpCreatedBy,
					wqpCreatedDate = eRPWarehouseRequisitionInformationDto.wqpCreatedDate,
					wqpDestinationWarehouseID = eRPWarehouseRequisitionInformationDto.wqpDestinationWarehouseID,
					wqpUniqueID = eRPWarehouseRequisitionInformationDto.wqpUniqueID,
					wqpClosed = eRPWarehouseRequisitionInformationDto.wqpClosed,
					wqpReadyToPrint = eRPWarehouseRequisitionInformationDto.wqpReadyToPrint,
					wqpRequestedShipDate = eRPWarehouseRequisitionInformationDto.wqpRequestedShipDate,
					wqpRequisitionCommentsRTF = eRPWarehouseRequisitionInformationDto.wqpRequisitionCommentsRTF,
					wqpRequisitionCommentsText = eRPWarehouseRequisitionInformationDto.wqpRequisitionCommentsText,
					wqpRequisitionDate = eRPWarehouseRequisitionInformationDto.wqpRequisitionDate,
					wqpRowVersion = eRPWarehouseRequisitionInformationDto.wqpRowVersion,
					wqpShippingMethodID = eRPWarehouseRequisitionInformationDto.wqpShippingMethodID,
					wqpShippingPaymentTypeID = eRPWarehouseRequisitionInformationDto.wqpShippingPaymentTypeID,
					wqpSourceWarehouseID = eRPWarehouseRequisitionInformationDto.wqpSourceWarehouseID,
					wqpStatus = eRPWarehouseRequisitionInformationDto.wqpStatus,
					CustomFields = eRPWarehouseRequisitionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseRequisitions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseRequisitionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseRequisitionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseRequisitionDto>> Process_PutWarehouseRequisition(ERPWarehouseRequisitionDto warehouseRequisition)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseRequisitionDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseRequisitionDto> result;
		try
		{
			IERPWarehouseRequisitionRepository iERPWarehouseRequisitionRepository = (base.ERPWarehouseRequisitionRepository = new ERPWarehouseRequisitionRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseRequisitionRepository.SaveWarehouseRequisition(warehouseRequisition);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseRequisitionInformationDto eRPWarehouseRequisitionInformationDto = await base.ERPWarehouseRequisitionRepository.GetWarehouseRequisition(warehouseRequisition.wqpUniqueID);
					createdObject = new ERPWarehouseRequisitionDto
					{
						wqpClosedDate = eRPWarehouseRequisitionInformationDto.wqpClosedDate,
						wqpWarehouseRequisitionID = eRPWarehouseRequisitionInformationDto.wqpWarehouseRequisitionID,
						wqpCreatedBy = eRPWarehouseRequisitionInformationDto.wqpCreatedBy,
						wqpCreatedDate = eRPWarehouseRequisitionInformationDto.wqpCreatedDate,
						wqpDestinationWarehouseID = eRPWarehouseRequisitionInformationDto.wqpDestinationWarehouseID,
						wqpUniqueID = eRPWarehouseRequisitionInformationDto.wqpUniqueID,
						wqpClosed = eRPWarehouseRequisitionInformationDto.wqpClosed,
						wqpReadyToPrint = eRPWarehouseRequisitionInformationDto.wqpReadyToPrint,
						wqpRequestedShipDate = eRPWarehouseRequisitionInformationDto.wqpRequestedShipDate,
						wqpRequisitionCommentsRTF = eRPWarehouseRequisitionInformationDto.wqpRequisitionCommentsRTF,
						wqpRequisitionCommentsText = eRPWarehouseRequisitionInformationDto.wqpRequisitionCommentsText,
						wqpRequisitionDate = eRPWarehouseRequisitionInformationDto.wqpRequisitionDate,
						wqpRowVersion = eRPWarehouseRequisitionInformationDto.wqpRowVersion,
						wqpShippingMethodID = eRPWarehouseRequisitionInformationDto.wqpShippingMethodID,
						wqpShippingPaymentTypeID = eRPWarehouseRequisitionInformationDto.wqpShippingPaymentTypeID,
						wqpSourceWarehouseID = eRPWarehouseRequisitionInformationDto.wqpSourceWarehouseID,
						wqpStatus = eRPWarehouseRequisitionInformationDto.wqpStatus,
						CustomFields = eRPWarehouseRequisitionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseRequisition [{warehouseRequisition.wqpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseRequisitionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseRequisition(Guid warehouseRequisitionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseRequisitionRepository iERPWarehouseRequisitionRepository = (base.ERPWarehouseRequisitionRepository = new ERPWarehouseRequisitionRepository(base.ApiClientContext));
		using (iERPWarehouseRequisitionRepository)
		{
			if (!(await base.ERPWarehouseRequisitionRepository.DoesWarehouseRequisitionExist(warehouseRequisitionId)))
			{
				base.ErrorsList.Add($"WarehouseRequisition [{warehouseRequisitionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseRequisitionInformationDto eRPWarehouseRequisitionInformationDto = await base.ERPWarehouseRequisitionRepository.GetWarehouseRequisition(warehouseRequisitionId);
				string text = await base.ERPWarehouseRequisitionRepository.WhereUsed("WarehouseRequisitions", new object[1] { eRPWarehouseRequisitionInformationDto.wqpWarehouseRequisitionID }, new object[1] { "wqpWarehouseRequisitionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseRequisition cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseRequisitionDto>> Process_DeleteWarehouseRequisition(Guid warehouseRequisitionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseRequisitionDto> result;
		try
		{
			IERPWarehouseRequisitionRepository iERPWarehouseRequisitionRepository = (base.ERPWarehouseRequisitionRepository = new ERPWarehouseRequisitionRepository(base.ApiClientContext));
			using (iERPWarehouseRequisitionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseRequisitionRepository.DeleteRowFromTable("WarehouseRequisitions", "wqp", warehouseRequisitionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseRequisition [{warehouseRequisitionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseRequisitionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseRequisitionDto()
			};
		}
		return result;
	}
}
