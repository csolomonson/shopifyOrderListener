using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.032", "Add fields to LotNumbers table", "2015-05-01")]
public class v900032d
{
	public v900032d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablExpirationDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablExpirationDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
