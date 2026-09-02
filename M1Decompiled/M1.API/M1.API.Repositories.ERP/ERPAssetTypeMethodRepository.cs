using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPAssetTypeMethodRepository : APIBaseRepository, IERPAssetTypeMethodRepository, IAPIBaseRepository, IDisposable
{
	public ERPAssetTypeMethodRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAssetTypeMethodExist(Guid assetTypeMethodId)
	{
		InitializeParameterLists();
		base.filterList.Add("famUniqueID|C", assetTypeMethodId);
		base.selectList.Add("famUniqueID");
		return Task.FromResult(GetAsObject("AssetTypeMethods", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAssetTypeMethodInformationDto>> GetAllAssetTypeMethods(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAssetTypeMethodInformationDto> collection = new List<ERPAssetTypeMethodInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"famAssetTypeID", "famBookDepreciationMethod", "famBookMultiplier", "famCalculationMethod", "famCreatedBy", "famCreatedDate", "famUniqueID", "famCurrentMethod", "famMonthCalculationType", "famRowVersion",
			"famAssetTypeMethodID", "famStartDate", "famTaxDepreciationMethod", "famTaxMultiplier"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AssetTypeMethods");
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
		using (DataTable dataTable = GetAsDataTable("AssetTypeMethods", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAssetTypeMethodInformationDto eRPAssetTypeMethodInformationDto = new ERPAssetTypeMethodInformationDto();
				eRPAssetTypeMethodInformationDto.famAssetTypeID = dataTable.Rows[i].Field<string>("famAssetTypeID");
				eRPAssetTypeMethodInformationDto.famBookDepreciationMethod = dataTable.Rows[i].Field<string>("famBookDepreciationMethod");
				eRPAssetTypeMethodInformationDto.famBookMultiplier = dataTable.Rows[i].Field<decimal>("famBookMultiplier");
				eRPAssetTypeMethodInformationDto.famCalculationMethod = dataTable.Rows[i].Field<string>("famCalculationMethod");
				eRPAssetTypeMethodInformationDto.famCreatedBy = dataTable.Rows[i].Field<string>("famCreatedBy");
				eRPAssetTypeMethodInformationDto.famCreatedDate = dataTable.Rows[i].Field<DateTime?>("famCreatedDate");
				eRPAssetTypeMethodInformationDto.famUniqueID = dataTable.Rows[i].Field<Guid>("famUniqueID");
				eRPAssetTypeMethodInformationDto.famCurrentMethod = dataTable.Rows[i].Field<bool>("famCurrentMethod");
				eRPAssetTypeMethodInformationDto.famMonthCalculationType = dataTable.Rows[i].Field<string>("famMonthCalculationType");
				eRPAssetTypeMethodInformationDto.famRowVersion = dataTable.Rows[i].Field<byte[]>("famRowVersion");
				eRPAssetTypeMethodInformationDto.famAssetTypeMethodID = dataTable.Rows[i].Field<short>("famAssetTypeMethodID");
				eRPAssetTypeMethodInformationDto.famStartDate = dataTable.Rows[i].Field<DateTime?>("famStartDate");
				eRPAssetTypeMethodInformationDto.famTaxDepreciationMethod = dataTable.Rows[i].Field<string>("famTaxDepreciationMethod");
				eRPAssetTypeMethodInformationDto.famTaxMultiplier = dataTable.Rows[i].Field<decimal>("famTaxMultiplier");
				eRPAssetTypeMethodInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAssetTypeMethodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAssetTypeMethodInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAssetTypeMethodInformationDto> GetAssetTypeMethod(Guid assetTypeMethodId)
	{
		ERPAssetTypeMethodInformationDto eRPAssetTypeMethodInformationDto = new ERPAssetTypeMethodInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"famAssetTypeID", "famBookDepreciationMethod", "famBookMultiplier", "famCalculationMethod", "famCreatedBy", "famCreatedDate", "famUniqueID", "famCurrentMethod", "famMonthCalculationType", "famRowVersion",
			"famAssetTypeMethodID", "famStartDate", "famTaxDepreciationMethod", "famTaxMultiplier"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("famUniqueID|C", assetTypeMethodId);
		AddCustomFieldsToSelectList("AssetTypeMethods");
		using (DataTable dataTable = GetAsDataTable("AssetTypeMethods", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAssetTypeMethodInformationDto);
			}
			eRPAssetTypeMethodInformationDto.famAssetTypeID = dataTable.Rows[0].Field<string>("famAssetTypeID");
			eRPAssetTypeMethodInformationDto.famBookDepreciationMethod = dataTable.Rows[0].Field<string>("famBookDepreciationMethod");
			eRPAssetTypeMethodInformationDto.famBookMultiplier = dataTable.Rows[0].Field<decimal>("famBookMultiplier");
			eRPAssetTypeMethodInformationDto.famCalculationMethod = dataTable.Rows[0].Field<string>("famCalculationMethod");
			eRPAssetTypeMethodInformationDto.famCreatedBy = dataTable.Rows[0].Field<string>("famCreatedBy");
			eRPAssetTypeMethodInformationDto.famCreatedDate = dataTable.Rows[0].Field<DateTime?>("famCreatedDate");
			eRPAssetTypeMethodInformationDto.famUniqueID = dataTable.Rows[0].Field<Guid>("famUniqueID");
			eRPAssetTypeMethodInformationDto.famCurrentMethod = dataTable.Rows[0].Field<bool>("famCurrentMethod");
			eRPAssetTypeMethodInformationDto.famMonthCalculationType = dataTable.Rows[0].Field<string>("famMonthCalculationType");
			eRPAssetTypeMethodInformationDto.famRowVersion = dataTable.Rows[0].Field<byte[]>("famRowVersion");
			eRPAssetTypeMethodInformationDto.famAssetTypeMethodID = dataTable.Rows[0].Field<short>("famAssetTypeMethodID");
			eRPAssetTypeMethodInformationDto.famStartDate = dataTable.Rows[0].Field<DateTime?>("famStartDate");
			eRPAssetTypeMethodInformationDto.famTaxDepreciationMethod = dataTable.Rows[0].Field<string>("famTaxDepreciationMethod");
			eRPAssetTypeMethodInformationDto.famTaxMultiplier = dataTable.Rows[0].Field<decimal>("famTaxMultiplier");
			eRPAssetTypeMethodInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAssetTypeMethodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAssetTypeMethodInformationDto);
	}
}
