using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.172", "Add fields to RMAClaimLines table", "2016-05-02")]
public class v900172a
{
	public v900172a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralCustomerPO"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralCustomerPO", "nvarchar", 40, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
