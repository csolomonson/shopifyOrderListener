using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert MarketingPrograms to support unicode", "2013-10-17")]
public class v810RebuildMarketingPrograms
{
	public v810RebuildMarketingPrograms(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MarketingPrograms", new DmoField[14]
		{
			new DmoField("looMarketingProgramID", "nvarchar", 5, 0, nullable: false),
			new DmoField("looShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("looActivityType", "nvarchar", 5, 0, nullable: false),
			new DmoField("looStartDate", "date", 14, 0, nullable: true),
			new DmoField("looEndDate", "date", 14, 0, nullable: true),
			new DmoField("looLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("looLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("looMarketingCost", "money", 12, 2, nullable: false),
			new DmoField("looExpectedRevenue", "money", 12, 2, nullable: false),
			new DmoField("looInactive", "bit", 1, 0, nullable: false),
			new DmoField("looInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("looCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("looCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("looUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("LOOMARKETINGPROGRAMID", unique: true),
			new DmoIndex("LOOUNIQUEID", unique: true),
			new DmoIndex("looInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
