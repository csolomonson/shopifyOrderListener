using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.016", "", "")]
public class v92016
{
	public v92016(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1MFGRECEIPTCOMPONENTSENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PURCHASEORDERCOMPONENTSENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1RECEIPTCOMPONENTSENTRY' and dgUserID <> ''");
	}
}
