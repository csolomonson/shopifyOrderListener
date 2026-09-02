using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPAssetTypePlantRepository : APIBaseRepository, IERPAssetTypePlantRepository, IAPIBaseRepository, IDisposable
{
	public ERPAssetTypePlantRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAssetTypePlantExist(Guid assetTypePlantId)
	{
		InitializeParameterLists();
		base.filterList.Add("fayUniqueID|C", assetTypePlantId);
		base.selectList.Add("fayUniqueID");
		return Task.FromResult(GetAsObject("AssetTypePlants", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAssetTypePlantInformationDto>> GetAllAssetTypePlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAssetTypePlantInformationDto> collection = new List<ERPAssetTypePlantInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"fayAccumDeprGlAccountID", "fayAssetGlAccountID", "fayAssetTypeID", "fayAssetTypePlantID", "fayCreatedBy", "fayCreatedDate", "fayDepreciationGlAccountID", "fayUniqueID", "fayExpenseGlAccountID", "fayLossGlAccountID",
			"fayProfitGlAccountID", "fayRepairsGlAccountID", "fayRevaluationGlAccountID", "fayRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AssetTypePlants");
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
		using (DataTable dataTable = GetAsDataTable("AssetTypePlants", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAssetTypePlantInformationDto eRPAssetTypePlantInformationDto = new ERPAssetTypePlantInformationDto();
				eRPAssetTypePlantInformationDto.fayAccumDeprGlAccountID = dataTable.Rows[i].Field<string>("fayAccumDeprGlAccountID");
				eRPAssetTypePlantInformationDto.fayAssetGlAccountID = dataTable.Rows[i].Field<string>("fayAssetGlAccountID");
				eRPAssetTypePlantInformationDto.fayAssetTypeID = dataTable.Rows[i].Field<string>("fayAssetTypeID");
				eRPAssetTypePlantInformationDto.fayAssetTypePlantID = dataTable.Rows[i].Field<string>("fayAssetTypePlantID");
				eRPAssetTypePlantInformationDto.fayCreatedBy = dataTable.Rows[i].Field<string>("fayCreatedBy");
				eRPAssetTypePlantInformationDto.fayCreatedDate = dataTable.Rows[i].Field<DateTime?>("fayCreatedDate");
				eRPAssetTypePlantInformationDto.fayDepreciationGlAccountID = dataTable.Rows[i].Field<string>("fayDepreciationGlAccountID");
				eRPAssetTypePlantInformationDto.fayUniqueID = dataTable.Rows[i].Field<Guid>("fayUniqueID");
				eRPAssetTypePlantInformationDto.fayExpenseGlAccountID = dataTable.Rows[i].Field<string>("fayExpenseGlAccountID");
				eRPAssetTypePlantInformationDto.fayLossGlAccountID = dataTable.Rows[i].Field<string>("fayLossGlAccountID");
				eRPAssetTypePlantInformationDto.fayProfitGlAccountID = dataTable.Rows[i].Field<string>("fayProfitGlAccountID");
				eRPAssetTypePlantInformationDto.fayRepairsGlAccountID = dataTable.Rows[i].Field<string>("fayRepairsGlAccountID");
				eRPAssetTypePlantInformationDto.fayRevaluationGlAccountID = dataTable.Rows[i].Field<string>("fayRevaluationGlAccountID");
				eRPAssetTypePlantInformationDto.fayRowVersion = dataTable.Rows[i].Field<byte[]>("fayRowVersion");
				eRPAssetTypePlantInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAssetTypePlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAssetTypePlantInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAssetTypePlantInformationDto> GetAssetTypePlant(Guid assetTypePlantId)
	{
		ERPAssetTypePlantInformationDto eRPAssetTypePlantInformationDto = new ERPAssetTypePlantInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"fayAccumDeprGlAccountID", "fayAssetGlAccountID", "fayAssetTypeID", "fayAssetTypePlantID", "fayCreatedBy", "fayCreatedDate", "fayDepreciationGlAccountID", "fayUniqueID", "fayExpenseGlAccountID", "fayLossGlAccountID",
			"fayProfitGlAccountID", "fayRepairsGlAccountID", "fayRevaluationGlAccountID", "fayRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("fayUniqueID|C", assetTypePlantId);
		AddCustomFieldsToSelectList("AssetTypePlants");
		using (DataTable dataTable = GetAsDataTable("AssetTypePlants", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAssetTypePlantInformationDto);
			}
			eRPAssetTypePlantInformationDto.fayAccumDeprGlAccountID = dataTable.Rows[0].Field<string>("fayAccumDeprGlAccountID");
			eRPAssetTypePlantInformationDto.fayAssetGlAccountID = dataTable.Rows[0].Field<string>("fayAssetGlAccountID");
			eRPAssetTypePlantInformationDto.fayAssetTypeID = dataTable.Rows[0].Field<string>("fayAssetTypeID");
			eRPAssetTypePlantInformationDto.fayAssetTypePlantID = dataTable.Rows[0].Field<string>("fayAssetTypePlantID");
			eRPAssetTypePlantInformationDto.fayCreatedBy = dataTable.Rows[0].Field<string>("fayCreatedBy");
			eRPAssetTypePlantInformationDto.fayCreatedDate = dataTable.Rows[0].Field<DateTime?>("fayCreatedDate");
			eRPAssetTypePlantInformationDto.fayDepreciationGlAccountID = dataTable.Rows[0].Field<string>("fayDepreciationGlAccountID");
			eRPAssetTypePlantInformationDto.fayUniqueID = dataTable.Rows[0].Field<Guid>("fayUniqueID");
			eRPAssetTypePlantInformationDto.fayExpenseGlAccountID = dataTable.Rows[0].Field<string>("fayExpenseGlAccountID");
			eRPAssetTypePlantInformationDto.fayLossGlAccountID = dataTable.Rows[0].Field<string>("fayLossGlAccountID");
			eRPAssetTypePlantInformationDto.fayProfitGlAccountID = dataTable.Rows[0].Field<string>("fayProfitGlAccountID");
			eRPAssetTypePlantInformationDto.fayRepairsGlAccountID = dataTable.Rows[0].Field<string>("fayRepairsGlAccountID");
			eRPAssetTypePlantInformationDto.fayRevaluationGlAccountID = dataTable.Rows[0].Field<string>("fayRevaluationGlAccountID");
			eRPAssetTypePlantInformationDto.fayRowVersion = dataTable.Rows[0].Field<byte[]>("fayRowVersion");
			eRPAssetTypePlantInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAssetTypePlantInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAssetTypePlantInformationDto);
	}
}
