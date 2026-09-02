using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert QuoteQuantities to support unicode", "2013-10-17")]
public class v810RebuildQuoteQuantities
{
	public v810RebuildQuoteQuantities(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", new DmoField[50]
		{
			new DmoField("qmqQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmqQuoteLineID", "smallint", 4, 0, nullable: false),
			new DmoField("qmqQuoteQuantityID", "tinyint", 2, 0, nullable: false),
			new DmoField("qmqQuoteQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("qmqScrapPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqTotalRunQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("qmqSetupHours", "numeric", 10, 2, nullable: false),
			new DmoField("qmqProductionHours", "numeric", 10, 2, nullable: false),
			new DmoField("qmqMaterialCost", "numeric", 15, 5, nullable: false),
			new DmoField("qmqSubcontractCost", "numeric", 15, 5, nullable: false),
			new DmoField("qmqLaborCost", "numeric", 15, 5, nullable: false),
			new DmoField("qmqOverheadCost", "numeric", 15, 5, nullable: false),
			new DmoField("qmqQuotingCost", "numeric", 15, 5, nullable: false),
			new DmoField("qmqMaterialMarkupPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqDiscountPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqSubcontractMarkupPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqUnitDiscountBase", "numeric", 15, 5, nullable: false),
			new DmoField("qmqLaborMarkupPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqOverheadMarkupPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqUnitDiscountForeign", "numeric", 15, 5, nullable: false),
			new DmoField("qmqQuotingMarkupPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqAdditionalCostAmount", "numeric", 15, 5, nullable: false),
			new DmoField("qmqAdditionalCostDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qmqAdditionalMarkupPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqCommissionPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqFullRevisedUnitPriceBase", "numeric", 15, 5, nullable: false),
			new DmoField("qmqFullRevisedUnitPriceForeign", "numeric", 15, 5, nullable: false),
			new DmoField("qmqRevisedUnitPriceBase", "numeric", 15, 5, nullable: false),
			new DmoField("qmqRevisedUnitPriceForeign", "numeric", 15, 5, nullable: false),
			new DmoField("qmqAdditionalChargeBase", "money", 12, 2, nullable: false),
			new DmoField("qmqAdditionalChargeForeign", "money", 12, 2, nullable: false),
			new DmoField("qmqAdditionalChargeDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qmqLeadTime", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmqStartDate", "date", 14, 0, nullable: true),
			new DmoField("qmqDueDate", "date", 14, 0, nullable: true),
			new DmoField("qmqClosed", "bit", 1, 0, nullable: false),
			new DmoField("qmqUnitTaxAmountBase", "money", 14, 4, nullable: false),
			new DmoField("qmqUnitTaxAmountForeign", "money", 14, 4, nullable: false),
			new DmoField("qmqUnitSecondTaxAmountBase", "money", 14, 4, nullable: false),
			new DmoField("qmqUnitSecondTaxAmountForeign", "money", 14, 4, nullable: false),
			new DmoField("qmqAddTaxAmountBase", "money", 14, 4, nullable: false),
			new DmoField("qmqAddTaxAmountForeign", "money", 14, 4, nullable: false),
			new DmoField("qmqAddSecondTaxAmountBase", "money", 14, 4, nullable: false),
			new DmoField("qmqAddSecondTaxAmountForeign", "money", 14, 4, nullable: false),
			new DmoField("qmqPurchaseToOrderCost", "numeric", 15, 5, nullable: false),
			new DmoField("qmqPurToOrderMarkupPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmqCreatedFromMobile", "bit", 1, 0, nullable: false),
			new DmoField("qmqCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qmqCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qmqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("QMQQUOTEID,QMQQUOTELINEID,QMQQUOTEQUANTITYID", unique: true),
			new DmoIndex("QMQUNIQUEID", unique: true),
			new DmoIndex("qmqQuoteID", unique: false),
			new DmoIndex("qmqQuoteLineID", unique: false),
			new DmoIndex("qmqQuoteQuantityID", unique: false),
			new DmoIndex("qmqClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
