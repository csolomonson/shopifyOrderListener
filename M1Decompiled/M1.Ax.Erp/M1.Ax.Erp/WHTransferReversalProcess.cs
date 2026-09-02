using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class WHTransferReversalProcess : ProcessParameters
{
	public WHTransferReversalProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "mwpWarehouseTransferID" };
		PromptFieldAllowMultiples = true;
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[1] { "mwpWarehouseTransferID" };
		KeyValueTableName = "WarehouseTransfers";
		Description = "Use this screen to reverse your posted transfers.";
		GridID = "M1ADDFROMREVERSALWHTRANSFERRECEIPTS";
		SecurityRole = "REVERSALS";
		ContinueMessage = "This will reverse the {0} selected warehouse transfer(s). Are you sure you want to continue?";
		BindingSourceTable = "WarehouseTransfers";
		PromptFieldValidations.Add(new PromptFieldValidationBool("mwpPosted", fieldValue: true, "Transfer is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("mwpReversalEntry", fieldValue: false, "Transfer has already been reversed."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("mwpReversed", fieldValue: false, "Transfer has already been reversed."));
		HeaderSourceFields = new string[15]
		{
			"mwpSourceWarehouseID", "mwpDestinationWarehouseID", "mwpShippingMethodID", "mwpShippingPaymentTypeID", "-mwpFreightCharge", "mwpShippingCommentsRTF", "mwpShippingCommentsText", "mwpPrintLabels", "mwpNumberOfLabels", "mwpPrintPacker",
			"mwpTrackingNumber", "mwpPosted=Convert(bit,0)", "mwpReversalEntry=Convert(bit,1)", "mwpClosed=Convert(bit,0)", "mwpReversed=Convert(bit,0)"
		};
		HeaderDestinationFields = new string[15]
		{
			"mwpSourceWarehouseID", "mwpDestinationWarehouseID", "mwpShippingMethodID", "mwpShippingPaymentTypeID", "mwpFreightCharge", "mwpShippingCommentsRTF", "mwpShippingCommentsText", "mwpPrintLabels", "mwpNumberOfLabels", "mwpPrintPacker",
			"mwpTrackingNumber", "mwpPosted", "mwpReversalEntry", "mwpClosed", "mwpReversed"
		};
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Receipt Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "mwpShipDate",
			AdditionalFields = "mwpShipDate",
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
		M1Database database = BindingSource.Database;
		M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("WarehouseTransfers", "WarehouseTransfers", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("WarehouseTransferLines", "WarehouseTransferLines", new string[23]
		{
			"mwlWarehouseTransferID", "mwlWarehouseTransferLineID", "mwlPartID", "mwlPartRevisionID", "mwlUnitOfMeasure", "mwlPartDescription", "mwlSourceWarehouseID", "mwlSourcePartBinID", "-mwlShipQuantity", "mwlShippedComplete=Convert(bit,0)",
			"mwlReceivedQuantity", "mwlReceivedDate", "mwlReceivedComplete=Convert(bit,0)", "mwlWarehouseRequisitionID", "mwlWarehouseRequisitionLineID", "mwlClosed=Convert(bit,0)", "mwlKitPart", "mwlWROpenQuantity", "mwlWRRequestedQuantity", "mwlDestinationWarehouseID",
			"-mwlQuantityInTransit", "mwlReversed=Convert(bit,0)", "mwlPosted=Convert(bit,0)"
		}, new string[23]
		{
			"mwlReverseWHTransferID", "mwlReverseWHTransferLineID", "mwlPartID", "mwlPartRevisionID", "mwlUnitOfMeasure", "mwlPartDescription", "mwlSourceWarehouseID", "mwlSourcePartBinID", "mwlShipQuantity", "mwlShippedComplete",
			"mwlReceivedQuantity", "mwlReceivedDate", "mwlReceivedComplete", "mwlWarehouseRequisitionID", "mwlWarehouseRequisitionLineID", "mwlClosed", "mwlKitPart", "mwlWROpenQuantity", "mwlWRRequestedQuantity", "mwlDestinationWarehouseID",
			"mwlQuantityInTransit", "mwlReversed", "mwlPosted"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("WarehouseTransferComponents", "WarehouseTransferComponents", new string[25]
		{
			"mwoWarehouseTransferID", "mwoWarehouseTransferLineID", "mwoWarehouseTransComponentID", "mwoPartID", "mwoPartRevisionID", "mwoSourceWarehouseID", "mwoSourcePartBinID", "-mwoParentQuantity", "mwoQuantityPerParent", "-mwoAdditionalQuantity",
			"-mwoShipQuantity", "mwoUnitOfMeasure", "mwoDescription", "mwoWeight", "mwoReceivedQuantity", "mwoReceivedComplete=Convert(bit,0)", "mwoClosed=Convert(bit,0)", "mwoWarehouseRequisitionID", "mwoWarehouseRequisitionLineID", "mwoWarehouseReqComponentID",
			"mwoShippedComplete=Convert(bit,0)", "mwoDestinationWarehouseID", "-mwoQuantityInTransit", "mwoPosted=Convert(bit,0)", "mwoReversed=Convert(bit,0)"
		}, new string[25]
		{
			"mwoReverseWHTransferID", "mwoReverseWHTransferLineID", "mwoReverseWHTransComponentID", "mwoPartID", "mwoPartRevisionID", "mwoSourceWarehouseID", "mwoSourcePartBinID", "mwoParentQuantity", "mwoQuantityPerParent", "mwoAdditionalQuantity",
			"mwoShipQuantity", "mwoUnitOfMeasure", "mwoDescription", "mwoWeight", "mwoReceivedQuantity", "mwoReceivedComplete", "mwoClosed", "mwoWarehouseRequisitionID", "mwoWarehouseRequisitionLineID", "mwoWarehouseReqComponentID",
			"mwoShippedComplete", "mwoDestinationWarehouseID", "mwoQuantityInTransit", "mwoPosted", "mwoReversed"
		});
		DataTable dataTable = database.GetDataTable("SELECT mwoUniqueID," + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " FROM WarehouseTransferComponents INNER JOIN WarehouseTransferLines ON mwoWarehouseTransferID = mwlWarehouseTransferID AND mwoWarehouseTransferLineID = mwlWarehouseTransferLineID  INNER JOIN WarehouseTransfers ON mwlWarehouseTransferID = mwpWarehouseTransferID WHERE " + text + " ORDER BY mwoWarehouseTransferID,mwoWarehouseTransferLineID,mwoWarehouseTransComponentID");
		DataTable dataTable2 = database.GetDataTable("SELECT mwpWarehouseTransferID,mwpShipDate,mwlUniqueID," + matchingFieldsInfo.GetSourceFieldList(string.Empty, ",") + matchingFieldsInfo2.GetSourceFieldList(string.Empty, "") + " FROM WarehouseTransferLines INNER JOIN WarehouseTransfers ON mwlWarehouseTransferID = mwpWarehouseTransferID WHERE " + text + " ORDER BY mwlWarehouseTransferID, mwlWarehouseTransferLineID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		string empty = string.Empty;
		string text2 = string.Empty;
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("WarehouseTransferLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("WarehouseTransferComponents");
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		foreach (DataRow row2 in dataTable2.Rows)
		{
			bool flag = true;
			DataTable dataTable3 = database.GetDataTable("select a.wrlReversed as ReversedFlag, a.wrlPosted as PostedFlag, Isnull(b.wrlPosted, 'false') as reversalPostedFlag from WarehouseReceiptLines a left join WarehouseReceiptLines b on a.wrlWarehouseReceiptID = b.wrlReverseWHReceiptID and a.wrlWarehouseReceiptLineID = b.wrlReverseWHReceiptLineID where a.wrlSourceTableUniqueID = " + row2.Field<Guid>("mwlUniqueID").ToSql());
			if (dataTable3.Rows.Count > 0)
			{
				foreach (DataRow row3 in dataTable3.Rows)
				{
					if (!(bool)row3["PostedFlag"] || !(bool)row3["reversalPostedFlag"])
					{
						messages.Add("Warehouse Transfer reversal for " + row2.Field<string>("mwlWarehouseTransferID").Trim() + " was not added because it has already been receipted and the receipt has not been reversed.");
						flag = false;
					}
				}
			}
			if (!flag)
			{
				continue;
			}
			if (!text2.Equals(row2.Field<string>("mwlWarehouseTransferID").Trim(), StringComparison.CurrentCultureIgnoreCase))
			{
				currentAsDataRow = (DataRow)BindingSource.AddNew();
				BindingSource.SetKeyToNextAvailable(currentAsDataRow);
				SetDefaultFieldValues(arg, currentAsDataRow);
				BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
				empty = currentAsDataRow.Field<string>("mwpWarehouseTransferID");
				text2 = row2.Field<string>("mwlWarehouseTransferID");
				base.TransferHeaderInfo(this, row2, matchingFieldsInfo, currentAsDataRow);
				CheckForHeaderKeyChange(this, row2, matchingFieldsInfo, currentAsDataRow);
				if (!string.IsNullOrWhiteSpace(empty))
				{
					List<object[]> keysCreated = arg.KeysCreated;
					object[] item = new string[1] { empty };
					keysCreated.Add(item);
				}
			}
			DataRow dataRow3 = TransferLineInfo(this, row2, childBindingSource, matchingFieldsInfo2);
			if (childBindingSource2.Count != 0)
			{
				childBindingSource2.RemoveWhere(string.Empty, dataRow3);
			}
			DataRow[] array = dataTable.Select("mwoWarehouseTransferID = " + row2.Field<string>("mwlWarehouseTransferID").Trim().ToLinq() + " and mwoWarehouseTransferLineID = " + Convert.ToInt32(row2["mwlWarehouseTransferLineID"]).ToLinq());
			foreach (DataRow sourceLineRow in array)
			{
				DataRow row = TransferLineInfo(this, sourceLineRow, childBindingSource2, matchingFieldsInfo3, dataRow3);
				AddSerialAndLotTransactionsComponents(database, null, row2.Field<string>("mwlWarehouseTransferID"), row2.Field<short>("mwlWarehouseTransferLineID"), row.Field<string>("mwoPartID"), row.Field<Guid>("mwoUniqueID"));
			}
			AddSerialAndLotTransactionsLine(database, null, row2.Field<string>("mwlWarehouseTransferID"), row2.Field<short>("mwlWarehouseTransferLineID"), dataRow3.Field<Guid>("mwlUniqueID"));
		}
		if (arg.KeysCreated.Count != 0)
		{
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = BindingSource.PrimaryTable.DefaultFormCollectionID;
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		destinationHeaderRow["mwpShipDate"] = sourceHeaderRow.Field<DateTime>("mwpShipDate").AddMinutes(1.0);
		destinationHeaderRow["mwpReversalEntry"] = true;
	}

	private void AddSerialAndLotTransactionsLine(M1Database database, SqlTransaction transaction, string id, int lineId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction FROM WarehouseTransferLines INNER JOIN SerialNumberTransactions on mwlUniqueID = sntTableUniqueID WHERE mwlWarehouseTransferID = @ID AND mwlWarehouseTransferLineID = @LineID and mwlPosted = 1 ORDER BY sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				if (row.Field<byte>("sntTransactionType") == 11)
				{
					status = 20;
					b = 66;
				}
				if (b != 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "WarehouseTransferLines", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction FROM WarehouseTransferLines inner join LotNumberTransactions on mwlUniqueID = abtTableUniqueID WHERE mwlWarehouseTransferID = @ID and mwlWarehouseTransferLineID = @LineID and mwlPosted = 1 ORDER BY abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			if (row2.Field<byte>("abtTransactionType") == 11)
			{
				status2 = 20;
				b2 = 66;
			}
			if (b2 != 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "WarehouseTransferLines", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
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
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction FROM WarehouseTransferComponents INNER JOIN SerialNumberTransactions on mwoUniqueID = sntTableUniqueID WHERE mwoWarehouseTransferID = @ID AND mwoWarehouseTransferLineID = @LineID and mwoPosted = 1 and mwoPartID=@PartID ORDER BY sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				if (row.Field<byte>("sntTransactionType") == 11)
				{
					status = 20;
					b = 66;
				}
				if (b != 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "WarehouseTransferComponents", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction FROM WarehouseTransferComponents inner join LotNumberTransactions on mwoUniqueID = abtTableUniqueID WHERE mwoWarehouseTransferID = @ID and mwoWarehouseTransferLineID = @LineID and mwoPosted = 1 and mwoPartID=@PartID ORDER BY abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			if (row2.Field<byte>("abtTransactionType") == 11)
			{
				status2 = 20;
				b2 = 66;
			}
			if (b2 != 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "WarehouseTransferComponents", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}
}
