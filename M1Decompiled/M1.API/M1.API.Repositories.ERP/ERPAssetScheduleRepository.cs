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

public class ERPAssetScheduleRepository : APIBaseRepository, IERPAssetScheduleRepository, IAPIBaseRepository, IDisposable
{
	public ERPAssetScheduleRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAssetScheduleExist(Guid assetScheduleId)
	{
		InitializeParameterLists();
		base.filterList.Add("fasUniqueID|C", assetScheduleId);
		base.selectList.Add("fasUniqueID");
		return Task.FromResult(GetAsObject("AssetSchedules", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAssetScheduleInformationDto>> GetAllAssetSchedules(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAssetScheduleInformationDto> collection = new List<ERPAssetScheduleInformationDto>();
		InitializeParameterLists();
		string[] array = new string[21]
		{
			"fasActualProductionUnits", "fasAdditionalAssetAmount", "fasAssetID", "fasClosingAccumBalance", "fasClosingAssetValue", "fasCreatedBy", "fasCreatedDate", "fasDepreciationAmount", "fasUniqueID", "fasEstimatedProductionUnits",
			"fasGlFiscalYearID", "fasGlFiscalYearPeriodID", "fasPostedToGl", "fasNetAssetValue", "fasOpeningAccumBalance", "fasOpeningAssetValue", "fasRowVersion", "fasAssetScheduleID", "fasSubtractAssetAmount", "fasType",
			"fasWritebackAmount"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AssetSchedules");
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
		using (DataTable dataTable = GetAsDataTable("AssetSchedules", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAssetScheduleInformationDto eRPAssetScheduleInformationDto = new ERPAssetScheduleInformationDto();
				eRPAssetScheduleInformationDto.fasActualProductionUnits = dataTable.Rows[i].Field<int>("fasActualProductionUnits");
				eRPAssetScheduleInformationDto.fasAdditionalAssetAmount = dataTable.Rows[i].Field<decimal>("fasAdditionalAssetAmount");
				eRPAssetScheduleInformationDto.fasAssetID = dataTable.Rows[i].Field<string>("fasAssetID");
				eRPAssetScheduleInformationDto.fasClosingAccumBalance = dataTable.Rows[i].Field<decimal>("fasClosingAccumBalance");
				eRPAssetScheduleInformationDto.fasClosingAssetValue = dataTable.Rows[i].Field<decimal>("fasClosingAssetValue");
				eRPAssetScheduleInformationDto.fasCreatedBy = dataTable.Rows[i].Field<string>("fasCreatedBy");
				eRPAssetScheduleInformationDto.fasCreatedDate = dataTable.Rows[i].Field<DateTime?>("fasCreatedDate");
				eRPAssetScheduleInformationDto.fasDepreciationAmount = dataTable.Rows[i].Field<decimal>("fasDepreciationAmount");
				eRPAssetScheduleInformationDto.fasUniqueID = dataTable.Rows[i].Field<Guid>("fasUniqueID");
				eRPAssetScheduleInformationDto.fasEstimatedProductionUnits = dataTable.Rows[i].Field<int>("fasEstimatedProductionUnits");
				eRPAssetScheduleInformationDto.fasGlFiscalYearID = dataTable.Rows[i].Field<short>("fasGlFiscalYearID");
				eRPAssetScheduleInformationDto.fasGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("fasGlFiscalYearPeriodID");
				eRPAssetScheduleInformationDto.fasPostedToGl = dataTable.Rows[i].Field<bool>("fasPostedToGl");
				eRPAssetScheduleInformationDto.fasNetAssetValue = dataTable.Rows[i].Field<decimal>("fasNetAssetValue");
				eRPAssetScheduleInformationDto.fasOpeningAccumBalance = dataTable.Rows[i].Field<decimal>("fasOpeningAccumBalance");
				eRPAssetScheduleInformationDto.fasOpeningAssetValue = dataTable.Rows[i].Field<decimal>("fasOpeningAssetValue");
				eRPAssetScheduleInformationDto.fasRowVersion = dataTable.Rows[i].Field<byte[]>("fasRowVersion");
				eRPAssetScheduleInformationDto.fasAssetScheduleID = dataTable.Rows[i].Field<int>("fasAssetScheduleID");
				eRPAssetScheduleInformationDto.fasSubtractAssetAmount = dataTable.Rows[i].Field<decimal>("fasSubtractAssetAmount");
				eRPAssetScheduleInformationDto.fasType = dataTable.Rows[i].Field<string>("fasType");
				eRPAssetScheduleInformationDto.fasWritebackAmount = dataTable.Rows[i].Field<decimal>("fasWritebackAmount");
				eRPAssetScheduleInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAssetScheduleInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAssetScheduleInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAssetScheduleInformationDto> GetAssetSchedule(Guid assetScheduleId)
	{
		ERPAssetScheduleInformationDto eRPAssetScheduleInformationDto = new ERPAssetScheduleInformationDto();
		InitializeParameterLists();
		string[] collection = new string[21]
		{
			"fasActualProductionUnits", "fasAdditionalAssetAmount", "fasAssetID", "fasClosingAccumBalance", "fasClosingAssetValue", "fasCreatedBy", "fasCreatedDate", "fasDepreciationAmount", "fasUniqueID", "fasEstimatedProductionUnits",
			"fasGlFiscalYearID", "fasGlFiscalYearPeriodID", "fasPostedToGl", "fasNetAssetValue", "fasOpeningAccumBalance", "fasOpeningAssetValue", "fasRowVersion", "fasAssetScheduleID", "fasSubtractAssetAmount", "fasType",
			"fasWritebackAmount"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("fasUniqueID|C", assetScheduleId);
		AddCustomFieldsToSelectList("AssetSchedules");
		using (DataTable dataTable = GetAsDataTable("AssetSchedules", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAssetScheduleInformationDto);
			}
			eRPAssetScheduleInformationDto.fasActualProductionUnits = dataTable.Rows[0].Field<int>("fasActualProductionUnits");
			eRPAssetScheduleInformationDto.fasAdditionalAssetAmount = dataTable.Rows[0].Field<decimal>("fasAdditionalAssetAmount");
			eRPAssetScheduleInformationDto.fasAssetID = dataTable.Rows[0].Field<string>("fasAssetID");
			eRPAssetScheduleInformationDto.fasClosingAccumBalance = dataTable.Rows[0].Field<decimal>("fasClosingAccumBalance");
			eRPAssetScheduleInformationDto.fasClosingAssetValue = dataTable.Rows[0].Field<decimal>("fasClosingAssetValue");
			eRPAssetScheduleInformationDto.fasCreatedBy = dataTable.Rows[0].Field<string>("fasCreatedBy");
			eRPAssetScheduleInformationDto.fasCreatedDate = dataTable.Rows[0].Field<DateTime?>("fasCreatedDate");
			eRPAssetScheduleInformationDto.fasDepreciationAmount = dataTable.Rows[0].Field<decimal>("fasDepreciationAmount");
			eRPAssetScheduleInformationDto.fasUniqueID = dataTable.Rows[0].Field<Guid>("fasUniqueID");
			eRPAssetScheduleInformationDto.fasEstimatedProductionUnits = dataTable.Rows[0].Field<int>("fasEstimatedProductionUnits");
			eRPAssetScheduleInformationDto.fasGlFiscalYearID = dataTable.Rows[0].Field<short>("fasGlFiscalYearID");
			eRPAssetScheduleInformationDto.fasGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("fasGlFiscalYearPeriodID");
			eRPAssetScheduleInformationDto.fasPostedToGl = dataTable.Rows[0].Field<bool>("fasPostedToGl");
			eRPAssetScheduleInformationDto.fasNetAssetValue = dataTable.Rows[0].Field<decimal>("fasNetAssetValue");
			eRPAssetScheduleInformationDto.fasOpeningAccumBalance = dataTable.Rows[0].Field<decimal>("fasOpeningAccumBalance");
			eRPAssetScheduleInformationDto.fasOpeningAssetValue = dataTable.Rows[0].Field<decimal>("fasOpeningAssetValue");
			eRPAssetScheduleInformationDto.fasRowVersion = dataTable.Rows[0].Field<byte[]>("fasRowVersion");
			eRPAssetScheduleInformationDto.fasAssetScheduleID = dataTable.Rows[0].Field<int>("fasAssetScheduleID");
			eRPAssetScheduleInformationDto.fasSubtractAssetAmount = dataTable.Rows[0].Field<decimal>("fasSubtractAssetAmount");
			eRPAssetScheduleInformationDto.fasType = dataTable.Rows[0].Field<string>("fasType");
			eRPAssetScheduleInformationDto.fasWritebackAmount = dataTable.Rows[0].Field<decimal>("fasWritebackAmount");
			eRPAssetScheduleInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAssetScheduleInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAssetScheduleInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAssetSchedule(ERPAssetScheduleDto assetSchedule)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM AssetSchedules WHERE fasUniqueID = " + M1Util.ConvertToLinq(assetSchedule.fasUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["fasAssetID"] = assetSchedule.fasAssetID.ToUpper();
				dataRow["fasAssetScheduleID"] = assetSchedule.fasAssetScheduleID;
				assetSchedule.fasUniqueID = ((assetSchedule.fasUniqueID == Guid.Empty) ? Guid.NewGuid() : assetSchedule.fasUniqueID);
				dataRow["fasUniqueID"] = assetSchedule.fasUniqueID;
				dataRow["fasCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["fasCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The AssetSchedule could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (assetSchedule.fasRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the AssetSchedule is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["fasRowVersion"], assetSchedule.fasRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the AssetSchedule has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the AssetSchedule again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["fasActualProductionUnits"] = assetSchedule.fasActualProductionUnits;
			dataRow["fasAdditionalAssetAmount"] = assetSchedule.fasAdditionalAssetAmount;
			dataRow["fasClosingAccumBalance"] = assetSchedule.fasClosingAccumBalance;
			dataRow["fasClosingAssetValue"] = assetSchedule.fasClosingAssetValue;
			dataRow["fasDepreciationAmount"] = assetSchedule.fasDepreciationAmount;
			dataRow["fasEstimatedProductionUnits"] = assetSchedule.fasEstimatedProductionUnits;
			dataRow["fasGlFiscalYearID"] = assetSchedule.fasGlFiscalYearID;
			dataRow["fasGlFiscalYearPeriodID"] = assetSchedule.fasGlFiscalYearPeriodID;
			dataRow["fasPostedToGl"] = assetSchedule.fasPostedToGl;
			dataRow["fasNetAssetValue"] = assetSchedule.fasNetAssetValue;
			dataRow["fasOpeningAccumBalance"] = assetSchedule.fasOpeningAccumBalance;
			dataRow["fasOpeningAssetValue"] = assetSchedule.fasOpeningAssetValue;
			dataRow["fasSubtractAssetAmount"] = assetSchedule.fasSubtractAssetAmount;
			dataRow["fasType"] = assetSchedule.fasType;
			dataRow["fasWritebackAmount"] = assetSchedule.fasWritebackAmount;
			if (assetSchedule.CustomFields != null && assetSchedule.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in assetSchedule.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the AssetSchedule [{assetSchedule.fasUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the AssetSchedule [{assetSchedule.fasUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
