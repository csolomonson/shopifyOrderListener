using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.092", "Add fields to FinancialProperties table", "2017-02-02")]
public class v92092a
{
	public v92092a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafCOGSStatusHistory"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafCOGSStatusHistory", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
