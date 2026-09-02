using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PurchaseOrderApprovals to support unicode", "2013-10-17")]
public class v810RebuildPurchaseOrderApprovals
{
	public v810RebuildPurchaseOrderApprovals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderApprovals", new DmoField[9]
		{
			new DmoField("pmaPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmaApprovalEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmaPurchaseOrderApprovalID", "tinyint", 2, 0, nullable: false),
			new DmoField("pmaStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("pmaStatusDate", "datetime", 14, 0, nullable: true),
			new DmoField("pmaDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pmaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pmaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pmaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("PMAPURCHASEORDERID,PMAAPPROVALEMPLOYEEID", unique: true),
			new DmoIndex("PMAUNIQUEID", unique: true),
			new DmoIndex("pmaPurchaseOrderID", unique: false),
			new DmoIndex("pmaApprovalEmployeeID", unique: false),
			new DmoIndex("pmaPurchaseOrderApprovalID", unique: false),
			new DmoIndex("pmaStatus", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
