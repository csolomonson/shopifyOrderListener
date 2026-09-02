using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Refresh indexes on PartTransactions", "2011-12-06")]
public class v800205l
{
	public v800205l(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartTransactions"))
		{
			parms.Dmo.VerifyIndexesOnTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", parms.Messages, null);
		}
	}
}
