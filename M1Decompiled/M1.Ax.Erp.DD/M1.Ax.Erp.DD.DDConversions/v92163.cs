using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.163", "", "")]
public class v92163
{
	public v92163(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1ADDFROMARINVOICESHIPMENT') and dgUserID <> ''");
	}
}
