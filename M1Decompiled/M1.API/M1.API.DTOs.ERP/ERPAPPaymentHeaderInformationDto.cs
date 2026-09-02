using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAPPaymentHeaderInformationDto
{
	public string aptApInvoiceContactID { get; set; }

	public string aptApInvoiceLocationID { get; set; }

	public int aptApPaymentSessionID { get; set; }

	public string aptBankAccountName { get; set; }

	public string aptBankAccountNumber { get; set; }

	public string aptBankAccountType { get; set; }

	public string aptBankInitials { get; set; }

	public string aptBic { get; set; }

	public string aptBsbNumber { get; set; }

	public string aptCashGlAccountID { get; set; }

	public string aptCreatedBy { get; set; }

	public string aptCreatedCreditApInvoiceID { get; set; }

	public DateTime? aptCreatedDate { get; set; }

	public string aptCreditApInvoiceID { get; set; }

	public string aptEftCode { get; set; }

	public string aptEftDescription { get; set; }

	public int aptEftNumber { get; set; }

	public string aptEftParticulars { get; set; }

	public Guid aptUniqueID { get; set; }

	public decimal aptExchangeAmount { get; set; }

	public string aptExchangeGlAccountID { get; set; }

	public byte aptForm1099Box { get; set; }

	public short aptGlFiscalYearID { get; set; }

	public byte aptGlFiscalYearPeriodID { get; set; }

	public string aptIban { get; set; }

	public bool aptCompleted { get; set; }

	public bool aptManualPayment { get; set; }

	public bool aptOpenPaymentLoad { get; set; }

	public bool aptOverpayment { get; set; }

	public bool aptPostedToGl { get; set; }

	public bool aptSuppressVoid { get; set; }

	public bool aptTaxReportable { get; set; }

	public bool aptVoidedPayment { get; set; }

	public string aptLongDescriptionRtf { get; set; }

	public string aptLongDescriptionText { get; set; }

	public decimal aptPaymentAmount { get; set; }

	public decimal aptPaymentAmountForeign { get; set; }

	public DateTime? aptPaymentDate { get; set; }

	public string aptPaymentMemo { get; set; }

	public int aptPaymentNumber { get; set; }

	public byte aptPaymentType { get; set; }

	public int aptRecurringPaymentID { get; set; }

	public byte[] aptRowVersion { get; set; }

	public int aptApPaymentHeaderID { get; set; }

	public bool aptShowAllInvoices { get; set; }

	public string aptSupplierOrganizationID { get; set; }

	public int aptVoidApPaymentHeaderID { get; set; }

	public int aptVoidApPaymentSessionID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
