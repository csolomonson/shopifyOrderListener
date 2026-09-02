using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPProductCategoryLineRepository : APIBaseRepository, IERPProductCategoryLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPProductCategoryLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProductCategoryLineExist(Guid productCategoryLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("insUniqueID|C", productCategoryLineId);
		base.selectList.Add("insUniqueID");
		return Task.FromResult(GetAsObject("ProductCategoryLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProductCategoryLineInformationDto>> GetAllProductCategoryLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProductCategoryLineInformationDto> collection = new List<ERPProductCategoryLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"insCreatedBy", "insCreatedDate", "insDescription", "insUniqueID", "insImageFilePath", "insInactiveDate", "insInactive", "insLevel", "insParentLineID", "insProductCategoryID",
			"INSRowVersion", "insProductCategoryLineID", "insStructureCode", "insStructureID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProductCategoryLines");
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
		using (DataTable dataTable = GetAsDataTable("ProductCategoryLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProductCategoryLineInformationDto eRPProductCategoryLineInformationDto = new ERPProductCategoryLineInformationDto();
				eRPProductCategoryLineInformationDto.insCreatedBy = dataTable.Rows[i].Field<string>("insCreatedBy");
				eRPProductCategoryLineInformationDto.insCreatedDate = dataTable.Rows[i].Field<DateTime?>("insCreatedDate");
				eRPProductCategoryLineInformationDto.insDescription = dataTable.Rows[i].Field<string>("insDescription");
				eRPProductCategoryLineInformationDto.insUniqueID = dataTable.Rows[i].Field<Guid>("insUniqueID");
				eRPProductCategoryLineInformationDto.insImageFilePath = dataTable.Rows[i].Field<string>("insImageFilePath");
				eRPProductCategoryLineInformationDto.insInactiveDate = dataTable.Rows[i].Field<DateTime?>("insInactiveDate");
				eRPProductCategoryLineInformationDto.insInactive = dataTable.Rows[i].Field<bool>("insInactive");
				eRPProductCategoryLineInformationDto.insLevel = dataTable.Rows[i].Field<byte>("insLevel");
				eRPProductCategoryLineInformationDto.insParentLineID = dataTable.Rows[i].Field<short>("insParentLineID");
				eRPProductCategoryLineInformationDto.insProductCategoryID = dataTable.Rows[i].Field<string>("insProductCategoryID");
				eRPProductCategoryLineInformationDto.INSRowVersion = dataTable.Rows[i].Field<byte[]>("INSRowVersion");
				eRPProductCategoryLineInformationDto.insProductCategoryLineID = dataTable.Rows[i].Field<short>("insProductCategoryLineID");
				eRPProductCategoryLineInformationDto.insStructureCode = dataTable.Rows[i].Field<string>("insStructureCode");
				eRPProductCategoryLineInformationDto.insStructureID = dataTable.Rows[i].Field<string>("insStructureID");
				eRPProductCategoryLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProductCategoryLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProductCategoryLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProductCategoryLineInformationDto> GetProductCategoryLine(Guid productCategoryLineId)
	{
		ERPProductCategoryLineInformationDto eRPProductCategoryLineInformationDto = new ERPProductCategoryLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"insCreatedBy", "insCreatedDate", "insDescription", "insUniqueID", "insImageFilePath", "insInactiveDate", "insInactive", "insLevel", "insParentLineID", "insProductCategoryID",
			"INSRowVersion", "insProductCategoryLineID", "insStructureCode", "insStructureID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("insUniqueID|C", productCategoryLineId);
		AddCustomFieldsToSelectList("ProductCategoryLines");
		using (DataTable dataTable = GetAsDataTable("ProductCategoryLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProductCategoryLineInformationDto);
			}
			eRPProductCategoryLineInformationDto.insCreatedBy = dataTable.Rows[0].Field<string>("insCreatedBy");
			eRPProductCategoryLineInformationDto.insCreatedDate = dataTable.Rows[0].Field<DateTime?>("insCreatedDate");
			eRPProductCategoryLineInformationDto.insDescription = dataTable.Rows[0].Field<string>("insDescription");
			eRPProductCategoryLineInformationDto.insUniqueID = dataTable.Rows[0].Field<Guid>("insUniqueID");
			eRPProductCategoryLineInformationDto.insImageFilePath = dataTable.Rows[0].Field<string>("insImageFilePath");
			eRPProductCategoryLineInformationDto.insInactiveDate = dataTable.Rows[0].Field<DateTime?>("insInactiveDate");
			eRPProductCategoryLineInformationDto.insInactive = dataTable.Rows[0].Field<bool>("insInactive");
			eRPProductCategoryLineInformationDto.insLevel = dataTable.Rows[0].Field<byte>("insLevel");
			eRPProductCategoryLineInformationDto.insParentLineID = dataTable.Rows[0].Field<short>("insParentLineID");
			eRPProductCategoryLineInformationDto.insProductCategoryID = dataTable.Rows[0].Field<string>("insProductCategoryID");
			eRPProductCategoryLineInformationDto.INSRowVersion = dataTable.Rows[0].Field<byte[]>("INSRowVersion");
			eRPProductCategoryLineInformationDto.insProductCategoryLineID = dataTable.Rows[0].Field<short>("insProductCategoryLineID");
			eRPProductCategoryLineInformationDto.insStructureCode = dataTable.Rows[0].Field<string>("insStructureCode");
			eRPProductCategoryLineInformationDto.insStructureID = dataTable.Rows[0].Field<string>("insStructureID");
			eRPProductCategoryLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProductCategoryLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProductCategoryLineInformationDto);
	}
}
