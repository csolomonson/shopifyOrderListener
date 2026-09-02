using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.024", "Add shift break to Timecards", "2010-05-12")]
public class v800024a
{
	public v800024a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Timecards", "lmpShiftBreakID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Timecards", "lmpShiftBreakID", "numeric", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
