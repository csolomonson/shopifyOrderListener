using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class WHReceiptReversalProcess : ProcessParameters
{
	public WHReceiptReversalProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "wrpWarehouseReceiptID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[1] { "wrpWarehouseReceiptID" };
		KeyValueTableName = "WarehouseReceipts";
		Description = "Use this screen to reverse your posted warehouse receipts.";
		GridID = "M1ADDFROMWHREVERSALRECEIPTS";
		SecurityRole = "REVERSALS";
		ContinueMessage = "This will reverse the {0} selected warehouse receipt(s). Are you sure you want to continue?";
		BindingSourceTable = "WarehouseReceipts";
		PromptFieldValidations.Add(new PromptFieldValidationBool("wrpPosted", fieldValue: true, "Warehouse receipt is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("wrpReversalEntry", fieldValue: false, "Warehouse receipt has already been reversed."));
		HeaderSourceFields = new string[8] { "wrpSourceWarehouseID", "wrpDestinationWarehouseID", "wrpReceiptDate", "wrpShippingMethodID", "wrpShippingPaymentTypeID", "-wrpFreightCharge", "wrpClosed=Convert(bit,0)", "wrpReversalEntry=Convert(bit,1)" };
		HeaderDestinationFields = new string[8] { "wrpSourceWarehouseID", "wrpDestinationWarehouseID", "wrpReceiptDate", "wrpShippingMethodID", "wrpShippingPaymentTypeID", "wrpFreightCharge", "wrpClosed", "wrpReversalEntry" };
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Receipt Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "wrpReceiptDate",
			AdditionalFields = "wrpReceiptDate",
			ValueStart = today,
			ValueEnd = value
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		BindingSource.SetKeyToNextAvailable(currentAsDataRow);
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("WarehouseReceipts", "WarehouseReceipts", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("WarehouseReceiptLines", "WarehouseReceiptLines", new string[25]
		{
			"wrlWarehouseReceiptID", "wrlWarehouseReceiptLineID", "wrlPartID", "wrlPartRevisionID", "wrlPartDescription", "wrlSourceWarehouseID", "wrlSourcePartBinID", "wrlDestinationWarehouseID", "wrlDestinationPartBinID", "-wrlQuantityReceived",
			"wrlUnitOfMeasure", "wrlUnitCost", "wrlReceivedComplete=Convert(bit,0)", "wrlReference", "wrlHeatLot", "wrlClosed=Convert(bit,0)", "wrlWarehouseRequisitionID", "wrlWarehouseRequisitionLineID", "wrlWarehouseTransferID", "wrlWarehouseTransferLineID",
			"wrlKitPart", "-wrlWTShippedQuantity", "-wrlWTOpenQuantity", "wrlPosted=Convert(bit,0)", "wrlReversed=Convert(bit,0)"
		}, new string[25]
		{
			"wrlReverseWHReceiptID", "wrlReverseWHReceiptLineID", "wrlPartID", "wrlPartRevisionID", "wrlPartDescription", "wrlSourceWarehouseID", "wrlSourcePartBinID", "wrlDestinationWarehouseID", "wrlDestinationPartBinID", "wrlQuantityReceived",
			"wrlUnitOfMeasure", "wrlUnitCost", "wrlReceivedComplete", "wrlReference", "wrlHeatLot", "wrlClosed", "wrlWarehouseRequisitionID", "wrlWarehouseRequisitionLineID", "wrlWarehouseTransferID", "wrlWarehouseTransferLineID",
			"wrlKitPart", "wrlWTShippedQuantity", "wrlWTOpenQuantity", "wrlPosted", "wrlReversed"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("WarehouseReceiptComponents", "WarehouseReceiptComponents", new string[24]
		{
			"wroWarehouseReceiptID", "wroWarehouseReceiptLineID", "wroWarehouseReceiptComponentID", "wroPartID", "wroPartRevisionID", "wroDestinationWarehouseID", "wroDestinationPartBinID", "wroQuantityPerParent", "-wroAdditionalQuantity", "wroUnitOfMeasure",
			"wroDescription", "wroWeight", "-wroQuantityReceived", "wroReceivedComplete", "wroClosed=Convert(bit,0)", "wroSourceWarehouseID", "wroSourcePartBinID", "wroWarehouseTransferID", "wroWarehouseTransferLineID", "wroWarehouseTransComponentID",
			"wroWarehouseRequisitionID", "wroWarehouseRequisitionLineID", "wroParentQuantity", "wroPosted=Convert(bit,0)"
		}, new string[24]
		{
			"wroReverseWHReceiptID", "wroReverseWHReceiptLineID", "wroReverseWHReceiptCompID", "wroPartID", "wroPartRevisionID", "wroDestinationWarehouseID", "wroDestinationPartBinID", "wroQuantityPerParent", "wroAdditionalQuantity", "wroUnitOfMeasure",
			"wroDescription", "wroWeight", "wroQuantityReceived", "wroReceivedComplete", "wroClosed", "wroSourceWarehouseID", "wroSourcePartBinID", "wroWarehouseTransferID", "wroWarehouseTransferLineID", "wroWarehouseTransComponentID",
			"wroWarehouseRequisitionID", "wroWarehouseRequisitionLineID", "wroParentQuantity", "wroPosted"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from WarehouseReceiptComponents inner join WarehouseReceiptLines  on wrlWarehouseReceiptID=wroWarehouseReceiptID and wrlWarehouseReceiptLineID=wroWarehouseReceiptLineID INNER JOIN WarehouseReceipts on wrpWarehouseReceiptID=wroWarehouseReceiptID where " + text + " order by wroWarehouseReceiptID,wroWarehouseReceiptLineID,wroWarehouseReceiptComponentID");
		DataTable dataTable2 = databaseForRow.GetDataTable("Select wrpWarehouseReceiptID,wrpReceiptDate," + matchingFieldsInfo.GetSourceFieldList(string.Empty, ",") + matchingFieldsInfo2.GetSourceFieldList(string.Empty, "") + " From WarehouseReceiptLines inner join WarehouseReceipts on wrlWarehouseReceiptID=wrpWarehouseReceiptID where " + text + " And (wrpPosted=1 And wrpReversalEntry=0) order by wrlWarehouseReceiptID,wrlWarehouseReceiptLineID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("WarehouseReceiptLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("WarehouseReceiptComponents");
		foreach (DataRow row in dataTable2.Rows)
		{
			if (databaseForRow.GetDataTable("Select wrlWarehouseReceiptID From WarehouseReceiptLines Where wrlReverseWHReceiptID = " + row.Field<string>("wrlWarehouseReceiptID").ToSql()).Rows.Count > 0)
			{
				messages.Add("Receipt reversal for " + row.Field<string>("wrlWarehouseReceiptID").Trim() + "/" + Convert.ToInt32(row["wrlWarehouseReceiptLineID"]).ToString().Trim() + " was not added because there is already one created.");
				continue;
			}
			base.TransferHeaderInfo(this, row, matchingFieldsInfo, currentAsDataRow);
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			DataRow dataRow2 = TransferLineInfo(this, row, childBindingSource, matchingFieldsInfo2);
			if (childBindingSource2.Count != 0)
			{
				childBindingSource2.RemoveWhere(string.Empty, row);
			}
			DataRow[] array = dataTable.Select("wroWarehouseReceiptID = " + row.Field<string>("wrlWarehouseReceiptID").Trim().ToLinq() + " and wroWarehouseReceiptLineID = " + Convert.ToInt32(row["wrlWarehouseReceiptLineID"]).ToLinq());
			foreach (DataRow sourceLineRow in array)
			{
				DataRow dataRow3 = TransferLineInfo(this, sourceLineRow, childBindingSource2, matchingFieldsInfo3, row);
				dataRow3["wroWarehouseReceiptID"] = dataRow2["wrlWarehouseReceiptID"];
				dataRow3["wroWarehouseReceiptLineID"] = dataRow2["wrlWarehouseReceiptLineID"];
				AddSerialAndLotTransactionsComponents(databaseForRow, null, row.Field<string>("wrlWarehouseReceiptID"), row.Field<short>("wrlWarehouseReceiptLineID"), dataRow3.Field<string>("wroPartID"), dataRow3.Field<Guid>("wroUniqueID"));
			}
			AddSerialAndLotTransactionsLine(databaseForRow, null, row.Field<string>("wrlWarehouseReceiptID"), row.Field<short>("wrlWarehouseReceiptLineID"), dataRow2.Field<Guid>("wrlUniqueID"));
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		destinationHeaderRow["wrpReceiptDate"] = sourceHeaderRow.Field<DateTime>("wrpReceiptDate").AddMinutes(1.0);
		destinationHeaderRow["wrpReversalEntry"] = true;
	}

	private void AddSerialAndLotTransactionsLine(M1Database database, SqlTransaction transaction, string id, int lineId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, wrlUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction FROM WarehouseReceiptLines inner join SerialNumberTransactions on sntTableUniqueID = wrlUniqueID WHERE wrlWarehouseReceiptID = @ID AND wrlWarehouseReceiptLineID = @LineID AND wrlPosted = 1 ORDER BY sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineId;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			foreach (DataRow row in dataTable.Rows)
			{
				byte status = 0;
				byte b = 0;
				bool negativeTrans = true;
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row.Field<string>("sntSerialNumberID"));
				if (row.Field<byte>("sntTransactionType") == 12)
				{
					status = 21;
					b = 67;
				}
				if (b != 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "WarehouseReceiptLines", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from WarehouseReceiptLines inner join LotNumberTransactions on wrlUniqueID = abtTableUniqueID where wrlWarehouseReceiptID = @ID and wrlWarehouseReceiptLineID = @LineID and wrlPosted = 1 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineId;
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
			bool negativeTrans2 = true;
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("abtLotNumberID"));
			if (row2.Field<byte>("abtTransactionType") == 12)
			{
				status2 = 21;
				b2 = 67;
			}
			if (b2 != 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "WarehouseReceiptLines", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}

	private void AddSerialAndLotTransactionsComponents(M1Database database, SqlTransaction transaction, string id, int lineId, string partId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, wroUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction FROM WarehouseReceiptComponents inner join SerialNumberTransactions on wroUniqueID = sntTableUniqueID where wroWarehouseReceiptID = @ID AND wroWarehouseReceiptLineID = @LineID AND wroPosted = 1 and wroPartID=@PartID ORDER BY sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineId;
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			foreach (DataRow row in dataTable.Rows)
			{
				byte status = 0;
				byte b = 0;
				bool negativeTrans = true;
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row.Field<string>("sntSerialNumberID"));
				if (row.Field<byte>("sntTransactionType") == 12)
				{
					status = 21;
					b = 67;
				}
				if (b != 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "WarehouseReceiptComponents", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("SELECT DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction FROM WarehouseReceiptComponents inner join LotNumberTransactions on wroUniqueID = abtTableUniqueID where wroWarehouseReceiptID = @ID and wroWarehouseReceiptLineID = @LineID and wroPosted = 1 and wroPartID=@PartID ORDER BY abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineId;
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
			bool negativeTrans2 = true;
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("abtLotNumberID"));
			if (row2.Field<byte>("abtTransactionType") == 12)
			{
				status2 = 21;
				b2 = 67;
			}
			if (b2 != 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "WarehouseReceiptComponents", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}
}
