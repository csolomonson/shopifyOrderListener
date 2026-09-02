using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.594", "", "")]
public class v92594
{
	public v92594(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ADDFROMSHIPMENTSO', 'M1ADDFROMSHIPMENTJOB') and dgUserID <> ''");
	}
}
