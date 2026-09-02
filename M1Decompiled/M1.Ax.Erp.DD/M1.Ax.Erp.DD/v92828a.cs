using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.828", "Add fields to MfgReceipts table", "2020-08-12")]
public class v92828a
{
	public v92828a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmMfgCostType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmMfgCostType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update MfgReceipts Set rmmMfgCostType = 1");
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmNotUpdateJobQtyComplete"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmNotUpdateJobQtyComplete", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update MfgReceipts Set rmmNotUpdateJobQtyComplete = 0");
			}
		}
	}
}
