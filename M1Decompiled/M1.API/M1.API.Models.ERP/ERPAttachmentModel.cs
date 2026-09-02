using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAttachmentModel : ERPBaseModel, IERPAttachmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAttachments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAttachmentRepository iERPAttachmentRepository = (base.ERPAttachmentRepository = new ERPAttachmentRepository(base.ApiClientContext));
		using (iERPAttachmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAttachmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAttachmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAttachmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAttachmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAttachment(Guid attachmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAttachmentRepository iERPAttachmentRepository = (base.ERPAttachmentRepository = new ERPAttachmentRepository(base.ApiClientContext));
		using (iERPAttachmentRepository)
		{
			if (!(await base.ERPAttachmentRepository.DoesAttachmentExist(attachmentId)))
			{
				errorsList.Add($"Attachment [{attachmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAttachment(ERPAttachmentDto attachment)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAttachmentRepository iERPAttachmentRepository = (base.ERPAttachmentRepository = new ERPAttachmentRepository(base.ApiClientContext));
		using (iERPAttachmentRepository)
		{
			if (!string.IsNullOrWhiteSpace(attachment.cmaOrganizationID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { attachment.cmaOrganizationID })))
			{
				errorsList.Add("cmaOrganizationID [" + attachment.cmaOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaLocationID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { attachment.cmaOrganizationID, attachment.cmaLocationID })))
			{
				errorsList.Add("cmaLocationID [" + attachment.cmaLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaContactID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { attachment.cmaOrganizationID, attachment.cmaLocationID, attachment.cmaContactID })))
			{
				errorsList.Add("cmaContactID [" + attachment.cmaContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaAttachmentTypeID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("AttachmentTypes", new object[1] { "CMTATTACHMENTTYPEID" }, new object[1] { attachment.cmaAttachmentTypeID })))
			{
				errorsList.Add("cmaAttachmentTypeID [" + attachment.cmaAttachmentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaCallID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Calls", new object[1] { "KBPCALLID" }, new object[1] { attachment.cmaCallID })))
			{
				errorsList.Add("cmaCallID [" + attachment.cmaCallID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaLeadID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Leads", new object[1] { "LOPLEADID" }, new object[1] { attachment.cmaLeadID })))
			{
				errorsList.Add("cmaLeadID [" + attachment.cmaLeadID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaQuoteID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { attachment.cmaQuoteID })))
			{
				errorsList.Add("cmaQuoteID [" + attachment.cmaQuoteID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaSalesOrderID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { attachment.cmaSalesOrderID })))
			{
				errorsList.Add("cmaSalesOrderID [" + attachment.cmaSalesOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaShipmentID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { attachment.cmaShipmentID })))
			{
				errorsList.Add("cmaShipmentID [" + attachment.cmaShipmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaArInvoiceID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { attachment.cmaArInvoiceID })))
			{
				errorsList.Add("cmaArInvoiceID [" + attachment.cmaArInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaPurchaseOrderID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { attachment.cmaPurchaseOrderID })))
			{
				errorsList.Add("cmaPurchaseOrderID [" + attachment.cmaPurchaseOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaReceiptID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { attachment.cmaReceiptID })))
			{
				errorsList.Add("cmaReceiptID [" + attachment.cmaReceiptID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaApInvoiceID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { attachment.cmaApInvoiceID })))
			{
				errorsList.Add("cmaApInvoiceID [" + attachment.cmaApInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaProjectID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { attachment.cmaProjectID })))
			{
				errorsList.Add("cmaProjectID [" + attachment.cmaProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaDmrClaimID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { attachment.cmaDmrClaimID })))
			{
				errorsList.Add("cmaDmrClaimID [" + attachment.cmaDmrClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaRmaClaimID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { attachment.cmaRmaClaimID })))
			{
				errorsList.Add("cmaRmaClaimID [" + attachment.cmaRmaClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaChangeRequestID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("ChangeRequests", new object[1] { "CHPCHANGEREQUESTID" }, new object[1] { attachment.cmaChangeRequestID })))
			{
				errorsList.Add("cmaChangeRequestID [" + attachment.cmaChangeRequestID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaKnowledgeBasePageID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("KnowledgeBasePages", new object[1] { "KBBKNOWLEDGEBASEPAGEID" }, new object[1] { attachment.cmaKnowledgeBasePageID })))
			{
				errorsList.Add("cmaKnowledgeBasePageID [" + attachment.cmaKnowledgeBasePageID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaJobID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { attachment.cmaJobID })))
			{
				errorsList.Add("cmaJobID [" + attachment.cmaJobID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaProjectAreaID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { attachment.cmaProjectID, attachment.cmaProjectAreaID })))
			{
				errorsList.Add("cmaProjectAreaID [" + attachment.cmaProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaPartID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { attachment.cmaPartID })))
			{
				errorsList.Add("cmaPartID [" + attachment.cmaPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaCustomerGroupID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("CustomerGroups", new object[1] { "CMUCUSTOMERGROUPID" }, new object[1] { attachment.cmaCustomerGroupID })))
			{
				errorsList.Add("cmaCustomerGroupID [" + attachment.cmaCustomerGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaNonConformanceID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("NonConformances", new object[1] { "QARNONCONFORMANCEID" }, new object[1] { attachment.cmaNonConformanceID })))
			{
				errorsList.Add("cmaNonConformanceID [" + attachment.cmaNonConformanceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaInspectionID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { attachment.cmaInspectionID })))
			{
				errorsList.Add("cmaInspectionID [" + attachment.cmaInspectionID + "] not found.");
			}
			if (attachment.cmaInspectionLineID > 0 && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { attachment.cmaInspectionID, attachment.cmaInspectionLineID })))
			{
				errorsList.Add($"cmaInspectionLineID [{attachment.cmaInspectionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaRfqID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { attachment.cmaRfqID })))
			{
				errorsList.Add("cmaRfqID [" + attachment.cmaRfqID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(attachment.cmaWorkFlowID) && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("WorkFlows", new object[1] { "WFPWORKFLOWID" }, new object[1] { attachment.cmaWorkFlowID })))
			{
				errorsList.Add("cmaWorkFlowID [" + attachment.cmaWorkFlowID + "] not found.");
			}
			if (attachment.cmaWorkFlowLineID > 0 && !(await base.ERPAttachmentRepository.DoesRecordExistInTableUsingKeys("WorkFlowLines", new object[2] { "WFLWORKFLOWID", "WFLWORKFLOWLINEID" }, new object[2] { attachment.cmaWorkFlowID, attachment.cmaWorkFlowLineID })))
			{
				errorsList.Add($"cmaWorkFlowLineID [{attachment.cmaWorkFlowLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAttachmentDto>>> Process_GetAllAttachments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAttachmentDto> allAttachmentsDto = new List<ERPAttachmentDto>();
		ERPResponseMessageDto<IList<ERPAttachmentDto>> result;
		try
		{
			IERPAttachmentRepository iERPAttachmentRepository = (base.ERPAttachmentRepository = new ERPAttachmentRepository(base.ApiClientContext));
			using (iERPAttachmentRepository)
			{
				foreach (ERPAttachmentInformationDto item2 in await base.ERPAttachmentRepository.GetAllAttachments(pageSize, pageNumber, filter, orderBy))
				{
					ERPAttachmentDto item = new ERPAttachmentDto
					{
						cmaApInvoiceID = item2.cmaApInvoiceID,
						cmaArInvoiceID = item2.cmaArInvoiceID,
						cmaAttachmentTypeID = item2.cmaAttachmentTypeID,
						cmaCallID = item2.cmaCallID,
						cmaChangeRequestID = item2.cmaChangeRequestID,
						cmaAttachmentID = item2.cmaAttachmentID,
						cmaContactID = item2.cmaContactID,
						cmaCreatedBy = item2.cmaCreatedBy,
						cmaCreatedDate = item2.cmaCreatedDate,
						cmaCustomerGroupID = item2.cmaCustomerGroupID,
						cmaDate = item2.cmaDate,
						cmaDmrClaimID = item2.cmaDmrClaimID,
						cmaUniqueID = item2.cmaUniqueID,
						cmaFileLocation = item2.cmaFileLocation,
						cmaFilename = item2.cmaFilename,
						cmaInspectionID = item2.cmaInspectionID,
						cmaInspectionLineID = item2.cmaInspectionLineID,
						cmaDoNotAllowDownload = item2.cmaDoNotAllowDownload,
						cmaEmailDefault = item2.cmaEmailDefault,
						cmaPrintDefault = item2.cmaPrintDefault,
						cmaReviewed = item2.cmaReviewed,
						cmaJobID = item2.cmaJobID,
						cmaKnowledgeBasePageID = item2.cmaKnowledgeBasePageID,
						cmaLeadID = item2.cmaLeadID,
						cmaLocationID = item2.cmaLocationID,
						cmaLongDescriptionRtf = item2.cmaLongDescriptionRtf,
						cmaLongDescriptionText = item2.cmaLongDescriptionText,
						cmaNonConformanceID = item2.cmaNonConformanceID,
						cmaOrganizationID = item2.cmaOrganizationID,
						cmaPartID = item2.cmaPartID,
						cmaProjectAreaID = item2.cmaProjectAreaID,
						cmaProjectID = item2.cmaProjectID,
						cmaPurchaseOrderID = item2.cmaPurchaseOrderID,
						cmaQuoteID = item2.cmaQuoteID,
						cmaReceiptID = item2.cmaReceiptID,
						cmaRfqID = item2.cmaRfqID,
						cmaRmaClaimID = item2.cmaRmaClaimID,
						cmaRowVersion = item2.cmaRowVersion,
						cmaSalesOrderID = item2.cmaSalesOrderID,
						cmaShipmentID = item2.cmaShipmentID,
						cmaShortDescription = item2.cmaShortDescription,
						cmaWorkFlowID = item2.cmaWorkFlowID,
						cmaWorkFlowLineID = item2.cmaWorkFlowLineID,
						CustomFields = item2.CustomFields
					};
					allAttachmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Attachments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAttachmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAttachmentsDto,
				RecordCount = allAttachmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAttachmentDto>> Process_GetAttachment(Guid attachmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAttachmentDto attachmentDto = null;
		ERPResponseMessageDto<ERPAttachmentDto> result;
		try
		{
			IERPAttachmentRepository iERPAttachmentRepository = (base.ERPAttachmentRepository = new ERPAttachmentRepository(base.ApiClientContext));
			using (iERPAttachmentRepository)
			{
				ERPAttachmentInformationDto eRPAttachmentInformationDto = await base.ERPAttachmentRepository.GetAttachment(attachmentId);
				attachmentDto = new ERPAttachmentDto
				{
					cmaApInvoiceID = eRPAttachmentInformationDto.cmaApInvoiceID,
					cmaArInvoiceID = eRPAttachmentInformationDto.cmaArInvoiceID,
					cmaAttachmentTypeID = eRPAttachmentInformationDto.cmaAttachmentTypeID,
					cmaCallID = eRPAttachmentInformationDto.cmaCallID,
					cmaChangeRequestID = eRPAttachmentInformationDto.cmaChangeRequestID,
					cmaAttachmentID = eRPAttachmentInformationDto.cmaAttachmentID,
					cmaContactID = eRPAttachmentInformationDto.cmaContactID,
					cmaCreatedBy = eRPAttachmentInformationDto.cmaCreatedBy,
					cmaCreatedDate = eRPAttachmentInformationDto.cmaCreatedDate,
					cmaCustomerGroupID = eRPAttachmentInformationDto.cmaCustomerGroupID,
					cmaDate = eRPAttachmentInformationDto.cmaDate,
					cmaDmrClaimID = eRPAttachmentInformationDto.cmaDmrClaimID,
					cmaUniqueID = eRPAttachmentInformationDto.cmaUniqueID,
					cmaFileLocation = eRPAttachmentInformationDto.cmaFileLocation,
					cmaFilename = eRPAttachmentInformationDto.cmaFilename,
					cmaInspectionID = eRPAttachmentInformationDto.cmaInspectionID,
					cmaInspectionLineID = eRPAttachmentInformationDto.cmaInspectionLineID,
					cmaDoNotAllowDownload = eRPAttachmentInformationDto.cmaDoNotAllowDownload,
					cmaEmailDefault = eRPAttachmentInformationDto.cmaEmailDefault,
					cmaPrintDefault = eRPAttachmentInformationDto.cmaPrintDefault,
					cmaReviewed = eRPAttachmentInformationDto.cmaReviewed,
					cmaJobID = eRPAttachmentInformationDto.cmaJobID,
					cmaKnowledgeBasePageID = eRPAttachmentInformationDto.cmaKnowledgeBasePageID,
					cmaLeadID = eRPAttachmentInformationDto.cmaLeadID,
					cmaLocationID = eRPAttachmentInformationDto.cmaLocationID,
					cmaLongDescriptionRtf = eRPAttachmentInformationDto.cmaLongDescriptionRtf,
					cmaLongDescriptionText = eRPAttachmentInformationDto.cmaLongDescriptionText,
					cmaNonConformanceID = eRPAttachmentInformationDto.cmaNonConformanceID,
					cmaOrganizationID = eRPAttachmentInformationDto.cmaOrganizationID,
					cmaPartID = eRPAttachmentInformationDto.cmaPartID,
					cmaProjectAreaID = eRPAttachmentInformationDto.cmaProjectAreaID,
					cmaProjectID = eRPAttachmentInformationDto.cmaProjectID,
					cmaPurchaseOrderID = eRPAttachmentInformationDto.cmaPurchaseOrderID,
					cmaQuoteID = eRPAttachmentInformationDto.cmaQuoteID,
					cmaReceiptID = eRPAttachmentInformationDto.cmaReceiptID,
					cmaRfqID = eRPAttachmentInformationDto.cmaRfqID,
					cmaRmaClaimID = eRPAttachmentInformationDto.cmaRmaClaimID,
					cmaRowVersion = eRPAttachmentInformationDto.cmaRowVersion,
					cmaSalesOrderID = eRPAttachmentInformationDto.cmaSalesOrderID,
					cmaShipmentID = eRPAttachmentInformationDto.cmaShipmentID,
					cmaShortDescription = eRPAttachmentInformationDto.cmaShortDescription,
					cmaWorkFlowID = eRPAttachmentInformationDto.cmaWorkFlowID,
					cmaWorkFlowLineID = eRPAttachmentInformationDto.cmaWorkFlowLineID,
					CustomFields = eRPAttachmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Attachments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAttachmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = attachmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAttachmentDto>> Process_PutAttachment(ERPAttachmentDto attachment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAttachmentDto createdObject = null;
		ERPResponseMessageDto<ERPAttachmentDto> result;
		try
		{
			IERPAttachmentRepository iERPAttachmentRepository = (base.ERPAttachmentRepository = new ERPAttachmentRepository(base.ApiClientContext));
			using (iERPAttachmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAttachmentRepository.SaveAttachment(attachment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAttachmentInformationDto eRPAttachmentInformationDto = await base.ERPAttachmentRepository.GetAttachment(attachment.cmaUniqueID);
					createdObject = new ERPAttachmentDto
					{
						cmaApInvoiceID = eRPAttachmentInformationDto.cmaApInvoiceID,
						cmaArInvoiceID = eRPAttachmentInformationDto.cmaArInvoiceID,
						cmaAttachmentTypeID = eRPAttachmentInformationDto.cmaAttachmentTypeID,
						cmaCallID = eRPAttachmentInformationDto.cmaCallID,
						cmaChangeRequestID = eRPAttachmentInformationDto.cmaChangeRequestID,
						cmaAttachmentID = eRPAttachmentInformationDto.cmaAttachmentID,
						cmaContactID = eRPAttachmentInformationDto.cmaContactID,
						cmaCreatedBy = eRPAttachmentInformationDto.cmaCreatedBy,
						cmaCreatedDate = eRPAttachmentInformationDto.cmaCreatedDate,
						cmaCustomerGroupID = eRPAttachmentInformationDto.cmaCustomerGroupID,
						cmaDate = eRPAttachmentInformationDto.cmaDate,
						cmaDmrClaimID = eRPAttachmentInformationDto.cmaDmrClaimID,
						cmaUniqueID = eRPAttachmentInformationDto.cmaUniqueID,
						cmaFileLocation = eRPAttachmentInformationDto.cmaFileLocation,
						cmaFilename = eRPAttachmentInformationDto.cmaFilename,
						cmaInspectionID = eRPAttachmentInformationDto.cmaInspectionID,
						cmaInspectionLineID = eRPAttachmentInformationDto.cmaInspectionLineID,
						cmaDoNotAllowDownload = eRPAttachmentInformationDto.cmaDoNotAllowDownload,
						cmaEmailDefault = eRPAttachmentInformationDto.cmaEmailDefault,
						cmaPrintDefault = eRPAttachmentInformationDto.cmaPrintDefault,
						cmaReviewed = eRPAttachmentInformationDto.cmaReviewed,
						cmaJobID = eRPAttachmentInformationDto.cmaJobID,
						cmaKnowledgeBasePageID = eRPAttachmentInformationDto.cmaKnowledgeBasePageID,
						cmaLeadID = eRPAttachmentInformationDto.cmaLeadID,
						cmaLocationID = eRPAttachmentInformationDto.cmaLocationID,
						cmaLongDescriptionRtf = eRPAttachmentInformationDto.cmaLongDescriptionRtf,
						cmaLongDescriptionText = eRPAttachmentInformationDto.cmaLongDescriptionText,
						cmaNonConformanceID = eRPAttachmentInformationDto.cmaNonConformanceID,
						cmaOrganizationID = eRPAttachmentInformationDto.cmaOrganizationID,
						cmaPartID = eRPAttachmentInformationDto.cmaPartID,
						cmaProjectAreaID = eRPAttachmentInformationDto.cmaProjectAreaID,
						cmaProjectID = eRPAttachmentInformationDto.cmaProjectID,
						cmaPurchaseOrderID = eRPAttachmentInformationDto.cmaPurchaseOrderID,
						cmaQuoteID = eRPAttachmentInformationDto.cmaQuoteID,
						cmaReceiptID = eRPAttachmentInformationDto.cmaReceiptID,
						cmaRfqID = eRPAttachmentInformationDto.cmaRfqID,
						cmaRmaClaimID = eRPAttachmentInformationDto.cmaRmaClaimID,
						cmaRowVersion = eRPAttachmentInformationDto.cmaRowVersion,
						cmaSalesOrderID = eRPAttachmentInformationDto.cmaSalesOrderID,
						cmaShipmentID = eRPAttachmentInformationDto.cmaShipmentID,
						cmaShortDescription = eRPAttachmentInformationDto.cmaShortDescription,
						cmaWorkFlowID = eRPAttachmentInformationDto.cmaWorkFlowID,
						cmaWorkFlowLineID = eRPAttachmentInformationDto.cmaWorkFlowLineID,
						CustomFields = eRPAttachmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Attachment [{attachment.cmaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAttachmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAttachment(Guid attachmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAttachmentRepository iERPAttachmentRepository = (base.ERPAttachmentRepository = new ERPAttachmentRepository(base.ApiClientContext));
		using (iERPAttachmentRepository)
		{
			if (!(await base.ERPAttachmentRepository.DoesAttachmentExist(attachmentId)))
			{
				base.ErrorsList.Add($"Attachment [{attachmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAttachmentInformationDto eRPAttachmentInformationDto = await base.ERPAttachmentRepository.GetAttachment(attachmentId);
				string text = await base.ERPAttachmentRepository.WhereUsed("Attachments", new object[1] { eRPAttachmentInformationDto.cmaAttachmentID }, new object[1] { "cmaAttachmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Attachment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAttachmentDto>> Process_DeleteAttachment(Guid attachmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAttachmentDto> result;
		try
		{
			IERPAttachmentRepository iERPAttachmentRepository = (base.ERPAttachmentRepository = new ERPAttachmentRepository(base.ApiClientContext));
			using (iERPAttachmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAttachmentRepository.DeleteRowFromTable("Attachments", "cma", attachmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Attachment [{attachmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAttachmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAttachmentDto()
			};
		}
		return result;
	}
}
