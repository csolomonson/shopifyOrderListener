using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class ShipmentReversalProcess : ProcessParameters
{
	public ShipmentReversalProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "smpShipmentID" };
		PromptFieldAllowMultiples = true;
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[1] { "smpShipmentID" };
		KeyValueTableName = "Shipments";
		Description = "Use this screen to reverse your posted shipments.";
		GridID = "M1ADDFROMREVERSALSHIPMENTS";
		SecurityRole = "REVERSALS";
		ContinueMessage = "This will reverse the {0} selected shipments(s). Are you sure you want to continue?";
		BindingSourceTable = "Shipments";
		PromptFieldValidations.Add(new PromptFieldValidationBool("smpPostedToGL", fieldValue: true, "Shipment is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("smpReversalEntry", fieldValue: false, "Shipment has already been reversed."));
		HeaderSourceFields = new string[67]
		{
			"smpPlantDepartmentID", "smpPlantID", "smpCustomerOrganizationID", "smpARInvoiceLocationID", "smpARInvoiceContactID", "smpShipOrganizationID", "smpShipLocationID", "smpShipContactID", "smpShippingMethodID", "smpShippingPaymentTypeID",
			"smpNumberOfLabels", "-smpFreightCharge", "smpStandardMessageID", "smpShippingCommentsRTF", "smpShippingCommentsText", "smpPrintPackingSlip", "smpPrintLabels", "smpTrackingNumber", "smpProjectID", "smpClosed=Convert(bit,0)",
			"smpWeightTotal", "smpAdditionalWeight", "-smpFreightChargeForeign", "smpCurrencyRateID", "smpCustomRate", "smpExchangeRate", "smpPostedToGL=Convert(bit,0)", "smpShipmentSubtotalForeign", "smpShipmentTotalForeign", "smpReversalEntry=Convert(bit,1)",
			"smpListCarrierFreightBase", "smpListCarrierFreightForeign", "smpListBaseChargeForeign", "smpListSurchargeForeign", "smpAccBaseChargeBase", "smpAccBaseChargeForeign", "smpAccSurchargeBase", "smpAccSurchargeForeign", "smpListDiscountBase", "smpListDiscountForeign",
			"smpAccDiscountBase", "smpAccDiscountForeign", "smpAccCarrierFreightBase", "smpAccCarrierFreightForeign", "smpReturnInstructionsRTF", "smpReturnInstructionsText", "smpUPS3rdPartyOrganizationID", "smpUPS3rdPartyLocationID", "smpShipmentIDNumber", "smpCarrierDocumentFilePath",
			"smpExportingCarrier", "smpDocuments", "smpAESITN", "smpReasonForExport", "smpListBaseChargeBase", "smpListSurchargeBase", "smpCODLabelFilePath", "smpUPSBillingOption", "smpUPSAccountNumber", "smpFedExAccountNumber",
			"smpFedExBillingOption", "smpBlindShipOrganizationID", "smpBlindShipLocationID", "smpBlindShipContactID", "smpFedEx3rdPartyOrganizationID", "smpFedEx3rdPartyLocationID", "smpReversed=Convert(bit,0)"
		};
		HeaderDestinationFields = new string[67]
		{
			"smpPlantDepartmentID", "smpPlantID", "smpCustomerOrganizationID", "smpARInvoiceLocationID", "smpARInvoiceContactID", "smpShipOrganizationID", "smpShipLocationID", "smpShipContactID", "smpShippingMethodID", "smpShippingPaymentTypeID",
			"smpNumberOfLabels", "smpFreightCharge", "smpStandardMessageID", "smpShippingCommentsRTF", "smpShippingCommentsText", "smpPrintPackingSlip", "smpPrintLabels", "smpTrackingNumber", "smpProjectID", "smpClosed",
			"smpWeightTotal", "smpAdditionalWeight", "smpFreightChargeForeign", "smpCurrencyRateID", "smpCustomRate", "smpExchangeRate", "smpPostedToGL", "smpShipmentSubtotalForeign", "smpShipmentTotalForeign", "smpReversalEntry",
			"smpListCarrierFreightBase", "smpListCarrierFreightForeign", "smpListBaseChargeForeign", "smpListSurchargeForeign", "smpAccBaseChargeBase", "smpAccBaseChargeForeign", "smpAccSurchargeBase", "smpAccSurchargeForeign", "smpListDiscountBase", "smpListDiscountForeign",
			"smpAccDiscountBase", "smpAccDiscountForeign", "smpAccCarrierFreightBase", "smpAccCarrierFreightForeign", "smpReturnInstructionsRTF", "smpReturnInstructionsText", "smpUPS3rdPartyOrganizationID", "smpUPS3rdPartyLocationID", "smpShipmentIDNumber", "smpCarrierDocumentFilePath",
			"smpExportingCarrier", "smpDocuments", "smpAESITN", "smpReasonForExport", "smpListBaseChargeBase", "smpListSurchargeBase", "smpCODLabelFilePath", "smpUPSBillingOption", "smpUPSAccountNumber", "smpFedExAccountNumber",
			"smpFedExBillingOption", "smpBlindShipOrganizationID", "smpBlindShipLocationID", "smpBlindShipContactID", "smpFedEx3rdPartyOrganizationID", "smpFedEx3rdPartyLocationID", "smpReversed"
		};
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Shipment Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "smpShipDate",
			AdditionalFields = "smpShipDate",
			ValueStart = today,
			ValueEnd = value
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "smpPlantID", "smpPlantDepartmentID" })
		{
			AdditionalFields = "smpPlantID,smpPlantDepartmentID",
			ValueFields = new string[2] { "smpPlantID", "smpPlantDepartmentID" }
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
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("Shipments", "Shipments", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("ShipmentLines", "ShipmentLines", new string[45]
		{
			"smlShipmentID", "smlShipmentLineID", "smlPartID", "smlPartRevisionID", "smlOrgPartID", "smlPartWarehouseLocationID", "smlPartBinID", "-smlQuantityShipped", "smlOverridePrice", "smlUnitPrice",
			"smlUnitPriceForeign", "smlExtendedPriceForeign", "-smlFreightAmount", "-smlFreightAmountForeign", "smlShippedComplete=Convert(bit,0)", "smlInvoicedComplete=Convert(bit,0)", "smlUnitOfMeasure", "smlKitPart", "smlDescription", "smlOrgPartShortDescription",
			"smlPartLongDescriptionRTF", "smlPartLongDescriptionText", "smlRequiresInspection", "smlSalesOrderID", "smlSalesOrderLineID", "smlSalesOrderDeliveryID", "smlJobID", "smlHeatLot", "smlPartGroupID", "smlWeight",
			"smlExtendedWeight", "smlProjectID", "smlProjectAreaID", "smlPOSSessionID", "smlPOSTransactionID", "smlPOSTransactionLineID", "smlClosed=Convert(bit,0)", "smlPostedToGL=Convert(bit,0)", "smlReversed=Convert(bit,0)", "smlSourceTableName",
			"smlSODeliveryQuantity", "-smlJobQuantityShipped", "smlSOOpenQuantity", "smlShipmentIDNumber", "smlWeightUnitOfMeasure"
		}, new string[45]
		{
			"smlReverseShipmentID", "smlReverseShipmentLineID", "smlPartID", "smlPartRevisionID", "smlOrgPartID", "smlPartWarehouseLocationID", "smlPartBinID", "smlQuantityShipped", "smlOverridePrice", "smlUnitPrice",
			"smlUnitPriceForeign", "smlExtendedPriceForeign", "smlFreightAmount", "smlFreightAmountForeign", "smlShippedComplete", "smlInvoicedComplete", "smlUnitOfMeasure", "smlKitPart", "smlDescription", "smlOrgPartShortDescription",
			"smlPartLongDescriptionRTF", "smlPartLongDescriptionText", "smlRequiresInspection", "smlSalesOrderID", "smlSalesOrderLineID", "smlSalesOrderDeliveryID", "smlJobID", "smlHeatLot", "smlPartGroupID", "smlWeight",
			"smlExtendedWeight", "smlProjectID", "smlProjectAreaID", "smlPOSSessionID", "smlPOSTransactionID", "smlPOSTransactionLineID", "smlClosed", "smlPostedToGL", "smlReversed", "smlSourceTableName",
			"smlSODeliveryQuantity", "smlJobQuantityShipped", "smlSOOpenQuantity", "smlShipmentIDNumber", "smlWeightUnitOfMeasure"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("ShipmentComponents,ShipmentLines", "ShipmentComponents", new string[26]
		{
			"smoShipmentID", "smoShipmentLineID", "smoShipmentComponentID", "smoPartID", "smoPartRevisionID", "smoPartWarehouseLocationID", "smoPartBinID", "-smoQuantityPerParent", "-smoAdditionalQuantity", "-smoQuantityShipped",
			"smoShippedComplete=Convert(bit,0)", "smoUnitOfMeasure", "smoDescription", "smoWeight", "smoSalesOrderID", "smoSalesOrderLineID", "smoSalesOrderDeliveryID", "smoSalesOrderComponentID", "smoClosed=Convert(bit,0)", "smoPostedToGL=Convert(bit,0)",
			"smoSourceTableName", "-smoParentQuantity", "smoJobID", "-smoJobParentQuantity", "-smoJobQuantityShipped", "smoReversed=Convert(bit,0)"
		}, new string[26]
		{
			"smoReverseShipmentID", "smoReverseShipmentLineID", "smoReverseShipmentComponentID", "smoPartID", "smoPartRevisionID", "smoPartWarehouseLocationID", "smoPartBinID", "smoQuantityPerParent", "smoAdditionalQuantity", "smoQuantityShipped",
			"smoShippedComplete", "smoUnitOfMeasure", "smoDescription", "smoWeight", "smoSalesOrderID", "smoSalesOrderLineID", "smoSalesOrderDeliveryID", "smoSalesOrderComponentID", "smoClosed", "smoPostedToGL",
			"smoSourceTableName", "smoParentQuantity", "smoJobID", "smoJobParentQuantity", "smoJobQuantityShipped", "smoReversed"
		});
		DataTable dataTable = database.GetDataTable("Select smpShipmentID, smpShipDate, " + matchingFieldsInfo.GetSourceFieldList(string.Empty, string.Empty) + " From Shipments Where " + text + " And (smpPostedToGL=1 And smpReversalEntry=0) order by smpShipmentID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		string empty = string.Empty;
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ShipmentLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("ShipmentComponents");
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow dataRow2 = (DataRow)BindingSource.AddNew();
			BindingSource.ActivateRow(dataRow2, null, doFlash: false);
			empty = dataRow2.Field<string>("smpShipmentID");
			if (string.IsNullOrWhiteSpace(empty))
			{
				BindingSource.SetKeyToNextAvailable(dataRow2);
				empty = dataRow2.Field<string>("smpShipmentID");
			}
			base.TransferHeaderInfo(this, row, matchingFieldsInfo, dataRow2);
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, dataRow2);
			if (Convert.ToDecimal(dataRow2["smpFreightCharge"]) != -Convert.ToDecimal(row["smpFreightCharge"]))
			{
				dataRow2["smpFreightCharge"] = -Convert.ToDecimal(row["smpFreightCharge"]);
			}
			if (Convert.ToDecimal(dataRow2["smpFreightChargeForeign"]) != -Convert.ToDecimal(row["smpFreightChargeForeign"]))
			{
				dataRow2["smpFreightChargeForeign"] = -Convert.ToDecimal(row["smpFreightChargeForeign"]);
			}
			if (!string.IsNullOrWhiteSpace(empty))
			{
				List<object[]> keysCreated = arg.KeysCreated;
				object[] item = new string[1] { empty };
				keysCreated.Add(item);
			}
			foreach (DataRow row2 in database.GetDataTable("Select " + matchingFieldsInfo2.GetSourceFieldList(string.Empty, string.Empty) + " From ShipmentLines Where smlShipmentID = " + row.Field<string>("smpShipmentID").ToSql() + " order by smlShipmentID, smlShipmentLineID").Rows)
			{
				DataRow dataRow4 = TransferLineInfo(this, row2, childBindingSource, matchingFieldsInfo2);
				if (Convert.ToDecimal(dataRow4["smlFreightAmount"]) != -Convert.ToDecimal(row2["smlFreightAmount"]))
				{
					dataRow4["smlFreightAmount"] = -Convert.ToDecimal(row2["smlFreightAmount"]);
				}
				if (Convert.ToDecimal(dataRow4["smlFreightAmountForeign"]) != -Convert.ToDecimal(row2["smlFreightAmountForeign"]))
				{
					dataRow4["smlFreightAmountForeign"] = -Convert.ToDecimal(row2["smlFreightAmountForeign"]);
				}
				if (childBindingSource2.Count != 0)
				{
					childBindingSource2.RemoveWhere(string.Empty, row2);
				}
				foreach (DataRow row3 in database.GetDataTable("select " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " From ShipmentComponents Where smoShipmentID = " + row.Field<string>("smpShipmentID").ToSql() + "And smoShipmentLineID = " + row2.Field<short>("smlShipmentLineID").ToSql() + " order by smoShipmentID,smoShipmentLineID,smoShipmentComponentID").Rows)
				{
					DataRow dataRow5 = TransferLineInfo(this, row3, childBindingSource2, matchingFieldsInfo3, dataRow4);
					childBindingSource2.SetKeyToNextAvailable(dataRow5);
					AddSerialAndLotTransactionsComponents(database, null, row.Field<string>("smpShipmentID"), row2.Field<short>("smlShipmentLineID"), dataRow5.Field<string>("smoPartID"), dataRow5.Field<Guid>("smoUniqueID"));
				}
				AddSerialAndLotTransactionsLine(database, null, row.Field<string>("smpShipmentID"), row2.Field<short>("smlShipmentLineID"), dataRow4.Field<Guid>("smlUniqueID"));
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
		destinationHeaderRow["smpShipDate"] = sourceHeaderRow.Field<DateTime>("smpShipDate").AddMinutes(1.0);
		destinationHeaderRow["smpReversalEntry"] = true;
	}

	private void AddSerialAndLotTransactionsLine(M1Database database, SqlTransaction transaction, string id, int lineId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, smlUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from ShipmentLines inner join SerialNumberTransactions on smlUniqueID = sntTableUniqueID where smlShipmentID = @ID and smlShipmentLineID = @LineID and smlPostedToGL = 1 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				case 5:
					status = 16;
					b = 51;
					break;
				case 40:
					status = 16;
					b = 52;
					break;
				}
				if (b != 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "ShipmentLines", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, smlUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from ShipmentLines inner join LotNumberTransactions on smlUniqueID = abtTableUniqueID where smlShipmentID = @ID and smlShipmentLineID = @LineID and smlPostedToGL = 1 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			case 5:
				status2 = 16;
				b2 = 51;
				break;
			case 40:
				status2 = 16;
				b2 = 52;
				break;
			}
			if (b2 != 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "ShipmentLines", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
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
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, smoUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from ShipmentComponents inner join SerialNumberTransactions on smoUniqueID = sntTableUniqueID where smoShipmentID = @ID and smoShipmentLineID = @LineID and smoPostedToGL = 1 and smoPartID=@PartID order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
				case 5:
					status = 16;
					b = 51;
					break;
				case 40:
					status = 16;
					b = 52;
					break;
				}
				if (b != 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "ShipmentComponents", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, smoUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from ShipmentComponents inner join LotNumberTransactions on smoUniqueID = abtTableUniqueID where smoShipmentID = @ID and smoShipmentLineID = @LineID and smoPostedToGL = 1 and smoPartID=@PartID order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
			case 5:
				status2 = 16;
				b2 = 51;
				break;
			case 40:
				status2 = 16;
				b2 = 52;
				break;
			}
			if (b2 != 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "ShipmentComponents", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}
}
