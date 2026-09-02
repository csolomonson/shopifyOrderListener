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

public class ERPAssetPoolTransactionRepository : APIBaseRepository, IERPAssetPoolTransactionRepository, IAPIBaseRepository, IDisposable
{
	public ERPAssetPoolTransactionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAssetPoolTransactionExist(Guid assetPoolTransactionId)
	{
		InitializeParameterLists();
		base.filterList.Add("fawUniqueID|C", assetPoolTransactionId);
		base.selectList.Add("fawUniqueID");
		return Task.FromResult(GetAsObject("AssetPoolTransactions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAssetPoolTransactionInformationDto>> GetAllAssetPoolTransactions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAssetPoolTransactionInformationDto> collection = new List<ERPAssetPoolTransactionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"fawAmount", "fawAssetAdjustmentID", "fawAssetID", "fawCreatedBy", "fawCreatedDate", "fawUniqueID", "fawPoolTransactionID", "fawPoolYearID", "fawRowVersion", "fawTransactionDate",
			"fawTransactionType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AssetPoolTransactions");
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
		using (DataTable dataTable = GetAsDataTable("AssetPoolTransactions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAssetPoolTransactionInformationDto eRPAssetPoolTransactionInformationDto = new ERPAssetPoolTransactionInformationDto();
				eRPAssetPoolTransactionInformationDto.fawAmount = dataTable.Rows[i].Field<decimal>("fawAmount");
				eRPAssetPoolTransactionInformationDto.fawAssetAdjustmentID = dataTable.Rows[i].Field<int>("fawAssetAdjustmentID");
				eRPAssetPoolTransactionInformationDto.fawAssetID = dataTable.Rows[i].Field<string>("fawAssetID");
				eRPAssetPoolTransactionInformationDto.fawCreatedBy = dataTable.Rows[i].Field<string>("fawCreatedBy");
				eRPAssetPoolTransactionInformationDto.fawCreatedDate = dataTable.Rows[i].Field<DateTime?>("fawCreatedDate");
				eRPAssetPoolTransactionInformationDto.fawUniqueID = dataTable.Rows[i].Field<Guid>("fawUniqueID");
				eRPAssetPoolTransactionInformationDto.fawPoolTransactionID = dataTable.Rows[i].Field<int>("fawPoolTransactionID");
				eRPAssetPoolTransactionInformationDto.fawPoolYearID = dataTable.Rows[i].Field<short>("fawPoolYearID");
				eRPAssetPoolTransactionInformationDto.fawRowVersion = dataTable.Rows[i].Field<byte[]>("fawRowVersion");
				eRPAssetPoolTransactionInformationDto.fawTransactionDate = dataTable.Rows[i].Field<DateTime?>("fawTransactionDate");
				eRPAssetPoolTransactionInformationDto.fawTransactionType = dataTable.Rows[i].Field<string>("fawTransactionType");
				eRPAssetPoolTransactionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAssetPoolTransactionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAssetPoolTransactionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAssetPoolTransactionInformationDto> GetAssetPoolTransaction(Guid assetPoolTransactionId)
	{
		ERPAssetPoolTransactionInformationDto eRPAssetPoolTransactionInformationDto = new ERPAssetPoolTransactionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"fawAmount", "fawAssetAdjustmentID", "fawAssetID", "fawCreatedBy", "fawCreatedDate", "fawUniqueID", "fawPoolTransactionID", "fawPoolYearID", "fawRowVersion", "fawTransactionDate",
			"fawTransactionType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("fawUniqueID|C", assetPoolTransactionId);
		AddCustomFieldsToSelectList("AssetPoolTransactions");
		using (DataTable dataTable = GetAsDataTable("AssetPoolTransactions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAssetPoolTransactionInformationDto);
			}
			eRPAssetPoolTransactionInformationDto.fawAmount = dataTable.Rows[0].Field<decimal>("fawAmount");
			eRPAssetPoolTransactionInformationDto.fawAssetAdjustmentID = dataTable.Rows[0].Field<int>("fawAssetAdjustmentID");
			eRPAssetPoolTransactionInformationDto.fawAssetID = dataTable.Rows[0].Field<string>("fawAssetID");
			eRPAssetPoolTransactionInformationDto.fawCreatedBy = dataTable.Rows[0].Field<string>("fawCreatedBy");
			eRPAssetPoolTransactionInformationDto.fawCreatedDate = dataTable.Rows[0].Field<DateTime?>("fawCreatedDate");
			eRPAssetPoolTransactionInformationDto.fawUniqueID = dataTable.Rows[0].Field<Guid>("fawUniqueID");
			eRPAssetPoolTransactionInformationDto.fawPoolTransactionID = dataTable.Rows[0].Field<int>("fawPoolTransactionID");
			eRPAssetPoolTransactionInformationDto.fawPoolYearID = dataTable.Rows[0].Field<short>("fawPoolYearID");
			eRPAssetPoolTransactionInformationDto.fawRowVersion = dataTable.Rows[0].Field<byte[]>("fawRowVersion");
			eRPAssetPoolTransactionInformationDto.fawTransactionDate = dataTable.Rows[0].Field<DateTime?>("fawTransactionDate");
			eRPAssetPoolTransactionInformationDto.fawTransactionType = dataTable.Rows[0].Field<string>("fawTransactionType");
			eRPAssetPoolTransactionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAssetPoolTransactionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAssetPoolTransactionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAssetPoolTransaction(ERPAssetPoolTransactionDto assetPoolTransaction)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM AssetPoolTransactions WHERE fawUniqueID = " + M1Util.ConvertToLinq(assetPoolTransaction.fawUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["fawPoolTransactionID"] = assetPoolTransaction.fawPoolTransactionID;
				assetPoolTransaction.fawUniqueID = ((assetPoolTransaction.fawUniqueID == Guid.Empty) ? Guid.NewGuid() : assetPoolTransaction.fawUniqueID);
				dataRow["fawUniqueID"] = assetPoolTransaction.fawUniqueID;
				dataRow["fawCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["fawCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The AssetPoolTransaction could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (assetPoolTransaction.fawRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the AssetPoolTransaction is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["fawRowVersion"], assetPoolTransaction.fawRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the AssetPoolTransaction has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the AssetPoolTransaction again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["fawAmount"] = assetPoolTransaction.fawAmount;
			dataRow["fawAssetAdjustmentID"] = assetPoolTransaction.fawAssetAdjustmentID;
			dataRow["fawAssetID"] = assetPoolTransaction.fawAssetID;
			dataRow["fawPoolYearID"] = assetPoolTransaction.fawPoolYearID;
			DataRow dataRow2 = dataRow;
			DateTime? fawTransactionDate = assetPoolTransaction.fawTransactionDate;
			dataRow2["fawTransactionDate"] = (fawTransactionDate.HasValue ? ((object)fawTransactionDate.GetValueOrDefault()) : dataRow["fawTransactionDate"]);
			dataRow["fawTransactionType"] = assetPoolTransaction.fawTransactionType;
			if (assetPoolTransaction.CustomFields != null && assetPoolTransaction.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in assetPoolTransaction.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the AssetPoolTransaction [{assetPoolTransaction.fawUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the AssetPoolTransaction [{assetPoolTransaction.fawUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
