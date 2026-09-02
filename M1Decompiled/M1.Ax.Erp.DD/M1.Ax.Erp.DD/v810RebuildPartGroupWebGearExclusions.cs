using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartGroupWebGearExclusions to support unicode", "2013-10-17")]
public class v810RebuildPartGroupWebGearExclusions
{
	public v810RebuildPartGroupWebGearExclusions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartGroupWebGearExclusions", new DmoField[3]
		{
			new DmoField("wgePartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wgeWebGearTemplate", "nvarchar", 20, 0, nullable: false),
			new DmoField("wgeUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("WGEPARTGROUPID,WGEWEBGEARTEMPLATE", unique: true),
			new DmoIndex("WGEUNIQUEID", unique: true),
			new DmoIndex("wgePartGroupID", unique: false),
			new DmoIndex("wgeWebGearTemplate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
