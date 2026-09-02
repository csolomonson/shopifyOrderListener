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

public class ERPAssetLowValuePoolRepository : APIBaseRepository, IERPAssetLowValuePoolRepository, IAPIBaseRepository, IDisposable
{
	public ERPAssetLowValuePoolRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAssetLowValuePoolExist(Guid assetLowValuePoolId)
	{
		InitializeParameterLists();
		base.filterList.Add("favUniqueID|C", assetLowValuePoolId);
		base.selectList.Add("favUniqueID");
		return Task.FromResult(GetAsObject("AssetLowValuePool", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAssetLowValuePoolInformationDto>> GetAllAssetLowValuePool(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAssetLowValuePoolInformationDto> collection = new List<ERPAssetLowValuePoolInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"favClosedDate", "favCreatedBy", "favCreatedDate", "favEndingBalance", "favUniqueID", "favHighRate", "favHighRateDepreciation", "favImprovement", "favClosed", "favLowCostAddition",
			"favLowRate", "favLowRateDepreciation", "favLowValueAddition", "favOpeningBalance", "favPoolYearID", "favRowVersion", "favTermination"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AssetLowValuePool");
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
		using (DataTable dataTable = GetAsDataTable("AssetLowValuePool", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAssetLowValuePoolInformationDto eRPAssetLowValuePoolInformationDto = new ERPAssetLowValuePoolInformationDto();
				eRPAssetLowValuePoolInformationDto.favClosedDate = dataTable.Rows[i].Field<DateTime?>("favClosedDate");
				eRPAssetLowValuePoolInformationDto.favCreatedBy = dataTable.Rows[i].Field<string>("favCreatedBy");
				eRPAssetLowValuePoolInformationDto.favCreatedDate = dataTable.Rows[i].Field<DateTime?>("favCreatedDate");
				eRPAssetLowValuePoolInformationDto.favEndingBalance = dataTable.Rows[i].Field<decimal>("favEndingBalance");
				eRPAssetLowValuePoolInformationDto.favUniqueID = dataTable.Rows[i].Field<Guid>("favUniqueID");
				eRPAssetLowValuePoolInformationDto.favHighRate = dataTable.Rows[i].Field<decimal>("favHighRate");
				eRPAssetLowValuePoolInformationDto.favHighRateDepreciation = dataTable.Rows[i].Field<decimal>("favHighRateDepreciation");
				eRPAssetLowValuePoolInformationDto.favImprovement = dataTable.Rows[i].Field<decimal>("favImprovement");
				eRPAssetLowValuePoolInformationDto.favClosed = dataTable.Rows[i].Field<bool>("favClosed");
				eRPAssetLowValuePoolInformationDto.favLowCostAddition = dataTable.Rows[i].Field<decimal>("favLowCostAddition");
				eRPAssetLowValuePoolInformationDto.favLowRate = dataTable.Rows[i].Field<decimal>("favLowRate");
				eRPAssetLowValuePoolInformationDto.favLowRateDepreciation = dataTable.Rows[i].Field<decimal>("favLowRateDepreciation");
				eRPAssetLowValuePoolInformationDto.favLowValueAddition = dataTable.Rows[i].Field<decimal>("favLowValueAddition");
				eRPAssetLowValuePoolInformationDto.favOpeningBalance = dataTable.Rows[i].Field<decimal>("favOpeningBalance");
				eRPAssetLowValuePoolInformationDto.favPoolYearID = dataTable.Rows[i].Field<short>("favPoolYearID");
				eRPAssetLowValuePoolInformationDto.favRowVersion = dataTable.Rows[i].Field<byte[]>("favRowVersion");
				eRPAssetLowValuePoolInformationDto.favTermination = dataTable.Rows[i].Field<decimal>("favTermination");
				eRPAssetLowValuePoolInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAssetLowValuePoolInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAssetLowValuePoolInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAssetLowValuePoolInformationDto> GetAssetLowValuePool(Guid assetLowValuePoolId)
	{
		ERPAssetLowValuePoolInformationDto eRPAssetLowValuePoolInformationDto = new ERPAssetLowValuePoolInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"favClosedDate", "favCreatedBy", "favCreatedDate", "favEndingBalance", "favUniqueID", "favHighRate", "favHighRateDepreciation", "favImprovement", "favClosed", "favLowCostAddition",
			"favLowRate", "favLowRateDepreciation", "favLowValueAddition", "favOpeningBalance", "favPoolYearID", "favRowVersion", "favTermination"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("favUniqueID|C", assetLowValuePoolId);
		AddCustomFieldsToSelectList("AssetLowValuePool");
		using (DataTable dataTable = GetAsDataTable("AssetLowValuePool", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAssetLowValuePoolInformationDto);
			}
			eRPAssetLowValuePoolInformationDto.favClosedDate = dataTable.Rows[0].Field<DateTime?>("favClosedDate");
			eRPAssetLowValuePoolInformationDto.favCreatedBy = dataTable.Rows[0].Field<string>("favCreatedBy");
			eRPAssetLowValuePoolInformationDto.favCreatedDate = dataTable.Rows[0].Field<DateTime?>("favCreatedDate");
			eRPAssetLowValuePoolInformationDto.favEndingBalance = dataTable.Rows[0].Field<decimal>("favEndingBalance");
			eRPAssetLowValuePoolInformationDto.favUniqueID = dataTable.Rows[0].Field<Guid>("favUniqueID");
			eRPAssetLowValuePoolInformationDto.favHighRate = dataTable.Rows[0].Field<decimal>("favHighRate");
			eRPAssetLowValuePoolInformationDto.favHighRateDepreciation = dataTable.Rows[0].Field<decimal>("favHighRateDepreciation");
			eRPAssetLowValuePoolInformationDto.favImprovement = dataTable.Rows[0].Field<decimal>("favImprovement");
			eRPAssetLowValuePoolInformationDto.favClosed = dataTable.Rows[0].Field<bool>("favClosed");
			eRPAssetLowValuePoolInformationDto.favLowCostAddition = dataTable.Rows[0].Field<decimal>("favLowCostAddition");
			eRPAssetLowValuePoolInformationDto.favLowRate = dataTable.Rows[0].Field<decimal>("favLowRate");
			eRPAssetLowValuePoolInformationDto.favLowRateDepreciation = dataTable.Rows[0].Field<decimal>("favLowRateDepreciation");
			eRPAssetLowValuePoolInformationDto.favLowValueAddition = dataTable.Rows[0].Field<decimal>("favLowValueAddition");
			eRPAssetLowValuePoolInformationDto.favOpeningBalance = dataTable.Rows[0].Field<decimal>("favOpeningBalance");
			eRPAssetLowValuePoolInformationDto.favPoolYearID = dataTable.Rows[0].Field<short>("favPoolYearID");
			eRPAssetLowValuePoolInformationDto.favRowVersion = dataTable.Rows[0].Field<byte[]>("favRowVersion");
			eRPAssetLowValuePoolInformationDto.favTermination = dataTable.Rows[0].Field<decimal>("favTermination");
			eRPAssetLowValuePoolInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAssetLowValuePoolInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAssetLowValuePoolInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAssetLowValuePool(ERPAssetLowValuePoolDto assetLowValuePool)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM AssetLowValuePool WHERE favUniqueID = " + M1Util.ConvertToLinq(assetLowValuePool.favUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["favPoolYearID"] = assetLowValuePool.favPoolYearID;
				assetLowValuePool.favUniqueID = ((assetLowValuePool.favUniqueID == Guid.Empty) ? Guid.NewGuid() : assetLowValuePool.favUniqueID);
				dataRow["favUniqueID"] = assetLowValuePool.favUniqueID;
				dataRow["favCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["favCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The AssetLowValuePool could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (assetLowValuePool.favRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the AssetLowValuePool is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["favRowVersion"], assetLowValuePool.favRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the AssetLowValuePool has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the AssetLowValuePool again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? favClosedDate = assetLowValuePool.favClosedDate;
			dataRow2["favClosedDate"] = (favClosedDate.HasValue ? ((object)favClosedDate.GetValueOrDefault()) : dataRow["favClosedDate"]);
			dataRow["favEndingBalance"] = assetLowValuePool.favEndingBalance;
			dataRow["favHighRate"] = assetLowValuePool.favHighRate;
			dataRow["favHighRateDepreciation"] = assetLowValuePool.favHighRateDepreciation;
			dataRow["favImprovement"] = assetLowValuePool.favImprovement;
			dataRow["favClosed"] = assetLowValuePool.favClosed;
			dataRow["favLowCostAddition"] = assetLowValuePool.favLowCostAddition;
			dataRow["favLowRate"] = assetLowValuePool.favLowRate;
			dataRow["favLowRateDepreciation"] = assetLowValuePool.favLowRateDepreciation;
			dataRow["favLowValueAddition"] = assetLowValuePool.favLowValueAddition;
			dataRow["favOpeningBalance"] = assetLowValuePool.favOpeningBalance;
			dataRow["favTermination"] = assetLowValuePool.favTermination;
			if (assetLowValuePool.CustomFields != null && assetLowValuePool.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in assetLowValuePool.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the AssetLowValuePool [{assetLowValuePool.favUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the AssetLowValuePool [{assetLowValuePool.favUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
