using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderApprovals to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderApprovals
{
	public v810RebuildSalesOrderApprovals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderApprovals", new DmoField[9]
		{
			new DmoField("omaSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omaApprovalEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omaSalesOrderApprovalID", "tinyint", 2, 0, nullable: false),
			new DmoField("omaStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("omaStatusDate", "datetime", 14, 0, nullable: true),
			new DmoField("omaDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("omaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("omaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("OMASALESORDERID,OMAAPPROVALEMPLOYEEID", unique: true),
			new DmoIndex("OMAUNIQUEID", unique: true),
			new DmoIndex("omaSalesOrderID", unique: false),
			new DmoIndex("omaApprovalEmployeeID", unique: false),
			new DmoIndex("omaSalesOrderApprovalID", unique: false),
			new DmoIndex("omaStatus", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
