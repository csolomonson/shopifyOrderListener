using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPARPaymentLineInformationDto
{
	public decimal arnAdjustmentAmount { get; set; }

	public decimal arnAdjustmentAmountForeign { get; set; }

	public string arnAdjustmentGlAccountID { get; set; }

	public string arnApInvoiceID { get; set; }

	public string arnArInvoiceID { get; set; }

	public int arnArPaymentEPayID { get; set; }

	public int arnArPaymentHeaderID { get; set; }

	public int arnArPaymentSessionID { get; set; }

	public string arnCreatedBy { get; set; }

	public DateTime? arnCreatedDate { get; set; }

	public decimal arnDiscountAmount { get; set; }

	public decimal arnDiscountAmountForeign { get; set; }

	public string arnDiscountGlAccountID { get; set; }

	public decimal arnDiscountTaxAmount { get; set; }

	public decimal arnDiscountTaxAmountForeign { get; set; }

	public string arnDiscountTaxCodeID { get; set; }

	public Guid arnUniqueID { get; set; }

	public decimal arnExchangeAmount { get; set; }

	public string arnExchangeGlAccountID { get; set; }

	public bool arnAvalaraTaxCalculated { get; set; }

	public bool arnOverpayment { get; set; }

	public bool arnPostedToGl { get; set; }

	public string arnNonTaxReasonID { get; set; }

	public decimal arnOriginalInvBalanceForeign { get; set; }

	public decimal arnOriginalInvoiceBalance { get; set; }

	public decimal arnPaymentAmount { get; set; }

	public decimal arnPaymentAmountForeign { get; set; }

	public decimal arnRetentionPayAmtForeign { get; set; }

	public decimal arnRetentionPaymentAmount { get; set; }

	public byte[] arnRowVersion { get; set; }

	public decimal arnSecondDiscountTaxAmount { get; set; }

	public string arnSecondDiscountTaxCodeID { get; set; }

	public decimal arnSecondDisTaxAmtForeign { get; set; }

	public decimal arnSecondTaxAmount { get; set; }

	public decimal arnSecondTaxAmountForeign { get; set; }

	public string arnSecondTaxCodeID { get; set; }

	public short arnArPaymentLineID { get; set; }

	public decimal arnTaxAmount { get; set; }

	public decimal arnTaxAmountForeign { get; set; }

	public string arnTaxCodeID { get; set; }

	public decimal arnTotalDiscountAmount { get; set; }

	public decimal arnTotalDiscountAmtForeign { get; set; }

	public decimal arnUnrealisedExchangeAmt { get; set; }

	public string arnUnrealisedExGlAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
