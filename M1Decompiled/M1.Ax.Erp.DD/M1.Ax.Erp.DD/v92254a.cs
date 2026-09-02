using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.254", "Add fields to LandedCostChargeDetails table", "2017-05-15")]
public class v92254a
{
	public v92254a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostChargeDetails", "rmiEstTotalCostForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostChargeDetails", "rmiEstTotalCostForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostChargeDetails", "rmiEstTotalCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostChargeDetails", "rmiEstTotalCost", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
