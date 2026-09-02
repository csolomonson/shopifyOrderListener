using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.512", "", "")]
public class v92512
{
	public v92512(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDSolutionDetails Set diType = 'SFEFormDefinition'  From DDSolutionDetails Inner Join DDForms On diName = dmFormID Where dmFormType = 2 And diType <> 'SFEFormDefinition'");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1LANDEDCOSTSENTRY') and dgUserID <> ''");
	}
}
