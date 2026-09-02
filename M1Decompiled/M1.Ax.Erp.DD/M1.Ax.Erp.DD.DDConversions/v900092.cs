using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.092", "", "")]
public class v900092
{
	public v900092(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMJOBMATISSUE' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PARTSONWEB' and dgUserID <> ''");
	}
}
