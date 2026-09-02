using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPARInvoiceLineInformationDto
{
	public decimal arlActualTotalCostOfGoodsSold { get; set; }

	public decimal arlActualTotalLaborCost { get; set; }

	public decimal arlActualTotalMaterialCost { get; set; }

	public decimal arlActualTotalOverheadCost { get; set; }

	public decimal arlActualTotalSubcontractCost { get; set; }

	public decimal arlActualUnitCostOfGoodsSold { get; set; }

	public decimal arlActualUnitLaborCost { get; set; }

	public decimal arlActualUnitMaterialCost { get; set; }

	public decimal arlActualUnitOverheadCost { get; set; }

	public decimal arlActualUnitSubcontractCost { get; set; }

	public decimal arlAmtForResellerCommission { get; set; }

	public decimal arlAmtForSalesCommission { get; set; }

	public string arlArInvoiceID { get; set; }

	public int arlArRecurringInvoiceID { get; set; }

	public short arlArRecurringInvoiceLineID { get; set; }

	public int arlAssetAdjustmentID { get; set; }

	public string arlAssetID { get; set; }

	public string arlCallID { get; set; }

	public DateTime? arlCogsCalculatedDate { get; set; }

	public decimal arlCommissionAmount { get; set; }

	public decimal arlCommissionRate { get; set; }

	public string arlCreatedBy { get; set; }

	public DateTime? arlCreatedDate { get; set; }

	public string arlCustomerPo { get; set; }

	public decimal arlDepositAmountBase { get; set; }

	public decimal arlDepositAmountForeign { get; set; }

	public decimal arlDepositBalanceBase { get; set; }

	public decimal arlDepositBalanceForeign { get; set; }

	public string arlDepositInvoiceID { get; set; }

	public short arlDepositInvoiceLineID { get; set; }

	public decimal arlDepositTransferredBase { get; set; }

	public decimal arlDepositTransferredForeign { get; set; }

	public decimal arlDiscountPercent { get; set; }

	public Guid arlUniqueID { get; set; }

	public decimal arlEstTotalCostOfGoodsSold { get; set; }

	public decimal arlEstTotalLaborCost { get; set; }

	public decimal arlEstTotalMaterialCost { get; set; }

	public decimal arlEstTotalOverheadCost { get; set; }

	public decimal arlEstTotalSubcontractCost { get; set; }

	public decimal arlEstUnitCostOfGoodsSold { get; set; }

	public decimal arlEstUnitLaborCost { get; set; }

	public decimal arlEstUnitMaterialCost { get; set; }

	public decimal arlEstUnitOverheadCost { get; set; }

	public decimal arlEstUnitSubcontractCost { get; set; }

	public decimal arlExtendedDiscountBase { get; set; }

	public decimal arlExtendedDiscountForeign { get; set; }

	public decimal arlExtendedPriceBase { get; set; }

	public decimal arlExtendedPriceForeign { get; set; }

	public string arlFinanceSourceInvoiceID { get; set; }

	public decimal arlFreightAmountBase { get; set; }

	public decimal arlFreightAmountForeign { get; set; }

	public decimal arlFullExtendedPriceBase { get; set; }

	public decimal arlFullExtendedPriceForeign { get; set; }

	public decimal arlFullUnitPriceBase { get; set; }

	public decimal arlFullUnitPriceForeign { get; set; }

	public decimal arlInvoiceQuantity { get; set; }

	public bool arlAvalaraIgnoreLine { get; set; }

	public bool arlCogsPostedToGl { get; set; }

	public bool arlDeliveryInvoicedComplete { get; set; }

	public bool arlDepositLine { get; set; }

	public bool arlIncludeTaxInRetention { get; set; }

	public bool arlIntraCompanyPosted { get; set; }

	public bool arlPayCommission { get; set; }

	public bool arlPostedToGl { get; set; }

	public bool arlRetention { get; set; }

	public int arlJobAssemblyID { get; set; }

	public string arlJobID { get; set; }

	public int arlJobMaterialID { get; set; }

	public byte arlLineType { get; set; }

	public string arlNonTaxReasonID { get; set; }

	public decimal arlOrderQuantity { get; set; }

	public string arlOrgPartID { get; set; }

	public string arlOrgPartShortDescription { get; set; }

	public string arlPartGroupID { get; set; }

	public string arlPartID { get; set; }

	public string arlPartLongDescriptionRtf { get; set; }

	public string arlPartLongDescriptionText { get; set; }

	public string arlPartRevisionID { get; set; }

	public string arlPartShortDescription { get; set; }

	public string arlProjectAreaID { get; set; }

	public string arlProjectID { get; set; }

	public decimal arlRetentionAmountBase { get; set; }

	public decimal arlRetentionAmountForeign { get; set; }

	public DateTime? arlRetentionDueDate { get; set; }

	public decimal arlRetentionPercent { get; set; }

	public string arlRmaClaimID { get; set; }

	public short arlRmaClaimLineID { get; set; }

	public string arlRmaReceiptID { get; set; }

	public short arlRmaReceiptLineID { get; set; }

	public byte[] arlRowVersion { get; set; }

	public short arlSalesOrderDeliveryID { get; set; }

	public string arlSalesOrderID { get; set; }

	public short arlSalesOrderLineID { get; set; }

	public decimal arlSecondTaxAmountBase { get; set; }

	public decimal arlSecondTaxAmountForeign { get; set; }

	public string arlSecondTaxCodeID { get; set; }

	public short arlArInvoiceLineID { get; set; }

	public string arlShipmentID { get; set; }

	public short arlShipmentLineID { get; set; }

	public decimal arlTaxAmountBase { get; set; }

	public decimal arlTaxAmountForeign { get; set; }

	public string arlTaxCodeID { get; set; }

	public DateTime? arlTaxDate { get; set; }

	public decimal arlUnitDiscountBase { get; set; }

	public decimal arlUnitDiscountForeign { get; set; }

	public string arlUnitOfMeasure { get; set; }

	public decimal arlUnitPriceBase { get; set; }

	public decimal arlUnitPriceForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
