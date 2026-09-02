using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class DMRShipmentReversalProcess : ProcessParameters
{
	public DMRShipmentReversalProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "dspDMRShipmentID" };
		PromptFieldAllowMultiples = true;
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[1] { "dspDMRShipmentID" };
		KeyValueTableName = "DMRShipments";
		Description = "Use this screen to reverse your posted DMR Shipments.";
		GridID = "M1ADDFROMREVERSALDMRSHIPMENTS";
		SecurityRole = "REVERSALS";
		ContinueMessage = "This will reverse the {0} selected DMR Shipment(s). Are you sure you want to continue?";
		BindingSourceTable = "DMRShipments";
		PromptFieldValidations.Add(new PromptFieldValidationBool("dspPosted", fieldValue: true, "DMR Shipment is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("dspReversalEntry", fieldValue: false, "DMR Shipment has already been reversed."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("dspReversed", fieldValue: false, "DMR Shipment has already been reversed."));
		HeaderSourceFields = new string[27]
		{
			"dspPlantDepartmentID", "dspPlantID", "dspSupplierOrganizationID", "dspShipDate", "dspShipLocationID", "dspShipContactID", "dspShippingMethodID", "dspShippingPaymentTypeID", "dspNumberOfLabels", "-dspFreightSubtotal",
			"-dspFreightCharge", "-dspFreightTotal", "dspStandardMessageID", "dspShippingCommentsRTF", "dspShippingCommentsText", "dspPrintDMRPackingSlip", "dspPrintLabels", "dspTrackingNumber", "dspProjectID", "dspAPInvoiceLocationID",
			"-dspFreightChargeForeign", "dspCurrencyRateID", "dspExchangeRate", "dspCustomRate", "dspPosted=Convert(bit,0)", "dspReversalEntry=Convert(bit,1)", "dspReversed=Convert(bit,0)"
		};
		HeaderDestinationFields = new string[27]
		{
			"dspPlantDepartmentID", "dspPlantID", "dspSupplierOrganizationID", "dspShipDate", "dspShipLocationID", "dspShipContactID", "dspShippingMethodID", "dspShippingPaymentTypeID", "dspNumberOfLabels", "dspFreightSubtotal",
			"dspFreightCharge", "dspFreightTotal", "dspStandardMessageID", "dspShippingCommentsRTF", "dspShippingCommentsText", "dspPrintDMRPackingSlip", "dspPrintLabels", "dspTrackingNumber", "dspProjectID", "dspAPInvoiceLocationID",
			"dspFreightChargeForeign", "dspCurrencyRateID", "dspExchangeRate", "dspCustomRate", "dspPosted", "dspReversalEntry", "dspReversed"
		};
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Ship Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "dspShipDate",
			AdditionalFields = "dspShipDate",
			ValueStart = today,
			ValueEnd = value
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Supplier", null, new string[1] { "dspSupplierOrganizationID" })
		{
			ValueFields = new string[1] { "dspSupplierOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "dspPlantID", "dspPlantDepartmentID" })
		{
			AdditionalFields = "dspPlantID,dspPlantDepartmentID",
			ValueFields = new string[2] { "dspPlantID", "dspPlantDepartmentID" }
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		M1Database database = BindingSource.Database;
		M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("DMRShipments", "DMRShipments", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("DMRShipmentLines", "DMRShipmentLines", new string[36]
		{
			"dslPartID", "dslPartRevisionID", "dslPartWarehouseLocationID", "dslPartBinID", "dslConversionFactor", "dslDMRClaimQuantity", "dslDMROpenQuantity", "-dslQuantityShipped", "dslUnitOfMeasure", "-dslInventoryQuantityShipped",
			"-dslReturnQuantityShipped", "-dslJobMatQuantityShipped", "-dslJobOprQuantityShipped", "dslInventoryUnitOfMeasure", "dslKitPart", "dslInvoicedComplete=Convert(bit,0)", "dslUnitPrice", "dslShippedComplete=Convert(bit,0)", "dslProjectID", "dslProjectAreaID",
			"dslDescription", "dslDMRClaimID", "dslDMRClaimLineID", "dslJobID", "dslJobAssemblyID", "dslJobMaterialID", "dslJobOperationID", "dslPartLongDescriptionText", "dslPartLongDescriptionRTF", "dslUnitPriceForeign",
			"dslPosted=Convert(bit,0)", "dslReversed=Convert(bit,0)", "dslDMRShipmentID", "dslDMRShipmentLineID", "dslInspectionID", "dslInspectionLineID"
		}, new string[36]
		{
			"dslPartID", "dslPartRevisionID", "dslPartWarehouseLocationID", "dslPartBinID", "dslConversionFactor", "dslDMRClaimQuantity", "dslDMROpenQuantity", "dslQuantityShipped", "dslUnitOfMeasure", "dslInventoryQuantityShipped",
			"dslReturnQuantityShipped", "dslJobMatQuantityShipped", "dslJobOprQuantityShipped", "dslInventoryUnitOfMeasure", "dslKitPart", "dslInvoicedComplete", "dslUnitPrice", "dslShippedComplete", "dslProjectID", "dslProjectAreaID",
			"dslDescription", "dslDMRClaimID", "dslDMRClaimLineID", "dslJobID", "dslJobAssemblyID", "dslJobMaterialID", "dslJobOperationID", "dslPartLongDescriptionText", "dslPartLongDescriptionRTF", "dslUnitPriceForeign",
			"dslPosted", "dslReversed", "dslReverseDMRShipmentID", "dslReverseDMRShipmentLineID", "dslInspectionID", "dslInspectionLineID"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("DMRShipmentComponents,DMRShipmentLines", "DMRShipmentComponents", new string[31]
		{
			"dsoPartID", "dsoPartRevisionID", "dsoPartWarehouseLocationID", "dsoPartBinID", "-dsoInvParentQuantity", "-dsoReturnParentQuantity", "-dsoJobMatParentQuantity", "dsoQuantityPerParent", "-dsoAdditionalQuantity", "-dsoInvQuantityShipped",
			"-dsoReturnQuantityShipped", "-dsoJobMatQuantityShipped", "dsoUnitOfMeasure", "dsoDescription", "dsoWeight", "dsoShippedComplete=Convert(bit,0)", "dsoDMRClaimID", "dsoDMRClaimLineID", "dsoDMRClaimComponentID", "dsoJobID",
			"dsoJobAssemblyID", "dsoJobMaterialID", "dsoJobMaterialComponentID", "dsoPosted=Convert(bit,0)", "dsoInspectionID", "dsoInspectionLineID", "dsoInspectionComponentID", "dsoReversed=Convert(bit,0)", "dsoDMRShipmentID", "dsoDMRShipmentLineID",
			"dsoDMRShipmentComponentID"
		}, new string[31]
		{
			"dsoPartID", "dsoPartRevisionID", "dsoPartWarehouseLocationID", "dsoPartBinID", "dsoInvParentQuantity", "dsoReturnParentQuantity", "dsoJobMatParentQuantity", "dsoQuantityPerParent", "dsoAdditionalQuantity", "dsoInvQuantityShipped",
			"dsoReturnQuantityShipped", "dsoJobMatQuantityShipped", "dsoUnitOfMeasure", "dsoDescription", "dsoWeight", "dsoShippedComplete", "dsoDMRClaimID", "dsoDMRClaimLineID", "dsoDMRClaimComponentID", "dsoJobID",
			"dsoJobAssemblyID", "dsoJobMaterialID", "dsoJobMaterialComponentID", "dsoPosted", "dsoInspectionID", "dsoInspectionLineID", "dsoInspectionComponentID", "dsoReversed", "dsoReverseDMRShipmentID", "dsoReverseDMRShipmentLineID",
			"dsoReverseDMRShipmentCompID"
		});
		DataTable dataTable = database.GetDataTable("Select dsoUniqueID,Parts.impTrackSerialNumbers, Parts.impTrackLotNumbers, " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " From DMRShipmentComponents INNER JOIN DMRShipmentLines ON dsoDMRShipmentID = dslDMRShipmentID AND dsoDMRShipmentLineID = dslDMRShipmentLineID INNER JOIN DMRShipments ON dslDMRShipmentID = dspDMRShipmentID INNER JOIN Parts ON DMRShipmentComponents.dsoPartID = Parts.impPartID WHERE " + text + "AND dspPosted = 1 ORDER BY dsoDMRShipmentID,dsoDMRShipmentLineID,dsoDMRShipmentComponentID ");
		DataTable dataTable2 = database.GetDataTable("SELECT dspDMRShipmentID," + matchingFieldsInfo.GetSourceFieldList(string.Empty, ",") + matchingFieldsInfo2.GetSourceFieldList(string.Empty, "") + " FROM DMRShipmentLines INNER JOIN DMRShipments ON dslDMRShipmentID = dspDMRShipmentID WHERE " + text + " AND (dspPosted = 1) AND (dspReversalEntry = 0) AND (dspReversed=0) ORDER BY dslDMRShipmentID, dslDMRShipmentLineID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("DMRShipmentLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("DMRShipmentComponents");
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		string empty = string.Empty;
		string text2 = string.Empty;
		foreach (DataRow row in dataTable2.Rows)
		{
			if (!text2.Equals(row.Field<string>("dslDMRShipmentID").Trim(), StringComparison.CurrentCultureIgnoreCase))
			{
				currentAsDataRow = (DataRow)BindingSource.AddNew();
				BindingSource.SetKeyToNextAvailable(currentAsDataRow);
				SetDefaultFieldValues(arg, currentAsDataRow);
				BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
				empty = currentAsDataRow.Field<string>("dspDMRShipmentID").Trim();
				text2 = row.Field<string>("dslDMRShipmentID").Trim();
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
				childBindingSource2.RemoveWhere(string.Empty, row);
			}
			DataRow[] array = dataTable.Select("dsoDMRShipmentID = " + row.Field<string>("dslDMRShipmentID").Trim().ToLinq() + " and dsoDMRShipmentLineID = " + Convert.ToInt32(row["dslDMRShipmentLineID"]).ToLinq());
			foreach (DataRow dataRow3 in array)
			{
				DataRow dataRow4 = TransferLineInfo(this, dataRow3, childBindingSource2, matchingFieldsInfo3, dataRow2);
				childBindingSource2.SetKeyToNextAvailable(dataRow4);
				if (dataRow3.Field<bool>("impTrackSerialNumbers") || dataRow3.Field<bool>("impTrackLotNumbers"))
				{
					AddSerialAndLotTransactionsComponents(database, null, row.Field<string>("dslDMRShipmentID"), row.Field<short>("dslDMRShipmentLineID"), dataRow4.Field<string>("dsoPartID"), dataRow4.Field<Guid>("dsoUniqueID"));
				}
			}
			AddSerialAndLotTransactionsLine(database, null, row.Field<string>("dslDMRShipmentID"), row.Field<short>("dslDMRShipmentLineID"), dataRow2.Field<Guid>("dslUniqueID"));
		}
		if (arg.KeysCreated.Count != 0)
		{
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "DMRShipment";
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		destinationHeaderRow["dspShipDate"] = sourceHeaderRow.Field<DateTime>("dspShipDate").AddMinutes(1.0);
		destinationHeaderRow["dspReversalEntry"] = true;
	}

	private void AddSerialAndLotTransactionsLine(M1Database database, SqlTransaction transaction, string id, int lineId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) AS sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, dslUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from DMRShipmentLines inner join SerialNumberTransactions on dslUniqueID = sntTableUniqueID where dslDMRShipmentID = @ID and dslDMRShipmentLineID = @LineID and dslPosted = 1 and dslReversed=0order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				case 5:
					status = 18;
					b = 61;
					break;
				case 42:
					status = 18;
					b = 62;
					break;
				case 40:
					status = 18;
					b = 63;
					break;
				}
				if (b > 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "DMRShipmentLines", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), 0, negativeTrans: true, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) AS abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, dslUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from DMRShipmentLines inner join LotNumberTransactions on dslUniqueID = abtTableUniqueID where dslDMRShipmentID = @ID and dslDMRShipmentLineID = @LineID and dslPosted = 1 And dslReversed=0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			case 5:
				status2 = 18;
				b2 = 61;
				break;
			case 42:
				status2 = 18;
				b2 = 62;
				break;
			case 40:
				status2 = 18;
				b2 = 63;
				break;
			}
			if (b2 > 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "DMRShipmentLines", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans: true, row2.Field<DateTime>("abtTransactionDate"));
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
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) AS sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, dsoUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from DMRShipmentComponents inner join SerialNumberTransactions on dsoUniqueID = sntTableUniqueID where dsoDMRShipmentID = @ID and dsoDMRShipmentLineID = @LineID and dsoPosted = 1 and dsoPartID=@PartID order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				case 5:
					status = 18;
					b = 61;
					break;
				case 42:
					status = 18;
					b = 62;
					break;
				case 40:
					status = 18;
					b = 63;
					break;
				}
				if (b > 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "DMRShipmentComponents", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans: true, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, dsoUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from DMRShipmentComponents inner join LotNumberTransactions on dsoUniqueID = abtTableUniqueID where dsoDMRShipmentID = @ID and dsoDMRShipmentLineID = @LineID and dsoPosted = 1 and dsoPartID=@PartID order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			case 5:
				status2 = 18;
				b2 = 61;
				break;
			case 42:
				status2 = 18;
				b2 = 62;
				break;
			case 40:
				status2 = 18;
				b2 = 63;
				break;
			}
			if (b2 > 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "DMRShipmentComponents", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans: true, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}
}
