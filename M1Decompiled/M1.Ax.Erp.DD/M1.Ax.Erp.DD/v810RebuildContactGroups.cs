using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ContactGroups to support unicode", "2013-10-17")]
public class v810RebuildContactGroups
{
	public v810RebuildContactGroups(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ContactGroups", new DmoField[5]
		{
			new DmoField("cmgContactGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmgDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmgCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmgCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmgUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CMGCONTACTGROUPID", unique: true),
			new DmoIndex("CMGUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
