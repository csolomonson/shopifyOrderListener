using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.093", "Added pagShiftGroup to PayrollHeaderTotals", "2010-12-10")]
public class v800093
{
	public v800093(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderTotals", "pagShiftGroup"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotals", "pagShiftGroup", "numeric", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
