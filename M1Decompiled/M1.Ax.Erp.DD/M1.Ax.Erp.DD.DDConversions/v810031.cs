using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.031", "Update code for references to App methods", "")]
public class v810031
{
	public v810031(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[12]
		{
			new TranslateInfo("App.Forms", "Forms", ignoreCase: true),
			new TranslateInfo("App.GetYearAndPeriod", "App.Ax(\"Financial\").GetYearAndPeriod", ignoreCase: true),
			new TranslateInfo("App.GetYear", "App.Ax(\"Financial\").GetYear", ignoreCase: true),
			new TranslateInfo("App.GetFiscalYearStartDate", "App.Ax(\"Financial\").GetFiscalYearStartDate", ignoreCase: true),
			new TranslateInfo("App.GetFiscalYearEndDate", "App.Ax(\"Financial\").GetFiscalYearEndDate", ignoreCase: true),
			new TranslateInfo("App.GetPeriod", "App.Ax(\"Financial\").GetPeriod", ignoreCase: true),
			new TranslateInfo("App.ARFunctions.DisableTaxFields", "App.Ax(\"AR\").DisableTaxFields", ignoreCase: true),
			new TranslateInfo("App.APFunctions.DisableTaxFields", "App.Ax(\"AP\").DisableTaxFields", ignoreCase: true),
			new TranslateInfo("App.SendEmployeeMessage", "App.Ax(\"Employee\").SendEmployeeMessage", ignoreCase: true),
			new TranslateInfo("App.DoRoleCheck", "Forms.DoRoleCheck", ignoreCase: true),
			new TranslateInfo("App.DoReportCheck", "Forms.DoReportCheck", ignoreCase: true),
			new TranslateInfo("App.DoRoleByTableCheck", "Forms.DoRoleByTableCheck", ignoreCase: true)
		});
	}
}
