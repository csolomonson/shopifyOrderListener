using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.016", "", "")]
public class v91016
{
	public v91016(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[1]
		{
			new TranslateInfo("App.Ax(\"PartFunctions\").GetLatestPartRevision", "App.Ax(\"Part\").GetLatestPartRevision", ignoreCase: true)
		});
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMRECEIPTPO' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMSHIPMENTSO' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMJOBMATISSUE' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMSHIPMENTJOB' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMDMRSHIPMENTDMRCLAIM' and dgUserID <> ''");
	}
}
