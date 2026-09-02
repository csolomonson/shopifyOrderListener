using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.114", "", "")]
public class v900114
{
	public v900114(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1CALLQUEUEENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDExplorer Set dxDisabled = 1 where dxtext like '%Landed Cost%'");
	}
}
