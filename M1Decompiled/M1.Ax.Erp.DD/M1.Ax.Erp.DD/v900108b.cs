using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.108", "Drop expiration date from SerialNumberTransactions table", "2015-11-25")]
public class v900108b
{
	public v900108b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "sntExpirationDate"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntExpirationDate", dropTriggers: true);
		}
	}
}
