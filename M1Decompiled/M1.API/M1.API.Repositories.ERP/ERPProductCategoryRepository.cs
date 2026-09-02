using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPProductCategoryRepository : APIBaseRepository, IERPProductCategoryRepository, IAPIBaseRepository, IDisposable
{
	public ERPProductCategoryRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProductCategoryExist(Guid productCategoryId)
	{
		InitializeParameterLists();
		base.filterList.Add("incUniqueID|C", productCategoryId);
		base.selectList.Add("incUniqueID");
		return Task.FromResult(GetAsObject("ProductCategories", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProductCategoryInformationDto>> GetAllProductCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProductCategoryInformationDto> collection = new List<ERPProductCategoryInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"incProductCategoryID", "incCreatedBy", "incCreatedDate", "incDescription", "incUniqueID", "incImageFilePath", "incInactiveDate", "incInactive", "INCRowVersion", "incStructureCode",
			"incStructureID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProductCategories");
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
		using (DataTable dataTable = GetAsDataTable("ProductCategories", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProductCategoryInformationDto eRPProductCategoryInformationDto = new ERPProductCategoryInformationDto();
				eRPProductCategoryInformationDto.incProductCategoryID = dataTable.Rows[i].Field<string>("incProductCategoryID");
				eRPProductCategoryInformationDto.incCreatedBy = dataTable.Rows[i].Field<string>("incCreatedBy");
				eRPProductCategoryInformationDto.incCreatedDate = dataTable.Rows[i].Field<DateTime?>("incCreatedDate");
				eRPProductCategoryInformationDto.incDescription = dataTable.Rows[i].Field<string>("incDescription");
				eRPProductCategoryInformationDto.incUniqueID = dataTable.Rows[i].Field<Guid>("incUniqueID");
				eRPProductCategoryInformationDto.incImageFilePath = dataTable.Rows[i].Field<string>("incImageFilePath");
				eRPProductCategoryInformationDto.incInactiveDate = dataTable.Rows[i].Field<DateTime?>("incInactiveDate");
				eRPProductCategoryInformationDto.incInactive = dataTable.Rows[i].Field<bool>("incInactive");
				eRPProductCategoryInformationDto.INCRowVersion = dataTable.Rows[i].Field<byte[]>("INCRowVersion");
				eRPProductCategoryInformationDto.incStructureCode = dataTable.Rows[i].Field<string>("incStructureCode");
				eRPProductCategoryInformationDto.incStructureID = dataTable.Rows[i].Field<string>("incStructureID");
				eRPProductCategoryInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProductCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProductCategoryInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProductCategoryInformationDto> GetProductCategory(Guid productCategoryId)
	{
		ERPProductCategoryInformationDto eRPProductCategoryInformationDto = new ERPProductCategoryInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"incProductCategoryID", "incCreatedBy", "incCreatedDate", "incDescription", "incUniqueID", "incImageFilePath", "incInactiveDate", "incInactive", "INCRowVersion", "incStructureCode",
			"incStructureID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("incUniqueID|C", productCategoryId);
		AddCustomFieldsToSelectList("ProductCategories");
		using (DataTable dataTable = GetAsDataTable("ProductCategories", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProductCategoryInformationDto);
			}
			eRPProductCategoryInformationDto.incProductCategoryID = dataTable.Rows[0].Field<string>("incProductCategoryID");
			eRPProductCategoryInformationDto.incCreatedBy = dataTable.Rows[0].Field<string>("incCreatedBy");
			eRPProductCategoryInformationDto.incCreatedDate = dataTable.Rows[0].Field<DateTime?>("incCreatedDate");
			eRPProductCategoryInformationDto.incDescription = dataTable.Rows[0].Field<string>("incDescription");
			eRPProductCategoryInformationDto.incUniqueID = dataTable.Rows[0].Field<Guid>("incUniqueID");
			eRPProductCategoryInformationDto.incImageFilePath = dataTable.Rows[0].Field<string>("incImageFilePath");
			eRPProductCategoryInformationDto.incInactiveDate = dataTable.Rows[0].Field<DateTime?>("incInactiveDate");
			eRPProductCategoryInformationDto.incInactive = dataTable.Rows[0].Field<bool>("incInactive");
			eRPProductCategoryInformationDto.INCRowVersion = dataTable.Rows[0].Field<byte[]>("INCRowVersion");
			eRPProductCategoryInformationDto.incStructureCode = dataTable.Rows[0].Field<string>("incStructureCode");
			eRPProductCategoryInformationDto.incStructureID = dataTable.Rows[0].Field<string>("incStructureID");
			eRPProductCategoryInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProductCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProductCategoryInformationDto);
	}
}
