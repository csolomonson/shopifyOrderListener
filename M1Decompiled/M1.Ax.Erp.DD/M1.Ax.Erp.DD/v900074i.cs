using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Add fields to InspectionLines table", "2015-08-14")]
public class v900074i
{
	public v900074i(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalQuantityToScrap"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalQuantityToScrap", "qalMfgReceiptQuantityToScrap", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalQuantityToReturn"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalQuantityToReturn", "qalMfgReceiptQuantityToReturn", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalQuantityAccepted"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalQuantityAccepted", "qalMfgReceiptQuantityAccepted", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalInvQuantityToScrap"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalInvQuantityToScrap", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalInvQuantityAccepted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalInvQuantityAccepted", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalInvQuantityToReturn"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalInvQuantityToReturn", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalInvQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalInvQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalInvQuantityToInspect"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InspectionLines Set qalInvQuantityToInspect = CASE WHEN qalQuantityToInspect-qalInvQuantityAccepted-qalInvQuantityToScrap-qalInvQuantityToReturn <= 0 OR qalInspectionComplete <> 0 THEN 0 ELSE qalQuantityToInspect-qalInvQuantityAccepted-qalInvQuantityToScrap-qalInvQuantityToReturn END");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InspectionLines Set qalInvQuantityAccepted = CASE WHEN qalInspectionType <> 1 THEN 0 ELSE qalMfgReceiptQuantityAccepted END, qalInvQuantityToScrap = CASE WHEN qalInspectionType <> 1 THEN 0 ELSE qalMfgReceiptQuantityToScrap END, qalInvQuantityToReturn = CASE WHEN qalInspectionType <> 1 THEN 0 ELSE qalMfgReceiptQuantityToReturn END");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InspectionLines Set qalMfgReceiptQuantityAccepted = CASE WHEN qalInspectionType <> 3 THEN 0 ELSE qalMfgReceiptQuantityAccepted END, qalMfgReceiptQuantityToScrap = CASE WHEN qalInspectionType <> 3 THEN 0 ELSE qalMfgReceiptQuantityToScrap END, qalMfgReceiptQuantityToReturn = CASE WHEN qalInspectionType <> 3 THEN 0 ELSE qalMfgReceiptQuantityToReturn END");
		}
	}
}
