using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.058", "Add fields to PartOrgReferences table", "2016-05-18")]
public class v91058i
{
	public v91058i(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartOrgReferences", "imzLeadTime"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOrgReferences", "imzLeadTime", "smallint", 3, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
