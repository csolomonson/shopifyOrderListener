using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.512b", "Update LandedCosts Totals ", "2017-09-04")]
public class v92512b
{
	public v92512b(DBConversionParms parms)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL varchar(8000) set @sql = ' Update LandedCosts Set rmcLandedCostPurchasesTotal = (select isnull(Sum(pmlPurchaseQuantity * pmlPurchaseUnitCostBase), 0) as TotalPOCost from PurchaseOrderLines where pmlLandedCostID = rmcLandedCostID),  rmcLandedCostReceiptsTotal = (select isnull(Sum(rmlExtendedCostBase), 0) as TotalReceiptCost from ReceiptLines left outer join Receipts on rmlReceiptID = rmpReceiptID where rmpLandedCostID = rmcLandedCostID),  rmcLandedCostTotal = (select isnull(Sum(rmlExtendedCostBase), 0) as TotalReceiptCost from ReceiptLines left outer join Receipts on rmlReceiptID = rmpReceiptID where rmpLandedCostID = rmcLandedCostID)  + rmcLandedCostChargesTotal;' exec(@sql) SET NOCOUNT OFF end;");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
	}
}
