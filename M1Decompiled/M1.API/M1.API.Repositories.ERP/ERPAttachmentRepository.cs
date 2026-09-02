using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPAttachmentRepository : APIBaseRepository, IERPAttachmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPAttachmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAttachmentExist(Guid attachmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmaUniqueID|C", attachmentId);
		base.selectList.Add("cmaUniqueID");
		return Task.FromResult(GetAsObject("Attachments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAttachmentInformationDto>> GetAllAttachments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAttachmentInformationDto> collection = new List<ERPAttachmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[43]
		{
			"cmaApInvoiceID", "cmaArInvoiceID", "cmaAttachmentTypeID", "cmaCallID", "cmaChangeRequestID", "cmaAttachmentID", "cmaContactID", "cmaCreatedBy", "cmaCreatedDate", "cmaCustomerGroupID",
			"cmaDate", "cmaDmrClaimID", "cmaUniqueID", "cmaFileLocation", "cmaFilename", "cmaInspectionID", "cmaInspectionLineID", "cmaDoNotAllowDownload", "cmaEmailDefault", "cmaPrintDefault",
			"cmaReviewed", "cmaJobID", "cmaKnowledgeBasePageID", "cmaLeadID", "cmaLocationID", "cmaLongDescriptionRtf", "cmaLongDescriptionText", "cmaNonConformanceID", "cmaOrganizationID", "cmaPartID",
			"cmaProjectAreaID", "cmaProjectID", "cmaPurchaseOrderID", "cmaQuoteID", "cmaReceiptID", "cmaRfqID", "cmaRmaClaimID", "cmaRowVersion", "cmaSalesOrderID", "cmaShipmentID",
			"cmaShortDescription", "cmaWorkFlowID", "cmaWorkFlowLineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Attachments");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("Attachments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAttachmentInformationDto eRPAttachmentInformationDto = new ERPAttachmentInformationDto();
				eRPAttachmentInformationDto.cmaApInvoiceID = dataTable.Rows[i].Field<string>("cmaApInvoiceID");
				eRPAttachmentInformationDto.cmaArInvoiceID = dataTable.Rows[i].Field<string>("cmaArInvoiceID");
				eRPAttachmentInformationDto.cmaAttachmentTypeID = dataTable.Rows[i].Field<string>("cmaAttachmentTypeID");
				eRPAttachmentInformationDto.cmaCallID = dataTable.Rows[i].Field<string>("cmaCallID");
				eRPAttachmentInformationDto.cmaChangeRequestID = dataTable.Rows[i].Field<string>("cmaChangeRequestID");
				eRPAttachmentInformationDto.cmaAttachmentID = dataTable.Rows[i].Field<string>("cmaAttachmentID");
				eRPAttachmentInformationDto.cmaContactID = dataTable.Rows[i].Field<string>("cmaContactID");
				eRPAttachmentInformationDto.cmaCreatedBy = dataTable.Rows[i].Field<string>("cmaCreatedBy");
				eRPAttachmentInformationDto.cmaCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmaCreatedDate");
				eRPAttachmentInformationDto.cmaCustomerGroupID = dataTable.Rows[i].Field<string>("cmaCustomerGroupID");
				eRPAttachmentInformationDto.cmaDate = dataTable.Rows[i].Field<DateTime?>("cmaDate");
				eRPAttachmentInformationDto.cmaDmrClaimID = dataTable.Rows[i].Field<string>("cmaDmrClaimID");
				eRPAttachmentInformationDto.cmaUniqueID = dataTable.Rows[i].Field<Guid>("cmaUniqueID");
				eRPAttachmentInformationDto.cmaFileLocation = dataTable.Rows[i].Field<string>("cmaFileLocation");
				eRPAttachmentInformationDto.cmaFilename = dataTable.Rows[i].Field<string>("cmaFilename");
				eRPAttachmentInformationDto.cmaInspectionID = dataTable.Rows[i].Field<string>("cmaInspectionID");
				eRPAttachmentInformationDto.cmaInspectionLineID = dataTable.Rows[i].Field<short>("cmaInspectionLineID");
				eRPAttachmentInformationDto.cmaDoNotAllowDownload = dataTable.Rows[i].Field<bool>("cmaDoNotAllowDownload");
				eRPAttachmentInformationDto.cmaEmailDefault = dataTable.Rows[i].Field<bool>("cmaEmailDefault");
				eRPAttachmentInformationDto.cmaPrintDefault = dataTable.Rows[i].Field<bool>("cmaPrintDefault");
				eRPAttachmentInformationDto.cmaReviewed = dataTable.Rows[i].Field<bool>("cmaReviewed");
				eRPAttachmentInformationDto.cmaJobID = dataTable.Rows[i].Field<string>("cmaJobID");
				eRPAttachmentInformationDto.cmaKnowledgeBasePageID = dataTable.Rows[i].Field<string>("cmaKnowledgeBasePageID");
				eRPAttachmentInformationDto.cmaLeadID = dataTable.Rows[i].Field<string>("cmaLeadID");
				eRPAttachmentInformationDto.cmaLocationID = dataTable.Rows[i].Field<string>("cmaLocationID");
				eRPAttachmentInformationDto.cmaLongDescriptionRtf = dataTable.Rows[i].Field<string>("cmaLongDescriptionRtf");
				eRPAttachmentInformationDto.cmaLongDescriptionText = dataTable.Rows[i].Field<string>("cmaLongDescriptionText");
				eRPAttachmentInformationDto.cmaNonConformanceID = dataTable.Rows[i].Field<string>("cmaNonConformanceID");
				eRPAttachmentInformationDto.cmaOrganizationID = dataTable.Rows[i].Field<string>("cmaOrganizationID");
				eRPAttachmentInformationDto.cmaPartID = dataTable.Rows[i].Field<string>("cmaPartID");
				eRPAttachmentInformationDto.cmaProjectAreaID = dataTable.Rows[i].Field<string>("cmaProjectAreaID");
				eRPAttachmentInformationDto.cmaProjectID = dataTable.Rows[i].Field<string>("cmaProjectID");
				eRPAttachmentInformationDto.cmaPurchaseOrderID = dataTable.Rows[i].Field<string>("cmaPurchaseOrderID");
				eRPAttachmentInformationDto.cmaQuoteID = dataTable.Rows[i].Field<string>("cmaQuoteID");
				eRPAttachmentInformationDto.cmaReceiptID = dataTable.Rows[i].Field<string>("cmaReceiptID");
				eRPAttachmentInformationDto.cmaRfqID = dataTable.Rows[i].Field<string>("cmaRfqID");
				eRPAttachmentInformationDto.cmaRmaClaimID = dataTable.Rows[i].Field<string>("cmaRmaClaimID");
				eRPAttachmentInformationDto.cmaRowVersion = dataTable.Rows[i].Field<byte[]>("cmaRowVersion");
				eRPAttachmentInformationDto.cmaSalesOrderID = dataTable.Rows[i].Field<string>("cmaSalesOrderID");
				eRPAttachmentInformationDto.cmaShipmentID = dataTable.Rows[i].Field<string>("cmaShipmentID");
				eRPAttachmentInformationDto.cmaShortDescription = dataTable.Rows[i].Field<string>("cmaShortDescription");
				eRPAttachmentInformationDto.cmaWorkFlowID = dataTable.Rows[i].Field<string>("cmaWorkFlowID");
				eRPAttachmentInformationDto.cmaWorkFlowLineID = dataTable.Rows[i].Field<short>("cmaWorkFlowLineID");
				eRPAttachmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAttachmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAttachmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAttachmentInformationDto> GetAttachment(Guid attachmentId)
	{
		ERPAttachmentInformationDto eRPAttachmentInformationDto = new ERPAttachmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[43]
		{
			"cmaApInvoiceID", "cmaArInvoiceID", "cmaAttachmentTypeID", "cmaCallID", "cmaChangeRequestID", "cmaAttachmentID", "cmaContactID", "cmaCreatedBy", "cmaCreatedDate", "cmaCustomerGroupID",
			"cmaDate", "cmaDmrClaimID", "cmaUniqueID", "cmaFileLocation", "cmaFilename", "cmaInspectionID", "cmaInspectionLineID", "cmaDoNotAllowDownload", "cmaEmailDefault", "cmaPrintDefault",
			"cmaReviewed", "cmaJobID", "cmaKnowledgeBasePageID", "cmaLeadID", "cmaLocationID", "cmaLongDescriptionRtf", "cmaLongDescriptionText", "cmaNonConformanceID", "cmaOrganizationID", "cmaPartID",
			"cmaProjectAreaID", "cmaProjectID", "cmaPurchaseOrderID", "cmaQuoteID", "cmaReceiptID", "cmaRfqID", "cmaRmaClaimID", "cmaRowVersion", "cmaSalesOrderID", "cmaShipmentID",
			"cmaShortDescription", "cmaWorkFlowID", "cmaWorkFlowLineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("cmaUniqueID|C", attachmentId);
		AddCustomFieldsToSelectList("Attachments");
		using (DataTable dataTable = GetAsDataTable("Attachments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAttachmentInformationDto);
			}
			eRPAttachmentInformationDto.cmaApInvoiceID = dataTable.Rows[0].Field<string>("cmaApInvoiceID");
			eRPAttachmentInformationDto.cmaArInvoiceID = dataTable.Rows[0].Field<string>("cmaArInvoiceID");
			eRPAttachmentInformationDto.cmaAttachmentTypeID = dataTable.Rows[0].Field<string>("cmaAttachmentTypeID");
			eRPAttachmentInformationDto.cmaCallID = dataTable.Rows[0].Field<string>("cmaCallID");
			eRPAttachmentInformationDto.cmaChangeRequestID = dataTable.Rows[0].Field<string>("cmaChangeRequestID");
			eRPAttachmentInformationDto.cmaAttachmentID = dataTable.Rows[0].Field<string>("cmaAttachmentID");
			eRPAttachmentInformationDto.cmaContactID = dataTable.Rows[0].Field<string>("cmaContactID");
			eRPAttachmentInformationDto.cmaCreatedBy = dataTable.Rows[0].Field<string>("cmaCreatedBy");
			eRPAttachmentInformationDto.cmaCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmaCreatedDate");
			eRPAttachmentInformationDto.cmaCustomerGroupID = dataTable.Rows[0].Field<string>("cmaCustomerGroupID");
			eRPAttachmentInformationDto.cmaDate = dataTable.Rows[0].Field<DateTime?>("cmaDate");
			eRPAttachmentInformationDto.cmaDmrClaimID = dataTable.Rows[0].Field<string>("cmaDmrClaimID");
			eRPAttachmentInformationDto.cmaUniqueID = dataTable.Rows[0].Field<Guid>("cmaUniqueID");
			eRPAttachmentInformationDto.cmaFileLocation = dataTable.Rows[0].Field<string>("cmaFileLocation");
			eRPAttachmentInformationDto.cmaFilename = dataTable.Rows[0].Field<string>("cmaFilename");
			eRPAttachmentInformationDto.cmaInspectionID = dataTable.Rows[0].Field<string>("cmaInspectionID");
			eRPAttachmentInformationDto.cmaInspectionLineID = dataTable.Rows[0].Field<short>("cmaInspectionLineID");
			eRPAttachmentInformationDto.cmaDoNotAllowDownload = dataTable.Rows[0].Field<bool>("cmaDoNotAllowDownload");
			eRPAttachmentInformationDto.cmaEmailDefault = dataTable.Rows[0].Field<bool>("cmaEmailDefault");
			eRPAttachmentInformationDto.cmaPrintDefault = dataTable.Rows[0].Field<bool>("cmaPrintDefault");
			eRPAttachmentInformationDto.cmaReviewed = dataTable.Rows[0].Field<bool>("cmaReviewed");
			eRPAttachmentInformationDto.cmaJobID = dataTable.Rows[0].Field<string>("cmaJobID");
			eRPAttachmentInformationDto.cmaKnowledgeBasePageID = dataTable.Rows[0].Field<string>("cmaKnowledgeBasePageID");
			eRPAttachmentInformationDto.cmaLeadID = dataTable.Rows[0].Field<string>("cmaLeadID");
			eRPAttachmentInformationDto.cmaLocationID = dataTable.Rows[0].Field<string>("cmaLocationID");
			eRPAttachmentInformationDto.cmaLongDescriptionRtf = dataTable.Rows[0].Field<string>("cmaLongDescriptionRtf");
			eRPAttachmentInformationDto.cmaLongDescriptionText = dataTable.Rows[0].Field<string>("cmaLongDescriptionText");
			eRPAttachmentInformationDto.cmaNonConformanceID = dataTable.Rows[0].Field<string>("cmaNonConformanceID");
			eRPAttachmentInformationDto.cmaOrganizationID = dataTable.Rows[0].Field<string>("cmaOrganizationID");
			eRPAttachmentInformationDto.cmaPartID = dataTable.Rows[0].Field<string>("cmaPartID");
			eRPAttachmentInformationDto.cmaProjectAreaID = dataTable.Rows[0].Field<string>("cmaProjectAreaID");
			eRPAttachmentInformationDto.cmaProjectID = dataTable.Rows[0].Field<string>("cmaProjectID");
			eRPAttachmentInformationDto.cmaPurchaseOrderID = dataTable.Rows[0].Field<string>("cmaPurchaseOrderID");
			eRPAttachmentInformationDto.cmaQuoteID = dataTable.Rows[0].Field<string>("cmaQuoteID");
			eRPAttachmentInformationDto.cmaReceiptID = dataTable.Rows[0].Field<string>("cmaReceiptID");
			eRPAttachmentInformationDto.cmaRfqID = dataTable.Rows[0].Field<string>("cmaRfqID");
			eRPAttachmentInformationDto.cmaRmaClaimID = dataTable.Rows[0].Field<string>("cmaRmaClaimID");
			eRPAttachmentInformationDto.cmaRowVersion = dataTable.Rows[0].Field<byte[]>("cmaRowVersion");
			eRPAttachmentInformationDto.cmaSalesOrderID = dataTable.Rows[0].Field<string>("cmaSalesOrderID");
			eRPAttachmentInformationDto.cmaShipmentID = dataTable.Rows[0].Field<string>("cmaShipmentID");
			eRPAttachmentInformationDto.cmaShortDescription = dataTable.Rows[0].Field<string>("cmaShortDescription");
			eRPAttachmentInformationDto.cmaWorkFlowID = dataTable.Rows[0].Field<string>("cmaWorkFlowID");
			eRPAttachmentInformationDto.cmaWorkFlowLineID = dataTable.Rows[0].Field<short>("cmaWorkFlowLineID");
			eRPAttachmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAttachmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAttachmentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAttachment(ERPAttachmentDto attachment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Attachments WHERE cmaUniqueID = " + M1Util.ConvertToLinq(attachment.cmaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmaAttachmentID"] = attachment.cmaAttachmentID.ToUpper();
				attachment.cmaUniqueID = ((attachment.cmaUniqueID == Guid.Empty) ? Guid.NewGuid() : attachment.cmaUniqueID);
				dataRow["cmaUniqueID"] = attachment.cmaUniqueID;
				dataRow["cmaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Attachment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (attachment.cmaRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Attachment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmaRowVersion"], attachment.cmaRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Attachment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Attachment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmaApInvoiceID"] = attachment.cmaApInvoiceID;
			dataRow["cmaArInvoiceID"] = attachment.cmaArInvoiceID;
			dataRow["cmaAttachmentTypeID"] = attachment.cmaAttachmentTypeID;
			dataRow["cmaCallID"] = attachment.cmaCallID;
			dataRow["cmaChangeRequestID"] = attachment.cmaChangeRequestID;
			dataRow["cmaContactID"] = attachment.cmaContactID;
			dataRow["cmaCustomerGroupID"] = attachment.cmaCustomerGroupID;
			DataRow dataRow2 = dataRow;
			DateTime? cmaDate = attachment.cmaDate;
			dataRow2["cmaDate"] = (cmaDate.HasValue ? ((object)cmaDate.GetValueOrDefault()) : dataRow["cmaDate"]);
			dataRow["cmaDmrClaimID"] = attachment.cmaDmrClaimID;
			dataRow["cmaFileLocation"] = attachment.cmaFileLocation;
			dataRow["cmaFilename"] = attachment.cmaFilename;
			dataRow["cmaInspectionID"] = attachment.cmaInspectionID;
			dataRow["cmaInspectionLineID"] = attachment.cmaInspectionLineID;
			dataRow["cmaDoNotAllowDownload"] = attachment.cmaDoNotAllowDownload;
			dataRow["cmaEmailDefault"] = attachment.cmaEmailDefault;
			dataRow["cmaPrintDefault"] = attachment.cmaPrintDefault;
			dataRow["cmaReviewed"] = attachment.cmaReviewed;
			dataRow["cmaJobID"] = attachment.cmaJobID;
			dataRow["cmaKnowledgeBasePageID"] = attachment.cmaKnowledgeBasePageID;
			dataRow["cmaLeadID"] = attachment.cmaLeadID;
			dataRow["cmaLocationID"] = attachment.cmaLocationID;
			dataRow["cmaLongDescriptionRtf"] = attachment.cmaLongDescriptionRtf ?? dataRow["cmaLongDescriptionRtf"];
			dataRow["cmaLongDescriptionText"] = attachment.cmaLongDescriptionText ?? dataRow["cmaLongDescriptionText"];
			dataRow["cmaNonConformanceID"] = attachment.cmaNonConformanceID;
			dataRow["cmaOrganizationID"] = attachment.cmaOrganizationID;
			dataRow["cmaPartID"] = attachment.cmaPartID;
			dataRow["cmaProjectAreaID"] = attachment.cmaProjectAreaID;
			dataRow["cmaProjectID"] = attachment.cmaProjectID;
			dataRow["cmaPurchaseOrderID"] = attachment.cmaPurchaseOrderID;
			dataRow["cmaQuoteID"] = attachment.cmaQuoteID;
			dataRow["cmaReceiptID"] = attachment.cmaReceiptID;
			dataRow["cmaRfqID"] = attachment.cmaRfqID;
			dataRow["cmaRmaClaimID"] = attachment.cmaRmaClaimID;
			dataRow["cmaSalesOrderID"] = attachment.cmaSalesOrderID;
			dataRow["cmaShipmentID"] = attachment.cmaShipmentID;
			dataRow["cmaShortDescription"] = attachment.cmaShortDescription;
			dataRow["cmaWorkFlowID"] = attachment.cmaWorkFlowID;
			dataRow["cmaWorkFlowLineID"] = attachment.cmaWorkFlowLineID;
			if (attachment.CustomFields != null && attachment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in attachment.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Attachment [{attachment.cmaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Attachment [{attachment.cmaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
