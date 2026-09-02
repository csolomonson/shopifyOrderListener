using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.245", "Add Field Union Fees to Deductions and EmployeeDeductions table.", "2012-06-01")]
public class v800245
{
	public v800245(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Deductions", "padUnionFees"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", "padUnionFees", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeDeductions", "paeUnionFees"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeDeductions", "paeUnionFees", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
