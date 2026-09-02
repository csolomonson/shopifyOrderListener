using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.069", "", "")]
public class v91069
{
	public v91069(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMARINVOICESO' and dgUserID <> ''");
	}
}
