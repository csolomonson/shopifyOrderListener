using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.156", "", "")]
public class v900156
{
	public v900156(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMRECEIPTPO' and dgUserID <> ''");
	}
}
