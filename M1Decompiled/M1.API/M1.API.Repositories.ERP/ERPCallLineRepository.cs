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

public class ERPCallLineRepository : APIBaseRepository, IERPCallLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPCallLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCallLineExist(Guid callLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("kblUniqueID|C", callLineId);
		base.selectList.Add("kblUniqueID");
		return Task.FromResult(GetAsObject("CallLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCallLineInformationDto>> GetAllCallLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCallLineInformationDto> collection = new List<ERPCallLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[19]
		{
			"kblAddedByEmployeeID", "kblAddedDate", "kblCallID", "kblContactMethodID", "kblCreatedBy", "kblCreatedDate", "kblUniqueID", "kblExtraTime", "kblBillable", "kblCreatedFromMobile",
			"kblInbound", "kblInternalOnly", "kblLongDescriptionRtf", "kblLongDescriptionText", "kblRowVersion", "kblCallLineID", "kblShortDescription", "kblTimeSpent", "kblTotalTime"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CallLines");
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
		using (DataTable dataTable = GetAsDataTable("CallLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCallLineInformationDto eRPCallLineInformationDto = new ERPCallLineInformationDto();
				eRPCallLineInformationDto.kblAddedByEmployeeID = dataTable.Rows[i].Field<string>("kblAddedByEmployeeID");
				eRPCallLineInformationDto.kblAddedDate = dataTable.Rows[i].Field<DateTime?>("kblAddedDate");
				eRPCallLineInformationDto.kblCallID = dataTable.Rows[i].Field<string>("kblCallID");
				eRPCallLineInformationDto.kblContactMethodID = dataTable.Rows[i].Field<string>("kblContactMethodID");
				eRPCallLineInformationDto.kblCreatedBy = dataTable.Rows[i].Field<string>("kblCreatedBy");
				eRPCallLineInformationDto.kblCreatedDate = dataTable.Rows[i].Field<DateTime?>("kblCreatedDate");
				eRPCallLineInformationDto.kblUniqueID = dataTable.Rows[i].Field<Guid>("kblUniqueID");
				eRPCallLineInformationDto.kblExtraTime = dataTable.Rows[i].Field<decimal>("kblExtraTime");
				eRPCallLineInformationDto.kblBillable = dataTable.Rows[i].Field<bool>("kblBillable");
				eRPCallLineInformationDto.kblCreatedFromMobile = dataTable.Rows[i].Field<bool>("kblCreatedFromMobile");
				eRPCallLineInformationDto.kblInbound = dataTable.Rows[i].Field<bool>("kblInbound");
				eRPCallLineInformationDto.kblInternalOnly = dataTable.Rows[i].Field<bool>("kblInternalOnly");
				eRPCallLineInformationDto.kblLongDescriptionRtf = dataTable.Rows[i].Field<string>("kblLongDescriptionRtf");
				eRPCallLineInformationDto.kblLongDescriptionText = dataTable.Rows[i].Field<string>("kblLongDescriptionText");
				eRPCallLineInformationDto.kblRowVersion = dataTable.Rows[i].Field<byte[]>("kblRowVersion");
				eRPCallLineInformationDto.kblCallLineID = dataTable.Rows[i].Field<short>("kblCallLineID");
				eRPCallLineInformationDto.kblShortDescription = dataTable.Rows[i].Field<string>("kblShortDescription");
				eRPCallLineInformationDto.kblTimeSpent = dataTable.Rows[i].Field<decimal>("kblTimeSpent");
				eRPCallLineInformationDto.kblTotalTime = dataTable.Rows[i].Field<decimal>("kblTotalTime");
				eRPCallLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCallLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCallLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCallLineInformationDto> GetCallLine(Guid callLineId)
	{
		ERPCallLineInformationDto eRPCallLineInformationDto = new ERPCallLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[19]
		{
			"kblAddedByEmployeeID", "kblAddedDate", "kblCallID", "kblContactMethodID", "kblCreatedBy", "kblCreatedDate", "kblUniqueID", "kblExtraTime", "kblBillable", "kblCreatedFromMobile",
			"kblInbound", "kblInternalOnly", "kblLongDescriptionRtf", "kblLongDescriptionText", "kblRowVersion", "kblCallLineID", "kblShortDescription", "kblTimeSpent", "kblTotalTime"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("kblUniqueID|C", callLineId);
		AddCustomFieldsToSelectList("CallLines");
		using (DataTable dataTable = GetAsDataTable("CallLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCallLineInformationDto);
			}
			eRPCallLineInformationDto.kblAddedByEmployeeID = dataTable.Rows[0].Field<string>("kblAddedByEmployeeID");
			eRPCallLineInformationDto.kblAddedDate = dataTable.Rows[0].Field<DateTime?>("kblAddedDate");
			eRPCallLineInformationDto.kblCallID = dataTable.Rows[0].Field<string>("kblCallID");
			eRPCallLineInformationDto.kblContactMethodID = dataTable.Rows[0].Field<string>("kblContactMethodID");
			eRPCallLineInformationDto.kblCreatedBy = dataTable.Rows[0].Field<string>("kblCreatedBy");
			eRPCallLineInformationDto.kblCreatedDate = dataTable.Rows[0].Field<DateTime?>("kblCreatedDate");
			eRPCallLineInformationDto.kblUniqueID = dataTable.Rows[0].Field<Guid>("kblUniqueID");
			eRPCallLineInformationDto.kblExtraTime = dataTable.Rows[0].Field<decimal>("kblExtraTime");
			eRPCallLineInformationDto.kblBillable = dataTable.Rows[0].Field<bool>("kblBillable");
			eRPCallLineInformationDto.kblCreatedFromMobile = dataTable.Rows[0].Field<bool>("kblCreatedFromMobile");
			eRPCallLineInformationDto.kblInbound = dataTable.Rows[0].Field<bool>("kblInbound");
			eRPCallLineInformationDto.kblInternalOnly = dataTable.Rows[0].Field<bool>("kblInternalOnly");
			eRPCallLineInformationDto.kblLongDescriptionRtf = dataTable.Rows[0].Field<string>("kblLongDescriptionRtf");
			eRPCallLineInformationDto.kblLongDescriptionText = dataTable.Rows[0].Field<string>("kblLongDescriptionText");
			eRPCallLineInformationDto.kblRowVersion = dataTable.Rows[0].Field<byte[]>("kblRowVersion");
			eRPCallLineInformationDto.kblCallLineID = dataTable.Rows[0].Field<short>("kblCallLineID");
			eRPCallLineInformationDto.kblShortDescription = dataTable.Rows[0].Field<string>("kblShortDescription");
			eRPCallLineInformationDto.kblTimeSpent = dataTable.Rows[0].Field<decimal>("kblTimeSpent");
			eRPCallLineInformationDto.kblTotalTime = dataTable.Rows[0].Field<decimal>("kblTotalTime");
			eRPCallLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCallLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCallLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCallLine(ERPCallLineDto callLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CallLines WHERE kblUniqueID = " + M1Util.ConvertToLinq(callLine.kblUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kblCallID"] = callLine.kblCallID.ToUpper();
				dataRow["kblCallLineID"] = callLine.kblCallLineID;
				callLine.kblUniqueID = ((callLine.kblUniqueID == Guid.Empty) ? Guid.NewGuid() : callLine.kblUniqueID);
				dataRow["kblUniqueID"] = callLine.kblUniqueID;
				dataRow["kblCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kblCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CallLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (callLine.kblRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CallLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kblRowVersion"], callLine.kblRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CallLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CallLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["kblAddedByEmployeeID"] = callLine.kblAddedByEmployeeID;
			DataRow dataRow2 = dataRow;
			DateTime? kblAddedDate = callLine.kblAddedDate;
			dataRow2["kblAddedDate"] = (kblAddedDate.HasValue ? ((object)kblAddedDate.GetValueOrDefault()) : dataRow["kblAddedDate"]);
			dataRow["kblContactMethodID"] = callLine.kblContactMethodID;
			dataRow["kblExtraTime"] = callLine.kblExtraTime;
			dataRow["kblBillable"] = callLine.kblBillable;
			dataRow["kblCreatedFromMobile"] = callLine.kblCreatedFromMobile;
			dataRow["kblInbound"] = callLine.kblInbound;
			dataRow["kblInternalOnly"] = callLine.kblInternalOnly;
			dataRow["kblLongDescriptionRtf"] = callLine.kblLongDescriptionRtf ?? dataRow["kblLongDescriptionRtf"];
			dataRow["kblLongDescriptionText"] = callLine.kblLongDescriptionText ?? dataRow["kblLongDescriptionText"];
			dataRow["kblShortDescription"] = callLine.kblShortDescription;
			dataRow["kblTimeSpent"] = callLine.kblTimeSpent;
			dataRow["kblTotalTime"] = callLine.kblTotalTime;
			if (callLine.CustomFields != null && callLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in callLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CallLine [{callLine.kblUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CallLine [{callLine.kblUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
