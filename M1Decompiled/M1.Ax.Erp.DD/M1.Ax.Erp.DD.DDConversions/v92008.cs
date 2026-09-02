using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.008", "", "")]
public class v92008
{
	public v92008(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormDetails Set deClassID = Replace(deClassID, 'M1CONTROLS.', 'M1CONTROLS92.')");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormDetails Set deClassID = Replace(deClassID, 'M1CONTROLS81.', 'M1CONTROLS92.')");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormDetails Set deClassID = Replace(deClassID, 'M1CONTROLS90.', 'M1CONTROLS92.')");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormDetails Set deClassID = Replace(deClassID, 'M1CONTROLS91.', 'M1CONTROLS92.')");
	}
}
