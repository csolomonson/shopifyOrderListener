using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.145", "", "")]
public class v710145
{
	public v710145(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYSHIPMENTS' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYRECEIPTS' and dgUserID <> ''");
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWPARTCLASS", 124, 188);
	}
}
