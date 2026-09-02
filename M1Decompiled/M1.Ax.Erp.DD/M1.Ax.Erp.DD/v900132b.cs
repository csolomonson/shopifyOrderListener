using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.132", "Creation of material issues and mfg/misc receipts from conversion data", "2016-02-17")]
public class v900132b
{
	public v900132b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartTransactions") || !parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MaterialIssues") || !parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MfgReceipts"))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin");
		stringBuilder.AppendLine("SET NOCOUNT ON");
		stringBuilder.AppendLine("DECLARE");
		stringBuilder.AppendLine("@SQL varchar(8000)");
		stringBuilder.AppendLine("set @sql = '");
		stringBuilder.AppendLine("select x.part_trans_id, x.imtUniqueID as unique_id, x.tran_date, x.ordered_rank, 0 as parent_id, x.child_id into Temp_Transactions from (");
		stringBuilder.AppendLine("select imtparttransactionid as part_trans_id, imtUniqueID, DATEADD(dd, DATEDIFF(dd, 0, imtTransactionDate), 0) as tran_Date");
		stringBuilder.AppendLine(", 1 + (row_number() over(partition by DATEADD(dd, DATEDIFF(dd, 0, imtTransactionDate), 0) order by imtTransactionDate asc) - 1) % 32767 as child_id");
		stringBuilder.AppendLine(",row_number() over(order by DATEADD(dd, DATEDIFF(dd, 0, imtTransactionDate), 0)) as ordered_rank");
		stringBuilder.AppendLine("from PartTransactions where imtSource = 3 and imtTransactionType = 2 and imtTableName = ''''");
		stringBuilder.AppendLine(") x");
		stringBuilder.AppendLine("ALTER TABLE Temp_Transactions ADD PRIMARY KEY NONCLUSTERED (part_trans_id)");
		stringBuilder.AppendLine("-- cursor to update parent id");
		stringBuilder.AppendLine("SET NOCOUNT ON");
		stringBuilder.AppendLine("DECLARE");
		stringBuilder.AppendLine("@part_trans_id      AS INT,");
		stringBuilder.AppendLine("@ordered_rank       AS INT,");
		stringBuilder.AppendLine("@parent_id          AS INT,");
		stringBuilder.AppendLine("@child_id           AS INT,");
		stringBuilder.AppendLine("@temp_parent        AS INT");
		stringBuilder.AppendLine("SET @temp_parent = (Select ISNULL(MAX(iniMaterialIssueID), 0) + 1 from MaterialIssues)");
		stringBuilder.AppendLine("DECLARE TRANS_CURSOR CURSOR FOR");
		stringBuilder.AppendLine("SELECT part_trans_id, ordered_rank, parent_id, child_id FROM Temp_Transactions ORDER BY ordered_rank ASC FOR UPDATE OF parent_id");
		stringBuilder.AppendLine("OPEN TRANS_CURSOR");
		stringBuilder.AppendLine("FETCH NEXT FROM TRANS_CURSOR");
		stringBuilder.AppendLine("INTO @part_trans_id, @ordered_rank, @parent_id, @child_id");
		stringBuilder.AppendLine("WHILE (@@FETCH_STATUS = 0)");
		stringBuilder.AppendLine("BEGIN");
		stringBuilder.AppendLine("IF @child_id = 1");
		stringBuilder.AppendLine("SELECT @temp_parent = @temp_parent + 1");
		stringBuilder.AppendLine("UPDATE Temp_Transactions SET parent_id = @temp_parent WHERE CURRENT OF TRANS_CURSOR");
		stringBuilder.AppendLine("FETCH NEXT FROM TRANS_CURSOR");
		stringBuilder.AppendLine("INTO @part_trans_id, @ordered_rank, @parent_id, @child_id");
		stringBuilder.AppendLine("END");
		stringBuilder.AppendLine("CLOSE TRANS_CURSOR");
		stringBuilder.AppendLine("DEALLOCATE TRANS_CURSOR");
		stringBuilder.AppendLine("SET NOCOUNT OFF");
		stringBuilder.AppendLine("-- insert for material issue header");
		stringBuilder.AppendLine("Insert Into MaterialIssues(iniMaterialIssueID, iniMaterialIssueDate, iniPosted, iniPostedDate, iniCreatedBy, iniCreatedDate)");
		stringBuilder.AppendLine("Select parent_id, GETDATE(), 1, GETDATE(), ''CONVERSION'', GETDATE()");
		stringBuilder.AppendLine("from Temp_Transactions group by parent_id");
		stringBuilder.AppendLine("-- insert for material issue lines");
		stringBuilder.AppendLine("Insert Into MaterialIssueLines(injMaterialIssueID, injMaterialIssueLineID, injIssueType, injJobID, injJobAssemblyID, injJobType, injJobMaterialID, injIssueComplete, injPartID, injPartRevisionID, injPartWarehouseLocationID, injPartBinID,");
		stringBuilder.AppendLine("injKitPart, injInvIssueQuantity, injInvScrapQuantity, injJobMatIssueQuantity, injJobMatScrapQuantity, injJobAsmIssueQuantity, injJobAsmScrapQuantity, injMiscIssueReasonID, injPlantID, injProjectID, injProjectAreaID, injPosted, injCreatedBy, injCreatedDate)");
		stringBuilder.AppendLine("Select parent_id, child_id, CASE WHEN imtJobID = '''' THEN 2 ELSE 1 END, imtJobID, imtJobAssemblyID, CASE WHEN imtJobID = '''' THEN 0 ELSE CASE WHEN imtJobMaterialID <> 0 THEN 1 ELSE 3 END END, imtJobMaterialID, imtJobCompleteStatus, imtPartID, imtPartRevisionID, imtPartWarehouseLocationID, imtPartBinID, 0,");
		stringBuilder.AppendLine("CASE WHEN imtIssueType = 2 THEN imtInventoryQuantityReceived ELSE 0 END, CASE WHEN imtIssueType = 2 THEN imtScrapQuantity ELSE 0 END,");
		stringBuilder.AppendLine("CASE WHEN imtIssueType = 1 THEN CASE WHEN imtJobType = 1 THEN imtInventoryQuantityReceived ELSE 0 END ELSE 0 END,");
		stringBuilder.AppendLine("CASE WHEN imtIssueType = 1 THEN CASE WHEN imtJobType = 1 THEN imtScrapQuantity ELSE 0 END ELSE 0 END,");
		stringBuilder.AppendLine("CASE WHEN imtIssueType = 1 THEN CASE WHEN imtJobMaterialID = 0 THEN imtInventoryQuantityReceived ELSE 0 END ELSE 0 END,");
		stringBuilder.AppendLine("CASE WHEN imtIssueType = 1 THEN CASE WHEN imtJobMaterialID = 0 THEN imtScrapQuantity ELSE 0 END ELSE 0 END, imtMiscIssueReasonID, imtPlantID, imtProjectID, imtProjectAreaID, 1, ''CONVERSION'', GETDATE()");
		stringBuilder.AppendLine("from Temp_Transactions inner join PartTransactions on imtPartTransactionID = part_trans_id");
		stringBuilder.AppendLine("-- update source table name");
		stringBuilder.AppendLine("Update PartTransactions Set imtTableName = ''MaterialIssueLines'', imtTableUniqueID = injUniqueID");
		stringBuilder.AppendLine("from MaterialIssueLines inner join Temp_Transactions on injMaterialIssueID = parent_id and injMaterialIssueLineID = child_id inner join PartTransactions on imtPartTransactionID = part_trans_id");
		stringBuilder.AppendLine("drop table Temp_Transactions");
		stringBuilder.AppendLine("'");
		stringBuilder.AppendLine("exec(@sql) ");
		stringBuilder.AppendLine("SET NOCOUNT OFF");
		stringBuilder.AppendLine("end;");
		try
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
		}
		finally
		{
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Temp_Transactions"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "Temp_Transactions");
			}
		}
		stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin  ");
		stringBuilder.AppendLine("SET NOCOUNT ON  ");
		stringBuilder.AppendLine("DECLARE  ");
		stringBuilder.AppendLine("@starting_id        AS INT,  ");
		stringBuilder.AppendLine("@SQL3 varchar(8000) ");
		stringBuilder.AppendLine("set @starting_id = (Select ISNULL(MAX(rmmMfgReceiptID), 10000) + 1 from MfgReceipts) ");
		stringBuilder.AppendLine("set @sql3 = ' ");
		stringBuilder.AppendLine("Select IDENTITY(INT,' + CAST(@starting_id as varchar) + ', 1) as rmmMfgReceiptID, rmmReceiptType, rmmReceiptDate, rmmJobID, rmmJobAssemblyID, rmmJobMaterialID, rmmJobOperationID, rmmJobType, rmmPartID,  ");
		stringBuilder.AppendLine("rmmPartRevisionID, rmmPartWarehouseLocationID, rmmPartBinID, rmmInventoryQuantityReceived, rmmMiscInvQuantityReceived, rmmJobMatQuantityReceived, rmmJobOprQuantityReceived, rmmQuantityToInspect, rmmScrapQuantity,  ");
		stringBuilder.AppendLine("rmmInventoryUnitOfMeasure, rmmReference, rmmRequiresInspection, rmmHeatLot, rmmPlantID, rmmProjectID, rmmProjectAreaID,  ");
		stringBuilder.AppendLine("rmmPosted, rmmPostedDate, rmmCreatedBy, rmmCreatedDate, PartTransactionID, rmmUnitLaborCost, rmmUnitOverheadCost, rmmUnitMaterialCost, rmmUnitSubcontractCost ");
		stringBuilder.AppendLine("Into Temp_MfgReceipts From ");
		stringBuilder.AppendLine("( ");
		stringBuilder.AppendLine("----------mfg receipts ");
		stringBuilder.AppendLine("Select 3 as rmmReceiptType, imtTransactionDate as rmmReceiptDate, imtJobID as rmmJobID, imtJobAssemblyID as rmmJobAssemblyID, 0 as rmmJobMaterialID, 0 as rmmJobOperationID, 3 as rmmJobType, imtPartID as rmmPartID,  ");
		stringBuilder.AppendLine("imtPartRevisionID as rmmPartRevisionID, imtPartWarehouseLocationID as rmmPartWarehouseLocationID, imtPartBinID as rmmPartBinID, Case when imtRequiresInspection <> 0 Then 0 Else imtInventoryQuantityReceived End as rmmInventoryQuantityReceived, 0 as rmmMiscInvQuantityReceived, 0 as rmmJobMatQuantityReceived, 0 as rmmJobOprQuantityReceived, Case when imtRequiresInspection = 0 Then 0 Else imtInventoryQuantityReceived End As rmmQuantityToInspect, imtScrapQuantity as rmmScrapQuantity,  ");
		stringBuilder.AppendLine("imtInventoryUnitOfMeasure as rmmInventoryUnitOfMeasure, imtReference as rmmReference, imtRequiresInspection as rmmRequiresInspection, imtHeatLot as rmmHeatLot, imtPlantID as rmmPlantID, imtProjectID as rmmProjectID, imtProjectAreaID as rmmProjectAreaID,  ");
		stringBuilder.AppendLine("1 as rmmPosted, GETDATE() as rmmPostedDate, ''CONVERSION'' as rmmCreatedBy, GETDATE() As rmmCreatedDate, imtPartTransactionID as PartTransactionID, imtUnitLaborCost as rmmUnitLaborCost, imtUnitOverheadCost as rmmUnitOverheadCost, imtUnitMaterialCost as rmmUnitMaterialCost, imtUnitSubcontractCost as rmmUnitSubcontractCost ");
		stringBuilder.AppendLine("From ");
		stringBuilder.AppendLine("PartTransactions where imtSource = 1 and imtTransactionType = 1 and imtJobID <> '''' and imtTableName = '''' ");
		stringBuilder.AppendLine("Union All ");
		stringBuilder.AppendLine("-- misc receipts ");
		stringBuilder.AppendLine("Select imtReceiptType as rmmReceiptType, imtTransactionDate as rmmReceiptDate, imtJobID as rmmJobID, imtJobAssemblyID as rmmJobAssemblyID, imtJobMaterialID as rmmJobMaterialID, imtJobOperationID as rmmJobOperationID, imtJobType as rmmJobType, imtPartID as rmmPartID,  ");
		stringBuilder.AppendLine("imtPartRevisionID as rmmPartRevisionID, imtPartWarehouseLocationID as rmmPartWarehouseLocationID, imtPartBinID as rmmPartBinID, 0 as rmmInventoryQuantityReceived, Case When imtReceiptType = 1 Then 0 Else imtInventoryQuantityReceived End as rmmMiscInvQuantityReceived, Case When imtReceiptType = 2 Then 0 Else Case When imtJobType = 1 Then imtInventoryQuantityReceived Else 0 End End as rmmJobMatQuantityReceived, Case When imtReceiptType = 2 Then 0 Else Case When imtJobType = 2 Then imtInventoryQuantityReceived Else 0 End End as rmmJobOprQuantityReceived, 0 as rmmQuantityToInspect, imtScrapQuantity as rmmScrapQuantity,  ");
		stringBuilder.AppendLine("imtInventoryUnitOfMeasure as rmmInventoryUnitOfMeasure, imtReference as rmmReference, imtRequiresInspection as rmmRequiresInspection, imtHeatLot as rmmHeatLot, imtPlantID as rmmPlantID, imtProjectID as rmmProjectID, imtProjectAreaID as rmmProjectAreaID,  ");
		stringBuilder.AppendLine("1 as rmmPosted, GETDATE() as rmmPostedDate, ''CONVERSION'' as rmmCreatedBy, GETDATE() As rmmCreatedDate, imtPartTransactionID as PartTransactionID, imtUnitLaborCost as rmmUnitLaborCost, imtUnitOverheadCost as rmmUnitOverheadCost, imtUnitMaterialCost as rmmUnitMaterialCost, imtUnitSubcontractCost as rmmUnitSubcontractCost ");
		stringBuilder.AppendLine("From ");
		stringBuilder.AppendLine("PartTransactions where imtSource = 2 and imtTransactionType = 1 and imtIssueType = 0 and imtReceiptID = '''' and imtPurchaseOrderID = '''' and imtTableName = '''' ");
		stringBuilder.AppendLine(") x ");
		stringBuilder.AppendLine("insert into MfgReceipts(rmmMfgReceiptID, rmmReceiptType, rmmReceiptDate, rmmJobID, rmmJobAssemblyID, rmmJobMaterialID, rmmJobOperationID, rmmJobType, rmmPartID,  ");
		stringBuilder.AppendLine("rmmPartRevisionID, rmmPartWarehouseLocationID, rmmPartBinID, rmmInventoryQuantityReceived, rmmMiscInvQuantityReceived, rmmJobMatQuantityReceived, rmmJobOprQuantityReceived, rmmQuantityToInspect, rmmScrapQuantity,  ");
		stringBuilder.AppendLine("rmmInventoryUnitOfMeasure, rmmReference, rmmRequiresInspection, rmmHeatLot, rmmPlantID, rmmProjectID, rmmProjectAreaID,  ");
		stringBuilder.AppendLine("rmmPosted, rmmPostedDate, rmmCreatedBy, rmmCreatedDate, rmmUnitLaborCost, rmmUnitOverheadCost, rmmUnitMaterialCost, rmmUnitSubcontractCost) ");
		stringBuilder.AppendLine("Select rmmMfgReceiptID, rmmReceiptType, rmmReceiptDate, rmmJobID, rmmJobAssemblyID, rmmJobMaterialID, rmmJobOperationID, rmmJobType, rmmPartID, ");
		stringBuilder.AppendLine("rmmPartRevisionID, rmmPartWarehouseLocationID, rmmPartBinID, rmmInventoryQuantityReceived, rmmMiscInvQuantityReceived, rmmJobMatQuantityReceived, rmmJobOprQuantityReceived, rmmQuantityToInspect, rmmScrapQuantity, ");
		stringBuilder.AppendLine("rmmInventoryUnitOfMeasure, rmmReference, rmmRequiresInspection, rmmHeatLot, rmmPlantID, rmmProjectID, rmmProjectAreaID, ");
		stringBuilder.AppendLine("rmmPosted, rmmPostedDate, rmmCreatedBy, rmmCreatedDate, rmmUnitLaborCost, rmmUnitOverheadCost, rmmUnitMaterialCost, rmmUnitSubcontractCost from Temp_MfgReceipts ");
		stringBuilder.AppendLine("Update PartTransactions Set imtTableName = ''MfgReceipts'', imtTableUniqueID = a.rmmUniqueID ");
		stringBuilder.AppendLine("from MfgReceipts a inner ");
		stringBuilder.AppendLine("join Temp_MfgReceipts b on a.rmmMfgReceiptID = b.rmmMfgReceiptID inner join PartTransactions on imtPartTransactionID = PartTransactionID ");
		stringBuilder.AppendLine("Drop Table Temp_MfgReceipts ");
		stringBuilder.AppendLine("' ");
		stringBuilder.AppendLine("exec(@sql3) ");
		stringBuilder.AppendLine("SET NOCOUNT OFF ");
		stringBuilder.AppendLine("end");
		try
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
		}
		finally
		{
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Temp_MfgReceipts"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "Temp_MfgReceipts");
			}
		}
	}
}
