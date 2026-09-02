using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Inspections to support unicode", "2013-10-17")]
public class v810RebuildInspections
{
	public v810RebuildInspections(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Inspections", new DmoField[11]
		{
			new DmoField("qapInspectionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qapPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qapPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qapProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qapInspectorEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qapInspectionDate", "datetime", 14, 0, nullable: true),
			new DmoField("qapStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("qapClosedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qapCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qapCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qapUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("QAPINSPECTIONID", unique: true),
			new DmoIndex("QAPUNIQUEID", unique: true),
			new DmoIndex("qapPlantDepartmentID", unique: false),
			new DmoIndex("qapPlantID", unique: false),
			new DmoIndex("qapProjectID", unique: false),
			new DmoIndex("qapInspectorEmployeeID", unique: false),
			new DmoIndex("qapStatus", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
