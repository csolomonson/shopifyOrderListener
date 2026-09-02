using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.024", "", "")]
public class v92024
{
	public v92024(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1INSPECTIONCOMPONENTSENTRY' and dgUserID <> ''");
	}
}
