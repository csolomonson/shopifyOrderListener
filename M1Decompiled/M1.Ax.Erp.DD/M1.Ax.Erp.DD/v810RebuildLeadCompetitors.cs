using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LeadCompetitors to support unicode", "2013-10-17")]
public class v810RebuildLeadCompetitors
{
	public v810RebuildLeadCompetitors(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LeadCompetitors", new DmoField[9]
		{
			new DmoField("locLeadID", "nvarchar", 10, 0, nullable: false),
			new DmoField("locLeadCompetitorID", "smallint", 4, 0, nullable: false),
			new DmoField("locOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("locProductName", "nvarchar", 50, 0, nullable: false),
			new DmoField("locLeadNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("locLeadNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("locCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("locCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("locUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LOCLEADID,LOCLEADCOMPETITORID", unique: true),
			new DmoIndex("LOCUNIQUEID", unique: true),
			new DmoIndex("locLeadID", unique: false),
			new DmoIndex("locLeadCompetitorID", unique: false),
			new DmoIndex("locOrganizationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
