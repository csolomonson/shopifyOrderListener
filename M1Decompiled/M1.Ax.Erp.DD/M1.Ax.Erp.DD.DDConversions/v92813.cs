using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.813", "", "")]
public class v92813
{
	public v92813(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1RECEIPTSTHISWEEK', 'M1UNPOSTEDRECEIPTS', 'M1UNINVOICEDRECEIPTS') and dgUserID <> ''");
	}
}
