using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add source and quantity job fields to InspectionLines table", "2014-09-25")]
public class v900003m
{
	public v900003m(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobOprQuantityRejected"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobOprQuantityRejected", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobOprQuantityAccepted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobOprQuantityAccepted", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobOprQuantityToScrap"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobOprQuantityToScrap", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobMatQuantityToScrap"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobMatQuantityToScrap", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobOprQuantityToReturn"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobOprQuantityToReturn", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalInspectionType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalInspectionType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobMaterialID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobMaterialID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobOperationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobOperationID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobID", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobMatQuantityToReturn"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobMatQuantityToReturn", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobAssemblyID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobAssemblyID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobMatQuantityRejected"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobMatQuantityRejected", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InspectionLines", "qalJobMatQuantityAccepted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLines", "qalJobMatQuantityAccepted", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "QualityRegisters"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InspectionLines Set qalJobID = qanJobID, qalJobAssemblyID = qanJobAssemblyID, qalJobMaterialID = qanJobMaterialID, qalJobOperationID = qanJobOperationID, qalJobType = Case When qanJobMaterialID <> 0 Then 1 When qanJobOperationID <> 0 Then 2 Else 0 End, qalInspectionType = Case When qanJobID = '' Then 1 Else 2 End From InspectionLines Inner Join QualityRegisters on qalQualityRegisterID = qanQualityRegisterID");
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InspectionLines Set qalInspectionType = 1 Where qalInspectionType = 0");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InspectionLines Set qalInspectionType = 3 Where qalJobID <> '' And qalJobMaterialID = 0 And qalJobOperationID = 0");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InspectionLines Set qalJobMatQuantityAccepted = Case When qalJobType = 1 Then qalQuantityAccepted Else 0 End, qalJobMatQuantityRejected = Case When qalJobType = 1 Then qalQuantityRejected Else 0 End, qalJobMatQuantityToScrap = Case When qalJobType = 1 Then qalQuantityToScrap Else 0 End, qalJobMatQuantityToReturn = Case When qalJobType = 1 Then qalQuantityToReturn Else 0 End, qalJobOprQuantityAccepted = Case When qalJobType = 2 Then qalQuantityAccepted Else 0 End, qalJobOprQuantityRejected = Case When qalJobType = 2 Then qalQuantityRejected Else 0 End, qalJobOprQuantityToScrap = Case When qalJobType = 2 Then qalQuantityToScrap Else 0 End, qalJobOprQuantityToReturn = Case When qalJobType = 2 Then qalQuantityToReturn Else 0 End, qalQuantityAccepted = Case When qalJobType <> 0 Then 0 Else qalQuantityAccepted End, qalQuantityRejected = Case When qalJobType <> 0 Then 0 Else qalQuantityRejected End, qalQuantityToScrap = Case When qalJobType <> 0 Then 0 Else qalQuantityToScrap End, qalQuantityToReturn = Case When qalJobType <> 0 Then 0 Else qalQuantityToReturn End");
	}
}
