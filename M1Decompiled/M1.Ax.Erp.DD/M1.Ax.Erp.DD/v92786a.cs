using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.786", "Add fields to LeaveAccruals table", "2018-10-08")]
public class v92786a
{
	public v92786a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LeaveAccruals", "pajIncludeUnpaidTimeOffInCalc"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LeaveAccruals", "pajIncludeUnpaidTimeOffInCalc", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
