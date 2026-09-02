using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.812", "", "")]
public class v92812
{
	public v92812(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ADDFROMJOBMATISSUE') and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ORGPARTINFOENTRY') and dgUserID <> ''");
	}
}
