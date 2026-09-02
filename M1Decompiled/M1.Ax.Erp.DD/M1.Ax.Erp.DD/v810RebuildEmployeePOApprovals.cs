using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeePOApprovals to support unicode", "2013-10-17")]
public class v810RebuildEmployeePOApprovals
{
	public v810RebuildEmployeePOApprovals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePOApprovals", new DmoField[6]
		{
			new DmoField("lmhEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmhApprovalEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmhEmployeePOApprovalID", "tinyint", 2, 0, nullable: false),
			new DmoField("lmhCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmhCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmhUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LMHEMPLOYEEID,LMHAPPROVALEMPLOYEEID", unique: true),
			new DmoIndex("LMHUNIQUEID", unique: true),
			new DmoIndex("lmhEmployeeID", unique: false),
			new DmoIndex("lmhApprovalEmployeeID", unique: false),
			new DmoIndex("lmhEmployeePOApprovalID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
