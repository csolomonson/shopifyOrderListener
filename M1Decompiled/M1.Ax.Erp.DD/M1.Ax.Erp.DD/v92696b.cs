using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.696", "Add fields to ProductionProperties table", "2018-04-29")]
public class v92696b
{
	public v92696b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapQAInspQueueRefreshInterval"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapQAInspQueueRefreshInterval", "smallint", 3, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
