using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.696", "", "")]
public class v92696
{
	public v92696(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ADDFROMINSPECTIONQUEUE') and dgUserID <> ''");
	}
}
