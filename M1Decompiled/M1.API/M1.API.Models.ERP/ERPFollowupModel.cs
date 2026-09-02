using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPFollowupModel : ERPBaseModel, IERPFollowupModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllFollowups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPFollowupRepository iERPFollowupRepository = (base.ERPFollowupRepository = new ERPFollowupRepository(base.ApiClientContext));
		using (iERPFollowupRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPFollowupRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPFollowupRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPFollowupRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPFollowupRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetFollowup(Guid followupId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFollowupRepository iERPFollowupRepository = (base.ERPFollowupRepository = new ERPFollowupRepository(base.ApiClientContext));
		using (iERPFollowupRepository)
		{
			if (!(await base.ERPFollowupRepository.DoesFollowupExist(followupId)))
			{
				errorsList.Add($"Followup [{followupId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutFollowup(ERPFollowupDto followup)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFollowupRepository iERPFollowupRepository = (base.ERPFollowupRepository = new ERPFollowupRepository(base.ApiClientContext));
		using (iERPFollowupRepository)
		{
			if (!string.IsNullOrWhiteSpace(followup.cmfOrganizationID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { followup.cmfOrganizationID })))
			{
				errorsList.Add("cmfOrganizationID [" + followup.cmfOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfLocationID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { followup.cmfOrganizationID, followup.cmfLocationID })))
			{
				errorsList.Add("cmfLocationID [" + followup.cmfLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfContactID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { followup.cmfOrganizationID, followup.cmfLocationID, followup.cmfContactID })))
			{
				errorsList.Add("cmfContactID [" + followup.cmfContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfAttachedToEmployeeID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { followup.cmfAttachedToEmployeeID })))
			{
				errorsList.Add("cmfAttachedToEmployeeID [" + followup.cmfAttachedToEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfAssignedToEmployeeID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { followup.cmfAssignedToEmployeeID })))
			{
				errorsList.Add("cmfAssignedToEmployeeID [" + followup.cmfAssignedToEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfCallID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Calls", new object[1] { "KBPCALLID" }, new object[1] { followup.cmfCallID })))
			{
				errorsList.Add("cmfCallID [" + followup.cmfCallID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfLeadID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Leads", new object[1] { "LOPLEADID" }, new object[1] { followup.cmfLeadID })))
			{
				errorsList.Add("cmfLeadID [" + followup.cmfLeadID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfQuoteID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { followup.cmfQuoteID })))
			{
				errorsList.Add("cmfQuoteID [" + followup.cmfQuoteID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfSalesOrderID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { followup.cmfSalesOrderID })))
			{
				errorsList.Add("cmfSalesOrderID [" + followup.cmfSalesOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfJobID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { followup.cmfJobID })))
			{
				errorsList.Add("cmfJobID [" + followup.cmfJobID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfShipmentID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { followup.cmfShipmentID })))
			{
				errorsList.Add("cmfShipmentID [" + followup.cmfShipmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfArInvoiceID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { followup.cmfArInvoiceID })))
			{
				errorsList.Add("cmfArInvoiceID [" + followup.cmfArInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfRfqID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { followup.cmfRfqID })))
			{
				errorsList.Add("cmfRfqID [" + followup.cmfRfqID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfPurchaseOrderID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { followup.cmfPurchaseOrderID })))
			{
				errorsList.Add("cmfPurchaseOrderID [" + followup.cmfPurchaseOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfReceiptID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { followup.cmfReceiptID })))
			{
				errorsList.Add("cmfReceiptID [" + followup.cmfReceiptID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfApInvoiceID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { followup.cmfApInvoiceID })))
			{
				errorsList.Add("cmfApInvoiceID [" + followup.cmfApInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfProjectID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { followup.cmfProjectID })))
			{
				errorsList.Add("cmfProjectID [" + followup.cmfProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfRmaClaimID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { followup.cmfRmaClaimID })))
			{
				errorsList.Add("cmfRmaClaimID [" + followup.cmfRmaClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfDmrClaimID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { followup.cmfDmrClaimID })))
			{
				errorsList.Add("cmfDmrClaimID [" + followup.cmfDmrClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfProjectAreaID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { followup.cmfProjectID, followup.cmfProjectAreaID })))
			{
				errorsList.Add("cmfProjectAreaID [" + followup.cmfProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfAssetID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { followup.cmfAssetID })))
			{
				errorsList.Add("cmfAssetID [" + followup.cmfAssetID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(followup.cmfChangeRequestID) && !(await base.ERPFollowupRepository.DoesRecordExistInTableUsingKeys("ChangeRequests", new object[1] { "CHPCHANGEREQUESTID" }, new object[1] { followup.cmfChangeRequestID })))
			{
				errorsList.Add("cmfChangeRequestID [" + followup.cmfChangeRequestID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPFollowupDto>>> Process_GetAllFollowups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPFollowupDto> allFollowupsDto = new List<ERPFollowupDto>();
		ERPResponseMessageDto<IList<ERPFollowupDto>> result;
		try
		{
			IERPFollowupRepository iERPFollowupRepository = (base.ERPFollowupRepository = new ERPFollowupRepository(base.ApiClientContext));
			using (iERPFollowupRepository)
			{
				foreach (ERPFollowupInformationDto item2 in await base.ERPFollowupRepository.GetAllFollowups(pageSize, pageNumber, filter, orderBy))
				{
					ERPFollowupDto item = new ERPFollowupDto
					{
						cmfApInvoiceID = item2.cmfApInvoiceID,
						cmfArInvoiceID = item2.cmfArInvoiceID,
						cmfAssetID = item2.cmfAssetID,
						cmfAssignedToEmployeeID = item2.cmfAssignedToEmployeeID,
						cmfAttachedToEmployeeID = item2.cmfAttachedToEmployeeID,
						cmfCallID = item2.cmfCallID,
						cmfChangeRequestID = item2.cmfChangeRequestID,
						cmfFollowupID = item2.cmfFollowupID,
						cmfCompletedDate = item2.cmfCompletedDate,
						cmfContactID = item2.cmfContactID,
						cmfCreatedBy = item2.cmfCreatedBy,
						cmfCreatedDate = item2.cmfCreatedDate,
						cmfDmrClaimID = item2.cmfDmrClaimID,
						cmfDueDate = item2.cmfDueDate,
						cmfUniqueID = item2.cmfUniqueID,
						cmfExchangeID = item2.cmfExchangeID,
						cmfFollowupType = item2.cmfFollowupType,
						cmfCreatedFromMobile = item2.cmfCreatedFromMobile,
						cmfJobID = item2.cmfJobID,
						cmfLeadID = item2.cmfLeadID,
						cmfLocationID = item2.cmfLocationID,
						cmfLongDescriptionRtf = item2.cmfLongDescriptionRtf,
						cmfLongDescriptionText = item2.cmfLongDescriptionText,
						cmfMeetingLocation = item2.cmfMeetingLocation,
						cmfOrganizationID = item2.cmfOrganizationID,
						cmfPriority = item2.cmfPriority,
						cmfProjectAreaID = item2.cmfProjectAreaID,
						cmfProjectID = item2.cmfProjectID,
						cmfPurchaseOrderID = item2.cmfPurchaseOrderID,
						cmfQuoteID = item2.cmfQuoteID,
						cmfReceiptID = item2.cmfReceiptID,
						cmfRfqID = item2.cmfRfqID,
						cmfRmaClaimID = item2.cmfRmaClaimID,
						cmfRowVersion = item2.cmfRowVersion,
						cmfSalesOrderID = item2.cmfSalesOrderID,
						cmfShipmentID = item2.cmfShipmentID,
						cmfShortDescription = item2.cmfShortDescription,
						cmfStartDate = item2.cmfStartDate,
						cmfStatus = item2.cmfStatus,
						CustomFields = item2.CustomFields
					};
					allFollowupsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Followups]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPFollowupDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allFollowupsDto,
				RecordCount = allFollowupsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFollowupDto>> Process_GetFollowup(Guid followupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPFollowupDto followupDto = null;
		ERPResponseMessageDto<ERPFollowupDto> result;
		try
		{
			IERPFollowupRepository iERPFollowupRepository = (base.ERPFollowupRepository = new ERPFollowupRepository(base.ApiClientContext));
			using (iERPFollowupRepository)
			{
				ERPFollowupInformationDto eRPFollowupInformationDto = await base.ERPFollowupRepository.GetFollowup(followupId);
				followupDto = new ERPFollowupDto
				{
					cmfApInvoiceID = eRPFollowupInformationDto.cmfApInvoiceID,
					cmfArInvoiceID = eRPFollowupInformationDto.cmfArInvoiceID,
					cmfAssetID = eRPFollowupInformationDto.cmfAssetID,
					cmfAssignedToEmployeeID = eRPFollowupInformationDto.cmfAssignedToEmployeeID,
					cmfAttachedToEmployeeID = eRPFollowupInformationDto.cmfAttachedToEmployeeID,
					cmfCallID = eRPFollowupInformationDto.cmfCallID,
					cmfChangeRequestID = eRPFollowupInformationDto.cmfChangeRequestID,
					cmfFollowupID = eRPFollowupInformationDto.cmfFollowupID,
					cmfCompletedDate = eRPFollowupInformationDto.cmfCompletedDate,
					cmfContactID = eRPFollowupInformationDto.cmfContactID,
					cmfCreatedBy = eRPFollowupInformationDto.cmfCreatedBy,
					cmfCreatedDate = eRPFollowupInformationDto.cmfCreatedDate,
					cmfDmrClaimID = eRPFollowupInformationDto.cmfDmrClaimID,
					cmfDueDate = eRPFollowupInformationDto.cmfDueDate,
					cmfUniqueID = eRPFollowupInformationDto.cmfUniqueID,
					cmfExchangeID = eRPFollowupInformationDto.cmfExchangeID,
					cmfFollowupType = eRPFollowupInformationDto.cmfFollowupType,
					cmfCreatedFromMobile = eRPFollowupInformationDto.cmfCreatedFromMobile,
					cmfJobID = eRPFollowupInformationDto.cmfJobID,
					cmfLeadID = eRPFollowupInformationDto.cmfLeadID,
					cmfLocationID = eRPFollowupInformationDto.cmfLocationID,
					cmfLongDescriptionRtf = eRPFollowupInformationDto.cmfLongDescriptionRtf,
					cmfLongDescriptionText = eRPFollowupInformationDto.cmfLongDescriptionText,
					cmfMeetingLocation = eRPFollowupInformationDto.cmfMeetingLocation,
					cmfOrganizationID = eRPFollowupInformationDto.cmfOrganizationID,
					cmfPriority = eRPFollowupInformationDto.cmfPriority,
					cmfProjectAreaID = eRPFollowupInformationDto.cmfProjectAreaID,
					cmfProjectID = eRPFollowupInformationDto.cmfProjectID,
					cmfPurchaseOrderID = eRPFollowupInformationDto.cmfPurchaseOrderID,
					cmfQuoteID = eRPFollowupInformationDto.cmfQuoteID,
					cmfReceiptID = eRPFollowupInformationDto.cmfReceiptID,
					cmfRfqID = eRPFollowupInformationDto.cmfRfqID,
					cmfRmaClaimID = eRPFollowupInformationDto.cmfRmaClaimID,
					cmfRowVersion = eRPFollowupInformationDto.cmfRowVersion,
					cmfSalesOrderID = eRPFollowupInformationDto.cmfSalesOrderID,
					cmfShipmentID = eRPFollowupInformationDto.cmfShipmentID,
					cmfShortDescription = eRPFollowupInformationDto.cmfShortDescription,
					cmfStartDate = eRPFollowupInformationDto.cmfStartDate,
					cmfStatus = eRPFollowupInformationDto.cmfStatus,
					CustomFields = eRPFollowupInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Followups []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFollowupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = followupDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFollowupDto>> Process_PutFollowup(ERPFollowupDto followup)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPFollowupDto createdObject = null;
		ERPResponseMessageDto<ERPFollowupDto> result;
		try
		{
			IERPFollowupRepository iERPFollowupRepository = (base.ERPFollowupRepository = new ERPFollowupRepository(base.ApiClientContext));
			using (iERPFollowupRepository)
			{
				APIValidationInfoDto postResult = await base.ERPFollowupRepository.SaveFollowup(followup);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPFollowupInformationDto eRPFollowupInformationDto = await base.ERPFollowupRepository.GetFollowup(followup.cmfUniqueID);
					createdObject = new ERPFollowupDto
					{
						cmfApInvoiceID = eRPFollowupInformationDto.cmfApInvoiceID,
						cmfArInvoiceID = eRPFollowupInformationDto.cmfArInvoiceID,
						cmfAssetID = eRPFollowupInformationDto.cmfAssetID,
						cmfAssignedToEmployeeID = eRPFollowupInformationDto.cmfAssignedToEmployeeID,
						cmfAttachedToEmployeeID = eRPFollowupInformationDto.cmfAttachedToEmployeeID,
						cmfCallID = eRPFollowupInformationDto.cmfCallID,
						cmfChangeRequestID = eRPFollowupInformationDto.cmfChangeRequestID,
						cmfFollowupID = eRPFollowupInformationDto.cmfFollowupID,
						cmfCompletedDate = eRPFollowupInformationDto.cmfCompletedDate,
						cmfContactID = eRPFollowupInformationDto.cmfContactID,
						cmfCreatedBy = eRPFollowupInformationDto.cmfCreatedBy,
						cmfCreatedDate = eRPFollowupInformationDto.cmfCreatedDate,
						cmfDmrClaimID = eRPFollowupInformationDto.cmfDmrClaimID,
						cmfDueDate = eRPFollowupInformationDto.cmfDueDate,
						cmfUniqueID = eRPFollowupInformationDto.cmfUniqueID,
						cmfExchangeID = eRPFollowupInformationDto.cmfExchangeID,
						cmfFollowupType = eRPFollowupInformationDto.cmfFollowupType,
						cmfCreatedFromMobile = eRPFollowupInformationDto.cmfCreatedFromMobile,
						cmfJobID = eRPFollowupInformationDto.cmfJobID,
						cmfLeadID = eRPFollowupInformationDto.cmfLeadID,
						cmfLocationID = eRPFollowupInformationDto.cmfLocationID,
						cmfLongDescriptionRtf = eRPFollowupInformationDto.cmfLongDescriptionRtf,
						cmfLongDescriptionText = eRPFollowupInformationDto.cmfLongDescriptionText,
						cmfMeetingLocation = eRPFollowupInformationDto.cmfMeetingLocation,
						cmfOrganizationID = eRPFollowupInformationDto.cmfOrganizationID,
						cmfPriority = eRPFollowupInformationDto.cmfPriority,
						cmfProjectAreaID = eRPFollowupInformationDto.cmfProjectAreaID,
						cmfProjectID = eRPFollowupInformationDto.cmfProjectID,
						cmfPurchaseOrderID = eRPFollowupInformationDto.cmfPurchaseOrderID,
						cmfQuoteID = eRPFollowupInformationDto.cmfQuoteID,
						cmfReceiptID = eRPFollowupInformationDto.cmfReceiptID,
						cmfRfqID = eRPFollowupInformationDto.cmfRfqID,
						cmfRmaClaimID = eRPFollowupInformationDto.cmfRmaClaimID,
						cmfRowVersion = eRPFollowupInformationDto.cmfRowVersion,
						cmfSalesOrderID = eRPFollowupInformationDto.cmfSalesOrderID,
						cmfShipmentID = eRPFollowupInformationDto.cmfShipmentID,
						cmfShortDescription = eRPFollowupInformationDto.cmfShortDescription,
						cmfStartDate = eRPFollowupInformationDto.cmfStartDate,
						cmfStatus = eRPFollowupInformationDto.cmfStatus,
						CustomFields = eRPFollowupInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Followup [{followup.cmfUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFollowupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteFollowup(Guid followupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFollowupRepository iERPFollowupRepository = (base.ERPFollowupRepository = new ERPFollowupRepository(base.ApiClientContext));
		using (iERPFollowupRepository)
		{
			if (!(await base.ERPFollowupRepository.DoesFollowupExist(followupId)))
			{
				base.ErrorsList.Add($"Followup [{followupId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPFollowupInformationDto eRPFollowupInformationDto = await base.ERPFollowupRepository.GetFollowup(followupId);
				string text = await base.ERPFollowupRepository.WhereUsed("Followups", new object[1] { eRPFollowupInformationDto.cmfFollowupID }, new object[1] { "cmfFollowupID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Followup cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPFollowupDto>> Process_DeleteFollowup(Guid followupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPFollowupDto> result;
		try
		{
			IERPFollowupRepository iERPFollowupRepository = (base.ERPFollowupRepository = new ERPFollowupRepository(base.ApiClientContext));
			using (iERPFollowupRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPFollowupRepository.DeleteRowFromTable("Followups", "cmf", followupId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Followup [{followupId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFollowupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPFollowupDto()
			};
		}
		return result;
	}
}
