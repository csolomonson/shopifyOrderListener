using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.00.132", "", "")]
public class v800132
{
	public v800132(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE DDExplorer SET dxGridid = 'M1RMAREQSWEBTODAY' WHERE dxGridid = 'M1RMAREQSEBTODAY'");
	}
}
