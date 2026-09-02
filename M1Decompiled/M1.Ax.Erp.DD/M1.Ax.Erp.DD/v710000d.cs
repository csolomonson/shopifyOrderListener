using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Award Line ID to Employee Pay Rates", "2008-03-27")]
public class v710000d
{
	public v710000d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePayRates", "lnrAwardLineID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePayRates", "lnrAwardLineID", "numeric", 4, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update EmployeePayRates Set lnrAwardLineID = IsNull((Select Top 1 lnnEmployeeAwardLineID From EmployeeAwardLines Where lnnEmployeeAwardID = lnrAwardID And lnnStartDate <= lnrStartDate Order By lnnStartDate DESC),0) ");
		}
	}
}
