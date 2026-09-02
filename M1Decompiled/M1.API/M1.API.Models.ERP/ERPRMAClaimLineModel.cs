using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRMAClaimLineModel : ERPBaseModel, IERPRMAClaimLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRMAClaimLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRMAClaimLineRepository iERPRMAClaimLineRepository = (base.ERPRMAClaimLineRepository = new ERPRMAClaimLineRepository(base.ApiClientContext));
		using (iERPRMAClaimLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRMAClaimLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRMAClaimLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRMAClaimLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRMAClaimLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRMAClaimLine(Guid rMAClaimLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAClaimLineRepository iERPRMAClaimLineRepository = (base.ERPRMAClaimLineRepository = new ERPRMAClaimLineRepository(base.ApiClientContext));
		using (iERPRMAClaimLineRepository)
		{
			if (!(await base.ERPRMAClaimLineRepository.DoesRMAClaimLineExist(rMAClaimLineId)))
			{
				errorsList.Add($"RMAClaimLine [{rMAClaimLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRMAClaimLine(ERPRMAClaimLineDto rMAClaimLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAClaimLineRepository iERPRMAClaimLineRepository = (base.ERPRMAClaimLineRepository = new ERPRMAClaimLineRepository(base.ApiClientContext));
		using (iERPRMAClaimLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralRmaClaimID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { rMAClaimLine.ralRmaClaimID })))
			{
				errorsList.Add("ralRmaClaimID [" + rMAClaimLine.ralRmaClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralPartID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { rMAClaimLine.ralPartID })))
			{
				errorsList.Add("ralPartID [" + rMAClaimLine.ralPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralPartRevisionID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { rMAClaimLine.ralPartID, rMAClaimLine.ralPartRevisionID })))
			{
				errorsList.Add("ralPartRevisionID [" + rMAClaimLine.ralPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralPartWarehouseLocationID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { rMAClaimLine.ralPartID, rMAClaimLine.ralPartRevisionID, rMAClaimLine.ralPartWarehouseLocationID })))
			{
				errorsList.Add("ralPartWarehouseLocationID [" + rMAClaimLine.ralPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralPartBinID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { rMAClaimLine.ralPartID, rMAClaimLine.ralPartRevisionID, rMAClaimLine.ralPartWarehouseLocationID, rMAClaimLine.ralPartBinID })))
			{
				errorsList.Add("ralPartBinID [" + rMAClaimLine.ralPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralPartGroupID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { rMAClaimLine.ralPartGroupID })))
			{
				errorsList.Add("ralPartGroupID [" + rMAClaimLine.ralPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralActionType) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("RMAActionTypes", new object[1] { "RATRMAACTIONTYPEID" }, new object[1] { rMAClaimLine.ralActionType })))
			{
				errorsList.Add("ralActionType [" + rMAClaimLine.ralActionType + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralReturnReasonID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { rMAClaimLine.ralReturnReasonID })))
			{
				errorsList.Add("ralReturnReasonID [" + rMAClaimLine.ralReturnReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralSupplierOrganizationID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { rMAClaimLine.ralSupplierOrganizationID })))
			{
				errorsList.Add("ralSupplierOrganizationID [" + rMAClaimLine.ralSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralPurchaseLocationID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { rMAClaimLine.ralSupplierOrganizationID, rMAClaimLine.ralPurchaseLocationID })))
			{
				errorsList.Add("ralPurchaseLocationID [" + rMAClaimLine.ralPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralShippingMethodID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { rMAClaimLine.ralShippingMethodID })))
			{
				errorsList.Add("ralShippingMethodID [" + rMAClaimLine.ralShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralProjectID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { rMAClaimLine.ralProjectID })))
			{
				errorsList.Add("ralProjectID [" + rMAClaimLine.ralProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralProjectAreaID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { rMAClaimLine.ralProjectID, rMAClaimLine.ralProjectAreaID })))
			{
				errorsList.Add("ralProjectAreaID [" + rMAClaimLine.ralProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralSupplierShippingMethodID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { rMAClaimLine.ralSupplierShippingMethodID })))
			{
				errorsList.Add("ralSupplierShippingMethodID [" + rMAClaimLine.ralSupplierShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralShippingPaymentTypeID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { rMAClaimLine.ralShippingPaymentTypeID })))
			{
				errorsList.Add("ralShippingPaymentTypeID [" + rMAClaimLine.ralShippingPaymentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralShipmentID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { rMAClaimLine.ralShipmentID })))
			{
				errorsList.Add("ralShipmentID [" + rMAClaimLine.ralShipmentID + "] not found.");
			}
			if (rMAClaimLine.ralShipmentLineID > 0 && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("ShipmentLines", new object[2] { "SMLSHIPMENTID", "SMLSHIPMENTLINEID" }, new object[2] { rMAClaimLine.ralShipmentID, rMAClaimLine.ralShipmentLineID })))
			{
				errorsList.Add($"ralShipmentLineID [{rMAClaimLine.ralShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaimLine.ralSalesOrderID) && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { rMAClaimLine.ralSalesOrderID })))
			{
				errorsList.Add("ralSalesOrderID [" + rMAClaimLine.ralSalesOrderID + "] not found.");
			}
			if (rMAClaimLine.ralSalesOrderLineID > 0 && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { rMAClaimLine.ralSalesOrderID, rMAClaimLine.ralSalesOrderLineID })))
			{
				errorsList.Add($"ralSalesOrderLineID [{rMAClaimLine.ralSalesOrderLineID}] not found.");
			}
			if (rMAClaimLine.ralSalesOrderDeliveryID > 0 && !(await base.ERPRMAClaimLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { rMAClaimLine.ralSalesOrderID, rMAClaimLine.ralSalesOrderLineID, rMAClaimLine.ralSalesOrderDeliveryID })))
			{
				errorsList.Add($"ralSalesOrderDeliveryID [{rMAClaimLine.ralSalesOrderDeliveryID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRMAClaimLineDto>>> Process_GetAllRMAClaimLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRMAClaimLineDto> allRMAClaimLinesDto = new List<ERPRMAClaimLineDto>();
		ERPResponseMessageDto<IList<ERPRMAClaimLineDto>> result;
		try
		{
			IERPRMAClaimLineRepository iERPRMAClaimLineRepository = (base.ERPRMAClaimLineRepository = new ERPRMAClaimLineRepository(base.ApiClientContext));
			using (iERPRMAClaimLineRepository)
			{
				foreach (ERPRMAClaimLineInformationDto item2 in await base.ERPRMAClaimLineRepository.GetAllRMAClaimLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPRMAClaimLineDto item = new ERPRMAClaimLineDto
					{
						ralActionType = item2.ralActionType,
						ralConversionFactor = item2.ralConversionFactor,
						ralCreatedBy = item2.ralCreatedBy,
						ralCreatedDate = item2.ralCreatedDate,
						ralCustomerPo = item2.ralCustomerPo,
						ralDiscountPercent = item2.ralDiscountPercent,
						ralUniqueID = item2.ralUniqueID,
						ralExtendedCost = item2.ralExtendedCost,
						ralExtendedCostForeign = item2.ralExtendedCostForeign,
						ralExtendedDiscountBase = item2.ralExtendedDiscountBase,
						ralExtendedDiscountForeign = item2.ralExtendedDiscountForeign,
						ralExtendedPrice = item2.ralExtendedPrice,
						ralExtendedPriceForeign = item2.ralExtendedPriceForeign,
						ralFullExtendedPriceBase = item2.ralFullExtendedPriceBase,
						ralFullExtendedPriceForeign = item2.ralFullExtendedPriceForeign,
						ralFullUnitPriceBase = item2.ralFullUnitPriceBase,
						ralFullUnitPriceForeign = item2.ralFullUnitPriceForeign,
						ralCustomerToPayForShipping = item2.ralCustomerToPayForShipping,
						ralInvoicedComplete = item2.ralInvoicedComplete,
						ralKitPart = item2.ralKitPart,
						ralReceivedComplete = item2.ralReceivedComplete,
						ralRequiresInspection = item2.ralRequiresInspection,
						ralReturnToSupplier = item2.ralReturnToSupplier,
						ralTransferredToSalesOrder = item2.ralTransferredToSalesOrder,
						ralOrgPartID = item2.ralOrgPartID,
						ralOrgPartShortDescription = item2.ralOrgPartShortDescription,
						ralPartBinID = item2.ralPartBinID,
						ralPartGroupID = item2.ralPartGroupID,
						ralPartID = item2.ralPartID,
						ralPartLongDescriptionRtf = item2.ralPartLongDescriptionRtf,
						ralPartLongDescriptionText = item2.ralPartLongDescriptionText,
						ralPartRevisionID = item2.ralPartRevisionID,
						ralPartShortDescription = item2.ralPartShortDescription,
						ralPartWarehouseLocationID = item2.ralPartWarehouseLocationID,
						ralProjectAreaID = item2.ralProjectAreaID,
						ralProjectID = item2.ralProjectID,
						ralPurchaseLocationID = item2.ralPurchaseLocationID,
						ralQuantity = item2.ralQuantity,
						ralQuantityReceived = item2.ralQuantityReceived,
						ralReceivedDate = item2.ralReceivedDate,
						ralRequiredDate = item2.ralRequiredDate,
						ralReturnedDate = item2.ralReturnedDate,
						ralReturnReasonID = item2.ralReturnReasonID,
						ralRmaClaimID = item2.ralRmaClaimID,
						ralRowVersion = item2.ralRowVersion,
						ralSalesOrderDeliveryID = item2.ralSalesOrderDeliveryID,
						ralSalesOrderID = item2.ralSalesOrderID,
						ralSalesOrderLineID = item2.ralSalesOrderLineID,
						ralSalesQuantity = item2.ralSalesQuantity,
						ralSalesUnitOfMeasure = item2.ralSalesUnitOfMeasure,
						ralRmaClaimLineID = item2.ralRmaClaimLineID,
						ralShipmentID = item2.ralShipmentID,
						ralShipmentLineID = item2.ralShipmentLineID,
						ralShippedDate = item2.ralShippedDate,
						ralShippingMethodID = item2.ralShippingMethodID,
						ralShippingPaymentTypeID = item2.ralShippingPaymentTypeID,
						ralSupplierAuthorizationNumber = item2.ralSupplierAuthorizationNumber,
						ralSupplierOrganizationID = item2.ralSupplierOrganizationID,
						ralSupplierShippingMethodID = item2.ralSupplierShippingMethodID,
						ralSupplierTrackingNumber = item2.ralSupplierTrackingNumber,
						ralTrackingNumber = item2.ralTrackingNumber,
						ralUnitCost = item2.ralUnitCost,
						ralUnitCostForeign = item2.ralUnitCostForeign,
						ralUnitDiscountBase = item2.ralUnitDiscountBase,
						ralUnitDiscountForeign = item2.ralUnitDiscountForeign,
						ralUnitOfMeasure = item2.ralUnitOfMeasure,
						ralUnitPrice = item2.ralUnitPrice,
						ralUnitPriceForeign = item2.ralUnitPriceForeign,
						CustomFields = item2.CustomFields
					};
					allRMAClaimLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RMAClaimLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRMAClaimLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRMAClaimLinesDto,
				RecordCount = allRMAClaimLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAClaimLineDto>> Process_GetRMAClaimLine(Guid rMAClaimLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRMAClaimLineDto rMAClaimLineDto = null;
		ERPResponseMessageDto<ERPRMAClaimLineDto> result;
		try
		{
			IERPRMAClaimLineRepository iERPRMAClaimLineRepository = (base.ERPRMAClaimLineRepository = new ERPRMAClaimLineRepository(base.ApiClientContext));
			using (iERPRMAClaimLineRepository)
			{
				ERPRMAClaimLineInformationDto eRPRMAClaimLineInformationDto = await base.ERPRMAClaimLineRepository.GetRMAClaimLine(rMAClaimLineId);
				rMAClaimLineDto = new ERPRMAClaimLineDto
				{
					ralActionType = eRPRMAClaimLineInformationDto.ralActionType,
					ralConversionFactor = eRPRMAClaimLineInformationDto.ralConversionFactor,
					ralCreatedBy = eRPRMAClaimLineInformationDto.ralCreatedBy,
					ralCreatedDate = eRPRMAClaimLineInformationDto.ralCreatedDate,
					ralCustomerPo = eRPRMAClaimLineInformationDto.ralCustomerPo,
					ralDiscountPercent = eRPRMAClaimLineInformationDto.ralDiscountPercent,
					ralUniqueID = eRPRMAClaimLineInformationDto.ralUniqueID,
					ralExtendedCost = eRPRMAClaimLineInformationDto.ralExtendedCost,
					ralExtendedCostForeign = eRPRMAClaimLineInformationDto.ralExtendedCostForeign,
					ralExtendedDiscountBase = eRPRMAClaimLineInformationDto.ralExtendedDiscountBase,
					ralExtendedDiscountForeign = eRPRMAClaimLineInformationDto.ralExtendedDiscountForeign,
					ralExtendedPrice = eRPRMAClaimLineInformationDto.ralExtendedPrice,
					ralExtendedPriceForeign = eRPRMAClaimLineInformationDto.ralExtendedPriceForeign,
					ralFullExtendedPriceBase = eRPRMAClaimLineInformationDto.ralFullExtendedPriceBase,
					ralFullExtendedPriceForeign = eRPRMAClaimLineInformationDto.ralFullExtendedPriceForeign,
					ralFullUnitPriceBase = eRPRMAClaimLineInformationDto.ralFullUnitPriceBase,
					ralFullUnitPriceForeign = eRPRMAClaimLineInformationDto.ralFullUnitPriceForeign,
					ralCustomerToPayForShipping = eRPRMAClaimLineInformationDto.ralCustomerToPayForShipping,
					ralInvoicedComplete = eRPRMAClaimLineInformationDto.ralInvoicedComplete,
					ralKitPart = eRPRMAClaimLineInformationDto.ralKitPart,
					ralReceivedComplete = eRPRMAClaimLineInformationDto.ralReceivedComplete,
					ralRequiresInspection = eRPRMAClaimLineInformationDto.ralRequiresInspection,
					ralReturnToSupplier = eRPRMAClaimLineInformationDto.ralReturnToSupplier,
					ralTransferredToSalesOrder = eRPRMAClaimLineInformationDto.ralTransferredToSalesOrder,
					ralOrgPartID = eRPRMAClaimLineInformationDto.ralOrgPartID,
					ralOrgPartShortDescription = eRPRMAClaimLineInformationDto.ralOrgPartShortDescription,
					ralPartBinID = eRPRMAClaimLineInformationDto.ralPartBinID,
					ralPartGroupID = eRPRMAClaimLineInformationDto.ralPartGroupID,
					ralPartID = eRPRMAClaimLineInformationDto.ralPartID,
					ralPartLongDescriptionRtf = eRPRMAClaimLineInformationDto.ralPartLongDescriptionRtf,
					ralPartLongDescriptionText = eRPRMAClaimLineInformationDto.ralPartLongDescriptionText,
					ralPartRevisionID = eRPRMAClaimLineInformationDto.ralPartRevisionID,
					ralPartShortDescription = eRPRMAClaimLineInformationDto.ralPartShortDescription,
					ralPartWarehouseLocationID = eRPRMAClaimLineInformationDto.ralPartWarehouseLocationID,
					ralProjectAreaID = eRPRMAClaimLineInformationDto.ralProjectAreaID,
					ralProjectID = eRPRMAClaimLineInformationDto.ralProjectID,
					ralPurchaseLocationID = eRPRMAClaimLineInformationDto.ralPurchaseLocationID,
					ralQuantity = eRPRMAClaimLineInformationDto.ralQuantity,
					ralQuantityReceived = eRPRMAClaimLineInformationDto.ralQuantityReceived,
					ralReceivedDate = eRPRMAClaimLineInformationDto.ralReceivedDate,
					ralRequiredDate = eRPRMAClaimLineInformationDto.ralRequiredDate,
					ralReturnedDate = eRPRMAClaimLineInformationDto.ralReturnedDate,
					ralReturnReasonID = eRPRMAClaimLineInformationDto.ralReturnReasonID,
					ralRmaClaimID = eRPRMAClaimLineInformationDto.ralRmaClaimID,
					ralRowVersion = eRPRMAClaimLineInformationDto.ralRowVersion,
					ralSalesOrderDeliveryID = eRPRMAClaimLineInformationDto.ralSalesOrderDeliveryID,
					ralSalesOrderID = eRPRMAClaimLineInformationDto.ralSalesOrderID,
					ralSalesOrderLineID = eRPRMAClaimLineInformationDto.ralSalesOrderLineID,
					ralSalesQuantity = eRPRMAClaimLineInformationDto.ralSalesQuantity,
					ralSalesUnitOfMeasure = eRPRMAClaimLineInformationDto.ralSalesUnitOfMeasure,
					ralRmaClaimLineID = eRPRMAClaimLineInformationDto.ralRmaClaimLineID,
					ralShipmentID = eRPRMAClaimLineInformationDto.ralShipmentID,
					ralShipmentLineID = eRPRMAClaimLineInformationDto.ralShipmentLineID,
					ralShippedDate = eRPRMAClaimLineInformationDto.ralShippedDate,
					ralShippingMethodID = eRPRMAClaimLineInformationDto.ralShippingMethodID,
					ralShippingPaymentTypeID = eRPRMAClaimLineInformationDto.ralShippingPaymentTypeID,
					ralSupplierAuthorizationNumber = eRPRMAClaimLineInformationDto.ralSupplierAuthorizationNumber,
					ralSupplierOrganizationID = eRPRMAClaimLineInformationDto.ralSupplierOrganizationID,
					ralSupplierShippingMethodID = eRPRMAClaimLineInformationDto.ralSupplierShippingMethodID,
					ralSupplierTrackingNumber = eRPRMAClaimLineInformationDto.ralSupplierTrackingNumber,
					ralTrackingNumber = eRPRMAClaimLineInformationDto.ralTrackingNumber,
					ralUnitCost = eRPRMAClaimLineInformationDto.ralUnitCost,
					ralUnitCostForeign = eRPRMAClaimLineInformationDto.ralUnitCostForeign,
					ralUnitDiscountBase = eRPRMAClaimLineInformationDto.ralUnitDiscountBase,
					ralUnitDiscountForeign = eRPRMAClaimLineInformationDto.ralUnitDiscountForeign,
					ralUnitOfMeasure = eRPRMAClaimLineInformationDto.ralUnitOfMeasure,
					ralUnitPrice = eRPRMAClaimLineInformationDto.ralUnitPrice,
					ralUnitPriceForeign = eRPRMAClaimLineInformationDto.ralUnitPriceForeign,
					CustomFields = eRPRMAClaimLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RMAClaimLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAClaimLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rMAClaimLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAClaimLineDto>> Process_PutRMAClaimLine(ERPRMAClaimLineDto rMAClaimLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRMAClaimLineDto createdObject = null;
		ERPResponseMessageDto<ERPRMAClaimLineDto> result;
		try
		{
			IERPRMAClaimLineRepository iERPRMAClaimLineRepository = (base.ERPRMAClaimLineRepository = new ERPRMAClaimLineRepository(base.ApiClientContext));
			using (iERPRMAClaimLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRMAClaimLineRepository.SaveRMAClaimLine(rMAClaimLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRMAClaimLineInformationDto eRPRMAClaimLineInformationDto = await base.ERPRMAClaimLineRepository.GetRMAClaimLine(rMAClaimLine.ralUniqueID);
					createdObject = new ERPRMAClaimLineDto
					{
						ralActionType = eRPRMAClaimLineInformationDto.ralActionType,
						ralConversionFactor = eRPRMAClaimLineInformationDto.ralConversionFactor,
						ralCreatedBy = eRPRMAClaimLineInformationDto.ralCreatedBy,
						ralCreatedDate = eRPRMAClaimLineInformationDto.ralCreatedDate,
						ralCustomerPo = eRPRMAClaimLineInformationDto.ralCustomerPo,
						ralDiscountPercent = eRPRMAClaimLineInformationDto.ralDiscountPercent,
						ralUniqueID = eRPRMAClaimLineInformationDto.ralUniqueID,
						ralExtendedCost = eRPRMAClaimLineInformationDto.ralExtendedCost,
						ralExtendedCostForeign = eRPRMAClaimLineInformationDto.ralExtendedCostForeign,
						ralExtendedDiscountBase = eRPRMAClaimLineInformationDto.ralExtendedDiscountBase,
						ralExtendedDiscountForeign = eRPRMAClaimLineInformationDto.ralExtendedDiscountForeign,
						ralExtendedPrice = eRPRMAClaimLineInformationDto.ralExtendedPrice,
						ralExtendedPriceForeign = eRPRMAClaimLineInformationDto.ralExtendedPriceForeign,
						ralFullExtendedPriceBase = eRPRMAClaimLineInformationDto.ralFullExtendedPriceBase,
						ralFullExtendedPriceForeign = eRPRMAClaimLineInformationDto.ralFullExtendedPriceForeign,
						ralFullUnitPriceBase = eRPRMAClaimLineInformationDto.ralFullUnitPriceBase,
						ralFullUnitPriceForeign = eRPRMAClaimLineInformationDto.ralFullUnitPriceForeign,
						ralCustomerToPayForShipping = eRPRMAClaimLineInformationDto.ralCustomerToPayForShipping,
						ralInvoicedComplete = eRPRMAClaimLineInformationDto.ralInvoicedComplete,
						ralKitPart = eRPRMAClaimLineInformationDto.ralKitPart,
						ralReceivedComplete = eRPRMAClaimLineInformationDto.ralReceivedComplete,
						ralRequiresInspection = eRPRMAClaimLineInformationDto.ralRequiresInspection,
						ralReturnToSupplier = eRPRMAClaimLineInformationDto.ralReturnToSupplier,
						ralTransferredToSalesOrder = eRPRMAClaimLineInformationDto.ralTransferredToSalesOrder,
						ralOrgPartID = eRPRMAClaimLineInformationDto.ralOrgPartID,
						ralOrgPartShortDescription = eRPRMAClaimLineInformationDto.ralOrgPartShortDescription,
						ralPartBinID = eRPRMAClaimLineInformationDto.ralPartBinID,
						ralPartGroupID = eRPRMAClaimLineInformationDto.ralPartGroupID,
						ralPartID = eRPRMAClaimLineInformationDto.ralPartID,
						ralPartLongDescriptionRtf = eRPRMAClaimLineInformationDto.ralPartLongDescriptionRtf,
						ralPartLongDescriptionText = eRPRMAClaimLineInformationDto.ralPartLongDescriptionText,
						ralPartRevisionID = eRPRMAClaimLineInformationDto.ralPartRevisionID,
						ralPartShortDescription = eRPRMAClaimLineInformationDto.ralPartShortDescription,
						ralPartWarehouseLocationID = eRPRMAClaimLineInformationDto.ralPartWarehouseLocationID,
						ralProjectAreaID = eRPRMAClaimLineInformationDto.ralProjectAreaID,
						ralProjectID = eRPRMAClaimLineInformationDto.ralProjectID,
						ralPurchaseLocationID = eRPRMAClaimLineInformationDto.ralPurchaseLocationID,
						ralQuantity = eRPRMAClaimLineInformationDto.ralQuantity,
						ralQuantityReceived = eRPRMAClaimLineInformationDto.ralQuantityReceived,
						ralReceivedDate = eRPRMAClaimLineInformationDto.ralReceivedDate,
						ralRequiredDate = eRPRMAClaimLineInformationDto.ralRequiredDate,
						ralReturnedDate = eRPRMAClaimLineInformationDto.ralReturnedDate,
						ralReturnReasonID = eRPRMAClaimLineInformationDto.ralReturnReasonID,
						ralRmaClaimID = eRPRMAClaimLineInformationDto.ralRmaClaimID,
						ralRowVersion = eRPRMAClaimLineInformationDto.ralRowVersion,
						ralSalesOrderDeliveryID = eRPRMAClaimLineInformationDto.ralSalesOrderDeliveryID,
						ralSalesOrderID = eRPRMAClaimLineInformationDto.ralSalesOrderID,
						ralSalesOrderLineID = eRPRMAClaimLineInformationDto.ralSalesOrderLineID,
						ralSalesQuantity = eRPRMAClaimLineInformationDto.ralSalesQuantity,
						ralSalesUnitOfMeasure = eRPRMAClaimLineInformationDto.ralSalesUnitOfMeasure,
						ralRmaClaimLineID = eRPRMAClaimLineInformationDto.ralRmaClaimLineID,
						ralShipmentID = eRPRMAClaimLineInformationDto.ralShipmentID,
						ralShipmentLineID = eRPRMAClaimLineInformationDto.ralShipmentLineID,
						ralShippedDate = eRPRMAClaimLineInformationDto.ralShippedDate,
						ralShippingMethodID = eRPRMAClaimLineInformationDto.ralShippingMethodID,
						ralShippingPaymentTypeID = eRPRMAClaimLineInformationDto.ralShippingPaymentTypeID,
						ralSupplierAuthorizationNumber = eRPRMAClaimLineInformationDto.ralSupplierAuthorizationNumber,
						ralSupplierOrganizationID = eRPRMAClaimLineInformationDto.ralSupplierOrganizationID,
						ralSupplierShippingMethodID = eRPRMAClaimLineInformationDto.ralSupplierShippingMethodID,
						ralSupplierTrackingNumber = eRPRMAClaimLineInformationDto.ralSupplierTrackingNumber,
						ralTrackingNumber = eRPRMAClaimLineInformationDto.ralTrackingNumber,
						ralUnitCost = eRPRMAClaimLineInformationDto.ralUnitCost,
						ralUnitCostForeign = eRPRMAClaimLineInformationDto.ralUnitCostForeign,
						ralUnitDiscountBase = eRPRMAClaimLineInformationDto.ralUnitDiscountBase,
						ralUnitDiscountForeign = eRPRMAClaimLineInformationDto.ralUnitDiscountForeign,
						ralUnitOfMeasure = eRPRMAClaimLineInformationDto.ralUnitOfMeasure,
						ralUnitPrice = eRPRMAClaimLineInformationDto.ralUnitPrice,
						ralUnitPriceForeign = eRPRMAClaimLineInformationDto.ralUnitPriceForeign,
						CustomFields = eRPRMAClaimLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RMAClaimLine [{rMAClaimLine.ralUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAClaimLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRMAClaimLine(Guid rMAClaimLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAClaimLineRepository iERPRMAClaimLineRepository = (base.ERPRMAClaimLineRepository = new ERPRMAClaimLineRepository(base.ApiClientContext));
		using (iERPRMAClaimLineRepository)
		{
			if (!(await base.ERPRMAClaimLineRepository.DoesRMAClaimLineExist(rMAClaimLineId)))
			{
				base.ErrorsList.Add($"RMAClaimLine [{rMAClaimLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRMAClaimLineInformationDto eRPRMAClaimLineInformationDto = await base.ERPRMAClaimLineRepository.GetRMAClaimLine(rMAClaimLineId);
				string text = await base.ERPRMAClaimLineRepository.WhereUsed("RMAClaimLines", new object[2] { eRPRMAClaimLineInformationDto.ralRmaClaimID, eRPRMAClaimLineInformationDto.ralRmaClaimLineID }, new object[2] { "ralRmaClaimID", "ralRmaClaimLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RMAClaimLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRMAClaimLineDto>> Process_DeleteRMAClaimLine(Guid rMAClaimLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRMAClaimLineDto> result;
		try
		{
			IERPRMAClaimLineRepository iERPRMAClaimLineRepository = (base.ERPRMAClaimLineRepository = new ERPRMAClaimLineRepository(base.ApiClientContext));
			using (iERPRMAClaimLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRMAClaimLineRepository.DeleteRowFromTable("RMAClaimLines", "ral", rMAClaimLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RMAClaimLine [{rMAClaimLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAClaimLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRMAClaimLineDto()
			};
		}
		return result;
	}
}
