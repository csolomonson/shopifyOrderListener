using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.084", "", "")]
public class v900084
{
	public v900084(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1RECEIPTLINESENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFRODMRCLAIMINSPECTION' and dgUserID <> ''");
	}
}
