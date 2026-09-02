using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert POSSessions to support unicode", "2013-10-17")]
public class v810RebuildPOSSessions
{
	public v810RebuildPOSSessions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "POSSessions", new DmoField[17]
		{
			new DmoField("pssPOSSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pssPointOfSaleTerminalID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pssSessionStartDate", "datetime", 14, 0, nullable: true),
			new DmoField("pssStartCash", "money", 12, 2, nullable: false),
			new DmoField("pssEndCash", "money", 12, 2, nullable: false),
			new DmoField("pssUserLoggedIn", "bit", 1, 0, nullable: false),
			new DmoField("pssLoginTime", "datetime", 14, 0, nullable: true),
			new DmoField("pssLogoffTime", "datetime", 14, 0, nullable: true),
			new DmoField("pssLastUserID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pssCurrCash", "money", 12, 2, nullable: false),
			new DmoField("pssSessionEnded", "bit", 1, 0, nullable: false),
			new DmoField("pssSessionEndDate", "datetime", 14, 0, nullable: true),
			new DmoField("pssPosted", "bit", 1, 0, nullable: false),
			new DmoField("pssPostedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pssCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pssCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pssUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("PSSPOSSESSIONID", unique: true),
			new DmoIndex("PSSUNIQUEID", unique: true),
			new DmoIndex("pssPointOfSaleTerminalID", unique: false),
			new DmoIndex("pssUserLoggedIn", unique: false),
			new DmoIndex("pssSessionEnded", unique: false),
			new DmoIndex("pssPosted", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
