using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.032", "Add fields to SerialNumberTransactions table", "2015-05-01")]
public class v900032c
{
	public v900032c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "sntExpirationDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntExpirationDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
