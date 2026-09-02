using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.00.148", "", "")]
public class v800148
{
	public v800148(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE DDGriddetails SET dgfbox = 0, dglopt = 0  WHERE dgGridid = 'M1QTYONORDERSALES'");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE DDGriddetails SET dgfbox = 0, dglopt = 0  WHERE dgGridid = 'M1QTYONORDERPURCHASES'");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE DDGriddetails SET dgfbox = 0, dglopt = 0  WHERE dgGridid = 'M1PARTALLOCATIONS'");
	}
}
