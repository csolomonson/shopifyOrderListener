using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.188", "Add fields to PurchasePlannerSessions table", "2017-03-10")]
public class v92188a
{
	public v92188a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsSupplierIDs"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", "ppsSupplierIDs", "nvarchar(max)", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsPartIDs"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", "ppsPartIDs", "nvarchar(max)", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
