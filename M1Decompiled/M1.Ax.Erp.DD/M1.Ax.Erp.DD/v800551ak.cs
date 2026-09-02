using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.551", "Add fields to EMPLOYEEALLOWANCES table", "2015-05-19")]
public class v800551ak
{
	public v800551ak(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEALLOWANCES", "pawMemberID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEALLOWANCES", "pawMemberID", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEALLOWANCES", "pawEmployerAddContrib"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEALLOWANCES", "pawEmployerAddContrib", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
