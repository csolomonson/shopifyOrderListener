using System;
using M1.Core;
using M1.Core.Script;
using M1Classes92;

namespace M1.Ax.Erp.IntegrationTestsHelper;

public class NegativeQuantityOnHandTestHelper
{
	public bool JobfunctionsBackflush(M1Database database, string jobID, int JobAssemblyID, int jobOperationSeq, decimal goodQtyMade, decimal scrapQty, bool jobComplete, DateTime transactionDate, string heatLot, string uniqueID)
	{
		return ((clsJobFunctions)((ScriptApp)database.GetService(typeof(ScriptApp))).Ax("JobFunctions")).Backflush(jobID, JobAssemblyID, jobOperationSeq, decimal.ToDouble(goodQtyMade), decimal.ToDouble(scrapQty), jobComplete, transactionDate, heatLot, uniqueID);
	}
}
