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

public class ERPSalesOrderPickListSessionRepository : APIBaseRepository, IERPSalesOrderPickListSessionRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderPickListSessionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderPickListSessionExist(Guid salesOrderPickListSessionId)
	{
		InitializeParameterLists();
		base.filterList.Add("omsUniqueID|C", salesOrderPickListSessionId);
		base.selectList.Add("omsUniqueID");
		return Task.FromResult(GetAsObject("SalesOrderPickListSessions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderPickListSessionInformationDto>> GetAllSalesOrderPickListSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderPickListSessionInformationDto> collection = new List<ERPSalesOrderPickListSessionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"omsCreatedBy", "omsCreatedDate", "omsDevice", "omsUniqueID", "omsPullFromStockOnly", "omsPickListSessionID", "omsPlantDepartmentID", "omsPlantID", "omsPostedDate", "omsRowVersion",
			"omsSessionDate", "omsStatus"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrderPickListSessions");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrderPickListSessions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderPickListSessionInformationDto eRPSalesOrderPickListSessionInformationDto = new ERPSalesOrderPickListSessionInformationDto();
				eRPSalesOrderPickListSessionInformationDto.omsCreatedBy = dataTable.Rows[i].Field<string>("omsCreatedBy");
				eRPSalesOrderPickListSessionInformationDto.omsCreatedDate = dataTable.Rows[i].Field<DateTime?>("omsCreatedDate");
				eRPSalesOrderPickListSessionInformationDto.omsDevice = dataTable.Rows[i].Field<byte>("omsDevice");
				eRPSalesOrderPickListSessionInformationDto.omsUniqueID = dataTable.Rows[i].Field<Guid>("omsUniqueID");
				eRPSalesOrderPickListSessionInformationDto.omsPullFromStockOnly = dataTable.Rows[i].Field<bool>("omsPullFromStockOnly");
				eRPSalesOrderPickListSessionInformationDto.omsPickListSessionID = dataTable.Rows[i].Field<int>("omsPickListSessionID");
				eRPSalesOrderPickListSessionInformationDto.omsPlantDepartmentID = dataTable.Rows[i].Field<string>("omsPlantDepartmentID");
				eRPSalesOrderPickListSessionInformationDto.omsPlantID = dataTable.Rows[i].Field<string>("omsPlantID");
				eRPSalesOrderPickListSessionInformationDto.omsPostedDate = dataTable.Rows[i].Field<DateTime?>("omsPostedDate");
				eRPSalesOrderPickListSessionInformationDto.omsRowVersion = dataTable.Rows[i].Field<byte[]>("omsRowVersion");
				eRPSalesOrderPickListSessionInformationDto.omsSessionDate = dataTable.Rows[i].Field<DateTime?>("omsSessionDate");
				eRPSalesOrderPickListSessionInformationDto.omsStatus = dataTable.Rows[i].Field<byte>("omsStatus");
				eRPSalesOrderPickListSessionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderPickListSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderPickListSessionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderPickListSessionInformationDto> GetSalesOrderPickListSession(Guid salesOrderPickListSessionId)
	{
		ERPSalesOrderPickListSessionInformationDto eRPSalesOrderPickListSessionInformationDto = new ERPSalesOrderPickListSessionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"omsCreatedBy", "omsCreatedDate", "omsDevice", "omsUniqueID", "omsPullFromStockOnly", "omsPickListSessionID", "omsPlantDepartmentID", "omsPlantID", "omsPostedDate", "omsRowVersion",
			"omsSessionDate", "omsStatus"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("omsUniqueID|C", salesOrderPickListSessionId);
		AddCustomFieldsToSelectList("SalesOrderPickListSessions");
		using (DataTable dataTable = GetAsDataTable("SalesOrderPickListSessions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderPickListSessionInformationDto);
			}
			eRPSalesOrderPickListSessionInformationDto.omsCreatedBy = dataTable.Rows[0].Field<string>("omsCreatedBy");
			eRPSalesOrderPickListSessionInformationDto.omsCreatedDate = dataTable.Rows[0].Field<DateTime?>("omsCreatedDate");
			eRPSalesOrderPickListSessionInformationDto.omsDevice = dataTable.Rows[0].Field<byte>("omsDevice");
			eRPSalesOrderPickListSessionInformationDto.omsUniqueID = dataTable.Rows[0].Field<Guid>("omsUniqueID");
			eRPSalesOrderPickListSessionInformationDto.omsPullFromStockOnly = dataTable.Rows[0].Field<bool>("omsPullFromStockOnly");
			eRPSalesOrderPickListSessionInformationDto.omsPickListSessionID = dataTable.Rows[0].Field<int>("omsPickListSessionID");
			eRPSalesOrderPickListSessionInformationDto.omsPlantDepartmentID = dataTable.Rows[0].Field<string>("omsPlantDepartmentID");
			eRPSalesOrderPickListSessionInformationDto.omsPlantID = dataTable.Rows[0].Field<string>("omsPlantID");
			eRPSalesOrderPickListSessionInformationDto.omsPostedDate = dataTable.Rows[0].Field<DateTime?>("omsPostedDate");
			eRPSalesOrderPickListSessionInformationDto.omsRowVersion = dataTable.Rows[0].Field<byte[]>("omsRowVersion");
			eRPSalesOrderPickListSessionInformationDto.omsSessionDate = dataTable.Rows[0].Field<DateTime?>("omsSessionDate");
			eRPSalesOrderPickListSessionInformationDto.omsStatus = dataTable.Rows[0].Field<byte>("omsStatus");
			eRPSalesOrderPickListSessionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderPickListSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderPickListSessionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrderPickListSession(ERPSalesOrderPickListSessionDto salesOrderPickListSession)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrderPickListSessions WHERE omsUniqueID = " + M1Util.ConvertToLinq(salesOrderPickListSession.omsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["omsPickListSessionID"] = salesOrderPickListSession.omsPickListSessionID;
				salesOrderPickListSession.omsUniqueID = ((salesOrderPickListSession.omsUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrderPickListSession.omsUniqueID);
				dataRow["omsUniqueID"] = salesOrderPickListSession.omsUniqueID;
				dataRow["omsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["omsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrderPickListSession could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrderPickListSession.omsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrderPickListSession is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["omsRowVersion"], salesOrderPickListSession.omsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrderPickListSession has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrderPickListSession again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["omsDevice"] = salesOrderPickListSession.omsDevice;
			dataRow["omsPullFromStockOnly"] = salesOrderPickListSession.omsPullFromStockOnly;
			dataRow["omsPlantDepartmentID"] = salesOrderPickListSession.omsPlantDepartmentID;
			dataRow["omsPlantID"] = salesOrderPickListSession.omsPlantID;
			DataRow dataRow2 = dataRow;
			DateTime? omsPostedDate = salesOrderPickListSession.omsPostedDate;
			dataRow2["omsPostedDate"] = (omsPostedDate.HasValue ? ((object)omsPostedDate.GetValueOrDefault()) : dataRow["omsPostedDate"]);
			DataRow dataRow3 = dataRow;
			omsPostedDate = salesOrderPickListSession.omsSessionDate;
			dataRow3["omsSessionDate"] = (omsPostedDate.HasValue ? ((object)omsPostedDate.GetValueOrDefault()) : dataRow["omsSessionDate"]);
			dataRow["omsStatus"] = salesOrderPickListSession.omsStatus;
			if (salesOrderPickListSession.CustomFields != null && salesOrderPickListSession.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrderPickListSession.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrderPickListSession [{salesOrderPickListSession.omsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrderPickListSession [{salesOrderPickListSession.omsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
