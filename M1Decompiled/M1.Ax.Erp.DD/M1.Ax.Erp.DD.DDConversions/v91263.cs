using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.263", "", "")]
public class v91263
{
	public v91263(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDSearches Set dsGridID = '' Where dsSearchID In ('M1SEARCHALLOCATIONS','M1SEARCHONORDERQTYVARIANCE','M1SEARCHALLOCATIONSVARIANCE','M1SEARCHQTYONORDERSALES','M1SEARCHQTYONORDERPURCHASES')");
	}
}
