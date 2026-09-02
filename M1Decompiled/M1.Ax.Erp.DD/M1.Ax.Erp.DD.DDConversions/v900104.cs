using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.104", "", "")]
public class v900104
{
	public v900104(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMINSPECTIONQUEUE' and dgUserID <> ''");
	}
}
