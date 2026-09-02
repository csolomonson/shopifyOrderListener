using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.400", "Add field smpFlaggedforWMS to Shipments table", "2024-08-30")]
public class v97400d
{
	public v97400d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Shipments", "smpFlaggedforWMS"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Shipments", "smpFlaggedforWMS", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
