using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.50.018", "Add expiration date to serial numbers", "2009-12-16")]
public class v750018b
{
	public v750018b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumbers", "imsExpirationDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumbers", "imsExpirationDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
