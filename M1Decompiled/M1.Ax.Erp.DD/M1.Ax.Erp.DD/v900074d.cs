using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Add fields to InspectionComponents table", "2015-08-14")]
public class v900074d
{
	public v900074d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamInvQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionComponents", "qamInvQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamInspectionType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionComponents", "qamInspectionType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionComponents", "qamInvQuantityToInspect"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InspectionComponents Set qamInvQuantityToInspect = CASE WHEN qamParentQtyToInspect-qamInvQuantityAccepted-qamInvQuantityToScrap-qamInvQuantityToReturn <= 0 OR qamInspectionComplete <> 0 THEN 0 ELSE qamParentQtyToInspect-qamInvQuantityAccepted-qamInvQuantityToScrap-qamInvQuantityToReturn END");
		}
	}
}
