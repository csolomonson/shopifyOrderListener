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

public class ERPMRPSessionRepository : APIBaseRepository, IERPMRPSessionRepository, IAPIBaseRepository, IDisposable
{
	public ERPMRPSessionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMRPSessionExist(Guid mRPSessionId)
	{
		InitializeParameterLists();
		base.filterList.Add("mrpUniqueID|C", mRPSessionId);
		base.selectList.Add("mrpUniqueID");
		return Task.FromResult(GetAsObject("MRPSessions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMRPSessionInformationDto>> GetAllMRPSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMRPSessionInformationDto> collection = new List<ERPMRPSessionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"mrpCompletedDate", "mrpCreatedBy", "mrpCreatedDate", "mrpCustomerIDs", "mrpCutoffDate", "mrpUniqueID", "mrpCompleted", "mrpConsolidatePartForecastJobs", "mrpGenerated", "mrpIncludePartForecasts",
			"mrpPartClassIDs", "mrpPartGroupIDs", "mrpPartIDs", "mrpPlantIDs", "mrpRowVersion", "mrpSessionID", "mrpWarehouseIDs"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MRPSessions");
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
		using (DataTable dataTable = GetAsDataTable("MRPSessions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMRPSessionInformationDto eRPMRPSessionInformationDto = new ERPMRPSessionInformationDto();
				eRPMRPSessionInformationDto.mrpCompletedDate = dataTable.Rows[i].Field<DateTime?>("mrpCompletedDate");
				eRPMRPSessionInformationDto.mrpCreatedBy = dataTable.Rows[i].Field<string>("mrpCreatedBy");
				eRPMRPSessionInformationDto.mrpCreatedDate = dataTable.Rows[i].Field<DateTime?>("mrpCreatedDate");
				eRPMRPSessionInformationDto.mrpCustomerIDs = dataTable.Rows[i].Field<string>("mrpCustomerIDs");
				eRPMRPSessionInformationDto.mrpCutoffDate = dataTable.Rows[i].Field<DateTime?>("mrpCutoffDate");
				eRPMRPSessionInformationDto.mrpUniqueID = dataTable.Rows[i].Field<Guid>("mrpUniqueID");
				eRPMRPSessionInformationDto.mrpCompleted = dataTable.Rows[i].Field<bool>("mrpCompleted");
				eRPMRPSessionInformationDto.mrpConsolidatePartForecastJobs = dataTable.Rows[i].Field<bool>("mrpConsolidatePartForecastJobs");
				eRPMRPSessionInformationDto.mrpGenerated = dataTable.Rows[i].Field<bool>("mrpGenerated");
				eRPMRPSessionInformationDto.mrpIncludePartForecasts = dataTable.Rows[i].Field<bool>("mrpIncludePartForecasts");
				eRPMRPSessionInformationDto.mrpPartClassIDs = dataTable.Rows[i].Field<string>("mrpPartClassIDs");
				eRPMRPSessionInformationDto.mrpPartGroupIDs = dataTable.Rows[i].Field<string>("mrpPartGroupIDs");
				eRPMRPSessionInformationDto.mrpPartIDs = dataTable.Rows[i].Field<string>("mrpPartIDs");
				eRPMRPSessionInformationDto.mrpPlantIDs = dataTable.Rows[i].Field<string>("mrpPlantIDs");
				eRPMRPSessionInformationDto.mrpRowVersion = dataTable.Rows[i].Field<byte[]>("mrpRowVersion");
				eRPMRPSessionInformationDto.mrpSessionID = dataTable.Rows[i].Field<string>("mrpSessionID");
				eRPMRPSessionInformationDto.mrpWarehouseIDs = dataTable.Rows[i].Field<string>("mrpWarehouseIDs");
				eRPMRPSessionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMRPSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMRPSessionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMRPSessionInformationDto> GetMRPSession(Guid mRPSessionId)
	{
		ERPMRPSessionInformationDto eRPMRPSessionInformationDto = new ERPMRPSessionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"mrpCompletedDate", "mrpCreatedBy", "mrpCreatedDate", "mrpCustomerIDs", "mrpCutoffDate", "mrpUniqueID", "mrpCompleted", "mrpConsolidatePartForecastJobs", "mrpGenerated", "mrpIncludePartForecasts",
			"mrpPartClassIDs", "mrpPartGroupIDs", "mrpPartIDs", "mrpPlantIDs", "mrpRowVersion", "mrpSessionID", "mrpWarehouseIDs"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("mrpUniqueID|C", mRPSessionId);
		AddCustomFieldsToSelectList("MRPSessions");
		using (DataTable dataTable = GetAsDataTable("MRPSessions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMRPSessionInformationDto);
			}
			eRPMRPSessionInformationDto.mrpCompletedDate = dataTable.Rows[0].Field<DateTime?>("mrpCompletedDate");
			eRPMRPSessionInformationDto.mrpCreatedBy = dataTable.Rows[0].Field<string>("mrpCreatedBy");
			eRPMRPSessionInformationDto.mrpCreatedDate = dataTable.Rows[0].Field<DateTime?>("mrpCreatedDate");
			eRPMRPSessionInformationDto.mrpCustomerIDs = dataTable.Rows[0].Field<string>("mrpCustomerIDs");
			eRPMRPSessionInformationDto.mrpCutoffDate = dataTable.Rows[0].Field<DateTime?>("mrpCutoffDate");
			eRPMRPSessionInformationDto.mrpUniqueID = dataTable.Rows[0].Field<Guid>("mrpUniqueID");
			eRPMRPSessionInformationDto.mrpCompleted = dataTable.Rows[0].Field<bool>("mrpCompleted");
			eRPMRPSessionInformationDto.mrpConsolidatePartForecastJobs = dataTable.Rows[0].Field<bool>("mrpConsolidatePartForecastJobs");
			eRPMRPSessionInformationDto.mrpGenerated = dataTable.Rows[0].Field<bool>("mrpGenerated");
			eRPMRPSessionInformationDto.mrpIncludePartForecasts = dataTable.Rows[0].Field<bool>("mrpIncludePartForecasts");
			eRPMRPSessionInformationDto.mrpPartClassIDs = dataTable.Rows[0].Field<string>("mrpPartClassIDs");
			eRPMRPSessionInformationDto.mrpPartGroupIDs = dataTable.Rows[0].Field<string>("mrpPartGroupIDs");
			eRPMRPSessionInformationDto.mrpPartIDs = dataTable.Rows[0].Field<string>("mrpPartIDs");
			eRPMRPSessionInformationDto.mrpPlantIDs = dataTable.Rows[0].Field<string>("mrpPlantIDs");
			eRPMRPSessionInformationDto.mrpRowVersion = dataTable.Rows[0].Field<byte[]>("mrpRowVersion");
			eRPMRPSessionInformationDto.mrpSessionID = dataTable.Rows[0].Field<string>("mrpSessionID");
			eRPMRPSessionInformationDto.mrpWarehouseIDs = dataTable.Rows[0].Field<string>("mrpWarehouseIDs");
			eRPMRPSessionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMRPSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMRPSessionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMRPSession(ERPMRPSessionDto mRPSession)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MRPSessions WHERE mrpUniqueID = " + M1Util.ConvertToLinq(mRPSession.mrpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mrpSessionID"] = mRPSession.mrpSessionID.ToUpper();
				mRPSession.mrpUniqueID = ((mRPSession.mrpUniqueID == Guid.Empty) ? Guid.NewGuid() : mRPSession.mrpUniqueID);
				dataRow["mrpUniqueID"] = mRPSession.mrpUniqueID;
				dataRow["mrpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mrpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MRPSession could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (mRPSession.mrpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MRPSession is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mrpRowVersion"], mRPSession.mrpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MRPSession has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MRPSession again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? mrpCompletedDate = mRPSession.mrpCompletedDate;
			dataRow2["mrpCompletedDate"] = (mrpCompletedDate.HasValue ? ((object)mrpCompletedDate.GetValueOrDefault()) : dataRow["mrpCompletedDate"]);
			dataRow["mrpCustomerIDs"] = mRPSession.mrpCustomerIDs ?? dataRow["mrpCustomerIDs"];
			DataRow dataRow3 = dataRow;
			mrpCompletedDate = mRPSession.mrpCutoffDate;
			dataRow3["mrpCutoffDate"] = (mrpCompletedDate.HasValue ? ((object)mrpCompletedDate.GetValueOrDefault()) : dataRow["mrpCutoffDate"]);
			dataRow["mrpCompleted"] = mRPSession.mrpCompleted;
			dataRow["mrpConsolidatePartForecastJobs"] = mRPSession.mrpConsolidatePartForecastJobs;
			dataRow["mrpGenerated"] = mRPSession.mrpGenerated;
			dataRow["mrpIncludePartForecasts"] = mRPSession.mrpIncludePartForecasts;
			dataRow["mrpPartClassIDs"] = mRPSession.mrpPartClassIDs ?? dataRow["mrpPartClassIDs"];
			dataRow["mrpPartGroupIDs"] = mRPSession.mrpPartGroupIDs ?? dataRow["mrpPartGroupIDs"];
			dataRow["mrpPartIDs"] = mRPSession.mrpPartIDs ?? dataRow["mrpPartIDs"];
			dataRow["mrpPlantIDs"] = mRPSession.mrpPlantIDs ?? dataRow["mrpPlantIDs"];
			dataRow["mrpWarehouseIDs"] = mRPSession.mrpWarehouseIDs ?? dataRow["mrpWarehouseIDs"];
			if (mRPSession.CustomFields != null && mRPSession.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in mRPSession.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MRPSession [{mRPSession.mrpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MRPSession [{mRPSession.mrpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
