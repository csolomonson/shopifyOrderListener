using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.108", "Drop expiration date from LotNumberTransactions table", "2015-11-25")]
public class v900108a
{
	public v900108a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtExpirationDate"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtExpirationDate", dropTriggers: true);
		}
	}
}
