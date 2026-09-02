using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.161", "Update Last Costs", "2011-09-02")]
public class v800161b
{
	public v800161b(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update PartTransactions set imtUnitCostLast = case when imtsource = 7 then imtUnitLaborCost + imtUnitOverheadCost + imtUnitMaterialCost + imtUnitSubcontractCost + imtUnitDutyCost + imtUnitFreightCost + imtUnitMiscCost + imtPrevUnitLaborCost + imtPrevUnitOverheadCost + imtPrevUnitMaterialCost + imtPrevUnitSubcontractCost + imtPrevUnitDutyCost + imtPrevUnitFreightCost + imtPrevUnitMiscCost else imtUnitLaborCost + imtUnitOverheadCost + imtUnitMaterialCost + imtUnitSubcontractCost + imtUnitDutyCost + imtUnitFreightCost + imtUnitMiscCost end,imtUnitCostStandard = case when imtsource = 7 then imtUnitLaborCost + imtUnitOverheadCost + imtUnitMaterialCost + imtUnitSubcontractCost + imtUnitDutyCost + imtUnitFreightCost + imtUnitMiscCost + imtPrevUnitLaborCost + imtPrevUnitOverheadCost + imtPrevUnitMaterialCost + imtPrevUnitSubcontractCost + imtPrevUnitDutyCost + imtPrevUnitFreightCost + imtPrevUnitMiscCost else imtUnitLaborCost + imtUnitOverheadCost + imtUnitMaterialCost + imtUnitSubcontractCost + imtUnitDutyCost + imtUnitFreightCost + imtUnitMiscCost end,imtUnitCostAverage = case when imtsource = 7 then imtUnitLaborCost + imtUnitOverheadCost + imtUnitMaterialCost + imtUnitSubcontractCost + imtUnitDutyCost + imtUnitFreightCost + imtUnitMiscCost + imtPrevUnitLaborCost + imtPrevUnitOverheadCost + imtPrevUnitMaterialCost + imtPrevUnitSubcontractCost + imtPrevUnitDutyCost + imtPrevUnitFreightCost + imtPrevUnitMiscCost else imtUnitLaborCost + imtUnitOverheadCost + imtUnitMaterialCost + imtUnitSubcontractCost + imtUnitDutyCost + imtUnitFreightCost + imtUnitMiscCost end");
	}
}
