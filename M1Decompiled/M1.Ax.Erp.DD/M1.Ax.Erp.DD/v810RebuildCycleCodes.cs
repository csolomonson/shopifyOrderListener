using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CycleCodes to support unicode", "2013-10-17")]
public class v810RebuildCycleCodes
{
	public v810RebuildCycleCodes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CycleCodes", new DmoField[5]
		{
			new DmoField("imdCycleCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imdDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imdCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imdCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("IMDCYCLECODEID", unique: true),
			new DmoIndex("IMDUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
