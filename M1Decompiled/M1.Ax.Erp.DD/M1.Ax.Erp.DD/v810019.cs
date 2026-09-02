using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.019", "Change PartRules primary key.", "2013-01-30")]
public class v810019
{
	public v810019(DBConversionParms parms)
	{
		parms.Dmo.VerifyIndexesOnTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRules", parms.Messages, null);
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRules", "pcrMethodRuleID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRules", "pcrMethodRuleID", dropTriggers: true);
		}
	}
}
