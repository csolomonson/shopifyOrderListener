using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.135", "Add Retention options to Organizations", "2008-09-09")]
public class v710135
{
	public v710135(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoAPIncludeTaxInRetention"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoAPIncludeTaxInRetention", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoARIncludeTaxInRetention"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoARIncludeTaxInRetention", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
