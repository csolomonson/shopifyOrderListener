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

public class ERPAPInvoiceRepository : APIBaseRepository, IERPAPInvoiceRepository, IAPIBaseRepository, IDisposable
{
	public ERPAPInvoiceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAPInvoiceExist(Guid aPInvoiceId)
	{
		InitializeParameterLists();
		base.filterList.Add("appUniqueID|C", aPInvoiceId);
		base.selectList.Add("appUniqueID");
		return Task.FromResult(GetAsObject("APInvoices", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAPInvoiceInformationDto>> GetAllAPInvoices(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAPInvoiceInformationDto> collection = new List<ERPAPInvoiceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[63]
		{
			"appApGlAccountID", "appApInvoiceContactID", "appApInvoiceLocationID", "appApInvoiceID", "appCreatedBy", "appCreatedDate", "appCreditApInvoiceID", "appCreditDate", "appCreditReasonID", "appCurrencyRateID",
			"appDiscountAmountBase", "appDiscountAmountForeign", "appDiscountDueDate", "appDueDate", "appUniqueID", "appExchangeRate", "appFreightAmountBase", "appFreightAmountForeign", "appFreightGlAccountID", "appFreightTaxAmountBase",
			"appFreightTaxAmountForeign", "appFreightTaxCodeID", "appGlFiscalYearID", "appGlFiscalYearPeriodID", "appInvoiceBalanceBase", "appInvoiceBalanceForeign", "appInvoiceCommentsRTF", "appInvoiceCommentsText", "appInvoiceDate", "appInvoiceDescription",
			"appInvoiceSubtotalBase", "appInvoiceSubtotalForeign", "appInvoiceTaxAmountBase", "appInvoiceTaxAmountForeign", "appInvoiceTotalBase", "appInvoiceTotalForeign", "appInvoiceType", "appCustomRate", "appOnHold", "appOpenInvoiceLoad",
			"appOverpayment", "appPaidComplete", "appPostedToGl", "appTaxReportable", "appOriginalExchangeRate", "appOverPaymentHeaderID", "appOverPaymentSessionID", "appPaidDate", "appPaymentTermID", "appPlantDepartmentID",
			"appPlantID", "appPostedDate", "appProjectID", "appRetentionBalanceBase", "appRetentionBalanceForeign", "appRetentionTotalBase", "appRetentionTotalForeign", "appRowVersion", "appSecondFreightTaxAmtBase", "appSecondFreightTaxAmtForeign",
			"appSecondFreightTaxCodeID", "appSupplierInvoiceNumber", "appSupplierOrganizationID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("APInvoices");
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
		using (DataTable dataTable = GetAsDataTable("APInvoices", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAPInvoiceInformationDto eRPAPInvoiceInformationDto = new ERPAPInvoiceInformationDto();
				eRPAPInvoiceInformationDto.appApGlAccountID = dataTable.Rows[i].Field<string>("appApGlAccountID");
				eRPAPInvoiceInformationDto.appApInvoiceContactID = dataTable.Rows[i].Field<string>("appApInvoiceContactID");
				eRPAPInvoiceInformationDto.appApInvoiceLocationID = dataTable.Rows[i].Field<string>("appApInvoiceLocationID");
				eRPAPInvoiceInformationDto.appApInvoiceID = dataTable.Rows[i].Field<string>("appApInvoiceID");
				eRPAPInvoiceInformationDto.appCreatedBy = dataTable.Rows[i].Field<string>("appCreatedBy");
				eRPAPInvoiceInformationDto.appCreatedDate = dataTable.Rows[i].Field<DateTime?>("appCreatedDate");
				eRPAPInvoiceInformationDto.appCreditApInvoiceID = dataTable.Rows[i].Field<string>("appCreditApInvoiceID");
				eRPAPInvoiceInformationDto.appCreditDate = dataTable.Rows[i].Field<DateTime?>("appCreditDate");
				eRPAPInvoiceInformationDto.appCreditReasonID = dataTable.Rows[i].Field<string>("appCreditReasonID");
				eRPAPInvoiceInformationDto.appCurrencyRateID = dataTable.Rows[i].Field<string>("appCurrencyRateID");
				eRPAPInvoiceInformationDto.appDiscountAmountBase = dataTable.Rows[i].Field<decimal>("appDiscountAmountBase");
				eRPAPInvoiceInformationDto.appDiscountAmountForeign = dataTable.Rows[i].Field<decimal>("appDiscountAmountForeign");
				eRPAPInvoiceInformationDto.appDiscountDueDate = dataTable.Rows[i].Field<DateTime?>("appDiscountDueDate");
				eRPAPInvoiceInformationDto.appDueDate = dataTable.Rows[i].Field<DateTime?>("appDueDate");
				eRPAPInvoiceInformationDto.appUniqueID = dataTable.Rows[i].Field<Guid>("appUniqueID");
				eRPAPInvoiceInformationDto.appExchangeRate = dataTable.Rows[i].Field<decimal>("appExchangeRate");
				eRPAPInvoiceInformationDto.appFreightAmountBase = dataTable.Rows[i].Field<decimal>("appFreightAmountBase");
				eRPAPInvoiceInformationDto.appFreightAmountForeign = dataTable.Rows[i].Field<decimal>("appFreightAmountForeign");
				eRPAPInvoiceInformationDto.appFreightGlAccountID = dataTable.Rows[i].Field<string>("appFreightGlAccountID");
				eRPAPInvoiceInformationDto.appFreightTaxAmountBase = dataTable.Rows[i].Field<decimal>("appFreightTaxAmountBase");
				eRPAPInvoiceInformationDto.appFreightTaxAmountForeign = dataTable.Rows[i].Field<decimal>("appFreightTaxAmountForeign");
				eRPAPInvoiceInformationDto.appFreightTaxCodeID = dataTable.Rows[i].Field<string>("appFreightTaxCodeID");
				eRPAPInvoiceInformationDto.appGlFiscalYearID = dataTable.Rows[i].Field<short>("appGlFiscalYearID");
				eRPAPInvoiceInformationDto.appGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("appGlFiscalYearPeriodID");
				eRPAPInvoiceInformationDto.appInvoiceBalanceBase = dataTable.Rows[i].Field<decimal>("appInvoiceBalanceBase");
				eRPAPInvoiceInformationDto.appInvoiceBalanceForeign = dataTable.Rows[i].Field<decimal>("appInvoiceBalanceForeign");
				eRPAPInvoiceInformationDto.appInvoiceCommentsRTF = dataTable.Rows[i].Field<string>("appInvoiceCommentsRTF");
				eRPAPInvoiceInformationDto.appInvoiceCommentsText = dataTable.Rows[i].Field<string>("appInvoiceCommentsText");
				eRPAPInvoiceInformationDto.appInvoiceDate = dataTable.Rows[i].Field<DateTime?>("appInvoiceDate");
				eRPAPInvoiceInformationDto.appInvoiceDescription = dataTable.Rows[i].Field<string>("appInvoiceDescription");
				eRPAPInvoiceInformationDto.appInvoiceSubtotalBase = dataTable.Rows[i].Field<decimal>("appInvoiceSubtotalBase");
				eRPAPInvoiceInformationDto.appInvoiceSubtotalForeign = dataTable.Rows[i].Field<decimal>("appInvoiceSubtotalForeign");
				eRPAPInvoiceInformationDto.appInvoiceTaxAmountBase = dataTable.Rows[i].Field<decimal>("appInvoiceTaxAmountBase");
				eRPAPInvoiceInformationDto.appInvoiceTaxAmountForeign = dataTable.Rows[i].Field<decimal>("appInvoiceTaxAmountForeign");
				eRPAPInvoiceInformationDto.appInvoiceTotalBase = dataTable.Rows[i].Field<decimal>("appInvoiceTotalBase");
				eRPAPInvoiceInformationDto.appInvoiceTotalForeign = dataTable.Rows[i].Field<decimal>("appInvoiceTotalForeign");
				eRPAPInvoiceInformationDto.appInvoiceType = dataTable.Rows[i].Field<byte>("appInvoiceType");
				eRPAPInvoiceInformationDto.appCustomRate = dataTable.Rows[i].Field<bool>("appCustomRate");
				eRPAPInvoiceInformationDto.appOnHold = dataTable.Rows[i].Field<bool>("appOnHold");
				eRPAPInvoiceInformationDto.appOpenInvoiceLoad = dataTable.Rows[i].Field<bool>("appOpenInvoiceLoad");
				eRPAPInvoiceInformationDto.appOverpayment = dataTable.Rows[i].Field<bool>("appOverpayment");
				eRPAPInvoiceInformationDto.appPaidComplete = dataTable.Rows[i].Field<bool>("appPaidComplete");
				eRPAPInvoiceInformationDto.appPostedToGl = dataTable.Rows[i].Field<bool>("appPostedToGl");
				eRPAPInvoiceInformationDto.appTaxReportable = dataTable.Rows[i].Field<bool>("appTaxReportable");
				eRPAPInvoiceInformationDto.appOriginalExchangeRate = dataTable.Rows[i].Field<decimal>("appOriginalExchangeRate");
				eRPAPInvoiceInformationDto.appOverPaymentHeaderID = dataTable.Rows[i].Field<int>("appOverPaymentHeaderID");
				eRPAPInvoiceInformationDto.appOverPaymentSessionID = dataTable.Rows[i].Field<int>("appOverPaymentSessionID");
				eRPAPInvoiceInformationDto.appPaidDate = dataTable.Rows[i].Field<DateTime?>("appPaidDate");
				eRPAPInvoiceInformationDto.appPaymentTermID = dataTable.Rows[i].Field<string>("appPaymentTermID");
				eRPAPInvoiceInformationDto.appPlantDepartmentID = dataTable.Rows[i].Field<string>("appPlantDepartmentID");
				eRPAPInvoiceInformationDto.appPlantID = dataTable.Rows[i].Field<string>("appPlantID");
				eRPAPInvoiceInformationDto.appPostedDate = dataTable.Rows[i].Field<DateTime?>("appPostedDate");
				eRPAPInvoiceInformationDto.appProjectID = dataTable.Rows[i].Field<string>("appProjectID");
				eRPAPInvoiceInformationDto.appRetentionBalanceBase = dataTable.Rows[i].Field<decimal>("appRetentionBalanceBase");
				eRPAPInvoiceInformationDto.appRetentionBalanceForeign = dataTable.Rows[i].Field<decimal>("appRetentionBalanceForeign");
				eRPAPInvoiceInformationDto.appRetentionTotalBase = dataTable.Rows[i].Field<decimal>("appRetentionTotalBase");
				eRPAPInvoiceInformationDto.appRetentionTotalForeign = dataTable.Rows[i].Field<decimal>("appRetentionTotalForeign");
				eRPAPInvoiceInformationDto.appRowVersion = dataTable.Rows[i].Field<byte[]>("appRowVersion");
				eRPAPInvoiceInformationDto.appSecondFreightTaxAmtBase = dataTable.Rows[i].Field<decimal>("appSecondFreightTaxAmtBase");
				eRPAPInvoiceInformationDto.appSecondFreightTaxAmtForeign = dataTable.Rows[i].Field<decimal>("appSecondFreightTaxAmtForeign");
				eRPAPInvoiceInformationDto.appSecondFreightTaxCodeID = dataTable.Rows[i].Field<string>("appSecondFreightTaxCodeID");
				eRPAPInvoiceInformationDto.appSupplierInvoiceNumber = dataTable.Rows[i].Field<string>("appSupplierInvoiceNumber");
				eRPAPInvoiceInformationDto.appSupplierOrganizationID = dataTable.Rows[i].Field<string>("appSupplierOrganizationID");
				eRPAPInvoiceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAPInvoiceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAPInvoiceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAPInvoiceInformationDto> GetAPInvoice(Guid aPInvoiceId)
	{
		ERPAPInvoiceInformationDto eRPAPInvoiceInformationDto = new ERPAPInvoiceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[63]
		{
			"appApGlAccountID", "appApInvoiceContactID", "appApInvoiceLocationID", "appApInvoiceID", "appCreatedBy", "appCreatedDate", "appCreditApInvoiceID", "appCreditDate", "appCreditReasonID", "appCurrencyRateID",
			"appDiscountAmountBase", "appDiscountAmountForeign", "appDiscountDueDate", "appDueDate", "appUniqueID", "appExchangeRate", "appFreightAmountBase", "appFreightAmountForeign", "appFreightGlAccountID", "appFreightTaxAmountBase",
			"appFreightTaxAmountForeign", "appFreightTaxCodeID", "appGlFiscalYearID", "appGlFiscalYearPeriodID", "appInvoiceBalanceBase", "appInvoiceBalanceForeign", "appInvoiceCommentsRTF", "appInvoiceCommentsText", "appInvoiceDate", "appInvoiceDescription",
			"appInvoiceSubtotalBase", "appInvoiceSubtotalForeign", "appInvoiceTaxAmountBase", "appInvoiceTaxAmountForeign", "appInvoiceTotalBase", "appInvoiceTotalForeign", "appInvoiceType", "appCustomRate", "appOnHold", "appOpenInvoiceLoad",
			"appOverpayment", "appPaidComplete", "appPostedToGl", "appTaxReportable", "appOriginalExchangeRate", "appOverPaymentHeaderID", "appOverPaymentSessionID", "appPaidDate", "appPaymentTermID", "appPlantDepartmentID",
			"appPlantID", "appPostedDate", "appProjectID", "appRetentionBalanceBase", "appRetentionBalanceForeign", "appRetentionTotalBase", "appRetentionTotalForeign", "appRowVersion", "appSecondFreightTaxAmtBase", "appSecondFreightTaxAmtForeign",
			"appSecondFreightTaxCodeID", "appSupplierInvoiceNumber", "appSupplierOrganizationID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("appUniqueID|C", aPInvoiceId);
		AddCustomFieldsToSelectList("APInvoices");
		using (DataTable dataTable = GetAsDataTable("APInvoices", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAPInvoiceInformationDto);
			}
			eRPAPInvoiceInformationDto.appApGlAccountID = dataTable.Rows[0].Field<string>("appApGlAccountID");
			eRPAPInvoiceInformationDto.appApInvoiceContactID = dataTable.Rows[0].Field<string>("appApInvoiceContactID");
			eRPAPInvoiceInformationDto.appApInvoiceLocationID = dataTable.Rows[0].Field<string>("appApInvoiceLocationID");
			eRPAPInvoiceInformationDto.appApInvoiceID = dataTable.Rows[0].Field<string>("appApInvoiceID");
			eRPAPInvoiceInformationDto.appCreatedBy = dataTable.Rows[0].Field<string>("appCreatedBy");
			eRPAPInvoiceInformationDto.appCreatedDate = dataTable.Rows[0].Field<DateTime?>("appCreatedDate");
			eRPAPInvoiceInformationDto.appCreditApInvoiceID = dataTable.Rows[0].Field<string>("appCreditApInvoiceID");
			eRPAPInvoiceInformationDto.appCreditDate = dataTable.Rows[0].Field<DateTime?>("appCreditDate");
			eRPAPInvoiceInformationDto.appCreditReasonID = dataTable.Rows[0].Field<string>("appCreditReasonID");
			eRPAPInvoiceInformationDto.appCurrencyRateID = dataTable.Rows[0].Field<string>("appCurrencyRateID");
			eRPAPInvoiceInformationDto.appDiscountAmountBase = dataTable.Rows[0].Field<decimal>("appDiscountAmountBase");
			eRPAPInvoiceInformationDto.appDiscountAmountForeign = dataTable.Rows[0].Field<decimal>("appDiscountAmountForeign");
			eRPAPInvoiceInformationDto.appDiscountDueDate = dataTable.Rows[0].Field<DateTime?>("appDiscountDueDate");
			eRPAPInvoiceInformationDto.appDueDate = dataTable.Rows[0].Field<DateTime?>("appDueDate");
			eRPAPInvoiceInformationDto.appUniqueID = dataTable.Rows[0].Field<Guid>("appUniqueID");
			eRPAPInvoiceInformationDto.appExchangeRate = dataTable.Rows[0].Field<decimal>("appExchangeRate");
			eRPAPInvoiceInformationDto.appFreightAmountBase = dataTable.Rows[0].Field<decimal>("appFreightAmountBase");
			eRPAPInvoiceInformationDto.appFreightAmountForeign = dataTable.Rows[0].Field<decimal>("appFreightAmountForeign");
			eRPAPInvoiceInformationDto.appFreightGlAccountID = dataTable.Rows[0].Field<string>("appFreightGlAccountID");
			eRPAPInvoiceInformationDto.appFreightTaxAmountBase = dataTable.Rows[0].Field<decimal>("appFreightTaxAmountBase");
			eRPAPInvoiceInformationDto.appFreightTaxAmountForeign = dataTable.Rows[0].Field<decimal>("appFreightTaxAmountForeign");
			eRPAPInvoiceInformationDto.appFreightTaxCodeID = dataTable.Rows[0].Field<string>("appFreightTaxCodeID");
			eRPAPInvoiceInformationDto.appGlFiscalYearID = dataTable.Rows[0].Field<short>("appGlFiscalYearID");
			eRPAPInvoiceInformationDto.appGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("appGlFiscalYearPeriodID");
			eRPAPInvoiceInformationDto.appInvoiceBalanceBase = dataTable.Rows[0].Field<decimal>("appInvoiceBalanceBase");
			eRPAPInvoiceInformationDto.appInvoiceBalanceForeign = dataTable.Rows[0].Field<decimal>("appInvoiceBalanceForeign");
			eRPAPInvoiceInformationDto.appInvoiceCommentsRTF = dataTable.Rows[0].Field<string>("appInvoiceCommentsRTF");
			eRPAPInvoiceInformationDto.appInvoiceCommentsText = dataTable.Rows[0].Field<string>("appInvoiceCommentsText");
			eRPAPInvoiceInformationDto.appInvoiceDate = dataTable.Rows[0].Field<DateTime?>("appInvoiceDate");
			eRPAPInvoiceInformationDto.appInvoiceDescription = dataTable.Rows[0].Field<string>("appInvoiceDescription");
			eRPAPInvoiceInformationDto.appInvoiceSubtotalBase = dataTable.Rows[0].Field<decimal>("appInvoiceSubtotalBase");
			eRPAPInvoiceInformationDto.appInvoiceSubtotalForeign = dataTable.Rows[0].Field<decimal>("appInvoiceSubtotalForeign");
			eRPAPInvoiceInformationDto.appInvoiceTaxAmountBase = dataTable.Rows[0].Field<decimal>("appInvoiceTaxAmountBase");
			eRPAPInvoiceInformationDto.appInvoiceTaxAmountForeign = dataTable.Rows[0].Field<decimal>("appInvoiceTaxAmountForeign");
			eRPAPInvoiceInformationDto.appInvoiceTotalBase = dataTable.Rows[0].Field<decimal>("appInvoiceTotalBase");
			eRPAPInvoiceInformationDto.appInvoiceTotalForeign = dataTable.Rows[0].Field<decimal>("appInvoiceTotalForeign");
			eRPAPInvoiceInformationDto.appInvoiceType = dataTable.Rows[0].Field<byte>("appInvoiceType");
			eRPAPInvoiceInformationDto.appCustomRate = dataTable.Rows[0].Field<bool>("appCustomRate");
			eRPAPInvoiceInformationDto.appOnHold = dataTable.Rows[0].Field<bool>("appOnHold");
			eRPAPInvoiceInformationDto.appOpenInvoiceLoad = dataTable.Rows[0].Field<bool>("appOpenInvoiceLoad");
			eRPAPInvoiceInformationDto.appOverpayment = dataTable.Rows[0].Field<bool>("appOverpayment");
			eRPAPInvoiceInformationDto.appPaidComplete = dataTable.Rows[0].Field<bool>("appPaidComplete");
			eRPAPInvoiceInformationDto.appPostedToGl = dataTable.Rows[0].Field<bool>("appPostedToGl");
			eRPAPInvoiceInformationDto.appTaxReportable = dataTable.Rows[0].Field<bool>("appTaxReportable");
			eRPAPInvoiceInformationDto.appOriginalExchangeRate = dataTable.Rows[0].Field<decimal>("appOriginalExchangeRate");
			eRPAPInvoiceInformationDto.appOverPaymentHeaderID = dataTable.Rows[0].Field<int>("appOverPaymentHeaderID");
			eRPAPInvoiceInformationDto.appOverPaymentSessionID = dataTable.Rows[0].Field<int>("appOverPaymentSessionID");
			eRPAPInvoiceInformationDto.appPaidDate = dataTable.Rows[0].Field<DateTime?>("appPaidDate");
			eRPAPInvoiceInformationDto.appPaymentTermID = dataTable.Rows[0].Field<string>("appPaymentTermID");
			eRPAPInvoiceInformationDto.appPlantDepartmentID = dataTable.Rows[0].Field<string>("appPlantDepartmentID");
			eRPAPInvoiceInformationDto.appPlantID = dataTable.Rows[0].Field<string>("appPlantID");
			eRPAPInvoiceInformationDto.appPostedDate = dataTable.Rows[0].Field<DateTime?>("appPostedDate");
			eRPAPInvoiceInformationDto.appProjectID = dataTable.Rows[0].Field<string>("appProjectID");
			eRPAPInvoiceInformationDto.appRetentionBalanceBase = dataTable.Rows[0].Field<decimal>("appRetentionBalanceBase");
			eRPAPInvoiceInformationDto.appRetentionBalanceForeign = dataTable.Rows[0].Field<decimal>("appRetentionBalanceForeign");
			eRPAPInvoiceInformationDto.appRetentionTotalBase = dataTable.Rows[0].Field<decimal>("appRetentionTotalBase");
			eRPAPInvoiceInformationDto.appRetentionTotalForeign = dataTable.Rows[0].Field<decimal>("appRetentionTotalForeign");
			eRPAPInvoiceInformationDto.appRowVersion = dataTable.Rows[0].Field<byte[]>("appRowVersion");
			eRPAPInvoiceInformationDto.appSecondFreightTaxAmtBase = dataTable.Rows[0].Field<decimal>("appSecondFreightTaxAmtBase");
			eRPAPInvoiceInformationDto.appSecondFreightTaxAmtForeign = dataTable.Rows[0].Field<decimal>("appSecondFreightTaxAmtForeign");
			eRPAPInvoiceInformationDto.appSecondFreightTaxCodeID = dataTable.Rows[0].Field<string>("appSecondFreightTaxCodeID");
			eRPAPInvoiceInformationDto.appSupplierInvoiceNumber = dataTable.Rows[0].Field<string>("appSupplierInvoiceNumber");
			eRPAPInvoiceInformationDto.appSupplierOrganizationID = dataTable.Rows[0].Field<string>("appSupplierOrganizationID");
			eRPAPInvoiceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAPInvoiceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAPInvoiceInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAPInvoice(ERPAPInvoiceDto aPInvoice)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM APInvoices WHERE appUniqueID = " + M1Util.ConvertToLinq(aPInvoice.appUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["appApInvoiceID"] = aPInvoice.appApInvoiceID.ToUpper();
				aPInvoice.appUniqueID = ((aPInvoice.appUniqueID == Guid.Empty) ? Guid.NewGuid() : aPInvoice.appUniqueID);
				dataRow["appUniqueID"] = aPInvoice.appUniqueID;
				dataRow["appCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["appCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The APInvoice could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aPInvoice.appRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the APInvoice is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["appRowVersion"], aPInvoice.appRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the APInvoice has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the APInvoice again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["appApGlAccountID"] = aPInvoice.appApGlAccountID;
			dataRow["appApInvoiceContactID"] = aPInvoice.appApInvoiceContactID;
			dataRow["appApInvoiceLocationID"] = aPInvoice.appApInvoiceLocationID;
			dataRow["appCreditApInvoiceID"] = aPInvoice.appCreditApInvoiceID;
			DataRow dataRow2 = dataRow;
			DateTime? appCreditDate = aPInvoice.appCreditDate;
			dataRow2["appCreditDate"] = (appCreditDate.HasValue ? ((object)appCreditDate.GetValueOrDefault()) : dataRow["appCreditDate"]);
			dataRow["appCreditReasonID"] = aPInvoice.appCreditReasonID;
			dataRow["appCurrencyRateID"] = aPInvoice.appCurrencyRateID;
			dataRow["appDiscountAmountBase"] = aPInvoice.appDiscountAmountBase;
			dataRow["appDiscountAmountForeign"] = aPInvoice.appDiscountAmountForeign;
			DataRow dataRow3 = dataRow;
			appCreditDate = aPInvoice.appDiscountDueDate;
			dataRow3["appDiscountDueDate"] = (appCreditDate.HasValue ? ((object)appCreditDate.GetValueOrDefault()) : dataRow["appDiscountDueDate"]);
			DataRow dataRow4 = dataRow;
			appCreditDate = aPInvoice.appDueDate;
			dataRow4["appDueDate"] = (appCreditDate.HasValue ? ((object)appCreditDate.GetValueOrDefault()) : dataRow["appDueDate"]);
			dataRow["appExchangeRate"] = aPInvoice.appExchangeRate;
			dataRow["appFreightAmountBase"] = aPInvoice.appFreightAmountBase;
			dataRow["appFreightAmountForeign"] = aPInvoice.appFreightAmountForeign;
			dataRow["appFreightGlAccountID"] = aPInvoice.appFreightGlAccountID;
			dataRow["appFreightTaxAmountBase"] = aPInvoice.appFreightTaxAmountBase;
			dataRow["appFreightTaxAmountForeign"] = aPInvoice.appFreightTaxAmountForeign;
			dataRow["appFreightTaxCodeID"] = aPInvoice.appFreightTaxCodeID;
			dataRow["appGlFiscalYearID"] = aPInvoice.appGlFiscalYearID;
			dataRow["appGlFiscalYearPeriodID"] = aPInvoice.appGlFiscalYearPeriodID;
			dataRow["appInvoiceBalanceBase"] = aPInvoice.appInvoiceBalanceBase;
			dataRow["appInvoiceBalanceForeign"] = aPInvoice.appInvoiceBalanceForeign;
			dataRow["appInvoiceCommentsRTF"] = aPInvoice.appInvoiceCommentsRTF ?? dataRow["appInvoiceCommentsRTF"];
			dataRow["appInvoiceCommentsText"] = aPInvoice.appInvoiceCommentsText ?? dataRow["appInvoiceCommentsText"];
			DataRow dataRow5 = dataRow;
			appCreditDate = aPInvoice.appInvoiceDate;
			dataRow5["appInvoiceDate"] = (appCreditDate.HasValue ? ((object)appCreditDate.GetValueOrDefault()) : dataRow["appInvoiceDate"]);
			dataRow["appInvoiceDescription"] = aPInvoice.appInvoiceDescription;
			dataRow["appInvoiceSubtotalBase"] = aPInvoice.appInvoiceSubtotalBase;
			dataRow["appInvoiceSubtotalForeign"] = aPInvoice.appInvoiceSubtotalForeign;
			dataRow["appInvoiceTaxAmountBase"] = aPInvoice.appInvoiceTaxAmountBase;
			dataRow["appInvoiceTaxAmountForeign"] = aPInvoice.appInvoiceTaxAmountForeign;
			dataRow["appInvoiceTotalBase"] = aPInvoice.appInvoiceTotalBase;
			dataRow["appInvoiceTotalForeign"] = aPInvoice.appInvoiceTotalForeign;
			dataRow["appInvoiceType"] = aPInvoice.appInvoiceType;
			dataRow["appCustomRate"] = aPInvoice.appCustomRate;
			dataRow["appOnHold"] = aPInvoice.appOnHold;
			dataRow["appOpenInvoiceLoad"] = aPInvoice.appOpenInvoiceLoad;
			dataRow["appOverpayment"] = aPInvoice.appOverpayment;
			dataRow["appPaidComplete"] = aPInvoice.appPaidComplete;
			dataRow["appPostedToGl"] = aPInvoice.appPostedToGl;
			dataRow["appTaxReportable"] = aPInvoice.appTaxReportable;
			dataRow["appOriginalExchangeRate"] = aPInvoice.appOriginalExchangeRate;
			dataRow["appOverPaymentHeaderID"] = aPInvoice.appOverPaymentHeaderID;
			dataRow["appOverPaymentSessionID"] = aPInvoice.appOverPaymentSessionID;
			DataRow dataRow6 = dataRow;
			appCreditDate = aPInvoice.appPaidDate;
			dataRow6["appPaidDate"] = (appCreditDate.HasValue ? ((object)appCreditDate.GetValueOrDefault()) : dataRow["appPaidDate"]);
			dataRow["appPaymentTermID"] = aPInvoice.appPaymentTermID;
			dataRow["appPlantDepartmentID"] = aPInvoice.appPlantDepartmentID;
			dataRow["appPlantID"] = aPInvoice.appPlantID;
			DataRow dataRow7 = dataRow;
			appCreditDate = aPInvoice.appPostedDate;
			dataRow7["appPostedDate"] = (appCreditDate.HasValue ? ((object)appCreditDate.GetValueOrDefault()) : dataRow["appPostedDate"]);
			dataRow["appProjectID"] = aPInvoice.appProjectID;
			dataRow["appRetentionBalanceBase"] = aPInvoice.appRetentionBalanceBase;
			dataRow["appRetentionBalanceForeign"] = aPInvoice.appRetentionBalanceForeign;
			dataRow["appRetentionTotalBase"] = aPInvoice.appRetentionTotalBase;
			dataRow["appRetentionTotalForeign"] = aPInvoice.appRetentionTotalForeign;
			dataRow["appSecondFreightTaxAmtBase"] = aPInvoice.appSecondFreightTaxAmtBase;
			dataRow["appSecondFreightTaxAmtForeign"] = aPInvoice.appSecondFreightTaxAmtForeign;
			dataRow["appSecondFreightTaxCodeID"] = aPInvoice.appSecondFreightTaxCodeID;
			dataRow["appSupplierInvoiceNumber"] = aPInvoice.appSupplierInvoiceNumber;
			dataRow["appSupplierOrganizationID"] = aPInvoice.appSupplierOrganizationID;
			if (aPInvoice.CustomFields != null && aPInvoice.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aPInvoice.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the APInvoice [{aPInvoice.appUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the APInvoice [{aPInvoice.appUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
