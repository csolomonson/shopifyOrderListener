using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.032", "Update code for references to App.Functions", "")]
public class v810032
{
	public v810032(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[19]
		{
			new TranslateInfo("App.ARFunctions.", "App.Ax(\"ARFunctions\").", ignoreCase: true),
			new TranslateInfo("App.APFunctions.", "App.Ax(\"APFunctions\").", ignoreCase: true),
			new TranslateInfo("App.GLFunctions.", "App.Ax(\"GLFunctions\").", ignoreCase: true),
			new TranslateInfo("App.LeadFunctions.", "App.Ax(\"LeadFunctions\").", ignoreCase: true),
			new TranslateInfo("App.QuoteFunctions.", "App.Ax(\"QuoteFunctions\").", ignoreCase: true),
			new TranslateInfo("App.OrderFunctions.", "App.Ax(\"OrderFunctions\").", ignoreCase: true),
			new TranslateInfo("App.ClaimFunctions.", "App.Ax(\"ClaimFunctions\").", ignoreCase: true),
			new TranslateInfo("App.ShipmentFunctions.", "App.Ax(\"ShipmentFunctions\").", ignoreCase: true),
			new TranslateInfo("App.FreightFunctions.", "App.Ax(\"FreightFunctions\").", ignoreCase: true),
			new TranslateInfo("App.ReceiptFunctions.", "App.Ax(\"ReceiptFunctions\").", ignoreCase: true),
			new TranslateInfo("App.RFQFunctions.", "App.Ax(\"RFQFunctions\").", ignoreCase: true),
			new TranslateInfo("App.POFunctions.", "App.Ax(\"POFunctions\").", ignoreCase: true),
			new TranslateInfo("App.QAFunctions.", "App.Ax(\"QAFunctions\").", ignoreCase: true),
			new TranslateInfo("App.ProjectFunctions.", "App.Ax(\"ProjectFunctions\").", ignoreCase: true),
			new TranslateInfo("App.JobFunctions.", "App.Ax(\"JobFunctions\").", ignoreCase: true),
			new TranslateInfo("App.PartFunctions.", "App.Ax(\"PartFunctions\").", ignoreCase: true),
			new TranslateInfo("App.TimecardFunctions.", "App.Ax(\"TimecardFunctions\").", ignoreCase: true),
			new TranslateInfo("App.LandedCostFunctions.", "App.Ax(\"LandedCostFunctions\").", ignoreCase: true),
			new TranslateInfo("App.PayrollFunctions.", "App.Ax(\"PayrollFunctions\").", ignoreCase: true)
		});
	}
}
