using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAPInvoiceInformationDto
{
	public string appApGlAccountID { get; set; }

	public string appApInvoiceContactID { get; set; }

	public string appApInvoiceLocationID { get; set; }

	public string appApInvoiceID { get; set; }

	public string appCreatedBy { get; set; }

	public DateTime? appCreatedDate { get; set; }

	public string appCreditApInvoiceID { get; set; }

	public DateTime? appCreditDate { get; set; }

	public string appCreditReasonID { get; set; }

	public string appCurrencyRateID { get; set; }

	public decimal appDiscountAmountBase { get; set; }

	public decimal appDiscountAmountForeign { get; set; }

	public DateTime? appDiscountDueDate { get; set; }

	public DateTime? appDueDate { get; set; }

	public Guid appUniqueID { get; set; }

	public decimal appExchangeRate { get; set; }

	public decimal appFreightAmountBase { get; set; }

	public decimal appFreightAmountForeign { get; set; }

	public string appFreightGlAccountID { get; set; }

	public decimal appFreightTaxAmountBase { get; set; }

	public decimal appFreightTaxAmountForeign { get; set; }

	public string appFreightTaxCodeID { get; set; }

	public short appGlFiscalYearID { get; set; }

	public byte appGlFiscalYearPeriodID { get; set; }

	public decimal appInvoiceBalanceBase { get; set; }

	public decimal appInvoiceBalanceForeign { get; set; }

	public string appInvoiceCommentsRTF { get; set; }

	public string appInvoiceCommentsText { get; set; }

	public DateTime? appInvoiceDate { get; set; }

	public string appInvoiceDescription { get; set; }

	public decimal appInvoiceSubtotalBase { get; set; }

	public decimal appInvoiceSubtotalForeign { get; set; }

	public decimal appInvoiceTaxAmountBase { get; set; }

	public decimal appInvoiceTaxAmountForeign { get; set; }

	public decimal appInvoiceTotalBase { get; set; }

	public decimal appInvoiceTotalForeign { get; set; }

	public byte appInvoiceType { get; set; }

	public bool appCustomRate { get; set; }

	public bool appOnHold { get; set; }

	public bool appOpenInvoiceLoad { get; set; }

	public bool appOverpayment { get; set; }

	public bool appPaidComplete { get; set; }

	public bool appPostedToGl { get; set; }

	public bool appTaxReportable { get; set; }

	public decimal appOriginalExchangeRate { get; set; }

	public int appOverPaymentHeaderID { get; set; }

	public int appOverPaymentSessionID { get; set; }

	public DateTime? appPaidDate { get; set; }

	public string appPaymentTermID { get; set; }

	public string appPlantDepartmentID { get; set; }

	public string appPlantID { get; set; }

	public DateTime? appPostedDate { get; set; }

	public string appProjectID { get; set; }

	public decimal appRetentionBalanceBase { get; set; }

	public decimal appRetentionBalanceForeign { get; set; }

	public decimal appRetentionTotalBase { get; set; }

	public decimal appRetentionTotalForeign { get; set; }

	public byte[] appRowVersion { get; set; }

	public decimal appSecondFreightTaxAmtBase { get; set; }

	public decimal appSecondFreightTaxAmtForeign { get; set; }

	public string appSecondFreightTaxCodeID { get; set; }

	public string appSupplierInvoiceNumber { get; set; }

	public string appSupplierOrganizationID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
