using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARRecurringInvoiceLines to support unicode", "2013-10-17")]
public class v810RebuildARRecurringInvoiceLines
{
	public v810RebuildARRecurringInvoiceLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoiceLines", new DmoField[47]
		{
			new DmoField("arqARRecurringInvoiceID", "int", 6, 0, nullable: false),
			new DmoField("arqARRecurringInvoiceLineID", "smallint", 4, 0, nullable: false),
			new DmoField("arqPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("arqOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("arqPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("arqUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("arqPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("arqOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("arqPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("arqPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("arqPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arqOrderQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("arqInvoiceQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("arqFullUnitPriceBase", "numeric", 15, 5, nullable: false),
			new DmoField("arqFullUnitPriceForeign", "numeric", 15, 5, nullable: false),
			new DmoField("arqDiscountPercent", "numeric", 6, 2, nullable: false),
			new DmoField("arqUnitDiscountBase", "numeric", 15, 5, nullable: false),
			new DmoField("arqUnitDiscountForeign", "numeric", 15, 5, nullable: false),
			new DmoField("arqUnitPriceBase", "numeric", 15, 5, nullable: false),
			new DmoField("arqUnitPriceForeign", "numeric", 15, 5, nullable: false),
			new DmoField("arqFullExtendedPriceBase", "money", 12, 2, nullable: false),
			new DmoField("arqFullExtendedPriceForeign", "money", 12, 2, nullable: false),
			new DmoField("arqExtendedDiscountBase", "money", 12, 2, nullable: false),
			new DmoField("arqExtendedDiscountForeign", "money", 12, 2, nullable: false),
			new DmoField("arqExtendedPriceBase", "money", 12, 2, nullable: false),
			new DmoField("arqExtendedPriceForeign", "money", 12, 2, nullable: false),
			new DmoField("arqFreightAmountBase", "money", 12, 2, nullable: false),
			new DmoField("arqFreightAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("arqTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arqNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arqTaxAmountBase", "money", 14, 4, nullable: false),
			new DmoField("arqTaxAmountForeign", "money", 14, 4, nullable: false),
			new DmoField("arqSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arqSecondTaxAmountBase", "money", 14, 4, nullable: false),
			new DmoField("arqSecondTaxAmountForeign", "money", 14, 4, nullable: false),
			new DmoField("arqPayCommission", "bit", 1, 0, nullable: false),
			new DmoField("arqCommissionRate", "numeric", 5, 2, nullable: false),
			new DmoField("arqCommissionAmount", "money", 12, 2, nullable: false),
			new DmoField("arqAmtForResellerCommission", "money", 12, 2, nullable: false),
			new DmoField("arqAmtForSalesCommission", "money", 12, 2, nullable: false),
			new DmoField("arqCustomerPO", "nvarchar", 40, 0, nullable: false),
			new DmoField("arqProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arqProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("arqInactive", "bit", 1, 0, nullable: false),
			new DmoField("arqCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("arqCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("arqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("ARQARRECURRINGINVOICEID,ARQARRECURRINGINVOICELINEID", unique: true),
			new DmoIndex("ARQUNIQUEID", unique: true),
			new DmoIndex("arqARRecurringInvoiceID", unique: false),
			new DmoIndex("arqARRecurringInvoiceLineID", unique: false),
			new DmoIndex("arqPartID", unique: false),
			new DmoIndex("arqOrgPartID", unique: false),
			new DmoIndex("arqPartRevisionID", unique: false),
			new DmoIndex("arqProjectID", unique: false),
			new DmoIndex("arqProjectAreaID", unique: false),
			new DmoIndex("arqInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
