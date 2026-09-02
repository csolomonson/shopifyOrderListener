using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.035", "", "")]
public class v900035
{
	public v900035(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE DDUsers SET duProperties = REPLACE(duProperties, 'ShowValidationBox = False', 'ShowValidationBox = True') WHERE duProperties like '%ShowValidationBox = False%'");
	}
}
