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

public class ERPARPaymentHeaderRepository : APIBaseRepository, IERPARPaymentHeaderRepository, IAPIBaseRepository, IDisposable
{
	public ERPARPaymentHeaderRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesARPaymentHeaderExist(Guid aRPaymentHeaderId)
	{
		InitializeParameterLists();
		base.filterList.Add("artUniqueID|C", aRPaymentHeaderId);
		base.selectList.Add("artUniqueID");
		return Task.FromResult(GetAsObject("ARPaymentHeaders", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPARPaymentHeaderInformationDto>> GetAllARPaymentHeaders(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPARPaymentHeaderInformationDto> collection = new List<ERPARPaymentHeaderInformationDto>();
		InitializeParameterLists();
		string[] array = new string[46]
		{
			"artArGlAccountID", "artArInvoiceContactID", "artArInvoiceLocationID", "artArPaymentSessionID", "artBankAccountName", "artBankAccountNumber", "artBankInitials", "artBsbNumber", "artCashGlAccountID", "artCreatedBy",
			"artCreatedCreditArInvoiceID", "artCreatedDate", "artCreditArInvoiceID", "artCustomerOrganizationID", "artCustomerPaymentNumber", "artDescription", "artUniqueID", "artExchangeAmount", "artExchangeGlAccountID", "artGlAccountID",
			"artGlFiscalYearID", "artGlFiscalYearPeriodID", "artAvalaraTaxCalculated", "artNet1PaymentProcessed", "artOpenPaymentLoad", "artPostedToGl", "artVoidedPayment", "artLongDescriptionRtf", "artLongDescriptionText", "artNonTaxReasonID",
			"artPaymentMethod", "artReceiptAmount", "artReceiptAmountForeign", "artReceiptDate", "artReceiptType", "artRowVersion", "artSecondTaxAmount", "artSecondTaxAmountForeign", "artSecondTaxCodeID", "artArPaymentHeaderID",
			"artShowAllInvoices", "artTaxAmount", "artTaxAmountForeign", "artTaxCodeID", "artVoidArPaymentHeaderId", "artVoidArPaymentSessionID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ARPaymentHeaders");
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
		using (DataTable dataTable = GetAsDataTable("ARPaymentHeaders", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPARPaymentHeaderInformationDto eRPARPaymentHeaderInformationDto = new ERPARPaymentHeaderInformationDto();
				eRPARPaymentHeaderInformationDto.artArGlAccountID = dataTable.Rows[i].Field<string>("artArGlAccountID");
				eRPARPaymentHeaderInformationDto.artArInvoiceContactID = dataTable.Rows[i].Field<string>("artArInvoiceContactID");
				eRPARPaymentHeaderInformationDto.artArInvoiceLocationID = dataTable.Rows[i].Field<string>("artArInvoiceLocationID");
				eRPARPaymentHeaderInformationDto.artArPaymentSessionID = dataTable.Rows[i].Field<int>("artArPaymentSessionID");
				eRPARPaymentHeaderInformationDto.artBankAccountName = dataTable.Rows[i].Field<string>("artBankAccountName");
				eRPARPaymentHeaderInformationDto.artBankAccountNumber = dataTable.Rows[i].Field<string>("artBankAccountNumber");
				eRPARPaymentHeaderInformationDto.artBankInitials = dataTable.Rows[i].Field<string>("artBankInitials");
				eRPARPaymentHeaderInformationDto.artBsbNumber = dataTable.Rows[i].Field<string>("artBsbNumber");
				eRPARPaymentHeaderInformationDto.artCashGlAccountID = dataTable.Rows[i].Field<string>("artCashGlAccountID");
				eRPARPaymentHeaderInformationDto.artCreatedBy = dataTable.Rows[i].Field<string>("artCreatedBy");
				eRPARPaymentHeaderInformationDto.artCreatedCreditArInvoiceID = dataTable.Rows[i].Field<string>("artCreatedCreditArInvoiceID");
				eRPARPaymentHeaderInformationDto.artCreatedDate = dataTable.Rows[i].Field<DateTime?>("artCreatedDate");
				eRPARPaymentHeaderInformationDto.artCreditArInvoiceID = dataTable.Rows[i].Field<string>("artCreditArInvoiceID");
				eRPARPaymentHeaderInformationDto.artCustomerOrganizationID = dataTable.Rows[i].Field<string>("artCustomerOrganizationID");
				eRPARPaymentHeaderInformationDto.artCustomerPaymentNumber = dataTable.Rows[i].Field<string>("artCustomerPaymentNumber");
				eRPARPaymentHeaderInformationDto.artDescription = dataTable.Rows[i].Field<string>("artDescription");
				eRPARPaymentHeaderInformationDto.artUniqueID = dataTable.Rows[i].Field<Guid>("artUniqueID");
				eRPARPaymentHeaderInformationDto.artExchangeAmount = dataTable.Rows[i].Field<decimal>("artExchangeAmount");
				eRPARPaymentHeaderInformationDto.artExchangeGlAccountID = dataTable.Rows[i].Field<string>("artExchangeGlAccountID");
				eRPARPaymentHeaderInformationDto.artGlAccountID = dataTable.Rows[i].Field<string>("artGlAccountID");
				eRPARPaymentHeaderInformationDto.artGlFiscalYearID = dataTable.Rows[i].Field<short>("artGlFiscalYearID");
				eRPARPaymentHeaderInformationDto.artGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("artGlFiscalYearPeriodID");
				eRPARPaymentHeaderInformationDto.artAvalaraTaxCalculated = dataTable.Rows[i].Field<bool>("artAvalaraTaxCalculated");
				eRPARPaymentHeaderInformationDto.artNet1PaymentProcessed = dataTable.Rows[i].Field<bool>("artNet1PaymentProcessed");
				eRPARPaymentHeaderInformationDto.artOpenPaymentLoad = dataTable.Rows[i].Field<bool>("artOpenPaymentLoad");
				eRPARPaymentHeaderInformationDto.artPostedToGl = dataTable.Rows[i].Field<bool>("artPostedToGl");
				eRPARPaymentHeaderInformationDto.artVoidedPayment = dataTable.Rows[i].Field<bool>("artVoidedPayment");
				eRPARPaymentHeaderInformationDto.artLongDescriptionRtf = dataTable.Rows[i].Field<string>("artLongDescriptionRtf");
				eRPARPaymentHeaderInformationDto.artLongDescriptionText = dataTable.Rows[i].Field<string>("artLongDescriptionText");
				eRPARPaymentHeaderInformationDto.artNonTaxReasonID = dataTable.Rows[i].Field<string>("artNonTaxReasonID");
				eRPARPaymentHeaderInformationDto.artPaymentMethod = dataTable.Rows[i].Field<byte>("artPaymentMethod");
				eRPARPaymentHeaderInformationDto.artReceiptAmount = dataTable.Rows[i].Field<decimal>("artReceiptAmount");
				eRPARPaymentHeaderInformationDto.artReceiptAmountForeign = dataTable.Rows[i].Field<decimal>("artReceiptAmountForeign");
				eRPARPaymentHeaderInformationDto.artReceiptDate = dataTable.Rows[i].Field<DateTime?>("artReceiptDate");
				eRPARPaymentHeaderInformationDto.artReceiptType = dataTable.Rows[i].Field<byte>("artReceiptType");
				eRPARPaymentHeaderInformationDto.artRowVersion = dataTable.Rows[i].Field<byte[]>("artRowVersion");
				eRPARPaymentHeaderInformationDto.artSecondTaxAmount = dataTable.Rows[i].Field<decimal>("artSecondTaxAmount");
				eRPARPaymentHeaderInformationDto.artSecondTaxAmountForeign = dataTable.Rows[i].Field<decimal>("artSecondTaxAmountForeign");
				eRPARPaymentHeaderInformationDto.artSecondTaxCodeID = dataTable.Rows[i].Field<string>("artSecondTaxCodeID");
				eRPARPaymentHeaderInformationDto.artArPaymentHeaderID = dataTable.Rows[i].Field<int>("artArPaymentHeaderID");
				eRPARPaymentHeaderInformationDto.artShowAllInvoices = dataTable.Rows[i].Field<bool>("artShowAllInvoices");
				eRPARPaymentHeaderInformationDto.artTaxAmount = dataTable.Rows[i].Field<decimal>("artTaxAmount");
				eRPARPaymentHeaderInformationDto.artTaxAmountForeign = dataTable.Rows[i].Field<decimal>("artTaxAmountForeign");
				eRPARPaymentHeaderInformationDto.artTaxCodeID = dataTable.Rows[i].Field<string>("artTaxCodeID");
				eRPARPaymentHeaderInformationDto.artVoidArPaymentHeaderId = dataTable.Rows[i].Field<int>("artVoidArPaymentHeaderId");
				eRPARPaymentHeaderInformationDto.artVoidArPaymentSessionID = dataTable.Rows[i].Field<int>("artVoidArPaymentSessionID");
				eRPARPaymentHeaderInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPARPaymentHeaderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPARPaymentHeaderInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPARPaymentHeaderInformationDto> GetARPaymentHeader(Guid aRPaymentHeaderId)
	{
		ERPARPaymentHeaderInformationDto eRPARPaymentHeaderInformationDto = new ERPARPaymentHeaderInformationDto();
		InitializeParameterLists();
		string[] collection = new string[46]
		{
			"artArGlAccountID", "artArInvoiceContactID", "artArInvoiceLocationID", "artArPaymentSessionID", "artBankAccountName", "artBankAccountNumber", "artBankInitials", "artBsbNumber", "artCashGlAccountID", "artCreatedBy",
			"artCreatedCreditArInvoiceID", "artCreatedDate", "artCreditArInvoiceID", "artCustomerOrganizationID", "artCustomerPaymentNumber", "artDescription", "artUniqueID", "artExchangeAmount", "artExchangeGlAccountID", "artGlAccountID",
			"artGlFiscalYearID", "artGlFiscalYearPeriodID", "artAvalaraTaxCalculated", "artNet1PaymentProcessed", "artOpenPaymentLoad", "artPostedToGl", "artVoidedPayment", "artLongDescriptionRtf", "artLongDescriptionText", "artNonTaxReasonID",
			"artPaymentMethod", "artReceiptAmount", "artReceiptAmountForeign", "artReceiptDate", "artReceiptType", "artRowVersion", "artSecondTaxAmount", "artSecondTaxAmountForeign", "artSecondTaxCodeID", "artArPaymentHeaderID",
			"artShowAllInvoices", "artTaxAmount", "artTaxAmountForeign", "artTaxCodeID", "artVoidArPaymentHeaderId", "artVoidArPaymentSessionID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("artUniqueID|C", aRPaymentHeaderId);
		AddCustomFieldsToSelectList("ARPaymentHeaders");
		using (DataTable dataTable = GetAsDataTable("ARPaymentHeaders", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPARPaymentHeaderInformationDto);
			}
			eRPARPaymentHeaderInformationDto.artArGlAccountID = dataTable.Rows[0].Field<string>("artArGlAccountID");
			eRPARPaymentHeaderInformationDto.artArInvoiceContactID = dataTable.Rows[0].Field<string>("artArInvoiceContactID");
			eRPARPaymentHeaderInformationDto.artArInvoiceLocationID = dataTable.Rows[0].Field<string>("artArInvoiceLocationID");
			eRPARPaymentHeaderInformationDto.artArPaymentSessionID = dataTable.Rows[0].Field<int>("artArPaymentSessionID");
			eRPARPaymentHeaderInformationDto.artBankAccountName = dataTable.Rows[0].Field<string>("artBankAccountName");
			eRPARPaymentHeaderInformationDto.artBankAccountNumber = dataTable.Rows[0].Field<string>("artBankAccountNumber");
			eRPARPaymentHeaderInformationDto.artBankInitials = dataTable.Rows[0].Field<string>("artBankInitials");
			eRPARPaymentHeaderInformationDto.artBsbNumber = dataTable.Rows[0].Field<string>("artBsbNumber");
			eRPARPaymentHeaderInformationDto.artCashGlAccountID = dataTable.Rows[0].Field<string>("artCashGlAccountID");
			eRPARPaymentHeaderInformationDto.artCreatedBy = dataTable.Rows[0].Field<string>("artCreatedBy");
			eRPARPaymentHeaderInformationDto.artCreatedCreditArInvoiceID = dataTable.Rows[0].Field<string>("artCreatedCreditArInvoiceID");
			eRPARPaymentHeaderInformationDto.artCreatedDate = dataTable.Rows[0].Field<DateTime?>("artCreatedDate");
			eRPARPaymentHeaderInformationDto.artCreditArInvoiceID = dataTable.Rows[0].Field<string>("artCreditArInvoiceID");
			eRPARPaymentHeaderInformationDto.artCustomerOrganizationID = dataTable.Rows[0].Field<string>("artCustomerOrganizationID");
			eRPARPaymentHeaderInformationDto.artCustomerPaymentNumber = dataTable.Rows[0].Field<string>("artCustomerPaymentNumber");
			eRPARPaymentHeaderInformationDto.artDescription = dataTable.Rows[0].Field<string>("artDescription");
			eRPARPaymentHeaderInformationDto.artUniqueID = dataTable.Rows[0].Field<Guid>("artUniqueID");
			eRPARPaymentHeaderInformationDto.artExchangeAmount = dataTable.Rows[0].Field<decimal>("artExchangeAmount");
			eRPARPaymentHeaderInformationDto.artExchangeGlAccountID = dataTable.Rows[0].Field<string>("artExchangeGlAccountID");
			eRPARPaymentHeaderInformationDto.artGlAccountID = dataTable.Rows[0].Field<string>("artGlAccountID");
			eRPARPaymentHeaderInformationDto.artGlFiscalYearID = dataTable.Rows[0].Field<short>("artGlFiscalYearID");
			eRPARPaymentHeaderInformationDto.artGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("artGlFiscalYearPeriodID");
			eRPARPaymentHeaderInformationDto.artAvalaraTaxCalculated = dataTable.Rows[0].Field<bool>("artAvalaraTaxCalculated");
			eRPARPaymentHeaderInformationDto.artNet1PaymentProcessed = dataTable.Rows[0].Field<bool>("artNet1PaymentProcessed");
			eRPARPaymentHeaderInformationDto.artOpenPaymentLoad = dataTable.Rows[0].Field<bool>("artOpenPaymentLoad");
			eRPARPaymentHeaderInformationDto.artPostedToGl = dataTable.Rows[0].Field<bool>("artPostedToGl");
			eRPARPaymentHeaderInformationDto.artVoidedPayment = dataTable.Rows[0].Field<bool>("artVoidedPayment");
			eRPARPaymentHeaderInformationDto.artLongDescriptionRtf = dataTable.Rows[0].Field<string>("artLongDescriptionRtf");
			eRPARPaymentHeaderInformationDto.artLongDescriptionText = dataTable.Rows[0].Field<string>("artLongDescriptionText");
			eRPARPaymentHeaderInformationDto.artNonTaxReasonID = dataTable.Rows[0].Field<string>("artNonTaxReasonID");
			eRPARPaymentHeaderInformationDto.artPaymentMethod = dataTable.Rows[0].Field<byte>("artPaymentMethod");
			eRPARPaymentHeaderInformationDto.artReceiptAmount = dataTable.Rows[0].Field<decimal>("artReceiptAmount");
			eRPARPaymentHeaderInformationDto.artReceiptAmountForeign = dataTable.Rows[0].Field<decimal>("artReceiptAmountForeign");
			eRPARPaymentHeaderInformationDto.artReceiptDate = dataTable.Rows[0].Field<DateTime?>("artReceiptDate");
			eRPARPaymentHeaderInformationDto.artReceiptType = dataTable.Rows[0].Field<byte>("artReceiptType");
			eRPARPaymentHeaderInformationDto.artRowVersion = dataTable.Rows[0].Field<byte[]>("artRowVersion");
			eRPARPaymentHeaderInformationDto.artSecondTaxAmount = dataTable.Rows[0].Field<decimal>("artSecondTaxAmount");
			eRPARPaymentHeaderInformationDto.artSecondTaxAmountForeign = dataTable.Rows[0].Field<decimal>("artSecondTaxAmountForeign");
			eRPARPaymentHeaderInformationDto.artSecondTaxCodeID = dataTable.Rows[0].Field<string>("artSecondTaxCodeID");
			eRPARPaymentHeaderInformationDto.artArPaymentHeaderID = dataTable.Rows[0].Field<int>("artArPaymentHeaderID");
			eRPARPaymentHeaderInformationDto.artShowAllInvoices = dataTable.Rows[0].Field<bool>("artShowAllInvoices");
			eRPARPaymentHeaderInformationDto.artTaxAmount = dataTable.Rows[0].Field<decimal>("artTaxAmount");
			eRPARPaymentHeaderInformationDto.artTaxAmountForeign = dataTable.Rows[0].Field<decimal>("artTaxAmountForeign");
			eRPARPaymentHeaderInformationDto.artTaxCodeID = dataTable.Rows[0].Field<string>("artTaxCodeID");
			eRPARPaymentHeaderInformationDto.artVoidArPaymentHeaderId = dataTable.Rows[0].Field<int>("artVoidArPaymentHeaderId");
			eRPARPaymentHeaderInformationDto.artVoidArPaymentSessionID = dataTable.Rows[0].Field<int>("artVoidArPaymentSessionID");
			eRPARPaymentHeaderInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPARPaymentHeaderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPARPaymentHeaderInformationDto);
	}

	public Task<APIValidationInfoDto> SaveARPaymentHeader(ERPARPaymentHeaderDto aRPaymentHeader)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ARPaymentHeaders WHERE artUniqueID = " + M1Util.ConvertToLinq(aRPaymentHeader.artUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["artArPaymentSessionID"] = aRPaymentHeader.artArPaymentSessionID;
				dataRow["artArPaymentHeaderID"] = aRPaymentHeader.artArPaymentHeaderID;
				aRPaymentHeader.artUniqueID = ((aRPaymentHeader.artUniqueID == Guid.Empty) ? Guid.NewGuid() : aRPaymentHeader.artUniqueID);
				dataRow["artUniqueID"] = aRPaymentHeader.artUniqueID;
				dataRow["artCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["artCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ARPaymentHeader could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aRPaymentHeader.artRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ARPaymentHeader is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["artRowVersion"], aRPaymentHeader.artRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ARPaymentHeader has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ARPaymentHeader again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["artArGlAccountID"] = aRPaymentHeader.artArGlAccountID;
			dataRow["artArInvoiceContactID"] = aRPaymentHeader.artArInvoiceContactID;
			dataRow["artArInvoiceLocationID"] = aRPaymentHeader.artArInvoiceLocationID;
			dataRow["artBankAccountName"] = aRPaymentHeader.artBankAccountName;
			dataRow["artBankAccountNumber"] = aRPaymentHeader.artBankAccountNumber;
			dataRow["artBankInitials"] = aRPaymentHeader.artBankInitials;
			dataRow["artBsbNumber"] = aRPaymentHeader.artBsbNumber;
			dataRow["artCashGlAccountID"] = aRPaymentHeader.artCashGlAccountID;
			dataRow["artCreatedCreditArInvoiceID"] = aRPaymentHeader.artCreatedCreditArInvoiceID;
			dataRow["artCreditArInvoiceID"] = aRPaymentHeader.artCreditArInvoiceID;
			dataRow["artCustomerOrganizationID"] = aRPaymentHeader.artCustomerOrganizationID;
			dataRow["artCustomerPaymentNumber"] = aRPaymentHeader.artCustomerPaymentNumber;
			dataRow["artDescription"] = aRPaymentHeader.artDescription;
			dataRow["artExchangeAmount"] = aRPaymentHeader.artExchangeAmount;
			dataRow["artExchangeGlAccountID"] = aRPaymentHeader.artExchangeGlAccountID;
			dataRow["artGlAccountID"] = aRPaymentHeader.artGlAccountID;
			dataRow["artGlFiscalYearID"] = aRPaymentHeader.artGlFiscalYearID;
			dataRow["artGlFiscalYearPeriodID"] = aRPaymentHeader.artGlFiscalYearPeriodID;
			dataRow["artAvalaraTaxCalculated"] = aRPaymentHeader.artAvalaraTaxCalculated;
			dataRow["artNet1PaymentProcessed"] = aRPaymentHeader.artNet1PaymentProcessed;
			dataRow["artOpenPaymentLoad"] = aRPaymentHeader.artOpenPaymentLoad;
			dataRow["artPostedToGl"] = aRPaymentHeader.artPostedToGl;
			dataRow["artVoidedPayment"] = aRPaymentHeader.artVoidedPayment;
			dataRow["artLongDescriptionRtf"] = aRPaymentHeader.artLongDescriptionRtf ?? dataRow["artLongDescriptionRtf"];
			dataRow["artLongDescriptionText"] = aRPaymentHeader.artLongDescriptionText ?? dataRow["artLongDescriptionText"];
			dataRow["artNonTaxReasonID"] = aRPaymentHeader.artNonTaxReasonID;
			dataRow["artPaymentMethod"] = aRPaymentHeader.artPaymentMethod;
			dataRow["artReceiptAmount"] = aRPaymentHeader.artReceiptAmount;
			dataRow["artReceiptAmountForeign"] = aRPaymentHeader.artReceiptAmountForeign;
			DataRow dataRow2 = dataRow;
			DateTime? artReceiptDate = aRPaymentHeader.artReceiptDate;
			dataRow2["artReceiptDate"] = (artReceiptDate.HasValue ? ((object)artReceiptDate.GetValueOrDefault()) : dataRow["artReceiptDate"]);
			dataRow["artReceiptType"] = aRPaymentHeader.artReceiptType;
			dataRow["artSecondTaxAmount"] = aRPaymentHeader.artSecondTaxAmount;
			dataRow["artSecondTaxAmountForeign"] = aRPaymentHeader.artSecondTaxAmountForeign;
			dataRow["artSecondTaxCodeID"] = aRPaymentHeader.artSecondTaxCodeID;
			dataRow["artShowAllInvoices"] = aRPaymentHeader.artShowAllInvoices;
			dataRow["artTaxAmount"] = aRPaymentHeader.artTaxAmount;
			dataRow["artTaxAmountForeign"] = aRPaymentHeader.artTaxAmountForeign;
			dataRow["artTaxCodeID"] = aRPaymentHeader.artTaxCodeID;
			dataRow["artVoidArPaymentHeaderId"] = aRPaymentHeader.artVoidArPaymentHeaderId;
			dataRow["artVoidArPaymentSessionID"] = aRPaymentHeader.artVoidArPaymentSessionID;
			if (aRPaymentHeader.CustomFields != null && aRPaymentHeader.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aRPaymentHeader.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ARPaymentHeader [{aRPaymentHeader.artUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ARPaymentHeader [{aRPaymentHeader.artUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
