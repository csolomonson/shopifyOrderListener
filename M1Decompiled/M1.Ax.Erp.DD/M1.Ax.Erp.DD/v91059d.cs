using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.059", "Remove PurchasePlannerSummaries table", "2016-05-23")]
public class v91059d
{
	public v91059d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSummaries"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PurchasePlannerSummaries");
		}
	}
}
