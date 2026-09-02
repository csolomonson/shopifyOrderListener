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

public class ERPAPPaymentLineRepository : APIBaseRepository, IERPAPPaymentLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPAPPaymentLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAPPaymentLineExist(Guid aPPaymentLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("apnUniqueID|C", aPPaymentLineId);
		base.selectList.Add("apnUniqueID");
		return Task.FromResult(GetAsObject("APPaymentLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAPPaymentLineInformationDto>> GetAllAPPaymentLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAPPaymentLineInformationDto> collection = new List<ERPAPPaymentLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[49]
		{
			"apnAdjustmentAmount", "apnAdjustmentAmountForeign", "apnAdjustmentGlAccountID", "apnApInvoiceID", "apnApPaymentHeaderID", "apnApPaymentSessionID", "apnArInvoiceID", "apnBankAccountID", "apnCreatedBy", "apnCreatedDate",
			"apnCurrencyRateID", "apnDescription", "apnDiscountAmount", "apnDiscountAmountForeign", "apnDiscountGlAccountID", "apnDiscountTaxAmount", "apnDiscountTaxAmountForeign", "apnDiscountTaxCodeID", "apnUniqueID", "apnExchangeAmount",
			"apnExchangeGlAccountID", "apnExchangeRate", "apnExpenseGlAccountID", "apnCompleted", "apnCustomRate", "apnOverpayment", "apnPostedToGl", "apnNonTaxReasonID", "apnOriginalInvBalanceForeign", "apnOriginalInvoiceBalance",
			"apnPaymentAmount", "apnPaymentAmountForeign", "apnRetentionPayAmtForeign", "apnRetentionPaymentAmount", "apnRowVersion", "apnSecondDiscountTaxAmount", "apnSecondDiscountTaxCodeID", "apnSecondDisTaxAmtForeign", "apnSecondTaxAmount", "apnSecondTaxAmountForeign",
			"apnSecondTaxCodeID", "apnApPaymentLineID", "apnTaxAmount", "apnTaxAmountForeign", "apnTaxCodeID", "apnTotalDiscountAmount", "apnTotalDiscountAmtForeign", "apnUnrealisedExchangeAmt", "apnUnrealisedExGlAccountID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("APPaymentLines");
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
		using (DataTable dataTable = GetAsDataTable("APPaymentLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAPPaymentLineInformationDto eRPAPPaymentLineInformationDto = new ERPAPPaymentLineInformationDto();
				eRPAPPaymentLineInformationDto.apnAdjustmentAmount = dataTable.Rows[i].Field<decimal>("apnAdjustmentAmount");
				eRPAPPaymentLineInformationDto.apnAdjustmentAmountForeign = dataTable.Rows[i].Field<decimal>("apnAdjustmentAmountForeign");
				eRPAPPaymentLineInformationDto.apnAdjustmentGlAccountID = dataTable.Rows[i].Field<string>("apnAdjustmentGlAccountID");
				eRPAPPaymentLineInformationDto.apnApInvoiceID = dataTable.Rows[i].Field<string>("apnApInvoiceID");
				eRPAPPaymentLineInformationDto.apnApPaymentHeaderID = dataTable.Rows[i].Field<int>("apnApPaymentHeaderID");
				eRPAPPaymentLineInformationDto.apnApPaymentSessionID = dataTable.Rows[i].Field<int>("apnApPaymentSessionID");
				eRPAPPaymentLineInformationDto.apnArInvoiceID = dataTable.Rows[i].Field<string>("apnArInvoiceID");
				eRPAPPaymentLineInformationDto.apnBankAccountID = dataTable.Rows[i].Field<string>("apnBankAccountID");
				eRPAPPaymentLineInformationDto.apnCreatedBy = dataTable.Rows[i].Field<string>("apnCreatedBy");
				eRPAPPaymentLineInformationDto.apnCreatedDate = dataTable.Rows[i].Field<DateTime?>("apnCreatedDate");
				eRPAPPaymentLineInformationDto.apnCurrencyRateID = dataTable.Rows[i].Field<string>("apnCurrencyRateID");
				eRPAPPaymentLineInformationDto.apnDescription = dataTable.Rows[i].Field<string>("apnDescription");
				eRPAPPaymentLineInformationDto.apnDiscountAmount = dataTable.Rows[i].Field<decimal>("apnDiscountAmount");
				eRPAPPaymentLineInformationDto.apnDiscountAmountForeign = dataTable.Rows[i].Field<decimal>("apnDiscountAmountForeign");
				eRPAPPaymentLineInformationDto.apnDiscountGlAccountID = dataTable.Rows[i].Field<string>("apnDiscountGlAccountID");
				eRPAPPaymentLineInformationDto.apnDiscountTaxAmount = dataTable.Rows[i].Field<decimal>("apnDiscountTaxAmount");
				eRPAPPaymentLineInformationDto.apnDiscountTaxAmountForeign = dataTable.Rows[i].Field<decimal>("apnDiscountTaxAmountForeign");
				eRPAPPaymentLineInformationDto.apnDiscountTaxCodeID = dataTable.Rows[i].Field<string>("apnDiscountTaxCodeID");
				eRPAPPaymentLineInformationDto.apnUniqueID = dataTable.Rows[i].Field<Guid>("apnUniqueID");
				eRPAPPaymentLineInformationDto.apnExchangeAmount = dataTable.Rows[i].Field<decimal>("apnExchangeAmount");
				eRPAPPaymentLineInformationDto.apnExchangeGlAccountID = dataTable.Rows[i].Field<string>("apnExchangeGlAccountID");
				eRPAPPaymentLineInformationDto.apnExchangeRate = dataTable.Rows[i].Field<decimal>("apnExchangeRate");
				eRPAPPaymentLineInformationDto.apnExpenseGlAccountID = dataTable.Rows[i].Field<string>("apnExpenseGlAccountID");
				eRPAPPaymentLineInformationDto.apnCompleted = dataTable.Rows[i].Field<bool>("apnCompleted");
				eRPAPPaymentLineInformationDto.apnCustomRate = dataTable.Rows[i].Field<bool>("apnCustomRate");
				eRPAPPaymentLineInformationDto.apnOverpayment = dataTable.Rows[i].Field<bool>("apnOverpayment");
				eRPAPPaymentLineInformationDto.apnPostedToGl = dataTable.Rows[i].Field<bool>("apnPostedToGl");
				eRPAPPaymentLineInformationDto.apnNonTaxReasonID = dataTable.Rows[i].Field<string>("apnNonTaxReasonID");
				eRPAPPaymentLineInformationDto.apnOriginalInvBalanceForeign = dataTable.Rows[i].Field<decimal>("apnOriginalInvBalanceForeign");
				eRPAPPaymentLineInformationDto.apnOriginalInvoiceBalance = dataTable.Rows[i].Field<decimal>("apnOriginalInvoiceBalance");
				eRPAPPaymentLineInformationDto.apnPaymentAmount = dataTable.Rows[i].Field<decimal>("apnPaymentAmount");
				eRPAPPaymentLineInformationDto.apnPaymentAmountForeign = dataTable.Rows[i].Field<decimal>("apnPaymentAmountForeign");
				eRPAPPaymentLineInformationDto.apnRetentionPayAmtForeign = dataTable.Rows[i].Field<decimal>("apnRetentionPayAmtForeign");
				eRPAPPaymentLineInformationDto.apnRetentionPaymentAmount = dataTable.Rows[i].Field<decimal>("apnRetentionPaymentAmount");
				eRPAPPaymentLineInformationDto.apnRowVersion = dataTable.Rows[i].Field<byte[]>("apnRowVersion");
				eRPAPPaymentLineInformationDto.apnSecondDiscountTaxAmount = dataTable.Rows[i].Field<decimal>("apnSecondDiscountTaxAmount");
				eRPAPPaymentLineInformationDto.apnSecondDiscountTaxCodeID = dataTable.Rows[i].Field<string>("apnSecondDiscountTaxCodeID");
				eRPAPPaymentLineInformationDto.apnSecondDisTaxAmtForeign = dataTable.Rows[i].Field<decimal>("apnSecondDisTaxAmtForeign");
				eRPAPPaymentLineInformationDto.apnSecondTaxAmount = dataTable.Rows[i].Field<decimal>("apnSecondTaxAmount");
				eRPAPPaymentLineInformationDto.apnSecondTaxAmountForeign = dataTable.Rows[i].Field<decimal>("apnSecondTaxAmountForeign");
				eRPAPPaymentLineInformationDto.apnSecondTaxCodeID = dataTable.Rows[i].Field<string>("apnSecondTaxCodeID");
				eRPAPPaymentLineInformationDto.apnApPaymentLineID = dataTable.Rows[i].Field<short>("apnApPaymentLineID");
				eRPAPPaymentLineInformationDto.apnTaxAmount = dataTable.Rows[i].Field<decimal>("apnTaxAmount");
				eRPAPPaymentLineInformationDto.apnTaxAmountForeign = dataTable.Rows[i].Field<decimal>("apnTaxAmountForeign");
				eRPAPPaymentLineInformationDto.apnTaxCodeID = dataTable.Rows[i].Field<string>("apnTaxCodeID");
				eRPAPPaymentLineInformationDto.apnTotalDiscountAmount = dataTable.Rows[i].Field<decimal>("apnTotalDiscountAmount");
				eRPAPPaymentLineInformationDto.apnTotalDiscountAmtForeign = dataTable.Rows[i].Field<decimal>("apnTotalDiscountAmtForeign");
				eRPAPPaymentLineInformationDto.apnUnrealisedExchangeAmt = dataTable.Rows[i].Field<decimal>("apnUnrealisedExchangeAmt");
				eRPAPPaymentLineInformationDto.apnUnrealisedExGlAccountID = dataTable.Rows[i].Field<string>("apnUnrealisedExGlAccountID");
				eRPAPPaymentLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAPPaymentLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAPPaymentLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAPPaymentLineInformationDto> GetAPPaymentLine(Guid aPPaymentLineId)
	{
		ERPAPPaymentLineInformationDto eRPAPPaymentLineInformationDto = new ERPAPPaymentLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[49]
		{
			"apnAdjustmentAmount", "apnAdjustmentAmountForeign", "apnAdjustmentGlAccountID", "apnApInvoiceID", "apnApPaymentHeaderID", "apnApPaymentSessionID", "apnArInvoiceID", "apnBankAccountID", "apnCreatedBy", "apnCreatedDate",
			"apnCurrencyRateID", "apnDescription", "apnDiscountAmount", "apnDiscountAmountForeign", "apnDiscountGlAccountID", "apnDiscountTaxAmount", "apnDiscountTaxAmountForeign", "apnDiscountTaxCodeID", "apnUniqueID", "apnExchangeAmount",
			"apnExchangeGlAccountID", "apnExchangeRate", "apnExpenseGlAccountID", "apnCompleted", "apnCustomRate", "apnOverpayment", "apnPostedToGl", "apnNonTaxReasonID", "apnOriginalInvBalanceForeign", "apnOriginalInvoiceBalance",
			"apnPaymentAmount", "apnPaymentAmountForeign", "apnRetentionPayAmtForeign", "apnRetentionPaymentAmount", "apnRowVersion", "apnSecondDiscountTaxAmount", "apnSecondDiscountTaxCodeID", "apnSecondDisTaxAmtForeign", "apnSecondTaxAmount", "apnSecondTaxAmountForeign",
			"apnSecondTaxCodeID", "apnApPaymentLineID", "apnTaxAmount", "apnTaxAmountForeign", "apnTaxCodeID", "apnTotalDiscountAmount", "apnTotalDiscountAmtForeign", "apnUnrealisedExchangeAmt", "apnUnrealisedExGlAccountID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("apnUniqueID|C", aPPaymentLineId);
		AddCustomFieldsToSelectList("APPaymentLines");
		using (DataTable dataTable = GetAsDataTable("APPaymentLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAPPaymentLineInformationDto);
			}
			eRPAPPaymentLineInformationDto.apnAdjustmentAmount = dataTable.Rows[0].Field<decimal>("apnAdjustmentAmount");
			eRPAPPaymentLineInformationDto.apnAdjustmentAmountForeign = dataTable.Rows[0].Field<decimal>("apnAdjustmentAmountForeign");
			eRPAPPaymentLineInformationDto.apnAdjustmentGlAccountID = dataTable.Rows[0].Field<string>("apnAdjustmentGlAccountID");
			eRPAPPaymentLineInformationDto.apnApInvoiceID = dataTable.Rows[0].Field<string>("apnApInvoiceID");
			eRPAPPaymentLineInformationDto.apnApPaymentHeaderID = dataTable.Rows[0].Field<int>("apnApPaymentHeaderID");
			eRPAPPaymentLineInformationDto.apnApPaymentSessionID = dataTable.Rows[0].Field<int>("apnApPaymentSessionID");
			eRPAPPaymentLineInformationDto.apnArInvoiceID = dataTable.Rows[0].Field<string>("apnArInvoiceID");
			eRPAPPaymentLineInformationDto.apnBankAccountID = dataTable.Rows[0].Field<string>("apnBankAccountID");
			eRPAPPaymentLineInformationDto.apnCreatedBy = dataTable.Rows[0].Field<string>("apnCreatedBy");
			eRPAPPaymentLineInformationDto.apnCreatedDate = dataTable.Rows[0].Field<DateTime?>("apnCreatedDate");
			eRPAPPaymentLineInformationDto.apnCurrencyRateID = dataTable.Rows[0].Field<string>("apnCurrencyRateID");
			eRPAPPaymentLineInformationDto.apnDescription = dataTable.Rows[0].Field<string>("apnDescription");
			eRPAPPaymentLineInformationDto.apnDiscountAmount = dataTable.Rows[0].Field<decimal>("apnDiscountAmount");
			eRPAPPaymentLineInformationDto.apnDiscountAmountForeign = dataTable.Rows[0].Field<decimal>("apnDiscountAmountForeign");
			eRPAPPaymentLineInformationDto.apnDiscountGlAccountID = dataTable.Rows[0].Field<string>("apnDiscountGlAccountID");
			eRPAPPaymentLineInformationDto.apnDiscountTaxAmount = dataTable.Rows[0].Field<decimal>("apnDiscountTaxAmount");
			eRPAPPaymentLineInformationDto.apnDiscountTaxAmountForeign = dataTable.Rows[0].Field<decimal>("apnDiscountTaxAmountForeign");
			eRPAPPaymentLineInformationDto.apnDiscountTaxCodeID = dataTable.Rows[0].Field<string>("apnDiscountTaxCodeID");
			eRPAPPaymentLineInformationDto.apnUniqueID = dataTable.Rows[0].Field<Guid>("apnUniqueID");
			eRPAPPaymentLineInformationDto.apnExchangeAmount = dataTable.Rows[0].Field<decimal>("apnExchangeAmount");
			eRPAPPaymentLineInformationDto.apnExchangeGlAccountID = dataTable.Rows[0].Field<string>("apnExchangeGlAccountID");
			eRPAPPaymentLineInformationDto.apnExchangeRate = dataTable.Rows[0].Field<decimal>("apnExchangeRate");
			eRPAPPaymentLineInformationDto.apnExpenseGlAccountID = dataTable.Rows[0].Field<string>("apnExpenseGlAccountID");
			eRPAPPaymentLineInformationDto.apnCompleted = dataTable.Rows[0].Field<bool>("apnCompleted");
			eRPAPPaymentLineInformationDto.apnCustomRate = dataTable.Rows[0].Field<bool>("apnCustomRate");
			eRPAPPaymentLineInformationDto.apnOverpayment = dataTable.Rows[0].Field<bool>("apnOverpayment");
			eRPAPPaymentLineInformationDto.apnPostedToGl = dataTable.Rows[0].Field<bool>("apnPostedToGl");
			eRPAPPaymentLineInformationDto.apnNonTaxReasonID = dataTable.Rows[0].Field<string>("apnNonTaxReasonID");
			eRPAPPaymentLineInformationDto.apnOriginalInvBalanceForeign = dataTable.Rows[0].Field<decimal>("apnOriginalInvBalanceForeign");
			eRPAPPaymentLineInformationDto.apnOriginalInvoiceBalance = dataTable.Rows[0].Field<decimal>("apnOriginalInvoiceBalance");
			eRPAPPaymentLineInformationDto.apnPaymentAmount = dataTable.Rows[0].Field<decimal>("apnPaymentAmount");
			eRPAPPaymentLineInformationDto.apnPaymentAmountForeign = dataTable.Rows[0].Field<decimal>("apnPaymentAmountForeign");
			eRPAPPaymentLineInformationDto.apnRetentionPayAmtForeign = dataTable.Rows[0].Field<decimal>("apnRetentionPayAmtForeign");
			eRPAPPaymentLineInformationDto.apnRetentionPaymentAmount = dataTable.Rows[0].Field<decimal>("apnRetentionPaymentAmount");
			eRPAPPaymentLineInformationDto.apnRowVersion = dataTable.Rows[0].Field<byte[]>("apnRowVersion");
			eRPAPPaymentLineInformationDto.apnSecondDiscountTaxAmount = dataTable.Rows[0].Field<decimal>("apnSecondDiscountTaxAmount");
			eRPAPPaymentLineInformationDto.apnSecondDiscountTaxCodeID = dataTable.Rows[0].Field<string>("apnSecondDiscountTaxCodeID");
			eRPAPPaymentLineInformationDto.apnSecondDisTaxAmtForeign = dataTable.Rows[0].Field<decimal>("apnSecondDisTaxAmtForeign");
			eRPAPPaymentLineInformationDto.apnSecondTaxAmount = dataTable.Rows[0].Field<decimal>("apnSecondTaxAmount");
			eRPAPPaymentLineInformationDto.apnSecondTaxAmountForeign = dataTable.Rows[0].Field<decimal>("apnSecondTaxAmountForeign");
			eRPAPPaymentLineInformationDto.apnSecondTaxCodeID = dataTable.Rows[0].Field<string>("apnSecondTaxCodeID");
			eRPAPPaymentLineInformationDto.apnApPaymentLineID = dataTable.Rows[0].Field<short>("apnApPaymentLineID");
			eRPAPPaymentLineInformationDto.apnTaxAmount = dataTable.Rows[0].Field<decimal>("apnTaxAmount");
			eRPAPPaymentLineInformationDto.apnTaxAmountForeign = dataTable.Rows[0].Field<decimal>("apnTaxAmountForeign");
			eRPAPPaymentLineInformationDto.apnTaxCodeID = dataTable.Rows[0].Field<string>("apnTaxCodeID");
			eRPAPPaymentLineInformationDto.apnTotalDiscountAmount = dataTable.Rows[0].Field<decimal>("apnTotalDiscountAmount");
			eRPAPPaymentLineInformationDto.apnTotalDiscountAmtForeign = dataTable.Rows[0].Field<decimal>("apnTotalDiscountAmtForeign");
			eRPAPPaymentLineInformationDto.apnUnrealisedExchangeAmt = dataTable.Rows[0].Field<decimal>("apnUnrealisedExchangeAmt");
			eRPAPPaymentLineInformationDto.apnUnrealisedExGlAccountID = dataTable.Rows[0].Field<string>("apnUnrealisedExGlAccountID");
			eRPAPPaymentLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAPPaymentLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAPPaymentLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAPPaymentLine(ERPAPPaymentLineDto aPPaymentLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM APPaymentLines WHERE apnUniqueID = " + M1Util.ConvertToLinq(aPPaymentLine.apnUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["apnApPaymentSessionID"] = aPPaymentLine.apnApPaymentSessionID;
				dataRow["apnApPaymentHeaderID"] = aPPaymentLine.apnApPaymentHeaderID;
				dataRow["apnApPaymentLineID"] = aPPaymentLine.apnApPaymentLineID;
				aPPaymentLine.apnUniqueID = ((aPPaymentLine.apnUniqueID == Guid.Empty) ? Guid.NewGuid() : aPPaymentLine.apnUniqueID);
				dataRow["apnUniqueID"] = aPPaymentLine.apnUniqueID;
				dataRow["apnCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["apnCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The APPaymentLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aPPaymentLine.apnRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the APPaymentLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["apnRowVersion"], aPPaymentLine.apnRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the APPaymentLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the APPaymentLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["apnAdjustmentAmount"] = aPPaymentLine.apnAdjustmentAmount;
			dataRow["apnAdjustmentAmountForeign"] = aPPaymentLine.apnAdjustmentAmountForeign;
			dataRow["apnAdjustmentGlAccountID"] = aPPaymentLine.apnAdjustmentGlAccountID;
			dataRow["apnApInvoiceID"] = aPPaymentLine.apnApInvoiceID;
			dataRow["apnArInvoiceID"] = aPPaymentLine.apnArInvoiceID;
			dataRow["apnBankAccountID"] = aPPaymentLine.apnBankAccountID;
			dataRow["apnCurrencyRateID"] = aPPaymentLine.apnCurrencyRateID;
			dataRow["apnDescription"] = aPPaymentLine.apnDescription;
			dataRow["apnDiscountAmount"] = aPPaymentLine.apnDiscountAmount;
			dataRow["apnDiscountAmountForeign"] = aPPaymentLine.apnDiscountAmountForeign;
			dataRow["apnDiscountGlAccountID"] = aPPaymentLine.apnDiscountGlAccountID;
			dataRow["apnDiscountTaxAmount"] = aPPaymentLine.apnDiscountTaxAmount;
			dataRow["apnDiscountTaxAmountForeign"] = aPPaymentLine.apnDiscountTaxAmountForeign;
			dataRow["apnDiscountTaxCodeID"] = aPPaymentLine.apnDiscountTaxCodeID;
			dataRow["apnExchangeAmount"] = aPPaymentLine.apnExchangeAmount;
			dataRow["apnExchangeGlAccountID"] = aPPaymentLine.apnExchangeGlAccountID;
			dataRow["apnExchangeRate"] = aPPaymentLine.apnExchangeRate;
			dataRow["apnExpenseGlAccountID"] = aPPaymentLine.apnExpenseGlAccountID;
			dataRow["apnCompleted"] = aPPaymentLine.apnCompleted;
			dataRow["apnCustomRate"] = aPPaymentLine.apnCustomRate;
			dataRow["apnOverpayment"] = aPPaymentLine.apnOverpayment;
			dataRow["apnPostedToGl"] = aPPaymentLine.apnPostedToGl;
			dataRow["apnNonTaxReasonID"] = aPPaymentLine.apnNonTaxReasonID;
			dataRow["apnOriginalInvBalanceForeign"] = aPPaymentLine.apnOriginalInvBalanceForeign;
			dataRow["apnOriginalInvoiceBalance"] = aPPaymentLine.apnOriginalInvoiceBalance;
			dataRow["apnPaymentAmount"] = aPPaymentLine.apnPaymentAmount;
			dataRow["apnPaymentAmountForeign"] = aPPaymentLine.apnPaymentAmountForeign;
			dataRow["apnRetentionPayAmtForeign"] = aPPaymentLine.apnRetentionPayAmtForeign;
			dataRow["apnRetentionPaymentAmount"] = aPPaymentLine.apnRetentionPaymentAmount;
			dataRow["apnSecondDiscountTaxAmount"] = aPPaymentLine.apnSecondDiscountTaxAmount;
			dataRow["apnSecondDiscountTaxCodeID"] = aPPaymentLine.apnSecondDiscountTaxCodeID;
			dataRow["apnSecondDisTaxAmtForeign"] = aPPaymentLine.apnSecondDisTaxAmtForeign;
			dataRow["apnSecondTaxAmount"] = aPPaymentLine.apnSecondTaxAmount;
			dataRow["apnSecondTaxAmountForeign"] = aPPaymentLine.apnSecondTaxAmountForeign;
			dataRow["apnSecondTaxCodeID"] = aPPaymentLine.apnSecondTaxCodeID;
			dataRow["apnTaxAmount"] = aPPaymentLine.apnTaxAmount;
			dataRow["apnTaxAmountForeign"] = aPPaymentLine.apnTaxAmountForeign;
			dataRow["apnTaxCodeID"] = aPPaymentLine.apnTaxCodeID;
			dataRow["apnTotalDiscountAmount"] = aPPaymentLine.apnTotalDiscountAmount;
			dataRow["apnTotalDiscountAmtForeign"] = aPPaymentLine.apnTotalDiscountAmtForeign;
			dataRow["apnUnrealisedExchangeAmt"] = aPPaymentLine.apnUnrealisedExchangeAmt;
			dataRow["apnUnrealisedExGlAccountID"] = aPPaymentLine.apnUnrealisedExGlAccountID;
			if (aPPaymentLine.CustomFields != null && aPPaymentLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aPPaymentLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the APPaymentLine [{aPPaymentLine.apnUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the APPaymentLine [{aPPaymentLine.apnUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
