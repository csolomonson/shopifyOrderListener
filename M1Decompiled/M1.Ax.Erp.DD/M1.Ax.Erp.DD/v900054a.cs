using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.054", "Update serial and lot numbers transactions", "2015-07-02")]
public class v900054a
{
	public v900054a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE SerialNumberTransactions SET sntTransactionType = CASE WHEN sntTransactionType = 7 THEN 46 END FROM SerialNumberTransactions WHERE sntOldTransactionType = 7");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE LotNumberTransactions SET abtTransactionType = CASE WHEN abtTransactionType = 7 THEN 46 END FROM LotNumberTransactions WHERE abtOldTransactionType = 7");
	}
}
