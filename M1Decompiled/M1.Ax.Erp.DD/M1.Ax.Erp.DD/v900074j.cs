using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Add fields to JobMaterialComponents table", "2015-08-14")]
public class v900074j
{
	public v900074j(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterialComponents", "jmtQuantityToReturn"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterialComponents", "jmtQuantityToReturn", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterialComponents", "jmtScrapQuantityReceived"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterialComponents", "jmtScrapQuantityReceived", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterialComponents", "jmtQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterialComponents", "jmtQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
