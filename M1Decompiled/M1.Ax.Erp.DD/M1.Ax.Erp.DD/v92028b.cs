using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.028", "Add fields to DMRShipments table", "2016-11-22")]
public class v92028b
{
	public v92028b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DMRShipments", "dspReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipments", "dspReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
