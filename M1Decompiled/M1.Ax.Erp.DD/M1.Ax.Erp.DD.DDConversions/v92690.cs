using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.690", "", "")]
public class v92690
{
	public v92690(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ADDFROMSOQUOTE') and dgUserID <> ''");
	}
}
