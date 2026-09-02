using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.028", "", "")]
public class v92028
{
	public v92028(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMINSPECTIONQUEUE' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1DMRSHIPMENTLINESENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1SHIPMENTCOMPONENTSENTRY' and dgUserID <> ''");
	}
}
