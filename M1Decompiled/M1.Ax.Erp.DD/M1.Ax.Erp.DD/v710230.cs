using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.230", "Add Combination Std Weekly Hours to PayrollDefinitions", "2009-02-03")]
public class v710230
{
	public v710230(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollDefinitions", "lmrComboStdWeekHours"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollDefinitions", "lmrComboStdWeekHours", "numeric", 8, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PayrollDefinitions Set lmrComboStdWeekHours = (Select Sum(lmwStandardHours) As StandardHours From PayrollOvertimeWeeks Where lmwPayrollDefinitionID = lmrPayrollDefinitionID) Where lmrCalculationType = 4 ");
		}
	}
}
