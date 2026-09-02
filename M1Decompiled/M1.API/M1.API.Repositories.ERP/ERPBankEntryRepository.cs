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

public class ERPBankEntryRepository : APIBaseRepository, IERPBankEntryRepository, IAPIBaseRepository, IDisposable
{
	public ERPBankEntryRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesBankEntryExist(Guid bankEntryId)
	{
		InitializeParameterLists();
		base.filterList.Add("gleUniqueID|C", bankEntryId);
		base.selectList.Add("gleUniqueID");
		return Task.FromResult(GetAsObject("BankEntries", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPBankEntryInformationDto>> GetAllBankEntries(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPBankEntryInformationDto> collection = new List<ERPBankEntryInformationDto>();
		InitializeParameterLists();
		string[] array = new string[45]
		{
			"gleApPaymentHeaderID", "gleApPaymentSessionID", "gleArPaymentHeaderID", "gleArPaymentSessionID", "gleBankStatementID", "gleCashGlAccountID", "gleCreatedBy", "gleCreatedDate", "gleCurrencyRateID", "gleDescription",
			"gleEftReferenceNumber", "gleEntryType", "gleUniqueID", "gleExchangeRate", "gleGlAccountID", "gleGlFiscalYearID", "gleGlFiscalYearPeriodID", "gleGlJournalID", "gleGlJournalLineID", "gleCleared",
			"gleCustomRate", "gleDoNotUpdateGl", "glePostedToGl", "gleUnpresentedPayment", "gleNonTaxReasonID", "gleOrganizationID", "gleOriginalAmount", "gleOriginalAmountForeign", "glePaymentAmount", "glePaymentAmountForeign",
			"glePaymentDate", "glePaymentNumber", "glePayrollHeaderID", "glePayrollSessionID", "glePayType", "glePresentedDate", "gleRowVersion", "gleBankEntryID", "gleSource", "gleTaxAmount",
			"gleTaxAmountForeign", "gleTaxCodeID", "gleTransactionDate", "gleVarianceAmount", "gleVarianceAmountForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("BankEntries");
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
		using (DataTable dataTable = GetAsDataTable("BankEntries", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPBankEntryInformationDto eRPBankEntryInformationDto = new ERPBankEntryInformationDto();
				eRPBankEntryInformationDto.gleApPaymentHeaderID = dataTable.Rows[i].Field<int>("gleApPaymentHeaderID");
				eRPBankEntryInformationDto.gleApPaymentSessionID = dataTable.Rows[i].Field<int>("gleApPaymentSessionID");
				eRPBankEntryInformationDto.gleArPaymentHeaderID = dataTable.Rows[i].Field<int>("gleArPaymentHeaderID");
				eRPBankEntryInformationDto.gleArPaymentSessionID = dataTable.Rows[i].Field<int>("gleArPaymentSessionID");
				eRPBankEntryInformationDto.gleBankStatementID = dataTable.Rows[i].Field<int>("gleBankStatementID");
				eRPBankEntryInformationDto.gleCashGlAccountID = dataTable.Rows[i].Field<string>("gleCashGlAccountID");
				eRPBankEntryInformationDto.gleCreatedBy = dataTable.Rows[i].Field<string>("gleCreatedBy");
				eRPBankEntryInformationDto.gleCreatedDate = dataTable.Rows[i].Field<DateTime?>("gleCreatedDate");
				eRPBankEntryInformationDto.gleCurrencyRateID = dataTable.Rows[i].Field<string>("gleCurrencyRateID");
				eRPBankEntryInformationDto.gleDescription = dataTable.Rows[i].Field<string>("gleDescription");
				eRPBankEntryInformationDto.gleEftReferenceNumber = dataTable.Rows[i].Field<string>("gleEftReferenceNumber");
				eRPBankEntryInformationDto.gleEntryType = dataTable.Rows[i].Field<byte>("gleEntryType");
				eRPBankEntryInformationDto.gleUniqueID = dataTable.Rows[i].Field<Guid>("gleUniqueID");
				eRPBankEntryInformationDto.gleExchangeRate = dataTable.Rows[i].Field<decimal>("gleExchangeRate");
				eRPBankEntryInformationDto.gleGlAccountID = dataTable.Rows[i].Field<string>("gleGlAccountID");
				eRPBankEntryInformationDto.gleGlFiscalYearID = dataTable.Rows[i].Field<short>("gleGlFiscalYearID");
				eRPBankEntryInformationDto.gleGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("gleGlFiscalYearPeriodID");
				eRPBankEntryInformationDto.gleGlJournalID = dataTable.Rows[i].Field<int>("gleGlJournalID");
				eRPBankEntryInformationDto.gleGlJournalLineID = dataTable.Rows[i].Field<int>("gleGlJournalLineID");
				eRPBankEntryInformationDto.gleCleared = dataTable.Rows[i].Field<bool>("gleCleared");
				eRPBankEntryInformationDto.gleCustomRate = dataTable.Rows[i].Field<bool>("gleCustomRate");
				eRPBankEntryInformationDto.gleDoNotUpdateGl = dataTable.Rows[i].Field<bool>("gleDoNotUpdateGl");
				eRPBankEntryInformationDto.glePostedToGl = dataTable.Rows[i].Field<bool>("glePostedToGl");
				eRPBankEntryInformationDto.gleUnpresentedPayment = dataTable.Rows[i].Field<bool>("gleUnpresentedPayment");
				eRPBankEntryInformationDto.gleNonTaxReasonID = dataTable.Rows[i].Field<string>("gleNonTaxReasonID");
				eRPBankEntryInformationDto.gleOrganizationID = dataTable.Rows[i].Field<string>("gleOrganizationID");
				eRPBankEntryInformationDto.gleOriginalAmount = dataTable.Rows[i].Field<decimal>("gleOriginalAmount");
				eRPBankEntryInformationDto.gleOriginalAmountForeign = dataTable.Rows[i].Field<decimal>("gleOriginalAmountForeign");
				eRPBankEntryInformationDto.glePaymentAmount = dataTable.Rows[i].Field<decimal>("glePaymentAmount");
				eRPBankEntryInformationDto.glePaymentAmountForeign = dataTable.Rows[i].Field<decimal>("glePaymentAmountForeign");
				eRPBankEntryInformationDto.glePaymentDate = dataTable.Rows[i].Field<DateTime?>("glePaymentDate");
				eRPBankEntryInformationDto.glePaymentNumber = dataTable.Rows[i].Field<int>("glePaymentNumber");
				eRPBankEntryInformationDto.glePayrollHeaderID = dataTable.Rows[i].Field<int>("glePayrollHeaderID");
				eRPBankEntryInformationDto.glePayrollSessionID = dataTable.Rows[i].Field<int>("glePayrollSessionID");
				eRPBankEntryInformationDto.glePayType = dataTable.Rows[i].Field<byte>("glePayType");
				eRPBankEntryInformationDto.glePresentedDate = dataTable.Rows[i].Field<DateTime?>("glePresentedDate");
				eRPBankEntryInformationDto.gleRowVersion = dataTable.Rows[i].Field<byte[]>("gleRowVersion");
				eRPBankEntryInformationDto.gleBankEntryID = dataTable.Rows[i].Field<int>("gleBankEntryID");
				eRPBankEntryInformationDto.gleSource = dataTable.Rows[i].Field<byte>("gleSource");
				eRPBankEntryInformationDto.gleTaxAmount = dataTable.Rows[i].Field<decimal>("gleTaxAmount");
				eRPBankEntryInformationDto.gleTaxAmountForeign = dataTable.Rows[i].Field<decimal>("gleTaxAmountForeign");
				eRPBankEntryInformationDto.gleTaxCodeID = dataTable.Rows[i].Field<string>("gleTaxCodeID");
				eRPBankEntryInformationDto.gleTransactionDate = dataTable.Rows[i].Field<DateTime?>("gleTransactionDate");
				eRPBankEntryInformationDto.gleVarianceAmount = dataTable.Rows[i].Field<decimal>("gleVarianceAmount");
				eRPBankEntryInformationDto.gleVarianceAmountForeign = dataTable.Rows[i].Field<decimal>("gleVarianceAmountForeign");
				eRPBankEntryInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPBankEntryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPBankEntryInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPBankEntryInformationDto> GetBankEntry(Guid bankEntryId)
	{
		ERPBankEntryInformationDto eRPBankEntryInformationDto = new ERPBankEntryInformationDto();
		InitializeParameterLists();
		string[] collection = new string[45]
		{
			"gleApPaymentHeaderID", "gleApPaymentSessionID", "gleArPaymentHeaderID", "gleArPaymentSessionID", "gleBankStatementID", "gleCashGlAccountID", "gleCreatedBy", "gleCreatedDate", "gleCurrencyRateID", "gleDescription",
			"gleEftReferenceNumber", "gleEntryType", "gleUniqueID", "gleExchangeRate", "gleGlAccountID", "gleGlFiscalYearID", "gleGlFiscalYearPeriodID", "gleGlJournalID", "gleGlJournalLineID", "gleCleared",
			"gleCustomRate", "gleDoNotUpdateGl", "glePostedToGl", "gleUnpresentedPayment", "gleNonTaxReasonID", "gleOrganizationID", "gleOriginalAmount", "gleOriginalAmountForeign", "glePaymentAmount", "glePaymentAmountForeign",
			"glePaymentDate", "glePaymentNumber", "glePayrollHeaderID", "glePayrollSessionID", "glePayType", "glePresentedDate", "gleRowVersion", "gleBankEntryID", "gleSource", "gleTaxAmount",
			"gleTaxAmountForeign", "gleTaxCodeID", "gleTransactionDate", "gleVarianceAmount", "gleVarianceAmountForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("gleUniqueID|C", bankEntryId);
		AddCustomFieldsToSelectList("BankEntries");
		using (DataTable dataTable = GetAsDataTable("BankEntries", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPBankEntryInformationDto);
			}
			eRPBankEntryInformationDto.gleApPaymentHeaderID = dataTable.Rows[0].Field<int>("gleApPaymentHeaderID");
			eRPBankEntryInformationDto.gleApPaymentSessionID = dataTable.Rows[0].Field<int>("gleApPaymentSessionID");
			eRPBankEntryInformationDto.gleArPaymentHeaderID = dataTable.Rows[0].Field<int>("gleArPaymentHeaderID");
			eRPBankEntryInformationDto.gleArPaymentSessionID = dataTable.Rows[0].Field<int>("gleArPaymentSessionID");
			eRPBankEntryInformationDto.gleBankStatementID = dataTable.Rows[0].Field<int>("gleBankStatementID");
			eRPBankEntryInformationDto.gleCashGlAccountID = dataTable.Rows[0].Field<string>("gleCashGlAccountID");
			eRPBankEntryInformationDto.gleCreatedBy = dataTable.Rows[0].Field<string>("gleCreatedBy");
			eRPBankEntryInformationDto.gleCreatedDate = dataTable.Rows[0].Field<DateTime?>("gleCreatedDate");
			eRPBankEntryInformationDto.gleCurrencyRateID = dataTable.Rows[0].Field<string>("gleCurrencyRateID");
			eRPBankEntryInformationDto.gleDescription = dataTable.Rows[0].Field<string>("gleDescription");
			eRPBankEntryInformationDto.gleEftReferenceNumber = dataTable.Rows[0].Field<string>("gleEftReferenceNumber");
			eRPBankEntryInformationDto.gleEntryType = dataTable.Rows[0].Field<byte>("gleEntryType");
			eRPBankEntryInformationDto.gleUniqueID = dataTable.Rows[0].Field<Guid>("gleUniqueID");
			eRPBankEntryInformationDto.gleExchangeRate = dataTable.Rows[0].Field<decimal>("gleExchangeRate");
			eRPBankEntryInformationDto.gleGlAccountID = dataTable.Rows[0].Field<string>("gleGlAccountID");
			eRPBankEntryInformationDto.gleGlFiscalYearID = dataTable.Rows[0].Field<short>("gleGlFiscalYearID");
			eRPBankEntryInformationDto.gleGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("gleGlFiscalYearPeriodID");
			eRPBankEntryInformationDto.gleGlJournalID = dataTable.Rows[0].Field<int>("gleGlJournalID");
			eRPBankEntryInformationDto.gleGlJournalLineID = dataTable.Rows[0].Field<int>("gleGlJournalLineID");
			eRPBankEntryInformationDto.gleCleared = dataTable.Rows[0].Field<bool>("gleCleared");
			eRPBankEntryInformationDto.gleCustomRate = dataTable.Rows[0].Field<bool>("gleCustomRate");
			eRPBankEntryInformationDto.gleDoNotUpdateGl = dataTable.Rows[0].Field<bool>("gleDoNotUpdateGl");
			eRPBankEntryInformationDto.glePostedToGl = dataTable.Rows[0].Field<bool>("glePostedToGl");
			eRPBankEntryInformationDto.gleUnpresentedPayment = dataTable.Rows[0].Field<bool>("gleUnpresentedPayment");
			eRPBankEntryInformationDto.gleNonTaxReasonID = dataTable.Rows[0].Field<string>("gleNonTaxReasonID");
			eRPBankEntryInformationDto.gleOrganizationID = dataTable.Rows[0].Field<string>("gleOrganizationID");
			eRPBankEntryInformationDto.gleOriginalAmount = dataTable.Rows[0].Field<decimal>("gleOriginalAmount");
			eRPBankEntryInformationDto.gleOriginalAmountForeign = dataTable.Rows[0].Field<decimal>("gleOriginalAmountForeign");
			eRPBankEntryInformationDto.glePaymentAmount = dataTable.Rows[0].Field<decimal>("glePaymentAmount");
			eRPBankEntryInformationDto.glePaymentAmountForeign = dataTable.Rows[0].Field<decimal>("glePaymentAmountForeign");
			eRPBankEntryInformationDto.glePaymentDate = dataTable.Rows[0].Field<DateTime?>("glePaymentDate");
			eRPBankEntryInformationDto.glePaymentNumber = dataTable.Rows[0].Field<int>("glePaymentNumber");
			eRPBankEntryInformationDto.glePayrollHeaderID = dataTable.Rows[0].Field<int>("glePayrollHeaderID");
			eRPBankEntryInformationDto.glePayrollSessionID = dataTable.Rows[0].Field<int>("glePayrollSessionID");
			eRPBankEntryInformationDto.glePayType = dataTable.Rows[0].Field<byte>("glePayType");
			eRPBankEntryInformationDto.glePresentedDate = dataTable.Rows[0].Field<DateTime?>("glePresentedDate");
			eRPBankEntryInformationDto.gleRowVersion = dataTable.Rows[0].Field<byte[]>("gleRowVersion");
			eRPBankEntryInformationDto.gleBankEntryID = dataTable.Rows[0].Field<int>("gleBankEntryID");
			eRPBankEntryInformationDto.gleSource = dataTable.Rows[0].Field<byte>("gleSource");
			eRPBankEntryInformationDto.gleTaxAmount = dataTable.Rows[0].Field<decimal>("gleTaxAmount");
			eRPBankEntryInformationDto.gleTaxAmountForeign = dataTable.Rows[0].Field<decimal>("gleTaxAmountForeign");
			eRPBankEntryInformationDto.gleTaxCodeID = dataTable.Rows[0].Field<string>("gleTaxCodeID");
			eRPBankEntryInformationDto.gleTransactionDate = dataTable.Rows[0].Field<DateTime?>("gleTransactionDate");
			eRPBankEntryInformationDto.gleVarianceAmount = dataTable.Rows[0].Field<decimal>("gleVarianceAmount");
			eRPBankEntryInformationDto.gleVarianceAmountForeign = dataTable.Rows[0].Field<decimal>("gleVarianceAmountForeign");
			eRPBankEntryInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPBankEntryInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPBankEntryInformationDto);
	}

	public Task<APIValidationInfoDto> SaveBankEntry(ERPBankEntryDto bankEntry)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM BankEntries WHERE gleUniqueID = " + M1Util.ConvertToLinq(bankEntry.gleUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["gleBankEntryID"] = bankEntry.gleBankEntryID;
				bankEntry.gleUniqueID = ((bankEntry.gleUniqueID == Guid.Empty) ? Guid.NewGuid() : bankEntry.gleUniqueID);
				dataRow["gleUniqueID"] = bankEntry.gleUniqueID;
				dataRow["gleCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["gleCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The BankEntry could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (bankEntry.gleRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the BankEntry is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["gleRowVersion"], bankEntry.gleRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the BankEntry has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the BankEntry again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["gleApPaymentHeaderID"] = bankEntry.gleApPaymentHeaderID;
			dataRow["gleApPaymentSessionID"] = bankEntry.gleApPaymentSessionID;
			dataRow["gleArPaymentHeaderID"] = bankEntry.gleArPaymentHeaderID;
			dataRow["gleArPaymentSessionID"] = bankEntry.gleArPaymentSessionID;
			dataRow["gleBankStatementID"] = bankEntry.gleBankStatementID;
			dataRow["gleCashGlAccountID"] = bankEntry.gleCashGlAccountID;
			dataRow["gleCurrencyRateID"] = bankEntry.gleCurrencyRateID;
			dataRow["gleDescription"] = bankEntry.gleDescription;
			dataRow["gleEftReferenceNumber"] = bankEntry.gleEftReferenceNumber;
			dataRow["gleEntryType"] = bankEntry.gleEntryType;
			dataRow["gleExchangeRate"] = bankEntry.gleExchangeRate;
			dataRow["gleGlAccountID"] = bankEntry.gleGlAccountID;
			dataRow["gleGlFiscalYearID"] = bankEntry.gleGlFiscalYearID;
			dataRow["gleGlFiscalYearPeriodID"] = bankEntry.gleGlFiscalYearPeriodID;
			dataRow["gleGlJournalID"] = bankEntry.gleGlJournalID;
			dataRow["gleGlJournalLineID"] = bankEntry.gleGlJournalLineID;
			dataRow["gleCleared"] = bankEntry.gleCleared;
			dataRow["gleCustomRate"] = bankEntry.gleCustomRate;
			dataRow["gleDoNotUpdateGl"] = bankEntry.gleDoNotUpdateGl;
			dataRow["glePostedToGl"] = bankEntry.glePostedToGl;
			dataRow["gleUnpresentedPayment"] = bankEntry.gleUnpresentedPayment;
			dataRow["gleNonTaxReasonID"] = bankEntry.gleNonTaxReasonID;
			dataRow["gleOrganizationID"] = bankEntry.gleOrganizationID;
			dataRow["gleOriginalAmount"] = bankEntry.gleOriginalAmount;
			dataRow["gleOriginalAmountForeign"] = bankEntry.gleOriginalAmountForeign;
			dataRow["glePaymentAmount"] = bankEntry.glePaymentAmount;
			dataRow["glePaymentAmountForeign"] = bankEntry.glePaymentAmountForeign;
			DataRow dataRow2 = dataRow;
			DateTime? glePaymentDate = bankEntry.glePaymentDate;
			dataRow2["glePaymentDate"] = (glePaymentDate.HasValue ? ((object)glePaymentDate.GetValueOrDefault()) : dataRow["glePaymentDate"]);
			dataRow["glePaymentNumber"] = bankEntry.glePaymentNumber;
			dataRow["glePayrollHeaderID"] = bankEntry.glePayrollHeaderID;
			dataRow["glePayrollSessionID"] = bankEntry.glePayrollSessionID;
			dataRow["glePayType"] = bankEntry.glePayType;
			DataRow dataRow3 = dataRow;
			glePaymentDate = bankEntry.glePresentedDate;
			dataRow3["glePresentedDate"] = (glePaymentDate.HasValue ? ((object)glePaymentDate.GetValueOrDefault()) : dataRow["glePresentedDate"]);
			dataRow["gleSource"] = bankEntry.gleSource;
			dataRow["gleTaxAmount"] = bankEntry.gleTaxAmount;
			dataRow["gleTaxAmountForeign"] = bankEntry.gleTaxAmountForeign;
			dataRow["gleTaxCodeID"] = bankEntry.gleTaxCodeID;
			DataRow dataRow4 = dataRow;
			glePaymentDate = bankEntry.gleTransactionDate;
			dataRow4["gleTransactionDate"] = (glePaymentDate.HasValue ? ((object)glePaymentDate.GetValueOrDefault()) : dataRow["gleTransactionDate"]);
			dataRow["gleVarianceAmount"] = bankEntry.gleVarianceAmount;
			dataRow["gleVarianceAmountForeign"] = bankEntry.gleVarianceAmountForeign;
			if (bankEntry.CustomFields != null && bankEntry.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in bankEntry.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the BankEntry [{bankEntry.gleUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the BankEntry [{bankEntry.gleUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
