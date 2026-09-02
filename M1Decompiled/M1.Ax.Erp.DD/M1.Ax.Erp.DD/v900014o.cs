using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to NonConformances table", "2014-12-15")]
public class v900014o
{
	public v900014o(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "NonConformances", "qarRMAClaimID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "NonConformances", "qarRMAClaimID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "NonConformances", "qarRMAClaimLineID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "NonConformances", "qarRMAClaimLineID", "smallint", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "NonConformances", "qarSubcontractAmountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "NonConformances", "qarSubcontractAmountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
