using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.713", "Add fields to Payroll table", "2018-05-29")]
public class v92713a
{
	public v92713a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollLines", "panAusDeductionType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollLines", "panAusDeductionType", "nvarchar", 1, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollLines", "panAusAllowanceType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollLines", "panAusAllowanceType", "nvarchar", 2, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Deductions", "padAusDeductionType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", "padAusDeductionType", "nvarchar", 1, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoAusAllowanceType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoAusAllowanceType", "nvarchar", 2, 0, isNullable: false, parms.Messages);
		}
	}
}
