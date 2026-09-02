using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.063", "", "")]
public class v92063
{
	public v92063(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1WAREHOUSERECEIPTLINESENTRY' and dgUserID <> ''");
	}
}
