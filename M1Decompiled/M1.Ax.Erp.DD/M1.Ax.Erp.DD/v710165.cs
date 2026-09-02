using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.165", "Add default cost method to landed cost categories", "2008-10-16")]
public class v710165
{
	public v710165(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostCategories", "rmaLandedCostMethod"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCategories", "rmaLandedCostMethod", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
