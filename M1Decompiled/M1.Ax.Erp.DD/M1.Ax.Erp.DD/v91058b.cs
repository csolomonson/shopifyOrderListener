using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.058", "Add fields to JobMaterials table", "2016-05-18")]
public class v91058b
{
	public v91058b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPurchaseToJobQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmPurchaseToJobQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPullFromStockQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmPullFromStockQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPullAllFromStock"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmPullAllFromStock", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPurchaseToJobQuantity"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", new DmoIndex[1]
			{
				new DmoIndex("jmmPurchaseToJobQuantity", unique: false)
			}, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPullFromStockQuantity"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", new DmoIndex[1]
			{
				new DmoIndex("jmmPullFromStockQuantity", unique: false)
			}, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPurchaseToJobQuantity"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobMaterials set jmmPurchaseToJobQuantity = (case when isnull(impBuyForInventory,0) <> 0 Or jmmBackflush <> 0 Then 0 else jmmEstimatedQuantity end) from jobmaterials left outer join parts on jmmPartid = imppartid");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPullFromStockQuantity"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobMaterials set jmmPullFromStockQuantity = (case when isnull(impBuyForInventory,0) <> 0 Or jmmBackflush <> 0 Then jmmEstimatedQuantity else 0 end) from jobmaterials left outer join parts on jmmPartid = imppartid");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmPullAllFromStock"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobMaterials Set jmmPullAllFromStock = 1 Where jmmPullFromStockQuantity <> 0 Or jmmBackflush <> 0");
		}
	}
}
