using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert JobScenarios to support unicode", "2013-10-17")]
public class v810RebuildJobScenarios
{
	public v810RebuildJobScenarios(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobScenarios", new DmoField[5]
		{
			new DmoField("jmnJobScenarioID", "nvarchar", 5, 0, nullable: false),
			new DmoField("jmnDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("jmnCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmnCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("jmnUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("JMNJOBSCENARIOID", unique: true),
			new DmoIndex("JMNUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
