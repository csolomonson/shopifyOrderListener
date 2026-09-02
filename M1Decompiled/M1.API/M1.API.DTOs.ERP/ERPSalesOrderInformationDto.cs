using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderInformationDto
{
	public DateTime? ompApprovalDecisionDate { get; set; }

	public DateTime? ompApprovalRequestDate { get; set; }

	public string ompArInvoiceContactID { get; set; }

	public string ompArInvoiceLocationID { get; set; }

	public string ompCallID { get; set; }

	public DateTime? ompClosedDate { get; set; }

	public string ompSalesOrderID { get; set; }

	public string ompCreatedBy { get; set; }

	public DateTime? ompCreatedDate { get; set; }

	public string ompCurrencyRateID { get; set; }

	public string ompCustomerOrganizationID { get; set; }

	public string ompCustomerPo { get; set; }

	public decimal ompDepositAmountBase { get; set; }

	public decimal ompDepositAmountForeign { get; set; }

	public decimal ompDepositPercent { get; set; }

	public decimal ompDiscountTotalBase { get; set; }

	public decimal ompDiscountTotalForeign { get; set; }

	public Guid ompUniqueID { get; set; }

	public decimal ompExchangeRate { get; set; }

	public string ompFedEx3rdPartyLocationID { get; set; }

	public string ompFedEx3rdPartyOrganizationID { get; set; }

	public string ompFedExAccountNumber { get; set; }

	public string ompFedExBillingOption { get; set; }

	public string ompFreeOnBoardDescription { get; set; }

	public decimal ompFreightAmountBase { get; set; }

	public decimal ompFreightAmountForeign { get; set; }

	public decimal ompFreightSubtotalBase { get; set; }

	public decimal ompFreightSubtotalForeign { get; set; }

	public decimal ompFreightTaxAmountBase { get; set; }

	public decimal ompFreightTaxAmountForeign { get; set; }

	public string ompFreightTaxCodeID { get; set; }

	public decimal ompFreightTotalBase { get; set; }

	public decimal ompFreightTotalForeign { get; set; }

	public decimal ompFullOrderSubtotalBase { get; set; }

	public decimal ompFullOrderSubtotalForeign { get; set; }

	public bool ompAvalaraTaxCalculated { get; set; }

	public bool ompClosed { get; set; }

	public bool ompCreatedByEdi { get; set; }

	public bool ompCustomRate { get; set; }

	public bool ompDeposit { get; set; }

	public bool ompDepositCreated { get; set; }

	public bool ompReadyToPrint { get; set; }

	public string ompNextApprovalEmployeeID { get; set; }

	public string ompOrderCommentsRTF { get; set; }

	public string ompOrderCommentsText { get; set; }

	public DateTime? ompOrderDate { get; set; }

	public decimal ompOrderSubtotalBase { get; set; }

	public decimal ompOrderSubTotalForeign { get; set; }

	public decimal ompOrderTaxAmountBase { get; set; }

	public decimal ompOrderTaxAmountForeign { get; set; }

	public decimal ompOrderTotalBase { get; set; }

	public decimal ompOrderTotalForeign { get; set; }

	public string ompPaymentTermID { get; set; }

	public string ompPlantDepartmentID { get; set; }

	public string ompPlantID { get; set; }

	public string ompProjectID { get; set; }

	public string ompQuoteContactID { get; set; }

	public string ompQuoteLocationID { get; set; }

	public DateTime? ompRequestedShipDate { get; set; }

	public string ompResellerContactID { get; set; }

	public string ompResellerLocationID { get; set; }

	public string ompResellerOrganizationID { get; set; }

	public byte[] ompRowVersion { get; set; }

	public decimal ompSecondFreightTaxAmtBase { get; set; }

	public decimal ompSecondFreightTaxAmtForeign { get; set; }

	public string ompSecondFreightTaxCodeID { get; set; }

	public string ompShipContactID { get; set; }

	public string ompShipLocationID { get; set; }

	public string ompShipOrganizationID { get; set; }

	public string ompShippingMethodID { get; set; }

	public string ompShippingPaymentTypeID { get; set; }

	public decimal ompSplitPercentTotal { get; set; }

	public string ompStandardMessageID { get; set; }

	public byte ompStatus { get; set; }

	public decimal ompTaxSubtotalBase { get; set; }

	public decimal ompTaxSubtotalForeign { get; set; }

	public decimal ompTotalOrderWeight { get; set; }

	public string ompUps3rdPartyLocationID { get; set; }

	public string ompUps3rdPartyOrganizationID { get; set; }

	public string ompUpsAccountNumber { get; set; }

	public string ompUpsBillingOption { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
