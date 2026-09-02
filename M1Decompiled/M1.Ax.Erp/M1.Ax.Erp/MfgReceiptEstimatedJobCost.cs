using M1.Core;

namespace M1.Ax.Erp;

public class MfgReceiptEstimatedJobCost
{
	private readonly string _jobId;

	private readonly int _jobAssemblyId;

	private readonly MfgReceiptEstimatedJobCostCalculator _calculator;

	public MfgReceiptEstimatedJobCost(string jobId, int jobAssemblyId, M1Database database)
	{
		_jobId = jobId;
		_jobAssemblyId = jobAssemblyId;
		_calculator = new MfgReceiptEstimatedJobCostCalculator(database);
	}

	public decimal CalculateUnitMaterialCost()
	{
		return _calculator.CalculateUnitCost(_jobId, _jobAssemblyId, "UnitEstMaterialCost", "SELECT ISNULL((SELECT SUM(jmmCalculatedUnitCost*jmmEstimatedQuantity) FROM JobMaterials WHERE jmmJobID=@jobID AND jmmJobAssemblyID=@jobAssemblyID),0) As UnitEstMaterialCost");
	}

	public decimal CalculateUnitLaborCost()
	{
		return _calculator.CalculateUnitCost(_jobId, _jobAssemblyId, "UnitEstLaborCost", "SELECT ( ISNULL((SELECT CASE WHEN @jmaPullAllFromStock <> 0 THEN 0 ELSE SUM(jmoSetupHours*jmoSetupRate) END FROM JobOperations WHERE jmoJobID=@jobID AND jmoJobAssemblyID=@jobAssemblyID And jmoOperationType = 1),0)\r\n                                                            + ISNULL((SELECT CASE WHEN @jmaPullAllFromStock <> 0 THEN 0 ELSE SUM(jmoEstimatedProductionHours*jmoProductionRate) END FROM JobOperations INNER JOIN WorkCenters ON xawWorkCenterID=jmoWorkcenterID WHERE jmoJobID=@jobID AND jmoJobAssemblyID=@jobAssemblyID And jmoOperationType = 1),0))\r\n                                                             AS UnitEstLaborCost");
	}

	public decimal CalculateUnitOverheadCost()
	{
		return _calculator.CalculateUnitCost(_jobId, _jobAssemblyId, "UnitEstOverheadCost", "SELECT ISNULL((SELECT CASE WHEN @jmaPullAllFromStock <> 0 THEN 0 ELSE SUM(jmoOverheadRate*(jmoEstimatedProductionHours+jmoSetupHours)) END FROM JobOperations WHERE jmoJobID=@jobID AND jmoJobAssemblyID=@jobAssemblyID And jmoOperationType=1 ),0)\r\n                                                              AS UnitEstOverheadCost");
	}

	public decimal CalculateUnitSubContractCost()
	{
		return _calculator.CalculateUnitCost(_jobId, _jobAssemblyId, "UnitEstContractCost", "SELECT ISNULL((SELECT CASE WHEN @jmaPullAllFromStock <> 0 THEN 0 ELSE SUM(jmoCalculatedUnitCost*jmoOperationQuantity) END FROM JobOperations WHERE jmoJobID=@jobID AND jmoJobAssemblyID=@jobAssemblyID And jmoOperationType = 2),0)\r\n                                                              AS UnitEstContractCost");
	}
}
