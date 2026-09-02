using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.605", "", "")]
public class v92605
{
	public v92605(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1SHIPMENTSOPEN', 'M1SHIPMENTSREADYTOPRINT','M1SHIPMENTSBYPART','M1LOOKUPSHIPMENTSOPEN','M1SHIPMENTSFORFOLLOWUP','M1UNINVOICEDSHIPMENTS','M1UNPOSTEDSHIPMENTS','M1LABELSREADYTOPRINT') and dgUserID <> ''");
	}
}
