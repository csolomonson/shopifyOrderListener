using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.058", "Add Purchase Planner tables", "2016-05-18")]
public class v91058a
{
	public v91058a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", new DmoField[14]
		{
			new DmoField("ppsSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ppsPartClassIDs", "nvarchar(max)", 4, 0, nullable: true),
			new DmoField("ppsCompletedDate", "date", 14, 0, nullable: true),
			new DmoField("ppsCompleted", "bit", 1, 0, nullable: false),
			new DmoField("ppsPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ppsCutoffDate", "date", 14, 0, nullable: true),
			new DmoField("ppsCalculateForAllParts", "bit", 1, 0, nullable: false),
			new DmoField("ppsWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ppsJobIDs", "nvarchar(max)", 4, 0, nullable: true),
			new DmoField("ppsBuyerEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ppsGenerated", "bit", 1, 0, nullable: false),
			new DmoField("ppsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ppsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ppsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("ppsSessionID", unique: true),
			new DmoIndex("ppsUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSummaries"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PurchasePlannerSummaries");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSummaries", new DmoField[15]
		{
			new DmoField("ppySessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ppySummaryID", "int", 7, 0, nullable: false),
			new DmoField("ppyPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("ppySupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ppyPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ppyPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("ppyUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("ppyPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("ppyTotalCost", "money", 15, 2, nullable: false),
			new DmoField("ppyUnitCost", "numeric", 15, 5, nullable: false),
			new DmoField("ppyPartQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("ppyCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ppyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ppyCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ppyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("ppySessionID,ppySummaryID", unique: true),
			new DmoIndex("ppyUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PurchasePlannerLines"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PurchasePlannerLines");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerLines", new DmoField[16]
		{
			new DmoField("pplSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pplLineID", "int", 7, 0, nullable: false),
			new DmoField("pplPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("pplPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("pplPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pplMaximumQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pplLastRunDate", "date", 14, 0, nullable: true),
			new DmoField("pplWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pplLotSize", "numeric", 15, 5, nullable: false),
			new DmoField("pplPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pplQuantityOnHand", "numeric", 15, 5, nullable: false),
			new DmoField("pplMinimumQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pplCompleted", "bit", 1, 0, nullable: false),
			new DmoField("pplCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pplCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pplUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("pplSessionID,pplLineID", unique: true),
			new DmoIndex("pplUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PurchasePlannerRequirements"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PurchasePlannerRequirements");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerRequirements", new DmoField[21]
		{
			new DmoField("pprSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pprLineID", "int", 7, 0, nullable: false),
			new DmoField("pprRequirementID", "int", 4, 0, nullable: false),
			new DmoField("pprPlannedRequirementQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pprPlannedReceiptQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pprProjectedBalance", "numeric", 15, 5, nullable: false),
			new DmoField("pprPurchaseType", "tinyint", 1, 0, nullable: false),
			new DmoField("pprJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("pprJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("pprJobMaterialID", "int", 5, 0, nullable: false),
			new DmoField("pprDueDate", "date", 14, 0, nullable: true),
			new DmoField("pprPurchaseToJobQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pprPurchaseOrderDate", "date", 14, 0, nullable: true),
			new DmoField("pprPullFromStockQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pprSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pprSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("pprSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("pprPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pprCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pprCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pprUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("pprSessionID,pprLineID,pprRequirementID", unique: true),
			new DmoIndex("pprUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PurchasePlannerOrderDetails"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PurchasePlannerOrderDetails");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerOrderDetails", new DmoField[30]
		{
			new DmoField("ppoSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ppoLineID", "int", 7, 0, nullable: false),
			new DmoField("ppoOrderDetailID", "int", 4, 0, nullable: false),
			new DmoField("ppoJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("ppoJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("ppoJobMaterialID", "int", 5, 0, nullable: false),
			new DmoField("ppoSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ppoPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ppoCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ppoInventoryQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("ppoPurchaseQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("ppoPurchaseType", "tinyint", 1, 0, nullable: false),
			new DmoField("ppoSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ppoSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("ppoSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("ppoPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("ppoPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("ppoLeadTime", "smallint", 3, 0, nullable: false),
			new DmoField("ppoProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ppoProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("ppoPurchaseUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("ppoInventoryUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("ppoConversionFactor", "numeric", 14, 8, nullable: false),
			new DmoField("ppoDueDate", "date", 14, 0, nullable: true),
			new DmoField("ppoUnitCostBase", "numeric", 15, 5, nullable: false),
			new DmoField("ppoUnitCostForeign", "numeric", 15, 5, nullable: false),
			new DmoField("ppoCompleted", "bit", 1, 0, nullable: false),
			new DmoField("ppoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ppoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ppoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("ppoSessionID,ppoLineID,ppoOrderDetailID", unique: true),
			new DmoIndex("ppoUniqueID", unique: true)
		});
	}
}
