using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.579", "Add fields to EmployeeAllowances table", "2015-06-23")]
public class v800579p
{
	public v800579p(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeAllowances", "pawSuperannuationFundID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeAllowances", "pawSuperannuationFundID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
