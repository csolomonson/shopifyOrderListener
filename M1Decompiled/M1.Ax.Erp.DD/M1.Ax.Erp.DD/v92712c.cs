using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.712", "Add fields to Allowances table", "2018-05-04")]
public class v92712c
{
	public v92712c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoAusOtherAllowanceType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoAusOtherAllowanceType", "nvarchar", 40, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoAusAllowanceType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoAusAllowanceType", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
