using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class ReceiptReversalProcess : ProcessParameters
{
	public ReceiptReversalProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "rmpReceiptID" };
		PromptFieldAllowMultiples = true;
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[1] { "rmpReceiptID" };
		KeyValueTableName = "Receipts";
		Description = "Use this screen to reverse your posted receipts.";
		GridID = "M1ADDFROMREVERSALRECEIPTS";
		SecurityRole = "REVERSALS";
		ContinueMessage = "This will reverse the {0} selected receipt(s). Are you sure you want to continue?";
		BindingSourceTable = "Receipts";
		PromptFieldValidations.Add(new PromptFieldValidationBool("rmpPostedToGL", fieldValue: true, "Receipt is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("rmpReversalEntry", fieldValue: false, "Receipt is a reversal."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("rmpReversed", fieldValue: false, "Receipt has already been reversed."));
		HeaderSourceFields = new string[19]
		{
			"rmpSupplierOrganizationID", "rmpAPInvoiceLocationID", "rmpAPInvoiceContactID", "rmpPurchaseLocationID", "rmpPurchaseContactID", "rmpShippingMethodID", "rmpCurrencyRateID", "rmpCustomRate", "rmpExchangeRate", "rmpProjectID",
			"rmpPlantID", "rmpPlantDepartmentID", "rmpLandedCost", "-rmpFreightCharge", "-rmpFreightChargeForeign", "rmpDeliveryDocket", "rmpReversalEntry=Convert(bit,1)", "rmpPostedToGL=Convert(bit,0)", "rmpClosed=Convert(bit,0)"
		};
		HeaderDestinationFields = new string[19]
		{
			"rmpSupplierOrganizationID", "rmpAPInvoiceLocationID", "rmpAPInvoiceContactID", "rmpPurchaseLocationID", "rmpPurchaseContactID", "rmpShippingMethodID", "rmpCurrencyRateID", "rmpCustomRate", "rmpExchangeRate", "rmpProjectID",
			"rmpPlantID", "rmpPlantDepartmentID", "rmpLandedCost", "rmpFreightCharge", "rmpFreightChargeForeign", "rmpDeliveryDocket", "rmpReversalEntry", "rmpPostedToGL", "rmpClosed"
		};
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Receipt Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "rmpReceiptDate",
			AdditionalFields = "rmpReceiptDate",
			ValueStart = today,
			ValueEnd = value
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Supplier", null, new string[1] { "rmpSupplierOrganizationID" })
		{
			ValueFields = new string[1] { "rmpSupplierOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "rmpPlantID", "rmpPlantDepartmentID" })
		{
			AdditionalFields = "rmpPlantID,rmpPlantDepartmentID",
			ValueFields = new string[2] { "rmpPlantID", "rmpPlantDepartmentID" }
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
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("Receipts", "Receipts", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("ReceiptLines", "ReceiptLines", new string[47]
		{
			"rmlReceiptID", "rmlReceiptLineID", "rmlPurchaseOrderID", "rmlPurchaseOrderLineID", "rmlRMAClaimID", "rmlRMAClaimLineID", "rmlForm1099Box", "rmlPurchaseUnitOfMeasure", "rmlInventoryUnitOfMeasure", "rmlKitPart",
			"rmlJobID", "rmlJobAssemblyID", "rmlJobType", "rmlJobMaterialID", "rmlJobOperationID", "rmlPartID", "rmlPartRevisionID", "rmlOrgPartID", "rmlOrgPartShortDescription", "rmlPartWarehouseLocationID",
			"rmlPartBinID", "rmlDescription", "rmlPartLongDescriptionRTF", "rmlPartLongDescriptionText", "rmlRequiresInspection", "rmlInspectionNotesRTF", "rmlInspectionNotesText", "rmlProjectID", "rmlProjectAreaID", "rmlSalesOrderID",
			"rmlSalesOrderLineID", "rmlSalesOrderDeliveryID", "rmlConversionFactor", "rmlSetupCharge", "rmlSetupChargeForeign", "-rmlPurchaseQuantityReceived", "-rmlInventoryQuantityReceived", "-rmlJobMatQuantityReceived", "-rmlJobOprQuantityReceived", "-rmlQuantityToInspect",
			"rmlPOReceivedComplete=Convert(bit,0)", "rmlJobReceivedComplete=Convert(bit,0)", "rmlInspectionComplete=Convert(bit,0)", "rmlInvoicedComplete=Convert(bit,0)", "rmlClosed=Convert(bit,0)", "rmlPostedToGL=Convert(bit,0)", "rmlReversed=Convert(bit,0)"
		}, new string[47]
		{
			"rmlReverseReceiptID", "rmlReverseReceiptLineID", "rmlPurchaseOrderID", "rmlPurchaseOrderLineID", "rmlRMAClaimID", "rmlRMAClaimLineID", "rmlForm1099Box", "rmlPurchaseUnitOfMeasure", "rmlInventoryUnitOfMeasure", "rmlKitPart",
			"rmlJobID", "rmlJobAssemblyID", "rmlJobType", "rmlJobMaterialID", "rmlJobOperationID", "rmlPartID", "rmlPartRevisionID", "rmlOrgPartID", "rmlOrgPartShortDescription", "rmlPartWarehouseLocationID",
			"rmlPartBinID", "rmlDescription", "rmlPartLongDescriptionRTF", "rmlPartLongDescriptionText", "rmlRequiresInspection", "rmlInspectionNotesRTF", "rmlInspectionNotesText", "rmlProjectID", "rmlProjectAreaID", "rmlSalesOrderID",
			"rmlSalesOrderLineID", "rmlSalesOrderDeliveryID", "rmlConversionFactor", "rmlSetupCharge", "rmlSetupChargeForeign", "rmlPurchaseQuantityReceived", "rmlInventoryQuantityReceived", "rmlJobMatQuantityReceived", "rmlJobOprQuantityReceived", "rmlQuantityToInspect",
			"rmlPOReceivedComplete", "rmlJobReceivedComplete", "rmlInspectionComplete", "rmlInvoicedComplete", "rmlClosed", "rmlPostedToGL", "rmlReversed"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("ReceiptLines,ReceiptComponents", "ReceiptComponents", new string[34]
		{
			"rmoReceiptID", "rmoReceiptLineID", "rmoReceiptComponentID", "-rmoInvParentQuantity", "-rmoJobParentQuantity", "-rmoInspParentQuantity", "rmoJobID", "rmoJobAssemblyID", "rmoJobMaterialID", "rmoJobMaterialComponentID",
			"rmoPurchaseOrderID", "rmoPurchaseOrderLineID", "rmoPurchaseOrderComponentID", "rmoPartID", "rmoPartRevisionID", "rmoPartWarehouseLocationID", "rmoPartBinID", "rmoQuantityPerParent", "-rmoAdditionalQuantity", "rmoUnitOfMeasure",
			"rmoDescription", "rmoWeight", "-rmoInvQuantityReceived", "-rmoJobQuantityReceived", "rmoReceivedComplete=Convert(bit,0)", "rmoClosed=Convert(bit,0)", "rmoPostedToGL=Convert(bit,0)", "rmoInspectionComplete=Convert(bit,0)", "rmoJobReceivedComplete=Convert(bit,0)", "rmoPurchaseUnitCost",
			"rmoPurchaseUnitCostForeign", "rmoInventoryUnitCost", "rmoInventoryUnitCostForeign", "rmoReversed=Convert(bit,0)"
		}, new string[34]
		{
			"rmoReverseReceiptID", "rmoReverseReceiptLineID", "rmoReverseReceiptComponentID", "rmoInvParentQuantity", "rmoJobParentQuantity", "rmoInspParentQuantity", "rmoJobID", "rmoJobAssemblyID", "rmoJobMaterialID", "rmoJobMaterialComponentID",
			"rmoPurchaseOrderID", "rmoPurchaseOrderLineID", "rmoPurchaseOrderComponentID", "rmoPartID", "rmoPartRevisionID", "rmoPartWarehouseLocationID", "rmoPartBinID", "rmoQuantityPerParent", "rmoAdditionalQuantity", "rmoUnitOfMeasure",
			"rmoDescription", "rmoWeight", "rmoInvQuantityReceived", "rmoJobQuantityReceived", "rmoReceivedComplete", "rmoClosed", "rmoPostedToGL", "rmoInspectionComplete", "rmoJobReceivedComplete", "rmoPurchaseUnitCost",
			"rmoPurchaseUnitCostForeign", "rmoInventoryUnitCost", "rmoInventoryUnitCostForeign", "rmoReversed"
		});
		DataTable dataTable = database.GetDataTable("Select rmpReceiptID, rmpReceiptDate, " + matchingFieldsInfo.GetSourceFieldList(string.Empty, string.Empty) + " From Receipts Where " + text + " And (rmpPostedToGL=1 And rmpReversalEntry=0) order by rmpReceiptID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		string empty = string.Empty;
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ReceiptLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("ReceiptComponents");
		foreach (DataRow row in dataTable.Rows)
		{
			if (database.GetDataTable("Select rmlReceiptID From ReceiptLines Where rmlReverseReceiptID = " + row.Field<string>("rmpReceiptID").ToSql()).Rows.Count > 0)
			{
				messages.Add("Receipt reversal for " + row.Field<string>("rmpReceiptID").Trim() + " was not added because there is already one created.");
				continue;
			}
			DataRow dataRow2 = (DataRow)BindingSource.AddNew();
			BindingSource.ActivateRow(dataRow2, null, doFlash: false);
			empty = dataRow2.Field<string>("rmpReceiptID");
			if (string.IsNullOrWhiteSpace(empty))
			{
				BindingSource.SetKeyToNextAvailable(dataRow2);
				empty = dataRow2.Field<string>("rmpReceiptID");
			}
			base.TransferHeaderInfo(this, row, matchingFieldsInfo, dataRow2);
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, dataRow2);
			foreach (DataRow row2 in database.GetDataTable("Select rmlPurchaseUnitCost, rmlPurchaseUnitCostForeign, rmlInventoryUnitCost, rmlInventoryUnitCostForeign, " + matchingFieldsInfo2.GetSourceFieldList(string.Empty, string.Empty) + " From ReceiptLines Where rmlReceiptID = " + row.Field<string>("rmpReceiptID").ToSql() + " order by rmlReceiptID,rmlReceiptLineID").Rows)
			{
				if (row2.Field<decimal>("rmlQuantityToInspect") != 0m)
				{
					messages.Add("Receipt reversal line for " + row2.Field<string>("rmlReceiptID").Trim() + " / " + row2.Field<short>("rmlReceiptLineID").ToString().Trim() + " was not added because the quantity to inspect is not zero.");
					continue;
				}
				DataRow dataRow4 = TransferLineInfo(this, row2, childBindingSource, matchingFieldsInfo2);
				dataRow4["rmlPurchaseUnitCost"] = row2["rmlPurchaseUnitCost"];
				dataRow4["rmlPurchaseUnitCostForeign"] = row2["rmlPurchaseUnitCostForeign"];
				dataRow4["rmlInventoryUnitCost"] = row2["rmlInventoryUnitCost"];
				dataRow4["rmlInventoryUnitCostForeign"] = row2["rmlInventoryUnitCostForeign"];
				if (!string.IsNullOrWhiteSpace(empty))
				{
					List<object[]> keysCreated = arg.KeysCreated;
					object[] item = new string[1] { empty };
					keysCreated.Add(item);
				}
				if (childBindingSource2.Count != 0)
				{
					childBindingSource2.RemoveWhere(string.Empty, row2);
				}
				foreach (DataRow row3 in database.GetDataTable("select " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " From ReceiptComponents Where rmoReceiptID = " + row.Field<string>("rmpReceiptID").ToSql() + "And rmoReceiptLineID = " + row2.Field<short>("rmlReceiptLineID").ToSql() + " order by rmoReceiptID,rmoReceiptLineID,rmoReceiptComponentID").Rows)
				{
					DataRow dataRow5 = TransferLineInfo(this, row3, childBindingSource2, matchingFieldsInfo3, dataRow4);
					childBindingSource2.SetKeyToNextAvailable(dataRow5);
					AddSerialAndLotTransactionsComponents(database, null, row.Field<string>("rmpReceiptID"), row2.Field<short>("rmlReceiptLineID"), dataRow5.Field<string>("rmoPartID"), dataRow5.Field<Guid>("rmoUniqueID"));
				}
				AddSerialAndLotTransactionsLine(database, null, row.Field<string>("rmpReceiptID"), row2.Field<short>("rmlReceiptLineID"), dataRow4.Field<Guid>("rmlUniqueID"));
			}
		}
		if (arg.KeysCreated.Count != 0)
		{
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = BindingSource.PrimaryTable.DefaultFormCollectionID;
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		destinationHeaderRow["rmpReceiptDate"] = sourceHeaderRow.Field<DateTime>("rmpReceiptDate").AddMinutes(1.0);
		destinationHeaderRow["rmpReversalEntry"] = true;
	}

	private void AddSerialAndLotTransactionsLine(M1Database database, SqlTransaction transaction, string id, int lineId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rmlUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from ReceiptLines inner join SerialNumberTransactions on rmlUniqueID = sntTableUniqueID where rmlReceiptID = @ID and rmlReceiptLineID = @LineID and rmlPostedToGL = 1 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				switch (row.Field<byte>("sntTransactionType"))
				{
				case 2:
					status = 15;
					b = 48;
					break;
				case 4:
					status = 15;
					b = 49;
					break;
				case 14:
					status = 15;
					b = 50;
					break;
				}
				if (b != 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "ReceiptLines", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rmlUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from ReceiptLines inner join LotNumberTransactions on rmlUniqueID = abtTableUniqueID where rmlReceiptID = @ID and rmlReceiptLineID = @LineID and rmlPostedToGL = 1 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			switch (row2.Field<byte>("abtTransactionType"))
			{
			case 2:
				status2 = 15;
				b2 = 48;
				break;
			case 4:
				status2 = 15;
				b2 = 49;
				break;
			case 14:
				status2 = 15;
				b2 = 50;
				break;
			}
			if (b2 != 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "ReceiptLines", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
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
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rmoUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from ReceiptComponents inner join SerialNumberTransactions on rmoUniqueID = sntTableUniqueID where rmoReceiptID = @ID and rmoReceiptLineID = @LineID and rmoPostedToGL = 1 and rmoPartID=@PartID order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				switch (row.Field<byte>("sntTransactionType"))
				{
				case 2:
					status = 15;
					b = 48;
					break;
				case 4:
					status = 15;
					b = 49;
					break;
				case 14:
					status = 15;
					b = 50;
					break;
				}
				if (b != 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "ReceiptComponents", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rmoUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from ReceiptComponents inner join LotNumberTransactions on rmoUniqueID = abtTableUniqueID where rmoReceiptID = @ID and rmoReceiptLineID = @LineID and rmoPostedToGL = 1 and rmoPartID=@PartID order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			switch (row2.Field<byte>("abtTransactionType"))
			{
			case 2:
				status2 = 15;
				b2 = 48;
				break;
			case 4:
				status2 = 15;
				b2 = 49;
				break;
			case 14:
				status2 = 15;
				b2 = 50;
				break;
			}
			if (b2 != 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "ReceiptComponents", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}
}
