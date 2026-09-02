using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ContactTitles to support unicode", "2013-10-17")]
public class v810RebuildContactTitles
{
	public v810RebuildContactTitles(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ContactTitles", new DmoField[5]
		{
			new DmoField("cmeContactTitleID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmeDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmeCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmeCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmeUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CMECONTACTTITLEID", unique: true),
			new DmoIndex("CMEUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
