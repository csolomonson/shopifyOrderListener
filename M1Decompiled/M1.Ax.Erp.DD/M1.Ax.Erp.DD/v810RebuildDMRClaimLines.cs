using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert DMRClaimLines to support unicode", "2013-10-17")]
public class v810RebuildDMRClaimLines
{
	public v810RebuildDMRClaimLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRClaimLines", new DmoField[46]
		{
			new DmoField("dmlDMRClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmlDMRClaimLineID", "smallint", 4, 0, nullable: false),
			new DmoField("dmlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("dmlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("dmlPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmlPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("dmlPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("dmlUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("dmlQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("dmlUnitCost", "numeric", 15, 5, nullable: false),
			new DmoField("dmlUnitCostForeign", "numeric", 15, 5, nullable: false),
			new DmoField("dmlExtendedCost", "money", 12, 2, nullable: false),
			new DmoField("dmlExtendedCostForeign", "money", 12, 2, nullable: false),
			new DmoField("dmlReturnReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmlSupplierAuthorizationNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("dmlShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dmlTrackingNumber", "nvarchar", 30, 0, nullable: false),
			new DmoField("dmlRequiredDate", "date", 14, 0, nullable: true),
			new DmoField("dmlInspectionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmlInspectionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("dmlReturnedDate", "date", 14, 0, nullable: true),
			new DmoField("dmlReceivedDate", "date", 14, 0, nullable: true),
			new DmoField("dmlShippedDate", "date", 14, 0, nullable: true),
			new DmoField("dmlInvoicedComplete", "bit", 1, 0, nullable: false),
			new DmoField("dmlTransferredToPurchaseOrder", "bit", 1, 0, nullable: false),
			new DmoField("dmlTransferredToDMRShipment", "bit", 1, 0, nullable: false),
			new DmoField("dmlDMRShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmlDMRShipmentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("dmlQuantityShipped", "numeric", 15, 5, nullable: false),
			new DmoField("dmlPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmlPurchaseOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("dmlReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmlReceiptLineID", "smallint", 4, 0, nullable: false),
			new DmoField("dmlPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("dmlPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("dmlScrap", "bit", 1, 0, nullable: false),
			new DmoField("dmlProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dmlProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("dmlOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("dmlOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("dmlConversionFactor", "numeric", 14, 8, nullable: false),
			new DmoField("dmlInventoryQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("dmlInventoryUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("dmlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("dmlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("dmlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[13]
		{
			new DmoIndex("DMLDMRCLAIMID,DMLDMRCLAIMLINEID", unique: true),
			new DmoIndex("DMLUNIQUEID", unique: true),
			new DmoIndex("dmlDMRClaimID", unique: false),
			new DmoIndex("dmlDMRClaimLineID", unique: false),
			new DmoIndex("dmlPartID", unique: false),
			new DmoIndex("dmlPartRevisionID", unique: false),
			new DmoIndex("dmlPartWarehouseLocationID", unique: false),
			new DmoIndex("dmlPartBinID", unique: false),
			new DmoIndex("dmlReturnReasonID", unique: false),
			new DmoIndex("dmlInvoicedComplete", unique: false),
			new DmoIndex("dmlProjectID", unique: false),
			new DmoIndex("dmlProjectAreaID", unique: false),
			new DmoIndex("dmlOrgPartID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
