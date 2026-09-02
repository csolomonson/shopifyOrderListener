using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPQuoteQuantityInformationDto
{
	public decimal qmqAdditionalChargeBase { get; set; }

	public string qmqAdditionalChargeDescription { get; set; }

	public decimal qmqAdditionalChargeForeign { get; set; }

	public decimal qmqAdditionalCostAmount { get; set; }

	public string qmqAdditionalCostDescription { get; set; }

	public decimal qmqAdditionalCostPrice { get; set; }

	public decimal qmqAdditionalMarkupPercent { get; set; }

	public decimal qmqAddSecondTaxAmountBase { get; set; }

	public decimal qmqAddSecondTaxAmountForeign { get; set; }

	public decimal qmqAddTaxAmountBase { get; set; }

	public decimal qmqAddTaxAmountForeign { get; set; }

	public decimal qmqCalculatedUnitPrice { get; set; }

	public decimal qmqCommissionPercent { get; set; }

	public string qmqCreatedBy { get; set; }

	public DateTime? qmqCreatedDate { get; set; }

	public decimal qmqDiscountPercent { get; set; }

	public DateTime? qmqDueDate { get; set; }

	public Guid qmqUniqueID { get; set; }

	public decimal qmqFullRevisedUnitPriceBase { get; set; }

	public decimal qmqFullRevisedUnitPriceForeign { get; set; }

	public bool qmqClosed { get; set; }

	public bool qmqCreatedFromMobile { get; set; }

	public bool qmqPurchaseToOrder { get; set; }

	public decimal qmqLaborCost { get; set; }

	public decimal qmqLaborMarkupPercent { get; set; }

	public decimal qmqLaborPrice { get; set; }

	public string qmqLeadTime { get; set; }

	public decimal qmqMaterialCost { get; set; }

	public decimal qmqMaterialMarkupPercent { get; set; }

	public decimal qmqMaterialPrice { get; set; }

	public decimal qmqOverheadCost { get; set; }

	public decimal qmqOverheadMarkupPercent { get; set; }

	public decimal qmqOverheadPrice { get; set; }

	public decimal qmqProductionHours { get; set; }

	public decimal qmqPurchaseToOrderCost { get; set; }

	public decimal qmqPurchaseToOrderPrice { get; set; }

	public decimal qmqPurchaseUnitCostBase { get; set; }

	public decimal qmqPurToOrderMarkupPercent { get; set; }

	public string qmqQuoteID { get; set; }

	public short qmqQuoteLineID { get; set; }

	public byte qmqQuoteMarkupType { get; set; }

	public decimal qmqQuoteQuantity { get; set; }

	public decimal qmqQuotingCost { get; set; }

	public decimal qmqQuotingMarkupPercent { get; set; }

	public decimal qmqQuotingPrice { get; set; }

	public decimal qmqRevisedUnitPriceBase { get; set; }

	public decimal qmqRevisedUnitPriceForeign { get; set; }

	public byte[] qmqRowVersion { get; set; }

	public decimal qmqScrapPercent { get; set; }

	public string qmqSecondTaxCodeID { get; set; }

	public byte qmqQuoteQuantityID { get; set; }

	public decimal qmqSetupHours { get; set; }

	public DateTime? qmqStartDate { get; set; }

	public decimal qmqSubcontractCost { get; set; }

	public decimal qmqSubcontractMarkupPercent { get; set; }

	public decimal qmqSubcontractPrice { get; set; }

	public string qmqTaxCodeID { get; set; }

	public DateTime? qmqTaxDate { get; set; }

	public decimal qmqTotalCost { get; set; }

	public decimal qmqTotalMarkupPercent { get; set; }

	public decimal qmqTotalPrice { get; set; }

	public decimal qmqTotalRunQuantity { get; set; }

	public decimal qmqTotalUnitCost { get; set; }

	public decimal qmqTotalUnitPrice { get; set; }

	public decimal qmqUnitDiscountBase { get; set; }

	public decimal qmqUnitDiscountForeign { get; set; }

	public decimal qmqUnitSecondTaxAmountBase { get; set; }

	public decimal qmqUnitSecondTaxAmountForeign { get; set; }

	public decimal qmqUnitTaxAmountBase { get; set; }

	public decimal qmqUnitTaxAmountForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
