using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPToolCategoryRepository : APIBaseRepository, IERPToolCategoryRepository, IAPIBaseRepository, IDisposable
{
	public ERPToolCategoryRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesToolCategoryExist(Guid toolCategoryId)
	{
		InitializeParameterLists();
		base.filterList.Add("xtcUniqueID|C", toolCategoryId);
		base.selectList.Add("xtcUniqueID");
		return Task.FromResult(GetAsObject("ToolCategories", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPToolCategoryInformationDto>> GetAllToolCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPToolCategoryInformationDto> collection = new List<ERPToolCategoryInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "xtcToolCategoryID", "xtcCreatedBy", "xtcCreatedDate", "xtcDescription", "xtcUniqueID", "xtcInactiveDate", "xtcInactive", "xtcRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ToolCategories");
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
		using (DataTable dataTable = GetAsDataTable("ToolCategories", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPToolCategoryInformationDto eRPToolCategoryInformationDto = new ERPToolCategoryInformationDto();
				eRPToolCategoryInformationDto.xtcToolCategoryID = dataTable.Rows[i].Field<string>("xtcToolCategoryID");
				eRPToolCategoryInformationDto.xtcCreatedBy = dataTable.Rows[i].Field<string>("xtcCreatedBy");
				eRPToolCategoryInformationDto.xtcCreatedDate = dataTable.Rows[i].Field<DateTime?>("xtcCreatedDate");
				eRPToolCategoryInformationDto.xtcDescription = dataTable.Rows[i].Field<string>("xtcDescription");
				eRPToolCategoryInformationDto.xtcUniqueID = dataTable.Rows[i].Field<Guid>("xtcUniqueID");
				eRPToolCategoryInformationDto.xtcInactiveDate = dataTable.Rows[i].Field<DateTime?>("xtcInactiveDate");
				eRPToolCategoryInformationDto.xtcInactive = dataTable.Rows[i].Field<bool>("xtcInactive");
				eRPToolCategoryInformationDto.xtcRowVersion = dataTable.Rows[i].Field<byte[]>("xtcRowVersion");
				eRPToolCategoryInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPToolCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPToolCategoryInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPToolCategoryInformationDto> GetToolCategory(Guid toolCategoryId)
	{
		ERPToolCategoryInformationDto eRPToolCategoryInformationDto = new ERPToolCategoryInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "xtcToolCategoryID", "xtcCreatedBy", "xtcCreatedDate", "xtcDescription", "xtcUniqueID", "xtcInactiveDate", "xtcInactive", "xtcRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xtcUniqueID|C", toolCategoryId);
		AddCustomFieldsToSelectList("ToolCategories");
		using (DataTable dataTable = GetAsDataTable("ToolCategories", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPToolCategoryInformationDto);
			}
			eRPToolCategoryInformationDto.xtcToolCategoryID = dataTable.Rows[0].Field<string>("xtcToolCategoryID");
			eRPToolCategoryInformationDto.xtcCreatedBy = dataTable.Rows[0].Field<string>("xtcCreatedBy");
			eRPToolCategoryInformationDto.xtcCreatedDate = dataTable.Rows[0].Field<DateTime?>("xtcCreatedDate");
			eRPToolCategoryInformationDto.xtcDescription = dataTable.Rows[0].Field<string>("xtcDescription");
			eRPToolCategoryInformationDto.xtcUniqueID = dataTable.Rows[0].Field<Guid>("xtcUniqueID");
			eRPToolCategoryInformationDto.xtcInactiveDate = dataTable.Rows[0].Field<DateTime?>("xtcInactiveDate");
			eRPToolCategoryInformationDto.xtcInactive = dataTable.Rows[0].Field<bool>("xtcInactive");
			eRPToolCategoryInformationDto.xtcRowVersion = dataTable.Rows[0].Field<byte[]>("xtcRowVersion");
			eRPToolCategoryInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPToolCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPToolCategoryInformationDto);
	}
}
