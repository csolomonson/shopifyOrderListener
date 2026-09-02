using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPChangeLogRepository : APIBaseRepository, IERPChangeLogRepository, IAPIBaseRepository, IDisposable
{
	public ERPChangeLogRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesChangeLogExist(Guid changeLogId)
	{
		InitializeParameterLists();
		base.filterList.Add("xagUniqueID|C", changeLogId);
		base.selectList.Add("xagUniqueID");
		return Task.FromResult(GetAsObject("ChangeLog", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPChangeLogInformationDto>> GetAllChangeLog(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPChangeLogInformationDto> collection = new List<ERPChangeLogInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "xagChangeDate", "xagChangeType", "xagChangeUserID", "xagRowVersion", "xagChangeLogID", "xagTableKeyValues", "xagTableName", "xagTableNewValues", "xagTableOldValues", "xagTableUniqueID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ChangeLog");
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
		using (DataTable dataTable = GetAsDataTable("ChangeLog", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPChangeLogInformationDto eRPChangeLogInformationDto = new ERPChangeLogInformationDto();
				eRPChangeLogInformationDto.xagChangeDate = dataTable.Rows[i].Field<DateTime?>("xagChangeDate");
				eRPChangeLogInformationDto.xagChangeType = dataTable.Rows[i].Field<string>("xagChangeType");
				eRPChangeLogInformationDto.xagChangeUserID = dataTable.Rows[i].Field<string>("xagChangeUserID");
				eRPChangeLogInformationDto.xagRowVersion = dataTable.Rows[i].Field<byte[]>("xagRowVersion");
				eRPChangeLogInformationDto.xagChangeLogID = dataTable.Rows[i].Field<int>("xagChangeLogID");
				eRPChangeLogInformationDto.xagTableKeyValues = dataTable.Rows[i].Field<string>("xagTableKeyValues");
				eRPChangeLogInformationDto.xagTableName = dataTable.Rows[i].Field<string>("xagTableName");
				eRPChangeLogInformationDto.xagTableNewValues = dataTable.Rows[i].Field<string>("xagTableNewValues");
				eRPChangeLogInformationDto.xagTableOldValues = dataTable.Rows[i].Field<string>("xagTableOldValues");
				eRPChangeLogInformationDto.xagTableUniqueID = dataTable.Rows[i].Field<Guid>("xagTableUniqueID");
				eRPChangeLogInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPChangeLogInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPChangeLogInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPChangeLogInformationDto> GetChangeLog(Guid changeLogId)
	{
		ERPChangeLogInformationDto eRPChangeLogInformationDto = new ERPChangeLogInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "xagChangeDate", "xagChangeType", "xagChangeUserID", "xagRowVersion", "xagChangeLogID", "xagTableKeyValues", "xagTableName", "xagTableNewValues", "xagTableOldValues", "xagTableUniqueID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xagUniqueID|C", changeLogId);
		AddCustomFieldsToSelectList("ChangeLog");
		using (DataTable dataTable = GetAsDataTable("ChangeLog", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPChangeLogInformationDto);
			}
			eRPChangeLogInformationDto.xagChangeDate = dataTable.Rows[0].Field<DateTime?>("xagChangeDate");
			eRPChangeLogInformationDto.xagChangeType = dataTable.Rows[0].Field<string>("xagChangeType");
			eRPChangeLogInformationDto.xagChangeUserID = dataTable.Rows[0].Field<string>("xagChangeUserID");
			eRPChangeLogInformationDto.xagRowVersion = dataTable.Rows[0].Field<byte[]>("xagRowVersion");
			eRPChangeLogInformationDto.xagChangeLogID = dataTable.Rows[0].Field<int>("xagChangeLogID");
			eRPChangeLogInformationDto.xagTableKeyValues = dataTable.Rows[0].Field<string>("xagTableKeyValues");
			eRPChangeLogInformationDto.xagTableName = dataTable.Rows[0].Field<string>("xagTableName");
			eRPChangeLogInformationDto.xagTableNewValues = dataTable.Rows[0].Field<string>("xagTableNewValues");
			eRPChangeLogInformationDto.xagTableOldValues = dataTable.Rows[0].Field<string>("xagTableOldValues");
			eRPChangeLogInformationDto.xagTableUniqueID = dataTable.Rows[0].Field<Guid>("xagTableUniqueID");
			eRPChangeLogInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPChangeLogInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPChangeLogInformationDto);
	}
}
