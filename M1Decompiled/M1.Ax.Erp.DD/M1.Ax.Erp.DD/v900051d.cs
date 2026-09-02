using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.051", "Add fields and modify SerialNumberTransactions table", "2015-06-25")]
public class v900051d
{
	public v900051d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "sntSerialNumberTransactionID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntSerialNumberTransactionID", dropTriggers: true);
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntSerialNumberTransactionID", "identity", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "sntJobMaterialComponentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntJobMaterialComponentID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "sntQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SerialNumberTransactions Set sntQuantity = Case When sntNegativeTransaction = 0 Then 1 Else -1 End");
		}
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", new DmoIndex[1]
		{
			new DmoIndex("SNTPARTID,SNTPARTREVISIONID,SNTSERIALNUMBERID,SNTSERIALNUMBERTRANSACTIONID", unique: true)
		}, parms.Messages);
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "SNTSERIALNUMBERTRANSACTIONID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", new DmoIndex[1]
			{
				new DmoIndex("SNTSERIALNUMBERTRANSACTIONID", unique: true)
			}, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "sntOldTransactionType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntOldTransactionType", "tinyint", 2, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE SerialNumberTransactions SET sntOldTransactionType = sntTransactionType, sntTransactionType =  CASE WHEN sntTransactionType = 2 And ((sntJobID <> '' And sntJobMaterialID <> 0) Or (imtJobID <> '' And imtJobMaterialID <> 0)) THEN 4  WHEN sntTransactionType = 4 And sntJobID <> '' and sntJobAssemblyID <> 0 And sntJobMaterialID = 0 THEN 20  WHEN sntTransactionType = 4 And sntJobID = '' and sntJobAssemblyID = 0 And sntJobMaterialID = 0 THEN 21  WHEN sntTransactionType = 5 And (sntJobID <> '' Or imtNonInventoryTransaction <> 0) THEN 40  WHEN sntTransactionType = 5 And sntDMRShipmentID <> '' THEN 62 WHEN sntTransactionType = 17 And sntJobID <> '' and sntJobAssemblyID = 0 And sntJobMaterialID = 0 and imtJobOperationID <> 0 THEN 44 WHEN sntTransactionType = 17 And sntJobID <> '' and (imtJobOperationID = 0 OR sntInspectionID <> '') And sntJobMaterialID = 0 THEN 23  WHEN sntTransactionType = 17 And sntJobID <> '' And sntJobMaterialID <> 0 THEN 22  WHEN sntTransactionType = 17 And sntJobID = '' THEN 17  WHEN sntTransactionType = 16 And sntJobID <> '' and sntJobAssemblyID = 0 And sntJobMaterialID = 0 THEN 43 WHEN sntTransactionType = 16 And sntJobID <> '' and sntJobAssemblyID <> 0 And sntJobMaterialID = 0 THEN 20  WHEN sntTransactionType = 16 And sntJobID <> '' And sntJobMaterialID <> 0 THEN 4  WHEN sntTransactionType = 16 And sntJobID = '' THEN 16  WHEN sntTransactionType = 18 And sntJobID <> '' and sntJobAssemblyID = 0 And sntJobMaterialID = 0 THEN 45 WHEN sntTransactionType = 18 And sntJobID <> '' and sntJobAssemblyID <> 0 And sntJobMaterialID = 0 THEN 25  WHEN sntTransactionType = 18 And sntJobID <> '' And sntJobMaterialID <> 0 THEN 24  WHEN sntTransactionType = 18 And sntJobID = '' THEN 18 WHEN sntTransactionType = 6 And sntInventoryCountID <> 0 And sntPartTransactionID <> 0 THEN 27 ELSE sntTransactionType\tEND FROM SerialNumberTransactions LEFT JOIN PartTransactions on sntPartTransactionID = imtPartTransactionID and sntPartTransactionID <> 0 WHERE sntOldTransactionType = 0");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SerialNumberTransactions set sntTransactionType = 42 From SerialNumberTransactions Inner Join DMRShipmentLines on sntDMRShipmentID = dslDMRShipmentID and sntDMRShipmentLineID = dslDMRShipmentLineID  inner join DMRClaimLines on dmlDMRClaimID = DSLDMRCLAIMID and dmlDMRClaimLineID = DSLDMRCLAIMLINEID inner join InspectionLines on dmlInspectionID = qalInspectionID and dmlInspectionLineID = qalInspectionLineID where sntDMRShipmentID <> '' and qalQuantityToReturn <> 0");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SerialNumberTransactions Set sntTransactionType = Case When sntTransactionType = 43 Then 53 When sntTransactionType = 45 Then 58 When sntTransactionType = 44 Then 57 Else sntTransactionType End From SerialNumberTransactions inner join InspectionLines on sntInspectionID = qalInspectionID and sntInspectionLineID = qalInspectionLineID where qalInspectionType = 3 and sntTransactionType IN (43, 44, 45)");
	}
}
