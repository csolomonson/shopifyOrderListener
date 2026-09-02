using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.00.067", "", "")]
public class v800067
{
	public v800067(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1GLJOURNALNOTBALANCE' and dgUserID <> ''");
	}
}
