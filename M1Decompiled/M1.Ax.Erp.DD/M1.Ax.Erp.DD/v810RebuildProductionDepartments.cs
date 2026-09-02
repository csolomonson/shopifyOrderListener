using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProductionDepartments to support unicode", "2013-10-17")]
public class v810RebuildProductionDepartments
{
	public v810RebuildProductionDepartments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionDepartments", new DmoField[5]
		{
			new DmoField("xaeProductionDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xaeDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xaeCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xaeCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xaeUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("XAEPRODUCTIONDEPARTMENTID", unique: true),
			new DmoIndex("XAEUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
