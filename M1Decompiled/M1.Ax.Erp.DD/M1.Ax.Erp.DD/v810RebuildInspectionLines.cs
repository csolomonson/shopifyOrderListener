using System;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert InspectionLines to support unicode", "2013-10-17")]
public class v810RebuildInspectionLines
{
	public v810RebuildInspectionLines(DBConversionParms parms)
	{
		bool isNullable = false;
		if (!parms.Dmo.GetCurrentFieldType(null, parms.User, parms.DatabaseName, "InspectionLines", "qalInspectionID", ref isNullable).Equals("nvarchar", StringComparison.CurrentCultureIgnoreCase) && !parms.Dmo.GetCurrentFieldType(null, parms.User, parms.DatabaseName, "InspectionLines", "qalStatus", ref isNullable).Equals("nvarchar", StringComparison.CurrentCultureIgnoreCase))
		{
			parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", new DmoField[37]
			{
				new DmoField("qalInspectionID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qalInspectionLineID", "smallint", 4, 0, nullable: false),
				new DmoField("qalPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("qalPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("qalPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qalPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("qalPartShortDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("qalUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("qalQuantityAccepted", "numeric", 15, 5, nullable: false),
				new DmoField("qalQuantityRejected", "numeric", 15, 5, nullable: false),
				new DmoField("qalQuantityToInspect", "numeric", 15, 5, nullable: false),
				new DmoField("qalQuantityToScrap", "numeric", 15, 5, nullable: false),
				new DmoField("qalQuantityToReturn", "numeric", 15, 5, nullable: false),
				new DmoField("qalActionType", "tinyint", 1, 0, nullable: false),
				new DmoField("qalReturnToSupplier", "bit", 1, 0, nullable: false),
				new DmoField("qalSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qalPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qalQualityRegisterID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qalScrapReasonID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qalInspectionComplete", "bit", 1, 0, nullable: false),
				new DmoField("qalTransferredToDMR", "bit", 1, 0, nullable: false),
				new DmoField("qalPartTransactionID", "int", 9, 0, nullable: false),
				new DmoField("qalStatus", "tinyint", 1, 0, nullable: false),
				new DmoField("qalApprovalRequestDate", "datetime", 14, 0, nullable: true),
				new DmoField("qalApprovalDecisionDate", "datetime", 14, 0, nullable: true),
				new DmoField("qalNextApprovalEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qalUnitCost", "numeric", 15, 5, nullable: false),
				new DmoField("qalInspectionNotesText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("qalInspectionNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("qalFirstOffInspection", "bit", 1, 0, nullable: false),
				new DmoField("qalPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("qalPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("qalProjectID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qalProjectAreaID", "nvarchar", 15, 0, nullable: false),
				new DmoField("qalCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("qalCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("qalUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[15]
			{
				new DmoIndex("QALINSPECTIONID,QALINSPECTIONLINEID", unique: true),
				new DmoIndex("QALUNIQUEID", unique: true),
				new DmoIndex("qalInspectionID", unique: false),
				new DmoIndex("qalInspectionLineID", unique: false),
				new DmoIndex("qalPartID", unique: false),
				new DmoIndex("qalPartRevisionID", unique: false),
				new DmoIndex("qalPartWarehouseLocationID", unique: false),
				new DmoIndex("qalPartBinID", unique: false),
				new DmoIndex("qalSupplierOrganizationID", unique: false),
				new DmoIndex("qalPurchaseLocationID", unique: false),
				new DmoIndex("qalPartTransactionID", unique: false),
				new DmoIndex("qalStatus", unique: false),
				new DmoIndex("qalNextApprovalEmployeeID", unique: false),
				new DmoIndex("qalProjectID", unique: false),
				new DmoIndex("qalProjectAreaID", unique: false)
			}, mergeCustomFields: true, disableTriggers: true);
		}
	}
}
