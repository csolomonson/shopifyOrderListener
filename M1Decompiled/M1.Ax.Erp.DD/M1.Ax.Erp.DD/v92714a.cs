using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.714", "Update BinQuantityOnHand with correct Conversion Factor", "2018-05-28")]
public class v92714a
{
	public v92714a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PartBins SET imbBinQuantityOnHand = imbQuantityOnHand / imbConversionFactor WHERE imbConversionFactor NOT IN (0, 1) AND imbQuantityOnHand <> 0");
	}
}
