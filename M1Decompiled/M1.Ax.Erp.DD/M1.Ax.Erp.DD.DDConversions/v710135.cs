using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.135", "", "")]
public class v710135
{
	public v710135(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYAUTOPAY' and dgUserID <> ''");
	}
}
