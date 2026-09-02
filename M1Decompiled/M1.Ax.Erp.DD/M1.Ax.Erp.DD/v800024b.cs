using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.024", "Add hours calculated fields to TimecardLines", "2010-05-12")]
public class v800024b
{
	public v800024b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "TimecardLines", "lmlMachineHoursCalculated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TimecardLines", "lmlMachineHoursCalculated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "TimecardLines", "lmlLaborHoursCalculated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TimecardLines", "lmlLaborHoursCalculated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
