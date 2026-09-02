using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Milestones to support unicode", "2013-10-17")]
public class v810RebuildMilestones
{
	public v810RebuildMilestones(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Milestones", new DmoField[8]
		{
			new DmoField("losMilestoneID", "nvarchar", 5, 0, nullable: false),
			new DmoField("losShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("losLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("losLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("losConfidenceFactor", "numeric", 4, 2, nullable: false),
			new DmoField("losCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("losCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("losUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("LOSMILESTONEID", unique: true),
			new DmoIndex("LOSUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
