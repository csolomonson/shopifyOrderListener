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

public class ERPCallMemoRepository : APIBaseRepository, IERPCallMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPCallMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCallMemoExist(Guid callMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbkUniqueID|C", callMemoId);
		base.selectList.Add("kbkUniqueID");
		return Task.FromResult(GetAsObject("CallMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCallMemoInformationDto>> GetAllCallMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCallMemoInformationDto> collection = new List<ERPCallMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"kbkCallID", "kbkCreatedBy", "kbkCreatedDate", "kbkUniqueID", "kbkLongDescriptionRtf", "kbkLongDescriptionText", "kbkMemoDate", "kbkRowVersion", "kbkCallMemoID", "kbkShortDescription",
			"kbkShowInCalls"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CallMemos");
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
		using (DataTable dataTable = GetAsDataTable("CallMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCallMemoInformationDto eRPCallMemoInformationDto = new ERPCallMemoInformationDto();
				eRPCallMemoInformationDto.kbkCallID = dataTable.Rows[i].Field<string>("kbkCallID");
				eRPCallMemoInformationDto.kbkCreatedBy = dataTable.Rows[i].Field<string>("kbkCreatedBy");
				eRPCallMemoInformationDto.kbkCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbkCreatedDate");
				eRPCallMemoInformationDto.kbkUniqueID = dataTable.Rows[i].Field<Guid>("kbkUniqueID");
				eRPCallMemoInformationDto.kbkLongDescriptionRtf = dataTable.Rows[i].Field<string>("kbkLongDescriptionRtf");
				eRPCallMemoInformationDto.kbkLongDescriptionText = dataTable.Rows[i].Field<string>("kbkLongDescriptionText");
				eRPCallMemoInformationDto.kbkMemoDate = dataTable.Rows[i].Field<DateTime?>("kbkMemoDate");
				eRPCallMemoInformationDto.kbkRowVersion = dataTable.Rows[i].Field<byte[]>("kbkRowVersion");
				eRPCallMemoInformationDto.kbkCallMemoID = dataTable.Rows[i].Field<short>("kbkCallMemoID");
				eRPCallMemoInformationDto.kbkShortDescription = dataTable.Rows[i].Field<string>("kbkShortDescription");
				eRPCallMemoInformationDto.kbkShowInCalls = dataTable.Rows[i].Field<bool>("kbkShowInCalls");
				eRPCallMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCallMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCallMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCallMemoInformationDto> GetCallMemo(Guid callMemoId)
	{
		ERPCallMemoInformationDto eRPCallMemoInformationDto = new ERPCallMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"kbkCallID", "kbkCreatedBy", "kbkCreatedDate", "kbkUniqueID", "kbkLongDescriptionRtf", "kbkLongDescriptionText", "kbkMemoDate", "kbkRowVersion", "kbkCallMemoID", "kbkShortDescription",
			"kbkShowInCalls"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("kbkUniqueID|C", callMemoId);
		AddCustomFieldsToSelectList("CallMemos");
		using (DataTable dataTable = GetAsDataTable("CallMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCallMemoInformationDto);
			}
			eRPCallMemoInformationDto.kbkCallID = dataTable.Rows[0].Field<string>("kbkCallID");
			eRPCallMemoInformationDto.kbkCreatedBy = dataTable.Rows[0].Field<string>("kbkCreatedBy");
			eRPCallMemoInformationDto.kbkCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbkCreatedDate");
			eRPCallMemoInformationDto.kbkUniqueID = dataTable.Rows[0].Field<Guid>("kbkUniqueID");
			eRPCallMemoInformationDto.kbkLongDescriptionRtf = dataTable.Rows[0].Field<string>("kbkLongDescriptionRtf");
			eRPCallMemoInformationDto.kbkLongDescriptionText = dataTable.Rows[0].Field<string>("kbkLongDescriptionText");
			eRPCallMemoInformationDto.kbkMemoDate = dataTable.Rows[0].Field<DateTime?>("kbkMemoDate");
			eRPCallMemoInformationDto.kbkRowVersion = dataTable.Rows[0].Field<byte[]>("kbkRowVersion");
			eRPCallMemoInformationDto.kbkCallMemoID = dataTable.Rows[0].Field<short>("kbkCallMemoID");
			eRPCallMemoInformationDto.kbkShortDescription = dataTable.Rows[0].Field<string>("kbkShortDescription");
			eRPCallMemoInformationDto.kbkShowInCalls = dataTable.Rows[0].Field<bool>("kbkShowInCalls");
			eRPCallMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCallMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCallMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCallMemo(ERPCallMemoDto callMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CallMemos WHERE kbkUniqueID = " + M1Util.ConvertToLinq(callMemo.kbkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kbkCallID"] = callMemo.kbkCallID.ToUpper();
				dataRow["kbkCallMemoID"] = callMemo.kbkCallMemoID;
				callMemo.kbkUniqueID = ((callMemo.kbkUniqueID == Guid.Empty) ? Guid.NewGuid() : callMemo.kbkUniqueID);
				dataRow["kbkUniqueID"] = callMemo.kbkUniqueID;
				dataRow["kbkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kbkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CallMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (callMemo.kbkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CallMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kbkRowVersion"], callMemo.kbkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CallMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CallMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["kbkLongDescriptionRtf"] = callMemo.kbkLongDescriptionRtf ?? dataRow["kbkLongDescriptionRtf"];
			dataRow["kbkLongDescriptionText"] = callMemo.kbkLongDescriptionText ?? dataRow["kbkLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? kbkMemoDate = callMemo.kbkMemoDate;
			dataRow2["kbkMemoDate"] = (kbkMemoDate.HasValue ? ((object)kbkMemoDate.GetValueOrDefault()) : dataRow["kbkMemoDate"]);
			dataRow["kbkShortDescription"] = callMemo.kbkShortDescription;
			dataRow["kbkShowInCalls"] = callMemo.kbkShowInCalls;
			if (callMemo.CustomFields != null && callMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in callMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CallMemo [{callMemo.kbkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CallMemo [{callMemo.kbkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
