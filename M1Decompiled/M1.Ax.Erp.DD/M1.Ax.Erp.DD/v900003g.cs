using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add fields to MfgReceipts table", "2014-09-25")]
public class v900003g
{
	public v900003g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MfgReceipts"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", new DmoField[53]
			{
				new DmoField("rmmMfgReceiptID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rmmReceiptType", "tinyint", 1, 0, nullable: false),
				new DmoField("rmmReceiptDate", "datetime", 14, 0, nullable: true),
				new DmoField("rmmClosed", "bit", 1, 0, nullable: false),
				new DmoField("rmmPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rmmPurchaseOrderLineID", "smallint", 4, 0, nullable: false),
				new DmoField("rmmPurchaseQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmmPOOpenQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmmPOLineReceivedComplete", "bit", 1, 0, nullable: false),
				new DmoField("rmmJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("rmmJobAssemblyID", "int", 5, 0, nullable: false),
				new DmoField("rmmJobMaterialID", "int", 5, 0, nullable: false),
				new DmoField("rmmJobOperationID", "int", 5, 0, nullable: false),
				new DmoField("rmmJobType", "tinyint", 1, 0, nullable: false),
				new DmoField("rmmEstimatedQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmmJobOpenQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmmReceivedComplete", "bit", 1, 0, nullable: false),
				new DmoField("rmmInventoryQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmmProductionQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmmQuantityCompleted", "numeric", 15, 5, nullable: false),
				new DmoField("rmmQuantityReceivedToInventory", "numeric", 15, 5, nullable: false),
				new DmoField("rmmJobScrapQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmmProductionComplete", "bit", 1, 0, nullable: false),
				new DmoField("rmmPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("rmmPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("rmmPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rmmPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("rmmSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rmmPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rmmQuantityOnHand", "numeric", 15, 5, nullable: false),
				new DmoField("rmmUnitLaborCost", "numeric", 15, 5, nullable: false),
				new DmoField("rmmUnitOverheadCost", "numeric", 15, 5, nullable: false),
				new DmoField("rmmUnitMaterialCost", "numeric", 15, 5, nullable: false),
				new DmoField("rmmUnitSubcontractCost", "numeric", 15, 5, nullable: false),
				new DmoField("rmmLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("rmmLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("rmmPurchaseQuantityReceived", "numeric", 15, 5, nullable: false),
				new DmoField("rmmPurchaseUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("rmmPurchaseUnitCost", "numeric", 15, 5, nullable: false),
				new DmoField("rmmSetupCharge", "numeric", 9, 2, nullable: false),
				new DmoField("rmmInventoryQuantityReceived", "numeric", 15, 5, nullable: false),
				new DmoField("rmmScrapQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmmInventoryUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("rmmReference", "nvarchar", 30, 0, nullable: false),
				new DmoField("rmmRequiresInspection", "bit", 1, 0, nullable: false),
				new DmoField("rmmHeatLot", "nvarchar", 50, 0, nullable: false),
				new DmoField("rmmPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rmmProjectID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rmmProjectAreaID", "nvarchar", 15, 0, nullable: false),
				new DmoField("rmmIMCostingMethod", "tinyint", 1, 0, nullable: false),
				new DmoField("rmmCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("rmmCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("rmmUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[16]
			{
				new DmoIndex("rmmMfgReceiptID", unique: true),
				new DmoIndex("rmmUniqueID", unique: true),
				new DmoIndex("rmmReceiptType", unique: false),
				new DmoIndex("rmmReceiptDate", unique: false),
				new DmoIndex("rmmJobID", unique: false),
				new DmoIndex("rmmJobAssemblyID", unique: false),
				new DmoIndex("rmmJobMaterialID", unique: false),
				new DmoIndex("rmmJobOperationID", unique: false),
				new DmoIndex("rmmJobType", unique: false),
				new DmoIndex("rmmReceivedComplete", unique: false),
				new DmoIndex("rmmProductionComplete", unique: false),
				new DmoIndex("rmmPartID", unique: false),
				new DmoIndex("rmmPartRevisionID", unique: false),
				new DmoIndex("rmmPartWarehouseLocationID", unique: false),
				new DmoIndex("rmmPartBinID", unique: false),
				new DmoIndex("rmmPlantID", unique: false)
			});
		}
	}
}
