using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.532", "Add fields to PLANTS table", "2015-05-19")]
public class v800532ap
{
	public v800532ap(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PLANTS", "xauCountryCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PLANTS", "xauCountryCode", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
