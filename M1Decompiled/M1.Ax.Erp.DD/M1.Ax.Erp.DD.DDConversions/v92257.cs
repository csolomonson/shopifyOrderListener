using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.257", "", "")]
public class v92257
{
	public v92257(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ADDFROMJOBMATISSUE') and dgUserID <> ''");
	}
}
