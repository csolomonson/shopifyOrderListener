using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.053", "Update code for references to App properties", "")]
public class v810053
{
	public v810053(DDConversionParms parms)
	{
		parms.DmoDD.ReplaceInDDCode(null, parms.DatabaseName, null, new TranslateInfo[37]
		{
			new TranslateInfo("App.BuyQuantityDecimals", "App.Ax(\"Production\").BuyQuantityDecimals", ignoreCase: true),
			new TranslateInfo("App.SellQuantityDecimals", "App.Ax(\"Production\").SellQuantityDecimals", ignoreCase: true),
			new TranslateInfo("App.InventoryQuantityDecimals", "App.Ax(\"Production\").InventoryQuantityDecimals", ignoreCase: true),
			new TranslateInfo("App.BuyerID", "App.Ax(\"Production\").BuyerID", ignoreCase: true),
			new TranslateInfo("App.BuyerAmount", "App.Ax(\"Production\").BuyerAmount", ignoreCase: true),
			new TranslateInfo("App.InspectorID", "App.Ax(\"Production\").InspectorID", ignoreCase: true),
			new TranslateInfo("App.InspectorAmount", "App.Ax(\"Production\").InspectorAmount", ignoreCase: true),
			new TranslateInfo("App.EngineerID", "App.Ax(\"Production\").EngineerID", ignoreCase: true),
			new TranslateInfo("App.PlannerID", "App.Ax(\"Production\").PlannerID", ignoreCase: true),
			new TranslateInfo("App.SalesPersonID", "App.Ax(\"Production\").SalesPersonID", ignoreCase: true),
			new TranslateInfo("App.SalesPersonAmount", "App.Ax(\"Production\").SalesPersonAmount", ignoreCase: true),
			new TranslateInfo("App.UserEmployeeID", "App.Ax(\"Production\").EmployeeID", ignoreCase: true),
			new TranslateInfo("App.UserPlantID", "App.Ax(\"Production\").PlantID", ignoreCase: true),
			new TranslateInfo("App.UserPlantDepartmentID", "App.Ax(\"Production\").PlantDepartmentID", ignoreCase: true),
			new TranslateInfo("App.PaypalActivated", "App.Ax(\"Financial\").PaypalActivated", ignoreCase: true),
			new TranslateInfo("App.NET1Activated", "App.Ax(\"Financial\").NET1Activated", ignoreCase: true),
			new TranslateInfo("App.ViewOnlyUser", "App.Security.ViewOnlyUser", ignoreCase: true),
			new TranslateInfo("App.IsUserInRole", "App.Security.IsInRole", ignoreCase: true),
			new TranslateInfo("App.IsUserInRoleByTable", "App.Security.IsInRoleByTable", ignoreCase: true),
			new TranslateInfo("App.GetObjectAccessLevel", "App.Security.GetObjectAccessLevel", ignoreCase: true),
			new TranslateInfo("App.GetTableAccessLevel", "App.Security.GetTableAccessLevel", ignoreCase: true),
			new TranslateInfo("App.GetGridAccessLevel", "App.Security.GetGridAccessLevel", ignoreCase: true),
			new TranslateInfo("App.GetFormAccessLevel", "App.Security.GetFormAccessLevel", ignoreCase: true),
			new TranslateInfo("App.GetModuleAccessLevel", "App.Security.GetModuleAccessLevel", ignoreCase: true),
			new TranslateInfo("App.GetReportAccessLevel", "App.Security.GetReportAccessLevel", ignoreCase: true),
			new TranslateInfo("App.IsAccessType", "App.Security.IsAccessType", ignoreCase: true),
			new TranslateInfo("App.GetAttachmentPath", "App.Ax(\"Organizations\").GetAttachmentPath", ignoreCase: true),
			new TranslateInfo("App.Ax(\"QuoteFunctions\").TransferQuoteToFollowup", "App.Ax(\"FollowUps\").TransferQuoteToFollowup", ignoreCase: true),
			new TranslateInfo("App.Ax(\"OrderFunctions\").ShowOrderBacklogSearchForReceipt", "Forms.Ax(\"SalesOrders\").ShowOrderBacklogSearchForReceipt", ignoreCase: true),
			new TranslateInfo("App.Ax(\"PartFunctions\").IsKitPart", "App.Ax(\"Part\").IsKitPart", ignoreCase: true),
			new TranslateInfo("App.Ax(\"PartFunctions\").RefreshPartAllocations", "App.Ax(\"Part\").RefreshPartAllocations", ignoreCase: true),
			new TranslateInfo("App.Ax(\"PartFunctions\").RefreshOnOrderQuantitesSales", "App.Ax(\"Part\").RefreshOnOrderQuantitesSales", ignoreCase: true),
			new TranslateInfo("App.Ax(\"PartFunctions\").RefreshOnOrderQuantitesPurchases", "App.Ax(\"Part\").RefreshOnOrderQuantitesPurchases", ignoreCase: true),
			new TranslateInfo("App.Ax(\"QuoteFunctions\").RefreshMatrix", "App.Ax(\"Quote\").RefreshMatrix", ignoreCase: true),
			new TranslateInfo("App.Ax(\"JobFunctions\").CompleteJob", "App.Ax(\"Job\").CompleteJob", ignoreCase: true),
			new TranslateInfo("App.Ax(\"JobFunctions\").GetJobID", "App.Ax(\"Job\").GetJobIDForOrder", ignoreCase: true),
			new TranslateInfo("App.Ax(\"JobFunctions\").DoesJobExist", "App.Ax(\"Job\").DoesJobExist", ignoreCase: true)
		});
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "Update DDExplorer Set dxSMod = 'DR' Where dxSMod = 'DS'");
	}
}
