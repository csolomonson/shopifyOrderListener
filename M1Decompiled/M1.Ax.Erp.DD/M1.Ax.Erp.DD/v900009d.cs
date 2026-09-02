using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.009", "Add fields to DMRClaimLines table", "2014-10-31")]
public class v900009d
{
	public v900009d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRClaimLines", "dmlInventoryQuantityShipped"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRClaimLines", "dmlInventoryQuantityShipped", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRClaimLines", "dmlInventoryQuantityShipped"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DMRClaimLines Set dmlInventoryQuantityShipped = dmlQuantityShipped / CASE WHEN dmlConversionFactor = 0 THEN 1 ELSE dmlConversionFactor END");
		}
	}
}
