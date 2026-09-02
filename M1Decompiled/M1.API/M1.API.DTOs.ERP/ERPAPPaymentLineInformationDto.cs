using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAPPaymentLineInformationDto
{
	public decimal apnAdjustmentAmount { get; set; }

	public decimal apnAdjustmentAmountForeign { get; set; }

	public string apnAdjustmentGlAccountID { get; set; }

	public string apnApInvoiceID { get; set; }

	public int apnApPaymentHeaderID { get; set; }

	public int apnApPaymentSessionID { get; set; }

	public string apnArInvoiceID { get; set; }

	public string apnBankAccountID { get; set; }

	public string apnCreatedBy { get; set; }

	public DateTime? apnCreatedDate { get; set; }

	public string apnCurrencyRateID { get; set; }

	public string apnDescription { get; set; }

	public decimal apnDiscountAmount { get; set; }

	public decimal apnDiscountAmountForeign { get; set; }

	public string apnDiscountGlAccountID { get; set; }

	public decimal apnDiscountTaxAmount { get; set; }

	public decimal apnDiscountTaxAmountForeign { get; set; }

	public string apnDiscountTaxCodeID { get; set; }

	public Guid apnUniqueID { get; set; }

	public decimal apnExchangeAmount { get; set; }

	public string apnExchangeGlAccountID { get; set; }

	public decimal apnExchangeRate { get; set; }

	public string apnExpenseGlAccountID { get; set; }

	public bool apnCompleted { get; set; }

	public bool apnCustomRate { get; set; }

	public bool apnOverpayment { get; set; }

	public bool apnPostedToGl { get; set; }

	public string apnNonTaxReasonID { get; set; }

	public decimal apnOriginalInvBalanceForeign { get; set; }

	public decimal apnOriginalInvoiceBalance { get; set; }

	public decimal apnPaymentAmount { get; set; }

	public decimal apnPaymentAmountForeign { get; set; }

	public decimal apnRetentionPayAmtForeign { get; set; }

	public decimal apnRetentionPaymentAmount { get; set; }

	public byte[] apnRowVersion { get; set; }

	public decimal apnSecondDiscountTaxAmount { get; set; }

	public string apnSecondDiscountTaxCodeID { get; set; }

	public decimal apnSecondDisTaxAmtForeign { get; set; }

	public decimal apnSecondTaxAmount { get; set; }

	public decimal apnSecondTaxAmountForeign { get; set; }

	public string apnSecondTaxCodeID { get; set; }

	public short apnApPaymentLineID { get; set; }

	public decimal apnTaxAmount { get; set; }

	public decimal apnTaxAmountForeign { get; set; }

	public string apnTaxCodeID { get; set; }

	public decimal apnTotalDiscountAmount { get; set; }

	public decimal apnTotalDiscountAmtForeign { get; set; }

	public decimal apnUnrealisedExchangeAmt { get; set; }

	public string apnUnrealisedExGlAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
