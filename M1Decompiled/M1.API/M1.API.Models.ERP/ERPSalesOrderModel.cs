using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderModel : ERPBaseModel, IERPSalesOrderModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderRepository iERPSalesOrderRepository = (base.ERPSalesOrderRepository = new ERPSalesOrderRepository(base.ApiClientContext));
		using (iERPSalesOrderRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrder(Guid salesOrderId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderRepository iERPSalesOrderRepository = (base.ERPSalesOrderRepository = new ERPSalesOrderRepository(base.ApiClientContext));
		using (iERPSalesOrderRepository)
		{
			if (!(await base.ERPSalesOrderRepository.DoesSalesOrderExist(salesOrderId)))
			{
				errorsList.Add($"SalesOrder [{salesOrderId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrder(ERPSalesOrderDto salesOrder)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderRepository iERPSalesOrderRepository = (base.ERPSalesOrderRepository = new ERPSalesOrderRepository(base.ApiClientContext));
		using (iERPSalesOrderRepository)
		{
			if (!string.IsNullOrWhiteSpace(salesOrder.ompPlantID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { salesOrder.ompPlantID })))
			{
				errorsList.Add("ompPlantID [" + salesOrder.ompPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompPlantDepartmentID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { salesOrder.ompPlantID, salesOrder.ompPlantDepartmentID })))
			{
				errorsList.Add("ompPlantDepartmentID [" + salesOrder.ompPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompCustomerOrganizationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { salesOrder.ompCustomerOrganizationID })))
			{
				errorsList.Add("ompCustomerOrganizationID [" + salesOrder.ompCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompArInvoiceLocationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { salesOrder.ompCustomerOrganizationID, salesOrder.ompArInvoiceLocationID })))
			{
				errorsList.Add("ompArInvoiceLocationID [" + salesOrder.ompArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompArInvoiceContactID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { salesOrder.ompCustomerOrganizationID, salesOrder.ompArInvoiceLocationID, salesOrder.ompArInvoiceContactID })))
			{
				errorsList.Add("ompArInvoiceContactID [" + salesOrder.ompArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompQuoteLocationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { salesOrder.ompCustomerOrganizationID, salesOrder.ompQuoteLocationID })))
			{
				errorsList.Add("ompQuoteLocationID [" + salesOrder.ompQuoteLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompQuoteContactID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { salesOrder.ompCustomerOrganizationID, salesOrder.ompQuoteLocationID, salesOrder.ompQuoteContactID })))
			{
				errorsList.Add("ompQuoteContactID [" + salesOrder.ompQuoteContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompShipOrganizationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { salesOrder.ompShipOrganizationID })))
			{
				errorsList.Add("ompShipOrganizationID [" + salesOrder.ompShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompShipLocationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { salesOrder.ompShipOrganizationID, salesOrder.ompShipLocationID })))
			{
				errorsList.Add("ompShipLocationID [" + salesOrder.ompShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompShipContactID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { salesOrder.ompShipOrganizationID, salesOrder.ompShipLocationID, salesOrder.ompShipContactID })))
			{
				errorsList.Add("ompShipContactID [" + salesOrder.ompShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompShippingMethodID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { salesOrder.ompShippingMethodID })))
			{
				errorsList.Add("ompShippingMethodID [" + salesOrder.ompShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompShippingPaymentTypeID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { salesOrder.ompShippingPaymentTypeID })))
			{
				errorsList.Add("ompShippingPaymentTypeID [" + salesOrder.ompShippingPaymentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompPaymentTermID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("PaymentTerms", new object[1] { "XATPAYMENTTERMID" }, new object[1] { salesOrder.ompPaymentTermID })))
			{
				errorsList.Add("ompPaymentTermID [" + salesOrder.ompPaymentTermID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompResellerOrganizationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { salesOrder.ompResellerOrganizationID })))
			{
				errorsList.Add("ompResellerOrganizationID [" + salesOrder.ompResellerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompResellerLocationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { salesOrder.ompResellerOrganizationID, salesOrder.ompResellerLocationID })))
			{
				errorsList.Add("ompResellerLocationID [" + salesOrder.ompResellerLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompResellerContactID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { salesOrder.ompResellerOrganizationID, salesOrder.ompResellerLocationID, salesOrder.ompResellerContactID })))
			{
				errorsList.Add("ompResellerContactID [" + salesOrder.ompResellerContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompStandardMessageID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("StandardMessages", new object[1] { "XAMSTANDARDMESSAGEID" }, new object[1] { salesOrder.ompStandardMessageID })))
			{
				errorsList.Add("ompStandardMessageID [" + salesOrder.ompStandardMessageID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompCurrencyRateID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { salesOrder.ompCurrencyRateID })))
			{
				errorsList.Add("ompCurrencyRateID [" + salesOrder.ompCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompFreightTaxCodeID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { salesOrder.ompFreightTaxCodeID })))
			{
				errorsList.Add("ompFreightTaxCodeID [" + salesOrder.ompFreightTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompSecondFreightTaxCodeID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { salesOrder.ompSecondFreightTaxCodeID })))
			{
				errorsList.Add("ompSecondFreightTaxCodeID [" + salesOrder.ompSecondFreightTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompNextApprovalEmployeeID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { salesOrder.ompNextApprovalEmployeeID })))
			{
				errorsList.Add("ompNextApprovalEmployeeID [" + salesOrder.ompNextApprovalEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompProjectID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { salesOrder.ompProjectID })))
			{
				errorsList.Add("ompProjectID [" + salesOrder.ompProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompCallID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("Calls", new object[1] { "KBPCALLID" }, new object[1] { salesOrder.ompCallID })))
			{
				errorsList.Add("ompCallID [" + salesOrder.ompCallID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompFedEx3rdPartyOrganizationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONS", new object[1] { "CMOORGANIZATIONID" }, new object[1] { salesOrder.ompFedEx3rdPartyOrganizationID })))
			{
				errorsList.Add("ompFedEx3rdPartyOrganizationID [" + salesOrder.ompFedEx3rdPartyOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompFedEx3rdPartyLocationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONLOCATIONS", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { salesOrder.ompFedEx3rdPartyOrganizationID, salesOrder.ompFedEx3rdPartyLocationID })))
			{
				errorsList.Add("ompFedEx3rdPartyLocationID [" + salesOrder.ompFedEx3rdPartyLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompUps3rdPartyOrganizationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONS", new object[1] { "CMOORGANIZATIONID" }, new object[1] { salesOrder.ompUps3rdPartyOrganizationID })))
			{
				errorsList.Add("ompUps3rdPartyOrganizationID [" + salesOrder.ompUps3rdPartyOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrder.ompUps3rdPartyLocationID) && !(await base.ERPSalesOrderRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONLOCATIONS", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { salesOrder.ompUps3rdPartyOrganizationID, salesOrder.ompUps3rdPartyLocationID })))
			{
				errorsList.Add("ompUps3rdPartyLocationID [" + salesOrder.ompUps3rdPartyLocationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderDto>>> Process_GetAllSalesOrders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderDto> allSalesOrdersDto = new List<ERPSalesOrderDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderDto>> result;
		try
		{
			IERPSalesOrderRepository iERPSalesOrderRepository = (base.ERPSalesOrderRepository = new ERPSalesOrderRepository(base.ApiClientContext));
			using (iERPSalesOrderRepository)
			{
				foreach (ERPSalesOrderInformationDto item2 in await base.ERPSalesOrderRepository.GetAllSalesOrders(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderDto item = new ERPSalesOrderDto
					{
						ompApprovalDecisionDate = item2.ompApprovalDecisionDate,
						ompApprovalRequestDate = item2.ompApprovalRequestDate,
						ompArInvoiceContactID = item2.ompArInvoiceContactID,
						ompArInvoiceLocationID = item2.ompArInvoiceLocationID,
						ompCallID = item2.ompCallID,
						ompClosedDate = item2.ompClosedDate,
						ompSalesOrderID = item2.ompSalesOrderID,
						ompCreatedBy = item2.ompCreatedBy,
						ompCreatedDate = item2.ompCreatedDate,
						ompCurrencyRateID = item2.ompCurrencyRateID,
						ompCustomerOrganizationID = item2.ompCustomerOrganizationID,
						ompCustomerPo = item2.ompCustomerPo,
						ompDepositAmountBase = item2.ompDepositAmountBase,
						ompDepositAmountForeign = item2.ompDepositAmountForeign,
						ompDepositPercent = item2.ompDepositPercent,
						ompDiscountTotalBase = item2.ompDiscountTotalBase,
						ompDiscountTotalForeign = item2.ompDiscountTotalForeign,
						ompUniqueID = item2.ompUniqueID,
						ompExchangeRate = item2.ompExchangeRate,
						ompFedEx3rdPartyLocationID = item2.ompFedEx3rdPartyLocationID,
						ompFedEx3rdPartyOrganizationID = item2.ompFedEx3rdPartyOrganizationID,
						ompFedExAccountNumber = item2.ompFedExAccountNumber,
						ompFedExBillingOption = item2.ompFedExBillingOption,
						ompFreeOnBoardDescription = item2.ompFreeOnBoardDescription,
						ompFreightAmountBase = item2.ompFreightAmountBase,
						ompFreightAmountForeign = item2.ompFreightAmountForeign,
						ompFreightSubtotalBase = item2.ompFreightSubtotalBase,
						ompFreightSubtotalForeign = item2.ompFreightSubtotalForeign,
						ompFreightTaxAmountBase = item2.ompFreightTaxAmountBase,
						ompFreightTaxAmountForeign = item2.ompFreightTaxAmountForeign,
						ompFreightTaxCodeID = item2.ompFreightTaxCodeID,
						ompFreightTotalBase = item2.ompFreightTotalBase,
						ompFreightTotalForeign = item2.ompFreightTotalForeign,
						ompFullOrderSubtotalBase = item2.ompFullOrderSubtotalBase,
						ompFullOrderSubtotalForeign = item2.ompFullOrderSubtotalForeign,
						ompAvalaraTaxCalculated = item2.ompAvalaraTaxCalculated,
						ompClosed = item2.ompClosed,
						ompCreatedByEdi = item2.ompCreatedByEdi,
						ompCustomRate = item2.ompCustomRate,
						ompDeposit = item2.ompDeposit,
						ompDepositCreated = item2.ompDepositCreated,
						ompReadyToPrint = item2.ompReadyToPrint,
						ompNextApprovalEmployeeID = item2.ompNextApprovalEmployeeID,
						ompOrderCommentsRTF = item2.ompOrderCommentsRTF,
						ompOrderCommentsText = item2.ompOrderCommentsText,
						ompOrderDate = item2.ompOrderDate,
						ompOrderSubtotalBase = item2.ompOrderSubtotalBase,
						ompOrderSubTotalForeign = item2.ompOrderSubTotalForeign,
						ompOrderTaxAmountBase = item2.ompOrderTaxAmountBase,
						ompOrderTaxAmountForeign = item2.ompOrderTaxAmountForeign,
						ompOrderTotalBase = item2.ompOrderTotalBase,
						ompOrderTotalForeign = item2.ompOrderTotalForeign,
						ompPaymentTermID = item2.ompPaymentTermID,
						ompPlantDepartmentID = item2.ompPlantDepartmentID,
						ompPlantID = item2.ompPlantID,
						ompProjectID = item2.ompProjectID,
						ompQuoteContactID = item2.ompQuoteContactID,
						ompQuoteLocationID = item2.ompQuoteLocationID,
						ompRequestedShipDate = item2.ompRequestedShipDate,
						ompResellerContactID = item2.ompResellerContactID,
						ompResellerLocationID = item2.ompResellerLocationID,
						ompResellerOrganizationID = item2.ompResellerOrganizationID,
						ompRowVersion = item2.ompRowVersion,
						ompSecondFreightTaxAmtBase = item2.ompSecondFreightTaxAmtBase,
						ompSecondFreightTaxAmtForeign = item2.ompSecondFreightTaxAmtForeign,
						ompSecondFreightTaxCodeID = item2.ompSecondFreightTaxCodeID,
						ompShipContactID = item2.ompShipContactID,
						ompShipLocationID = item2.ompShipLocationID,
						ompShipOrganizationID = item2.ompShipOrganizationID,
						ompShippingMethodID = item2.ompShippingMethodID,
						ompShippingPaymentTypeID = item2.ompShippingPaymentTypeID,
						ompSplitPercentTotal = item2.ompSplitPercentTotal,
						ompStandardMessageID = item2.ompStandardMessageID,
						ompStatus = item2.ompStatus,
						ompTaxSubtotalBase = item2.ompTaxSubtotalBase,
						ompTaxSubtotalForeign = item2.ompTaxSubtotalForeign,
						ompTotalOrderWeight = item2.ompTotalOrderWeight,
						ompUps3rdPartyLocationID = item2.ompUps3rdPartyLocationID,
						ompUps3rdPartyOrganizationID = item2.ompUps3rdPartyOrganizationID,
						ompUpsAccountNumber = item2.ompUpsAccountNumber,
						ompUpsBillingOption = item2.ompUpsBillingOption,
						CustomFields = item2.CustomFields
					};
					allSalesOrdersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrders]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrdersDto,
				RecordCount = allSalesOrdersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderDto>> Process_GetSalesOrder(Guid salesOrderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderDto salesOrderDto = null;
		ERPResponseMessageDto<ERPSalesOrderDto> result;
		try
		{
			IERPSalesOrderRepository iERPSalesOrderRepository = (base.ERPSalesOrderRepository = new ERPSalesOrderRepository(base.ApiClientContext));
			using (iERPSalesOrderRepository)
			{
				ERPSalesOrderInformationDto eRPSalesOrderInformationDto = await base.ERPSalesOrderRepository.GetSalesOrder(salesOrderId);
				salesOrderDto = new ERPSalesOrderDto
				{
					ompApprovalDecisionDate = eRPSalesOrderInformationDto.ompApprovalDecisionDate,
					ompApprovalRequestDate = eRPSalesOrderInformationDto.ompApprovalRequestDate,
					ompArInvoiceContactID = eRPSalesOrderInformationDto.ompArInvoiceContactID,
					ompArInvoiceLocationID = eRPSalesOrderInformationDto.ompArInvoiceLocationID,
					ompCallID = eRPSalesOrderInformationDto.ompCallID,
					ompClosedDate = eRPSalesOrderInformationDto.ompClosedDate,
					ompSalesOrderID = eRPSalesOrderInformationDto.ompSalesOrderID,
					ompCreatedBy = eRPSalesOrderInformationDto.ompCreatedBy,
					ompCreatedDate = eRPSalesOrderInformationDto.ompCreatedDate,
					ompCurrencyRateID = eRPSalesOrderInformationDto.ompCurrencyRateID,
					ompCustomerOrganizationID = eRPSalesOrderInformationDto.ompCustomerOrganizationID,
					ompCustomerPo = eRPSalesOrderInformationDto.ompCustomerPo,
					ompDepositAmountBase = eRPSalesOrderInformationDto.ompDepositAmountBase,
					ompDepositAmountForeign = eRPSalesOrderInformationDto.ompDepositAmountForeign,
					ompDepositPercent = eRPSalesOrderInformationDto.ompDepositPercent,
					ompDiscountTotalBase = eRPSalesOrderInformationDto.ompDiscountTotalBase,
					ompDiscountTotalForeign = eRPSalesOrderInformationDto.ompDiscountTotalForeign,
					ompUniqueID = eRPSalesOrderInformationDto.ompUniqueID,
					ompExchangeRate = eRPSalesOrderInformationDto.ompExchangeRate,
					ompFedEx3rdPartyLocationID = eRPSalesOrderInformationDto.ompFedEx3rdPartyLocationID,
					ompFedEx3rdPartyOrganizationID = eRPSalesOrderInformationDto.ompFedEx3rdPartyOrganizationID,
					ompFedExAccountNumber = eRPSalesOrderInformationDto.ompFedExAccountNumber,
					ompFedExBillingOption = eRPSalesOrderInformationDto.ompFedExBillingOption,
					ompFreeOnBoardDescription = eRPSalesOrderInformationDto.ompFreeOnBoardDescription,
					ompFreightAmountBase = eRPSalesOrderInformationDto.ompFreightAmountBase,
					ompFreightAmountForeign = eRPSalesOrderInformationDto.ompFreightAmountForeign,
					ompFreightSubtotalBase = eRPSalesOrderInformationDto.ompFreightSubtotalBase,
					ompFreightSubtotalForeign = eRPSalesOrderInformationDto.ompFreightSubtotalForeign,
					ompFreightTaxAmountBase = eRPSalesOrderInformationDto.ompFreightTaxAmountBase,
					ompFreightTaxAmountForeign = eRPSalesOrderInformationDto.ompFreightTaxAmountForeign,
					ompFreightTaxCodeID = eRPSalesOrderInformationDto.ompFreightTaxCodeID,
					ompFreightTotalBase = eRPSalesOrderInformationDto.ompFreightTotalBase,
					ompFreightTotalForeign = eRPSalesOrderInformationDto.ompFreightTotalForeign,
					ompFullOrderSubtotalBase = eRPSalesOrderInformationDto.ompFullOrderSubtotalBase,
					ompFullOrderSubtotalForeign = eRPSalesOrderInformationDto.ompFullOrderSubtotalForeign,
					ompAvalaraTaxCalculated = eRPSalesOrderInformationDto.ompAvalaraTaxCalculated,
					ompClosed = eRPSalesOrderInformationDto.ompClosed,
					ompCreatedByEdi = eRPSalesOrderInformationDto.ompCreatedByEdi,
					ompCustomRate = eRPSalesOrderInformationDto.ompCustomRate,
					ompDeposit = eRPSalesOrderInformationDto.ompDeposit,
					ompDepositCreated = eRPSalesOrderInformationDto.ompDepositCreated,
					ompReadyToPrint = eRPSalesOrderInformationDto.ompReadyToPrint,
					ompNextApprovalEmployeeID = eRPSalesOrderInformationDto.ompNextApprovalEmployeeID,
					ompOrderCommentsRTF = eRPSalesOrderInformationDto.ompOrderCommentsRTF,
					ompOrderCommentsText = eRPSalesOrderInformationDto.ompOrderCommentsText,
					ompOrderDate = eRPSalesOrderInformationDto.ompOrderDate,
					ompOrderSubtotalBase = eRPSalesOrderInformationDto.ompOrderSubtotalBase,
					ompOrderSubTotalForeign = eRPSalesOrderInformationDto.ompOrderSubTotalForeign,
					ompOrderTaxAmountBase = eRPSalesOrderInformationDto.ompOrderTaxAmountBase,
					ompOrderTaxAmountForeign = eRPSalesOrderInformationDto.ompOrderTaxAmountForeign,
					ompOrderTotalBase = eRPSalesOrderInformationDto.ompOrderTotalBase,
					ompOrderTotalForeign = eRPSalesOrderInformationDto.ompOrderTotalForeign,
					ompPaymentTermID = eRPSalesOrderInformationDto.ompPaymentTermID,
					ompPlantDepartmentID = eRPSalesOrderInformationDto.ompPlantDepartmentID,
					ompPlantID = eRPSalesOrderInformationDto.ompPlantID,
					ompProjectID = eRPSalesOrderInformationDto.ompProjectID,
					ompQuoteContactID = eRPSalesOrderInformationDto.ompQuoteContactID,
					ompQuoteLocationID = eRPSalesOrderInformationDto.ompQuoteLocationID,
					ompRequestedShipDate = eRPSalesOrderInformationDto.ompRequestedShipDate,
					ompResellerContactID = eRPSalesOrderInformationDto.ompResellerContactID,
					ompResellerLocationID = eRPSalesOrderInformationDto.ompResellerLocationID,
					ompResellerOrganizationID = eRPSalesOrderInformationDto.ompResellerOrganizationID,
					ompRowVersion = eRPSalesOrderInformationDto.ompRowVersion,
					ompSecondFreightTaxAmtBase = eRPSalesOrderInformationDto.ompSecondFreightTaxAmtBase,
					ompSecondFreightTaxAmtForeign = eRPSalesOrderInformationDto.ompSecondFreightTaxAmtForeign,
					ompSecondFreightTaxCodeID = eRPSalesOrderInformationDto.ompSecondFreightTaxCodeID,
					ompShipContactID = eRPSalesOrderInformationDto.ompShipContactID,
					ompShipLocationID = eRPSalesOrderInformationDto.ompShipLocationID,
					ompShipOrganizationID = eRPSalesOrderInformationDto.ompShipOrganizationID,
					ompShippingMethodID = eRPSalesOrderInformationDto.ompShippingMethodID,
					ompShippingPaymentTypeID = eRPSalesOrderInformationDto.ompShippingPaymentTypeID,
					ompSplitPercentTotal = eRPSalesOrderInformationDto.ompSplitPercentTotal,
					ompStandardMessageID = eRPSalesOrderInformationDto.ompStandardMessageID,
					ompStatus = eRPSalesOrderInformationDto.ompStatus,
					ompTaxSubtotalBase = eRPSalesOrderInformationDto.ompTaxSubtotalBase,
					ompTaxSubtotalForeign = eRPSalesOrderInformationDto.ompTaxSubtotalForeign,
					ompTotalOrderWeight = eRPSalesOrderInformationDto.ompTotalOrderWeight,
					ompUps3rdPartyLocationID = eRPSalesOrderInformationDto.ompUps3rdPartyLocationID,
					ompUps3rdPartyOrganizationID = eRPSalesOrderInformationDto.ompUps3rdPartyOrganizationID,
					ompUpsAccountNumber = eRPSalesOrderInformationDto.ompUpsAccountNumber,
					ompUpsBillingOption = eRPSalesOrderInformationDto.ompUpsBillingOption,
					CustomFields = eRPSalesOrderInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrders []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderDto>> Process_PutSalesOrder(ERPSalesOrderDto salesOrder)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderDto> result;
		try
		{
			IERPSalesOrderRepository iERPSalesOrderRepository = (base.ERPSalesOrderRepository = new ERPSalesOrderRepository(base.ApiClientContext));
			using (iERPSalesOrderRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderRepository.SaveSalesOrder(salesOrder);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderInformationDto eRPSalesOrderInformationDto = await base.ERPSalesOrderRepository.GetSalesOrder(salesOrder.ompUniqueID);
					createdObject = new ERPSalesOrderDto
					{
						ompApprovalDecisionDate = eRPSalesOrderInformationDto.ompApprovalDecisionDate,
						ompApprovalRequestDate = eRPSalesOrderInformationDto.ompApprovalRequestDate,
						ompArInvoiceContactID = eRPSalesOrderInformationDto.ompArInvoiceContactID,
						ompArInvoiceLocationID = eRPSalesOrderInformationDto.ompArInvoiceLocationID,
						ompCallID = eRPSalesOrderInformationDto.ompCallID,
						ompClosedDate = eRPSalesOrderInformationDto.ompClosedDate,
						ompSalesOrderID = eRPSalesOrderInformationDto.ompSalesOrderID,
						ompCreatedBy = eRPSalesOrderInformationDto.ompCreatedBy,
						ompCreatedDate = eRPSalesOrderInformationDto.ompCreatedDate,
						ompCurrencyRateID = eRPSalesOrderInformationDto.ompCurrencyRateID,
						ompCustomerOrganizationID = eRPSalesOrderInformationDto.ompCustomerOrganizationID,
						ompCustomerPo = eRPSalesOrderInformationDto.ompCustomerPo,
						ompDepositAmountBase = eRPSalesOrderInformationDto.ompDepositAmountBase,
						ompDepositAmountForeign = eRPSalesOrderInformationDto.ompDepositAmountForeign,
						ompDepositPercent = eRPSalesOrderInformationDto.ompDepositPercent,
						ompDiscountTotalBase = eRPSalesOrderInformationDto.ompDiscountTotalBase,
						ompDiscountTotalForeign = eRPSalesOrderInformationDto.ompDiscountTotalForeign,
						ompUniqueID = eRPSalesOrderInformationDto.ompUniqueID,
						ompExchangeRate = eRPSalesOrderInformationDto.ompExchangeRate,
						ompFedEx3rdPartyLocationID = eRPSalesOrderInformationDto.ompFedEx3rdPartyLocationID,
						ompFedEx3rdPartyOrganizationID = eRPSalesOrderInformationDto.ompFedEx3rdPartyOrganizationID,
						ompFedExAccountNumber = eRPSalesOrderInformationDto.ompFedExAccountNumber,
						ompFedExBillingOption = eRPSalesOrderInformationDto.ompFedExBillingOption,
						ompFreeOnBoardDescription = eRPSalesOrderInformationDto.ompFreeOnBoardDescription,
						ompFreightAmountBase = eRPSalesOrderInformationDto.ompFreightAmountBase,
						ompFreightAmountForeign = eRPSalesOrderInformationDto.ompFreightAmountForeign,
						ompFreightSubtotalBase = eRPSalesOrderInformationDto.ompFreightSubtotalBase,
						ompFreightSubtotalForeign = eRPSalesOrderInformationDto.ompFreightSubtotalForeign,
						ompFreightTaxAmountBase = eRPSalesOrderInformationDto.ompFreightTaxAmountBase,
						ompFreightTaxAmountForeign = eRPSalesOrderInformationDto.ompFreightTaxAmountForeign,
						ompFreightTaxCodeID = eRPSalesOrderInformationDto.ompFreightTaxCodeID,
						ompFreightTotalBase = eRPSalesOrderInformationDto.ompFreightTotalBase,
						ompFreightTotalForeign = eRPSalesOrderInformationDto.ompFreightTotalForeign,
						ompFullOrderSubtotalBase = eRPSalesOrderInformationDto.ompFullOrderSubtotalBase,
						ompFullOrderSubtotalForeign = eRPSalesOrderInformationDto.ompFullOrderSubtotalForeign,
						ompAvalaraTaxCalculated = eRPSalesOrderInformationDto.ompAvalaraTaxCalculated,
						ompClosed = eRPSalesOrderInformationDto.ompClosed,
						ompCreatedByEdi = eRPSalesOrderInformationDto.ompCreatedByEdi,
						ompCustomRate = eRPSalesOrderInformationDto.ompCustomRate,
						ompDeposit = eRPSalesOrderInformationDto.ompDeposit,
						ompDepositCreated = eRPSalesOrderInformationDto.ompDepositCreated,
						ompReadyToPrint = eRPSalesOrderInformationDto.ompReadyToPrint,
						ompNextApprovalEmployeeID = eRPSalesOrderInformationDto.ompNextApprovalEmployeeID,
						ompOrderCommentsRTF = eRPSalesOrderInformationDto.ompOrderCommentsRTF,
						ompOrderCommentsText = eRPSalesOrderInformationDto.ompOrderCommentsText,
						ompOrderDate = eRPSalesOrderInformationDto.ompOrderDate,
						ompOrderSubtotalBase = eRPSalesOrderInformationDto.ompOrderSubtotalBase,
						ompOrderSubTotalForeign = eRPSalesOrderInformationDto.ompOrderSubTotalForeign,
						ompOrderTaxAmountBase = eRPSalesOrderInformationDto.ompOrderTaxAmountBase,
						ompOrderTaxAmountForeign = eRPSalesOrderInformationDto.ompOrderTaxAmountForeign,
						ompOrderTotalBase = eRPSalesOrderInformationDto.ompOrderTotalBase,
						ompOrderTotalForeign = eRPSalesOrderInformationDto.ompOrderTotalForeign,
						ompPaymentTermID = eRPSalesOrderInformationDto.ompPaymentTermID,
						ompPlantDepartmentID = eRPSalesOrderInformationDto.ompPlantDepartmentID,
						ompPlantID = eRPSalesOrderInformationDto.ompPlantID,
						ompProjectID = eRPSalesOrderInformationDto.ompProjectID,
						ompQuoteContactID = eRPSalesOrderInformationDto.ompQuoteContactID,
						ompQuoteLocationID = eRPSalesOrderInformationDto.ompQuoteLocationID,
						ompRequestedShipDate = eRPSalesOrderInformationDto.ompRequestedShipDate,
						ompResellerContactID = eRPSalesOrderInformationDto.ompResellerContactID,
						ompResellerLocationID = eRPSalesOrderInformationDto.ompResellerLocationID,
						ompResellerOrganizationID = eRPSalesOrderInformationDto.ompResellerOrganizationID,
						ompRowVersion = eRPSalesOrderInformationDto.ompRowVersion,
						ompSecondFreightTaxAmtBase = eRPSalesOrderInformationDto.ompSecondFreightTaxAmtBase,
						ompSecondFreightTaxAmtForeign = eRPSalesOrderInformationDto.ompSecondFreightTaxAmtForeign,
						ompSecondFreightTaxCodeID = eRPSalesOrderInformationDto.ompSecondFreightTaxCodeID,
						ompShipContactID = eRPSalesOrderInformationDto.ompShipContactID,
						ompShipLocationID = eRPSalesOrderInformationDto.ompShipLocationID,
						ompShipOrganizationID = eRPSalesOrderInformationDto.ompShipOrganizationID,
						ompShippingMethodID = eRPSalesOrderInformationDto.ompShippingMethodID,
						ompShippingPaymentTypeID = eRPSalesOrderInformationDto.ompShippingPaymentTypeID,
						ompSplitPercentTotal = eRPSalesOrderInformationDto.ompSplitPercentTotal,
						ompStandardMessageID = eRPSalesOrderInformationDto.ompStandardMessageID,
						ompStatus = eRPSalesOrderInformationDto.ompStatus,
						ompTaxSubtotalBase = eRPSalesOrderInformationDto.ompTaxSubtotalBase,
						ompTaxSubtotalForeign = eRPSalesOrderInformationDto.ompTaxSubtotalForeign,
						ompTotalOrderWeight = eRPSalesOrderInformationDto.ompTotalOrderWeight,
						ompUps3rdPartyLocationID = eRPSalesOrderInformationDto.ompUps3rdPartyLocationID,
						ompUps3rdPartyOrganizationID = eRPSalesOrderInformationDto.ompUps3rdPartyOrganizationID,
						ompUpsAccountNumber = eRPSalesOrderInformationDto.ompUpsAccountNumber,
						ompUpsBillingOption = eRPSalesOrderInformationDto.ompUpsBillingOption,
						CustomFields = eRPSalesOrderInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrder [{salesOrder.ompUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrder(Guid salesOrderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderRepository iERPSalesOrderRepository = (base.ERPSalesOrderRepository = new ERPSalesOrderRepository(base.ApiClientContext));
		using (iERPSalesOrderRepository)
		{
			if (!(await base.ERPSalesOrderRepository.DoesSalesOrderExist(salesOrderId)))
			{
				base.ErrorsList.Add($"SalesOrder [{salesOrderId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderInformationDto eRPSalesOrderInformationDto = await base.ERPSalesOrderRepository.GetSalesOrder(salesOrderId);
				string text = await base.ERPSalesOrderRepository.WhereUsed("SalesOrders", new object[1] { eRPSalesOrderInformationDto.ompSalesOrderID }, new object[1] { "ompSalesOrderID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrder cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderDto>> Process_DeleteSalesOrder(Guid salesOrderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderDto> result;
		try
		{
			IERPSalesOrderRepository iERPSalesOrderRepository = (base.ERPSalesOrderRepository = new ERPSalesOrderRepository(base.ApiClientContext));
			using (iERPSalesOrderRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderRepository.DeleteRowFromTable("SalesOrders", "omp", salesOrderId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrder [{salesOrderId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderDto()
			};
		}
		return result;
	}
}
