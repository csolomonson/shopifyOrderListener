using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLDepartments to support unicode", "2013-10-17")]
public class v810RebuildGLDepartments
{
	public v810RebuildGLDepartments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLDepartments", new DmoField[5]
		{
			new DmoField("gldGLDepartmentID", "nvarchar", 3, 0, nullable: false),
			new DmoField("gldDescription", "nvarchar", 30, 0, nullable: false),
			new DmoField("gldCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("gldCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("gldUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("GLDGLDEPARTMENTID", unique: true),
			new DmoIndex("GLDUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
