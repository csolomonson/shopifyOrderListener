using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.140", "Move landed cost method to charges table", "2008-09-25")]
public class v710140b
{
	public v710140b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostCharges", "rmhLandedCostMethod"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCharges", "rmhLandedCostMethod", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update LandedCostCharges Set rmhLandedCostMethod=rmcLandedCostMethod From LandedCosts Inner Join LandedCostCharges On rmcLandedCostID=rmhLandedCostID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCosts", "rmcLandedCostMethod"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCosts", "rmcLandedCostMethod", dropTriggers: true);
		}
	}
}
