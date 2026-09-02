using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.712", "Add fields to Deductions table", "2018-05-04")]
public class v92712d
{
	public v92712d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Deductions", "padAusDeductionType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", "padAusDeductionType", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
