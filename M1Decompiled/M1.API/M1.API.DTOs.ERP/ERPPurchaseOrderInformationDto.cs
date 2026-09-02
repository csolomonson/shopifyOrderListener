using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchaseOrderInformationDto
{
	public string pmpApInvoiceContactID { get; set; }

	public string pmpApInvoiceLocationID { get; set; }

	public DateTime? pmpApprovalDecisionDate { get; set; }

	public DateTime? pmpApprovalRequestDate { get; set; }

	public string pmpBuyerEmployeeID { get; set; }

	public DateTime? pmpClosedDate { get; set; }

	public string pmpPurchaseOrderID { get; set; }

	public string pmpCreatedBy { get; set; }

	public DateTime? pmpCreatedDate { get; set; }

	public string pmpCurrencyRateID { get; set; }

	public string pmpDocuments { get; set; }

	public string pmpDropShipContactID { get; set; }

	public string pmpDropShipLocationID { get; set; }

	public string pmpDropShipOrganizationID { get; set; }

	public DateTime? pmpDueDate { get; set; }

	public Guid pmpUniqueID { get; set; }

	public decimal pmpExchangeRate { get; set; }

	public string pmpFreeOnBoardDescription { get; set; }

	public DateTime? pmpIntraCompanyPostedDate { get; set; }

	public bool pmpClosed { get; set; }

	public bool pmpCustomRate { get; set; }

	public bool pmpIntraCompany { get; set; }

	public bool pmpIntraCompanyPosted { get; set; }

	public bool pmpReadyToPrint { get; set; }

	public string pmpNextApprovalEmployeeID { get; set; }

	public string pmpOrderCommentsRTF { get; set; }

	public string pmpOrderCommentsText { get; set; }

	public DateTime? pmpOrderDate { get; set; }

	public decimal pmpOrderSubtotalBase { get; set; }

	public decimal pmpOrderSubtotalForeign { get; set; }

	public decimal pmpOrderTaxAmountBase { get; set; }

	public decimal pmpOrderTaxAmountForeign { get; set; }

	public decimal pmpOrderTotalBase { get; set; }

	public decimal pmpOrderTotalForeign { get; set; }

	public string pmpPaymentTermID { get; set; }

	public string pmpPlantDepartmentID { get; set; }

	public string pmpPlantID { get; set; }

	public string pmpProjectID { get; set; }

	public string pmpPurchaseContactID { get; set; }

	public string pmpPurchaseLocationID { get; set; }

	public byte[] pmpRowVersion { get; set; }

	public string pmpShippingMethodID { get; set; }

	public string pmpStandardMessageID { get; set; }

	public byte pmpStatus { get; set; }

	public string pmpSupplierOrganizationID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
