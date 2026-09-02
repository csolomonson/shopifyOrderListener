using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.324", "", "")]
public class v92324
{
	public v92324(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1GLBDGDTL') and dgUserID <> ''");
	}
}
