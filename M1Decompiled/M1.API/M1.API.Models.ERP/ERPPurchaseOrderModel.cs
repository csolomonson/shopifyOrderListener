using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchaseOrderModel : ERPBaseModel, IERPPurchaseOrderModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchaseOrderRepository iERPPurchaseOrderRepository = (base.ERPPurchaseOrderRepository = new ERPPurchaseOrderRepository(base.ApiClientContext));
		using (iERPPurchaseOrderRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchaseOrderRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchaseOrderRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchaseOrderRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchaseOrderRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrder(Guid purchaseOrderId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderRepository iERPPurchaseOrderRepository = (base.ERPPurchaseOrderRepository = new ERPPurchaseOrderRepository(base.ApiClientContext));
		using (iERPPurchaseOrderRepository)
		{
			if (!(await base.ERPPurchaseOrderRepository.DoesPurchaseOrderExist(purchaseOrderId)))
			{
				errorsList.Add($"PurchaseOrder [{purchaseOrderId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrder(ERPPurchaseOrderDto purchaseOrder)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderRepository iERPPurchaseOrderRepository = (base.ERPPurchaseOrderRepository = new ERPPurchaseOrderRepository(base.ApiClientContext));
		using (iERPPurchaseOrderRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpPlantDepartmentID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { purchaseOrder.pmpPlantID, purchaseOrder.pmpPlantDepartmentID })))
			{
				errorsList.Add("pmpPlantDepartmentID [" + purchaseOrder.pmpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpPlantID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { purchaseOrder.pmpPlantID })))
			{
				errorsList.Add("pmpPlantID [" + purchaseOrder.pmpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpSupplierOrganizationID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { purchaseOrder.pmpSupplierOrganizationID })))
			{
				errorsList.Add("pmpSupplierOrganizationID [" + purchaseOrder.pmpSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpApInvoiceLocationID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { purchaseOrder.pmpSupplierOrganizationID, purchaseOrder.pmpApInvoiceLocationID })))
			{
				errorsList.Add("pmpApInvoiceLocationID [" + purchaseOrder.pmpApInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpApInvoiceContactID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { purchaseOrder.pmpSupplierOrganizationID, purchaseOrder.pmpApInvoiceLocationID, purchaseOrder.pmpApInvoiceContactID })))
			{
				errorsList.Add("pmpApInvoiceContactID [" + purchaseOrder.pmpApInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpPurchaseLocationID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { purchaseOrder.pmpSupplierOrganizationID, purchaseOrder.pmpPurchaseLocationID })))
			{
				errorsList.Add("pmpPurchaseLocationID [" + purchaseOrder.pmpPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpPurchaseContactID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { purchaseOrder.pmpSupplierOrganizationID, purchaseOrder.pmpPurchaseLocationID, purchaseOrder.pmpPurchaseContactID })))
			{
				errorsList.Add("pmpPurchaseContactID [" + purchaseOrder.pmpPurchaseContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpDropShipOrganizationID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { purchaseOrder.pmpDropShipOrganizationID })))
			{
				errorsList.Add("pmpDropShipOrganizationID [" + purchaseOrder.pmpDropShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpDropShipLocationID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { purchaseOrder.pmpDropShipOrganizationID, purchaseOrder.pmpDropShipLocationID })))
			{
				errorsList.Add("pmpDropShipLocationID [" + purchaseOrder.pmpDropShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpDropShipContactID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { purchaseOrder.pmpDropShipOrganizationID, purchaseOrder.pmpDropShipLocationID, purchaseOrder.pmpDropShipContactID })))
			{
				errorsList.Add("pmpDropShipContactID [" + purchaseOrder.pmpDropShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpPaymentTermID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("PaymentTerms", new object[1] { "XATPAYMENTTERMID" }, new object[1] { purchaseOrder.pmpPaymentTermID })))
			{
				errorsList.Add("pmpPaymentTermID [" + purchaseOrder.pmpPaymentTermID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpShippingMethodID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { purchaseOrder.pmpShippingMethodID })))
			{
				errorsList.Add("pmpShippingMethodID [" + purchaseOrder.pmpShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpStandardMessageID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("StandardMessages", new object[1] { "XAMSTANDARDMESSAGEID" }, new object[1] { purchaseOrder.pmpStandardMessageID })))
			{
				errorsList.Add("pmpStandardMessageID [" + purchaseOrder.pmpStandardMessageID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpCurrencyRateID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { purchaseOrder.pmpCurrencyRateID })))
			{
				errorsList.Add("pmpCurrencyRateID [" + purchaseOrder.pmpCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpBuyerEmployeeID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { purchaseOrder.pmpBuyerEmployeeID })))
			{
				errorsList.Add("pmpBuyerEmployeeID [" + purchaseOrder.pmpBuyerEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpNextApprovalEmployeeID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { purchaseOrder.pmpNextApprovalEmployeeID })))
			{
				errorsList.Add("pmpNextApprovalEmployeeID [" + purchaseOrder.pmpNextApprovalEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrder.pmpProjectID) && !(await base.ERPPurchaseOrderRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { purchaseOrder.pmpProjectID })))
			{
				errorsList.Add("pmpProjectID [" + purchaseOrder.pmpProjectID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchaseOrderDto>>> Process_GetAllPurchaseOrders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchaseOrderDto> allPurchaseOrdersDto = new List<ERPPurchaseOrderDto>();
		ERPResponseMessageDto<IList<ERPPurchaseOrderDto>> result;
		try
		{
			IERPPurchaseOrderRepository iERPPurchaseOrderRepository = (base.ERPPurchaseOrderRepository = new ERPPurchaseOrderRepository(base.ApiClientContext));
			using (iERPPurchaseOrderRepository)
			{
				foreach (ERPPurchaseOrderInformationDto item2 in await base.ERPPurchaseOrderRepository.GetAllPurchaseOrders(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchaseOrderDto item = new ERPPurchaseOrderDto
					{
						pmpApInvoiceContactID = item2.pmpApInvoiceContactID,
						pmpApInvoiceLocationID = item2.pmpApInvoiceLocationID,
						pmpApprovalDecisionDate = item2.pmpApprovalDecisionDate,
						pmpApprovalRequestDate = item2.pmpApprovalRequestDate,
						pmpBuyerEmployeeID = item2.pmpBuyerEmployeeID,
						pmpClosedDate = item2.pmpClosedDate,
						pmpPurchaseOrderID = item2.pmpPurchaseOrderID,
						pmpCreatedBy = item2.pmpCreatedBy,
						pmpCreatedDate = item2.pmpCreatedDate,
						pmpCurrencyRateID = item2.pmpCurrencyRateID,
						pmpDocuments = item2.pmpDocuments,
						pmpDropShipContactID = item2.pmpDropShipContactID,
						pmpDropShipLocationID = item2.pmpDropShipLocationID,
						pmpDropShipOrganizationID = item2.pmpDropShipOrganizationID,
						pmpDueDate = item2.pmpDueDate,
						pmpUniqueID = item2.pmpUniqueID,
						pmpExchangeRate = item2.pmpExchangeRate,
						pmpFreeOnBoardDescription = item2.pmpFreeOnBoardDescription,
						pmpIntraCompanyPostedDate = item2.pmpIntraCompanyPostedDate,
						pmpClosed = item2.pmpClosed,
						pmpCustomRate = item2.pmpCustomRate,
						pmpIntraCompany = item2.pmpIntraCompany,
						pmpIntraCompanyPosted = item2.pmpIntraCompanyPosted,
						pmpReadyToPrint = item2.pmpReadyToPrint,
						pmpNextApprovalEmployeeID = item2.pmpNextApprovalEmployeeID,
						pmpOrderCommentsRTF = item2.pmpOrderCommentsRTF,
						pmpOrderCommentsText = item2.pmpOrderCommentsText,
						pmpOrderDate = item2.pmpOrderDate,
						pmpOrderSubtotalBase = item2.pmpOrderSubtotalBase,
						pmpOrderSubtotalForeign = item2.pmpOrderSubtotalForeign,
						pmpOrderTaxAmountBase = item2.pmpOrderTaxAmountBase,
						pmpOrderTaxAmountForeign = item2.pmpOrderTaxAmountForeign,
						pmpOrderTotalBase = item2.pmpOrderTotalBase,
						pmpOrderTotalForeign = item2.pmpOrderTotalForeign,
						pmpPaymentTermID = item2.pmpPaymentTermID,
						pmpPlantDepartmentID = item2.pmpPlantDepartmentID,
						pmpPlantID = item2.pmpPlantID,
						pmpProjectID = item2.pmpProjectID,
						pmpPurchaseContactID = item2.pmpPurchaseContactID,
						pmpPurchaseLocationID = item2.pmpPurchaseLocationID,
						pmpRowVersion = item2.pmpRowVersion,
						pmpShippingMethodID = item2.pmpShippingMethodID,
						pmpStandardMessageID = item2.pmpStandardMessageID,
						pmpStatus = item2.pmpStatus,
						pmpSupplierOrganizationID = item2.pmpSupplierOrganizationID,
						CustomFields = item2.CustomFields
					};
					allPurchaseOrdersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchaseOrders]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchaseOrderDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchaseOrdersDto,
				RecordCount = allPurchaseOrdersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderDto>> Process_GetPurchaseOrder(Guid purchaseOrderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchaseOrderDto purchaseOrderDto = null;
		ERPResponseMessageDto<ERPPurchaseOrderDto> result;
		try
		{
			IERPPurchaseOrderRepository iERPPurchaseOrderRepository = (base.ERPPurchaseOrderRepository = new ERPPurchaseOrderRepository(base.ApiClientContext));
			using (iERPPurchaseOrderRepository)
			{
				ERPPurchaseOrderInformationDto eRPPurchaseOrderInformationDto = await base.ERPPurchaseOrderRepository.GetPurchaseOrder(purchaseOrderId);
				purchaseOrderDto = new ERPPurchaseOrderDto
				{
					pmpApInvoiceContactID = eRPPurchaseOrderInformationDto.pmpApInvoiceContactID,
					pmpApInvoiceLocationID = eRPPurchaseOrderInformationDto.pmpApInvoiceLocationID,
					pmpApprovalDecisionDate = eRPPurchaseOrderInformationDto.pmpApprovalDecisionDate,
					pmpApprovalRequestDate = eRPPurchaseOrderInformationDto.pmpApprovalRequestDate,
					pmpBuyerEmployeeID = eRPPurchaseOrderInformationDto.pmpBuyerEmployeeID,
					pmpClosedDate = eRPPurchaseOrderInformationDto.pmpClosedDate,
					pmpPurchaseOrderID = eRPPurchaseOrderInformationDto.pmpPurchaseOrderID,
					pmpCreatedBy = eRPPurchaseOrderInformationDto.pmpCreatedBy,
					pmpCreatedDate = eRPPurchaseOrderInformationDto.pmpCreatedDate,
					pmpCurrencyRateID = eRPPurchaseOrderInformationDto.pmpCurrencyRateID,
					pmpDocuments = eRPPurchaseOrderInformationDto.pmpDocuments,
					pmpDropShipContactID = eRPPurchaseOrderInformationDto.pmpDropShipContactID,
					pmpDropShipLocationID = eRPPurchaseOrderInformationDto.pmpDropShipLocationID,
					pmpDropShipOrganizationID = eRPPurchaseOrderInformationDto.pmpDropShipOrganizationID,
					pmpDueDate = eRPPurchaseOrderInformationDto.pmpDueDate,
					pmpUniqueID = eRPPurchaseOrderInformationDto.pmpUniqueID,
					pmpExchangeRate = eRPPurchaseOrderInformationDto.pmpExchangeRate,
					pmpFreeOnBoardDescription = eRPPurchaseOrderInformationDto.pmpFreeOnBoardDescription,
					pmpIntraCompanyPostedDate = eRPPurchaseOrderInformationDto.pmpIntraCompanyPostedDate,
					pmpClosed = eRPPurchaseOrderInformationDto.pmpClosed,
					pmpCustomRate = eRPPurchaseOrderInformationDto.pmpCustomRate,
					pmpIntraCompany = eRPPurchaseOrderInformationDto.pmpIntraCompany,
					pmpIntraCompanyPosted = eRPPurchaseOrderInformationDto.pmpIntraCompanyPosted,
					pmpReadyToPrint = eRPPurchaseOrderInformationDto.pmpReadyToPrint,
					pmpNextApprovalEmployeeID = eRPPurchaseOrderInformationDto.pmpNextApprovalEmployeeID,
					pmpOrderCommentsRTF = eRPPurchaseOrderInformationDto.pmpOrderCommentsRTF,
					pmpOrderCommentsText = eRPPurchaseOrderInformationDto.pmpOrderCommentsText,
					pmpOrderDate = eRPPurchaseOrderInformationDto.pmpOrderDate,
					pmpOrderSubtotalBase = eRPPurchaseOrderInformationDto.pmpOrderSubtotalBase,
					pmpOrderSubtotalForeign = eRPPurchaseOrderInformationDto.pmpOrderSubtotalForeign,
					pmpOrderTaxAmountBase = eRPPurchaseOrderInformationDto.pmpOrderTaxAmountBase,
					pmpOrderTaxAmountForeign = eRPPurchaseOrderInformationDto.pmpOrderTaxAmountForeign,
					pmpOrderTotalBase = eRPPurchaseOrderInformationDto.pmpOrderTotalBase,
					pmpOrderTotalForeign = eRPPurchaseOrderInformationDto.pmpOrderTotalForeign,
					pmpPaymentTermID = eRPPurchaseOrderInformationDto.pmpPaymentTermID,
					pmpPlantDepartmentID = eRPPurchaseOrderInformationDto.pmpPlantDepartmentID,
					pmpPlantID = eRPPurchaseOrderInformationDto.pmpPlantID,
					pmpProjectID = eRPPurchaseOrderInformationDto.pmpProjectID,
					pmpPurchaseContactID = eRPPurchaseOrderInformationDto.pmpPurchaseContactID,
					pmpPurchaseLocationID = eRPPurchaseOrderInformationDto.pmpPurchaseLocationID,
					pmpRowVersion = eRPPurchaseOrderInformationDto.pmpRowVersion,
					pmpShippingMethodID = eRPPurchaseOrderInformationDto.pmpShippingMethodID,
					pmpStandardMessageID = eRPPurchaseOrderInformationDto.pmpStandardMessageID,
					pmpStatus = eRPPurchaseOrderInformationDto.pmpStatus,
					pmpSupplierOrganizationID = eRPPurchaseOrderInformationDto.pmpSupplierOrganizationID,
					CustomFields = eRPPurchaseOrderInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchaseOrders []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchaseOrderDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderDto>> Process_PutPurchaseOrder(ERPPurchaseOrderDto purchaseOrder)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchaseOrderDto createdObject = null;
		ERPResponseMessageDto<ERPPurchaseOrderDto> result;
		try
		{
			IERPPurchaseOrderRepository iERPPurchaseOrderRepository = (base.ERPPurchaseOrderRepository = new ERPPurchaseOrderRepository(base.ApiClientContext));
			using (iERPPurchaseOrderRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchaseOrderRepository.SavePurchaseOrder(purchaseOrder);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchaseOrderInformationDto eRPPurchaseOrderInformationDto = await base.ERPPurchaseOrderRepository.GetPurchaseOrder(purchaseOrder.pmpUniqueID);
					createdObject = new ERPPurchaseOrderDto
					{
						pmpApInvoiceContactID = eRPPurchaseOrderInformationDto.pmpApInvoiceContactID,
						pmpApInvoiceLocationID = eRPPurchaseOrderInformationDto.pmpApInvoiceLocationID,
						pmpApprovalDecisionDate = eRPPurchaseOrderInformationDto.pmpApprovalDecisionDate,
						pmpApprovalRequestDate = eRPPurchaseOrderInformationDto.pmpApprovalRequestDate,
						pmpBuyerEmployeeID = eRPPurchaseOrderInformationDto.pmpBuyerEmployeeID,
						pmpClosedDate = eRPPurchaseOrderInformationDto.pmpClosedDate,
						pmpPurchaseOrderID = eRPPurchaseOrderInformationDto.pmpPurchaseOrderID,
						pmpCreatedBy = eRPPurchaseOrderInformationDto.pmpCreatedBy,
						pmpCreatedDate = eRPPurchaseOrderInformationDto.pmpCreatedDate,
						pmpCurrencyRateID = eRPPurchaseOrderInformationDto.pmpCurrencyRateID,
						pmpDocuments = eRPPurchaseOrderInformationDto.pmpDocuments,
						pmpDropShipContactID = eRPPurchaseOrderInformationDto.pmpDropShipContactID,
						pmpDropShipLocationID = eRPPurchaseOrderInformationDto.pmpDropShipLocationID,
						pmpDropShipOrganizationID = eRPPurchaseOrderInformationDto.pmpDropShipOrganizationID,
						pmpDueDate = eRPPurchaseOrderInformationDto.pmpDueDate,
						pmpUniqueID = eRPPurchaseOrderInformationDto.pmpUniqueID,
						pmpExchangeRate = eRPPurchaseOrderInformationDto.pmpExchangeRate,
						pmpFreeOnBoardDescription = eRPPurchaseOrderInformationDto.pmpFreeOnBoardDescription,
						pmpIntraCompanyPostedDate = eRPPurchaseOrderInformationDto.pmpIntraCompanyPostedDate,
						pmpClosed = eRPPurchaseOrderInformationDto.pmpClosed,
						pmpCustomRate = eRPPurchaseOrderInformationDto.pmpCustomRate,
						pmpIntraCompany = eRPPurchaseOrderInformationDto.pmpIntraCompany,
						pmpIntraCompanyPosted = eRPPurchaseOrderInformationDto.pmpIntraCompanyPosted,
						pmpReadyToPrint = eRPPurchaseOrderInformationDto.pmpReadyToPrint,
						pmpNextApprovalEmployeeID = eRPPurchaseOrderInformationDto.pmpNextApprovalEmployeeID,
						pmpOrderCommentsRTF = eRPPurchaseOrderInformationDto.pmpOrderCommentsRTF,
						pmpOrderCommentsText = eRPPurchaseOrderInformationDto.pmpOrderCommentsText,
						pmpOrderDate = eRPPurchaseOrderInformationDto.pmpOrderDate,
						pmpOrderSubtotalBase = eRPPurchaseOrderInformationDto.pmpOrderSubtotalBase,
						pmpOrderSubtotalForeign = eRPPurchaseOrderInformationDto.pmpOrderSubtotalForeign,
						pmpOrderTaxAmountBase = eRPPurchaseOrderInformationDto.pmpOrderTaxAmountBase,
						pmpOrderTaxAmountForeign = eRPPurchaseOrderInformationDto.pmpOrderTaxAmountForeign,
						pmpOrderTotalBase = eRPPurchaseOrderInformationDto.pmpOrderTotalBase,
						pmpOrderTotalForeign = eRPPurchaseOrderInformationDto.pmpOrderTotalForeign,
						pmpPaymentTermID = eRPPurchaseOrderInformationDto.pmpPaymentTermID,
						pmpPlantDepartmentID = eRPPurchaseOrderInformationDto.pmpPlantDepartmentID,
						pmpPlantID = eRPPurchaseOrderInformationDto.pmpPlantID,
						pmpProjectID = eRPPurchaseOrderInformationDto.pmpProjectID,
						pmpPurchaseContactID = eRPPurchaseOrderInformationDto.pmpPurchaseContactID,
						pmpPurchaseLocationID = eRPPurchaseOrderInformationDto.pmpPurchaseLocationID,
						pmpRowVersion = eRPPurchaseOrderInformationDto.pmpRowVersion,
						pmpShippingMethodID = eRPPurchaseOrderInformationDto.pmpShippingMethodID,
						pmpStandardMessageID = eRPPurchaseOrderInformationDto.pmpStandardMessageID,
						pmpStatus = eRPPurchaseOrderInformationDto.pmpStatus,
						pmpSupplierOrganizationID = eRPPurchaseOrderInformationDto.pmpSupplierOrganizationID,
						CustomFields = eRPPurchaseOrderInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchaseOrder [{purchaseOrder.pmpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrder(Guid purchaseOrderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderRepository iERPPurchaseOrderRepository = (base.ERPPurchaseOrderRepository = new ERPPurchaseOrderRepository(base.ApiClientContext));
		using (iERPPurchaseOrderRepository)
		{
			if (!(await base.ERPPurchaseOrderRepository.DoesPurchaseOrderExist(purchaseOrderId)))
			{
				base.ErrorsList.Add($"PurchaseOrder [{purchaseOrderId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchaseOrderInformationDto eRPPurchaseOrderInformationDto = await base.ERPPurchaseOrderRepository.GetPurchaseOrder(purchaseOrderId);
				string text = await base.ERPPurchaseOrderRepository.WhereUsed("PurchaseOrders", new object[1] { eRPPurchaseOrderInformationDto.pmpPurchaseOrderID }, new object[1] { "pmpPurchaseOrderID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchaseOrder cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderDto>> Process_DeletePurchaseOrder(Guid purchaseOrderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchaseOrderDto> result;
		try
		{
			IERPPurchaseOrderRepository iERPPurchaseOrderRepository = (base.ERPPurchaseOrderRepository = new ERPPurchaseOrderRepository(base.ApiClientContext));
			using (iERPPurchaseOrderRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchaseOrderRepository.DeleteRowFromTable("PurchaseOrders", "pmp", purchaseOrderId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchaseOrder [{purchaseOrderId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchaseOrderDto()
			};
		}
		return result;
	}
}
