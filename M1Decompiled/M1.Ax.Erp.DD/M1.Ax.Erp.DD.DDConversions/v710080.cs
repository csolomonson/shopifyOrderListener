using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.080", "", "")]
public class v710080
{
	public v710080(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYPURCHSUMMARYMAIN' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYPURCHWIZPARAMETERS' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYPURCHWIZMAINNEW' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ENTRYPURCHWIZPRICESNEW' and dgUserID <> ''");
	}
}
