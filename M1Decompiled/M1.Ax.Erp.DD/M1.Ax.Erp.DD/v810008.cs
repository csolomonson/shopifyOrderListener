using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.008", "Add Quality Register ID to RMA Receipt Lines", "2012-10-15")]
public class v810008
{
	public v810008(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMARECEIPTLINES", "rrlQualityRegisterID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMARECEIPTLINES", "rrlQualityRegisterID", "char", 10, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralQualityRegisterID"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RMAReceiptLines Set rrlQualityRegisterID = ralQualityRegisterID From RMAReceiptLines Inner Join RMAClaimLines On rrlRMAClaimID = ralRMAClaimID And rrlRMAClaimLineID = ralRMAClaimLineID");
			}
		}
	}
}
