using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.098", "", "")]
public class v900098
{
	public v900098(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMSOQUOTE' and dgUserID <> ''");
	}
}
