using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.116", "", "")]
public class v900116
{
	public v900116(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1SALESORDERDELIVERIESENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1QUOTESBYPART' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PARTREVADVISORQUOTEENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PARTREVADVISORPOENTRY' and dgUserID <> ''");
	}
}
