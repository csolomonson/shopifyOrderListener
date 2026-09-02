using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.010", "", "")]
public class v810010
{
	public v810010(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormDetails Set deClassID = Replace(deClassID, 'M1CONTROLS.', 'M1CONTROLS92.')");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDForms Set dmAllInDD = 1 Where dmCustom = 1");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormDetails Set deCustom = Case When dmCustom = 1 Then 1 Else deCustom End From DDFormDetails Inner Join DDForms on deFormID=dmFormID");
	}
}
