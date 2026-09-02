using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.819", "", "")]
public class v92819
{
	public v92819(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMPICKLISTSO' and dgUserID <> ''");
	}
}
