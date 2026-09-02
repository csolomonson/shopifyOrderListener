using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert OrganizationContactGroupLinks to support unicode", "2013-10-17")]
public class v810RebuildOrganizationContactGroupLinks
{
	public v810RebuildOrganizationContactGroupLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationContactGroupLinks", new DmoField[7]
		{
			new DmoField("cmrOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmrLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmrContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmrContactGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("CMRORGANIZATIONID,CMRLOCATIONID,CMRCONTACTID,CMRCONTACTGROUPID", unique: true),
			new DmoIndex("CMRUNIQUEID", unique: true),
			new DmoIndex("cmrOrganizationID", unique: false),
			new DmoIndex("cmrLocationID", unique: false),
			new DmoIndex("cmrContactID", unique: false),
			new DmoIndex("cmrContactGroupID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
