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

public class ERPCurrencyRateLineRepository : APIBaseRepository, IERPCurrencyRateLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPCurrencyRateLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCurrencyRateLineExist(Guid currencyRateLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("mclUniqueID|C", currencyRateLineId);
		base.selectList.Add("mclUniqueID");
		return Task.FromResult(GetAsObject("CurrencyRateLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCurrencyRateLineInformationDto>> GetAllCurrencyRateLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCurrencyRateLineInformationDto> collection = new List<ERPCurrencyRateLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "mclCreatedBy", "mclCreatedDate", "mclCurrencyRateID", "mclEffectiveDate", "mclUniqueID", "mclExchangeRate", "mclReference", "mclRowVersion", "mclCurrencyRateLineID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CurrencyRateLines");
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
		using (DataTable dataTable = GetAsDataTable("CurrencyRateLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCurrencyRateLineInformationDto eRPCurrencyRateLineInformationDto = new ERPCurrencyRateLineInformationDto();
				eRPCurrencyRateLineInformationDto.mclCreatedBy = dataTable.Rows[i].Field<string>("mclCreatedBy");
				eRPCurrencyRateLineInformationDto.mclCreatedDate = dataTable.Rows[i].Field<DateTime?>("mclCreatedDate");
				eRPCurrencyRateLineInformationDto.mclCurrencyRateID = dataTable.Rows[i].Field<string>("mclCurrencyRateID");
				eRPCurrencyRateLineInformationDto.mclEffectiveDate = dataTable.Rows[i].Field<DateTime?>("mclEffectiveDate");
				eRPCurrencyRateLineInformationDto.mclUniqueID = dataTable.Rows[i].Field<Guid>("mclUniqueID");
				eRPCurrencyRateLineInformationDto.mclExchangeRate = dataTable.Rows[i].Field<decimal>("mclExchangeRate");
				eRPCurrencyRateLineInformationDto.mclReference = dataTable.Rows[i].Field<string>("mclReference");
				eRPCurrencyRateLineInformationDto.mclRowVersion = dataTable.Rows[i].Field<byte[]>("mclRowVersion");
				eRPCurrencyRateLineInformationDto.mclCurrencyRateLineID = dataTable.Rows[i].Field<int>("mclCurrencyRateLineID");
				eRPCurrencyRateLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCurrencyRateLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCurrencyRateLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCurrencyRateLineInformationDto> GetCurrencyRateLine(Guid currencyRateLineId)
	{
		ERPCurrencyRateLineInformationDto eRPCurrencyRateLineInformationDto = new ERPCurrencyRateLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "mclCreatedBy", "mclCreatedDate", "mclCurrencyRateID", "mclEffectiveDate", "mclUniqueID", "mclExchangeRate", "mclReference", "mclRowVersion", "mclCurrencyRateLineID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("mclUniqueID|C", currencyRateLineId);
		AddCustomFieldsToSelectList("CurrencyRateLines");
		using (DataTable dataTable = GetAsDataTable("CurrencyRateLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCurrencyRateLineInformationDto);
			}
			eRPCurrencyRateLineInformationDto.mclCreatedBy = dataTable.Rows[0].Field<string>("mclCreatedBy");
			eRPCurrencyRateLineInformationDto.mclCreatedDate = dataTable.Rows[0].Field<DateTime?>("mclCreatedDate");
			eRPCurrencyRateLineInformationDto.mclCurrencyRateID = dataTable.Rows[0].Field<string>("mclCurrencyRateID");
			eRPCurrencyRateLineInformationDto.mclEffectiveDate = dataTable.Rows[0].Field<DateTime?>("mclEffectiveDate");
			eRPCurrencyRateLineInformationDto.mclUniqueID = dataTable.Rows[0].Field<Guid>("mclUniqueID");
			eRPCurrencyRateLineInformationDto.mclExchangeRate = dataTable.Rows[0].Field<decimal>("mclExchangeRate");
			eRPCurrencyRateLineInformationDto.mclReference = dataTable.Rows[0].Field<string>("mclReference");
			eRPCurrencyRateLineInformationDto.mclRowVersion = dataTable.Rows[0].Field<byte[]>("mclRowVersion");
			eRPCurrencyRateLineInformationDto.mclCurrencyRateLineID = dataTable.Rows[0].Field<int>("mclCurrencyRateLineID");
			eRPCurrencyRateLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCurrencyRateLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCurrencyRateLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCurrencyRateLine(ERPCurrencyRateLineDto currencyRateLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CurrencyRateLines WHERE mclUniqueID = " + M1Util.ConvertToLinq(currencyRateLine.mclUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mclCurrencyRateID"] = currencyRateLine.mclCurrencyRateID.ToUpper();
				dataRow["mclCurrencyRateLineID"] = currencyRateLine.mclCurrencyRateLineID;
				currencyRateLine.mclUniqueID = ((currencyRateLine.mclUniqueID == Guid.Empty) ? Guid.NewGuid() : currencyRateLine.mclUniqueID);
				dataRow["mclUniqueID"] = currencyRateLine.mclUniqueID;
				dataRow["mclCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mclCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CurrencyRateLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (currencyRateLine.mclRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CurrencyRateLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mclRowVersion"], currencyRateLine.mclRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CurrencyRateLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CurrencyRateLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? mclEffectiveDate = currencyRateLine.mclEffectiveDate;
			dataRow2["mclEffectiveDate"] = (mclEffectiveDate.HasValue ? ((object)mclEffectiveDate.GetValueOrDefault()) : dataRow["mclEffectiveDate"]);
			dataRow["mclExchangeRate"] = currencyRateLine.mclExchangeRate;
			dataRow["mclReference"] = currencyRateLine.mclReference;
			if (currencyRateLine.CustomFields != null && currencyRateLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in currencyRateLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CurrencyRateLine [{currencyRateLine.mclUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CurrencyRateLine [{currencyRateLine.mclUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
