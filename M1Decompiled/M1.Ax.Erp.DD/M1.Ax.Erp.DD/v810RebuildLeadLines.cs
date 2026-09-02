using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LeadLines to support unicode", "2013-10-17")]
public class v810RebuildLeadLines
{
	public v810RebuildLeadLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LeadLines", new DmoField[28]
		{
			new DmoField("lolLeadID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lolLeadLineID", "smallint", 4, 0, nullable: false),
			new DmoField("lolPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("lolOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("lolPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("lolOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lolDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lolUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("lolPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lolQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("lolGrossAmount", "money", 12, 2, nullable: false),
			new DmoField("lolRevenueForecast", "money", 12, 2, nullable: false),
			new DmoField("lolRevenueForecastForeign", "money", 12, 2, nullable: false),
			new DmoField("lolForecastDate", "date", 14, 0, nullable: true),
			new DmoField("lolTransferredToQuote", "bit", 1, 0, nullable: false),
			new DmoField("lolResolutionReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lolDiscountPercent", "numeric", 6, 2, nullable: false),
			new DmoField("lolGrossAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("lolDiscountAmount", "money", 12, 2, nullable: false),
			new DmoField("lolDiscountAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("lolLeadDate", "date", 14, 0, nullable: true),
			new DmoField("lolCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lolExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("lolCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("lolCreatedFromMobile", "bit", 1, 0, nullable: false),
			new DmoField("lolCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lolCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lolUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("LOLLEADID,LOLLEADLINEID", unique: true),
			new DmoIndex("LOLUNIQUEID", unique: true),
			new DmoIndex("lolLeadID", unique: false),
			new DmoIndex("lolLeadLineID", unique: false),
			new DmoIndex("lolPartID", unique: false),
			new DmoIndex("lolOrgPartID", unique: false),
			new DmoIndex("lolPartRevisionID", unique: false),
			new DmoIndex("lolPartGroupID", unique: false),
			new DmoIndex("lolResolutionReasonID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
