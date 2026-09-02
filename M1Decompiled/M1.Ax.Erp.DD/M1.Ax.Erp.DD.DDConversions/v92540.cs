using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.540", "", "")]
public class v92540
{
	public v92540(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ADDFROMJOBMATISSUE') and dgUserID <> ''");
	}
}
