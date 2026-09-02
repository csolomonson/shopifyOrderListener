using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.100", "Update allowance columns to support two characters (STP)", "2021-09-21")]
public class v95100a
{
	public v95100a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoAusAllowanceType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoAusAllowanceType", "nvarchar", 2, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPAllowances", "staAllowanceType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPAllowances", "staAllowanceType", "nvarchar", 2, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollLines", "panAusAllowanceType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollLines", "panAusAllowanceType", "nvarchar", 2, 0, isNullable: false, parms.Messages);
		}
	}
}
