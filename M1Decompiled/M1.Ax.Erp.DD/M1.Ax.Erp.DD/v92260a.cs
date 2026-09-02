using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.260", "Add fields to FinancialProperties table", "2017-05-22")]
public class v92260a
{
	public v92260a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafRoundingGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafRoundingGLAccountID", "nvarchar", 11, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
