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

public class ERPFollowupRepository : APIBaseRepository, IERPFollowupRepository, IAPIBaseRepository, IDisposable
{
	public ERPFollowupRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesFollowupExist(Guid followupId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmfUniqueID|C", followupId);
		base.selectList.Add("cmfUniqueID");
		return Task.FromResult(GetAsObject("Followups", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPFollowupInformationDto>> GetAllFollowups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPFollowupInformationDto> collection = new List<ERPFollowupInformationDto>();
		InitializeParameterLists();
		string[] array = new string[39]
		{
			"cmfApInvoiceID", "cmfArInvoiceID", "cmfAssetID", "cmfAssignedToEmployeeID", "cmfAttachedToEmployeeID", "cmfCallID", "cmfChangeRequestID", "cmfFollowupID", "cmfCompletedDate", "cmfContactID",
			"cmfCreatedBy", "cmfCreatedDate", "cmfDmrClaimID", "cmfDueDate", "cmfUniqueID", "cmfExchangeID", "cmfFollowupType", "cmfCreatedFromMobile", "cmfJobID", "cmfLeadID",
			"cmfLocationID", "cmfLongDescriptionRtf", "cmfLongDescriptionText", "cmfMeetingLocation", "cmfOrganizationID", "cmfPriority", "cmfProjectAreaID", "cmfProjectID", "cmfPurchaseOrderID", "cmfQuoteID",
			"cmfReceiptID", "cmfRfqID", "cmfRmaClaimID", "cmfRowVersion", "cmfSalesOrderID", "cmfShipmentID", "cmfShortDescription", "cmfStartDate", "cmfStatus"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Followups");
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
		using (DataTable dataTable = GetAsDataTable("Followups", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPFollowupInformationDto eRPFollowupInformationDto = new ERPFollowupInformationDto();
				eRPFollowupInformationDto.cmfApInvoiceID = dataTable.Rows[i].Field<string>("cmfApInvoiceID");
				eRPFollowupInformationDto.cmfArInvoiceID = dataTable.Rows[i].Field<string>("cmfArInvoiceID");
				eRPFollowupInformationDto.cmfAssetID = dataTable.Rows[i].Field<string>("cmfAssetID");
				eRPFollowupInformationDto.cmfAssignedToEmployeeID = dataTable.Rows[i].Field<string>("cmfAssignedToEmployeeID");
				eRPFollowupInformationDto.cmfAttachedToEmployeeID = dataTable.Rows[i].Field<string>("cmfAttachedToEmployeeID");
				eRPFollowupInformationDto.cmfCallID = dataTable.Rows[i].Field<string>("cmfCallID");
				eRPFollowupInformationDto.cmfChangeRequestID = dataTable.Rows[i].Field<string>("cmfChangeRequestID");
				eRPFollowupInformationDto.cmfFollowupID = dataTable.Rows[i].Field<string>("cmfFollowupID");
				eRPFollowupInformationDto.cmfCompletedDate = dataTable.Rows[i].Field<DateTime?>("cmfCompletedDate");
				eRPFollowupInformationDto.cmfContactID = dataTable.Rows[i].Field<string>("cmfContactID");
				eRPFollowupInformationDto.cmfCreatedBy = dataTable.Rows[i].Field<string>("cmfCreatedBy");
				eRPFollowupInformationDto.cmfCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmfCreatedDate");
				eRPFollowupInformationDto.cmfDmrClaimID = dataTable.Rows[i].Field<string>("cmfDmrClaimID");
				eRPFollowupInformationDto.cmfDueDate = dataTable.Rows[i].Field<DateTime?>("cmfDueDate");
				eRPFollowupInformationDto.cmfUniqueID = dataTable.Rows[i].Field<Guid>("cmfUniqueID");
				eRPFollowupInformationDto.cmfExchangeID = dataTable.Rows[i].Field<string>("cmfExchangeID");
				eRPFollowupInformationDto.cmfFollowupType = dataTable.Rows[i].Field<byte>("cmfFollowupType");
				eRPFollowupInformationDto.cmfCreatedFromMobile = dataTable.Rows[i].Field<bool>("cmfCreatedFromMobile");
				eRPFollowupInformationDto.cmfJobID = dataTable.Rows[i].Field<string>("cmfJobID");
				eRPFollowupInformationDto.cmfLeadID = dataTable.Rows[i].Field<string>("cmfLeadID");
				eRPFollowupInformationDto.cmfLocationID = dataTable.Rows[i].Field<string>("cmfLocationID");
				eRPFollowupInformationDto.cmfLongDescriptionRtf = dataTable.Rows[i].Field<string>("cmfLongDescriptionRtf");
				eRPFollowupInformationDto.cmfLongDescriptionText = dataTable.Rows[i].Field<string>("cmfLongDescriptionText");
				eRPFollowupInformationDto.cmfMeetingLocation = dataTable.Rows[i].Field<string>("cmfMeetingLocation");
				eRPFollowupInformationDto.cmfOrganizationID = dataTable.Rows[i].Field<string>("cmfOrganizationID");
				eRPFollowupInformationDto.cmfPriority = dataTable.Rows[i].Field<byte>("cmfPriority");
				eRPFollowupInformationDto.cmfProjectAreaID = dataTable.Rows[i].Field<string>("cmfProjectAreaID");
				eRPFollowupInformationDto.cmfProjectID = dataTable.Rows[i].Field<string>("cmfProjectID");
				eRPFollowupInformationDto.cmfPurchaseOrderID = dataTable.Rows[i].Field<string>("cmfPurchaseOrderID");
				eRPFollowupInformationDto.cmfQuoteID = dataTable.Rows[i].Field<string>("cmfQuoteID");
				eRPFollowupInformationDto.cmfReceiptID = dataTable.Rows[i].Field<string>("cmfReceiptID");
				eRPFollowupInformationDto.cmfRfqID = dataTable.Rows[i].Field<string>("cmfRfqID");
				eRPFollowupInformationDto.cmfRmaClaimID = dataTable.Rows[i].Field<string>("cmfRmaClaimID");
				eRPFollowupInformationDto.cmfRowVersion = dataTable.Rows[i].Field<byte[]>("cmfRowVersion");
				eRPFollowupInformationDto.cmfSalesOrderID = dataTable.Rows[i].Field<string>("cmfSalesOrderID");
				eRPFollowupInformationDto.cmfShipmentID = dataTable.Rows[i].Field<string>("cmfShipmentID");
				eRPFollowupInformationDto.cmfShortDescription = dataTable.Rows[i].Field<string>("cmfShortDescription");
				eRPFollowupInformationDto.cmfStartDate = dataTable.Rows[i].Field<DateTime?>("cmfStartDate");
				eRPFollowupInformationDto.cmfStatus = dataTable.Rows[i].Field<byte>("cmfStatus");
				eRPFollowupInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPFollowupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPFollowupInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPFollowupInformationDto> GetFollowup(Guid followupId)
	{
		ERPFollowupInformationDto eRPFollowupInformationDto = new ERPFollowupInformationDto();
		InitializeParameterLists();
		string[] collection = new string[39]
		{
			"cmfApInvoiceID", "cmfArInvoiceID", "cmfAssetID", "cmfAssignedToEmployeeID", "cmfAttachedToEmployeeID", "cmfCallID", "cmfChangeRequestID", "cmfFollowupID", "cmfCompletedDate", "cmfContactID",
			"cmfCreatedBy", "cmfCreatedDate", "cmfDmrClaimID", "cmfDueDate", "cmfUniqueID", "cmfExchangeID", "cmfFollowupType", "cmfCreatedFromMobile", "cmfJobID", "cmfLeadID",
			"cmfLocationID", "cmfLongDescriptionRtf", "cmfLongDescriptionText", "cmfMeetingLocation", "cmfOrganizationID", "cmfPriority", "cmfProjectAreaID", "cmfProjectID", "cmfPurchaseOrderID", "cmfQuoteID",
			"cmfReceiptID", "cmfRfqID", "cmfRmaClaimID", "cmfRowVersion", "cmfSalesOrderID", "cmfShipmentID", "cmfShortDescription", "cmfStartDate", "cmfStatus"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("cmfUniqueID|C", followupId);
		AddCustomFieldsToSelectList("Followups");
		using (DataTable dataTable = GetAsDataTable("Followups", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPFollowupInformationDto);
			}
			eRPFollowupInformationDto.cmfApInvoiceID = dataTable.Rows[0].Field<string>("cmfApInvoiceID");
			eRPFollowupInformationDto.cmfArInvoiceID = dataTable.Rows[0].Field<string>("cmfArInvoiceID");
			eRPFollowupInformationDto.cmfAssetID = dataTable.Rows[0].Field<string>("cmfAssetID");
			eRPFollowupInformationDto.cmfAssignedToEmployeeID = dataTable.Rows[0].Field<string>("cmfAssignedToEmployeeID");
			eRPFollowupInformationDto.cmfAttachedToEmployeeID = dataTable.Rows[0].Field<string>("cmfAttachedToEmployeeID");
			eRPFollowupInformationDto.cmfCallID = dataTable.Rows[0].Field<string>("cmfCallID");
			eRPFollowupInformationDto.cmfChangeRequestID = dataTable.Rows[0].Field<string>("cmfChangeRequestID");
			eRPFollowupInformationDto.cmfFollowupID = dataTable.Rows[0].Field<string>("cmfFollowupID");
			eRPFollowupInformationDto.cmfCompletedDate = dataTable.Rows[0].Field<DateTime?>("cmfCompletedDate");
			eRPFollowupInformationDto.cmfContactID = dataTable.Rows[0].Field<string>("cmfContactID");
			eRPFollowupInformationDto.cmfCreatedBy = dataTable.Rows[0].Field<string>("cmfCreatedBy");
			eRPFollowupInformationDto.cmfCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmfCreatedDate");
			eRPFollowupInformationDto.cmfDmrClaimID = dataTable.Rows[0].Field<string>("cmfDmrClaimID");
			eRPFollowupInformationDto.cmfDueDate = dataTable.Rows[0].Field<DateTime?>("cmfDueDate");
			eRPFollowupInformationDto.cmfUniqueID = dataTable.Rows[0].Field<Guid>("cmfUniqueID");
			eRPFollowupInformationDto.cmfExchangeID = dataTable.Rows[0].Field<string>("cmfExchangeID");
			eRPFollowupInformationDto.cmfFollowupType = dataTable.Rows[0].Field<byte>("cmfFollowupType");
			eRPFollowupInformationDto.cmfCreatedFromMobile = dataTable.Rows[0].Field<bool>("cmfCreatedFromMobile");
			eRPFollowupInformationDto.cmfJobID = dataTable.Rows[0].Field<string>("cmfJobID");
			eRPFollowupInformationDto.cmfLeadID = dataTable.Rows[0].Field<string>("cmfLeadID");
			eRPFollowupInformationDto.cmfLocationID = dataTable.Rows[0].Field<string>("cmfLocationID");
			eRPFollowupInformationDto.cmfLongDescriptionRtf = dataTable.Rows[0].Field<string>("cmfLongDescriptionRtf");
			eRPFollowupInformationDto.cmfLongDescriptionText = dataTable.Rows[0].Field<string>("cmfLongDescriptionText");
			eRPFollowupInformationDto.cmfMeetingLocation = dataTable.Rows[0].Field<string>("cmfMeetingLocation");
			eRPFollowupInformationDto.cmfOrganizationID = dataTable.Rows[0].Field<string>("cmfOrganizationID");
			eRPFollowupInformationDto.cmfPriority = dataTable.Rows[0].Field<byte>("cmfPriority");
			eRPFollowupInformationDto.cmfProjectAreaID = dataTable.Rows[0].Field<string>("cmfProjectAreaID");
			eRPFollowupInformationDto.cmfProjectID = dataTable.Rows[0].Field<string>("cmfProjectID");
			eRPFollowupInformationDto.cmfPurchaseOrderID = dataTable.Rows[0].Field<string>("cmfPurchaseOrderID");
			eRPFollowupInformationDto.cmfQuoteID = dataTable.Rows[0].Field<string>("cmfQuoteID");
			eRPFollowupInformationDto.cmfReceiptID = dataTable.Rows[0].Field<string>("cmfReceiptID");
			eRPFollowupInformationDto.cmfRfqID = dataTable.Rows[0].Field<string>("cmfRfqID");
			eRPFollowupInformationDto.cmfRmaClaimID = dataTable.Rows[0].Field<string>("cmfRmaClaimID");
			eRPFollowupInformationDto.cmfRowVersion = dataTable.Rows[0].Field<byte[]>("cmfRowVersion");
			eRPFollowupInformationDto.cmfSalesOrderID = dataTable.Rows[0].Field<string>("cmfSalesOrderID");
			eRPFollowupInformationDto.cmfShipmentID = dataTable.Rows[0].Field<string>("cmfShipmentID");
			eRPFollowupInformationDto.cmfShortDescription = dataTable.Rows[0].Field<string>("cmfShortDescription");
			eRPFollowupInformationDto.cmfStartDate = dataTable.Rows[0].Field<DateTime?>("cmfStartDate");
			eRPFollowupInformationDto.cmfStatus = dataTable.Rows[0].Field<byte>("cmfStatus");
			eRPFollowupInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPFollowupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPFollowupInformationDto);
	}

	public Task<APIValidationInfoDto> SaveFollowup(ERPFollowupDto followup)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Followups WHERE cmfUniqueID = " + M1Util.ConvertToLinq(followup.cmfUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmfFollowupID"] = followup.cmfFollowupID.ToUpper();
				followup.cmfUniqueID = ((followup.cmfUniqueID == Guid.Empty) ? Guid.NewGuid() : followup.cmfUniqueID);
				dataRow["cmfUniqueID"] = followup.cmfUniqueID;
				dataRow["cmfCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmfCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Followup could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (followup.cmfRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Followup is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmfRowVersion"], followup.cmfRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Followup has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Followup again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmfApInvoiceID"] = followup.cmfApInvoiceID;
			dataRow["cmfArInvoiceID"] = followup.cmfArInvoiceID;
			dataRow["cmfAssetID"] = followup.cmfAssetID;
			dataRow["cmfAssignedToEmployeeID"] = followup.cmfAssignedToEmployeeID;
			dataRow["cmfAttachedToEmployeeID"] = followup.cmfAttachedToEmployeeID;
			dataRow["cmfCallID"] = followup.cmfCallID;
			dataRow["cmfChangeRequestID"] = followup.cmfChangeRequestID;
			DataRow dataRow2 = dataRow;
			DateTime? cmfCompletedDate = followup.cmfCompletedDate;
			dataRow2["cmfCompletedDate"] = (cmfCompletedDate.HasValue ? ((object)cmfCompletedDate.GetValueOrDefault()) : dataRow["cmfCompletedDate"]);
			dataRow["cmfContactID"] = followup.cmfContactID;
			dataRow["cmfDmrClaimID"] = followup.cmfDmrClaimID;
			DataRow dataRow3 = dataRow;
			cmfCompletedDate = followup.cmfDueDate;
			dataRow3["cmfDueDate"] = (cmfCompletedDate.HasValue ? ((object)cmfCompletedDate.GetValueOrDefault()) : dataRow["cmfDueDate"]);
			dataRow["cmfExchangeID"] = followup.cmfExchangeID ?? dataRow["cmfExchangeID"];
			dataRow["cmfFollowupType"] = followup.cmfFollowupType;
			dataRow["cmfCreatedFromMobile"] = followup.cmfCreatedFromMobile;
			dataRow["cmfJobID"] = followup.cmfJobID;
			dataRow["cmfLeadID"] = followup.cmfLeadID;
			dataRow["cmfLocationID"] = followup.cmfLocationID;
			dataRow["cmfLongDescriptionRtf"] = followup.cmfLongDescriptionRtf ?? dataRow["cmfLongDescriptionRtf"];
			dataRow["cmfLongDescriptionText"] = followup.cmfLongDescriptionText ?? dataRow["cmfLongDescriptionText"];
			dataRow["cmfMeetingLocation"] = followup.cmfMeetingLocation;
			dataRow["cmfOrganizationID"] = followup.cmfOrganizationID;
			dataRow["cmfPriority"] = followup.cmfPriority;
			dataRow["cmfProjectAreaID"] = followup.cmfProjectAreaID;
			dataRow["cmfProjectID"] = followup.cmfProjectID;
			dataRow["cmfPurchaseOrderID"] = followup.cmfPurchaseOrderID;
			dataRow["cmfQuoteID"] = followup.cmfQuoteID;
			dataRow["cmfReceiptID"] = followup.cmfReceiptID;
			dataRow["cmfRfqID"] = followup.cmfRfqID;
			dataRow["cmfRmaClaimID"] = followup.cmfRmaClaimID;
			dataRow["cmfSalesOrderID"] = followup.cmfSalesOrderID;
			dataRow["cmfShipmentID"] = followup.cmfShipmentID;
			dataRow["cmfShortDescription"] = followup.cmfShortDescription;
			DataRow dataRow4 = dataRow;
			cmfCompletedDate = followup.cmfStartDate;
			dataRow4["cmfStartDate"] = (cmfCompletedDate.HasValue ? ((object)cmfCompletedDate.GetValueOrDefault()) : dataRow["cmfStartDate"]);
			dataRow["cmfStatus"] = followup.cmfStatus;
			if (followup.CustomFields != null && followup.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in followup.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Followup [{followup.cmfUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Followup [{followup.cmfUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
