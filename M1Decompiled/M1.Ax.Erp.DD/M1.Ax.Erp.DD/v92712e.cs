using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.712", "Add fields to PayrollHeaderTotals table", "2018-05-21")]
public class v92712e
{
	public v92712e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderTotals", "pagAusLumpSumAType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotals", "pagAusLumpSumAType", "nvarchar", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderTotals", "pagAusLumpSumType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotals", "pagAusLumpSumType", "nvarchar", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
