using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.237", "Add fields to FinancialProperties table", "2017-05-03")]
public class v92237a
{
	public v92237a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafARLaborPartRevisionID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafARLaborPartRevisionID", "nvarchar", 15, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafARLaborPartID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafARLaborPartID", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
