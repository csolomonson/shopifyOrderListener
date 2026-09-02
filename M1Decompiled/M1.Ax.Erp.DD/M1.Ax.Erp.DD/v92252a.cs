using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.252", "Add fields to LandedCosts table", "2017-05-04")]
public class v92252a
{
	public v92252a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCosts", "rmcChargesJournalsCreated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCosts", "rmcChargesJournalsCreated", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCosts", "rmcPOInTransitJournalsCreated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCosts", "rmcPOInTransitJournalsCreated", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
