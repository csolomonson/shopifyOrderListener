using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add Include RDO as Taken in Leave Accruals", "2011-12-06")]
public class v800205h
{
	public v800205h(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LeaveAccruals", "pajIncludeRDOAsTaken"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LeaveAccruals", "pajIncludeRDOAsTaken", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE LeaveAccruals SET pajIncludeRDOAsTaken = 1 FROM LeaveAccruals INNER JOIN PayrollRates On pajPayrollRateID = payPayrollRateID WHERE payAccrueRDO = 1");
		}
	}
}
