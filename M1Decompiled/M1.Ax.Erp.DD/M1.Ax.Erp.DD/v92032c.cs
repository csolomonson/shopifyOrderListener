using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.032", "Add fields to Shipments table", "2016-11-24")]
public class v92032c
{
	public v92032c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Shipments", "smpReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Shipments", "smpReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
