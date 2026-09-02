using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Employees to support unicode", "2013-10-17")]
public class v810RebuildEmployees
{
	public v810RebuildEmployees(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Employees", new DmoField[49]
		{
			new DmoField("lmeEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmeEmployeeName", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmeContactTitleID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmeWorkEmailAddress", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmeHireDate", "date", 14, 0, nullable: true),
			new DmoField("lmeTerminationDate", "date", 14, 0, nullable: true),
			new DmoField("lmeTerminationReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmePayrollEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmePlannerEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmeShopEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmeSupportEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmeEngineerEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmeInspectorEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmePlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmePlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmeHomeProductionDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmeDefaultWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmeDirectExpenseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmeIndirectExpenseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmeCallTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmeLockShift", "bit", 1, 0, nullable: false),
			new DmoField("lmeDefaultShiftID", "smallint", 3, 0, nullable: false),
			new DmoField("lmePassword", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmeLanguage", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmeSalesEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmeCommissionRate", "numeric", 5, 2, nullable: false),
			new DmoField("lmeEarningType", "tinyint", 1, 0, nullable: false),
			new DmoField("lmeCanChangePOSPrices", "bit", 1, 0, nullable: false),
			new DmoField("lmeSOApprovalAmount", "money", 12, 2, nullable: false),
			new DmoField("lmeQuoterEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmeBuyerEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmePOApprovalAmount", "money", 12, 2, nullable: false),
			new DmoField("lmeUserID", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmeProjectManagerEmployee", "bit", 1, 0, nullable: false),
			new DmoField("lmePicture", "image", 4, 0, nullable: true),
			new DmoField("lmeWebLoginEnabled", "bit", 1, 0, nullable: false),
			new DmoField("lmeWebPassword", "nvarchar", 80, 0, nullable: false),
			new DmoField("lmeWebTemplate", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmeWebExpirationDate", "date", 14, 0, nullable: true),
			new DmoField("lmeWebTemplateUseM1UserID", "bit", 1, 0, nullable: false),
			new DmoField("lmeQAApprovalAmount", "money", 12, 2, nullable: false),
			new DmoField("lmeSortSFEbyWorkcenter", "bit", 1, 0, nullable: false),
			new DmoField("lmeUseEmail", "tinyint", 1, 0, nullable: false),
			new DmoField("lmeCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmeCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmeUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("lmeCountyCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmeCessationType", "nvarchar", 1, 0, nullable: false),
			new DmoField("lmePreviousEmployeeID", "nvarchar", 10, 0, nullable: false)
		}, new DmoIndex[20]
		{
			new DmoIndex("LMEEMPLOYEEID", unique: true),
			new DmoIndex("LMEUNIQUEID", unique: true),
			new DmoIndex("lmeEmployeeName", unique: false),
			new DmoIndex("lmeContactTitleID", unique: false),
			new DmoIndex("lmeTerminationReasonID", unique: false),
			new DmoIndex("lmePayrollEmployee", unique: false),
			new DmoIndex("lmePlannerEmployee", unique: false),
			new DmoIndex("lmeShopEmployee", unique: false),
			new DmoIndex("lmeSupportEmployee", unique: false),
			new DmoIndex("lmeEngineerEmployee", unique: false),
			new DmoIndex("lmeInspectorEmployee", unique: false),
			new DmoIndex("lmePlantDepartmentID", unique: false),
			new DmoIndex("lmePlantID", unique: false),
			new DmoIndex("lmeHomeProductionDepartmentID", unique: false),
			new DmoIndex("lmeDefaultWorkCenterID", unique: false),
			new DmoIndex("lmeCallTypeID", unique: false),
			new DmoIndex("lmeSalesEmployee", unique: false),
			new DmoIndex("lmeQuoterEmployee", unique: false),
			new DmoIndex("lmeProjectManagerEmployee", unique: false),
			new DmoIndex("lmeWebLoginEnabled", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
