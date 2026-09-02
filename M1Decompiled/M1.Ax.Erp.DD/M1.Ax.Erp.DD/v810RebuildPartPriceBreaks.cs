using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartPriceBreaks to support unicode", "2013-10-17")]
public class v810RebuildPartPriceBreaks
{
	public v810RebuildPartPriceBreaks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartPriceBreaks", new DmoField[9]
		{
			new DmoField("imjPartPriceID", "int", 9, 0, nullable: false),
			new DmoField("imjPartPriceBreakID", "smallint", 3, 0, nullable: false),
			new DmoField("imjUnitPrice", "numeric", 15, 5, nullable: false),
			new DmoField("imjQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("imjDiscount", "numeric", 6, 2, nullable: false),
			new DmoField("imjLeadTime", "smallint", 3, 0, nullable: false),
			new DmoField("imjCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imjCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imjUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("IMJPARTPRICEID,IMJPARTPRICEBREAKID", unique: true),
			new DmoIndex("IMJUNIQUEID", unique: true),
			new DmoIndex("imjPartPriceID", unique: false),
			new DmoIndex("imjPartPriceBreakID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
