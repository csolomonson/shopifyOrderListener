using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.50.016", "Add Low Value Asset Pool tables and fields", "2009-06-09")]
public class v750016
{
	public v750016(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Assets", "fapLowCostAsset"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Assets", "fapLowCostAsset", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Assets", "fapLowValueAssetInPool"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Assets", "fapLowValueAssetInPool", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Assets", "fapStartYearInPool"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Assets", "fapStartYearInPool", "numeric", 4, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Assets", "fapTaxableUsePercentage"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Assets", "fapTaxableUsePercentage", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "AssetLowValuePool"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetLowValuePool");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "AssetPoolTransactions"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetPoolTransactions");
		}
	}
}
