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

public class ERPARInvoiceRepository : APIBaseRepository, IERPARInvoiceRepository, IAPIBaseRepository, IDisposable
{
	public ERPARInvoiceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesARInvoiceExist(Guid aRInvoiceId)
	{
		InitializeParameterLists();
		base.filterList.Add("arpUniqueID|C", aRInvoiceId);
		base.selectList.Add("arpUniqueID");
		return Task.FromResult(GetAsObject("ARInvoices", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPARInvoiceInformationDto>> GetAllARInvoices(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPARInvoiceInformationDto> collection = new List<ERPARInvoiceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[115]
		{
			"arpArGlAccountID", "arpArInvoiceContactID", "arpArInvoiceLocationID", "arpArInvoiceID", "arpCommissionAmountBase", "arpCreatedBy", "arpCreatedDate", "arpCreditArInvoiceID", "arpCreditDate", "arpCreditReasonID",
			"arpCurrencyRateID", "arpCustomerOrganizationID", "arpDepositAppliedBase", "arpDepositAppliedForeign", "arpDepositBalanceBase", "arpDepositBalanceForeign", "arpDepositGlAccountID", "arpDepositTransferredBase", "arpDepositTransferredForeign", "arpDiscountDueDate",
			"arpDiscountGlAccountID", "arpDiscountTotalBase", "arpDiscountTotalForeign", "arpDueDate", "arpEdiTransferredDate", "arpUniqueID", "arpExchangeRate", "arpFreeOnBoardDescription", "arpFreightAmountBase", "arpFreightAmountForeign",
			"arpFreightGlAccountID", "arpFreightSubtotalBase", "arpFreightSubtotalForeign", "arpFreightTaxAmountBase", "arpFreightTaxAmountForeign", "arpFreightTaxCodeID", "arpFreightTotalBase", "arpFreightTotalForeign", "arpFullInvoiceSubtotalBase", "arpFullInvoiceSubtotalForeign",
			"arpGlFiscalYearID", "arpGlFiscalYearPeriodID", "arpIntraCompanyPostedDate", "arpInvoiceBalanceBase", "arpInvoiceBalanceForeign", "arpInvoiceCommentsRTF", "arpInvoiceCommentsText", "arpInvoiceDate", "arpInvoicePaidBase", "arpInvoicePaidForeign",
			"arpInvoiceSubtotalBase", "arpInvoiceSubtotalForeign", "arpInvoiceTaxAmountBase", "arpInvoiceTaxAmountForeign", "arpInvoiceTotalBase", "arpInvoiceTotalForeign", "arpInvoiceType", "arpAvalaraOverrideTax", "arpAvalaraTaxCalculated", "arpCustomRate",
			"arpDepositCredit", "arpEdiTransferred", "arpIncludeFreightInPrice", "arpIncludeTaxInRetention", "arpIntraCompany", "arpIntraCompanyPosted", "arpOnHold", "arpOpenInvoiceLoad", "arpOverpayment", "arpPaidComplete",
			"arpPostedToGl", "arpReadyToPrint", "arpRecurringInvoice", "arpRefundCheckRequired", "arpLineCommissionTotal", "arpOrderDate", "arpOriginalExchangeRate", "arpOverPaymentHeaderID", "arpOverPaymentSessionID", "arpPaidDate",
			"arpPaymentTermID", "arpPlantDepartmentID", "arpPlantID", "arpPointOfSaleTerminalID", "arpPostedDate", "arpProjectID", "arpResellerCommissionAmount", "arpResellerCommissionRate", "arpResellerContactID", "arpResellerLocationID",
			"arpResellerOrganizationID", "arpRetentionBalanceBase", "arpRetentionBalanceForeign", "arpRetentionPaidBase", "arpRetentionPaidForeign", "arpRetentionTotalBase", "arpRetentionTotalForeign", "arpRowVersion", "arpSalesCommissionTotal", "arpSalesGlAccountID",
			"arpSecondFreightTaxAmtBase", "arpSecondFreightTaxAmtForeign", "arpSecondFreightTaxCodeID", "arpShipContactID", "arpShipLocationID", "arpShipOrganizationID", "arpShippingMethodID", "arpShippingPaymentTypeID", "arpSplitPercentTotal", "arpStandardMessageID",
			"arpTaxDate", "arpTaxSubtotalBase", "arpTaxSubtotalForeign", "arpTotalForResellerCommission", "arpTotalForSalesCommission"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ARInvoices");
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
		using (DataTable dataTable = GetAsDataTable("ARInvoices", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPARInvoiceInformationDto eRPARInvoiceInformationDto = new ERPARInvoiceInformationDto();
				eRPARInvoiceInformationDto.arpArGlAccountID = dataTable.Rows[i].Field<string>("arpArGlAccountID");
				eRPARInvoiceInformationDto.arpArInvoiceContactID = dataTable.Rows[i].Field<string>("arpArInvoiceContactID");
				eRPARInvoiceInformationDto.arpArInvoiceLocationID = dataTable.Rows[i].Field<string>("arpArInvoiceLocationID");
				eRPARInvoiceInformationDto.arpArInvoiceID = dataTable.Rows[i].Field<string>("arpArInvoiceID");
				eRPARInvoiceInformationDto.arpCommissionAmountBase = dataTable.Rows[i].Field<decimal>("arpCommissionAmountBase");
				eRPARInvoiceInformationDto.arpCreatedBy = dataTable.Rows[i].Field<string>("arpCreatedBy");
				eRPARInvoiceInformationDto.arpCreatedDate = dataTable.Rows[i].Field<DateTime?>("arpCreatedDate");
				eRPARInvoiceInformationDto.arpCreditArInvoiceID = dataTable.Rows[i].Field<string>("arpCreditArInvoiceID");
				eRPARInvoiceInformationDto.arpCreditDate = dataTable.Rows[i].Field<DateTime?>("arpCreditDate");
				eRPARInvoiceInformationDto.arpCreditReasonID = dataTable.Rows[i].Field<string>("arpCreditReasonID");
				eRPARInvoiceInformationDto.arpCurrencyRateID = dataTable.Rows[i].Field<string>("arpCurrencyRateID");
				eRPARInvoiceInformationDto.arpCustomerOrganizationID = dataTable.Rows[i].Field<string>("arpCustomerOrganizationID");
				eRPARInvoiceInformationDto.arpDepositAppliedBase = dataTable.Rows[i].Field<decimal>("arpDepositAppliedBase");
				eRPARInvoiceInformationDto.arpDepositAppliedForeign = dataTable.Rows[i].Field<decimal>("arpDepositAppliedForeign");
				eRPARInvoiceInformationDto.arpDepositBalanceBase = dataTable.Rows[i].Field<decimal>("arpDepositBalanceBase");
				eRPARInvoiceInformationDto.arpDepositBalanceForeign = dataTable.Rows[i].Field<decimal>("arpDepositBalanceForeign");
				eRPARInvoiceInformationDto.arpDepositGlAccountID = dataTable.Rows[i].Field<string>("arpDepositGlAccountID");
				eRPARInvoiceInformationDto.arpDepositTransferredBase = dataTable.Rows[i].Field<decimal>("arpDepositTransferredBase");
				eRPARInvoiceInformationDto.arpDepositTransferredForeign = dataTable.Rows[i].Field<decimal>("arpDepositTransferredForeign");
				eRPARInvoiceInformationDto.arpDiscountDueDate = dataTable.Rows[i].Field<DateTime?>("arpDiscountDueDate");
				eRPARInvoiceInformationDto.arpDiscountGlAccountID = dataTable.Rows[i].Field<string>("arpDiscountGlAccountID");
				eRPARInvoiceInformationDto.arpDiscountTotalBase = dataTable.Rows[i].Field<decimal>("arpDiscountTotalBase");
				eRPARInvoiceInformationDto.arpDiscountTotalForeign = dataTable.Rows[i].Field<decimal>("arpDiscountTotalForeign");
				eRPARInvoiceInformationDto.arpDueDate = dataTable.Rows[i].Field<DateTime?>("arpDueDate");
				eRPARInvoiceInformationDto.arpEdiTransferredDate = dataTable.Rows[i].Field<DateTime?>("arpEdiTransferredDate");
				eRPARInvoiceInformationDto.arpUniqueID = dataTable.Rows[i].Field<Guid>("arpUniqueID");
				eRPARInvoiceInformationDto.arpExchangeRate = dataTable.Rows[i].Field<decimal>("arpExchangeRate");
				eRPARInvoiceInformationDto.arpFreeOnBoardDescription = dataTable.Rows[i].Field<string>("arpFreeOnBoardDescription");
				eRPARInvoiceInformationDto.arpFreightAmountBase = dataTable.Rows[i].Field<decimal>("arpFreightAmountBase");
				eRPARInvoiceInformationDto.arpFreightAmountForeign = dataTable.Rows[i].Field<decimal>("arpFreightAmountForeign");
				eRPARInvoiceInformationDto.arpFreightGlAccountID = dataTable.Rows[i].Field<string>("arpFreightGlAccountID");
				eRPARInvoiceInformationDto.arpFreightSubtotalBase = dataTable.Rows[i].Field<decimal>("arpFreightSubtotalBase");
				eRPARInvoiceInformationDto.arpFreightSubtotalForeign = dataTable.Rows[i].Field<decimal>("arpFreightSubtotalForeign");
				eRPARInvoiceInformationDto.arpFreightTaxAmountBase = dataTable.Rows[i].Field<decimal>("arpFreightTaxAmountBase");
				eRPARInvoiceInformationDto.arpFreightTaxAmountForeign = dataTable.Rows[i].Field<decimal>("arpFreightTaxAmountForeign");
				eRPARInvoiceInformationDto.arpFreightTaxCodeID = dataTable.Rows[i].Field<string>("arpFreightTaxCodeID");
				eRPARInvoiceInformationDto.arpFreightTotalBase = dataTable.Rows[i].Field<decimal>("arpFreightTotalBase");
				eRPARInvoiceInformationDto.arpFreightTotalForeign = dataTable.Rows[i].Field<decimal>("arpFreightTotalForeign");
				eRPARInvoiceInformationDto.arpFullInvoiceSubtotalBase = dataTable.Rows[i].Field<decimal>("arpFullInvoiceSubtotalBase");
				eRPARInvoiceInformationDto.arpFullInvoiceSubtotalForeign = dataTable.Rows[i].Field<decimal>("arpFullInvoiceSubtotalForeign");
				eRPARInvoiceInformationDto.arpGlFiscalYearID = dataTable.Rows[i].Field<short>("arpGlFiscalYearID");
				eRPARInvoiceInformationDto.arpGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("arpGlFiscalYearPeriodID");
				eRPARInvoiceInformationDto.arpIntraCompanyPostedDate = dataTable.Rows[i].Field<DateTime?>("arpIntraCompanyPostedDate");
				eRPARInvoiceInformationDto.arpInvoiceBalanceBase = dataTable.Rows[i].Field<decimal>("arpInvoiceBalanceBase");
				eRPARInvoiceInformationDto.arpInvoiceBalanceForeign = dataTable.Rows[i].Field<decimal>("arpInvoiceBalanceForeign");
				eRPARInvoiceInformationDto.arpInvoiceCommentsRTF = dataTable.Rows[i].Field<string>("arpInvoiceCommentsRTF");
				eRPARInvoiceInformationDto.arpInvoiceCommentsText = dataTable.Rows[i].Field<string>("arpInvoiceCommentsText");
				eRPARInvoiceInformationDto.arpInvoiceDate = dataTable.Rows[i].Field<DateTime?>("arpInvoiceDate");
				eRPARInvoiceInformationDto.arpInvoicePaidBase = dataTable.Rows[i].Field<decimal>("arpInvoicePaidBase");
				eRPARInvoiceInformationDto.arpInvoicePaidForeign = dataTable.Rows[i].Field<decimal>("arpInvoicePaidForeign");
				eRPARInvoiceInformationDto.arpInvoiceSubtotalBase = dataTable.Rows[i].Field<decimal>("arpInvoiceSubtotalBase");
				eRPARInvoiceInformationDto.arpInvoiceSubtotalForeign = dataTable.Rows[i].Field<decimal>("arpInvoiceSubtotalForeign");
				eRPARInvoiceInformationDto.arpInvoiceTaxAmountBase = dataTable.Rows[i].Field<decimal>("arpInvoiceTaxAmountBase");
				eRPARInvoiceInformationDto.arpInvoiceTaxAmountForeign = dataTable.Rows[i].Field<decimal>("arpInvoiceTaxAmountForeign");
				eRPARInvoiceInformationDto.arpInvoiceTotalBase = dataTable.Rows[i].Field<decimal>("arpInvoiceTotalBase");
				eRPARInvoiceInformationDto.arpInvoiceTotalForeign = dataTable.Rows[i].Field<decimal>("arpInvoiceTotalForeign");
				eRPARInvoiceInformationDto.arpInvoiceType = dataTable.Rows[i].Field<byte>("arpInvoiceType");
				eRPARInvoiceInformationDto.arpAvalaraOverrideTax = dataTable.Rows[i].Field<bool>("arpAvalaraOverrideTax");
				eRPARInvoiceInformationDto.arpAvalaraTaxCalculated = dataTable.Rows[i].Field<bool>("arpAvalaraTaxCalculated");
				eRPARInvoiceInformationDto.arpCustomRate = dataTable.Rows[i].Field<bool>("arpCustomRate");
				eRPARInvoiceInformationDto.arpDepositCredit = dataTable.Rows[i].Field<bool>("arpDepositCredit");
				eRPARInvoiceInformationDto.arpEdiTransferred = dataTable.Rows[i].Field<bool>("arpEdiTransferred");
				eRPARInvoiceInformationDto.arpIncludeFreightInPrice = dataTable.Rows[i].Field<bool>("arpIncludeFreightInPrice");
				eRPARInvoiceInformationDto.arpIncludeTaxInRetention = dataTable.Rows[i].Field<bool>("arpIncludeTaxInRetention");
				eRPARInvoiceInformationDto.arpIntraCompany = dataTable.Rows[i].Field<bool>("arpIntraCompany");
				eRPARInvoiceInformationDto.arpIntraCompanyPosted = dataTable.Rows[i].Field<bool>("arpIntraCompanyPosted");
				eRPARInvoiceInformationDto.arpOnHold = dataTable.Rows[i].Field<bool>("arpOnHold");
				eRPARInvoiceInformationDto.arpOpenInvoiceLoad = dataTable.Rows[i].Field<bool>("arpOpenInvoiceLoad");
				eRPARInvoiceInformationDto.arpOverpayment = dataTable.Rows[i].Field<bool>("arpOverpayment");
				eRPARInvoiceInformationDto.arpPaidComplete = dataTable.Rows[i].Field<bool>("arpPaidComplete");
				eRPARInvoiceInformationDto.arpPostedToGl = dataTable.Rows[i].Field<bool>("arpPostedToGl");
				eRPARInvoiceInformationDto.arpReadyToPrint = dataTable.Rows[i].Field<bool>("arpReadyToPrint");
				eRPARInvoiceInformationDto.arpRecurringInvoice = dataTable.Rows[i].Field<bool>("arpRecurringInvoice");
				eRPARInvoiceInformationDto.arpRefundCheckRequired = dataTable.Rows[i].Field<bool>("arpRefundCheckRequired");
				eRPARInvoiceInformationDto.arpLineCommissionTotal = dataTable.Rows[i].Field<decimal>("arpLineCommissionTotal");
				eRPARInvoiceInformationDto.arpOrderDate = dataTable.Rows[i].Field<DateTime?>("arpOrderDate");
				eRPARInvoiceInformationDto.arpOriginalExchangeRate = dataTable.Rows[i].Field<decimal>("arpOriginalExchangeRate");
				eRPARInvoiceInformationDto.arpOverPaymentHeaderID = dataTable.Rows[i].Field<int>("arpOverPaymentHeaderID");
				eRPARInvoiceInformationDto.arpOverPaymentSessionID = dataTable.Rows[i].Field<int>("arpOverPaymentSessionID");
				eRPARInvoiceInformationDto.arpPaidDate = dataTable.Rows[i].Field<DateTime?>("arpPaidDate");
				eRPARInvoiceInformationDto.arpPaymentTermID = dataTable.Rows[i].Field<string>("arpPaymentTermID");
				eRPARInvoiceInformationDto.arpPlantDepartmentID = dataTable.Rows[i].Field<string>("arpPlantDepartmentID");
				eRPARInvoiceInformationDto.arpPlantID = dataTable.Rows[i].Field<string>("arpPlantID");
				eRPARInvoiceInformationDto.arpPointOfSaleTerminalID = dataTable.Rows[i].Field<string>("arpPointOfSaleTerminalID");
				eRPARInvoiceInformationDto.arpPostedDate = dataTable.Rows[i].Field<DateTime?>("arpPostedDate");
				eRPARInvoiceInformationDto.arpProjectID = dataTable.Rows[i].Field<string>("arpProjectID");
				eRPARInvoiceInformationDto.arpResellerCommissionAmount = dataTable.Rows[i].Field<decimal>("arpResellerCommissionAmount");
				eRPARInvoiceInformationDto.arpResellerCommissionRate = dataTable.Rows[i].Field<decimal>("arpResellerCommissionRate");
				eRPARInvoiceInformationDto.arpResellerContactID = dataTable.Rows[i].Field<string>("arpResellerContactID");
				eRPARInvoiceInformationDto.arpResellerLocationID = dataTable.Rows[i].Field<string>("arpResellerLocationID");
				eRPARInvoiceInformationDto.arpResellerOrganizationID = dataTable.Rows[i].Field<string>("arpResellerOrganizationID");
				eRPARInvoiceInformationDto.arpRetentionBalanceBase = dataTable.Rows[i].Field<decimal>("arpRetentionBalanceBase");
				eRPARInvoiceInformationDto.arpRetentionBalanceForeign = dataTable.Rows[i].Field<decimal>("arpRetentionBalanceForeign");
				eRPARInvoiceInformationDto.arpRetentionPaidBase = dataTable.Rows[i].Field<decimal>("arpRetentionPaidBase");
				eRPARInvoiceInformationDto.arpRetentionPaidForeign = dataTable.Rows[i].Field<decimal>("arpRetentionPaidForeign");
				eRPARInvoiceInformationDto.arpRetentionTotalBase = dataTable.Rows[i].Field<decimal>("arpRetentionTotalBase");
				eRPARInvoiceInformationDto.arpRetentionTotalForeign = dataTable.Rows[i].Field<decimal>("arpRetentionTotalForeign");
				eRPARInvoiceInformationDto.arpRowVersion = dataTable.Rows[i].Field<byte[]>("arpRowVersion");
				eRPARInvoiceInformationDto.arpSalesCommissionTotal = dataTable.Rows[i].Field<decimal>("arpSalesCommissionTotal");
				eRPARInvoiceInformationDto.arpSalesGlAccountID = dataTable.Rows[i].Field<string>("arpSalesGlAccountID");
				eRPARInvoiceInformationDto.arpSecondFreightTaxAmtBase = dataTable.Rows[i].Field<decimal>("arpSecondFreightTaxAmtBase");
				eRPARInvoiceInformationDto.arpSecondFreightTaxAmtForeign = dataTable.Rows[i].Field<decimal>("arpSecondFreightTaxAmtForeign");
				eRPARInvoiceInformationDto.arpSecondFreightTaxCodeID = dataTable.Rows[i].Field<string>("arpSecondFreightTaxCodeID");
				eRPARInvoiceInformationDto.arpShipContactID = dataTable.Rows[i].Field<string>("arpShipContactID");
				eRPARInvoiceInformationDto.arpShipLocationID = dataTable.Rows[i].Field<string>("arpShipLocationID");
				eRPARInvoiceInformationDto.arpShipOrganizationID = dataTable.Rows[i].Field<string>("arpShipOrganizationID");
				eRPARInvoiceInformationDto.arpShippingMethodID = dataTable.Rows[i].Field<string>("arpShippingMethodID");
				eRPARInvoiceInformationDto.arpShippingPaymentTypeID = dataTable.Rows[i].Field<string>("arpShippingPaymentTypeID");
				eRPARInvoiceInformationDto.arpSplitPercentTotal = dataTable.Rows[i].Field<decimal>("arpSplitPercentTotal");
				eRPARInvoiceInformationDto.arpStandardMessageID = dataTable.Rows[i].Field<string>("arpStandardMessageID");
				eRPARInvoiceInformationDto.arpTaxDate = dataTable.Rows[i].Field<DateTime?>("arpTaxDate");
				eRPARInvoiceInformationDto.arpTaxSubtotalBase = dataTable.Rows[i].Field<decimal>("arpTaxSubtotalBase");
				eRPARInvoiceInformationDto.arpTaxSubtotalForeign = dataTable.Rows[i].Field<decimal>("arpTaxSubtotalForeign");
				eRPARInvoiceInformationDto.arpTotalForResellerCommission = dataTable.Rows[i].Field<decimal>("arpTotalForResellerCommission");
				eRPARInvoiceInformationDto.arpTotalForSalesCommission = dataTable.Rows[i].Field<decimal>("arpTotalForSalesCommission");
				eRPARInvoiceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPARInvoiceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPARInvoiceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPARInvoiceInformationDto> GetARInvoice(Guid aRInvoiceId)
	{
		ERPARInvoiceInformationDto eRPARInvoiceInformationDto = new ERPARInvoiceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[115]
		{
			"arpArGlAccountID", "arpArInvoiceContactID", "arpArInvoiceLocationID", "arpArInvoiceID", "arpCommissionAmountBase", "arpCreatedBy", "arpCreatedDate", "arpCreditArInvoiceID", "arpCreditDate", "arpCreditReasonID",
			"arpCurrencyRateID", "arpCustomerOrganizationID", "arpDepositAppliedBase", "arpDepositAppliedForeign", "arpDepositBalanceBase", "arpDepositBalanceForeign", "arpDepositGlAccountID", "arpDepositTransferredBase", "arpDepositTransferredForeign", "arpDiscountDueDate",
			"arpDiscountGlAccountID", "arpDiscountTotalBase", "arpDiscountTotalForeign", "arpDueDate", "arpEdiTransferredDate", "arpUniqueID", "arpExchangeRate", "arpFreeOnBoardDescription", "arpFreightAmountBase", "arpFreightAmountForeign",
			"arpFreightGlAccountID", "arpFreightSubtotalBase", "arpFreightSubtotalForeign", "arpFreightTaxAmountBase", "arpFreightTaxAmountForeign", "arpFreightTaxCodeID", "arpFreightTotalBase", "arpFreightTotalForeign", "arpFullInvoiceSubtotalBase", "arpFullInvoiceSubtotalForeign",
			"arpGlFiscalYearID", "arpGlFiscalYearPeriodID", "arpIntraCompanyPostedDate", "arpInvoiceBalanceBase", "arpInvoiceBalanceForeign", "arpInvoiceCommentsRTF", "arpInvoiceCommentsText", "arpInvoiceDate", "arpInvoicePaidBase", "arpInvoicePaidForeign",
			"arpInvoiceSubtotalBase", "arpInvoiceSubtotalForeign", "arpInvoiceTaxAmountBase", "arpInvoiceTaxAmountForeign", "arpInvoiceTotalBase", "arpInvoiceTotalForeign", "arpInvoiceType", "arpAvalaraOverrideTax", "arpAvalaraTaxCalculated", "arpCustomRate",
			"arpDepositCredit", "arpEdiTransferred", "arpIncludeFreightInPrice", "arpIncludeTaxInRetention", "arpIntraCompany", "arpIntraCompanyPosted", "arpOnHold", "arpOpenInvoiceLoad", "arpOverpayment", "arpPaidComplete",
			"arpPostedToGl", "arpReadyToPrint", "arpRecurringInvoice", "arpRefundCheckRequired", "arpLineCommissionTotal", "arpOrderDate", "arpOriginalExchangeRate", "arpOverPaymentHeaderID", "arpOverPaymentSessionID", "arpPaidDate",
			"arpPaymentTermID", "arpPlantDepartmentID", "arpPlantID", "arpPointOfSaleTerminalID", "arpPostedDate", "arpProjectID", "arpResellerCommissionAmount", "arpResellerCommissionRate", "arpResellerContactID", "arpResellerLocationID",
			"arpResellerOrganizationID", "arpRetentionBalanceBase", "arpRetentionBalanceForeign", "arpRetentionPaidBase", "arpRetentionPaidForeign", "arpRetentionTotalBase", "arpRetentionTotalForeign", "arpRowVersion", "arpSalesCommissionTotal", "arpSalesGlAccountID",
			"arpSecondFreightTaxAmtBase", "arpSecondFreightTaxAmtForeign", "arpSecondFreightTaxCodeID", "arpShipContactID", "arpShipLocationID", "arpShipOrganizationID", "arpShippingMethodID", "arpShippingPaymentTypeID", "arpSplitPercentTotal", "arpStandardMessageID",
			"arpTaxDate", "arpTaxSubtotalBase", "arpTaxSubtotalForeign", "arpTotalForResellerCommission", "arpTotalForSalesCommission"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("arpUniqueID|C", aRInvoiceId);
		AddCustomFieldsToSelectList("ARInvoices");
		using (DataTable dataTable = GetAsDataTable("ARInvoices", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPARInvoiceInformationDto);
			}
			eRPARInvoiceInformationDto.arpArGlAccountID = dataTable.Rows[0].Field<string>("arpArGlAccountID");
			eRPARInvoiceInformationDto.arpArInvoiceContactID = dataTable.Rows[0].Field<string>("arpArInvoiceContactID");
			eRPARInvoiceInformationDto.arpArInvoiceLocationID = dataTable.Rows[0].Field<string>("arpArInvoiceLocationID");
			eRPARInvoiceInformationDto.arpArInvoiceID = dataTable.Rows[0].Field<string>("arpArInvoiceID");
			eRPARInvoiceInformationDto.arpCommissionAmountBase = dataTable.Rows[0].Field<decimal>("arpCommissionAmountBase");
			eRPARInvoiceInformationDto.arpCreatedBy = dataTable.Rows[0].Field<string>("arpCreatedBy");
			eRPARInvoiceInformationDto.arpCreatedDate = dataTable.Rows[0].Field<DateTime?>("arpCreatedDate");
			eRPARInvoiceInformationDto.arpCreditArInvoiceID = dataTable.Rows[0].Field<string>("arpCreditArInvoiceID");
			eRPARInvoiceInformationDto.arpCreditDate = dataTable.Rows[0].Field<DateTime?>("arpCreditDate");
			eRPARInvoiceInformationDto.arpCreditReasonID = dataTable.Rows[0].Field<string>("arpCreditReasonID");
			eRPARInvoiceInformationDto.arpCurrencyRateID = dataTable.Rows[0].Field<string>("arpCurrencyRateID");
			eRPARInvoiceInformationDto.arpCustomerOrganizationID = dataTable.Rows[0].Field<string>("arpCustomerOrganizationID");
			eRPARInvoiceInformationDto.arpDepositAppliedBase = dataTable.Rows[0].Field<decimal>("arpDepositAppliedBase");
			eRPARInvoiceInformationDto.arpDepositAppliedForeign = dataTable.Rows[0].Field<decimal>("arpDepositAppliedForeign");
			eRPARInvoiceInformationDto.arpDepositBalanceBase = dataTable.Rows[0].Field<decimal>("arpDepositBalanceBase");
			eRPARInvoiceInformationDto.arpDepositBalanceForeign = dataTable.Rows[0].Field<decimal>("arpDepositBalanceForeign");
			eRPARInvoiceInformationDto.arpDepositGlAccountID = dataTable.Rows[0].Field<string>("arpDepositGlAccountID");
			eRPARInvoiceInformationDto.arpDepositTransferredBase = dataTable.Rows[0].Field<decimal>("arpDepositTransferredBase");
			eRPARInvoiceInformationDto.arpDepositTransferredForeign = dataTable.Rows[0].Field<decimal>("arpDepositTransferredForeign");
			eRPARInvoiceInformationDto.arpDiscountDueDate = dataTable.Rows[0].Field<DateTime?>("arpDiscountDueDate");
			eRPARInvoiceInformationDto.arpDiscountGlAccountID = dataTable.Rows[0].Field<string>("arpDiscountGlAccountID");
			eRPARInvoiceInformationDto.arpDiscountTotalBase = dataTable.Rows[0].Field<decimal>("arpDiscountTotalBase");
			eRPARInvoiceInformationDto.arpDiscountTotalForeign = dataTable.Rows[0].Field<decimal>("arpDiscountTotalForeign");
			eRPARInvoiceInformationDto.arpDueDate = dataTable.Rows[0].Field<DateTime?>("arpDueDate");
			eRPARInvoiceInformationDto.arpEdiTransferredDate = dataTable.Rows[0].Field<DateTime?>("arpEdiTransferredDate");
			eRPARInvoiceInformationDto.arpUniqueID = dataTable.Rows[0].Field<Guid>("arpUniqueID");
			eRPARInvoiceInformationDto.arpExchangeRate = dataTable.Rows[0].Field<decimal>("arpExchangeRate");
			eRPARInvoiceInformationDto.arpFreeOnBoardDescription = dataTable.Rows[0].Field<string>("arpFreeOnBoardDescription");
			eRPARInvoiceInformationDto.arpFreightAmountBase = dataTable.Rows[0].Field<decimal>("arpFreightAmountBase");
			eRPARInvoiceInformationDto.arpFreightAmountForeign = dataTable.Rows[0].Field<decimal>("arpFreightAmountForeign");
			eRPARInvoiceInformationDto.arpFreightGlAccountID = dataTable.Rows[0].Field<string>("arpFreightGlAccountID");
			eRPARInvoiceInformationDto.arpFreightSubtotalBase = dataTable.Rows[0].Field<decimal>("arpFreightSubtotalBase");
			eRPARInvoiceInformationDto.arpFreightSubtotalForeign = dataTable.Rows[0].Field<decimal>("arpFreightSubtotalForeign");
			eRPARInvoiceInformationDto.arpFreightTaxAmountBase = dataTable.Rows[0].Field<decimal>("arpFreightTaxAmountBase");
			eRPARInvoiceInformationDto.arpFreightTaxAmountForeign = dataTable.Rows[0].Field<decimal>("arpFreightTaxAmountForeign");
			eRPARInvoiceInformationDto.arpFreightTaxCodeID = dataTable.Rows[0].Field<string>("arpFreightTaxCodeID");
			eRPARInvoiceInformationDto.arpFreightTotalBase = dataTable.Rows[0].Field<decimal>("arpFreightTotalBase");
			eRPARInvoiceInformationDto.arpFreightTotalForeign = dataTable.Rows[0].Field<decimal>("arpFreightTotalForeign");
			eRPARInvoiceInformationDto.arpFullInvoiceSubtotalBase = dataTable.Rows[0].Field<decimal>("arpFullInvoiceSubtotalBase");
			eRPARInvoiceInformationDto.arpFullInvoiceSubtotalForeign = dataTable.Rows[0].Field<decimal>("arpFullInvoiceSubtotalForeign");
			eRPARInvoiceInformationDto.arpGlFiscalYearID = dataTable.Rows[0].Field<short>("arpGlFiscalYearID");
			eRPARInvoiceInformationDto.arpGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("arpGlFiscalYearPeriodID");
			eRPARInvoiceInformationDto.arpIntraCompanyPostedDate = dataTable.Rows[0].Field<DateTime?>("arpIntraCompanyPostedDate");
			eRPARInvoiceInformationDto.arpInvoiceBalanceBase = dataTable.Rows[0].Field<decimal>("arpInvoiceBalanceBase");
			eRPARInvoiceInformationDto.arpInvoiceBalanceForeign = dataTable.Rows[0].Field<decimal>("arpInvoiceBalanceForeign");
			eRPARInvoiceInformationDto.arpInvoiceCommentsRTF = dataTable.Rows[0].Field<string>("arpInvoiceCommentsRTF");
			eRPARInvoiceInformationDto.arpInvoiceCommentsText = dataTable.Rows[0].Field<string>("arpInvoiceCommentsText");
			eRPARInvoiceInformationDto.arpInvoiceDate = dataTable.Rows[0].Field<DateTime?>("arpInvoiceDate");
			eRPARInvoiceInformationDto.arpInvoicePaidBase = dataTable.Rows[0].Field<decimal>("arpInvoicePaidBase");
			eRPARInvoiceInformationDto.arpInvoicePaidForeign = dataTable.Rows[0].Field<decimal>("arpInvoicePaidForeign");
			eRPARInvoiceInformationDto.arpInvoiceSubtotalBase = dataTable.Rows[0].Field<decimal>("arpInvoiceSubtotalBase");
			eRPARInvoiceInformationDto.arpInvoiceSubtotalForeign = dataTable.Rows[0].Field<decimal>("arpInvoiceSubtotalForeign");
			eRPARInvoiceInformationDto.arpInvoiceTaxAmountBase = dataTable.Rows[0].Field<decimal>("arpInvoiceTaxAmountBase");
			eRPARInvoiceInformationDto.arpInvoiceTaxAmountForeign = dataTable.Rows[0].Field<decimal>("arpInvoiceTaxAmountForeign");
			eRPARInvoiceInformationDto.arpInvoiceTotalBase = dataTable.Rows[0].Field<decimal>("arpInvoiceTotalBase");
			eRPARInvoiceInformationDto.arpInvoiceTotalForeign = dataTable.Rows[0].Field<decimal>("arpInvoiceTotalForeign");
			eRPARInvoiceInformationDto.arpInvoiceType = dataTable.Rows[0].Field<byte>("arpInvoiceType");
			eRPARInvoiceInformationDto.arpAvalaraOverrideTax = dataTable.Rows[0].Field<bool>("arpAvalaraOverrideTax");
			eRPARInvoiceInformationDto.arpAvalaraTaxCalculated = dataTable.Rows[0].Field<bool>("arpAvalaraTaxCalculated");
			eRPARInvoiceInformationDto.arpCustomRate = dataTable.Rows[0].Field<bool>("arpCustomRate");
			eRPARInvoiceInformationDto.arpDepositCredit = dataTable.Rows[0].Field<bool>("arpDepositCredit");
			eRPARInvoiceInformationDto.arpEdiTransferred = dataTable.Rows[0].Field<bool>("arpEdiTransferred");
			eRPARInvoiceInformationDto.arpIncludeFreightInPrice = dataTable.Rows[0].Field<bool>("arpIncludeFreightInPrice");
			eRPARInvoiceInformationDto.arpIncludeTaxInRetention = dataTable.Rows[0].Field<bool>("arpIncludeTaxInRetention");
			eRPARInvoiceInformationDto.arpIntraCompany = dataTable.Rows[0].Field<bool>("arpIntraCompany");
			eRPARInvoiceInformationDto.arpIntraCompanyPosted = dataTable.Rows[0].Field<bool>("arpIntraCompanyPosted");
			eRPARInvoiceInformationDto.arpOnHold = dataTable.Rows[0].Field<bool>("arpOnHold");
			eRPARInvoiceInformationDto.arpOpenInvoiceLoad = dataTable.Rows[0].Field<bool>("arpOpenInvoiceLoad");
			eRPARInvoiceInformationDto.arpOverpayment = dataTable.Rows[0].Field<bool>("arpOverpayment");
			eRPARInvoiceInformationDto.arpPaidComplete = dataTable.Rows[0].Field<bool>("arpPaidComplete");
			eRPARInvoiceInformationDto.arpPostedToGl = dataTable.Rows[0].Field<bool>("arpPostedToGl");
			eRPARInvoiceInformationDto.arpReadyToPrint = dataTable.Rows[0].Field<bool>("arpReadyToPrint");
			eRPARInvoiceInformationDto.arpRecurringInvoice = dataTable.Rows[0].Field<bool>("arpRecurringInvoice");
			eRPARInvoiceInformationDto.arpRefundCheckRequired = dataTable.Rows[0].Field<bool>("arpRefundCheckRequired");
			eRPARInvoiceInformationDto.arpLineCommissionTotal = dataTable.Rows[0].Field<decimal>("arpLineCommissionTotal");
			eRPARInvoiceInformationDto.arpOrderDate = dataTable.Rows[0].Field<DateTime?>("arpOrderDate");
			eRPARInvoiceInformationDto.arpOriginalExchangeRate = dataTable.Rows[0].Field<decimal>("arpOriginalExchangeRate");
			eRPARInvoiceInformationDto.arpOverPaymentHeaderID = dataTable.Rows[0].Field<int>("arpOverPaymentHeaderID");
			eRPARInvoiceInformationDto.arpOverPaymentSessionID = dataTable.Rows[0].Field<int>("arpOverPaymentSessionID");
			eRPARInvoiceInformationDto.arpPaidDate = dataTable.Rows[0].Field<DateTime?>("arpPaidDate");
			eRPARInvoiceInformationDto.arpPaymentTermID = dataTable.Rows[0].Field<string>("arpPaymentTermID");
			eRPARInvoiceInformationDto.arpPlantDepartmentID = dataTable.Rows[0].Field<string>("arpPlantDepartmentID");
			eRPARInvoiceInformationDto.arpPlantID = dataTable.Rows[0].Field<string>("arpPlantID");
			eRPARInvoiceInformationDto.arpPointOfSaleTerminalID = dataTable.Rows[0].Field<string>("arpPointOfSaleTerminalID");
			eRPARInvoiceInformationDto.arpPostedDate = dataTable.Rows[0].Field<DateTime?>("arpPostedDate");
			eRPARInvoiceInformationDto.arpProjectID = dataTable.Rows[0].Field<string>("arpProjectID");
			eRPARInvoiceInformationDto.arpResellerCommissionAmount = dataTable.Rows[0].Field<decimal>("arpResellerCommissionAmount");
			eRPARInvoiceInformationDto.arpResellerCommissionRate = dataTable.Rows[0].Field<decimal>("arpResellerCommissionRate");
			eRPARInvoiceInformationDto.arpResellerContactID = dataTable.Rows[0].Field<string>("arpResellerContactID");
			eRPARInvoiceInformationDto.arpResellerLocationID = dataTable.Rows[0].Field<string>("arpResellerLocationID");
			eRPARInvoiceInformationDto.arpResellerOrganizationID = dataTable.Rows[0].Field<string>("arpResellerOrganizationID");
			eRPARInvoiceInformationDto.arpRetentionBalanceBase = dataTable.Rows[0].Field<decimal>("arpRetentionBalanceBase");
			eRPARInvoiceInformationDto.arpRetentionBalanceForeign = dataTable.Rows[0].Field<decimal>("arpRetentionBalanceForeign");
			eRPARInvoiceInformationDto.arpRetentionPaidBase = dataTable.Rows[0].Field<decimal>("arpRetentionPaidBase");
			eRPARInvoiceInformationDto.arpRetentionPaidForeign = dataTable.Rows[0].Field<decimal>("arpRetentionPaidForeign");
			eRPARInvoiceInformationDto.arpRetentionTotalBase = dataTable.Rows[0].Field<decimal>("arpRetentionTotalBase");
			eRPARInvoiceInformationDto.arpRetentionTotalForeign = dataTable.Rows[0].Field<decimal>("arpRetentionTotalForeign");
			eRPARInvoiceInformationDto.arpRowVersion = dataTable.Rows[0].Field<byte[]>("arpRowVersion");
			eRPARInvoiceInformationDto.arpSalesCommissionTotal = dataTable.Rows[0].Field<decimal>("arpSalesCommissionTotal");
			eRPARInvoiceInformationDto.arpSalesGlAccountID = dataTable.Rows[0].Field<string>("arpSalesGlAccountID");
			eRPARInvoiceInformationDto.arpSecondFreightTaxAmtBase = dataTable.Rows[0].Field<decimal>("arpSecondFreightTaxAmtBase");
			eRPARInvoiceInformationDto.arpSecondFreightTaxAmtForeign = dataTable.Rows[0].Field<decimal>("arpSecondFreightTaxAmtForeign");
			eRPARInvoiceInformationDto.arpSecondFreightTaxCodeID = dataTable.Rows[0].Field<string>("arpSecondFreightTaxCodeID");
			eRPARInvoiceInformationDto.arpShipContactID = dataTable.Rows[0].Field<string>("arpShipContactID");
			eRPARInvoiceInformationDto.arpShipLocationID = dataTable.Rows[0].Field<string>("arpShipLocationID");
			eRPARInvoiceInformationDto.arpShipOrganizationID = dataTable.Rows[0].Field<string>("arpShipOrganizationID");
			eRPARInvoiceInformationDto.arpShippingMethodID = dataTable.Rows[0].Field<string>("arpShippingMethodID");
			eRPARInvoiceInformationDto.arpShippingPaymentTypeID = dataTable.Rows[0].Field<string>("arpShippingPaymentTypeID");
			eRPARInvoiceInformationDto.arpSplitPercentTotal = dataTable.Rows[0].Field<decimal>("arpSplitPercentTotal");
			eRPARInvoiceInformationDto.arpStandardMessageID = dataTable.Rows[0].Field<string>("arpStandardMessageID");
			eRPARInvoiceInformationDto.arpTaxDate = dataTable.Rows[0].Field<DateTime?>("arpTaxDate");
			eRPARInvoiceInformationDto.arpTaxSubtotalBase = dataTable.Rows[0].Field<decimal>("arpTaxSubtotalBase");
			eRPARInvoiceInformationDto.arpTaxSubtotalForeign = dataTable.Rows[0].Field<decimal>("arpTaxSubtotalForeign");
			eRPARInvoiceInformationDto.arpTotalForResellerCommission = dataTable.Rows[0].Field<decimal>("arpTotalForResellerCommission");
			eRPARInvoiceInformationDto.arpTotalForSalesCommission = dataTable.Rows[0].Field<decimal>("arpTotalForSalesCommission");
			eRPARInvoiceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPARInvoiceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPARInvoiceInformationDto);
	}

	public Task<APIValidationInfoDto> SaveARInvoice(ERPARInvoiceDto aRInvoice)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ARInvoices WHERE arpUniqueID = " + M1Util.ConvertToLinq(aRInvoice.arpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["arpArInvoiceID"] = aRInvoice.arpArInvoiceID.ToUpper();
				aRInvoice.arpUniqueID = ((aRInvoice.arpUniqueID == Guid.Empty) ? Guid.NewGuid() : aRInvoice.arpUniqueID);
				dataRow["arpUniqueID"] = aRInvoice.arpUniqueID;
				dataRow["arpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["arpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ARInvoice could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aRInvoice.arpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ARInvoice is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["arpRowVersion"], aRInvoice.arpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ARInvoice has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ARInvoice again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["arpArGlAccountID"] = aRInvoice.arpArGlAccountID;
			dataRow["arpArInvoiceContactID"] = aRInvoice.arpArInvoiceContactID;
			dataRow["arpArInvoiceLocationID"] = aRInvoice.arpArInvoiceLocationID;
			dataRow["arpCommissionAmountBase"] = aRInvoice.arpCommissionAmountBase;
			dataRow["arpCreditArInvoiceID"] = aRInvoice.arpCreditArInvoiceID;
			DataRow dataRow2 = dataRow;
			DateTime? arpCreditDate = aRInvoice.arpCreditDate;
			dataRow2["arpCreditDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpCreditDate"]);
			dataRow["arpCreditReasonID"] = aRInvoice.arpCreditReasonID;
			dataRow["arpCurrencyRateID"] = aRInvoice.arpCurrencyRateID;
			dataRow["arpCustomerOrganizationID"] = aRInvoice.arpCustomerOrganizationID;
			dataRow["arpDepositAppliedBase"] = aRInvoice.arpDepositAppliedBase;
			dataRow["arpDepositAppliedForeign"] = aRInvoice.arpDepositAppliedForeign;
			dataRow["arpDepositBalanceBase"] = aRInvoice.arpDepositBalanceBase;
			dataRow["arpDepositBalanceForeign"] = aRInvoice.arpDepositBalanceForeign;
			dataRow["arpDepositGlAccountID"] = aRInvoice.arpDepositGlAccountID;
			dataRow["arpDepositTransferredBase"] = aRInvoice.arpDepositTransferredBase;
			dataRow["arpDepositTransferredForeign"] = aRInvoice.arpDepositTransferredForeign;
			DataRow dataRow3 = dataRow;
			arpCreditDate = aRInvoice.arpDiscountDueDate;
			dataRow3["arpDiscountDueDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpDiscountDueDate"]);
			dataRow["arpDiscountGlAccountID"] = aRInvoice.arpDiscountGlAccountID;
			dataRow["arpDiscountTotalBase"] = aRInvoice.arpDiscountTotalBase;
			dataRow["arpDiscountTotalForeign"] = aRInvoice.arpDiscountTotalForeign;
			DataRow dataRow4 = dataRow;
			arpCreditDate = aRInvoice.arpDueDate;
			dataRow4["arpDueDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpDueDate"]);
			DataRow dataRow5 = dataRow;
			arpCreditDate = aRInvoice.arpEdiTransferredDate;
			dataRow5["arpEdiTransferredDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpEdiTransferredDate"]);
			dataRow["arpExchangeRate"] = aRInvoice.arpExchangeRate;
			dataRow["arpFreeOnBoardDescription"] = aRInvoice.arpFreeOnBoardDescription;
			dataRow["arpFreightAmountBase"] = aRInvoice.arpFreightAmountBase;
			dataRow["arpFreightAmountForeign"] = aRInvoice.arpFreightAmountForeign;
			dataRow["arpFreightGlAccountID"] = aRInvoice.arpFreightGlAccountID;
			dataRow["arpFreightSubtotalBase"] = aRInvoice.arpFreightSubtotalBase;
			dataRow["arpFreightSubtotalForeign"] = aRInvoice.arpFreightSubtotalForeign;
			dataRow["arpFreightTaxAmountBase"] = aRInvoice.arpFreightTaxAmountBase;
			dataRow["arpFreightTaxAmountForeign"] = aRInvoice.arpFreightTaxAmountForeign;
			dataRow["arpFreightTaxCodeID"] = aRInvoice.arpFreightTaxCodeID;
			dataRow["arpFreightTotalBase"] = aRInvoice.arpFreightTotalBase;
			dataRow["arpFreightTotalForeign"] = aRInvoice.arpFreightTotalForeign;
			dataRow["arpFullInvoiceSubtotalBase"] = aRInvoice.arpFullInvoiceSubtotalBase;
			dataRow["arpFullInvoiceSubtotalForeign"] = aRInvoice.arpFullInvoiceSubtotalForeign;
			dataRow["arpGlFiscalYearID"] = aRInvoice.arpGlFiscalYearID;
			dataRow["arpGlFiscalYearPeriodID"] = aRInvoice.arpGlFiscalYearPeriodID;
			DataRow dataRow6 = dataRow;
			arpCreditDate = aRInvoice.arpIntraCompanyPostedDate;
			dataRow6["arpIntraCompanyPostedDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpIntraCompanyPostedDate"]);
			dataRow["arpInvoiceBalanceBase"] = aRInvoice.arpInvoiceBalanceBase;
			dataRow["arpInvoiceBalanceForeign"] = aRInvoice.arpInvoiceBalanceForeign;
			dataRow["arpInvoiceCommentsRTF"] = aRInvoice.arpInvoiceCommentsRTF ?? dataRow["arpInvoiceCommentsRTF"];
			dataRow["arpInvoiceCommentsText"] = aRInvoice.arpInvoiceCommentsText ?? dataRow["arpInvoiceCommentsText"];
			DataRow dataRow7 = dataRow;
			arpCreditDate = aRInvoice.arpInvoiceDate;
			dataRow7["arpInvoiceDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpInvoiceDate"]);
			dataRow["arpInvoicePaidBase"] = aRInvoice.arpInvoicePaidBase;
			dataRow["arpInvoicePaidForeign"] = aRInvoice.arpInvoicePaidForeign;
			dataRow["arpInvoiceSubtotalBase"] = aRInvoice.arpInvoiceSubtotalBase;
			dataRow["arpInvoiceSubtotalForeign"] = aRInvoice.arpInvoiceSubtotalForeign;
			dataRow["arpInvoiceTaxAmountBase"] = aRInvoice.arpInvoiceTaxAmountBase;
			dataRow["arpInvoiceTaxAmountForeign"] = aRInvoice.arpInvoiceTaxAmountForeign;
			dataRow["arpInvoiceTotalBase"] = aRInvoice.arpInvoiceTotalBase;
			dataRow["arpInvoiceTotalForeign"] = aRInvoice.arpInvoiceTotalForeign;
			dataRow["arpInvoiceType"] = aRInvoice.arpInvoiceType;
			dataRow["arpAvalaraOverrideTax"] = aRInvoice.arpAvalaraOverrideTax;
			dataRow["arpAvalaraTaxCalculated"] = aRInvoice.arpAvalaraTaxCalculated;
			dataRow["arpCustomRate"] = aRInvoice.arpCustomRate;
			dataRow["arpDepositCredit"] = aRInvoice.arpDepositCredit;
			dataRow["arpEdiTransferred"] = aRInvoice.arpEdiTransferred;
			dataRow["arpIncludeFreightInPrice"] = aRInvoice.arpIncludeFreightInPrice;
			dataRow["arpIncludeTaxInRetention"] = aRInvoice.arpIncludeTaxInRetention;
			dataRow["arpIntraCompany"] = aRInvoice.arpIntraCompany;
			dataRow["arpIntraCompanyPosted"] = aRInvoice.arpIntraCompanyPosted;
			dataRow["arpOnHold"] = aRInvoice.arpOnHold;
			dataRow["arpOpenInvoiceLoad"] = aRInvoice.arpOpenInvoiceLoad;
			dataRow["arpOverpayment"] = aRInvoice.arpOverpayment;
			dataRow["arpPaidComplete"] = aRInvoice.arpPaidComplete;
			dataRow["arpPostedToGl"] = aRInvoice.arpPostedToGl;
			dataRow["arpReadyToPrint"] = aRInvoice.arpReadyToPrint;
			dataRow["arpRecurringInvoice"] = aRInvoice.arpRecurringInvoice;
			dataRow["arpRefundCheckRequired"] = aRInvoice.arpRefundCheckRequired;
			dataRow["arpLineCommissionTotal"] = aRInvoice.arpLineCommissionTotal;
			DataRow dataRow8 = dataRow;
			arpCreditDate = aRInvoice.arpOrderDate;
			dataRow8["arpOrderDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpOrderDate"]);
			dataRow["arpOriginalExchangeRate"] = aRInvoice.arpOriginalExchangeRate;
			dataRow["arpOverPaymentHeaderID"] = aRInvoice.arpOverPaymentHeaderID;
			dataRow["arpOverPaymentSessionID"] = aRInvoice.arpOverPaymentSessionID;
			DataRow dataRow9 = dataRow;
			arpCreditDate = aRInvoice.arpPaidDate;
			dataRow9["arpPaidDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpPaidDate"]);
			dataRow["arpPaymentTermID"] = aRInvoice.arpPaymentTermID;
			dataRow["arpPlantDepartmentID"] = aRInvoice.arpPlantDepartmentID;
			dataRow["arpPlantID"] = aRInvoice.arpPlantID;
			dataRow["arpPointOfSaleTerminalID"] = aRInvoice.arpPointOfSaleTerminalID;
			DataRow dataRow10 = dataRow;
			arpCreditDate = aRInvoice.arpPostedDate;
			dataRow10["arpPostedDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpPostedDate"]);
			dataRow["arpProjectID"] = aRInvoice.arpProjectID;
			dataRow["arpResellerCommissionAmount"] = aRInvoice.arpResellerCommissionAmount;
			dataRow["arpResellerCommissionRate"] = aRInvoice.arpResellerCommissionRate;
			dataRow["arpResellerContactID"] = aRInvoice.arpResellerContactID;
			dataRow["arpResellerLocationID"] = aRInvoice.arpResellerLocationID;
			dataRow["arpResellerOrganizationID"] = aRInvoice.arpResellerOrganizationID;
			dataRow["arpRetentionBalanceBase"] = aRInvoice.arpRetentionBalanceBase;
			dataRow["arpRetentionBalanceForeign"] = aRInvoice.arpRetentionBalanceForeign;
			dataRow["arpRetentionPaidBase"] = aRInvoice.arpRetentionPaidBase;
			dataRow["arpRetentionPaidForeign"] = aRInvoice.arpRetentionPaidForeign;
			dataRow["arpRetentionTotalBase"] = aRInvoice.arpRetentionTotalBase;
			dataRow["arpRetentionTotalForeign"] = aRInvoice.arpRetentionTotalForeign;
			dataRow["arpSalesCommissionTotal"] = aRInvoice.arpSalesCommissionTotal;
			dataRow["arpSalesGlAccountID"] = aRInvoice.arpSalesGlAccountID;
			dataRow["arpSecondFreightTaxAmtBase"] = aRInvoice.arpSecondFreightTaxAmtBase;
			dataRow["arpSecondFreightTaxAmtForeign"] = aRInvoice.arpSecondFreightTaxAmtForeign;
			dataRow["arpSecondFreightTaxCodeID"] = aRInvoice.arpSecondFreightTaxCodeID;
			dataRow["arpShipContactID"] = aRInvoice.arpShipContactID;
			dataRow["arpShipLocationID"] = aRInvoice.arpShipLocationID;
			dataRow["arpShipOrganizationID"] = aRInvoice.arpShipOrganizationID;
			dataRow["arpShippingMethodID"] = aRInvoice.arpShippingMethodID;
			dataRow["arpShippingPaymentTypeID"] = aRInvoice.arpShippingPaymentTypeID;
			dataRow["arpSplitPercentTotal"] = aRInvoice.arpSplitPercentTotal;
			dataRow["arpStandardMessageID"] = aRInvoice.arpStandardMessageID;
			DataRow dataRow11 = dataRow;
			arpCreditDate = aRInvoice.arpTaxDate;
			dataRow11["arpTaxDate"] = (arpCreditDate.HasValue ? ((object)arpCreditDate.GetValueOrDefault()) : dataRow["arpTaxDate"]);
			dataRow["arpTaxSubtotalBase"] = aRInvoice.arpTaxSubtotalBase;
			dataRow["arpTaxSubtotalForeign"] = aRInvoice.arpTaxSubtotalForeign;
			dataRow["arpTotalForResellerCommission"] = aRInvoice.arpTotalForResellerCommission;
			dataRow["arpTotalForSalesCommission"] = aRInvoice.arpTotalForSalesCommission;
			if (aRInvoice.CustomFields != null && aRInvoice.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aRInvoice.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ARInvoice [{aRInvoice.arpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ARInvoice [{aRInvoice.arpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
