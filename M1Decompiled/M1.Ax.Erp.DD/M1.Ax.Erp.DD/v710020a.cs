using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.020", "Add Termination Hours to PayrollHeaderTotals", "2008-07-04")]
public class v710020a
{
	public v710020a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderTotals", "pagTerminationHours"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotals", "pagTerminationHours", "numeric", 9, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
