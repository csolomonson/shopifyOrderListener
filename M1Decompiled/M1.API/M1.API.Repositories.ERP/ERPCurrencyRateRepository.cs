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

public class ERPCurrencyRateRepository : APIBaseRepository, IERPCurrencyRateRepository, IAPIBaseRepository, IDisposable
{
	public ERPCurrencyRateRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCurrencyRateExist(Guid currencyRateId)
	{
		InitializeParameterLists();
		base.filterList.Add("mcpUniqueID|C", currencyRateId);
		base.selectList.Add("mcpUniqueID");
		return Task.FromResult(GetAsObject("CurrencyRates", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCurrencyRateInformationDto>> GetAllCurrencyRates(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCurrencyRateInformationDto> collection = new List<ERPCurrencyRateInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"mcpApGlAccountID", "mcpArGlAccountID", "mcpCurrencyRateID", "mcpCreatedBy", "mcpCreatedDate", "mcpDescription", "mcpUniqueID", "mcpExchangeGainGlAccountID", "mcpExchangeLossGlAccountID", "mcpRowVersion",
			"mcpSymbol", "mcpUnrealisedExGainGlAccountID", "mcpUnrealisedExLossGlAccountID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("CurrencyRates");
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
		using (DataTable dataTable = GetAsDataTable("CurrencyRates", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCurrencyRateInformationDto eRPCurrencyRateInformationDto = new ERPCurrencyRateInformationDto();
				eRPCurrencyRateInformationDto.mcpApGlAccountID = dataTable.Rows[i].Field<string>("mcpApGlAccountID");
				eRPCurrencyRateInformationDto.mcpArGlAccountID = dataTable.Rows[i].Field<string>("mcpArGlAccountID");
				eRPCurrencyRateInformationDto.mcpCurrencyRateID = dataTable.Rows[i].Field<string>("mcpCurrencyRateID");
				eRPCurrencyRateInformationDto.mcpCreatedBy = dataTable.Rows[i].Field<string>("mcpCreatedBy");
				eRPCurrencyRateInformationDto.mcpCreatedDate = dataTable.Rows[i].Field<DateTime?>("mcpCreatedDate");
				eRPCurrencyRateInformationDto.mcpDescription = dataTable.Rows[i].Field<string>("mcpDescription");
				eRPCurrencyRateInformationDto.mcpUniqueID = dataTable.Rows[i].Field<Guid>("mcpUniqueID");
				eRPCurrencyRateInformationDto.mcpExchangeGainGlAccountID = dataTable.Rows[i].Field<string>("mcpExchangeGainGlAccountID");
				eRPCurrencyRateInformationDto.mcpExchangeLossGlAccountID = dataTable.Rows[i].Field<string>("mcpExchangeLossGlAccountID");
				eRPCurrencyRateInformationDto.mcpRowVersion = dataTable.Rows[i].Field<byte[]>("mcpRowVersion");
				eRPCurrencyRateInformationDto.mcpSymbol = dataTable.Rows[i].Field<string>("mcpSymbol");
				eRPCurrencyRateInformationDto.mcpUnrealisedExGainGlAccountID = dataTable.Rows[i].Field<string>("mcpUnrealisedExGainGlAccountID");
				eRPCurrencyRateInformationDto.mcpUnrealisedExLossGlAccountID = dataTable.Rows[i].Field<string>("mcpUnrealisedExLossGlAccountID");
				eRPCurrencyRateInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCurrencyRateInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCurrencyRateInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCurrencyRateInformationDto> GetCurrencyRate(Guid currencyRateId)
	{
		ERPCurrencyRateInformationDto eRPCurrencyRateInformationDto = new ERPCurrencyRateInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"mcpApGlAccountID", "mcpArGlAccountID", "mcpCurrencyRateID", "mcpCreatedBy", "mcpCreatedDate", "mcpDescription", "mcpUniqueID", "mcpExchangeGainGlAccountID", "mcpExchangeLossGlAccountID", "mcpRowVersion",
			"mcpSymbol", "mcpUnrealisedExGainGlAccountID", "mcpUnrealisedExLossGlAccountID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("mcpUniqueID|C", currencyRateId);
		AddCustomFieldsToSelectList("CurrencyRates");
		using (DataTable dataTable = GetAsDataTable("CurrencyRates", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCurrencyRateInformationDto);
			}
			eRPCurrencyRateInformationDto.mcpApGlAccountID = dataTable.Rows[0].Field<string>("mcpApGlAccountID");
			eRPCurrencyRateInformationDto.mcpArGlAccountID = dataTable.Rows[0].Field<string>("mcpArGlAccountID");
			eRPCurrencyRateInformationDto.mcpCurrencyRateID = dataTable.Rows[0].Field<string>("mcpCurrencyRateID");
			eRPCurrencyRateInformationDto.mcpCreatedBy = dataTable.Rows[0].Field<string>("mcpCreatedBy");
			eRPCurrencyRateInformationDto.mcpCreatedDate = dataTable.Rows[0].Field<DateTime?>("mcpCreatedDate");
			eRPCurrencyRateInformationDto.mcpDescription = dataTable.Rows[0].Field<string>("mcpDescription");
			eRPCurrencyRateInformationDto.mcpUniqueID = dataTable.Rows[0].Field<Guid>("mcpUniqueID");
			eRPCurrencyRateInformationDto.mcpExchangeGainGlAccountID = dataTable.Rows[0].Field<string>("mcpExchangeGainGlAccountID");
			eRPCurrencyRateInformationDto.mcpExchangeLossGlAccountID = dataTable.Rows[0].Field<string>("mcpExchangeLossGlAccountID");
			eRPCurrencyRateInformationDto.mcpRowVersion = dataTable.Rows[0].Field<byte[]>("mcpRowVersion");
			eRPCurrencyRateInformationDto.mcpSymbol = dataTable.Rows[0].Field<string>("mcpSymbol");
			eRPCurrencyRateInformationDto.mcpUnrealisedExGainGlAccountID = dataTable.Rows[0].Field<string>("mcpUnrealisedExGainGlAccountID");
			eRPCurrencyRateInformationDto.mcpUnrealisedExLossGlAccountID = dataTable.Rows[0].Field<string>("mcpUnrealisedExLossGlAccountID");
			eRPCurrencyRateInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCurrencyRateInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCurrencyRateInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCurrencyRate(ERPCurrencyRateDto currencyRate)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM CurrencyRates WHERE mcpUniqueID = " + M1Util.ConvertToLinq(currencyRate.mcpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mcpCurrencyRateID"] = currencyRate.mcpCurrencyRateID.ToUpper();
				currencyRate.mcpUniqueID = ((currencyRate.mcpUniqueID == Guid.Empty) ? Guid.NewGuid() : currencyRate.mcpUniqueID);
				dataRow["mcpUniqueID"] = currencyRate.mcpUniqueID;
				dataRow["mcpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mcpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The CurrencyRate could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (currencyRate.mcpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the CurrencyRate is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mcpRowVersion"], currencyRate.mcpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the CurrencyRate has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the CurrencyRate again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["mcpApGlAccountID"] = currencyRate.mcpApGlAccountID;
			dataRow["mcpArGlAccountID"] = currencyRate.mcpArGlAccountID;
			dataRow["mcpDescription"] = currencyRate.mcpDescription;
			dataRow["mcpExchangeGainGlAccountID"] = currencyRate.mcpExchangeGainGlAccountID;
			dataRow["mcpExchangeLossGlAccountID"] = currencyRate.mcpExchangeLossGlAccountID;
			dataRow["mcpSymbol"] = currencyRate.mcpSymbol;
			dataRow["mcpUnrealisedExGainGlAccountID"] = currencyRate.mcpUnrealisedExGainGlAccountID;
			dataRow["mcpUnrealisedExLossGlAccountID"] = currencyRate.mcpUnrealisedExLossGlAccountID;
			if (currencyRate.CustomFields != null && currencyRate.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in currencyRate.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the CurrencyRate [{currencyRate.mcpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the CurrencyRate [{currencyRate.mcpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
