using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.506", "", "")]
public class v92506
{
	public v92506(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1OPENRECEIPTS', 'M1PURCHASEORDERLINESLC') and dgUserID <> ''");
	}
}
