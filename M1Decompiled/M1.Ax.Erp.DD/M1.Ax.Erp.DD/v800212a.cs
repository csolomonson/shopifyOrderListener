using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.212", "Add Est Exchange Rate to Landed Cost Charges table", "2012-01-16")]
public class v800212a
{
	public v800212a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostCharges", "rmhEstExchangeRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCharges", "rmhEstExchangeRate", "numeric", 13, 6, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update LandedCostCharges Set rmhEstExchangeRate = rmhExchangeRate");
		}
	}
}
