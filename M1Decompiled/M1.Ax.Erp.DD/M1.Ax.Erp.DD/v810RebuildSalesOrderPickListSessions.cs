using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderPickListSessions to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderPickListSessions
{
	public v810RebuildSalesOrderPickListSessions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderPickListSessions", new DmoField[12]
		{
			new DmoField("omsPickListSessionID", "int", 9, 0, nullable: false),
			new DmoField("omsSessionDate", "datetime", 14, 0, nullable: true),
			new DmoField("omsStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("omsClosed", "bit", 1, 0, nullable: false),
			new DmoField("omsPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omsPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omsDevice", "tinyint", 1, 0, nullable: false),
			new DmoField("omsPostedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omsPullFromStockOnly", "bit", 1, 0, nullable: false),
			new DmoField("omsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("omsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("OMSPICKLISTSESSIONID", unique: true),
			new DmoIndex("OMSUNIQUEID", unique: true),
			new DmoIndex("omsPlantDepartmentID", unique: false),
			new DmoIndex("omsPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
