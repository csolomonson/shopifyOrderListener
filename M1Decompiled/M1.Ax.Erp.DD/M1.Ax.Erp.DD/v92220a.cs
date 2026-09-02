using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.220", "Add fields to ManufacturingVarianceLog table", "2017-04-13")]
public class v92220a
{
	public v92220a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ManufacturingVarianceLog"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ManufacturingVarianceLog", new DmoField[35]
			{
				new DmoField("mvlManufacturingVarianceLogID", "numeric", 9, 0, nullable: false),
				new DmoField("mvlCostType", "tinyint", 1, 0, nullable: false),
				new DmoField("mvlPartTransactionID", "int", 9, 0, nullable: false),
				new DmoField("mvlPartTransactionCostID", "int", 4, 0, nullable: false),
				new DmoField("mvlPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("mvlPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("mvlJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("mvlSalesOrderID", "nvarchar", 10, 0, nullable: false),
				new DmoField("mvlSalesOrderLineID", "smallint", 4, 0, nullable: false),
				new DmoField("mvlSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
				new DmoField("mvlDeliveryType", "tinyint", 1, 0, nullable: false),
				new DmoField("mvlPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("mvlPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("mvlUnitDifference", "numeric", 15, 5, nullable: false),
				new DmoField("mvlNewUnitCOGSCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlNewUnitMaterialCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlNewUnitLaborCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlNewUnitOverheadCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlNewUnitSubcontractCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlNewUnitDutyCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlNewUnitFreightCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlNewUnitMiscCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlOldUnitCOGSCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlOldUnitMaterialCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlOldUnitLaborCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlOldUnitOverheadCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlOldUnitSubcontractCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlOldUnitDutyCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlOldUnitFreightCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlOldUnitMiscCost", "numeric", 15, 5, nullable: false),
				new DmoField("mvlTransactionDate", "datetime", 14, 0, nullable: true),
				new DmoField("mvlSource", "tinyint", 1, 0, nullable: false),
				new DmoField("mvlCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("mvlCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("mvlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("mvlManufacturingVarianceLogID", unique: true),
				new DmoIndex("mvlUniqueID", unique: true)
			});
		}
	}
}
