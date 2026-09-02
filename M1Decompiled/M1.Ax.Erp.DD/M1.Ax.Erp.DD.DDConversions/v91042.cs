using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.042", "", "")]
public class v91042
{
	public v91042(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMARINVOICERECURRING' and dgUserID <> ''");
	}
}
