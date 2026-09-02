using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLCategories to support unicode", "2013-10-17")]
public class v810RebuildGLCategories
{
	public v810RebuildGLCategories(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLCategories", new DmoField[7]
		{
			new DmoField("gltGLCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("gltDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("gltReportSequence", "tinyint", 2, 0, nullable: false),
			new DmoField("gltCategoryType", "tinyint", 1, 0, nullable: false),
			new DmoField("gltCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("gltCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("gltUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("GLTGLCATEGORYID", unique: true),
			new DmoIndex("GLTUNIQUEID", unique: true),
			new DmoIndex("gltReportSequence", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
