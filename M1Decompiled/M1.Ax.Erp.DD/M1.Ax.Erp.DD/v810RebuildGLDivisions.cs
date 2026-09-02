using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLDivisions to support unicode", "2013-10-17")]
public class v810RebuildGLDivisions
{
	public v810RebuildGLDivisions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLDivisions", new DmoField[6]
		{
			new DmoField("glvGLDivisionID", "nvarchar", 3, 0, nullable: false),
			new DmoField("glvDescription", "nvarchar", 30, 0, nullable: false),
			new DmoField("glvRetainedEarningsAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("glvCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glvCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glvUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("GLVGLDIVISIONID", unique: true),
			new DmoIndex("GLVUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
