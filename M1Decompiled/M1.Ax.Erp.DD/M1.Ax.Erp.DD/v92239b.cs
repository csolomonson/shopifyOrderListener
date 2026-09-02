using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.239", "Add fields to LandedCostChargeDetails table", "2017-05-04")]
public class v92239b
{
	public v92239b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "LandedCostChargeDetails"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostChargeDetails", new DmoField[12]
			{
				new DmoField("rmiLandedCostID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rmiLandedCostChargeID", "smallint", 3, 0, nullable: false),
				new DmoField("rmiLandedCostChargeDetailID", "int", 4, 0, nullable: false),
				new DmoField("rmiPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rmiPurchaseOrderLineID", "smallint", 4, 0, nullable: false),
				new DmoField("rmiTotalCost", "money", 12, 2, nullable: false),
				new DmoField("rmiTotalCostForeign", "money", 12, 2, nullable: false),
				new DmoField("rmiEstTotalCost", "money", 12, 2, nullable: false),
				new DmoField("rmiEstTotalCostForeign", "money", 12, 2, nullable: false),
				new DmoField("rmiCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("rmiCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("rmiUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("rmiLandedCostID,rmiLandedCostChargeID,rmiLandedCostChargeDetailID", unique: true),
				new DmoIndex("rmiUniqueID", unique: true)
			});
		}
	}
}
