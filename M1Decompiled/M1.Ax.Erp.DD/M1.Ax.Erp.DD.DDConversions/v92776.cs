using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.776", "", "")]
public class v92776
{
	public v92776(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ORGPARTINFOENTRY') and dgUserID <> ''");
	}
}
