using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.021", "", "")]
public class v810021
{
	public v810021(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormDetails Set deClassID = 'M1SFE.M1SFELabel' From DDFormDetails Inner Join DDForms On deFormID = dmFormID Where dmFormType = 2 And (deClassID = 'VB.Label' Or deClassID = 'M1SFELABEL')");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDFormDetails Set deClassID = 'M1SFE.M1SFE3DHLine' From DDFormDetails Inner Join DDForms On deFormID = dmFormID Where dmFormType = 2 And (deClassID = 'VB.Line' Or deClassID = 'M1SFE.M1SFELINE' Or deClassID = 'M1SFELINE' Or deClassID = 'M1SFE3DHLINE')");
	}
}
