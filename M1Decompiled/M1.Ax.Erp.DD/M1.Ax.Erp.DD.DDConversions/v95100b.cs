using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.5.100", "Copy access from Job Wizard to Manufacturing Requirements Planner component", "2021-11-15")]
public class v95100b
{
	public v95100b(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Insert Into DDSecurityGroups(dzGroupID, dzUserID, dzDataset) Select 'MRP', dzUserID, dzDataset From DDSecurityGroups Where dzGroupID = 'JOBWIZARD' And 'MRP' + '-' + dzUserID + '-' + dzDataset Not In(Select dzGroupID + '-' + dzUserID + '-' + dzDataset From DDSecurityGroups)");
	}
}
