using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.301", "Add new field for directors fees on STP Lines Entry (STP)", "2022-07-11")]
public class v95301b
{
	public v95301b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPLines", "stlDirectorsFees"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", "stlDirectorsFees", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
