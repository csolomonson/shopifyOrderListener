using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.00.140", "", "")]
public class v700140
{
	public v700140(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYQUOTEMATRIX' and dgUserID <> ''");
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWFINANCIALPROPERTIES", 932, 972);
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYAPINVWIZARD' and dgUserID <> ''");
	}
}
