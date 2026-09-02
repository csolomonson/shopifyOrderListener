using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProjectTypes to support unicode", "2013-10-17")]
public class v810RebuildProjectTypes
{
	public v810RebuildProjectTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProjectTypes", new DmoField[7]
		{
			new DmoField("prtProjectTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("prtDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("prtInactive", "bit", 1, 0, nullable: false),
			new DmoField("prtInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("prtCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("prtCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("prtUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("PRTPROJECTTYPEID", unique: true),
			new DmoIndex("PRTUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
