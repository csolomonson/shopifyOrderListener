using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.067", "", "")]
public class v92067c
{
	public v92067c(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PARTBINSENTRY' and dgUserID <> ''");
	}
}
