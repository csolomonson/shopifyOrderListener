using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProjectContacts to support unicode", "2013-10-17")]
public class v810RebuildProjectContacts
{
	public v810RebuildProjectContacts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProjectContacts", new DmoField[11]
		{
			new DmoField("prcProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("prcProjectContactID", "smallint", 4, 0, nullable: false),
			new DmoField("prcOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("prcLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("prcContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("prcContactTitleID", "nvarchar", 5, 0, nullable: false),
			new DmoField("prcNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("prcNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("prcCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("prcCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("prcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("PRCPROJECTID,PRCPROJECTCONTACTID", unique: true),
			new DmoIndex("PRCUNIQUEID", unique: true),
			new DmoIndex("prcProjectID", unique: false),
			new DmoIndex("prcProjectContactID", unique: false),
			new DmoIndex("prcOrganizationID", unique: false),
			new DmoIndex("prcLocationID", unique: false),
			new DmoIndex("prcContactID", unique: false),
			new DmoIndex("prcContactTitleID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
