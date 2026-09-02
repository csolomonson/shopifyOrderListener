using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.029", "", "")]
public class v900029
{
	public v900029(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDSecurityGroups WHERE dzGroupID = 'CASHFLOW'");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDUsers where duUserID = 'CASHFLOW'");
	}
}
