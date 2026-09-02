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

public class ERPBankStatementRepository : APIBaseRepository, IERPBankStatementRepository, IAPIBaseRepository, IDisposable
{
	public ERPBankStatementRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesBankStatementExist(Guid bankStatementId)
	{
		InitializeParameterLists();
		base.filterList.Add("glsUniqueID|C", bankStatementId);
		base.selectList.Add("glsUniqueID");
		return Task.FromResult(GetAsObject("BankStatements", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPBankStatementInformationDto>> GetAllBankStatements(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPBankStatementInformationDto> collection = new List<ERPBankStatementInformationDto>();
		InitializeParameterLists();
		string[] array = new string[23]
		{
			"glsBankAccountID", "glsBankStatementReference", "glsCashGlAccountID", "glsCreatedBy", "glsCreatedDate", "glsCurrencyRateID", "glsEndingBalance", "glsEndingBalanceForeign", "glsEndingDate", "glsUniqueID",
			"glsExchangeAmount", "glsExchangeGlAccountID", "glsExchangeRate", "glsGlFiscalYearID", "glsCustomRate", "glsPostedToGl", "glsOpeningBalance", "glsOpeningBalanceForeign", "glsOpeningDate", "glsPostedDate",
			"glsRowVersion", "glsBankStatementID", "glsShowTransactions"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("BankStatements");
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
		using (DataTable dataTable = GetAsDataTable("BankStatements", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPBankStatementInformationDto eRPBankStatementInformationDto = new ERPBankStatementInformationDto();
				eRPBankStatementInformationDto.glsBankAccountID = dataTable.Rows[i].Field<string>("glsBankAccountID");
				eRPBankStatementInformationDto.glsBankStatementReference = dataTable.Rows[i].Field<int>("glsBankStatementReference");
				eRPBankStatementInformationDto.glsCashGlAccountID = dataTable.Rows[i].Field<string>("glsCashGlAccountID");
				eRPBankStatementInformationDto.glsCreatedBy = dataTable.Rows[i].Field<string>("glsCreatedBy");
				eRPBankStatementInformationDto.glsCreatedDate = dataTable.Rows[i].Field<DateTime?>("glsCreatedDate");
				eRPBankStatementInformationDto.glsCurrencyRateID = dataTable.Rows[i].Field<string>("glsCurrencyRateID");
				eRPBankStatementInformationDto.glsEndingBalance = dataTable.Rows[i].Field<decimal>("glsEndingBalance");
				eRPBankStatementInformationDto.glsEndingBalanceForeign = dataTable.Rows[i].Field<decimal>("glsEndingBalanceForeign");
				eRPBankStatementInformationDto.glsEndingDate = dataTable.Rows[i].Field<DateTime?>("glsEndingDate");
				eRPBankStatementInformationDto.glsUniqueID = dataTable.Rows[i].Field<Guid>("glsUniqueID");
				eRPBankStatementInformationDto.glsExchangeAmount = dataTable.Rows[i].Field<decimal>("glsExchangeAmount");
				eRPBankStatementInformationDto.glsExchangeGlAccountID = dataTable.Rows[i].Field<string>("glsExchangeGlAccountID");
				eRPBankStatementInformationDto.glsExchangeRate = dataTable.Rows[i].Field<decimal>("glsExchangeRate");
				eRPBankStatementInformationDto.glsGlFiscalYearID = dataTable.Rows[i].Field<short>("glsGlFiscalYearID");
				eRPBankStatementInformationDto.glsCustomRate = dataTable.Rows[i].Field<bool>("glsCustomRate");
				eRPBankStatementInformationDto.glsPostedToGl = dataTable.Rows[i].Field<bool>("glsPostedToGl");
				eRPBankStatementInformationDto.glsOpeningBalance = dataTable.Rows[i].Field<decimal>("glsOpeningBalance");
				eRPBankStatementInformationDto.glsOpeningBalanceForeign = dataTable.Rows[i].Field<decimal>("glsOpeningBalanceForeign");
				eRPBankStatementInformationDto.glsOpeningDate = dataTable.Rows[i].Field<DateTime?>("glsOpeningDate");
				eRPBankStatementInformationDto.glsPostedDate = dataTable.Rows[i].Field<DateTime?>("glsPostedDate");
				eRPBankStatementInformationDto.glsRowVersion = dataTable.Rows[i].Field<byte[]>("glsRowVersion");
				eRPBankStatementInformationDto.glsBankStatementID = dataTable.Rows[i].Field<int>("glsBankStatementID");
				eRPBankStatementInformationDto.glsShowTransactions = dataTable.Rows[i].Field<bool>("glsShowTransactions");
				eRPBankStatementInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPBankStatementInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPBankStatementInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPBankStatementInformationDto> GetBankStatement(Guid bankStatementId)
	{
		ERPBankStatementInformationDto eRPBankStatementInformationDto = new ERPBankStatementInformationDto();
		InitializeParameterLists();
		string[] collection = new string[23]
		{
			"glsBankAccountID", "glsBankStatementReference", "glsCashGlAccountID", "glsCreatedBy", "glsCreatedDate", "glsCurrencyRateID", "glsEndingBalance", "glsEndingBalanceForeign", "glsEndingDate", "glsUniqueID",
			"glsExchangeAmount", "glsExchangeGlAccountID", "glsExchangeRate", "glsGlFiscalYearID", "glsCustomRate", "glsPostedToGl", "glsOpeningBalance", "glsOpeningBalanceForeign", "glsOpeningDate", "glsPostedDate",
			"glsRowVersion", "glsBankStatementID", "glsShowTransactions"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("glsUniqueID|C", bankStatementId);
		AddCustomFieldsToSelectList("BankStatements");
		using (DataTable dataTable = GetAsDataTable("BankStatements", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPBankStatementInformationDto);
			}
			eRPBankStatementInformationDto.glsBankAccountID = dataTable.Rows[0].Field<string>("glsBankAccountID");
			eRPBankStatementInformationDto.glsBankStatementReference = dataTable.Rows[0].Field<int>("glsBankStatementReference");
			eRPBankStatementInformationDto.glsCashGlAccountID = dataTable.Rows[0].Field<string>("glsCashGlAccountID");
			eRPBankStatementInformationDto.glsCreatedBy = dataTable.Rows[0].Field<string>("glsCreatedBy");
			eRPBankStatementInformationDto.glsCreatedDate = dataTable.Rows[0].Field<DateTime?>("glsCreatedDate");
			eRPBankStatementInformationDto.glsCurrencyRateID = dataTable.Rows[0].Field<string>("glsCurrencyRateID");
			eRPBankStatementInformationDto.glsEndingBalance = dataTable.Rows[0].Field<decimal>("glsEndingBalance");
			eRPBankStatementInformationDto.glsEndingBalanceForeign = dataTable.Rows[0].Field<decimal>("glsEndingBalanceForeign");
			eRPBankStatementInformationDto.glsEndingDate = dataTable.Rows[0].Field<DateTime?>("glsEndingDate");
			eRPBankStatementInformationDto.glsUniqueID = dataTable.Rows[0].Field<Guid>("glsUniqueID");
			eRPBankStatementInformationDto.glsExchangeAmount = dataTable.Rows[0].Field<decimal>("glsExchangeAmount");
			eRPBankStatementInformationDto.glsExchangeGlAccountID = dataTable.Rows[0].Field<string>("glsExchangeGlAccountID");
			eRPBankStatementInformationDto.glsExchangeRate = dataTable.Rows[0].Field<decimal>("glsExchangeRate");
			eRPBankStatementInformationDto.glsGlFiscalYearID = dataTable.Rows[0].Field<short>("glsGlFiscalYearID");
			eRPBankStatementInformationDto.glsCustomRate = dataTable.Rows[0].Field<bool>("glsCustomRate");
			eRPBankStatementInformationDto.glsPostedToGl = dataTable.Rows[0].Field<bool>("glsPostedToGl");
			eRPBankStatementInformationDto.glsOpeningBalance = dataTable.Rows[0].Field<decimal>("glsOpeningBalance");
			eRPBankStatementInformationDto.glsOpeningBalanceForeign = dataTable.Rows[0].Field<decimal>("glsOpeningBalanceForeign");
			eRPBankStatementInformationDto.glsOpeningDate = dataTable.Rows[0].Field<DateTime?>("glsOpeningDate");
			eRPBankStatementInformationDto.glsPostedDate = dataTable.Rows[0].Field<DateTime?>("glsPostedDate");
			eRPBankStatementInformationDto.glsRowVersion = dataTable.Rows[0].Field<byte[]>("glsRowVersion");
			eRPBankStatementInformationDto.glsBankStatementID = dataTable.Rows[0].Field<int>("glsBankStatementID");
			eRPBankStatementInformationDto.glsShowTransactions = dataTable.Rows[0].Field<bool>("glsShowTransactions");
			eRPBankStatementInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPBankStatementInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPBankStatementInformationDto);
	}

	public Task<APIValidationInfoDto> SaveBankStatement(ERPBankStatementDto bankStatement)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM BankStatements WHERE glsUniqueID = " + M1Util.ConvertToLinq(bankStatement.glsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glsBankStatementID"] = bankStatement.glsBankStatementID;
				bankStatement.glsUniqueID = ((bankStatement.glsUniqueID == Guid.Empty) ? Guid.NewGuid() : bankStatement.glsUniqueID);
				dataRow["glsUniqueID"] = bankStatement.glsUniqueID;
				dataRow["glsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The BankStatement could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (bankStatement.glsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the BankStatement is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glsRowVersion"], bankStatement.glsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the BankStatement has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the BankStatement again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glsBankAccountID"] = bankStatement.glsBankAccountID;
			dataRow["glsBankStatementReference"] = bankStatement.glsBankStatementReference;
			dataRow["glsCashGlAccountID"] = bankStatement.glsCashGlAccountID;
			dataRow["glsCurrencyRateID"] = bankStatement.glsCurrencyRateID;
			dataRow["glsEndingBalance"] = bankStatement.glsEndingBalance;
			dataRow["glsEndingBalanceForeign"] = bankStatement.glsEndingBalanceForeign;
			DataRow dataRow2 = dataRow;
			DateTime? glsEndingDate = bankStatement.glsEndingDate;
			dataRow2["glsEndingDate"] = (glsEndingDate.HasValue ? ((object)glsEndingDate.GetValueOrDefault()) : dataRow["glsEndingDate"]);
			dataRow["glsExchangeAmount"] = bankStatement.glsExchangeAmount;
			dataRow["glsExchangeGlAccountID"] = bankStatement.glsExchangeGlAccountID;
			dataRow["glsExchangeRate"] = bankStatement.glsExchangeRate;
			dataRow["glsGlFiscalYearID"] = bankStatement.glsGlFiscalYearID;
			dataRow["glsCustomRate"] = bankStatement.glsCustomRate;
			dataRow["glsPostedToGl"] = bankStatement.glsPostedToGl;
			dataRow["glsOpeningBalance"] = bankStatement.glsOpeningBalance;
			dataRow["glsOpeningBalanceForeign"] = bankStatement.glsOpeningBalanceForeign;
			DataRow dataRow3 = dataRow;
			glsEndingDate = bankStatement.glsOpeningDate;
			dataRow3["glsOpeningDate"] = (glsEndingDate.HasValue ? ((object)glsEndingDate.GetValueOrDefault()) : dataRow["glsOpeningDate"]);
			DataRow dataRow4 = dataRow;
			glsEndingDate = bankStatement.glsPostedDate;
			dataRow4["glsPostedDate"] = (glsEndingDate.HasValue ? ((object)glsEndingDate.GetValueOrDefault()) : dataRow["glsPostedDate"]);
			dataRow["glsShowTransactions"] = bankStatement.glsShowTransactions;
			if (bankStatement.CustomFields != null && bankStatement.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in bankStatement.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the BankStatement [{bankStatement.glsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the BankStatement [{bankStatement.glsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
