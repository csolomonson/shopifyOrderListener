using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ExpenseAccountSplits to support unicode", "2013-10-17")]
public class v810RebuildExpenseAccountSplits
{
	public v810RebuildExpenseAccountSplits(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ExpenseAccountSplits", new DmoField[10]
		{
			new DmoField("xazExpenseAccountSplitID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("xazSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("xazPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("xazPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("xazSequence", "smallint", 4, 0, nullable: false),
			new DmoField("xazExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xazPercent", "numeric", 9, 5, nullable: false),
			new DmoField("xazLandedCostCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xazCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xazCreatedDate", "datetime", 14, 0, nullable: true)
		}, new DmoIndex[6]
		{
			new DmoIndex("XAZEXPENSEACCOUNTSPLITID", unique: true),
			new DmoIndex("xazSupplierOrganizationID", unique: false),
			new DmoIndex("xazPartID", unique: false),
			new DmoIndex("xazPartRevisionID", unique: false),
			new DmoIndex("xazSequence", unique: false),
			new DmoIndex("xazLandedCostCategoryID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
