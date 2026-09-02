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

public class ERPBankAccountRepository : APIBaseRepository, IERPBankAccountRepository, IAPIBaseRepository, IDisposable
{
	public ERPBankAccountRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesBankAccountExist(Guid bankAccountId)
	{
		InitializeParameterLists();
		base.filterList.Add("glnUniqueID|C", bankAccountId);
		base.selectList.Add("glnUniqueID");
		return Task.FromResult(GetAsObject("BankAccounts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPBankAccountInformationDto>> GetAllBankAccounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPBankAccountInformationDto> collection = new List<ERPBankAccountInformationDto>();
		InitializeParameterLists();
		string[] array = new string[39]
		{
			"glnBankAccountName", "glnBankAccountNumber", "glnBankInitials", "glnBankName", "glnBic", "glnBsbNumber", "glnCanadianEftType", "glnCashGlAccountID", "glnBankAccountID", "glnCreatedBy",
			"glnCreatedDate", "glnCurrencyRateID", "glnDataCenterCode", "glnDescription", "glnDirectEntryUserID", "glnDirectEntryUserName", "glnEftApDescription", "glnEftCompanyID", "glnEftCompanyName", "glnEftDiscretionaryData",
			"glnEftFileID", "glnEftFileIDModifier", "glnEftFileLocation", "glnEftPayrollDescription", "glnEftReferenceCode", "glnUniqueID", "glnFileCreationNumber", "glnIban", "glnInactiveDate", "glnAChFormat",
			"glnInactive", "glnEftCreateOffsettingDebit", "glnPayrollOnly", "glnLanguageCode", "glnNextEftNumber", "glnNextPaymentNumber", "glnNZEftType", "glnOrganizationID", "glnRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("BankAccounts");
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
		using (DataTable dataTable = GetAsDataTable("BankAccounts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPBankAccountInformationDto eRPBankAccountInformationDto = new ERPBankAccountInformationDto();
				eRPBankAccountInformationDto.glnBankAccountName = dataTable.Rows[i].Field<string>("glnBankAccountName");
				eRPBankAccountInformationDto.glnBankAccountNumber = dataTable.Rows[i].Field<string>("glnBankAccountNumber");
				eRPBankAccountInformationDto.glnBankInitials = dataTable.Rows[i].Field<string>("glnBankInitials");
				eRPBankAccountInformationDto.glnBankName = dataTable.Rows[i].Field<string>("glnBankName");
				eRPBankAccountInformationDto.glnBic = dataTable.Rows[i].Field<string>("glnBic");
				eRPBankAccountInformationDto.glnBsbNumber = dataTable.Rows[i].Field<string>("glnBsbNumber");
				eRPBankAccountInformationDto.glnCanadianEftType = dataTable.Rows[i].Field<string>("glnCanadianEftType");
				eRPBankAccountInformationDto.glnCashGlAccountID = dataTable.Rows[i].Field<string>("glnCashGlAccountID");
				eRPBankAccountInformationDto.glnBankAccountID = dataTable.Rows[i].Field<string>("glnBankAccountID");
				eRPBankAccountInformationDto.glnCreatedBy = dataTable.Rows[i].Field<string>("glnCreatedBy");
				eRPBankAccountInformationDto.glnCreatedDate = dataTable.Rows[i].Field<DateTime?>("glnCreatedDate");
				eRPBankAccountInformationDto.glnCurrencyRateID = dataTable.Rows[i].Field<string>("glnCurrencyRateID");
				eRPBankAccountInformationDto.glnDataCenterCode = dataTable.Rows[i].Field<decimal>("glnDataCenterCode");
				eRPBankAccountInformationDto.glnDescription = dataTable.Rows[i].Field<string>("glnDescription");
				eRPBankAccountInformationDto.glnDirectEntryUserID = dataTable.Rows[i].Field<string>("glnDirectEntryUserID");
				eRPBankAccountInformationDto.glnDirectEntryUserName = dataTable.Rows[i].Field<string>("glnDirectEntryUserName");
				eRPBankAccountInformationDto.glnEftApDescription = dataTable.Rows[i].Field<string>("glnEftApDescription");
				eRPBankAccountInformationDto.glnEftCompanyID = dataTable.Rows[i].Field<string>("glnEftCompanyID");
				eRPBankAccountInformationDto.glnEftCompanyName = dataTable.Rows[i].Field<string>("glnEftCompanyName");
				eRPBankAccountInformationDto.glnEftDiscretionaryData = dataTable.Rows[i].Field<string>("glnEftDiscretionaryData");
				eRPBankAccountInformationDto.glnEftFileID = dataTable.Rows[i].Field<string>("glnEftFileID");
				eRPBankAccountInformationDto.glnEftFileIDModifier = dataTable.Rows[i].Field<string>("glnEftFileIDModifier");
				eRPBankAccountInformationDto.glnEftFileLocation = dataTable.Rows[i].Field<string>("glnEftFileLocation");
				eRPBankAccountInformationDto.glnEftPayrollDescription = dataTable.Rows[i].Field<string>("glnEftPayrollDescription");
				eRPBankAccountInformationDto.glnEftReferenceCode = dataTable.Rows[i].Field<string>("glnEftReferenceCode");
				eRPBankAccountInformationDto.glnUniqueID = dataTable.Rows[i].Field<Guid>("glnUniqueID");
				eRPBankAccountInformationDto.glnFileCreationNumber = dataTable.Rows[i].Field<short>("glnFileCreationNumber");
				eRPBankAccountInformationDto.glnIban = dataTable.Rows[i].Field<string>("glnIban");
				eRPBankAccountInformationDto.glnInactiveDate = dataTable.Rows[i].Field<DateTime?>("glnInactiveDate");
				eRPBankAccountInformationDto.glnAChFormat = dataTable.Rows[i].Field<bool>("glnAChFormat");
				eRPBankAccountInformationDto.glnInactive = dataTable.Rows[i].Field<bool>("glnInactive");
				eRPBankAccountInformationDto.glnEftCreateOffsettingDebit = dataTable.Rows[i].Field<bool>("glnEftCreateOffsettingDebit");
				eRPBankAccountInformationDto.glnPayrollOnly = dataTable.Rows[i].Field<bool>("glnPayrollOnly");
				eRPBankAccountInformationDto.glnLanguageCode = dataTable.Rows[i].Field<string>("glnLanguageCode");
				eRPBankAccountInformationDto.glnNextEftNumber = dataTable.Rows[i].Field<int>("glnNextEftNumber");
				eRPBankAccountInformationDto.glnNextPaymentNumber = dataTable.Rows[i].Field<int>("glnNextPaymentNumber");
				eRPBankAccountInformationDto.glnNZEftType = dataTable.Rows[i].Field<string>("glnNZEftType");
				eRPBankAccountInformationDto.glnOrganizationID = dataTable.Rows[i].Field<string>("glnOrganizationID");
				eRPBankAccountInformationDto.glnRowVersion = dataTable.Rows[i].Field<byte[]>("glnRowVersion");
				eRPBankAccountInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPBankAccountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPBankAccountInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPBankAccountInformationDto> GetBankAccount(Guid bankAccountId)
	{
		ERPBankAccountInformationDto eRPBankAccountInformationDto = new ERPBankAccountInformationDto();
		InitializeParameterLists();
		string[] collection = new string[39]
		{
			"glnBankAccountName", "glnBankAccountNumber", "glnBankInitials", "glnBankName", "glnBic", "glnBsbNumber", "glnCanadianEftType", "glnCashGlAccountID", "glnBankAccountID", "glnCreatedBy",
			"glnCreatedDate", "glnCurrencyRateID", "glnDataCenterCode", "glnDescription", "glnDirectEntryUserID", "glnDirectEntryUserName", "glnEftApDescription", "glnEftCompanyID", "glnEftCompanyName", "glnEftDiscretionaryData",
			"glnEftFileID", "glnEftFileIDModifier", "glnEftFileLocation", "glnEftPayrollDescription", "glnEftReferenceCode", "glnUniqueID", "glnFileCreationNumber", "glnIban", "glnInactiveDate", "glnAChFormat",
			"glnInactive", "glnEftCreateOffsettingDebit", "glnPayrollOnly", "glnLanguageCode", "glnNextEftNumber", "glnNextPaymentNumber", "glnNZEftType", "glnOrganizationID", "glnRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("glnUniqueID|C", bankAccountId);
		AddCustomFieldsToSelectList("BankAccounts");
		using (DataTable dataTable = GetAsDataTable("BankAccounts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPBankAccountInformationDto);
			}
			eRPBankAccountInformationDto.glnBankAccountName = dataTable.Rows[0].Field<string>("glnBankAccountName");
			eRPBankAccountInformationDto.glnBankAccountNumber = dataTable.Rows[0].Field<string>("glnBankAccountNumber");
			eRPBankAccountInformationDto.glnBankInitials = dataTable.Rows[0].Field<string>("glnBankInitials");
			eRPBankAccountInformationDto.glnBankName = dataTable.Rows[0].Field<string>("glnBankName");
			eRPBankAccountInformationDto.glnBic = dataTable.Rows[0].Field<string>("glnBic");
			eRPBankAccountInformationDto.glnBsbNumber = dataTable.Rows[0].Field<string>("glnBsbNumber");
			eRPBankAccountInformationDto.glnCanadianEftType = dataTable.Rows[0].Field<string>("glnCanadianEftType");
			eRPBankAccountInformationDto.glnCashGlAccountID = dataTable.Rows[0].Field<string>("glnCashGlAccountID");
			eRPBankAccountInformationDto.glnBankAccountID = dataTable.Rows[0].Field<string>("glnBankAccountID");
			eRPBankAccountInformationDto.glnCreatedBy = dataTable.Rows[0].Field<string>("glnCreatedBy");
			eRPBankAccountInformationDto.glnCreatedDate = dataTable.Rows[0].Field<DateTime?>("glnCreatedDate");
			eRPBankAccountInformationDto.glnCurrencyRateID = dataTable.Rows[0].Field<string>("glnCurrencyRateID");
			eRPBankAccountInformationDto.glnDataCenterCode = dataTable.Rows[0].Field<decimal>("glnDataCenterCode");
			eRPBankAccountInformationDto.glnDescription = dataTable.Rows[0].Field<string>("glnDescription");
			eRPBankAccountInformationDto.glnDirectEntryUserID = dataTable.Rows[0].Field<string>("glnDirectEntryUserID");
			eRPBankAccountInformationDto.glnDirectEntryUserName = dataTable.Rows[0].Field<string>("glnDirectEntryUserName");
			eRPBankAccountInformationDto.glnEftApDescription = dataTable.Rows[0].Field<string>("glnEftApDescription");
			eRPBankAccountInformationDto.glnEftCompanyID = dataTable.Rows[0].Field<string>("glnEftCompanyID");
			eRPBankAccountInformationDto.glnEftCompanyName = dataTable.Rows[0].Field<string>("glnEftCompanyName");
			eRPBankAccountInformationDto.glnEftDiscretionaryData = dataTable.Rows[0].Field<string>("glnEftDiscretionaryData");
			eRPBankAccountInformationDto.glnEftFileID = dataTable.Rows[0].Field<string>("glnEftFileID");
			eRPBankAccountInformationDto.glnEftFileIDModifier = dataTable.Rows[0].Field<string>("glnEftFileIDModifier");
			eRPBankAccountInformationDto.glnEftFileLocation = dataTable.Rows[0].Field<string>("glnEftFileLocation");
			eRPBankAccountInformationDto.glnEftPayrollDescription = dataTable.Rows[0].Field<string>("glnEftPayrollDescription");
			eRPBankAccountInformationDto.glnEftReferenceCode = dataTable.Rows[0].Field<string>("glnEftReferenceCode");
			eRPBankAccountInformationDto.glnUniqueID = dataTable.Rows[0].Field<Guid>("glnUniqueID");
			eRPBankAccountInformationDto.glnFileCreationNumber = dataTable.Rows[0].Field<short>("glnFileCreationNumber");
			eRPBankAccountInformationDto.glnIban = dataTable.Rows[0].Field<string>("glnIban");
			eRPBankAccountInformationDto.glnInactiveDate = dataTable.Rows[0].Field<DateTime?>("glnInactiveDate");
			eRPBankAccountInformationDto.glnAChFormat = dataTable.Rows[0].Field<bool>("glnAChFormat");
			eRPBankAccountInformationDto.glnInactive = dataTable.Rows[0].Field<bool>("glnInactive");
			eRPBankAccountInformationDto.glnEftCreateOffsettingDebit = dataTable.Rows[0].Field<bool>("glnEftCreateOffsettingDebit");
			eRPBankAccountInformationDto.glnPayrollOnly = dataTable.Rows[0].Field<bool>("glnPayrollOnly");
			eRPBankAccountInformationDto.glnLanguageCode = dataTable.Rows[0].Field<string>("glnLanguageCode");
			eRPBankAccountInformationDto.glnNextEftNumber = dataTable.Rows[0].Field<int>("glnNextEftNumber");
			eRPBankAccountInformationDto.glnNextPaymentNumber = dataTable.Rows[0].Field<int>("glnNextPaymentNumber");
			eRPBankAccountInformationDto.glnNZEftType = dataTable.Rows[0].Field<string>("glnNZEftType");
			eRPBankAccountInformationDto.glnOrganizationID = dataTable.Rows[0].Field<string>("glnOrganizationID");
			eRPBankAccountInformationDto.glnRowVersion = dataTable.Rows[0].Field<byte[]>("glnRowVersion");
			eRPBankAccountInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPBankAccountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPBankAccountInformationDto);
	}

	public Task<APIValidationInfoDto> SaveBankAccount(ERPBankAccountDto bankAccount)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM BankAccounts WHERE glnUniqueID = " + M1Util.ConvertToLinq(bankAccount.glnUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glnBankAccountID"] = bankAccount.glnBankAccountID.ToUpper();
				bankAccount.glnUniqueID = ((bankAccount.glnUniqueID == Guid.Empty) ? Guid.NewGuid() : bankAccount.glnUniqueID);
				dataRow["glnUniqueID"] = bankAccount.glnUniqueID;
				dataRow["glnCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glnCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The BankAccount could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (bankAccount.glnRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the BankAccount is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glnRowVersion"], bankAccount.glnRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the BankAccount has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the BankAccount again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glnBankAccountName"] = bankAccount.glnBankAccountName;
			dataRow["glnBankAccountNumber"] = bankAccount.glnBankAccountNumber;
			dataRow["glnBankInitials"] = bankAccount.glnBankInitials;
			dataRow["glnBankName"] = bankAccount.glnBankName;
			dataRow["glnBic"] = bankAccount.glnBic;
			dataRow["glnBsbNumber"] = bankAccount.glnBsbNumber;
			dataRow["glnCanadianEftType"] = bankAccount.glnCanadianEftType;
			dataRow["glnCashGlAccountID"] = bankAccount.glnCashGlAccountID;
			dataRow["glnCurrencyRateID"] = bankAccount.glnCurrencyRateID;
			dataRow["glnDataCenterCode"] = bankAccount.glnDataCenterCode;
			dataRow["glnDescription"] = bankAccount.glnDescription;
			dataRow["glnDirectEntryUserID"] = bankAccount.glnDirectEntryUserID;
			dataRow["glnDirectEntryUserName"] = bankAccount.glnDirectEntryUserName;
			dataRow["glnEftApDescription"] = bankAccount.glnEftApDescription;
			dataRow["glnEftCompanyID"] = bankAccount.glnEftCompanyID;
			dataRow["glnEftCompanyName"] = bankAccount.glnEftCompanyName;
			dataRow["glnEftDiscretionaryData"] = bankAccount.glnEftDiscretionaryData;
			dataRow["glnEftFileID"] = bankAccount.glnEftFileID;
			dataRow["glnEftFileIDModifier"] = bankAccount.glnEftFileIDModifier;
			dataRow["glnEftFileLocation"] = bankAccount.glnEftFileLocation ?? dataRow["glnEftFileLocation"];
			dataRow["glnEftPayrollDescription"] = bankAccount.glnEftPayrollDescription;
			dataRow["glnEftReferenceCode"] = bankAccount.glnEftReferenceCode;
			dataRow["glnFileCreationNumber"] = bankAccount.glnFileCreationNumber;
			dataRow["glnIban"] = bankAccount.glnIban;
			DataRow dataRow2 = dataRow;
			DateTime? glnInactiveDate = bankAccount.glnInactiveDate;
			dataRow2["glnInactiveDate"] = (glnInactiveDate.HasValue ? ((object)glnInactiveDate.GetValueOrDefault()) : dataRow["glnInactiveDate"]);
			dataRow["glnAChFormat"] = bankAccount.glnAChFormat;
			dataRow["glnInactive"] = bankAccount.glnInactive;
			dataRow["glnEftCreateOffsettingDebit"] = bankAccount.glnEftCreateOffsettingDebit;
			dataRow["glnPayrollOnly"] = bankAccount.glnPayrollOnly;
			dataRow["glnLanguageCode"] = bankAccount.glnLanguageCode;
			dataRow["glnNextEftNumber"] = bankAccount.glnNextEftNumber;
			dataRow["glnNextPaymentNumber"] = bankAccount.glnNextPaymentNumber;
			dataRow["glnNZEftType"] = bankAccount.glnNZEftType;
			dataRow["glnOrganizationID"] = bankAccount.glnOrganizationID;
			if (bankAccount.CustomFields != null && bankAccount.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in bankAccount.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the BankAccount [{bankAccount.glnUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the BankAccount [{bankAccount.glnUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
