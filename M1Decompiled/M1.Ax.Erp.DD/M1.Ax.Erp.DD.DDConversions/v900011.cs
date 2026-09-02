using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.011", "", "")]
public class v900011
{
	public v900011(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDSecurityGroups WHERE dzGroupID = 'CASHFLOW'");
	}
}
