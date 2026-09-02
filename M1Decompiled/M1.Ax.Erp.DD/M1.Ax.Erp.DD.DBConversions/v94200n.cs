using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.200", "Add fields to ProductionProperties table", "2021-09-01")]
public class v94200n
{
	public v94200n(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapSMEDI856CustomLabel"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapSMEDI856CustomLabel", "nvarchar", 35, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
