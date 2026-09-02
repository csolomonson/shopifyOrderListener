namespace M1.Ax.Erp;

public static class MfgReceiptEstimatedJobCostConst
{
	public const string JobId = "@jobID";

	public const string JobAssemblyId = "@JobAssemblyID";

	public const string JmaPullAllFromStock = "@jmaPullAllFromStock";

	public const string JmaParentAssembly = "@parentAssemblyID";

	public const string GetMaterialCostQuery = "SELECT ISNULL((SELECT SUM(jmmCalculatedUnitCost*jmmEstimatedQuantity) FROM JobMaterials WHERE jmmJobID=@jobID AND jmmJobAssemblyID=@jobAssemblyID),0) As UnitEstMaterialCost";

	public const string GetJobAssemblyMaterialCostQuery = "SELECT ISNULL((SELECT SUM(jmaEstimatedUnitCost * jmaQuantityToPull) FROM JobAssemblies b WHERE b.jmaJobID=@jobID AND b.jmaParentAssemblyID=@jobAssemblyID AND b.jmaJobAssemblyID!=@jobAssemblyID), 0) AS UnitEstMaterialCost";

	public const string GetLaborCostQuery = "SELECT ( ISNULL((SELECT CASE WHEN @jmaPullAllFromStock <> 0 THEN 0 ELSE SUM(jmoSetupHours*jmoSetupRate) END FROM JobOperations WHERE jmoJobID=@jobID AND jmoJobAssemblyID=@jobAssemblyID And jmoOperationType = 1),0)\r\n                                                            + ISNULL((SELECT CASE WHEN @jmaPullAllFromStock <> 0 THEN 0 ELSE SUM(jmoEstimatedProductionHours*jmoProductionRate) END FROM JobOperations INNER JOIN WorkCenters ON xawWorkCenterID=jmoWorkcenterID WHERE jmoJobID=@jobID AND jmoJobAssemblyID=@jobAssemblyID And jmoOperationType = 1),0))\r\n                                                             AS UnitEstLaborCost";

	public const string GetOverheadCostQuery = "SELECT ISNULL((SELECT CASE WHEN @jmaPullAllFromStock <> 0 THEN 0 ELSE SUM(jmoOverheadRate*(jmoEstimatedProductionHours+jmoSetupHours)) END FROM JobOperations WHERE jmoJobID=@jobID AND jmoJobAssemblyID=@jobAssemblyID And jmoOperationType=1 ),0)\r\n                                                              AS UnitEstOverheadCost";

	public const string GetContractCostQuery = "SELECT ISNULL((SELECT CASE WHEN @jmaPullAllFromStock <> 0 THEN 0 ELSE SUM(jmoCalculatedUnitCost*jmoOperationQuantity) END FROM JobOperations WHERE jmoJobID=@jobID AND jmoJobAssemblyID=@jobAssemblyID And jmoOperationType = 2),0)\r\n                                                              AS UnitEstContractCost";

	public const string GetPullAllFromStockQuery = "SELECT ISNULL(jmaPullAllFromStock,0) AS PullAllFromStock, jmaParentAssemblyID, jmaQuantityToMake\r\n                                                         FROM JobAssemblies b WHERE b.jmaJobID=@jobID AND b.jmaJobAssemblyID=@jobAssemblyID ";

	public const string GetNestedAssemblies = "SELECT jmaJobAssemblyID, jmaQuantityPerParent, jmaQuantityToPull\r\n                                                    FROM JobAssemblies b WHERE b.jmaJobID=@jobID AND b.jmaParentAssemblyID=@parentAssemblyID AND b.jmaJobAssemblyID!=@parentAssemblyID";

	public const string GetProductionQuantityQuery = "SELECT jmpProductionQuantity FROM Jobs WHERE jmpJobID=@jobID";

	public const string UnitMaterialCost = "UnitEstMaterialCost";

	public const string UnitLaborCost = "UnitEstLaborCost";

	public const string UnitOverheadCost = "UnitEstOverheadCost";

	public const string UnitContractCost = "UnitEstContractCost";

	public const string PullAllFromStock = "PullAllFromStock";

	public const string QuantityToMake = "jmaQuantityToMake";

	public const string ProductionQuantity = "jmpProductionQuantity";

	public const string ParentAssemblyId = "jmaParentAssemblyID";

	public const string JobAssemblyIdKey = "jmaJobAssemblyID";
}
