using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderDeliveryModel : ERPBaseModel, IERPSalesOrderDeliveryModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderDeliveries(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderDeliveryRepository iERPSalesOrderDeliveryRepository = (base.ERPSalesOrderDeliveryRepository = new ERPSalesOrderDeliveryRepository(base.ApiClientContext));
		using (iERPSalesOrderDeliveryRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderDeliveryRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderDeliveryRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderDeliveryRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderDeliveryRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderDelivery(Guid salesOrderDeliveryId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderDeliveryRepository iERPSalesOrderDeliveryRepository = (base.ERPSalesOrderDeliveryRepository = new ERPSalesOrderDeliveryRepository(base.ApiClientContext));
		using (iERPSalesOrderDeliveryRepository)
		{
			if (!(await base.ERPSalesOrderDeliveryRepository.DoesSalesOrderDeliveryExist(salesOrderDeliveryId)))
			{
				errorsList.Add($"SalesOrderDelivery [{salesOrderDeliveryId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderDelivery(ERPSalesOrderDeliveryDto salesOrderDelivery)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderDeliveryRepository iERPSalesOrderDeliveryRepository = (base.ERPSalesOrderDeliveryRepository = new ERPSalesOrderDeliveryRepository(base.ApiClientContext));
		using (iERPSalesOrderDeliveryRepository)
		{
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdSalesOrderID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { salesOrderDelivery.omdSalesOrderID })))
			{
				errorsList.Add("omdSalesOrderID [" + salesOrderDelivery.omdSalesOrderID + "] not found.");
			}
			if (salesOrderDelivery.omdSalesOrderLineID > 0 && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { salesOrderDelivery.omdSalesOrderID, salesOrderDelivery.omdSalesOrderLineID })))
			{
				errorsList.Add($"omdSalesOrderLineID [{salesOrderDelivery.omdSalesOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdPartID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { salesOrderDelivery.omdPartID })))
			{
				errorsList.Add("omdPartID [" + salesOrderDelivery.omdPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdPartRevisionID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { salesOrderDelivery.omdPartID, salesOrderDelivery.omdPartRevisionID })))
			{
				errorsList.Add("omdPartRevisionID [" + salesOrderDelivery.omdPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdPartWarehouseLocationID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { salesOrderDelivery.omdPartID, salesOrderDelivery.omdPartRevisionID, salesOrderDelivery.omdPartWarehouseLocationID })))
			{
				errorsList.Add("omdPartWarehouseLocationID [" + salesOrderDelivery.omdPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdPartBinID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { salesOrderDelivery.omdPartID, salesOrderDelivery.omdPartRevisionID, salesOrderDelivery.omdPartWarehouseLocationID, salesOrderDelivery.omdPartBinID })))
			{
				errorsList.Add("omdPartBinID [" + salesOrderDelivery.omdPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdCustomerOrganizationID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { salesOrderDelivery.omdCustomerOrganizationID })))
			{
				errorsList.Add("omdCustomerOrganizationID [" + salesOrderDelivery.omdCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdShipLocationID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { salesOrderDelivery.omdCustomerOrganizationID, salesOrderDelivery.omdShipLocationID })))
			{
				errorsList.Add("omdShipLocationID [" + salesOrderDelivery.omdShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdShipContactID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { salesOrderDelivery.omdCustomerOrganizationID, salesOrderDelivery.omdShipLocationID, salesOrderDelivery.omdShipContactID })))
			{
				errorsList.Add("omdShipContactID [" + salesOrderDelivery.omdShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdShippingMethodID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { salesOrderDelivery.omdShippingMethodID })))
			{
				errorsList.Add("omdShippingMethodID [" + salesOrderDelivery.omdShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdShippingPaymentTypeID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { salesOrderDelivery.omdShippingPaymentTypeID })))
			{
				errorsList.Add("omdShippingPaymentTypeID [" + salesOrderDelivery.omdShippingPaymentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdSupplierOrganizationID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { salesOrderDelivery.omdSupplierOrganizationID })))
			{
				errorsList.Add("omdSupplierOrganizationID [" + salesOrderDelivery.omdSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdPurchaseLocationID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { salesOrderDelivery.omdSupplierOrganizationID, salesOrderDelivery.omdPurchaseLocationID })))
			{
				errorsList.Add("omdPurchaseLocationID [" + salesOrderDelivery.omdPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderDelivery.omdAvalaraNonTaxReasonID) && !(await base.ERPSalesOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("REASONS", new object[1] { "XARREASONID" }, new object[1] { salesOrderDelivery.omdAvalaraNonTaxReasonID })))
			{
				errorsList.Add("omdAvalaraNonTaxReasonID [" + salesOrderDelivery.omdAvalaraNonTaxReasonID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderDeliveryDto>>> Process_GetAllSalesOrderDeliveries(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderDeliveryDto> allSalesOrderDeliveriesDto = new List<ERPSalesOrderDeliveryDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderDeliveryDto>> result;
		try
		{
			IERPSalesOrderDeliveryRepository iERPSalesOrderDeliveryRepository = (base.ERPSalesOrderDeliveryRepository = new ERPSalesOrderDeliveryRepository(base.ApiClientContext));
			using (iERPSalesOrderDeliveryRepository)
			{
				foreach (ERPSalesOrderDeliveryInformationDto item2 in await base.ERPSalesOrderDeliveryRepository.GetAllSalesOrderDeliveries(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderDeliveryDto item = new ERPSalesOrderDeliveryDto
					{
						omdAmountToInvoice = item2.omdAmountToInvoice,
						omdAmountToInvoiceForeign = item2.omdAmountToInvoiceForeign,
						omdAvalaraNonTaxReasonID = item2.omdAvalaraNonTaxReasonID,
						omdCreatedBy = item2.omdCreatedBy,
						omdCreatedDate = item2.omdCreatedDate,
						omdCustomerOrganizationID = item2.omdCustomerOrganizationID,
						omdDeliveryDate = item2.omdDeliveryDate,
						omdDeliveryQuantity = item2.omdDeliveryQuantity,
						omdDeliveryType = item2.omdDeliveryType,
						omdUniqueID = item2.omdUniqueID,
						omdExtendedWeight = item2.omdExtendedWeight,
						omdFreightAmountBase = item2.omdFreightAmountBase,
						omdFreightAmountForeign = item2.omdFreightAmountForeign,
						omdClosed = item2.omdClosed,
						omdDifferentLocation = item2.omdDifferentLocation,
						omdFirm = item2.omdFirm,
						omdInvoicedComplete = item2.omdInvoicedComplete,
						omdKitPart = item2.omdKitPart,
						omdPickInProgress = item2.omdPickInProgress,
						omdReceivedComplete = item2.omdReceivedComplete,
						omdRequiresInspection = item2.omdRequiresInspection,
						omdShippedComplete = item2.omdShippedComplete,
						omdPartBinID = item2.omdPartBinID,
						omdPartID = item2.omdPartID,
						omdPartRevisionID = item2.omdPartRevisionID,
						omdPartWarehouseLocationID = item2.omdPartWarehouseLocationID,
						omdPurchaseLocationID = item2.omdPurchaseLocationID,
						omdPurchaseUnitCostBase = item2.omdPurchaseUnitCostBase,
						omdPurchaseUnitCostForeign = item2.omdPurchaseUnitCostForeign,
						omdQuantityAllocated = item2.omdQuantityAllocated,
						omdQuantityInvoiced = item2.omdQuantityInvoiced,
						omdQuantityOnOrder = item2.omdQuantityOnOrder,
						omdQuantityReceived = item2.omdQuantityReceived,
						omdQuantityShipped = item2.omdQuantityShipped,
						omdRowVersion = item2.omdRowVersion,
						omdSalesOrderID = item2.omdSalesOrderID,
						omdSalesOrderLineID = item2.omdSalesOrderLineID,
						omdSalesOrderDeliveryID = item2.omdSalesOrderDeliveryID,
						omdShipContactID = item2.omdShipContactID,
						omdShipLocationID = item2.omdShipLocationID,
						omdShippingMethodID = item2.omdShippingMethodID,
						omdShippingPaymentTypeID = item2.omdShippingPaymentTypeID,
						omdSupplierOrganizationID = item2.omdSupplierOrganizationID,
						omdWeight = item2.omdWeight,
						CustomFields = item2.CustomFields
					};
					allSalesOrderDeliveriesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrderDeliveries]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderDeliveryDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrderDeliveriesDto,
				RecordCount = allSalesOrderDeliveriesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderDeliveryDto>> Process_GetSalesOrderDelivery(Guid salesOrderDeliveryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderDeliveryDto salesOrderDeliveryDto = null;
		ERPResponseMessageDto<ERPSalesOrderDeliveryDto> result;
		try
		{
			IERPSalesOrderDeliveryRepository iERPSalesOrderDeliveryRepository = (base.ERPSalesOrderDeliveryRepository = new ERPSalesOrderDeliveryRepository(base.ApiClientContext));
			using (iERPSalesOrderDeliveryRepository)
			{
				ERPSalesOrderDeliveryInformationDto eRPSalesOrderDeliveryInformationDto = await base.ERPSalesOrderDeliveryRepository.GetSalesOrderDelivery(salesOrderDeliveryId);
				salesOrderDeliveryDto = new ERPSalesOrderDeliveryDto
				{
					omdAmountToInvoice = eRPSalesOrderDeliveryInformationDto.omdAmountToInvoice,
					omdAmountToInvoiceForeign = eRPSalesOrderDeliveryInformationDto.omdAmountToInvoiceForeign,
					omdAvalaraNonTaxReasonID = eRPSalesOrderDeliveryInformationDto.omdAvalaraNonTaxReasonID,
					omdCreatedBy = eRPSalesOrderDeliveryInformationDto.omdCreatedBy,
					omdCreatedDate = eRPSalesOrderDeliveryInformationDto.omdCreatedDate,
					omdCustomerOrganizationID = eRPSalesOrderDeliveryInformationDto.omdCustomerOrganizationID,
					omdDeliveryDate = eRPSalesOrderDeliveryInformationDto.omdDeliveryDate,
					omdDeliveryQuantity = eRPSalesOrderDeliveryInformationDto.omdDeliveryQuantity,
					omdDeliveryType = eRPSalesOrderDeliveryInformationDto.omdDeliveryType,
					omdUniqueID = eRPSalesOrderDeliveryInformationDto.omdUniqueID,
					omdExtendedWeight = eRPSalesOrderDeliveryInformationDto.omdExtendedWeight,
					omdFreightAmountBase = eRPSalesOrderDeliveryInformationDto.omdFreightAmountBase,
					omdFreightAmountForeign = eRPSalesOrderDeliveryInformationDto.omdFreightAmountForeign,
					omdClosed = eRPSalesOrderDeliveryInformationDto.omdClosed,
					omdDifferentLocation = eRPSalesOrderDeliveryInformationDto.omdDifferentLocation,
					omdFirm = eRPSalesOrderDeliveryInformationDto.omdFirm,
					omdInvoicedComplete = eRPSalesOrderDeliveryInformationDto.omdInvoicedComplete,
					omdKitPart = eRPSalesOrderDeliveryInformationDto.omdKitPart,
					omdPickInProgress = eRPSalesOrderDeliveryInformationDto.omdPickInProgress,
					omdReceivedComplete = eRPSalesOrderDeliveryInformationDto.omdReceivedComplete,
					omdRequiresInspection = eRPSalesOrderDeliveryInformationDto.omdRequiresInspection,
					omdShippedComplete = eRPSalesOrderDeliveryInformationDto.omdShippedComplete,
					omdPartBinID = eRPSalesOrderDeliveryInformationDto.omdPartBinID,
					omdPartID = eRPSalesOrderDeliveryInformationDto.omdPartID,
					omdPartRevisionID = eRPSalesOrderDeliveryInformationDto.omdPartRevisionID,
					omdPartWarehouseLocationID = eRPSalesOrderDeliveryInformationDto.omdPartWarehouseLocationID,
					omdPurchaseLocationID = eRPSalesOrderDeliveryInformationDto.omdPurchaseLocationID,
					omdPurchaseUnitCostBase = eRPSalesOrderDeliveryInformationDto.omdPurchaseUnitCostBase,
					omdPurchaseUnitCostForeign = eRPSalesOrderDeliveryInformationDto.omdPurchaseUnitCostForeign,
					omdQuantityAllocated = eRPSalesOrderDeliveryInformationDto.omdQuantityAllocated,
					omdQuantityInvoiced = eRPSalesOrderDeliveryInformationDto.omdQuantityInvoiced,
					omdQuantityOnOrder = eRPSalesOrderDeliveryInformationDto.omdQuantityOnOrder,
					omdQuantityReceived = eRPSalesOrderDeliveryInformationDto.omdQuantityReceived,
					omdQuantityShipped = eRPSalesOrderDeliveryInformationDto.omdQuantityShipped,
					omdRowVersion = eRPSalesOrderDeliveryInformationDto.omdRowVersion,
					omdSalesOrderID = eRPSalesOrderDeliveryInformationDto.omdSalesOrderID,
					omdSalesOrderLineID = eRPSalesOrderDeliveryInformationDto.omdSalesOrderLineID,
					omdSalesOrderDeliveryID = eRPSalesOrderDeliveryInformationDto.omdSalesOrderDeliveryID,
					omdShipContactID = eRPSalesOrderDeliveryInformationDto.omdShipContactID,
					omdShipLocationID = eRPSalesOrderDeliveryInformationDto.omdShipLocationID,
					omdShippingMethodID = eRPSalesOrderDeliveryInformationDto.omdShippingMethodID,
					omdShippingPaymentTypeID = eRPSalesOrderDeliveryInformationDto.omdShippingPaymentTypeID,
					omdSupplierOrganizationID = eRPSalesOrderDeliveryInformationDto.omdSupplierOrganizationID,
					omdWeight = eRPSalesOrderDeliveryInformationDto.omdWeight,
					CustomFields = eRPSalesOrderDeliveryInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrderDeliveries []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderDeliveryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderDeliveryDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderDeliveryDto>> Process_PutSalesOrderDelivery(ERPSalesOrderDeliveryDto salesOrderDelivery)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderDeliveryDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderDeliveryDto> result;
		try
		{
			IERPSalesOrderDeliveryRepository iERPSalesOrderDeliveryRepository = (base.ERPSalesOrderDeliveryRepository = new ERPSalesOrderDeliveryRepository(base.ApiClientContext));
			using (iERPSalesOrderDeliveryRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderDeliveryRepository.SaveSalesOrderDelivery(salesOrderDelivery);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderDeliveryInformationDto eRPSalesOrderDeliveryInformationDto = await base.ERPSalesOrderDeliveryRepository.GetSalesOrderDelivery(salesOrderDelivery.omdUniqueID);
					createdObject = new ERPSalesOrderDeliveryDto
					{
						omdAmountToInvoice = eRPSalesOrderDeliveryInformationDto.omdAmountToInvoice,
						omdAmountToInvoiceForeign = eRPSalesOrderDeliveryInformationDto.omdAmountToInvoiceForeign,
						omdAvalaraNonTaxReasonID = eRPSalesOrderDeliveryInformationDto.omdAvalaraNonTaxReasonID,
						omdCreatedBy = eRPSalesOrderDeliveryInformationDto.omdCreatedBy,
						omdCreatedDate = eRPSalesOrderDeliveryInformationDto.omdCreatedDate,
						omdCustomerOrganizationID = eRPSalesOrderDeliveryInformationDto.omdCustomerOrganizationID,
						omdDeliveryDate = eRPSalesOrderDeliveryInformationDto.omdDeliveryDate,
						omdDeliveryQuantity = eRPSalesOrderDeliveryInformationDto.omdDeliveryQuantity,
						omdDeliveryType = eRPSalesOrderDeliveryInformationDto.omdDeliveryType,
						omdUniqueID = eRPSalesOrderDeliveryInformationDto.omdUniqueID,
						omdExtendedWeight = eRPSalesOrderDeliveryInformationDto.omdExtendedWeight,
						omdFreightAmountBase = eRPSalesOrderDeliveryInformationDto.omdFreightAmountBase,
						omdFreightAmountForeign = eRPSalesOrderDeliveryInformationDto.omdFreightAmountForeign,
						omdClosed = eRPSalesOrderDeliveryInformationDto.omdClosed,
						omdDifferentLocation = eRPSalesOrderDeliveryInformationDto.omdDifferentLocation,
						omdFirm = eRPSalesOrderDeliveryInformationDto.omdFirm,
						omdInvoicedComplete = eRPSalesOrderDeliveryInformationDto.omdInvoicedComplete,
						omdKitPart = eRPSalesOrderDeliveryInformationDto.omdKitPart,
						omdPickInProgress = eRPSalesOrderDeliveryInformationDto.omdPickInProgress,
						omdReceivedComplete = eRPSalesOrderDeliveryInformationDto.omdReceivedComplete,
						omdRequiresInspection = eRPSalesOrderDeliveryInformationDto.omdRequiresInspection,
						omdShippedComplete = eRPSalesOrderDeliveryInformationDto.omdShippedComplete,
						omdPartBinID = eRPSalesOrderDeliveryInformationDto.omdPartBinID,
						omdPartID = eRPSalesOrderDeliveryInformationDto.omdPartID,
						omdPartRevisionID = eRPSalesOrderDeliveryInformationDto.omdPartRevisionID,
						omdPartWarehouseLocationID = eRPSalesOrderDeliveryInformationDto.omdPartWarehouseLocationID,
						omdPurchaseLocationID = eRPSalesOrderDeliveryInformationDto.omdPurchaseLocationID,
						omdPurchaseUnitCostBase = eRPSalesOrderDeliveryInformationDto.omdPurchaseUnitCostBase,
						omdPurchaseUnitCostForeign = eRPSalesOrderDeliveryInformationDto.omdPurchaseUnitCostForeign,
						omdQuantityAllocated = eRPSalesOrderDeliveryInformationDto.omdQuantityAllocated,
						omdQuantityInvoiced = eRPSalesOrderDeliveryInformationDto.omdQuantityInvoiced,
						omdQuantityOnOrder = eRPSalesOrderDeliveryInformationDto.omdQuantityOnOrder,
						omdQuantityReceived = eRPSalesOrderDeliveryInformationDto.omdQuantityReceived,
						omdQuantityShipped = eRPSalesOrderDeliveryInformationDto.omdQuantityShipped,
						omdRowVersion = eRPSalesOrderDeliveryInformationDto.omdRowVersion,
						omdSalesOrderID = eRPSalesOrderDeliveryInformationDto.omdSalesOrderID,
						omdSalesOrderLineID = eRPSalesOrderDeliveryInformationDto.omdSalesOrderLineID,
						omdSalesOrderDeliveryID = eRPSalesOrderDeliveryInformationDto.omdSalesOrderDeliveryID,
						omdShipContactID = eRPSalesOrderDeliveryInformationDto.omdShipContactID,
						omdShipLocationID = eRPSalesOrderDeliveryInformationDto.omdShipLocationID,
						omdShippingMethodID = eRPSalesOrderDeliveryInformationDto.omdShippingMethodID,
						omdShippingPaymentTypeID = eRPSalesOrderDeliveryInformationDto.omdShippingPaymentTypeID,
						omdSupplierOrganizationID = eRPSalesOrderDeliveryInformationDto.omdSupplierOrganizationID,
						omdWeight = eRPSalesOrderDeliveryInformationDto.omdWeight,
						CustomFields = eRPSalesOrderDeliveryInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrderDelivery [{salesOrderDelivery.omdUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderDeliveryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderDelivery(Guid salesOrderDeliveryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderDeliveryRepository iERPSalesOrderDeliveryRepository = (base.ERPSalesOrderDeliveryRepository = new ERPSalesOrderDeliveryRepository(base.ApiClientContext));
		using (iERPSalesOrderDeliveryRepository)
		{
			if (!(await base.ERPSalesOrderDeliveryRepository.DoesSalesOrderDeliveryExist(salesOrderDeliveryId)))
			{
				base.ErrorsList.Add($"SalesOrderDelivery [{salesOrderDeliveryId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderDeliveryInformationDto eRPSalesOrderDeliveryInformationDto = await base.ERPSalesOrderDeliveryRepository.GetSalesOrderDelivery(salesOrderDeliveryId);
				string text = await base.ERPSalesOrderDeliveryRepository.WhereUsed("SalesOrderDeliveries", new object[3] { eRPSalesOrderDeliveryInformationDto.omdSalesOrderID, eRPSalesOrderDeliveryInformationDto.omdSalesOrderLineID, eRPSalesOrderDeliveryInformationDto.omdSalesOrderDeliveryID }, new object[3] { "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrderDelivery cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderDeliveryDto>> Process_DeleteSalesOrderDelivery(Guid salesOrderDeliveryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderDeliveryDto> result;
		try
		{
			IERPSalesOrderDeliveryRepository iERPSalesOrderDeliveryRepository = (base.ERPSalesOrderDeliveryRepository = new ERPSalesOrderDeliveryRepository(base.ApiClientContext));
			using (iERPSalesOrderDeliveryRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderDeliveryRepository.DeleteRowFromTable("SalesOrderDeliveries", "omd", salesOrderDeliveryId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrderDelivery [{salesOrderDeliveryId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderDeliveryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderDeliveryDto()
			};
		}
		return result;
	}
}
