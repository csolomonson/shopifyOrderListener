using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.146", "Add fields to Timecards/TimecardLines table", "2011-06-23")]
public class v800146
{
	public v800146(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Timecards", "lmpCreatedFromPayrollSession"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Timecards", "lmpCreatedFromPayrollSession", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "TimecardLines", "lmlCreatedFromPayrollSession"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TimecardLines", "lmlCreatedFromPayrollSession", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
