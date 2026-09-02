using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.605", "Add fields to ProductionProperties table", "2017-12-21")]
public class v92605a
{
	public v92605a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapDateToSchedule"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapDateToSchedule", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
