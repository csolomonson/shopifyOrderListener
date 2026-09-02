using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.158", "", "")]
public class v900158
{
	public v900158(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDExplorer WHERE dxGridID = 'M1RMAREQSWEBTODAY' OR dxGridID = 'M1UNPROCWEBRMAREQS'");
	}
}
