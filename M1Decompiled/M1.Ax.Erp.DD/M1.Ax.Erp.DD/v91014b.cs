using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.014", "Add fields to MfgReceipts table", "2016-02-22")]
public class v91014b
{
	public v91014b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmTotalUnitCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmTotalUnitCost", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmTotalUnitCost"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update MfgReceipts Set rmmTotalUnitCost = rmmUnitLaborCost+rmmUnitOverheadCost+rmmUnitMaterialCost+rmmUnitSubcontractCost");
		}
	}
}
