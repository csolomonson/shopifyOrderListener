using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CallChangeRequestLinks to support unicode", "2013-10-17")]
public class v810RebuildCallChangeRequestLinks
{
	public v810RebuildCallChangeRequestLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CallChangeRequestLinks", new DmoField[6]
		{
			new DmoField("kbiCallID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbiCallChangeRequestLinkID", "smallint", 4, 0, nullable: false),
			new DmoField("kbiChangeRequestID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbiCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbiCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbiUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("KBICALLID,KBICALLCHANGEREQUESTLINKID", unique: true),
			new DmoIndex("KBIUNIQUEID", unique: true),
			new DmoIndex("kbiCallID", unique: false),
			new DmoIndex("kbiCallChangeRequestLinkID", unique: false),
			new DmoIndex("kbiChangeRequestID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
