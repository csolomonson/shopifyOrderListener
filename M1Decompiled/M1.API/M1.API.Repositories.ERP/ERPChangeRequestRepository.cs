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

public class ERPChangeRequestRepository : APIBaseRepository, IERPChangeRequestRepository, IAPIBaseRepository, IDisposable
{
	public ERPChangeRequestRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesChangeRequestExist(Guid changeRequestId)
	{
		InitializeParameterLists();
		base.filterList.Add("chpUniqueID|C", changeRequestId);
		base.selectList.Add("chpUniqueID");
		return Task.FromResult(GetAsObject("ChangeRequests", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPChangeRequestInformationDto>> GetAllChangeRequests(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPChangeRequestInformationDto> collection = new List<ERPChangeRequestInformationDto>();
		InitializeParameterLists();
		string[] array = new string[31]
		{
			"chpActualHours", "chpAssignedDate", "chpAssignedToEmployeeID", "chpAuthorizedByEmployeeID", "chpAuthorizedDate", "chpChangeRequestTypeID", "chpClosedByEmployeeID", "chpClosedDate", "chpClosedReasonID", "chpChangeRequestID",
			"chpCreatedBy", "chpCreatedDate", "chpDueDate", "chpUniqueID", "chpEstimatedHours", "chpJobID", "chpLongDescriptionRtf", "chpLongDescriptionText", "chpNonConformanceID", "chpOpenedByEmployeeID",
			"chpOpenedDate", "chpPartID", "chpPartRevisionID", "chpPriorityID", "chpProjectAreaID", "chpProjectID", "chpResolvedPartID", "chpResolvedPartRevisionID", "chpRowVersion", "chpShortDescription",
			"chpStatus"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ChangeRequests");
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
		using (DataTable dataTable = GetAsDataTable("ChangeRequests", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPChangeRequestInformationDto eRPChangeRequestInformationDto = new ERPChangeRequestInformationDto();
				eRPChangeRequestInformationDto.chpActualHours = dataTable.Rows[i].Field<decimal>("chpActualHours");
				eRPChangeRequestInformationDto.chpAssignedDate = dataTable.Rows[i].Field<DateTime?>("chpAssignedDate");
				eRPChangeRequestInformationDto.chpAssignedToEmployeeID = dataTable.Rows[i].Field<string>("chpAssignedToEmployeeID");
				eRPChangeRequestInformationDto.chpAuthorizedByEmployeeID = dataTable.Rows[i].Field<string>("chpAuthorizedByEmployeeID");
				eRPChangeRequestInformationDto.chpAuthorizedDate = dataTable.Rows[i].Field<DateTime?>("chpAuthorizedDate");
				eRPChangeRequestInformationDto.chpChangeRequestTypeID = dataTable.Rows[i].Field<string>("chpChangeRequestTypeID");
				eRPChangeRequestInformationDto.chpClosedByEmployeeID = dataTable.Rows[i].Field<string>("chpClosedByEmployeeID");
				eRPChangeRequestInformationDto.chpClosedDate = dataTable.Rows[i].Field<DateTime?>("chpClosedDate");
				eRPChangeRequestInformationDto.chpClosedReasonID = dataTable.Rows[i].Field<string>("chpClosedReasonID");
				eRPChangeRequestInformationDto.chpChangeRequestID = dataTable.Rows[i].Field<string>("chpChangeRequestID");
				eRPChangeRequestInformationDto.chpCreatedBy = dataTable.Rows[i].Field<string>("chpCreatedBy");
				eRPChangeRequestInformationDto.chpCreatedDate = dataTable.Rows[i].Field<DateTime?>("chpCreatedDate");
				eRPChangeRequestInformationDto.chpDueDate = dataTable.Rows[i].Field<DateTime?>("chpDueDate");
				eRPChangeRequestInformationDto.chpUniqueID = dataTable.Rows[i].Field<Guid>("chpUniqueID");
				eRPChangeRequestInformationDto.chpEstimatedHours = dataTable.Rows[i].Field<decimal>("chpEstimatedHours");
				eRPChangeRequestInformationDto.chpJobID = dataTable.Rows[i].Field<string>("chpJobID");
				eRPChangeRequestInformationDto.chpLongDescriptionRtf = dataTable.Rows[i].Field<string>("chpLongDescriptionRtf");
				eRPChangeRequestInformationDto.chpLongDescriptionText = dataTable.Rows[i].Field<string>("chpLongDescriptionText");
				eRPChangeRequestInformationDto.chpNonConformanceID = dataTable.Rows[i].Field<string>("chpNonConformanceID");
				eRPChangeRequestInformationDto.chpOpenedByEmployeeID = dataTable.Rows[i].Field<string>("chpOpenedByEmployeeID");
				eRPChangeRequestInformationDto.chpOpenedDate = dataTable.Rows[i].Field<DateTime?>("chpOpenedDate");
				eRPChangeRequestInformationDto.chpPartID = dataTable.Rows[i].Field<string>("chpPartID");
				eRPChangeRequestInformationDto.chpPartRevisionID = dataTable.Rows[i].Field<string>("chpPartRevisionID");
				eRPChangeRequestInformationDto.chpPriorityID = dataTable.Rows[i].Field<byte>("chpPriorityID");
				eRPChangeRequestInformationDto.chpProjectAreaID = dataTable.Rows[i].Field<string>("chpProjectAreaID");
				eRPChangeRequestInformationDto.chpProjectID = dataTable.Rows[i].Field<string>("chpProjectID");
				eRPChangeRequestInformationDto.chpResolvedPartID = dataTable.Rows[i].Field<string>("chpResolvedPartID");
				eRPChangeRequestInformationDto.chpResolvedPartRevisionID = dataTable.Rows[i].Field<string>("chpResolvedPartRevisionID");
				eRPChangeRequestInformationDto.chpRowVersion = dataTable.Rows[i].Field<byte[]>("chpRowVersion");
				eRPChangeRequestInformationDto.chpShortDescription = dataTable.Rows[i].Field<string>("chpShortDescription");
				eRPChangeRequestInformationDto.chpStatus = dataTable.Rows[i].Field<string>("chpStatus");
				eRPChangeRequestInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPChangeRequestInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPChangeRequestInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPChangeRequestInformationDto> GetChangeRequest(Guid changeRequestId)
	{
		ERPChangeRequestInformationDto eRPChangeRequestInformationDto = new ERPChangeRequestInformationDto();
		InitializeParameterLists();
		string[] collection = new string[31]
		{
			"chpActualHours", "chpAssignedDate", "chpAssignedToEmployeeID", "chpAuthorizedByEmployeeID", "chpAuthorizedDate", "chpChangeRequestTypeID", "chpClosedByEmployeeID", "chpClosedDate", "chpClosedReasonID", "chpChangeRequestID",
			"chpCreatedBy", "chpCreatedDate", "chpDueDate", "chpUniqueID", "chpEstimatedHours", "chpJobID", "chpLongDescriptionRtf", "chpLongDescriptionText", "chpNonConformanceID", "chpOpenedByEmployeeID",
			"chpOpenedDate", "chpPartID", "chpPartRevisionID", "chpPriorityID", "chpProjectAreaID", "chpProjectID", "chpResolvedPartID", "chpResolvedPartRevisionID", "chpRowVersion", "chpShortDescription",
			"chpStatus"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("chpUniqueID|C", changeRequestId);
		AddCustomFieldsToSelectList("ChangeRequests");
		using (DataTable dataTable = GetAsDataTable("ChangeRequests", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPChangeRequestInformationDto);
			}
			eRPChangeRequestInformationDto.chpActualHours = dataTable.Rows[0].Field<decimal>("chpActualHours");
			eRPChangeRequestInformationDto.chpAssignedDate = dataTable.Rows[0].Field<DateTime?>("chpAssignedDate");
			eRPChangeRequestInformationDto.chpAssignedToEmployeeID = dataTable.Rows[0].Field<string>("chpAssignedToEmployeeID");
			eRPChangeRequestInformationDto.chpAuthorizedByEmployeeID = dataTable.Rows[0].Field<string>("chpAuthorizedByEmployeeID");
			eRPChangeRequestInformationDto.chpAuthorizedDate = dataTable.Rows[0].Field<DateTime?>("chpAuthorizedDate");
			eRPChangeRequestInformationDto.chpChangeRequestTypeID = dataTable.Rows[0].Field<string>("chpChangeRequestTypeID");
			eRPChangeRequestInformationDto.chpClosedByEmployeeID = dataTable.Rows[0].Field<string>("chpClosedByEmployeeID");
			eRPChangeRequestInformationDto.chpClosedDate = dataTable.Rows[0].Field<DateTime?>("chpClosedDate");
			eRPChangeRequestInformationDto.chpClosedReasonID = dataTable.Rows[0].Field<string>("chpClosedReasonID");
			eRPChangeRequestInformationDto.chpChangeRequestID = dataTable.Rows[0].Field<string>("chpChangeRequestID");
			eRPChangeRequestInformationDto.chpCreatedBy = dataTable.Rows[0].Field<string>("chpCreatedBy");
			eRPChangeRequestInformationDto.chpCreatedDate = dataTable.Rows[0].Field<DateTime?>("chpCreatedDate");
			eRPChangeRequestInformationDto.chpDueDate = dataTable.Rows[0].Field<DateTime?>("chpDueDate");
			eRPChangeRequestInformationDto.chpUniqueID = dataTable.Rows[0].Field<Guid>("chpUniqueID");
			eRPChangeRequestInformationDto.chpEstimatedHours = dataTable.Rows[0].Field<decimal>("chpEstimatedHours");
			eRPChangeRequestInformationDto.chpJobID = dataTable.Rows[0].Field<string>("chpJobID");
			eRPChangeRequestInformationDto.chpLongDescriptionRtf = dataTable.Rows[0].Field<string>("chpLongDescriptionRtf");
			eRPChangeRequestInformationDto.chpLongDescriptionText = dataTable.Rows[0].Field<string>("chpLongDescriptionText");
			eRPChangeRequestInformationDto.chpNonConformanceID = dataTable.Rows[0].Field<string>("chpNonConformanceID");
			eRPChangeRequestInformationDto.chpOpenedByEmployeeID = dataTable.Rows[0].Field<string>("chpOpenedByEmployeeID");
			eRPChangeRequestInformationDto.chpOpenedDate = dataTable.Rows[0].Field<DateTime?>("chpOpenedDate");
			eRPChangeRequestInformationDto.chpPartID = dataTable.Rows[0].Field<string>("chpPartID");
			eRPChangeRequestInformationDto.chpPartRevisionID = dataTable.Rows[0].Field<string>("chpPartRevisionID");
			eRPChangeRequestInformationDto.chpPriorityID = dataTable.Rows[0].Field<byte>("chpPriorityID");
			eRPChangeRequestInformationDto.chpProjectAreaID = dataTable.Rows[0].Field<string>("chpProjectAreaID");
			eRPChangeRequestInformationDto.chpProjectID = dataTable.Rows[0].Field<string>("chpProjectID");
			eRPChangeRequestInformationDto.chpResolvedPartID = dataTable.Rows[0].Field<string>("chpResolvedPartID");
			eRPChangeRequestInformationDto.chpResolvedPartRevisionID = dataTable.Rows[0].Field<string>("chpResolvedPartRevisionID");
			eRPChangeRequestInformationDto.chpRowVersion = dataTable.Rows[0].Field<byte[]>("chpRowVersion");
			eRPChangeRequestInformationDto.chpShortDescription = dataTable.Rows[0].Field<string>("chpShortDescription");
			eRPChangeRequestInformationDto.chpStatus = dataTable.Rows[0].Field<string>("chpStatus");
			eRPChangeRequestInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPChangeRequestInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPChangeRequestInformationDto);
	}

	public Task<APIValidationInfoDto> SaveChangeRequest(ERPChangeRequestDto changeRequest)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ChangeRequests WHERE chpUniqueID = " + M1Util.ConvertToLinq(changeRequest.chpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["chpChangeRequestID"] = changeRequest.chpChangeRequestID.ToUpper();
				changeRequest.chpUniqueID = ((changeRequest.chpUniqueID == Guid.Empty) ? Guid.NewGuid() : changeRequest.chpUniqueID);
				dataRow["chpUniqueID"] = changeRequest.chpUniqueID;
				dataRow["chpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["chpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ChangeRequest could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (changeRequest.chpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ChangeRequest is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["chpRowVersion"], changeRequest.chpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ChangeRequest has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ChangeRequest again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["chpActualHours"] = changeRequest.chpActualHours;
			DataRow dataRow2 = dataRow;
			DateTime? chpAssignedDate = changeRequest.chpAssignedDate;
			dataRow2["chpAssignedDate"] = (chpAssignedDate.HasValue ? ((object)chpAssignedDate.GetValueOrDefault()) : dataRow["chpAssignedDate"]);
			dataRow["chpAssignedToEmployeeID"] = changeRequest.chpAssignedToEmployeeID;
			dataRow["chpAuthorizedByEmployeeID"] = changeRequest.chpAuthorizedByEmployeeID;
			DataRow dataRow3 = dataRow;
			chpAssignedDate = changeRequest.chpAuthorizedDate;
			dataRow3["chpAuthorizedDate"] = (chpAssignedDate.HasValue ? ((object)chpAssignedDate.GetValueOrDefault()) : dataRow["chpAuthorizedDate"]);
			dataRow["chpChangeRequestTypeID"] = changeRequest.chpChangeRequestTypeID;
			dataRow["chpClosedByEmployeeID"] = changeRequest.chpClosedByEmployeeID;
			DataRow dataRow4 = dataRow;
			chpAssignedDate = changeRequest.chpClosedDate;
			dataRow4["chpClosedDate"] = (chpAssignedDate.HasValue ? ((object)chpAssignedDate.GetValueOrDefault()) : dataRow["chpClosedDate"]);
			dataRow["chpClosedReasonID"] = changeRequest.chpClosedReasonID;
			DataRow dataRow5 = dataRow;
			chpAssignedDate = changeRequest.chpDueDate;
			dataRow5["chpDueDate"] = (chpAssignedDate.HasValue ? ((object)chpAssignedDate.GetValueOrDefault()) : dataRow["chpDueDate"]);
			dataRow["chpEstimatedHours"] = changeRequest.chpEstimatedHours;
			dataRow["chpJobID"] = changeRequest.chpJobID;
			dataRow["chpLongDescriptionRtf"] = changeRequest.chpLongDescriptionRtf ?? dataRow["chpLongDescriptionRtf"];
			dataRow["chpLongDescriptionText"] = changeRequest.chpLongDescriptionText ?? dataRow["chpLongDescriptionText"];
			dataRow["chpNonConformanceID"] = changeRequest.chpNonConformanceID;
			dataRow["chpOpenedByEmployeeID"] = changeRequest.chpOpenedByEmployeeID;
			DataRow dataRow6 = dataRow;
			chpAssignedDate = changeRequest.chpOpenedDate;
			dataRow6["chpOpenedDate"] = (chpAssignedDate.HasValue ? ((object)chpAssignedDate.GetValueOrDefault()) : dataRow["chpOpenedDate"]);
			dataRow["chpPartID"] = changeRequest.chpPartID;
			dataRow["chpPartRevisionID"] = changeRequest.chpPartRevisionID;
			dataRow["chpPriorityID"] = changeRequest.chpPriorityID;
			dataRow["chpProjectAreaID"] = changeRequest.chpProjectAreaID;
			dataRow["chpProjectID"] = changeRequest.chpProjectID;
			dataRow["chpResolvedPartID"] = changeRequest.chpResolvedPartID;
			dataRow["chpResolvedPartRevisionID"] = changeRequest.chpResolvedPartRevisionID;
			dataRow["chpShortDescription"] = changeRequest.chpShortDescription;
			dataRow["chpStatus"] = changeRequest.chpStatus;
			if (changeRequest.CustomFields != null && changeRequest.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in changeRequest.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ChangeRequest [{changeRequest.chpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ChangeRequest [{changeRequest.chpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
