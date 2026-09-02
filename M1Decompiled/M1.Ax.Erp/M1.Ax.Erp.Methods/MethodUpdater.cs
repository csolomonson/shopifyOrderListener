using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.Methods;

public static class MethodUpdater
{
	public static void UpdateQuantity(M1Database database, Assembly asm, decimal newQuantity)
	{
		byte decimals = database.Props("DS").Field<byte>("xadInventoryQuantityDecimals");
		Job jobFunc = new Job();
		UpdateQuantity(database, asm, newQuantity, decimals, jobFunc);
	}

	private static void UpdateQuantity(M1Database database, Assembly asm, decimal newQuantity, short decimals, Job jobFunc)
	{
		asm.ProductionQuantity = newQuantity;
		foreach (Operation operation in asm.Operations)
		{
			operation.OperationQuantity = M1Math.Round(asm.ProductionQuantity * operation.QuantityPerAssembly, decimals);
			operation.EstimatedProductionHours = (decimal)jobFunc.CalculateProductionHours(database, (double)operation.OperationQuantity, (double)operation.ProductionStandard, operation.StandardFactor, operation.WorkCenterID, 2);
		}
		foreach (Material material in asm.Materials)
		{
			material.EstimatedQuantity = (decimal)jobFunc.CalculateQtyWithScrap(database, (double)(material.QuantityPerAssembly * asm.ProductionQuantity), (double)material.ScrapPercent, (double)material.ScrapQuantity, decimals);
		}
		foreach (Assembly subAssembly in asm.SubAssemblies)
		{
			UpdateQuantity(database, subAssembly, subAssembly.QuantityPerParent * asm.ProductionQuantity, decimals, jobFunc);
		}
	}
}
