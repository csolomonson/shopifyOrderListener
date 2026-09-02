using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RMAActionTypes to support unicode", "2013-10-17")]
public class v810RebuildRMAActionTypes
{
	public v810RebuildRMAActionTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAActionTypes", new DmoField[3]
		{
			new DmoField("ratRMAActionTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ratDescription", "nvarchar", 30, 0, nullable: false),
			new DmoField("ratUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("RATRMAACTIONTYPEID", unique: true),
			new DmoIndex("RATUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
