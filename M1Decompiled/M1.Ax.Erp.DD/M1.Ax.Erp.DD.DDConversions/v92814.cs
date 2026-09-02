using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.814", "", "")]
public class v92814
{
	public v92814(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1OPENDMRSHIPMENTS', 'M1UNPOSTEDDMRSHIPMENTS') and dgUserID <> ''");
	}
}
