using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.140", "", "")]
public class v710140
{
	public v710140(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1LANDEDCOSTCHARGESALL' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYLANDEDCOSTCHARGES' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1LANDEDCOSTSALL' and dgUserID <> ''");
	}
}
