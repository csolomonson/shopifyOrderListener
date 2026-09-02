using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert QualityRegisters to support unicode", "2013-10-17")]
public class v810RebuildQualityRegisters
{
	public v810RebuildQualityRegisters(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "QualityRegisters"))
		{
			parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QualityRegisters", new DmoField[60]
			{
				new DmoField("qanQualityRegisterID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qanPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qanCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanShipLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qanShipContactID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qanPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("qanPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("qanPartShortDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("qanOpenedByEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanAssignedToEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanClosedByEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanAssignedDate", "datetime", 14, 0, nullable: true),
				new DmoField("qanOpenedDate", "datetime", 14, 0, nullable: true),
				new DmoField("qanClosedDate", "datetime", 14, 0, nullable: true),
				new DmoField("qanClosed", "bit", 1, 0, nullable: false),
				new DmoField("qanLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("qanLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("qanPartTransactionID", "int", 9, 0, nullable: false),
				new DmoField("qanRegisterQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("qanQuantityInspected", "numeric", 15, 5, nullable: false),
				new DmoField("qanReceiptID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanReceiptLineID", "smallint", 4, 0, nullable: false),
				new DmoField("qanPartWareHouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qanPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("qanInventoryUnitofMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("qanJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("qanJobAssemblyID", "int", 5, 0, nullable: false),
				new DmoField("qanJobMaterialID", "int", 5, 0, nullable: false),
				new DmoField("qanJobOperationID", "int", 5, 0, nullable: false),
				new DmoField("qanSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qanPurchaseContactID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qanShipmentID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanShipmentLineID", "smallint", 4, 0, nullable: false),
				new DmoField("qanSalesOrderID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanSalesOrderLineID", "smallint", 4, 0, nullable: false),
				new DmoField("qanUnitCost", "numeric", 15, 5, nullable: false),
				new DmoField("qanInspectionNotesText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("qanInspectionNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("qanPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanPurchaseOrderLineID", "smallint", 4, 0, nullable: false),
				new DmoField("qanFirstOffInspection", "bit", 1, 0, nullable: false),
				new DmoField("qanSerialNumberID", "nvarchar", 30, 0, nullable: false),
				new DmoField("qanSource", "tinyint", 1, 0, nullable: false),
				new DmoField("qanResellerOrganizationID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanResellerLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qanResellerContactID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qanCreatedFromWeb", "bit", 1, 0, nullable: false),
				new DmoField("qanProjectID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanQualityRegisterType", "tinyint", 1, 0, nullable: false),
				new DmoField("qanSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
				new DmoField("qanPulledFromSource", "bit", 1, 0, nullable: false),
				new DmoField("qanCallID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qanStatus", "tinyint", 1, 0, nullable: false),
				new DmoField("qanProjectAreaID", "nvarchar", 15, 0, nullable: false),
				new DmoField("qanRMAClaimCreated", "bit", 1, 0, nullable: false),
				new DmoField("qanCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("qanCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("qanUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[22]
			{
				new DmoIndex("QANQUALITYREGISTERID", unique: true),
				new DmoIndex("QANUNIQUEID", unique: true),
				new DmoIndex("qanPlantDepartmentID", unique: false),
				new DmoIndex("qanPlantID", unique: false),
				new DmoIndex("qanShipContactID", unique: false),
				new DmoIndex("qanPartID", unique: false),
				new DmoIndex("qanPartRevisionID", unique: false),
				new DmoIndex("qanPartWareHouseLocationID", unique: false),
				new DmoIndex("qanPartBinID", unique: false),
				new DmoIndex("qanPurchaseContactID", unique: false),
				new DmoIndex("qanShipmentID", unique: false),
				new DmoIndex("qanShipmentLineID", unique: false),
				new DmoIndex("qanSalesOrderID", unique: false),
				new DmoIndex("qanSalesOrderLineID", unique: false),
				new DmoIndex("qanPurchaseOrderID", unique: false),
				new DmoIndex("qanPurchaseOrderLineID", unique: false),
				new DmoIndex("qanSerialNumberID", unique: false),
				new DmoIndex("qanResellerOrganizationID", unique: false),
				new DmoIndex("qanProjectID", unique: false),
				new DmoIndex("qanSalesOrderDeliveryID", unique: false),
				new DmoIndex("qanCallID", unique: false),
				new DmoIndex("qanProjectAreaID", unique: false)
			}, mergeCustomFields: true, disableTriggers: true);
		}
	}
}
