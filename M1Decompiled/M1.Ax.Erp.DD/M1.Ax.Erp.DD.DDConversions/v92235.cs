using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.235", "", "")]
public class v92235
{
	public v92235(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDVisualizers WHERE dvVisualizerID IN ('M1TARIFFSALL') and dvUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDSeries WHERE diVisualizerID IN ('M1TARIFFSALL') and diUserID <> ''");
	}
}
