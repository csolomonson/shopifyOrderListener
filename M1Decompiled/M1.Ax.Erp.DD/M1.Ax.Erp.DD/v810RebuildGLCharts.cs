using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLCharts to support unicode", "2013-10-17")]
public class v810RebuildGLCharts
{
	public v810RebuildGLCharts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLCharts", new DmoField[13]
		{
			new DmoField("glcGLChartID", "nvarchar", 5, 0, nullable: false),
			new DmoField("glcDescription", "nvarchar", 35, 0, nullable: false),
			new DmoField("glcParentAccount", "bit", 1, 0, nullable: false),
			new DmoField("glcParentGLChartID", "nvarchar", 5, 0, nullable: false),
			new DmoField("glcParentDescription", "nvarchar", 35, 0, nullable: false),
			new DmoField("glcGLCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("glcAccountType", "tinyint", 1, 0, nullable: false),
			new DmoField("glcNormalBalance", "tinyint", 1, 0, nullable: false),
			new DmoField("glcCashEquivalents", "bit", 1, 0, nullable: false),
			new DmoField("glcCashFlowCategory", "tinyint", 1, 0, nullable: false),
			new DmoField("glcCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glcCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("GLCGLCHARTID", unique: true),
			new DmoIndex("GLCUNIQUEID", unique: true),
			new DmoIndex("glcParentAccount", unique: false),
			new DmoIndex("glcParentGLChartID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
