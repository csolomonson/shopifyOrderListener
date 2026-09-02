using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.045", "", "")]
public class v710045
{
	public v710045(DDConversionParms parms)
	{
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWQUOTEMATERIAL", 801, 857);
	}
}
