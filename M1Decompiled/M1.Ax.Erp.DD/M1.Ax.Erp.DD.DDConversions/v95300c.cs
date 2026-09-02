using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.5.300", "Copy access from Job+Order Entry component to new Split Job component", "2022-04-25")]
public class v95300c
{
	public v95300c(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Insert Into DDSecurityGroups(dzGroupID, dzUserID, dzDataset) Select 'SPLITJOB', dzUserID, dzDataset From DDSecurityGroups Where dzGroupID = 'JOBANDORDERENTRY' And 'SPLITJOB' + '-' + dzUserID + '-' + dzDataset Not In(Select dzGroupID + '-' + dzUserID + '-' + dzDataset From DDSecurityGroups)");
	}
}
