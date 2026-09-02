using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.214", "Add total hours to PayrollDefinitions table", "2012-01-22")]
public class v800214
{
	public v800214(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollDefinitions", "lmrTotalHours"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollDefinitions", "lmrTotalHours", "numeric", 7, 2, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PayrollDefinitions Set lmrTotalHours = detailTotal From PayrollDefinitions Inner Join (Select lmwPayrollDefinitionID,Sum(lmwStandardHours+lmwOvertimePeriod1+lmwOvertimePeriod2+lmwOvertimePeriod3+lmwOvertimePeriod4) As detailTotal From PayrollOvertimeWeeks Group By lmwPayrollDefinitionID) As PayrollOverTimeWeeks On lmrPayrollDefinitionID = lmwPayrollDefinitionID");
		}
	}
}
