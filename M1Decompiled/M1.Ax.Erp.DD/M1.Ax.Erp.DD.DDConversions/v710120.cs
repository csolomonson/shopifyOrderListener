using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.120", "", "")]
public class v710120
{
	public v710120(DDConversionParms parms)
	{
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWRECEIPT", 494, 549);
	}
}
