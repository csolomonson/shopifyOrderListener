using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.636", "", "")]
public class v91636
{
	public v91636(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1SHIPMENTSOPEN', 'M1SHIPMENTSREADYTOPRINT','M1SHIPMENTSBYPART','M1LOOKUPSHIPMENTSOPEN','M1SHIPMENTSFORFOLLOWUP','M1UNINVOICEDSHIPMENTS','M1UNPOSTEDSHIPMENTS','M1LABELSREADYTOPRINT') and dgUserID <> ''");
	}
}
