using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.313", "Add fields to PAYROLLHEADERTOTALLINES table", "2015-05-19")]
public class v800313al
{
	public v800313al(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PAYROLLHEADERTOTALLINES", "paiProcessPayRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PAYROLLHEADERTOTALLINES", "paiProcessPayRate", "numeric", 8, 4, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
