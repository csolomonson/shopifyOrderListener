using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLAccounts to support unicode", "2013-10-17")]
public class v810RebuildGLAccounts
{
	public v810RebuildGLAccounts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLAccounts", new DmoField[10]
		{
			new DmoField("glaGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("glaGLDivisionID", "nvarchar", 3, 0, nullable: false),
			new DmoField("glaGLChartID", "nvarchar", 5, 0, nullable: false),
			new DmoField("glaGLDepartmentID", "nvarchar", 3, 0, nullable: false),
			new DmoField("glaExternalGLCode", "nvarchar", 11, 0, nullable: false),
			new DmoField("glaInactive", "bit", 1, 0, nullable: false),
			new DmoField("glaInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("glaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("GLAGLACCOUNTID", unique: true),
			new DmoIndex("GLAUNIQUEID", unique: true),
			new DmoIndex("glaInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
