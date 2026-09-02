using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.035", "Add salary sacrifice fields to Payroll Header/Line", "2008-02-29")]
public class v700035a
{
	public v700035a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaders", "patTotalSalarySacrifice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "patTotalSalarySacrifice", "money", 10, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollLines", "panSalarySacrifice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollLines", "panSalarySacrifice", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaders", "patTotalAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaders", "patTotalAmount", "money", 10, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PayrollHeaders Set patTotalAmount =  patGrossPayAmount + patTotalSalarySacrifice ");
		}
	}
}
