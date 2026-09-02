using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.512a", "Add fields to LandedCosts table", "2017-09-04")]
public class v92512a
{
	public v92512a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCosts", "rmcLandedCostReceiptsTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCosts", "rmcLandedCostReceiptsTotal", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
