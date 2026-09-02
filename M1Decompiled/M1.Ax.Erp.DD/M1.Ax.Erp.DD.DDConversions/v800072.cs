using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.00.072", "", "")]
public class v800072
{
	public v800072(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1APINVPOSTEDUNPAID' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ARINVPOSTEDUNPAID' and dgUserID <> ''");
	}
}
