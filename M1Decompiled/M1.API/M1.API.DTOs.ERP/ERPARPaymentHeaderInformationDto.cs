using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPARPaymentHeaderInformationDto
{
	public string artArGlAccountID { get; set; }

	public string artArInvoiceContactID { get; set; }

	public string artArInvoiceLocationID { get; set; }

	public int artArPaymentSessionID { get; set; }

	public string artBankAccountName { get; set; }

	public string artBankAccountNumber { get; set; }

	public string artBankInitials { get; set; }

	public string artBsbNumber { get; set; }

	public string artCashGlAccountID { get; set; }

	public string artCreatedBy { get; set; }

	public string artCreatedCreditArInvoiceID { get; set; }

	public DateTime? artCreatedDate { get; set; }

	public string artCreditArInvoiceID { get; set; }

	public string artCustomerOrganizationID { get; set; }

	public string artCustomerPaymentNumber { get; set; }

	public string artDescription { get; set; }

	public Guid artUniqueID { get; set; }

	public decimal artExchangeAmount { get; set; }

	public string artExchangeGlAccountID { get; set; }

	public string artGlAccountID { get; set; }

	public short artGlFiscalYearID { get; set; }

	public byte artGlFiscalYearPeriodID { get; set; }

	public bool artAvalaraTaxCalculated { get; set; }

	public bool artNet1PaymentProcessed { get; set; }

	public bool artOpenPaymentLoad { get; set; }

	public bool artPostedToGl { get; set; }

	public bool artVoidedPayment { get; set; }

	public string artLongDescriptionRtf { get; set; }

	public string artLongDescriptionText { get; set; }

	public string artNonTaxReasonID { get; set; }

	public byte artPaymentMethod { get; set; }

	public decimal artReceiptAmount { get; set; }

	public decimal artReceiptAmountForeign { get; set; }

	public DateTime? artReceiptDate { get; set; }

	public byte artReceiptType { get; set; }

	public byte[] artRowVersion { get; set; }

	public decimal artSecondTaxAmount { get; set; }

	public decimal artSecondTaxAmountForeign { get; set; }

	public string artSecondTaxCodeID { get; set; }

	public int artArPaymentHeaderID { get; set; }

	public bool artShowAllInvoices { get; set; }

	public decimal artTaxAmount { get; set; }

	public decimal artTaxAmountForeign { get; set; }

	public string artTaxCodeID { get; set; }

	public int artVoidArPaymentHeaderId { get; set; }

	public int artVoidArPaymentSessionID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
