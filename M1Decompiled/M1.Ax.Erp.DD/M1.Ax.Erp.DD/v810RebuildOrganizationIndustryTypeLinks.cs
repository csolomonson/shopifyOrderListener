using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert OrganizationIndustryTypeLinks to support unicode", "2013-10-17")]
public class v810RebuildOrganizationIndustryTypeLinks
{
	public v810RebuildOrganizationIndustryTypeLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationIndustryTypeLinks", new DmoField[5]
		{
			new DmoField("cmdOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmdIndustryTypeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmdCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmdCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("CMDORGANIZATIONID,CMDINDUSTRYTYPEID", unique: true),
			new DmoIndex("CMDUNIQUEID", unique: true),
			new DmoIndex("cmdOrganizationID", unique: false),
			new DmoIndex("cmdIndustryTypeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
