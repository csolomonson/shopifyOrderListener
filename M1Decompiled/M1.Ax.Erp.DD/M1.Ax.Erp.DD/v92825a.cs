using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.825", "Add fields to IncomeTaxTableRevisions table", "2019-11-19")]
public class v92825a
{
	public v92825a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTableRevisions", "parStandardAdjustmentAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableRevisions", "parStandardAdjustmentAmount", "money", 10, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
