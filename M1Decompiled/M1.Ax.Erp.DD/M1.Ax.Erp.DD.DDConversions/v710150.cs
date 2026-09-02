using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.150", "", "")]
public class v710150
{
	public v710150(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYLANDEDCOSTCHARGES' and dgUserID <> ''");
	}
}
