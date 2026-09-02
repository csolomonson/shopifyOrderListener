using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.195", "", "")]
public class v710195
{
	public v710195(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1LANDEDCOSTRECEIPTSALL' and dgUserID <> ''");
	}
}
