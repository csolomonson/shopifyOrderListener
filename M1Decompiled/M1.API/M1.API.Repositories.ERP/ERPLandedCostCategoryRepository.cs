using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPLandedCostCategoryRepository : APIBaseRepository, IERPLandedCostCategoryRepository, IAPIBaseRepository, IDisposable
{
	public ERPLandedCostCategoryRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLandedCostCategoryExist(Guid landedCostCategoryId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmaUniqueID|C", landedCostCategoryId);
		base.selectList.Add("rmaUniqueID");
		return Task.FromResult(GetAsObject("LandedCostCategories", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLandedCostCategoryInformationDto>> GetAllLandedCostCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLandedCostCategoryInformationDto> collection = new List<ERPLandedCostCategoryInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"rmaCategoryType", "rmaLandedCostCategoryID", "rmaCreatedBy", "rmaCreatedDate", "rmaDescription", "rmaUniqueID", "rmaExpenseSplitPercentTotal", "rmaDefault", "rmaLandedCostMethod", "rmaRowVersion",
			"rmaSupplierLocationID", "rmaSupplierOrganizationID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LandedCostCategories");
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
		using (DataTable dataTable = GetAsDataTable("LandedCostCategories", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLandedCostCategoryInformationDto eRPLandedCostCategoryInformationDto = new ERPLandedCostCategoryInformationDto();
				eRPLandedCostCategoryInformationDto.rmaCategoryType = dataTable.Rows[i].Field<byte>("rmaCategoryType");
				eRPLandedCostCategoryInformationDto.rmaLandedCostCategoryID = dataTable.Rows[i].Field<string>("rmaLandedCostCategoryID");
				eRPLandedCostCategoryInformationDto.rmaCreatedBy = dataTable.Rows[i].Field<string>("rmaCreatedBy");
				eRPLandedCostCategoryInformationDto.rmaCreatedDate = dataTable.Rows[i].Field<DateTime?>("rmaCreatedDate");
				eRPLandedCostCategoryInformationDto.rmaDescription = dataTable.Rows[i].Field<string>("rmaDescription");
				eRPLandedCostCategoryInformationDto.rmaUniqueID = dataTable.Rows[i].Field<Guid>("rmaUniqueID");
				eRPLandedCostCategoryInformationDto.rmaExpenseSplitPercentTotal = dataTable.Rows[i].Field<decimal>("rmaExpenseSplitPercentTotal");
				eRPLandedCostCategoryInformationDto.rmaDefault = dataTable.Rows[i].Field<bool>("rmaDefault");
				eRPLandedCostCategoryInformationDto.rmaLandedCostMethod = dataTable.Rows[i].Field<byte>("rmaLandedCostMethod");
				eRPLandedCostCategoryInformationDto.rmaRowVersion = dataTable.Rows[i].Field<byte[]>("rmaRowVersion");
				eRPLandedCostCategoryInformationDto.rmaSupplierLocationID = dataTable.Rows[i].Field<string>("rmaSupplierLocationID");
				eRPLandedCostCategoryInformationDto.rmaSupplierOrganizationID = dataTable.Rows[i].Field<string>("rmaSupplierOrganizationID");
				eRPLandedCostCategoryInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLandedCostCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLandedCostCategoryInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLandedCostCategoryInformationDto> GetLandedCostCategory(Guid landedCostCategoryId)
	{
		ERPLandedCostCategoryInformationDto eRPLandedCostCategoryInformationDto = new ERPLandedCostCategoryInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"rmaCategoryType", "rmaLandedCostCategoryID", "rmaCreatedBy", "rmaCreatedDate", "rmaDescription", "rmaUniqueID", "rmaExpenseSplitPercentTotal", "rmaDefault", "rmaLandedCostMethod", "rmaRowVersion",
			"rmaSupplierLocationID", "rmaSupplierOrganizationID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rmaUniqueID|C", landedCostCategoryId);
		AddCustomFieldsToSelectList("LandedCostCategories");
		using (DataTable dataTable = GetAsDataTable("LandedCostCategories", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLandedCostCategoryInformationDto);
			}
			eRPLandedCostCategoryInformationDto.rmaCategoryType = dataTable.Rows[0].Field<byte>("rmaCategoryType");
			eRPLandedCostCategoryInformationDto.rmaLandedCostCategoryID = dataTable.Rows[0].Field<string>("rmaLandedCostCategoryID");
			eRPLandedCostCategoryInformationDto.rmaCreatedBy = dataTable.Rows[0].Field<string>("rmaCreatedBy");
			eRPLandedCostCategoryInformationDto.rmaCreatedDate = dataTable.Rows[0].Field<DateTime?>("rmaCreatedDate");
			eRPLandedCostCategoryInformationDto.rmaDescription = dataTable.Rows[0].Field<string>("rmaDescription");
			eRPLandedCostCategoryInformationDto.rmaUniqueID = dataTable.Rows[0].Field<Guid>("rmaUniqueID");
			eRPLandedCostCategoryInformationDto.rmaExpenseSplitPercentTotal = dataTable.Rows[0].Field<decimal>("rmaExpenseSplitPercentTotal");
			eRPLandedCostCategoryInformationDto.rmaDefault = dataTable.Rows[0].Field<bool>("rmaDefault");
			eRPLandedCostCategoryInformationDto.rmaLandedCostMethod = dataTable.Rows[0].Field<byte>("rmaLandedCostMethod");
			eRPLandedCostCategoryInformationDto.rmaRowVersion = dataTable.Rows[0].Field<byte[]>("rmaRowVersion");
			eRPLandedCostCategoryInformationDto.rmaSupplierLocationID = dataTable.Rows[0].Field<string>("rmaSupplierLocationID");
			eRPLandedCostCategoryInformationDto.rmaSupplierOrganizationID = dataTable.Rows[0].Field<string>("rmaSupplierOrganizationID");
			eRPLandedCostCategoryInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLandedCostCategoryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLandedCostCategoryInformationDto);
	}
}
