using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ChangeRequestGroupLinks to support unicode", "2013-10-17")]
public class v810RebuildChangeRequestGroupLinks
{
	public v810RebuildChangeRequestGroupLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ChangeRequestGroupLinks", new DmoField[6]
		{
			new DmoField("chrChangeRequestID", "nvarchar", 10, 0, nullable: false),
			new DmoField("chrChangeRequestGroupLinkID", "smallint", 4, 0, nullable: false),
			new DmoField("chrChangeRequestGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("chrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("chrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("chrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("CHRCHANGEREQUESTID,CHRCHANGEREQUESTGROUPLINKID", unique: true),
			new DmoIndex("CHRUNIQUEID", unique: true),
			new DmoIndex("chrChangeRequestID", unique: false),
			new DmoIndex("chrChangeRequestGroupLinkID", unique: false),
			new DmoIndex("chrChangeRequestGroupID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
