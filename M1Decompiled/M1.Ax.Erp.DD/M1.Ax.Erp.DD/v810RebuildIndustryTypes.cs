using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IndustryTypes to support unicode", "2013-10-17")]
public class v810RebuildIndustryTypes
{
	public v810RebuildIndustryTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IndustryTypes", new DmoField[7]
		{
			new DmoField("cmiIndustryTypeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmiShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmiLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmiLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmiCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmiCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmiUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CMIINDUSTRYTYPEID", unique: true),
			new DmoIndex("CMIUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
