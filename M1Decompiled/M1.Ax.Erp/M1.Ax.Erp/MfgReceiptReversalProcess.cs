using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class MfgReceiptReversalProcess : ProcessParameters
{
	public MfgReceiptReversalProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "rmmMfgReceiptID" };
		PromptFieldAllowMultiples = true;
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[1] { "rmmMfgReceiptID" };
		KeyValueTableName = "MfgReceipts";
		Description = "Use this screen to reverse your posted Mfg receipts.";
		GridID = "M1ADDFROMREVERSALMFGRECEIPTS";
		SecurityRole = "REVERSALS";
		ContinueMessage = "This will reverse the {0} selected Mfg receipt(s). Are you sure you want to continue?";
		BindingSourceTable = "MfgReceipts";
		PromptFieldValidations.Add(new PromptFieldValidationBool("rmmPosted", fieldValue: true, "Mfg Receipt is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("rmmReversalEntry", fieldValue: false, "Mfg Receipt has already been reversed."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("rmmReversed", fieldValue: false, "Mfg Receipt has already been reversed."));
		HeaderSourceFields = new string[64]
		{
			"rmmReversalEntry=Convert(bit,1)", "rmmReceiptType", "rmmReceiptDate", "rmmPurchaseOrderID", "rmmPurchaseOrderLineID", "rmmPurchaseQuantity", "rmmPOOpenQuantity", "rmmPOLineReceivedComplete=Convert(bit,0)", "rmmJobID", "rmmJobType",
			"rmmJobAssemblyID", "rmmJobMaterialID", "rmmJobOperationID", "rmmEstimatedQuantity", "rmmJobOpenQuantity", "rmmReceivedComplete=Convert(bit,0)", "-rmmInventoryQuantity", "-rmmProductionQuantity", "-rmmQuantityCompleted", "-rmmQuantityReceivedToInventory",
			"-rmmJobScrapQuantity", "rmmProductionComplete=Convert(bit,0)", "rmmPartID", "rmmPartRevisionID", "rmmPartWarehouseLocationID", "rmmPartBinID", "rmmSupplierOrganizationID", "rmmPurchaseLocationID", "rmmQuantityOnHand", "rmmUnitLaborCost",
			"rmmUnitOverheadCost", "rmmUnitMaterialCost", "rmmUnitSubcontractCost", "rmmLongDescriptionRTF", "rmmLongDescriptionText", "-rmmPurchaseQuantityReceived", "rmmPurchaseUnitOfMeasure", "rmmPurchaseUnitCost", "rmmSetupCharge", "rmmExtendedCostBase",
			"-rmmInventoryQuantityReceived", "-rmmMiscInvQuantityReceived", "-rmmScrapQuantity", "rmmInventoryUnitOfMeasure", "-rmmJobAsmQuantityReceived", "-rmmJobMatQuantityReceived", "-rmmJobOprQuantityReceived", "-rmmQuantityToInspect", "rmmReference", "rmmRequiresInspection=Convert(bit,0)",
			"rmmInInspection=Convert(bit,0)", "rmmInspectionComplete=Convert(bit,0)", "rmmHeatLot", "rmmPlantID", "rmmPlantDepartmentID", "rmmProjectID", "rmmProjectAreaID", "rmmIMCostingMethod", "rmmPosted=Convert(bit,0)", "rmmKitPart",
			"rmmCreateJobSeq", "rmmTotalUnitCost", "rmmMfgReceiptID", "rmmMfgCostType"
		};
		HeaderDestinationFields = new string[64]
		{
			"rmmReversalEntry", "rmmReceiptType", "rmmReceiptDate", "rmmPurchaseOrderID", "rmmPurchaseOrderLineID", "rmmPurchaseQuantity", "rmmPOOpenQuantity", "rmmPOLineReceivedComplete", "rmmJobID", "rmmJobType",
			"rmmJobAssemblyID", "rmmJobMaterialID", "rmmJobOperationID", "rmmEstimatedQuantity", "rmmJobOpenQuantity", "rmmReceivedComplete", "rmmInventoryQuantity", "rmmProductionQuantity", "rmmQuantityCompleted", "rmmQuantityReceivedToInventory",
			"rmmJobScrapQuantity", "rmmProductionComplete", "rmmPartID", "rmmPartRevisionID", "rmmPartWarehouseLocationID", "rmmPartBinID", "rmmSupplierOrganizationID", "rmmPurchaseLocationID", "rmmQuantityOnHand", "rmmUnitLaborCost",
			"rmmUnitOverheadCost", "rmmUnitMaterialCost", "rmmUnitSubcontractCost", "rmmLongDescriptionRTF", "rmmLongDescriptionText", "rmmPurchaseQuantityReceived", "rmmPurchaseUnitOfMeasure", "rmmPurchaseUnitCost", "rmmSetupCharge", "rmmExtendedCostBase",
			"rmmInventoryQuantityReceived", "rmmMiscInvQuantityReceived", "rmmScrapQuantity", "rmmInventoryUnitOfMeasure", "rmmJobAsmQuantityReceived", "rmmJobMatQuantityReceived", "rmmJobOprQuantityReceived", "rmmQuantityToInspect", "rmmReference", "rmmRequiresInspection",
			"rmmInInspection", "rmmInspectionComplete", "rmmHeatLot", "rmmPlantID", "rmmPlantDepartmentID", "rmmProjectID", "rmmProjectAreaID", "rmmIMCostingMethod", "rmmPosted", "rmmKitPart",
			"rmmCreateJobSeq", "rmmTotalUnitCost", "rmmReverseMfgReceiptID", "rmmMfgCostType"
		};
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Mfg Receipt Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "rmmReceiptDate",
			AdditionalFields = "rmmReceiptDate",
			ValueStart = today,
			ValueEnd = value
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "rmmPlantID", "rmmPlantDepartmentID" })
		{
			AdditionalFields = "rmmPlantID,rmmPlantDepartmentID",
			ValueFields = new string[2] { "rmmPlantID", "rmmPlantDepartmentID" }
		});
		Duration = 10;
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		string empty = string.Empty;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		M1Database database = BindingSource.Database;
		M1DataDictionary obj = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("MfgReceipts", "MfgReceipts", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("MfgReceiptComponents", "MfgReceiptComponents", new string[23]
		{
			"rmnPartID", "rmnPartRevisionID", "rmnPartWarehouseLocationID", "rmnPartBinID", "rmnInvParentQuantity", "rmnJobMatParentQuantity", "rmnQuantityPerParent", "rmnAdditionalQuantity", "rmnReceivedComplete=Convert(bit,0)", "rmnUnitOfMeasure",
			"rmnDescription", "rmnWeight", "rmnJobID", "rmnJobAssemblyID", "rmnJobMaterialID", "rmnJobMaterialComponentID", "rmnPosted=Convert(bit,0)", "-rmnInvReceiptQuantity", "-rmnJobMatReceiptQuantity", "rmnReversed=Convert(bit,0)",
			"rmnMfgReceiptID", "rmnMfgReceiptComponentID", "rmnUnitCost"
		}, new string[23]
		{
			"rmnPartID", "rmnPartRevisionID", "rmnPartWarehouseLocationID", "rmnPartBinID", "rmnInvParentQuantity", "rmnJobMatParentQuantity", "rmnQuantityPerParent", "rmnAdditionalQuantity", "rmnReceivedComplete", "rmnUnitOfMeasure",
			"rmnDescription", "rmnWeight", "rmnJobID", "rmnJobAssemblyID", "rmnJobMaterialID", "rmnJobMaterialComponentID", "rmnPosted", "rmnInvReceiptQuantity", "rmnJobMatReceiptQuantity", "rmnReversed",
			"rmnReverseMfgReceiptID", "rmnReverseMfgReceiptCompID", "rmnUnitCost"
		});
		DataTable dataTable = database.GetDataTable("select rmmMfgReceiptID,rmnUniqueID," + matchingFieldsInfo2.GetSourceFieldList(string.Empty, string.Empty) + " from MfgReceiptComponents inner join MfgReceipts on rmnMfgReceiptID=rmmMfgReceiptID where " + text + " order by rmnMfgReceiptID,rmnMfgReceiptComponentID");
		DataTable dataTable2 = database.GetDataTable("Select rmmReceiptDate, rmmUniqueID, rmmJobID, rmmJobAssemblyID, " + matchingFieldsInfo.GetSourceFieldList(string.Empty, string.Empty) + " From MfgReceipts where " + text + " And (rmmPosted=1 And rmmReversalEntry=0) order by rmmMfgReceiptID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		string empty2 = string.Empty;
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("MfgReceiptComponents");
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		foreach (DataRow row in dataTable2.Rows)
		{
			if (row.Field<decimal>("rmmQuantityToInspect") != 0m)
			{
				messages.Add("Mfg/Misc Receipt reversal " + row.Field<string>("rmmMfgReceiptID").Trim() + " was not added because the quantity to inspect is not zero.");
				continue;
			}
			if (row.Field<string>("rmmMfgReceiptID") != null && row.Field<byte>("rmmReceiptType") == 3)
			{
				SqlTransaction sqlTransaction = null;
				empty = row.Field<string>("rmmJobID");
				int num = row.Field<int>("rmmJobAssemblyID");
				string text2 = row.Field<string>("rmmMfgReceiptID");
				decimal qtyTotalInventoryReceived = GetQtyTotalInventoryReceived(database, empty);
				DataTable qtyAndIsJobCompletedFromJobValues = GetQtyAndIsJobCompletedFromJobValues(database, empty);
				DataTable qtyAndIsJobCompletedFromReceiptValues = GetQtyAndIsJobCompletedFromReceiptValues(database, empty, text2);
				decimal num2 = qtyAndIsJobCompletedFromReceiptValues.Rows[0].Field<decimal>("rmmInventoryQuantityReceived");
				bool flag = qtyAndIsJobCompletedFromReceiptValues.Rows[0].Field<bool>("rmmProductionComplete");
				decimal num3 = qtyAndIsJobCompletedFromJobValues.Rows[0].Field<decimal>("jmpQuantityCompleted");
				bool num4 = qtyAndIsJobCompletedFromJobValues.Rows[0].Field<bool>("jmpProductionComplete");
				decimal num5 = qtyTotalInventoryReceived - num2;
				string arg2 = string.Empty;
				string text3 = "0";
				if (num4)
				{
					text3 = (flag ? "0" : "1");
					arg2 = (text3.Equals("1") ? $", jmpQuantityReceivedToInventory = {num5}" : string.Empty);
					num5 = (text3.Equals("1") ? num3 : (qtyTotalInventoryReceived - num2));
				}
				SqlCommand sqlCommand;
				if (num == 0)
				{
					sqlCommand = database.NewSqlCommand($"UPDATE Jobs\r\n                                                            SET jmpQuantityCompleted = {num5}, jmpProductionComplete = {text3} {arg2}\r\n                                                            WHERE jmpJobID = @jobID");
					sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.VarChar)).Value = empty;
					if (database.GetDataTable(sqlCommand, null).HasErrors)
					{
						messages.Add("Failed updating quantity complete to zero in Job \"" + empty + "\"");
					}
				}
				sqlCommand = database.NewSqlCommand($"UPDATE JobAssemblies\r\n                                                            SET jmaQuantityCompleted = {num5}, jmaProductionComplete = {text3}\r\n                                                            WHERE jmaJobID = @jobID AND jmaJobAssemblyID = @jobAssemblyID");
				sqlCommand.Parameters.Add(new SqlParameter("@jobAssemblyID", SqlDbType.Int)).Value = num;
				sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.VarChar)).Value = empty;
				DataTable dataTable3 = database.GetDataTable(sqlCommand, null);
				if (dataTable3.HasErrors)
				{
					messages.Add("Failed updating quantity complete to zero in Job/Assembly \"" + empty + "\"/\"" + num + "\" ");
				}
				else
				{
					messages.Add($"MfgReceipt \"{text2}\" with Job/Assembly \"{empty}\"/\"{num}\" will have Production Complete set to Not complete and Job/Assembly quantity complete will be updated to \"{num5}\".");
				}
				if (!dataTable3.HasErrors)
				{
					IList<int> allSubAssembliesIds = GetAllSubAssembliesIds(empty, num, database, sqlTransaction);
					allSubAssembliesIds.Add(num);
					if (allSubAssembliesIds.Count != 0)
					{
						Job job = new Job();
						bool complete = text3.Equals("1");
						bool updateJobs = false;
						double qtyComplete = decimal.ToDouble(num5);
						foreach (int item2 in allSubAssembliesIds)
						{
							job.CompleteJob(database, sqlTransaction, empty, complete, updateJobs, qtyComplete, item2);
						}
					}
				}
			}
			currentAsDataRow = (DataRow)BindingSource.AddNew();
			SetDefaultFieldValues(arg, currentAsDataRow);
			BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
			empty2 = currentAsDataRow.Field<string>("rmmMfgReceiptID");
			base.TransferHeaderInfo(this, row, matchingFieldsInfo, currentAsDataRow);
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			if (!string.IsNullOrWhiteSpace(empty2))
			{
				List<object[]> keysCreated = arg.KeysCreated;
				object[] item = new string[1] { empty2 };
				keysCreated.Add(item);
			}
			if (childBindingSource.Count != 0)
			{
				childBindingSource.RemoveWhere(string.Empty, currentAsDataRow);
			}
			DataRow[] array = dataTable.Select("rmmMfgReceiptID = " + row.Field<string>("rmmMfgReceiptID").Trim().ToLinq());
			foreach (DataRow sourceLineRow in array)
			{
				DataRow dataRow2 = TransferLineInfo(this, sourceLineRow, childBindingSource, matchingFieldsInfo2, currentAsDataRow);
				childBindingSource.SetKeyToNextAvailable(dataRow2);
				AddSerialAndLotTransactionsComponents(database, null, row.Field<string>("rmmMfgReceiptID"), dataRow2.Field<string>("rmnPartID").Trim(), dataRow2.Field<Guid>("rmnUniqueID"));
			}
			AddSerialAndLotTransactionsLine(database, null, row.Field<string>("rmmMfgReceiptID"), currentAsDataRow.Field<Guid>("rmmUniqueID"));
		}
		if (arg.KeysCreated.Count != 0)
		{
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "MfgReceipt";
		}
	}

	private decimal GetQtyTotalInventoryReceived(M1Database database, string jobId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT SUM(rmmInventoryQuantityReceived) AS TotalQuantityCompleted FROM MfgReceipts WHERE rmmJobID = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobId;
		return Convert.ToDecimal(database.ExecuteScalar(sqlCommand));
	}

	private DataTable GetQtyAndIsJobCompletedFromReceiptValues(M1Database database, string jobId, string receiptId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT rmmInventoryQuantityReceived, rmmProductionComplete FROM MfgReceipts WHERE rmmJobID = @JobID AND rmmMfgReceiptID = @ReceiptID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobId;
		sqlCommand.Parameters.Add(new SqlParameter("@ReceiptID", SqlDbType.NVarChar)).Value = receiptId;
		return database.GetDataTable(sqlCommand);
	}

	private DataTable GetQtyAndIsJobCompletedFromJobValues(M1Database database, string jobId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT jmpQuantityCompleted, jmpProductionComplete FROM Jobs WHERE jmpJobID = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobId;
		return database.GetDataTable(sqlCommand);
	}

	private static IList<int> GetAllSubAssembliesIds(string jobId, int jobAssemblyId, M1Database database, SqlTransaction sqlTransaction)
	{
		List<int> subAssemblyIds = new List<int>();
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT jmaJobAssemblyID,jmaParentAssemblyID FROM JobAssemblies WHERE jmaJobID = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobId;
		DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
		if (dataTable.Rows.Count != 0)
		{
			SaveNextAssemblyLevel(dataTable, jobAssemblyId, ref subAssemblyIds);
		}
		return subAssemblyIds;
	}

	private static void SaveNextAssemblyLevel(DataTable assembliesTable, int parentAssemblyId, ref List<int> subAssemblyIds)
	{
		DataRow[] array = assembliesTable.Select("jmaParentAssemblyID = " + M1Util.ConvertToLinq(parentAssemblyId) + " and jmaJobAssemblyID <> 0");
		for (int i = 0; i < array.Length; i++)
		{
			int num = Convert.ToInt32(array[i]["jmaJobAssemblyID"]);
			subAssemblyIds.Add(num);
			SaveNextAssemblyLevel(assembliesTable, num, ref subAssemblyIds);
		}
	}

	private void AddSerialAndLotTransactionsLine(M1Database database, SqlTransaction transaction, string id, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rmmUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from MfgReceipts inner join SerialNumberTransactions on rmmUniqueID = sntTableUniqueID where rmmMfgReceiptID = @ID and rmmPosted = 1 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			foreach (DataRow row in dataTable.Rows)
			{
				byte status = 0;
				byte b = 0;
				bool flag = true;
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row.Field<string>("sntSerialNumberID"));
				switch (row.Field<byte>("sntTransactionType"))
				{
				case 3:
					if (flag)
					{
						status = 12;
					}
					b = 34;
					break;
				case 23:
					if (flag)
					{
						status = 12;
					}
					b = 35;
					break;
				case 20:
					if (flag)
					{
						status = 12;
					}
					b = 36;
					break;
				case 4:
					if (flag)
					{
						status = 12;
					}
					b = 37;
					break;
				case 2:
					if (flag)
					{
						status = 12;
					}
					b = 38;
					break;
				case 14:
					if (flag)
					{
						status = 12;
					}
					b = 39;
					break;
				}
				if (b > 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "MfgReceipts", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), flag, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rmmUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from MfgReceipts inner join LotNumberTransactions on rmmUniqueID = abtTableUniqueID where rmmMfgReceiptID = @ID and rmmPosted = 1 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
		foreach (DataRow row2 in dataTable.Rows)
		{
			byte status2 = 0;
			byte b2 = 0;
			bool flag2 = row2.Field<bool>("abtNegativeTransaction");
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("abtLotNumberID"));
			switch (row2.Field<byte>("abtTransactionType"))
			{
			case 3:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 34;
				break;
			case 23:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 35;
				break;
			case 20:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 36;
				break;
			case 4:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 37;
				break;
			case 2:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 38;
				break;
			case 14:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 39;
				break;
			}
			if (b2 > 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "MfgReceipts", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), flag2, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		destinationHeaderRow["rmmReceiptDate"] = sourceHeaderRow.Field<DateTime>("rmmReceiptDate").AddMinutes(1.0);
		destinationHeaderRow["rmmReversalEntry"] = true;
	}

	private void AddSerialAndLotTransactionsComponents(M1Database database, SqlTransaction transaction, string id, string partId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rmnUniqueID, sntJobID, sntJobAssemblyID,sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from MfgReceiptComponents inner join SerialNumberTransactions on rmnUniqueID = sntTableUniqueID where rmnMfgReceiptID = @ID and rmnPosted = 1 and rmnPartID=@PartID order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			foreach (DataRow row in dataTable.Rows)
			{
				byte status = 0;
				byte b = 0;
				bool flag = true;
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row.Field<string>("sntSerialNumberID"));
				switch (row.Field<byte>("sntTransactionType"))
				{
				case 3:
					if (flag)
					{
						status = 12;
					}
					b = 34;
					break;
				case 23:
					if (flag)
					{
						status = 12;
					}
					b = 35;
					break;
				case 20:
					if (flag)
					{
						status = 12;
					}
					b = 36;
					break;
				case 4:
					if (flag)
					{
						status = 12;
					}
					b = 37;
					break;
				case 2:
					if (flag)
					{
						status = 12;
					}
					b = 38;
					break;
				case 14:
					if (flag)
					{
						status = 12;
					}
					b = 39;
					break;
				}
				if (b > 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "MfgReceiptComponents", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), flag, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rmnUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from MfgReceiptComponents inner join LotNumberTransactions on rmnUniqueID = abtTableUniqueID where rmnMfgReceiptID = @ID and rmnPosted = 1  and rmnPartID=@PartID order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
		foreach (DataRow row2 in dataTable.Rows)
		{
			byte status2 = 0;
			byte b2 = 0;
			bool flag2 = true;
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("abtLotNumberID"));
			switch (row2.Field<byte>("abtTransactionType"))
			{
			case 3:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 34;
				break;
			case 23:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 35;
				break;
			case 20:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 36;
				break;
			case 4:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 37;
				break;
			case 2:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 38;
				break;
			case 14:
				if (flag2)
				{
					status2 = 12;
				}
				b2 = 39;
				break;
			}
			if (b2 > 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "MfgReceiptComponents", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), flag2, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}
}
