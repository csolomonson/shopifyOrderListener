using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.000", "", "")]
public class v710000
{
	public v710000(DDConversionParms parms)
	{
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWSALESORDERLINE", 464, 597);
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWJOBMATERIAL", 884, 1018);
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWAPPAYMENTHEADER", 1284, 1356);
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWRMACLAIMLINE", 324, 404);
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWPAYROLLDEFINITION", 316, 356);
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWJOB", 180, 224);
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWJOBANDORDER", 368, 496);
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYAPINVWIZARD' and dgUserID <> ''");
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWPURCHASEORDER", 388, 408);
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWAPINVOICE", 492, 550);
		parms.DmoDD.MoveControlsOnForm(parms.DatabaseName, "M1.VIEWPARTREVISION", 1664, 1804);
	}
}
