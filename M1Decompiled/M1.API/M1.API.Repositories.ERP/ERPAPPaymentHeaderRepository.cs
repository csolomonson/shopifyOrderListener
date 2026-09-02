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

public class ERPAPPaymentHeaderRepository : APIBaseRepository, IERPAPPaymentHeaderRepository, IAPIBaseRepository, IDisposable
{
	public ERPAPPaymentHeaderRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAPPaymentHeaderExist(Guid aPPaymentHeaderId)
	{
		InitializeParameterLists();
		base.filterList.Add("aptUniqueID|C", aPPaymentHeaderId);
		base.selectList.Add("aptUniqueID");
		return Task.FromResult(GetAsObject("APPaymentHeaders", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAPPaymentHeaderInformationDto>> GetAllAPPaymentHeaders(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAPPaymentHeaderInformationDto> collection = new List<ERPAPPaymentHeaderInformationDto>();
		InitializeParameterLists();
		string[] array = new string[48]
		{
			"aptApInvoiceContactID", "aptApInvoiceLocationID", "aptApPaymentSessionID", "aptBankAccountName", "aptBankAccountNumber", "aptBankAccountType", "aptBankInitials", "aptBic", "aptBsbNumber", "aptCashGlAccountID",
			"aptCreatedBy", "aptCreatedCreditApInvoiceID", "aptCreatedDate", "aptCreditApInvoiceID", "aptEftCode", "aptEftDescription", "aptEftNumber", "aptEftParticulars", "aptUniqueID", "aptExchangeAmount",
			"aptExchangeGlAccountID", "aptForm1099Box", "aptGlFiscalYearID", "aptGlFiscalYearPeriodID", "aptIban", "aptCompleted", "aptManualPayment", "aptOpenPaymentLoad", "aptOverpayment", "aptPostedToGl",
			"aptSuppressVoid", "aptTaxReportable", "aptVoidedPayment", "aptLongDescriptionRtf", "aptLongDescriptionText", "aptPaymentAmount", "aptPaymentAmountForeign", "aptPaymentDate", "aptPaymentMemo", "aptPaymentNumber",
			"aptPaymentType", "aptRecurringPaymentID", "aptRowVersion", "aptApPaymentHeaderID", "aptShowAllInvoices", "aptSupplierOrganizationID", "aptVoidApPaymentHeaderID", "aptVoidApPaymentSessionID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("APPaymentHeaders");
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
		using (DataTable dataTable = GetAsDataTable("APPaymentHeaders", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAPPaymentHeaderInformationDto eRPAPPaymentHeaderInformationDto = new ERPAPPaymentHeaderInformationDto();
				eRPAPPaymentHeaderInformationDto.aptApInvoiceContactID = dataTable.Rows[i].Field<string>("aptApInvoiceContactID");
				eRPAPPaymentHeaderInformationDto.aptApInvoiceLocationID = dataTable.Rows[i].Field<string>("aptApInvoiceLocationID");
				eRPAPPaymentHeaderInformationDto.aptApPaymentSessionID = dataTable.Rows[i].Field<int>("aptApPaymentSessionID");
				eRPAPPaymentHeaderInformationDto.aptBankAccountName = dataTable.Rows[i].Field<string>("aptBankAccountName");
				eRPAPPaymentHeaderInformationDto.aptBankAccountNumber = dataTable.Rows[i].Field<string>("aptBankAccountNumber");
				eRPAPPaymentHeaderInformationDto.aptBankAccountType = dataTable.Rows[i].Field<string>("aptBankAccountType");
				eRPAPPaymentHeaderInformationDto.aptBankInitials = dataTable.Rows[i].Field<string>("aptBankInitials");
				eRPAPPaymentHeaderInformationDto.aptBic = dataTable.Rows[i].Field<string>("aptBic");
				eRPAPPaymentHeaderInformationDto.aptBsbNumber = dataTable.Rows[i].Field<string>("aptBsbNumber");
				eRPAPPaymentHeaderInformationDto.aptCashGlAccountID = dataTable.Rows[i].Field<string>("aptCashGlAccountID");
				eRPAPPaymentHeaderInformationDto.aptCreatedBy = dataTable.Rows[i].Field<string>("aptCreatedBy");
				eRPAPPaymentHeaderInformationDto.aptCreatedCreditApInvoiceID = dataTable.Rows[i].Field<string>("aptCreatedCreditApInvoiceID");
				eRPAPPaymentHeaderInformationDto.aptCreatedDate = dataTable.Rows[i].Field<DateTime?>("aptCreatedDate");
				eRPAPPaymentHeaderInformationDto.aptCreditApInvoiceID = dataTable.Rows[i].Field<string>("aptCreditApInvoiceID");
				eRPAPPaymentHeaderInformationDto.aptEftCode = dataTable.Rows[i].Field<string>("aptEftCode");
				eRPAPPaymentHeaderInformationDto.aptEftDescription = dataTable.Rows[i].Field<string>("aptEftDescription");
				eRPAPPaymentHeaderInformationDto.aptEftNumber = dataTable.Rows[i].Field<int>("aptEftNumber");
				eRPAPPaymentHeaderInformationDto.aptEftParticulars = dataTable.Rows[i].Field<string>("aptEftParticulars");
				eRPAPPaymentHeaderInformationDto.aptUniqueID = dataTable.Rows[i].Field<Guid>("aptUniqueID");
				eRPAPPaymentHeaderInformationDto.aptExchangeAmount = dataTable.Rows[i].Field<decimal>("aptExchangeAmount");
				eRPAPPaymentHeaderInformationDto.aptExchangeGlAccountID = dataTable.Rows[i].Field<string>("aptExchangeGlAccountID");
				eRPAPPaymentHeaderInformationDto.aptForm1099Box = dataTable.Rows[i].Field<byte>("aptForm1099Box");
				eRPAPPaymentHeaderInformationDto.aptGlFiscalYearID = dataTable.Rows[i].Field<short>("aptGlFiscalYearID");
				eRPAPPaymentHeaderInformationDto.aptGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("aptGlFiscalYearPeriodID");
				eRPAPPaymentHeaderInformationDto.aptIban = dataTable.Rows[i].Field<string>("aptIban");
				eRPAPPaymentHeaderInformationDto.aptCompleted = dataTable.Rows[i].Field<bool>("aptCompleted");
				eRPAPPaymentHeaderInformationDto.aptManualPayment = dataTable.Rows[i].Field<bool>("aptManualPayment");
				eRPAPPaymentHeaderInformationDto.aptOpenPaymentLoad = dataTable.Rows[i].Field<bool>("aptOpenPaymentLoad");
				eRPAPPaymentHeaderInformationDto.aptOverpayment = dataTable.Rows[i].Field<bool>("aptOverpayment");
				eRPAPPaymentHeaderInformationDto.aptPostedToGl = dataTable.Rows[i].Field<bool>("aptPostedToGl");
				eRPAPPaymentHeaderInformationDto.aptSuppressVoid = dataTable.Rows[i].Field<bool>("aptSuppressVoid");
				eRPAPPaymentHeaderInformationDto.aptTaxReportable = dataTable.Rows[i].Field<bool>("aptTaxReportable");
				eRPAPPaymentHeaderInformationDto.aptVoidedPayment = dataTable.Rows[i].Field<bool>("aptVoidedPayment");
				eRPAPPaymentHeaderInformationDto.aptLongDescriptionRtf = dataTable.Rows[i].Field<string>("aptLongDescriptionRtf");
				eRPAPPaymentHeaderInformationDto.aptLongDescriptionText = dataTable.Rows[i].Field<string>("aptLongDescriptionText");
				eRPAPPaymentHeaderInformationDto.aptPaymentAmount = dataTable.Rows[i].Field<decimal>("aptPaymentAmount");
				eRPAPPaymentHeaderInformationDto.aptPaymentAmountForeign = dataTable.Rows[i].Field<decimal>("aptPaymentAmountForeign");
				eRPAPPaymentHeaderInformationDto.aptPaymentDate = dataTable.Rows[i].Field<DateTime?>("aptPaymentDate");
				eRPAPPaymentHeaderInformationDto.aptPaymentMemo = dataTable.Rows[i].Field<string>("aptPaymentMemo");
				eRPAPPaymentHeaderInformationDto.aptPaymentNumber = dataTable.Rows[i].Field<int>("aptPaymentNumber");
				eRPAPPaymentHeaderInformationDto.aptPaymentType = dataTable.Rows[i].Field<byte>("aptPaymentType");
				eRPAPPaymentHeaderInformationDto.aptRecurringPaymentID = dataTable.Rows[i].Field<int>("aptRecurringPaymentID");
				eRPAPPaymentHeaderInformationDto.aptRowVersion = dataTable.Rows[i].Field<byte[]>("aptRowVersion");
				eRPAPPaymentHeaderInformationDto.aptApPaymentHeaderID = dataTable.Rows[i].Field<int>("aptApPaymentHeaderID");
				eRPAPPaymentHeaderInformationDto.aptShowAllInvoices = dataTable.Rows[i].Field<bool>("aptShowAllInvoices");
				eRPAPPaymentHeaderInformationDto.aptSupplierOrganizationID = dataTable.Rows[i].Field<string>("aptSupplierOrganizationID");
				eRPAPPaymentHeaderInformationDto.aptVoidApPaymentHeaderID = dataTable.Rows[i].Field<int>("aptVoidApPaymentHeaderID");
				eRPAPPaymentHeaderInformationDto.aptVoidApPaymentSessionID = dataTable.Rows[i].Field<int>("aptVoidApPaymentSessionID");
				eRPAPPaymentHeaderInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAPPaymentHeaderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAPPaymentHeaderInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAPPaymentHeaderInformationDto> GetAPPaymentHeader(Guid aPPaymentHeaderId)
	{
		ERPAPPaymentHeaderInformationDto eRPAPPaymentHeaderInformationDto = new ERPAPPaymentHeaderInformationDto();
		InitializeParameterLists();
		string[] collection = new string[48]
		{
			"aptApInvoiceContactID", "aptApInvoiceLocationID", "aptApPaymentSessionID", "aptBankAccountName", "aptBankAccountNumber", "aptBankAccountType", "aptBankInitials", "aptBic", "aptBsbNumber", "aptCashGlAccountID",
			"aptCreatedBy", "aptCreatedCreditApInvoiceID", "aptCreatedDate", "aptCreditApInvoiceID", "aptEftCode", "aptEftDescription", "aptEftNumber", "aptEftParticulars", "aptUniqueID", "aptExchangeAmount",
			"aptExchangeGlAccountID", "aptForm1099Box", "aptGlFiscalYearID", "aptGlFiscalYearPeriodID", "aptIban", "aptCompleted", "aptManualPayment", "aptOpenPaymentLoad", "aptOverpayment", "aptPostedToGl",
			"aptSuppressVoid", "aptTaxReportable", "aptVoidedPayment", "aptLongDescriptionRtf", "aptLongDescriptionText", "aptPaymentAmount", "aptPaymentAmountForeign", "aptPaymentDate", "aptPaymentMemo", "aptPaymentNumber",
			"aptPaymentType", "aptRecurringPaymentID", "aptRowVersion", "aptApPaymentHeaderID", "aptShowAllInvoices", "aptSupplierOrganizationID", "aptVoidApPaymentHeaderID", "aptVoidApPaymentSessionID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("aptUniqueID|C", aPPaymentHeaderId);
		AddCustomFieldsToSelectList("APPaymentHeaders");
		using (DataTable dataTable = GetAsDataTable("APPaymentHeaders", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAPPaymentHeaderInformationDto);
			}
			eRPAPPaymentHeaderInformationDto.aptApInvoiceContactID = dataTable.Rows[0].Field<string>("aptApInvoiceContactID");
			eRPAPPaymentHeaderInformationDto.aptApInvoiceLocationID = dataTable.Rows[0].Field<string>("aptApInvoiceLocationID");
			eRPAPPaymentHeaderInformationDto.aptApPaymentSessionID = dataTable.Rows[0].Field<int>("aptApPaymentSessionID");
			eRPAPPaymentHeaderInformationDto.aptBankAccountName = dataTable.Rows[0].Field<string>("aptBankAccountName");
			eRPAPPaymentHeaderInformationDto.aptBankAccountNumber = dataTable.Rows[0].Field<string>("aptBankAccountNumber");
			eRPAPPaymentHeaderInformationDto.aptBankAccountType = dataTable.Rows[0].Field<string>("aptBankAccountType");
			eRPAPPaymentHeaderInformationDto.aptBankInitials = dataTable.Rows[0].Field<string>("aptBankInitials");
			eRPAPPaymentHeaderInformationDto.aptBic = dataTable.Rows[0].Field<string>("aptBic");
			eRPAPPaymentHeaderInformationDto.aptBsbNumber = dataTable.Rows[0].Field<string>("aptBsbNumber");
			eRPAPPaymentHeaderInformationDto.aptCashGlAccountID = dataTable.Rows[0].Field<string>("aptCashGlAccountID");
			eRPAPPaymentHeaderInformationDto.aptCreatedBy = dataTable.Rows[0].Field<string>("aptCreatedBy");
			eRPAPPaymentHeaderInformationDto.aptCreatedCreditApInvoiceID = dataTable.Rows[0].Field<string>("aptCreatedCreditApInvoiceID");
			eRPAPPaymentHeaderInformationDto.aptCreatedDate = dataTable.Rows[0].Field<DateTime?>("aptCreatedDate");
			eRPAPPaymentHeaderInformationDto.aptCreditApInvoiceID = dataTable.Rows[0].Field<string>("aptCreditApInvoiceID");
			eRPAPPaymentHeaderInformationDto.aptEftCode = dataTable.Rows[0].Field<string>("aptEftCode");
			eRPAPPaymentHeaderInformationDto.aptEftDescription = dataTable.Rows[0].Field<string>("aptEftDescription");
			eRPAPPaymentHeaderInformationDto.aptEftNumber = dataTable.Rows[0].Field<int>("aptEftNumber");
			eRPAPPaymentHeaderInformationDto.aptEftParticulars = dataTable.Rows[0].Field<string>("aptEftParticulars");
			eRPAPPaymentHeaderInformationDto.aptUniqueID = dataTable.Rows[0].Field<Guid>("aptUniqueID");
			eRPAPPaymentHeaderInformationDto.aptExchangeAmount = dataTable.Rows[0].Field<decimal>("aptExchangeAmount");
			eRPAPPaymentHeaderInformationDto.aptExchangeGlAccountID = dataTable.Rows[0].Field<string>("aptExchangeGlAccountID");
			eRPAPPaymentHeaderInformationDto.aptForm1099Box = dataTable.Rows[0].Field<byte>("aptForm1099Box");
			eRPAPPaymentHeaderInformationDto.aptGlFiscalYearID = dataTable.Rows[0].Field<short>("aptGlFiscalYearID");
			eRPAPPaymentHeaderInformationDto.aptGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("aptGlFiscalYearPeriodID");
			eRPAPPaymentHeaderInformationDto.aptIban = dataTable.Rows[0].Field<string>("aptIban");
			eRPAPPaymentHeaderInformationDto.aptCompleted = dataTable.Rows[0].Field<bool>("aptCompleted");
			eRPAPPaymentHeaderInformationDto.aptManualPayment = dataTable.Rows[0].Field<bool>("aptManualPayment");
			eRPAPPaymentHeaderInformationDto.aptOpenPaymentLoad = dataTable.Rows[0].Field<bool>("aptOpenPaymentLoad");
			eRPAPPaymentHeaderInformationDto.aptOverpayment = dataTable.Rows[0].Field<bool>("aptOverpayment");
			eRPAPPaymentHeaderInformationDto.aptPostedToGl = dataTable.Rows[0].Field<bool>("aptPostedToGl");
			eRPAPPaymentHeaderInformationDto.aptSuppressVoid = dataTable.Rows[0].Field<bool>("aptSuppressVoid");
			eRPAPPaymentHeaderInformationDto.aptTaxReportable = dataTable.Rows[0].Field<bool>("aptTaxReportable");
			eRPAPPaymentHeaderInformationDto.aptVoidedPayment = dataTable.Rows[0].Field<bool>("aptVoidedPayment");
			eRPAPPaymentHeaderInformationDto.aptLongDescriptionRtf = dataTable.Rows[0].Field<string>("aptLongDescriptionRtf");
			eRPAPPaymentHeaderInformationDto.aptLongDescriptionText = dataTable.Rows[0].Field<string>("aptLongDescriptionText");
			eRPAPPaymentHeaderInformationDto.aptPaymentAmount = dataTable.Rows[0].Field<decimal>("aptPaymentAmount");
			eRPAPPaymentHeaderInformationDto.aptPaymentAmountForeign = dataTable.Rows[0].Field<decimal>("aptPaymentAmountForeign");
			eRPAPPaymentHeaderInformationDto.aptPaymentDate = dataTable.Rows[0].Field<DateTime?>("aptPaymentDate");
			eRPAPPaymentHeaderInformationDto.aptPaymentMemo = dataTable.Rows[0].Field<string>("aptPaymentMemo");
			eRPAPPaymentHeaderInformationDto.aptPaymentNumber = dataTable.Rows[0].Field<int>("aptPaymentNumber");
			eRPAPPaymentHeaderInformationDto.aptPaymentType = dataTable.Rows[0].Field<byte>("aptPaymentType");
			eRPAPPaymentHeaderInformationDto.aptRecurringPaymentID = dataTable.Rows[0].Field<int>("aptRecurringPaymentID");
			eRPAPPaymentHeaderInformationDto.aptRowVersion = dataTable.Rows[0].Field<byte[]>("aptRowVersion");
			eRPAPPaymentHeaderInformationDto.aptApPaymentHeaderID = dataTable.Rows[0].Field<int>("aptApPaymentHeaderID");
			eRPAPPaymentHeaderInformationDto.aptShowAllInvoices = dataTable.Rows[0].Field<bool>("aptShowAllInvoices");
			eRPAPPaymentHeaderInformationDto.aptSupplierOrganizationID = dataTable.Rows[0].Field<string>("aptSupplierOrganizationID");
			eRPAPPaymentHeaderInformationDto.aptVoidApPaymentHeaderID = dataTable.Rows[0].Field<int>("aptVoidApPaymentHeaderID");
			eRPAPPaymentHeaderInformationDto.aptVoidApPaymentSessionID = dataTable.Rows[0].Field<int>("aptVoidApPaymentSessionID");
			eRPAPPaymentHeaderInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAPPaymentHeaderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAPPaymentHeaderInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAPPaymentHeader(ERPAPPaymentHeaderDto aPPaymentHeader)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM APPaymentHeaders WHERE aptUniqueID = " + M1Util.ConvertToLinq(aPPaymentHeader.aptUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["aptApPaymentSessionID"] = aPPaymentHeader.aptApPaymentSessionID;
				dataRow["aptApPaymentHeaderID"] = aPPaymentHeader.aptApPaymentHeaderID;
				aPPaymentHeader.aptUniqueID = ((aPPaymentHeader.aptUniqueID == Guid.Empty) ? Guid.NewGuid() : aPPaymentHeader.aptUniqueID);
				dataRow["aptUniqueID"] = aPPaymentHeader.aptUniqueID;
				dataRow["aptCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["aptCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The APPaymentHeader could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aPPaymentHeader.aptRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the APPaymentHeader is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["aptRowVersion"], aPPaymentHeader.aptRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the APPaymentHeader has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the APPaymentHeader again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["aptApInvoiceContactID"] = aPPaymentHeader.aptApInvoiceContactID;
			dataRow["aptApInvoiceLocationID"] = aPPaymentHeader.aptApInvoiceLocationID;
			dataRow["aptBankAccountName"] = aPPaymentHeader.aptBankAccountName;
			dataRow["aptBankAccountNumber"] = aPPaymentHeader.aptBankAccountNumber;
			dataRow["aptBankAccountType"] = aPPaymentHeader.aptBankAccountType;
			dataRow["aptBankInitials"] = aPPaymentHeader.aptBankInitials;
			dataRow["aptBic"] = aPPaymentHeader.aptBic;
			dataRow["aptBsbNumber"] = aPPaymentHeader.aptBsbNumber;
			dataRow["aptCashGlAccountID"] = aPPaymentHeader.aptCashGlAccountID;
			dataRow["aptCreatedCreditApInvoiceID"] = aPPaymentHeader.aptCreatedCreditApInvoiceID;
			dataRow["aptCreditApInvoiceID"] = aPPaymentHeader.aptCreditApInvoiceID;
			dataRow["aptEftCode"] = aPPaymentHeader.aptEftCode;
			dataRow["aptEftDescription"] = aPPaymentHeader.aptEftDescription;
			dataRow["aptEftNumber"] = aPPaymentHeader.aptEftNumber;
			dataRow["aptEftParticulars"] = aPPaymentHeader.aptEftParticulars;
			dataRow["aptExchangeAmount"] = aPPaymentHeader.aptExchangeAmount;
			dataRow["aptExchangeGlAccountID"] = aPPaymentHeader.aptExchangeGlAccountID;
			dataRow["aptForm1099Box"] = aPPaymentHeader.aptForm1099Box;
			dataRow["aptGlFiscalYearID"] = aPPaymentHeader.aptGlFiscalYearID;
			dataRow["aptGlFiscalYearPeriodID"] = aPPaymentHeader.aptGlFiscalYearPeriodID;
			dataRow["aptIban"] = aPPaymentHeader.aptIban;
			dataRow["aptCompleted"] = aPPaymentHeader.aptCompleted;
			dataRow["aptManualPayment"] = aPPaymentHeader.aptManualPayment;
			dataRow["aptOpenPaymentLoad"] = aPPaymentHeader.aptOpenPaymentLoad;
			dataRow["aptOverpayment"] = aPPaymentHeader.aptOverpayment;
			dataRow["aptPostedToGl"] = aPPaymentHeader.aptPostedToGl;
			dataRow["aptSuppressVoid"] = aPPaymentHeader.aptSuppressVoid;
			dataRow["aptTaxReportable"] = aPPaymentHeader.aptTaxReportable;
			dataRow["aptVoidedPayment"] = aPPaymentHeader.aptVoidedPayment;
			dataRow["aptLongDescriptionRtf"] = aPPaymentHeader.aptLongDescriptionRtf ?? dataRow["aptLongDescriptionRtf"];
			dataRow["aptLongDescriptionText"] = aPPaymentHeader.aptLongDescriptionText ?? dataRow["aptLongDescriptionText"];
			dataRow["aptPaymentAmount"] = aPPaymentHeader.aptPaymentAmount;
			dataRow["aptPaymentAmountForeign"] = aPPaymentHeader.aptPaymentAmountForeign;
			DataRow dataRow2 = dataRow;
			DateTime? aptPaymentDate = aPPaymentHeader.aptPaymentDate;
			dataRow2["aptPaymentDate"] = (aptPaymentDate.HasValue ? ((object)aptPaymentDate.GetValueOrDefault()) : dataRow["aptPaymentDate"]);
			dataRow["aptPaymentMemo"] = aPPaymentHeader.aptPaymentMemo;
			dataRow["aptPaymentNumber"] = aPPaymentHeader.aptPaymentNumber;
			dataRow["aptPaymentType"] = aPPaymentHeader.aptPaymentType;
			dataRow["aptRecurringPaymentID"] = aPPaymentHeader.aptRecurringPaymentID;
			dataRow["aptShowAllInvoices"] = aPPaymentHeader.aptShowAllInvoices;
			dataRow["aptSupplierOrganizationID"] = aPPaymentHeader.aptSupplierOrganizationID;
			dataRow["aptVoidApPaymentHeaderID"] = aPPaymentHeader.aptVoidApPaymentHeaderID;
			dataRow["aptVoidApPaymentSessionID"] = aPPaymentHeader.aptVoidApPaymentSessionID;
			if (aPPaymentHeader.CustomFields != null && aPPaymentHeader.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aPPaymentHeader.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the APPaymentHeader [{aPPaymentHeader.aptUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the APPaymentHeader [{aPPaymentHeader.aptUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
