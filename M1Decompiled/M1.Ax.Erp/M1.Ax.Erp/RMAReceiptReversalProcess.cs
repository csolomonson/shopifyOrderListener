using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class RMAReceiptReversalProcess : ProcessParameters
{
	public RMAReceiptReversalProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "rrpRMAReceiptID" };
		PromptFieldAllowMultiples = true;
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[1] { "rrpRMAReceiptID" };
		KeyValueTableName = "RMAReceipts";
		Description = "Use this screen to reverse your posted RMA receipts.";
		GridID = "M1ADDFROMREVERSALRMARECEIPTS";
		SecurityRole = "REVERSALS";
		ContinueMessage = "This will reverse the {0} selected RMA receipt(s). Are you sure you want to continue?";
		BindingSourceTable = "RMAReceipts";
		PromptFieldValidations.Add(new PromptFieldValidationBool("rrpPosted", fieldValue: true, "RMA receipt is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("rrpReversalEntry", fieldValue: false, "Receipt has already been reversed."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("rrpReversed", fieldValue: false, "Receipt has already been reversed."));
		HeaderSourceFields = new string[19]
		{
			"rrpDeliveryDocket", "rrpPlantDepartmentID", "rrpPlantID", "rrpCustomerOrganizationID", "rrpARInvoiceLocationID", "rrpARInvoiceContactID", "rrpShipOrganizationID", "rrpShipLocationID", "rrpShipContactID", "rrpShippingMethodID",
			"-rrpFreightCharge", "rrpProjectID", "rrpClosed=Convert(bit,0)", "rrpCurrencyRateID", "rrpCustomRate", "rrpExchangeRate", "-rrpFreightChargeForeign", "rrpPosted=Convert(bit,0)", "rrpReversalEntry=Convert(bit,1)"
		};
		HeaderDestinationFields = new string[19]
		{
			"rrpDeliveryDocket", "rrpPlantDepartmentID", "rrpPlantID", "rrpCustomerOrganizationID", "rrpARInvoiceLocationID", "rrpARInvoiceContactID", "rrpShipOrganizationID", "rrpShipLocationID", "rrpShipContactID", "rrpShippingMethodID",
			"rrpFreightCharge", "rrpProjectID", "rrpClosed", "rrpCurrencyRateID", "rrpCustomRate", "rrpExchangeRate", "rrpFreightChargeForeign", "rrpPosted", "rrpReversalEntry"
		};
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Receipt Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "rrpReceiptDate",
			AdditionalFields = "rrpReceiptDate",
			ValueStart = today,
			ValueEnd = value
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Customer", null, new string[1] { "rrpCustomerOrganizationID" })
		{
			ValueFields = new string[1] { "rrpCustomerOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "rrpPlantID", "rrpPlantDepartmentID" })
		{
			AdditionalFields = "rrpPlantID,rrpPlantDepartmentID",
			ValueFields = new string[2] { "rrpPlantID", "rrpPlantDepartmentID" }
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
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("RMAReceipts", "RMAReceipts", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("RMAReceiptLines", "RMAReceiptLines", new string[38]
		{
			"rrlRMAClaimID", "rrlRMAClaimLineID", "rrlPartID", "rrlPartRevisionID", "rrlOrgPartID", "rrlOrgPartShortDescription", "rrlDescription", "rrlPartWarehouseLocationID", "rrlPartBinID", "rrlKitPart",
			"-rrlRMAClaimQuantity", "-rrlRMAOpenQuantity", "-rrlQuantityToInspect", "rrlRequiresInspection=Convert(bit,0)", "rrlInInspection=Convert(bit,0)", "rrlInspectionComplete=Convert(bit,0)", "rrlConversionFactor", "-rrlInventoryQuantityReceived", "rrlInventoryUnitOfMeasure", "-rrlSalesQuantityReceived",
			"rrlSalesUnitOfMeasure", "rrlReference", "rrlHeatLot", "rrlClosed=Convert(bit,0)", "rrlReceivedComplete=Convert(bit,0)", "rrlInvoicedComplete=Convert(bit,0)", "rrlProjectID", "rrlProjectAreaID", "rrlPartLongDescriptionText", "rrlPartLongDescriptionRTF",
			"rrlUnitCost", "rrlExtendedCost", "rrlUnitCostForeign", "rrlExtendedCostForeign", "rrlPosted=Convert(bit,0)", "rrlReversed=Convert(bit,0)", "rrlRMAReceiptID", "rrlRMAReceiptLineID"
		}, new string[38]
		{
			"rrlRMAClaimID", "rrlRMAClaimLineID", "rrlPartID", "rrlPartRevisionID", "rrlOrgPartID", "rrlOrgPartShortDescription", "rrlDescription", "rrlPartWarehouseLocationID", "rrlPartBinID", "rrlKitPart",
			"rrlRMAClaimQuantity", "rrlRMAOpenQuantity", "rrlQuantityToInspect", "rrlRequiresInspection", "rrlInInspection", "rrlInspectionComplete", "rrlConversionFactor", "rrlInventoryQuantityReceived", "rrlInventoryUnitOfMeasure", "rrlSalesQuantityReceived",
			"rrlSalesUnitOfMeasure", "rrlReference", "rrlHeatLot", "rrlClosed", "rrlReceivedComplete", "rrlInvoicedComplete", "rrlProjectID", "rrlProjectAreaID", "rrlPartLongDescriptionText", "rrlPartLongDescriptionRTF",
			"rrlUnitCost", "rrlExtendedCost", "rrlUnitCostForeign", "rrlExtendedCostForeign", "rrlPosted", "rrlReversed", "rrlReverseRMAReceiptID", "rrlReverseRMAReceiptLineID"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("RMAReceiptComponents,RMAReceiptLines", "RMAReceiptComponents", new string[24]
		{
			"rroPartID", "rroPartRevisionID", "rroPartWarehouseLocationID", "rroPartBinID", "rroParentQuantity", "rroInspParentQuantity", "rroQuantityPerParent", "-rroAdditionalQuantity", "-rroQuantityReceived", "-rroQuantityToInspect",
			"rroUnitOfMeasure", "rroDescription", "rroWeight", "rroReceivedComplete=Convert(bit,0)", "rroInspectionComplete=Convert(bit,0)", "rroClosed=Convert(bit,0)", "rroRMAClaimID", "rroRMAClaimLineID", "rroRMAClaimComponentID", "rroPosted=Convert(bit,0)",
			"rroReversed=Convert(bit,0)", "rroRMAReceiptID", "rroRMAReceiptLineID", "rroRMAReceiptComponentID"
		}, new string[24]
		{
			"rroPartID", "rroPartRevisionID", "rroPartWarehouseLocationID", "rroPartBinID", "rroParentQuantity", "rroInspParentQuantity", "rroQuantityPerParent", "rroAdditionalQuantity", "rroQuantityReceived", "rroQuantityToInspect",
			"rroUnitOfMeasure", "rroDescription", "rroWeight", "rroReceivedComplete", "rroInspectionComplete", "rroClosed", "rroRMAClaimID", "rroRMAClaimLineID", "rroRMAClaimComponentID", "rroPosted",
			"rroReversed", "rroReverseRMAReceiptID", "rroReverseRMAReceiptLineID", "rroReverseRMAReceiptCompID"
		});
		DataTable dataTable = database.GetDataTable("Select rroUniqueID, " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " From RMAReceiptComponents INNER JOIN RMAReceiptLines ON RMAReceiptComponents.rroRMAReceiptID = RMAReceiptLines.rrlRMAReceiptID AND RMAReceiptComponents.rroRMAReceiptLineID = RMAReceiptLines.rrlRMAReceiptLineID INNER JOIN RMAReceipts ON RMAReceiptLines.rrlRMAReceiptID = RMAReceipts.rrpRMAReceiptID WHERE " + text + "AND RMAReceiptComponents.rroPosted = 1 ORDER BY RMAReceiptComponents.rroRMAReceiptID, RMAReceiptComponents.rroRMAReceiptLineID,RMAReceiptComponents.rroRMAReceiptComponentID  ");
		DataTable dataTable2 = database.GetDataTable("SELECT rrpRMAReceiptID,rrpReceiptDate,RMAReceiptLines.rrlRMAReceiptID,RMAReceiptLines.rrlRMAReceiptLineID," + matchingFieldsInfo.GetSourceFieldList(string.Empty, ",") + matchingFieldsInfo2.GetSourceFieldList(string.Empty, "") + " FROM RMAReceiptLines INNER JOIN RMAReceipts ON RMAReceiptLines.rrlRMAReceiptID = RMAReceipts.rrpRMAReceiptID WHERE " + text + " AND (RMAReceipts.rrpPosted = 1) AND (RMAReceipts.rrpReversalEntry = 0) ORDER BY RMAReceiptLines.rrlRMAReceiptID, RMAReceiptLines.rrlRMAReceiptLineID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("RMAReceiptLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("RMAReceiptComponents");
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		string empty = string.Empty;
		string text2 = string.Empty;
		foreach (DataRow row in dataTable2.Rows)
		{
			if (row.Field<decimal>("rrlQuantityToInspect") != 0m)
			{
				messages.Add("RMA Receipt reversal line for " + row.Field<string>("rrlRMAReceiptID").Trim() + " / " + row.Field<short>("rrlRMAReceiptLineID").ToString().Trim() + " was not added because the quantity to inspect is not zero.");
				continue;
			}
			if (!text2.Equals(row.Field<string>("rrlRMAReceiptID").Trim(), StringComparison.CurrentCultureIgnoreCase))
			{
				currentAsDataRow = (DataRow)BindingSource.AddNew();
				BindingSource.SetKeyToNextAvailable(currentAsDataRow);
				SetDefaultFieldValues(arg, currentAsDataRow);
				BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
				empty = currentAsDataRow.Field<string>("rrpRMAReceiptID").Trim();
				text2 = row.Field<string>("rrlRMAReceiptID").Trim();
				base.TransferHeaderInfo(this, row, matchingFieldsInfo, currentAsDataRow);
				CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
				if (!string.IsNullOrWhiteSpace(empty))
				{
					List<object[]> keysCreated = arg.KeysCreated;
					object[] item = new string[1] { empty };
					keysCreated.Add(item);
				}
			}
			DataRow dataRow2 = TransferLineInfo(this, row, childBindingSource, matchingFieldsInfo2);
			if (childBindingSource2.Count != 0)
			{
				childBindingSource2.RemoveWhere(string.Empty, dataRow2);
			}
			DataRow[] array = dataTable.Select("rroRMAReceiptID = " + row.Field<string>("rrlRMAReceiptID").Trim().ToLinq() + " and rroRMAReceiptLineID = " + Convert.ToInt32(row["rrlRMAReceiptLineID"]).ToLinq());
			foreach (DataRow sourceLineRow in array)
			{
				DataRow dataRow3 = TransferLineInfo(this, sourceLineRow, childBindingSource2, matchingFieldsInfo3, dataRow2);
				childBindingSource2.SetKeyToNextAvailable(dataRow3);
				AddSerialAndLotTransactionsComponents(database, null, row.Field<string>("rrlRMAReceiptID"), row.Field<short>("rrlRMAReceiptLineID"), dataRow3.Field<string>("rroPartID"), dataRow3.Field<Guid>("rroUniqueID"));
			}
			AddSerialAndLotTransactionsLine(database, null, row.Field<string>("rrlRMAReceiptID"), row.Field<short>("rrlRMAReceiptLineID"), dataRow2.Field<Guid>("rrlUniqueID"));
		}
		if (arg.KeysCreated.Count != 0)
		{
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "RmaReceipt";
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		destinationHeaderRow["rrpReceiptDate"] = sourceHeaderRow.Field<DateTime>("rrpReceiptDate").AddMinutes(1.0);
		destinationHeaderRow["rrpReversalEntry"] = true;
	}

	private void AddSerialAndLotTransactionsLine(M1Database database, SqlTransaction transaction, string id, int lineId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) AS sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rrlUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from RMAReceiptLines inner join SerialNumberTransactions on rrlUniqueID = sntTableUniqueID where rrlRMAReceiptID = @ID and rrlRMAReceiptLineID = @LineID and rrlPosted = 1 and rrlReversed=0order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row.Field<string>("sntSerialNumberID"));
				switch (row.Field<byte>("sntTransactionType"))
				{
				case 2:
					status = 19;
					b = 64;
					break;
				case 14:
					status = 19;
					b = 65;
					break;
				}
				if (b > 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "RMAReceiptLines", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), 0, negativeTrans: true, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) AS abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rrlUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from RMAReceiptLines inner join LotNumberTransactions on rrlUniqueID = abtTableUniqueID where rrlRMAReceiptID = @ID and rrlRMAReceiptLineID = @LineID and rrlPosted = 1 And rrlReversed=0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("abtLotNumberID"));
			switch (row2.Field<byte>("abtTransactionType"))
			{
			case 2:
				status2 = 19;
				b2 = 64;
				break;
			case 14:
				status2 = 19;
				b2 = 65;
				break;
			}
			if (b2 > 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "RMAReceiptLines", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans: true, row2.Field<DateTime>("abtTransactionDate"));
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
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) AS sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rroUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from RMAReceiptComponents inner join SerialNumberTransactions on rroUniqueID = sntTableUniqueID where rroRMAReceiptID = @ID and rroRMAReceiptLineID = @LineID and rroPosted = 1 and rroPartID=@PartID order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row.Field<string>("sntSerialNumberID"));
				switch (row.Field<byte>("sntTransactionType"))
				{
				case 2:
					status = 19;
					b = 64;
					break;
				case 14:
					status = 19;
					b = 65;
					break;
				}
				if (b > 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "RMAReceiptComponents", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans: true, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) AS abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rroUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from RMAReceiptComponents inner join LotNumberTransactions on rroUniqueID = abtTableUniqueID where rroRMAReceiptID = @ID and rroRMAReceiptLineID = @LineID and rroPosted = 1 and rroPartID=@PartID order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("abtLotNumberID"));
			switch (row2.Field<byte>("abtTransactionType"))
			{
			case 2:
				status2 = 19;
				b2 = 64;
				break;
			case 14:
				status2 = 19;
				b2 = 65;
				break;
			}
			if (b2 > 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "RMAReceiptComponents", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans: true, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}
}
