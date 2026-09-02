using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPARInvoiceInformationDto
{
	public string arpArGlAccountID { get; set; }

	public string arpArInvoiceContactID { get; set; }

	public string arpArInvoiceLocationID { get; set; }

	public string arpArInvoiceID { get; set; }

	public decimal arpCommissionAmountBase { get; set; }

	public string arpCreatedBy { get; set; }

	public DateTime? arpCreatedDate { get; set; }

	public string arpCreditArInvoiceID { get; set; }

	public DateTime? arpCreditDate { get; set; }

	public string arpCreditReasonID { get; set; }

	public string arpCurrencyRateID { get; set; }

	public string arpCustomerOrganizationID { get; set; }

	public decimal arpDepositAppliedBase { get; set; }

	public decimal arpDepositAppliedForeign { get; set; }

	public decimal arpDepositBalanceBase { get; set; }

	public decimal arpDepositBalanceForeign { get; set; }

	public string arpDepositGlAccountID { get; set; }

	public decimal arpDepositTransferredBase { get; set; }

	public decimal arpDepositTransferredForeign { get; set; }

	public DateTime? arpDiscountDueDate { get; set; }

	public string arpDiscountGlAccountID { get; set; }

	public decimal arpDiscountTotalBase { get; set; }

	public decimal arpDiscountTotalForeign { get; set; }

	public DateTime? arpDueDate { get; set; }

	public DateTime? arpEdiTransferredDate { get; set; }

	public Guid arpUniqueID { get; set; }

	public decimal arpExchangeRate { get; set; }

	public string arpFreeOnBoardDescription { get; set; }

	public decimal arpFreightAmountBase { get; set; }

	public decimal arpFreightAmountForeign { get; set; }

	public string arpFreightGlAccountID { get; set; }

	public decimal arpFreightSubtotalBase { get; set; }

	public decimal arpFreightSubtotalForeign { get; set; }

	public decimal arpFreightTaxAmountBase { get; set; }

	public decimal arpFreightTaxAmountForeign { get; set; }

	public string arpFreightTaxCodeID { get; set; }

	public decimal arpFreightTotalBase { get; set; }

	public decimal arpFreightTotalForeign { get; set; }

	public decimal arpFullInvoiceSubtotalBase { get; set; }

	public decimal arpFullInvoiceSubtotalForeign { get; set; }

	public short arpGlFiscalYearID { get; set; }

	public byte arpGlFiscalYearPeriodID { get; set; }

	public DateTime? arpIntraCompanyPostedDate { get; set; }

	public decimal arpInvoiceBalanceBase { get; set; }

	public decimal arpInvoiceBalanceForeign { get; set; }

	public string arpInvoiceCommentsRTF { get; set; }

	public string arpInvoiceCommentsText { get; set; }

	public DateTime? arpInvoiceDate { get; set; }

	public decimal arpInvoicePaidBase { get; set; }

	public decimal arpInvoicePaidForeign { get; set; }

	public decimal arpInvoiceSubtotalBase { get; set; }

	public decimal arpInvoiceSubtotalForeign { get; set; }

	public decimal arpInvoiceTaxAmountBase { get; set; }

	public decimal arpInvoiceTaxAmountForeign { get; set; }

	public decimal arpInvoiceTotalBase { get; set; }

	public decimal arpInvoiceTotalForeign { get; set; }

	public byte arpInvoiceType { get; set; }

	public bool arpAvalaraOverrideTax { get; set; }

	public bool arpAvalaraTaxCalculated { get; set; }

	public bool arpCustomRate { get; set; }

	public bool arpDepositCredit { get; set; }

	public bool arpEdiTransferred { get; set; }

	public bool arpIncludeFreightInPrice { get; set; }

	public bool arpIncludeTaxInRetention { get; set; }

	public bool arpIntraCompany { get; set; }

	public bool arpIntraCompanyPosted { get; set; }

	public bool arpOnHold { get; set; }

	public bool arpOpenInvoiceLoad { get; set; }

	public bool arpOverpayment { get; set; }

	public bool arpPaidComplete { get; set; }

	public bool arpPostedToGl { get; set; }

	public bool arpReadyToPrint { get; set; }

	public bool arpRecurringInvoice { get; set; }

	public bool arpRefundCheckRequired { get; set; }

	public decimal arpLineCommissionTotal { get; set; }

	public DateTime? arpOrderDate { get; set; }

	public decimal arpOriginalExchangeRate { get; set; }

	public int arpOverPaymentHeaderID { get; set; }

	public int arpOverPaymentSessionID { get; set; }

	public DateTime? arpPaidDate { get; set; }

	public string arpPaymentTermID { get; set; }

	public string arpPlantDepartmentID { get; set; }

	public string arpPlantID { get; set; }

	public string arpPointOfSaleTerminalID { get; set; }

	public DateTime? arpPostedDate { get; set; }

	public string arpProjectID { get; set; }

	public decimal arpResellerCommissionAmount { get; set; }

	public decimal arpResellerCommissionRate { get; set; }

	public string arpResellerContactID { get; set; }

	public string arpResellerLocationID { get; set; }

	public string arpResellerOrganizationID { get; set; }

	public decimal arpRetentionBalanceBase { get; set; }

	public decimal arpRetentionBalanceForeign { get; set; }

	public decimal arpRetentionPaidBase { get; set; }

	public decimal arpRetentionPaidForeign { get; set; }

	public decimal arpRetentionTotalBase { get; set; }

	public decimal arpRetentionTotalForeign { get; set; }

	public byte[] arpRowVersion { get; set; }

	public decimal arpSalesCommissionTotal { get; set; }

	public string arpSalesGlAccountID { get; set; }

	public decimal arpSecondFreightTaxAmtBase { get; set; }

	public decimal arpSecondFreightTaxAmtForeign { get; set; }

	public string arpSecondFreightTaxCodeID { get; set; }

	public string arpShipContactID { get; set; }

	public string arpShipLocationID { get; set; }

	public string arpShipOrganizationID { get; set; }

	public string arpShippingMethodID { get; set; }

	public string arpShippingPaymentTypeID { get; set; }

	public decimal arpSplitPercentTotal { get; set; }

	public string arpStandardMessageID { get; set; }

	public DateTime? arpTaxDate { get; set; }

	public decimal arpTaxSubtotalBase { get; set; }

	public decimal arpTaxSubtotalForeign { get; set; }

	public decimal arpTotalForResellerCommission { get; set; }

	public decimal arpTotalForSalesCommission { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
