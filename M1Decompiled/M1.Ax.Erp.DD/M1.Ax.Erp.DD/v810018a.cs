using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.018", "Add Column lnrAwardRate to EmployeePayRates table.", "2013-01-14")]
public class v810018a
{
	public v810018a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeePayRates", "lnrAwardRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeePayRates", "lnrAwardRate", "numeric", 8, 4, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update EmployeePayRates Set lnrAwardRate = IsNull((Select Top 1 lnnPayRate From EmployeeAwardLines Where lnnEmployeeAwardID = lnrAwardID and lnnStartDate <= lnrStartDate Order By lnnStartDate Desc),0) Where lnrAwardID <> ''");
		}
	}
}
