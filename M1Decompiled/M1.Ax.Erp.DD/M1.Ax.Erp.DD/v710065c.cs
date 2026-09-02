using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.065", "Add Price Source fields to QuoteMaterials", "2008-07-24")]
public class v710065c
{
	public v710065c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmSourcePriceID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmSourcePriceID", "numeric", 9, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmSourceRFQID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmSourceRFQID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
