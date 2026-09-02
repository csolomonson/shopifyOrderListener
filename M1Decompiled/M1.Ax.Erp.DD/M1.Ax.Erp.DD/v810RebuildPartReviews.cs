using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartReviews to support unicode", "2013-10-17")]
public class v810RebuildPartReviews
{
	public v810RebuildPartReviews(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartReviews", new DmoField[6]
		{
			new DmoField("wgrPartReviewID", "identity", 4, 0, nullable: false),
			new DmoField("wgrPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("wgrReviewerName", "nvarchar", 50, 0, nullable: false),
			new DmoField("wgrReviewerEmailAddress", "nvarchar", 50, 0, nullable: false),
			new DmoField("wgrRating", "int", 4, 0, nullable: false),
			new DmoField("wgrComments", "nvarchar(max)", 4, 0, nullable: true)
		}, new DmoIndex[2]
		{
			new DmoIndex("WGRPARTREVIEWID", unique: true),
			new DmoIndex("wgrPartID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
