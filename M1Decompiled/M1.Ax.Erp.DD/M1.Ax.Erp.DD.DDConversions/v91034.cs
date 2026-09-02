using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.034", "", "")]
public class v91034
{
	public v91034(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMRECEIPTPO' and dgUserID <> ''");
	}
}
