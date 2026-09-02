using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProjectAreas to support unicode", "2013-10-17")]
public class v810RebuildProjectAreas
{
	public v810RebuildProjectAreas(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProjectAreas", new DmoField[6]
		{
			new DmoField("praProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("praProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("praDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("praCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("praCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("praUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("PRAPROJECTID,PRAPROJECTAREAID", unique: true),
			new DmoIndex("PRAUNIQUEID", unique: true),
			new DmoIndex("praProjectID", unique: false),
			new DmoIndex("praProjectAreaID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
