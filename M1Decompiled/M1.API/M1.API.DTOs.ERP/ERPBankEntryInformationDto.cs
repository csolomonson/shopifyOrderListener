using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPBankEntryInformationDto
{
	public int gleApPaymentHeaderID { get; set; }

	public int gleApPaymentSessionID { get; set; }

	public int gleArPaymentHeaderID { get; set; }

	public int gleArPaymentSessionID { get; set; }

	public int gleBankStatementID { get; set; }

	public string gleCashGlAccountID { get; set; }

	public string gleCreatedBy { get; set; }

	public DateTime? gleCreatedDate { get; set; }

	public string gleCurrencyRateID { get; set; }

	public string gleDescription { get; set; }

	public string gleEftReferenceNumber { get; set; }

	public byte gleEntryType { get; set; }

	public Guid gleUniqueID { get; set; }

	public decimal gleExchangeRate { get; set; }

	public string gleGlAccountID { get; set; }

	public short gleGlFiscalYearID { get; set; }

	public byte gleGlFiscalYearPeriodID { get; set; }

	public int gleGlJournalID { get; set; }

	public int gleGlJournalLineID { get; set; }

	public bool gleCleared { get; set; }

	public bool gleCustomRate { get; set; }

	public bool gleDoNotUpdateGl { get; set; }

	public bool glePostedToGl { get; set; }

	public bool gleUnpresentedPayment { get; set; }

	public string gleNonTaxReasonID { get; set; }

	public string gleOrganizationID { get; set; }

	public decimal gleOriginalAmount { get; set; }

	public decimal gleOriginalAmountForeign { get; set; }

	public decimal glePaymentAmount { get; set; }

	public decimal glePaymentAmountForeign { get; set; }

	public DateTime? glePaymentDate { get; set; }

	public int glePaymentNumber { get; set; }

	public int glePayrollHeaderID { get; set; }

	public int glePayrollSessionID { get; set; }

	public byte glePayType { get; set; }

	public DateTime? glePresentedDate { get; set; }

	public byte[] gleRowVersion { get; set; }

	public int gleBankEntryID { get; set; }

	public byte gleSource { get; set; }

	public decimal gleTaxAmount { get; set; }

	public decimal gleTaxAmountForeign { get; set; }

	public string gleTaxCodeID { get; set; }

	public DateTime? gleTransactionDate { get; set; }

	public decimal gleVarianceAmount { get; set; }

	public decimal gleVarianceAmountForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
