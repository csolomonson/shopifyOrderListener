using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderLines to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderLines
{
	public v810RebuildSalesOrderLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderLines", new DmoField[64]
		{
			new DmoField("omlSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omlSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("omlOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("omlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omlUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("omlPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omlPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("omlOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("omlPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("omlPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("omlOrderQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("omlFullUnitPriceBase", "numeric", 15, 5, nullable: false),
			new DmoField("omlFullUnitPriceForeign", "numeric", 15, 5, nullable: false),
			new DmoField("omlDiscountPercent", "numeric", 6, 2, nullable: false),
			new DmoField("omlUnitDiscountBase", "numeric", 15, 5, nullable: false),
			new DmoField("omlUnitDiscountForeign", "numeric", 15, 5, nullable: false),
			new DmoField("omlUnitPriceBase", "numeric", 15, 5, nullable: false),
			new DmoField("omlUnitPriceForeign", "numeric", 15, 5, nullable: false),
			new DmoField("omlFullExtendedPriceBase", "money", 12, 2, nullable: false),
			new DmoField("omlFullExtendedPriceForeign", "money", 12, 2, nullable: false),
			new DmoField("omlExtendedDiscountBase", "money", 12, 2, nullable: false),
			new DmoField("omlExtendedDiscountForeign", "money", 12, 2, nullable: false),
			new DmoField("omlExtendedPriceBase", "money", 12, 2, nullable: false),
			new DmoField("omlExtendedPriceForeign", "money", 12, 2, nullable: false),
			new DmoField("omlFreightAmountBase", "money", 12, 2, nullable: false),
			new DmoField("omlFreightAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("omlTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omlNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omlTaxAmountBase", "money", 14, 4, nullable: false),
			new DmoField("omlTaxAmountForeign", "money", 14, 4, nullable: false),
			new DmoField("omlSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omlSecondTaxAmountBase", "money", 14, 4, nullable: false),
			new DmoField("omlSecondTaxAmountForeign", "money", 14, 4, nullable: false),
			new DmoField("omlPayCommission", "bit", 1, 0, nullable: false),
			new DmoField("omlTimeAndMaterial", "bit", 1, 0, nullable: false),
			new DmoField("omlQuantityShipped", "numeric", 15, 5, nullable: false),
			new DmoField("omlQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omlQuoteLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omlQuoteQuantityID", "tinyint", 2, 0, nullable: false),
			new DmoField("omlLeadID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omlLeadLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omlRMAClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omlRMAClaimLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omlConfigured", "bit", 1, 0, nullable: false),
			new DmoField("omlProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omlProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omlWeight", "numeric", 15, 5, nullable: false),
			new DmoField("omlPOSSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omlPOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omlPOSTransactionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omlClosed", "bit", 1, 0, nullable: false),
			new DmoField("omlPriceOverride", "bit", 1, 0, nullable: false),
			new DmoField("omlDeposit", "bit", 1, 0, nullable: false),
			new DmoField("omlDepositPercent", "numeric", 6, 2, nullable: false),
			new DmoField("omlDepositAmountBase", "money", 12, 2, nullable: false),
			new DmoField("omlDepositAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("omlDepositCreated", "bit", 1, 0, nullable: false),
			new DmoField("omlDepositCredited", "bit", 1, 0, nullable: false),
			new DmoField("omlDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("omlAvalaraIgnoreLine", "bit", 1, 0, nullable: false),
			new DmoField("omlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("omlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[14]
		{
			new DmoIndex("OMLSALESORDERID,OMLSALESORDERLINEID", unique: true),
			new DmoIndex("OMLUNIQUEID", unique: true),
			new DmoIndex("omlSalesOrderID", unique: false),
			new DmoIndex("omlSalesOrderLineID", unique: false),
			new DmoIndex("omlPartID", unique: false),
			new DmoIndex("omlOrgPartID", unique: false),
			new DmoIndex("omlPartRevisionID", unique: false),
			new DmoIndex("omlRMAClaimID", unique: false),
			new DmoIndex("omlRMAClaimLineID", unique: false),
			new DmoIndex("omlProjectID", unique: false),
			new DmoIndex("omlProjectAreaID", unique: false),
			new DmoIndex("omlPOSSessionID", unique: false),
			new DmoIndex("omlPOSTransactionID", unique: false),
			new DmoIndex("omlClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
