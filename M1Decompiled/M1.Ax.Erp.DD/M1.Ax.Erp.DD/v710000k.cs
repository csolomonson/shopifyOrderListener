using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add default Payroll Bank Account to Dataset Props", "2008-05-13")]
public class v710000k
{
	public v710000k(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadPayrollBankAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadPayrollBankAccountID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE DatasetProperties SET xadPayrollBankAccountID = xadBankAccountID ");
		}
	}
}
