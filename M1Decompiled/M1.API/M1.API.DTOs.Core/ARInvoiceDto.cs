using System;
using System.Collections.Generic;

namespace M1.API.DTOs.Core;

public class ARInvoiceDto
{
	public string ARInvoiceID { get; set; }

	public DateTime InvoiceDate { get; set; }

	public string CustomerOrganizationID { get; set; }

	public string ShipOrganizationID { get; set; }

	public string ShipLocationID { get; set; }

	public string ShippingMethodID { get; set; }

	public string CurrencyRateID { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal InvoiceSubtotalBase { get; set; }

	public decimal FreightAmountBase { get; set; }

	public string FreightTaxCodeID { get; set; }

	public decimal FreightTaxAmountBase { get; set; }

	public decimal InvoiceTaxAmountBase { get; set; }

	public decimal InvoiceTotalBase { get; set; }

	public decimal InvoiceBalanceBase { get; set; }

	public decimal InvoiceTotalForeign { get; set; }

	public decimal InvoiceBalanceForeign { get; set; }

	public decimal FreightAmountForeign { get; set; }

	public decimal InvoiceTaxAmountForeign { get; set; }

	public decimal InvoiceSubtotalForeign { get; set; }

	public decimal FreightTaxAmountForeign { get; set; }

	public decimal FreightSubtotalBase { get; set; }

	public decimal FreightSubtotalForeign { get; set; }

	public decimal FreightTotalBase { get; set; }

	public decimal FreightTotalForeign { get; set; }

	public decimal FullInvoiceSubtotalBase { get; set; }

	public decimal FullInvoiceSubtotalForeign { get; set; }

	public decimal DiscountTotalBase { get; set; }

	public decimal DiscountTotalForeign { get; set; }

	public string PaymentTermID { get; set; }

	public string ShippingPaymentTypeID { get; set; }

	public string ARInvoiceLocationID { get; set; }

	public string ShipContactID { get; set; }

	public bool EDITransferred { get; set; }

	public DateTime EDITransferredDate { get; set; }

	public bool PostedToGL { get; set; }

	public decimal InvoiceType { get; set; }

	public DateTime DueDate { get; set; }

	public string FreeOnBoardDescription { get; set; }

	public List<ARInvoiceLineDto> ARInvoiceLines { get; } = new List<ARInvoiceLineDto>();
}
