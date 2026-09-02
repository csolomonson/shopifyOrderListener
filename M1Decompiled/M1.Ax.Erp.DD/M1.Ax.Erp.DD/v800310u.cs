using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.310", "Add fields to EMPLOYEEDEDUCTIONS table", "2015-05-19")]
public class v800310u
{
	public v800310u(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeChildSupport"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeChildSupport", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeChildSupportCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeChildSupportCode", "nvarchar", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeStudentLoan"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeStudentLoan", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeStudentLoanType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeStudentLoanType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeMemberID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeMemberID", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeSpouseContribution"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EMPLOYEEDEDUCTIONS", "paeSpouseContribution", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
