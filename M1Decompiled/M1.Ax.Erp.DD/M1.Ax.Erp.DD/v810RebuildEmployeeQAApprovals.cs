using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeQAApprovals to support unicode", "2013-10-17")]
public class v810RebuildEmployeeQAApprovals
{
	public v810RebuildEmployeeQAApprovals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeQAApprovals", new DmoField[6]
		{
			new DmoField("lmbEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmbApprovalEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmbEmployeeQAApprovalID", "tinyint", 2, 0, nullable: false),
			new DmoField("lmbCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmbCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmbUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LMBEMPLOYEEID,LMBAPPROVALEMPLOYEEID", unique: true),
			new DmoIndex("LMBUNIQUEID", unique: true),
			new DmoIndex("lmbEmployeeID", unique: false),
			new DmoIndex("lmbApprovalEmployeeID", unique: false),
			new DmoIndex("lmbEmployeeQAApprovalID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
