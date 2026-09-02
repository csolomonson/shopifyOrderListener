using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Add Working Holiday Maker field to STPLine table", "2022-02-16")]
public class v95200s
{
	public v95200s(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPLines", "stlWorkingHolidayMaker"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", "stlWorkingHolidayMaker", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
