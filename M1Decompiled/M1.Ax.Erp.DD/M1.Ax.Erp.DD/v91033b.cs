using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.033", "Update quotequantities to refresh upon opening", "2016-03-31")]
public class v91033b
{
	public v91033b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "QuoteQuantities"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QuoteQuantities Set qmqLaborCost = 0, qmqOverheadCost = 0, qmqQuotingCost = 0, qmqSubcontractCost = 0, qmqMaterialCost = 0, qmqSetupHours = 0, qmqProductionHours = 0");
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "QuoteLines"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QuoteLines Set qmlMatrixCalculated = 0");
		}
	}
}
