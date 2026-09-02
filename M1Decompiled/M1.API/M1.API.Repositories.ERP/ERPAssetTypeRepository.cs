using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPAssetTypeRepository : APIBaseRepository, IERPAssetTypeRepository, IAPIBaseRepository, IDisposable
{
	public ERPAssetTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAssetTypeExist(Guid assetTypeId)
	{
		InitializeParameterLists();
		base.filterList.Add("fatUniqueID|C", assetTypeId);
		base.selectList.Add("fatUniqueID");
		return Task.FromResult(GetAsObject("AssetTypes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAssetTypeInformationDto>> GetAllAssetTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAssetTypeInformationDto> collection = new List<ERPAssetTypeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"fatAccumDeprGlAccountID", "fatAssetGlAccountID", "fatAssetTypeID", "fatCreatedBy", "fatCreatedDate", "fatDepreciationGlAccountID", "fatDescription", "fatUniqueID", "fatExpenseGlAccountID", "fatLossGlAccountID",
			"fatProfitGlAccountID", "fatRepairsGlAccountID", "fatRevaluationGlAccountID", "fatRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AssetTypes");
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
		using (DataTable dataTable = GetAsDataTable("AssetTypes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAssetTypeInformationDto eRPAssetTypeInformationDto = new ERPAssetTypeInformationDto();
				eRPAssetTypeInformationDto.fatAccumDeprGlAccountID = dataTable.Rows[i].Field<string>("fatAccumDeprGlAccountID");
				eRPAssetTypeInformationDto.fatAssetGlAccountID = dataTable.Rows[i].Field<string>("fatAssetGlAccountID");
				eRPAssetTypeInformationDto.fatAssetTypeID = dataTable.Rows[i].Field<string>("fatAssetTypeID");
				eRPAssetTypeInformationDto.fatCreatedBy = dataTable.Rows[i].Field<string>("fatCreatedBy");
				eRPAssetTypeInformationDto.fatCreatedDate = dataTable.Rows[i].Field<DateTime?>("fatCreatedDate");
				eRPAssetTypeInformationDto.fatDepreciationGlAccountID = dataTable.Rows[i].Field<string>("fatDepreciationGlAccountID");
				eRPAssetTypeInformationDto.fatDescription = dataTable.Rows[i].Field<string>("fatDescription");
				eRPAssetTypeInformationDto.fatUniqueID = dataTable.Rows[i].Field<Guid>("fatUniqueID");
				eRPAssetTypeInformationDto.fatExpenseGlAccountID = dataTable.Rows[i].Field<string>("fatExpenseGlAccountID");
				eRPAssetTypeInformationDto.fatLossGlAccountID = dataTable.Rows[i].Field<string>("fatLossGlAccountID");
				eRPAssetTypeInformationDto.fatProfitGlAccountID = dataTable.Rows[i].Field<string>("fatProfitGlAccountID");
				eRPAssetTypeInformationDto.fatRepairsGlAccountID = dataTable.Rows[i].Field<string>("fatRepairsGlAccountID");
				eRPAssetTypeInformationDto.fatRevaluationGlAccountID = dataTable.Rows[i].Field<string>("fatRevaluationGlAccountID");
				eRPAssetTypeInformationDto.fatRowVersion = dataTable.Rows[i].Field<byte[]>("fatRowVersion");
				eRPAssetTypeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAssetTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAssetTypeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAssetTypeInformationDto> GetAssetType(Guid assetTypeId)
	{
		ERPAssetTypeInformationDto eRPAssetTypeInformationDto = new ERPAssetTypeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"fatAccumDeprGlAccountID", "fatAssetGlAccountID", "fatAssetTypeID", "fatCreatedBy", "fatCreatedDate", "fatDepreciationGlAccountID", "fatDescription", "fatUniqueID", "fatExpenseGlAccountID", "fatLossGlAccountID",
			"fatProfitGlAccountID", "fatRepairsGlAccountID", "fatRevaluationGlAccountID", "fatRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("fatUniqueID|C", assetTypeId);
		AddCustomFieldsToSelectList("AssetTypes");
		using (DataTable dataTable = GetAsDataTable("AssetTypes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAssetTypeInformationDto);
			}
			eRPAssetTypeInformationDto.fatAccumDeprGlAccountID = dataTable.Rows[0].Field<string>("fatAccumDeprGlAccountID");
			eRPAssetTypeInformationDto.fatAssetGlAccountID = dataTable.Rows[0].Field<string>("fatAssetGlAccountID");
			eRPAssetTypeInformationDto.fatAssetTypeID = dataTable.Rows[0].Field<string>("fatAssetTypeID");
			eRPAssetTypeInformationDto.fatCreatedBy = dataTable.Rows[0].Field<string>("fatCreatedBy");
			eRPAssetTypeInformationDto.fatCreatedDate = dataTable.Rows[0].Field<DateTime?>("fatCreatedDate");
			eRPAssetTypeInformationDto.fatDepreciationGlAccountID = dataTable.Rows[0].Field<string>("fatDepreciationGlAccountID");
			eRPAssetTypeInformationDto.fatDescription = dataTable.Rows[0].Field<string>("fatDescription");
			eRPAssetTypeInformationDto.fatUniqueID = dataTable.Rows[0].Field<Guid>("fatUniqueID");
			eRPAssetTypeInformationDto.fatExpenseGlAccountID = dataTable.Rows[0].Field<string>("fatExpenseGlAccountID");
			eRPAssetTypeInformationDto.fatLossGlAccountID = dataTable.Rows[0].Field<string>("fatLossGlAccountID");
			eRPAssetTypeInformationDto.fatProfitGlAccountID = dataTable.Rows[0].Field<string>("fatProfitGlAccountID");
			eRPAssetTypeInformationDto.fatRepairsGlAccountID = dataTable.Rows[0].Field<string>("fatRepairsGlAccountID");
			eRPAssetTypeInformationDto.fatRevaluationGlAccountID = dataTable.Rows[0].Field<string>("fatRevaluationGlAccountID");
			eRPAssetTypeInformationDto.fatRowVersion = dataTable.Rows[0].Field<byte[]>("fatRowVersion");
			eRPAssetTypeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAssetTypeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAssetTypeInformationDto);
	}
}
