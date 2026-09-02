using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.075", "Add fields to PurchasePlannerRequirements table", "2016-06-14")]
public class v91075f
{
	public v91075f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerRequirements", "pprSource"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerRequirements", "pprSource", "nvarchar", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
