using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.017", "", "")]
public class v92017
{
	public v92017(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PARTTRANSCOSTSLOOKUP' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMWHRECEIPTWHTRANSFER' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1WAREHOUSERECEIPTLINESENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1SHIPMENTCOMPONENTSENTRY' and dgUserID <> ''");
	}
}
