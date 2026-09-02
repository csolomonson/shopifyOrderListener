using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ChangeRequestGroups to support unicode", "2013-10-17")]
public class v810RebuildChangeRequestGroups
{
	public v810RebuildChangeRequestGroups(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ChangeRequestGroups", new DmoField[7]
		{
			new DmoField("chgChangeRequestGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("chgDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("chgInactive", "bit", 1, 0, nullable: false),
			new DmoField("chgInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("chgCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("chgCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("chgUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CHGCHANGEREQUESTGROUPID", unique: true),
			new DmoIndex("CHGUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
