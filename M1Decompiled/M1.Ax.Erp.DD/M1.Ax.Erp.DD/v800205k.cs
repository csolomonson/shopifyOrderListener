using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Refresh indexes on PurchaseOrderAccounts", "2011-12-06")]
public class v800205k
{
	public v800205k(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PurchaseOrderAccounts"))
		{
			parms.Dmo.VerifyIndexesOnTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderAccounts", parms.Messages, null);
		}
	}
}
