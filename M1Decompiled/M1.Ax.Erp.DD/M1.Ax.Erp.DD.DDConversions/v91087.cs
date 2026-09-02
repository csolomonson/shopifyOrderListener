using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.087", "", "")]
public class v91087
{
	public v91087(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Insert Into DDSecurityGroups(dzGroupID, dzUserID, dzDataset) Select 'PURCHASEPLANNER', dzUserID, dzDataset From DDSecurityGroups Where dzGroupID = 'POWIZARD' And 'PURCHASEPLANNER' + '-' + dzUserID + '-' + dzDataset Not In(Select dzGroupID + '-' + dzUserID + '-' + dzDataset From DDSecurityGroups)");
	}
}
