using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.032", "Add fields to LotNumberTransactions table", "2015-05-01")]
public class v900032b
{
	public v900032b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtExpirationDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtExpirationDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
