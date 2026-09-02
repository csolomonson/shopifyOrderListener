using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.052", "", "")]
public class v900052
{
	public v900052(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDExplorer Set dxextd = Replace(dxextd,'Forms.Show.Report','Forms.Report.Run') where dxExtd like '%Forms.Show.Report%'");
	}
}
