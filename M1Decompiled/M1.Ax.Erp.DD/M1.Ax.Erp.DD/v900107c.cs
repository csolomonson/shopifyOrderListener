using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.107", "Update field bindings for expense account totals", "2015-11-24")]
public class v900107c
{
	public v900107c(DBConversionParms parms)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL3 varchar(8000) set @sql3 = ' Update PurchaseOrderLines Set pmlExpenseSplitPercentTotal = DetailAmount From PurchaseOrderLines Inner Join (Select PMXPURCHASEORDERID,PMXPURCHASEORDERLINEID,Sum(pmxPercent) As DetailAmount From PurchaseOrderAccounts Group By PMXPURCHASEORDERID,PMXPURCHASEORDERLINEID) As DetailTable On PMLPURCHASEORDERID = PMXPURCHASEORDERID And PMLPURCHASEORDERLINEID = PMXPURCHASEORDERLINEID; Update Organizations Set cmoExpenseSplitPercentTotal = DetailAmount From Organizations Inner Join (Select xazSupplierOrganizationID, Sum(xazPercent)As DetailAmount From ExpenseAccountSplits Where xazSupplierOrganizationID <> '''' and xazPartID = '''' and xazLandedCostCategoryID = '''' Group By xazSupplierOrganizationID) As DetailTable On cmoOrganizationID = xazSupplierOrganizationID; Update PartRevisions Set imrExpenseSplitPercentTotal = DetailAmount From PartRevisions Inner Join (Select xazPartID, xazPartRevisionID, Sum(xazPercent)As DetailAmount From ExpenseAccountSplits Where xazSupplierOrganizationID = '''' and xazPartID <> '''' and xazLandedCostCategoryID = '''' Group By xazPartID, xazPartRevisionID) As DetailTable On imrPartID = xazPartID and imrPartRevisionID = xazPartRevisionID; Update LandedCostCategories Set rmaExpenseSplitPercentTotal = DetailAmount From LandedCostCategories Inner Join (Select xazLandedCostCategoryID, Sum(xazPercent)As DetailAmount From ExpenseAccountSplits Where xazSupplierOrganizationID = '''' and xazPartID = '''' and xazLandedCostCategoryID <> '''' Group By xazLandedCostCategoryID) As DetailTable On rmaLandedCostCategoryID = xazLandedCostCategoryID; ' exec(@sql3) SET NOCOUNT OFF end;");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
	}
}
