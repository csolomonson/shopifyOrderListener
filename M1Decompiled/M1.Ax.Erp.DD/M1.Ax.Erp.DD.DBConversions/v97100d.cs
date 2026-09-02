using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.100", "Add field pagAusIsETP to PayrollHeaderTotals table", "2024-02-07")]
public class v97100d
{
	public v97100d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderTotals", "pagAusIsETP"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotals", "pagAusIsETP", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
