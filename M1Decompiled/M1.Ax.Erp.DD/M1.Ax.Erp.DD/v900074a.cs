using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Add fields to JobMaterials table", "2015-08-14")]
public class v900074a
{
	public v900074a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmQuantityToReturn"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmQuantityToReturn", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmQuantityAllocated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmQuantityAllocated", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmQuantityAllocated"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobMaterials Set jmmQuantityAllocated = CASE WHEN jmmEstimatedQuantity-jmmQuantityReceived <= 0 OR jmmReceivedComplete <> 0 THEN 0 ELSE jmmEstimatedQuantity-jmmQuantityReceived END");
		}
	}
}
