using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.161", "Update Part Transactions", "2011-09-02")]
public class v800161a
{
	public v800161a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update PartTransactions set imtUnitCostAverage = imrAverageDutyCost + imrAverageFreightCost + imrAverageLaborCost + imrAverageMaterialCost + imrAverageMiscCost + imrAverageOverheadCost,imtUnitCostLast = imrLastDutyCost + imrLastFreightCost + imrLastLaborCost + imrLastMaterialCost + imrLastMiscCost + imrLastOverheadCost,imtUnitCostStandard = imrStandardDutyCost + imrStandardFreightCost + imrStandardLaborCost + imrStandardMaterialCost + imrStandardMiscCost + imrStandardOverheadCost from PartRevisions where imtPartTransactionID in (select top 1 imtPartTransactionID from PartTransactions as trans2 where trans2.imtPartID = imrPartID and trans2.imtPartRevisionID = imrPartRevisionID order by imtTransactionDate desc) and imtPartID = imrPartID and imtPartRevisionID = imrPartRevisionID");
	}
}
