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

public class ERPToolMemoRepository : APIBaseRepository, IERPToolMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPToolMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesToolMemoExist(Guid toolMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("xtmUniqueID|C", toolMemoId);
		base.selectList.Add("xtmUniqueID");
		return Task.FromResult(GetAsObject("ToolMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPToolMemoInformationDto>> GetAllToolMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPToolMemoInformationDto> collection = new List<ERPToolMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "xtmCreatedBy", "xtmCreatedDate", "xtmUniqueID", "xtmLongDescriptionRtf", "xtmLongDescriptionText", "xtmMemoDate", "xtmRowVersion", "xtmToolMemoID", "xtmShortDescription", "xtmToolID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ToolMemos");
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
		using (DataTable dataTable = GetAsDataTable("ToolMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPToolMemoInformationDto eRPToolMemoInformationDto = new ERPToolMemoInformationDto();
				eRPToolMemoInformationDto.xtmCreatedBy = dataTable.Rows[i].Field<string>("xtmCreatedBy");
				eRPToolMemoInformationDto.xtmCreatedDate = dataTable.Rows[i].Field<DateTime?>("xtmCreatedDate");
				eRPToolMemoInformationDto.xtmUniqueID = dataTable.Rows[i].Field<Guid>("xtmUniqueID");
				eRPToolMemoInformationDto.xtmLongDescriptionRtf = dataTable.Rows[i].Field<string>("xtmLongDescriptionRtf");
				eRPToolMemoInformationDto.xtmLongDescriptionText = dataTable.Rows[i].Field<string>("xtmLongDescriptionText");
				eRPToolMemoInformationDto.xtmMemoDate = dataTable.Rows[i].Field<DateTime?>("xtmMemoDate");
				eRPToolMemoInformationDto.xtmRowVersion = dataTable.Rows[i].Field<byte[]>("xtmRowVersion");
				eRPToolMemoInformationDto.xtmToolMemoID = dataTable.Rows[i].Field<short>("xtmToolMemoID");
				eRPToolMemoInformationDto.xtmShortDescription = dataTable.Rows[i].Field<string>("xtmShortDescription");
				eRPToolMemoInformationDto.xtmToolID = dataTable.Rows[i].Field<string>("xtmToolID");
				eRPToolMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPToolMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPToolMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPToolMemoInformationDto> GetToolMemo(Guid toolMemoId)
	{
		ERPToolMemoInformationDto eRPToolMemoInformationDto = new ERPToolMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "xtmCreatedBy", "xtmCreatedDate", "xtmUniqueID", "xtmLongDescriptionRtf", "xtmLongDescriptionText", "xtmMemoDate", "xtmRowVersion", "xtmToolMemoID", "xtmShortDescription", "xtmToolID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xtmUniqueID|C", toolMemoId);
		AddCustomFieldsToSelectList("ToolMemos");
		using (DataTable dataTable = GetAsDataTable("ToolMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPToolMemoInformationDto);
			}
			eRPToolMemoInformationDto.xtmCreatedBy = dataTable.Rows[0].Field<string>("xtmCreatedBy");
			eRPToolMemoInformationDto.xtmCreatedDate = dataTable.Rows[0].Field<DateTime?>("xtmCreatedDate");
			eRPToolMemoInformationDto.xtmUniqueID = dataTable.Rows[0].Field<Guid>("xtmUniqueID");
			eRPToolMemoInformationDto.xtmLongDescriptionRtf = dataTable.Rows[0].Field<string>("xtmLongDescriptionRtf");
			eRPToolMemoInformationDto.xtmLongDescriptionText = dataTable.Rows[0].Field<string>("xtmLongDescriptionText");
			eRPToolMemoInformationDto.xtmMemoDate = dataTable.Rows[0].Field<DateTime?>("xtmMemoDate");
			eRPToolMemoInformationDto.xtmRowVersion = dataTable.Rows[0].Field<byte[]>("xtmRowVersion");
			eRPToolMemoInformationDto.xtmToolMemoID = dataTable.Rows[0].Field<short>("xtmToolMemoID");
			eRPToolMemoInformationDto.xtmShortDescription = dataTable.Rows[0].Field<string>("xtmShortDescription");
			eRPToolMemoInformationDto.xtmToolID = dataTable.Rows[0].Field<string>("xtmToolID");
			eRPToolMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPToolMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPToolMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveToolMemo(ERPToolMemoDto toolMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ToolMemos WHERE xtmUniqueID = " + M1Util.ConvertToLinq(toolMemo.xtmUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xtmToolID"] = toolMemo.xtmToolID.ToUpper();
				dataRow["xtmToolMemoID"] = toolMemo.xtmToolMemoID;
				toolMemo.xtmUniqueID = ((toolMemo.xtmUniqueID == Guid.Empty) ? Guid.NewGuid() : toolMemo.xtmUniqueID);
				dataRow["xtmUniqueID"] = toolMemo.xtmUniqueID;
				dataRow["xtmCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xtmCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ToolMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (toolMemo.xtmRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ToolMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xtmRowVersion"], toolMemo.xtmRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ToolMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ToolMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xtmLongDescriptionRtf"] = toolMemo.xtmLongDescriptionRtf ?? dataRow["xtmLongDescriptionRtf"];
			dataRow["xtmLongDescriptionText"] = toolMemo.xtmLongDescriptionText ?? dataRow["xtmLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? xtmMemoDate = toolMemo.xtmMemoDate;
			dataRow2["xtmMemoDate"] = (xtmMemoDate.HasValue ? ((object)xtmMemoDate.GetValueOrDefault()) : dataRow["xtmMemoDate"]);
			dataRow["xtmShortDescription"] = toolMemo.xtmShortDescription;
			if (toolMemo.CustomFields != null && toolMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in toolMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ToolMemo [{toolMemo.xtmUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ToolMemo [{toolMemo.xtmUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
