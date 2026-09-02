using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.313", "Add fields to PAYROLLHEADERTOTALS table", "2015-05-19")]
public class v800313am
{
	public v800313am(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PAYROLLHEADERTOTALS", "pagBaseRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PAYROLLHEADERTOTALS", "pagBaseRate", "numeric", 8, 4, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
