using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.052", "", "")]
public class v91052
{
	public v91052(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMJOBMATISSUE' and dgUserID <> ''");
	}
}
