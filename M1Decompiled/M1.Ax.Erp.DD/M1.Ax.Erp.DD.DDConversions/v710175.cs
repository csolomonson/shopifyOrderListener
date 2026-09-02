using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.175", "", "")]
public class v710175
{
	public v710175(DDConversionParms parms)
	{
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWSHIPMENT", 864, 896);
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWRECEIPT", 788, 820);
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYAUTOPAY' and dgUserID <> ''");
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWDEDUCTION", 428, 492);
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYLANDEDCOSTCHARGES' and dgUserID <> ''");
	}
}
