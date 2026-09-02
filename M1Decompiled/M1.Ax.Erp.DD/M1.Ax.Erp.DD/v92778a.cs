using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.778", "Update source field in PartTransactions for rename bin transactions", "2018-07-04")]
public class v92778a
{
	public v92778a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartTransactions Set imtSource = 13 where imtSource = 0 and imtTableName = 'PartBins'");
	}
}
