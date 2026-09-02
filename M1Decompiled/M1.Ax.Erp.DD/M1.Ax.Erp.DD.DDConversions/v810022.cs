using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.022", "", "")]
public class v810022
{
	public v810022(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Delete from DDExplorer Where dxText = 'Implementation Checklist' And dxCustom <> 0");
	}
}
