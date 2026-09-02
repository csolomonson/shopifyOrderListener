using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.145", "Add Landed Costs fields for Reversal Entries", "2008-08-12")]
public class v710145h
{
	public v710145h(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCosts", "rmcReversalEntry"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCosts", "rmcReversalEntry", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCosts", "rmcReverseLandedCostID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCosts", "rmcReverseLandedCostID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCosts", "rmcReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCosts", "rmcReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostCharges", "rmhReverseLandedCostID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCharges", "rmhReverseLandedCostID", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostCharges", "rmhReverseLandedCostID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCharges", "rmhReverseLandedCostID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostCharges", "rmhReverseLandedCostChargeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCharges", "rmhReverseLandedCostChargeID", "numeric", 3, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostCharges", "rmhReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCharges", "rmhReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
