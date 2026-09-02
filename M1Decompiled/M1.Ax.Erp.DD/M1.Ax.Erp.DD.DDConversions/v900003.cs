using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.003", "", "")]
public class v900003
{
	public v900003(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1RECEIPTLINESENTRY' and dgUserID <> ''");
	}
}
