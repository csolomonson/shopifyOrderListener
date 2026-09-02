using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.010", "Add Finance Source Invoice ID to AR Invoice Line", "2009-06-09")]
public class v720010
{
	public v720010(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlFinanceSourceInvoiceID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlFinanceSourceInvoiceID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
