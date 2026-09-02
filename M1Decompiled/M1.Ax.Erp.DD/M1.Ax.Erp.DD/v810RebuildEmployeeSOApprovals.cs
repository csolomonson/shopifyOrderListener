using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeSOApprovals to support unicode", "2013-10-17")]
public class v810RebuildEmployeeSOApprovals
{
	public v810RebuildEmployeeSOApprovals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeSOApprovals", new DmoField[6]
		{
			new DmoField("lmoEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmoApprovalEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmoEmployeeSOApprovalID", "tinyint", 2, 0, nullable: false),
			new DmoField("lmoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LMOEMPLOYEEID,LMOAPPROVALEMPLOYEEID", unique: true),
			new DmoIndex("LMOUNIQUEID", unique: true),
			new DmoIndex("lmoEmployeeID", unique: false),
			new DmoIndex("lmoApprovalEmployeeID", unique: false),
			new DmoIndex("lmoEmployeeSOApprovalID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
