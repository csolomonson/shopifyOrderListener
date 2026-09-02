using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.170", "", "")]
public class v710170
{
	public v710170(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1GETEMAILCONTACT' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1JOBSINPRODUCTION' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1LANDEDCOSTRECEIPTSALL' and dgUserID <> ''");
	}
}
