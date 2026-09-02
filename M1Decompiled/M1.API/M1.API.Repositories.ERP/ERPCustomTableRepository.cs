using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPCustomTableRepository : APIBaseRepository, IERPCustomTableRepository, IAPIBaseRepository, IDisposable
{
	public ERPCustomTableRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
	}

	public Task<bool> DoesCustomTableRecordExist(string tableName, Guid customTableUniqueId)
	{
		InitializeParameterLists();
		if (!DoesCustomTableExist(tableName))
		{
			return Task.FromResult(result: false);
		}
		string customTablePrefix = GetCustomTablePrefix(tableName);
		if (string.IsNullOrEmpty(customTablePrefix))
		{
			return Task.FromResult(result: false);
		}
		base.filterList.Add(customTablePrefix + "UniqueID|C", customTableUniqueId);
		base.selectList.Add(customTablePrefix + "UniqueID");
		return Task.FromResult(GetAsObject(tableName, base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCustomTableInformationDto>> GetAllCustomTableRecords(string tableName, int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCustomTableInformationDto> collection = new List<ERPCustomTableInformationDto>();
		string customTablePrefix = GetCustomTablePrefix(tableName);
		if (string.IsNullOrEmpty(customTablePrefix))
		{
			Task.FromResult(collection);
		}
		InitializeParameterLists();
		List<string> allColumnsForCustomTable = GetAllColumnsForCustomTable(tableName, customTablePrefix);
		if (allColumnsForCustomTable == null || allColumnsForCustomTable.Count == 0)
		{
			return Task.FromResult(collection);
		}
		base.selectList.AddRange(allColumnsForCustomTable);
		List<string> list = new List<string>();
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, allColumnsForCustomTable.ToArray());
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, allColumnsForCustomTable.ToArray());
		}
		using (DataTable dataTable = GetAsDataTable(tableName, base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCustomTableInformationDto eRPCustomTableInformationDto = new ERPCustomTableInformationDto
				{
					CustomFields = new Dictionary<string, object>()
				};
				foreach (DataColumn column in dataTable.Columns)
				{
					eRPCustomTableInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
				}
				collection.Add(eRPCustomTableInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCustomTableInformationDto> GetCustomTableRecord(string tableName, Guid customTableUniqueId)
	{
		ERPCustomTableInformationDto eRPCustomTableInformationDto = new ERPCustomTableInformationDto();
		string customTablePrefix = GetCustomTablePrefix(tableName);
		if (string.IsNullOrEmpty(customTablePrefix))
		{
			Task.FromResult(eRPCustomTableInformationDto);
		}
		InitializeParameterLists();
		string[] collection = new string[1] { "*" };
		base.selectList.AddRange(collection);
		base.filterList.Add(customTablePrefix + "UniqueID|C", customTableUniqueId);
		using (DataTable dataTable = GetAsDataTable(tableName, base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCustomTableInformationDto);
			}
			eRPCustomTableInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				eRPCustomTableInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
			}
		}
		return Task.FromResult(eRPCustomTableInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCustomTableRecord(string tableName, ERPCustomTableDto customTable)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		string customTablePrefix = GetCustomTablePrefix(tableName);
		if (string.IsNullOrEmpty(customTablePrefix))
		{
			aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
			Task.FromResult(aPIValidationInfoDto);
		}
		if (!customTable.CustomFields.TryGetValue(customTablePrefix + "UniqueID", out var value) || !Guid.TryParse(value.ToString(), out var result))
		{
			aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
			return Task.FromResult(aPIValidationInfoDto);
		}
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM " + tableName + " WHERE " + customTablePrefix + "UniqueID = " + M1Util.ConvertToLinq(result), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow[customTablePrefix + "UniqueID"] = result;
				flag = true;
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			foreach (KeyValuePair<string, object> customField in customTable.CustomFields)
			{
				if (dataTable.Columns.Contains(customField.Key) && !(customField.Key == customTablePrefix + "UniqueID"))
				{
					dataRow[customField.Key] = customField.Value;
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CustomTable [{tableName}] record [{result}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CustomTable [{tableName}] record [{result}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
