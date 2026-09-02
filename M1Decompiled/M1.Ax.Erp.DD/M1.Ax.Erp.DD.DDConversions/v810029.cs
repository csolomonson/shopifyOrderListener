using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.029", "", "")]
public class v810029
{
	public v810029(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Delete From DDExplorer Where dxType = 14 Or dxType = 13");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1SENDMESSAGECONTACTS' and dgUserID <> ''");
	}
}
