using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add currency fields to Asset Adjustments table", "2011-12-06")]
public class v800205e
{
	public v800205e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "AssetAdjustments", "faaCurrencyRateID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetAdjustments", "faaCurrencyRateID", "char", 5, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "AssetAdjustments", "faaCustomRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetAdjustments", "faaCustomRate", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "AssetAdjustments", "faaExchangeRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetAdjustments", "faaExchangeRate", "numeric", 13, 6, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "AssetAdjustments", "faaValueForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetAdjustments", "faaValueForeign", "numeric", 12, 2, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE AssetAdjustments SET faaCurrencyRateID = (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), faaExchangeRate  = 1, faaCustomRate= 0, faaValueForeign = faaValue Where (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		}
	}
}
