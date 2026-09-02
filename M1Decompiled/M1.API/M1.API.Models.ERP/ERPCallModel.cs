using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCallModel : ERPBaseModel, IERPCallModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCalls(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCallRepository iERPCallRepository = (base.ERPCallRepository = new ERPCallRepository(base.ApiClientContext));
		using (iERPCallRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCallRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCallRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCallRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCallRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCall(Guid callId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallRepository iERPCallRepository = (base.ERPCallRepository = new ERPCallRepository(base.ApiClientContext));
		using (iERPCallRepository)
		{
			if (!(await base.ERPCallRepository.DoesCallExist(callId)))
			{
				errorsList.Add($"Call [{callId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCall(ERPCallDto call)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallRepository iERPCallRepository = (base.ERPCallRepository = new ERPCallRepository(base.ApiClientContext));
		using (iERPCallRepository)
		{
			if (!string.IsNullOrWhiteSpace(call.kbpOrganizationID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { call.kbpOrganizationID })))
			{
				errorsList.Add("kbpOrganizationID [" + call.kbpOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpLocationID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { call.kbpOrganizationID, call.kbpLocationID })))
			{
				errorsList.Add("kbpLocationID [" + call.kbpLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpContactID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { call.kbpOrganizationID, call.kbpLocationID, call.kbpContactID })))
			{
				errorsList.Add("kbpContactID [" + call.kbpContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpCallTypeID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("CallTypes", new object[1] { "KBTCALLTYPEID" }, new object[1] { call.kbpCallTypeID })))
			{
				errorsList.Add("kbpCallTypeID [" + call.kbpCallTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpOpenedByEmployeeID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { call.kbpOpenedByEmployeeID })))
			{
				errorsList.Add("kbpOpenedByEmployeeID [" + call.kbpOpenedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpAssignedToEmployeeID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { call.kbpAssignedToEmployeeID })))
			{
				errorsList.Add("kbpAssignedToEmployeeID [" + call.kbpAssignedToEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpClosedByEmployeeID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { call.kbpClosedByEmployeeID })))
			{
				errorsList.Add("kbpClosedByEmployeeID [" + call.kbpClosedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpPartID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { call.kbpPartID })))
			{
				errorsList.Add("kbpPartID [" + call.kbpPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpPartRevisionID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { call.kbpPartID, call.kbpPartRevisionID })))
			{
				errorsList.Add("kbpPartRevisionID [" + call.kbpPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpReasonID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { call.kbpReasonID })))
			{
				errorsList.Add("kbpReasonID [" + call.kbpReasonID + "] not found.");
			}
			if (call.kbpPriorityID > 0 && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Priorities", new object[1] { "KBRPRIORITYID" }, new object[1] { call.kbpPriorityID })))
			{
				errorsList.Add($"kbpPriorityID [{call.kbpPriorityID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpContactMethodID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("ContactMethods", new object[1] { "KBCCONTACTMETHODID" }, new object[1] { call.kbpContactMethodID })))
			{
				errorsList.Add("kbpContactMethodID [" + call.kbpContactMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpLeadID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Leads", new object[1] { "LOPLEADID" }, new object[1] { call.kbpLeadID })))
			{
				errorsList.Add("kbpLeadID [" + call.kbpLeadID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpQuoteID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { call.kbpQuoteID })))
			{
				errorsList.Add("kbpQuoteID [" + call.kbpQuoteID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpSalesOrderID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { call.kbpSalesOrderID })))
			{
				errorsList.Add("kbpSalesOrderID [" + call.kbpSalesOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpJobID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { call.kbpJobID })))
			{
				errorsList.Add("kbpJobID [" + call.kbpJobID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpShipmentID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { call.kbpShipmentID })))
			{
				errorsList.Add("kbpShipmentID [" + call.kbpShipmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpArInvoiceID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { call.kbpArInvoiceID })))
			{
				errorsList.Add("kbpArInvoiceID [" + call.kbpArInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpPurchaseOrderID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { call.kbpPurchaseOrderID })))
			{
				errorsList.Add("kbpPurchaseOrderID [" + call.kbpPurchaseOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpReceiptID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { call.kbpReceiptID })))
			{
				errorsList.Add("kbpReceiptID [" + call.kbpReceiptID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpApInvoiceID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { call.kbpApInvoiceID })))
			{
				errorsList.Add("kbpApInvoiceID [" + call.kbpApInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpProjectID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { call.kbpProjectID })))
			{
				errorsList.Add("kbpProjectID [" + call.kbpProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpRmaClaimID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { call.kbpRmaClaimID })))
			{
				errorsList.Add("kbpRmaClaimID [" + call.kbpRmaClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpDmrClaimID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { call.kbpDmrClaimID })))
			{
				errorsList.Add("kbpDmrClaimID [" + call.kbpDmrClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpProjectAreaID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { call.kbpProjectID, call.kbpProjectAreaID })))
			{
				errorsList.Add("kbpProjectAreaID [" + call.kbpProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpSerialNumberID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("SerialNumbers", new object[3] { "IMSPARTID", "IMSPARTREVISIONID", "IMSSERIALNUMBERID" }, new object[3] { call.kbpPartID, call.kbpPartRevisionID, call.kbpSerialNumberID })))
			{
				errorsList.Add("kbpSerialNumberID [" + call.kbpSerialNumberID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpMethodPartID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { call.kbpMethodPartID })))
			{
				errorsList.Add("kbpMethodPartID [" + call.kbpMethodPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpMethodRevisionID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { call.kbpMethodPartID, call.kbpMethodRevisionID })))
			{
				errorsList.Add("kbpMethodRevisionID [" + call.kbpMethodRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpArInvoiceOrganizationID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { call.kbpArInvoiceOrganizationID })))
			{
				errorsList.Add("kbpArInvoiceOrganizationID [" + call.kbpArInvoiceOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpArInvoiceLocationID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { call.kbpArInvoiceOrganizationID, call.kbpArInvoiceLocationID })))
			{
				errorsList.Add("kbpArInvoiceLocationID [" + call.kbpArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpArInvoiceContactID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { call.kbpArInvoiceOrganizationID, call.kbpArInvoiceLocationID, call.kbpArInvoiceContactID })))
			{
				errorsList.Add("kbpArInvoiceContactID [" + call.kbpArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpPartGroupID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { call.kbpPartGroupID })))
			{
				errorsList.Add("kbpPartGroupID [" + call.kbpPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpRfqID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { call.kbpRfqID })))
			{
				errorsList.Add("kbpRfqID [" + call.kbpRfqID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(call.kbpCurrencyRateID) && !(await base.ERPCallRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { call.kbpCurrencyRateID })))
			{
				errorsList.Add("kbpCurrencyRateID [" + call.kbpCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCallDto>>> Process_GetAllCalls(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCallDto> allCallsDto = new List<ERPCallDto>();
		ERPResponseMessageDto<IList<ERPCallDto>> result;
		try
		{
			IERPCallRepository iERPCallRepository = (base.ERPCallRepository = new ERPCallRepository(base.ApiClientContext));
			using (iERPCallRepository)
			{
				foreach (ERPCallInformationDto item2 in await base.ERPCallRepository.GetAllCalls(pageSize, pageNumber, filter, orderBy))
				{
					ERPCallDto item = new ERPCallDto
					{
						kbpAcceptedDate = item2.kbpAcceptedDate,
						kbpApInvoiceID = item2.kbpApInvoiceID,
						kbpArInvoiceContactID = item2.kbpArInvoiceContactID,
						kbpArInvoiceID = item2.kbpArInvoiceID,
						kbpArInvoiceLocationID = item2.kbpArInvoiceLocationID,
						kbpArInvoiceOrganizationID = item2.kbpArInvoiceOrganizationID,
						kbpAssignedDate = item2.kbpAssignedDate,
						kbpAssignedToEmployeeID = item2.kbpAssignedToEmployeeID,
						kbpCallTypeID = item2.kbpCallTypeID,
						kbpClosedByEmployeeID = item2.kbpClosedByEmployeeID,
						kbpClosedDate = item2.kbpClosedDate,
						kbpCallID = item2.kbpCallID,
						kbpContactID = item2.kbpContactID,
						kbpContactMethodID = item2.kbpContactMethodID,
						kbpCreatedBy = item2.kbpCreatedBy,
						kbpCreatedDate = item2.kbpCreatedDate,
						kbpCurrencyRateID = item2.kbpCurrencyRateID,
						kbpDmrClaimID = item2.kbpDmrClaimID,
						kbpDueDate = item2.kbpDueDate,
						kbpUniqueID = item2.kbpUniqueID,
						kbpExchangeRate = item2.kbpExchangeRate,
						kbpExtraTime = item2.kbpExtraTime,
						kbpBillable = item2.kbpBillable,
						kbpCreatedFromMobile = item2.kbpCreatedFromMobile,
						kbpCustomRate = item2.kbpCustomRate,
						kbpFieldServiceCall = item2.kbpFieldServiceCall,
						kbpFieldServiceJobCreated = item2.kbpFieldServiceJobCreated,
						kbpInbound = item2.kbpInbound,
						kbpInternalOnly = item2.kbpInternalOnly,
						kbpInvoicedComplete = item2.kbpInvoicedComplete,
						kbpPublished = item2.kbpPublished,
						kbpJobID = item2.kbpJobID,
						kbpLeadID = item2.kbpLeadID,
						kbpLocationID = item2.kbpLocationID,
						kbpLongDescriptionRtf = item2.kbpLongDescriptionRtf,
						kbpLongDescriptionText = item2.kbpLongDescriptionText,
						kbpMethodPartID = item2.kbpMethodPartID,
						kbpMethodRevisionID = item2.kbpMethodRevisionID,
						kbpOpenedByEmployeeID = item2.kbpOpenedByEmployeeID,
						kbpOpenedDate = item2.kbpOpenedDate,
						kbpOrganizationID = item2.kbpOrganizationID,
						kbpOrgPartID = item2.kbpOrgPartID,
						kbpPartGroupID = item2.kbpPartGroupID,
						kbpPartID = item2.kbpPartID,
						kbpPartRevisionID = item2.kbpPartRevisionID,
						kbpPartShortDescription = item2.kbpPartShortDescription,
						kbpPhoneNumber = item2.kbpPhoneNumber,
						kbpPriorityID = item2.kbpPriorityID,
						kbpProjectAreaID = item2.kbpProjectAreaID,
						kbpProjectID = item2.kbpProjectID,
						kbpPurchaseOrderID = item2.kbpPurchaseOrderID,
						kbpQuoteID = item2.kbpQuoteID,
						kbpReasonID = item2.kbpReasonID,
						kbpReceiptID = item2.kbpReceiptID,
						kbpRfqID = item2.kbpRfqID,
						kbpRmaClaimID = item2.kbpRmaClaimID,
						kbpRowVersion = item2.kbpRowVersion,
						kbpSalesOrderID = item2.kbpSalesOrderID,
						kbpSerialNumberID = item2.kbpSerialNumberID,
						kbpShipmentID = item2.kbpShipmentID,
						kbpShortDescription = item2.kbpShortDescription,
						kbpStatus = item2.kbpStatus,
						kbpSubTotalTime = item2.kbpSubTotalTime,
						kbpTemplateFile = item2.kbpTemplateFile,
						kbpTimeSpent = item2.kbpTimeSpent,
						kbpTotalTime = item2.kbpTotalTime,
						CustomFields = item2.CustomFields
					};
					allCallsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Calls]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCallDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCallsDto,
				RecordCount = allCallsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCallDto>> Process_GetCall(Guid callId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCallDto callDto = null;
		ERPResponseMessageDto<ERPCallDto> result;
		try
		{
			IERPCallRepository iERPCallRepository = (base.ERPCallRepository = new ERPCallRepository(base.ApiClientContext));
			using (iERPCallRepository)
			{
				ERPCallInformationDto eRPCallInformationDto = await base.ERPCallRepository.GetCall(callId);
				callDto = new ERPCallDto
				{
					kbpAcceptedDate = eRPCallInformationDto.kbpAcceptedDate,
					kbpApInvoiceID = eRPCallInformationDto.kbpApInvoiceID,
					kbpArInvoiceContactID = eRPCallInformationDto.kbpArInvoiceContactID,
					kbpArInvoiceID = eRPCallInformationDto.kbpArInvoiceID,
					kbpArInvoiceLocationID = eRPCallInformationDto.kbpArInvoiceLocationID,
					kbpArInvoiceOrganizationID = eRPCallInformationDto.kbpArInvoiceOrganizationID,
					kbpAssignedDate = eRPCallInformationDto.kbpAssignedDate,
					kbpAssignedToEmployeeID = eRPCallInformationDto.kbpAssignedToEmployeeID,
					kbpCallTypeID = eRPCallInformationDto.kbpCallTypeID,
					kbpClosedByEmployeeID = eRPCallInformationDto.kbpClosedByEmployeeID,
					kbpClosedDate = eRPCallInformationDto.kbpClosedDate,
					kbpCallID = eRPCallInformationDto.kbpCallID,
					kbpContactID = eRPCallInformationDto.kbpContactID,
					kbpContactMethodID = eRPCallInformationDto.kbpContactMethodID,
					kbpCreatedBy = eRPCallInformationDto.kbpCreatedBy,
					kbpCreatedDate = eRPCallInformationDto.kbpCreatedDate,
					kbpCurrencyRateID = eRPCallInformationDto.kbpCurrencyRateID,
					kbpDmrClaimID = eRPCallInformationDto.kbpDmrClaimID,
					kbpDueDate = eRPCallInformationDto.kbpDueDate,
					kbpUniqueID = eRPCallInformationDto.kbpUniqueID,
					kbpExchangeRate = eRPCallInformationDto.kbpExchangeRate,
					kbpExtraTime = eRPCallInformationDto.kbpExtraTime,
					kbpBillable = eRPCallInformationDto.kbpBillable,
					kbpCreatedFromMobile = eRPCallInformationDto.kbpCreatedFromMobile,
					kbpCustomRate = eRPCallInformationDto.kbpCustomRate,
					kbpFieldServiceCall = eRPCallInformationDto.kbpFieldServiceCall,
					kbpFieldServiceJobCreated = eRPCallInformationDto.kbpFieldServiceJobCreated,
					kbpInbound = eRPCallInformationDto.kbpInbound,
					kbpInternalOnly = eRPCallInformationDto.kbpInternalOnly,
					kbpInvoicedComplete = eRPCallInformationDto.kbpInvoicedComplete,
					kbpPublished = eRPCallInformationDto.kbpPublished,
					kbpJobID = eRPCallInformationDto.kbpJobID,
					kbpLeadID = eRPCallInformationDto.kbpLeadID,
					kbpLocationID = eRPCallInformationDto.kbpLocationID,
					kbpLongDescriptionRtf = eRPCallInformationDto.kbpLongDescriptionRtf,
					kbpLongDescriptionText = eRPCallInformationDto.kbpLongDescriptionText,
					kbpMethodPartID = eRPCallInformationDto.kbpMethodPartID,
					kbpMethodRevisionID = eRPCallInformationDto.kbpMethodRevisionID,
					kbpOpenedByEmployeeID = eRPCallInformationDto.kbpOpenedByEmployeeID,
					kbpOpenedDate = eRPCallInformationDto.kbpOpenedDate,
					kbpOrganizationID = eRPCallInformationDto.kbpOrganizationID,
					kbpOrgPartID = eRPCallInformationDto.kbpOrgPartID,
					kbpPartGroupID = eRPCallInformationDto.kbpPartGroupID,
					kbpPartID = eRPCallInformationDto.kbpPartID,
					kbpPartRevisionID = eRPCallInformationDto.kbpPartRevisionID,
					kbpPartShortDescription = eRPCallInformationDto.kbpPartShortDescription,
					kbpPhoneNumber = eRPCallInformationDto.kbpPhoneNumber,
					kbpPriorityID = eRPCallInformationDto.kbpPriorityID,
					kbpProjectAreaID = eRPCallInformationDto.kbpProjectAreaID,
					kbpProjectID = eRPCallInformationDto.kbpProjectID,
					kbpPurchaseOrderID = eRPCallInformationDto.kbpPurchaseOrderID,
					kbpQuoteID = eRPCallInformationDto.kbpQuoteID,
					kbpReasonID = eRPCallInformationDto.kbpReasonID,
					kbpReceiptID = eRPCallInformationDto.kbpReceiptID,
					kbpRfqID = eRPCallInformationDto.kbpRfqID,
					kbpRmaClaimID = eRPCallInformationDto.kbpRmaClaimID,
					kbpRowVersion = eRPCallInformationDto.kbpRowVersion,
					kbpSalesOrderID = eRPCallInformationDto.kbpSalesOrderID,
					kbpSerialNumberID = eRPCallInformationDto.kbpSerialNumberID,
					kbpShipmentID = eRPCallInformationDto.kbpShipmentID,
					kbpShortDescription = eRPCallInformationDto.kbpShortDescription,
					kbpStatus = eRPCallInformationDto.kbpStatus,
					kbpSubTotalTime = eRPCallInformationDto.kbpSubTotalTime,
					kbpTemplateFile = eRPCallInformationDto.kbpTemplateFile,
					kbpTimeSpent = eRPCallInformationDto.kbpTimeSpent,
					kbpTotalTime = eRPCallInformationDto.kbpTotalTime,
					CustomFields = eRPCallInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Calls []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = callDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCallDto>> Process_PutCall(ERPCallDto call)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCallDto createdObject = null;
		ERPResponseMessageDto<ERPCallDto> result;
		try
		{
			IERPCallRepository iERPCallRepository = (base.ERPCallRepository = new ERPCallRepository(base.ApiClientContext));
			using (iERPCallRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCallRepository.SaveCall(call);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCallInformationDto eRPCallInformationDto = await base.ERPCallRepository.GetCall(call.kbpUniqueID);
					createdObject = new ERPCallDto
					{
						kbpAcceptedDate = eRPCallInformationDto.kbpAcceptedDate,
						kbpApInvoiceID = eRPCallInformationDto.kbpApInvoiceID,
						kbpArInvoiceContactID = eRPCallInformationDto.kbpArInvoiceContactID,
						kbpArInvoiceID = eRPCallInformationDto.kbpArInvoiceID,
						kbpArInvoiceLocationID = eRPCallInformationDto.kbpArInvoiceLocationID,
						kbpArInvoiceOrganizationID = eRPCallInformationDto.kbpArInvoiceOrganizationID,
						kbpAssignedDate = eRPCallInformationDto.kbpAssignedDate,
						kbpAssignedToEmployeeID = eRPCallInformationDto.kbpAssignedToEmployeeID,
						kbpCallTypeID = eRPCallInformationDto.kbpCallTypeID,
						kbpClosedByEmployeeID = eRPCallInformationDto.kbpClosedByEmployeeID,
						kbpClosedDate = eRPCallInformationDto.kbpClosedDate,
						kbpCallID = eRPCallInformationDto.kbpCallID,
						kbpContactID = eRPCallInformationDto.kbpContactID,
						kbpContactMethodID = eRPCallInformationDto.kbpContactMethodID,
						kbpCreatedBy = eRPCallInformationDto.kbpCreatedBy,
						kbpCreatedDate = eRPCallInformationDto.kbpCreatedDate,
						kbpCurrencyRateID = eRPCallInformationDto.kbpCurrencyRateID,
						kbpDmrClaimID = eRPCallInformationDto.kbpDmrClaimID,
						kbpDueDate = eRPCallInformationDto.kbpDueDate,
						kbpUniqueID = eRPCallInformationDto.kbpUniqueID,
						kbpExchangeRate = eRPCallInformationDto.kbpExchangeRate,
						kbpExtraTime = eRPCallInformationDto.kbpExtraTime,
						kbpBillable = eRPCallInformationDto.kbpBillable,
						kbpCreatedFromMobile = eRPCallInformationDto.kbpCreatedFromMobile,
						kbpCustomRate = eRPCallInformationDto.kbpCustomRate,
						kbpFieldServiceCall = eRPCallInformationDto.kbpFieldServiceCall,
						kbpFieldServiceJobCreated = eRPCallInformationDto.kbpFieldServiceJobCreated,
						kbpInbound = eRPCallInformationDto.kbpInbound,
						kbpInternalOnly = eRPCallInformationDto.kbpInternalOnly,
						kbpInvoicedComplete = eRPCallInformationDto.kbpInvoicedComplete,
						kbpPublished = eRPCallInformationDto.kbpPublished,
						kbpJobID = eRPCallInformationDto.kbpJobID,
						kbpLeadID = eRPCallInformationDto.kbpLeadID,
						kbpLocationID = eRPCallInformationDto.kbpLocationID,
						kbpLongDescriptionRtf = eRPCallInformationDto.kbpLongDescriptionRtf,
						kbpLongDescriptionText = eRPCallInformationDto.kbpLongDescriptionText,
						kbpMethodPartID = eRPCallInformationDto.kbpMethodPartID,
						kbpMethodRevisionID = eRPCallInformationDto.kbpMethodRevisionID,
						kbpOpenedByEmployeeID = eRPCallInformationDto.kbpOpenedByEmployeeID,
						kbpOpenedDate = eRPCallInformationDto.kbpOpenedDate,
						kbpOrganizationID = eRPCallInformationDto.kbpOrganizationID,
						kbpOrgPartID = eRPCallInformationDto.kbpOrgPartID,
						kbpPartGroupID = eRPCallInformationDto.kbpPartGroupID,
						kbpPartID = eRPCallInformationDto.kbpPartID,
						kbpPartRevisionID = eRPCallInformationDto.kbpPartRevisionID,
						kbpPartShortDescription = eRPCallInformationDto.kbpPartShortDescription,
						kbpPhoneNumber = eRPCallInformationDto.kbpPhoneNumber,
						kbpPriorityID = eRPCallInformationDto.kbpPriorityID,
						kbpProjectAreaID = eRPCallInformationDto.kbpProjectAreaID,
						kbpProjectID = eRPCallInformationDto.kbpProjectID,
						kbpPurchaseOrderID = eRPCallInformationDto.kbpPurchaseOrderID,
						kbpQuoteID = eRPCallInformationDto.kbpQuoteID,
						kbpReasonID = eRPCallInformationDto.kbpReasonID,
						kbpReceiptID = eRPCallInformationDto.kbpReceiptID,
						kbpRfqID = eRPCallInformationDto.kbpRfqID,
						kbpRmaClaimID = eRPCallInformationDto.kbpRmaClaimID,
						kbpRowVersion = eRPCallInformationDto.kbpRowVersion,
						kbpSalesOrderID = eRPCallInformationDto.kbpSalesOrderID,
						kbpSerialNumberID = eRPCallInformationDto.kbpSerialNumberID,
						kbpShipmentID = eRPCallInformationDto.kbpShipmentID,
						kbpShortDescription = eRPCallInformationDto.kbpShortDescription,
						kbpStatus = eRPCallInformationDto.kbpStatus,
						kbpSubTotalTime = eRPCallInformationDto.kbpSubTotalTime,
						kbpTemplateFile = eRPCallInformationDto.kbpTemplateFile,
						kbpTimeSpent = eRPCallInformationDto.kbpTimeSpent,
						kbpTotalTime = eRPCallInformationDto.kbpTotalTime,
						CustomFields = eRPCallInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Call [{call.kbpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCall(Guid callId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallRepository iERPCallRepository = (base.ERPCallRepository = new ERPCallRepository(base.ApiClientContext));
		using (iERPCallRepository)
		{
			if (!(await base.ERPCallRepository.DoesCallExist(callId)))
			{
				base.ErrorsList.Add($"Call [{callId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCallInformationDto eRPCallInformationDto = await base.ERPCallRepository.GetCall(callId);
				string text = await base.ERPCallRepository.WhereUsed("Calls", new object[1] { eRPCallInformationDto.kbpCallID }, new object[1] { "kbpCallID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Call cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCallDto>> Process_DeleteCall(Guid callId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCallDto> result;
		try
		{
			IERPCallRepository iERPCallRepository = (base.ERPCallRepository = new ERPCallRepository(base.ApiClientContext));
			using (iERPCallRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCallRepository.DeleteRowFromTable("Calls", "kbp", callId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Call [{callId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCallDto()
			};
		}
		return result;
	}
}
