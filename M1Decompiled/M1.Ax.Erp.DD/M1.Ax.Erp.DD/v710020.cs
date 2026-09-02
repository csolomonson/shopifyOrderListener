using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.020", "Add Inactive flag to Income Tax Tables", "2008-06-24")]
public class v710020
{
	public v710020(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTables", "pazInactive"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTables", "pazInactive", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTables", "pazInactiveDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTables", "pazInactiveDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
