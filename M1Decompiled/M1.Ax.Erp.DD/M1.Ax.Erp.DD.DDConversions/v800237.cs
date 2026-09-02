using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.00.237", "", "")]
public class v800237
{
	public v800237(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1FREIGHTPACKAGERATESALL' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1FREIGHTPACKAGELINKSALL' and dgUserID <> ''");
	}
}
