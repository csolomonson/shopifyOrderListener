using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CorrectiveActionCodes to support unicode", "2013-10-17")]
public class v810RebuildCorrectiveActionCodes
{
	public v810RebuildCorrectiveActionCodes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CorrectiveActionCodes", new DmoField[7]
		{
			new DmoField("qaoCorrectiveActionCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qaoDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qaoHoursAllowed", "numeric", 8, 2, nullable: false),
			new DmoField("qaoCorrectiveActionCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qaoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qaoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qaoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("QAOCORRECTIVEACTIONCODEID", unique: true),
			new DmoIndex("QAOUNIQUEID", unique: true),
			new DmoIndex("qaoCorrectiveActionCategoryID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
