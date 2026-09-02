using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.336", "Add fields to INCOMETAXTABLEREVISIONS table", "2015-05-19")]
public class v800336x
{
	public v800336x(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "INCOMETAXTABLEREVISIONS", "parUseYTDAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "INCOMETAXTABLEREVISIONS", "parUseYTDAmount", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "INCOMETAXTABLEREVISIONS", "parTaxAbatementPercent"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "INCOMETAXTABLEREVISIONS", "parTaxAbatementPercent", "numeric", 6, 3, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
