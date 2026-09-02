using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.051", "Add fields and modify LotNumberTransactions table", "2015-06-25")]
public class v900051c
{
	public v900051c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtLotNumberTransactionID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtLotNumberTransactionID", dropTriggers: true);
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtLotNumberTransactionID", "identity", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtJobMaterialComponentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtJobMaterialComponentID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "LotNumberTransactions", new DmoIndex[1]
		{
			new DmoIndex("ABTPARTID,ABTPARTREVISIONID,ABTPARTWAREHOUSELOCATIONID,ABTPARTBINID,ABTLOTNUMBERID,ABTLOTNUMBERTRANSACTIONID", unique: true)
		}, parms.Messages);
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "ABTLOTNUMBERTRANSACTIONID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", new DmoIndex[1]
			{
				new DmoIndex("ABTLOTNUMBERTRANSACTIONID", unique: true)
			}, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtOldTransactionType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtOldTransactionType", "tinyint", 2, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE LotNumberTransactions SET abtOldTransactionType = abtTransactionType, abtTransactionType =  CASE WHEN abtTransactionType = 2 And ((abtJobID <> '' And abtJobMaterialID <> 0) Or (imtJobID <> '' And imtJobMaterialID <> 0)) THEN 4  WHEN abtTransactionType = 5 And (abtJobID <> '' Or abtNonInventoryTransaction <> 0) THEN 40  WHEN abtTransactionType = 17 THEN 14  WHEN abtTransactionType = 4 And abtJobID <> '' and abtJobAssemblyID <> 0 And abtJobMaterialID = 0 THEN 20  WHEN abtTransactionType = 11 And abtJobID <> '' and abtJobMaterialID = 0 and abtInspectionID <> '' THEN 55 WHEN abtTransactionType = 11 And abtJobID <> '' and abtJobAssemblyID = 0 And abtJobMaterialID = 0 and imtJobOperationID <> 0 THEN 44 WHEN abtTransactionType = 11 And abtJobID <> '' and abtJobAssemblyID <> 0 And abtJobMaterialID = 0 and imtJobOperationID = 0 THEN 23  WHEN abtTransactionType = 11 And abtJobID <> '' And abtJobMaterialID <> 0 THEN 22  WHEN abtTransactionType = 11 And abtJobID = '' and abtInspectionID <> '' THEN 57 WHEN abtTransactionType = 11 And abtJobID = '' THEN 17   WHEN abtTransactionType = 12 And abtJobID <> '' and abtInspectionID <> '' THEN 54 WHEN abtTransactionType = 12 And abtJobID <> '' and abtJobAssemblyID = 0 And abtJobMaterialID = 0 THEN 43 WHEN abtTransactionType = 12 And abtJobID <> '' and abtJobAssemblyID <> 0 And abtJobMaterialID = 0 THEN 20  WHEN abtTransactionType = 12 And abtJobID <> '' And abtJobMaterialID <> 0 THEN 4  WHEN abtTransactionType = 12 And abtJobID = '' THEN 16  WHEN abtTransactionType = 13 And abtJobID <> '' and abtJobAssemblyID = 0 And abtJobMaterialID = 0 THEN 45 WHEN abtTransactionType = 13 And abtJobID <> '' and abtJobAssemblyID <> 0 And abtJobMaterialID = 0 THEN 25  WHEN abtTransactionType = 13 And abtJobID <> '' And abtJobMaterialID <> 0 THEN 24  WHEN abtTransactionType = 13 And abtJobID = '' THEN 18  WHEN abtTransactionType = 16 And abtJobID = '' THEN 5  WHEN abtTransactionType = 16 And abtJobID <> '' THEN 40  WHEN abtTransactionType = 5 And abtWarehouseTransferID <> '' And abtWarehouseTransferLineID <> 0 THEN 11  WHEN abtTransactionType = 2 And abtWarehouseReceiptID <> '' And abtWarehouseReceiptLineID <> 0 THEN 12  WHEN abtTransactionType = 9 And abtNegativeTransaction <> 0 THEN 10  WHEN abtTransactionType = 14 THEN 14  WHEN abtTransactionType = 10 THEN 26  WHEN abtTransactionType = 15 THEN 14 WHEN abtTransactionType = 6 And abtInventoryCountID <> 0 And abtPartTransactionID <> 0 And abtInProgress = 0 THEN 27 WHEN abtTransactionType = 4 And abtJobID = '' and abtJobAssemblyID = 0 And abtJobMaterialID = 0 And abtReceiptID = '' And abtInspectionID = '' THEN 21 WHEN abtTransactionType = 3 And imtScrapQuantity <> 0 And imtInventoryQuantityReceived = 0 And (abtJobID <> '' Or imtJobID <> '') THEN 23 WHEN abtTransactionType = 3 And imtScrapQuantity <> 0 And imtInventoryQuantityReceived <> 0 And(abtJobID <> '' Or imtJobID <> '')  And abtLotNumberTransactionID in (select abtLotNumberTransactionID from (select abtLotNumberTransactionID, row_number() over(order by abtPartTransactionID desc) as rn from LotNumberTransactions x where x.abtPartTransactionID = y.abtPartTransactionID) x where x.rn = 2) THEN 23 ELSE abtTransactionType END, abtTableName = CASE WHEN abtInspectionID <> '' THEN 'INSPECTIONLINES' WHEN abtTableName = '' And abtTransactionType = 4 And abtJobID <> '' And abtReceiptID = '' And abtInspectionID = '' THEN 'MATERIALISSUELINES' WHEN abtTableName = '' And abtTransactionType = 11 THEN 'MATERIALISSUELINES' ELSE abtTableName END FROM LotNumberTransactions y LEFT JOIN PartTransactions on abtPartTransactionID = imtPartTransactionID and abtPartTransactionID <> 0 WHERE abtOldTransactionType = 0");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update LotNumberTransactions set abtTransactionType = 42 From LotNumberTransactions Inner Join DMRShipmentLines on abtDMRShipmentID = dslDMRShipmentID and abtDMRShipmentLineID = dslDMRShipmentLineID  inner join DMRClaimLines on dmlDMRClaimID = DSLDMRCLAIMID and dmlDMRClaimLineID = DSLDMRCLAIMLINEID inner join InspectionLines on dmlInspectionID = qalInspectionID and dmlInspectionLineID = qalInspectionLineID where abtDMRShipmentID <> '' and qalQuantityToReturn <> 0");
	}
}
