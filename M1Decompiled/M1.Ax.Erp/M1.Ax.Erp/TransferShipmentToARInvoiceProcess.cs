using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferShipmentToARInvoiceProcess : ProcessParameters
{
	public TransferShipmentToARInvoiceProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	public TransferShipmentToARInvoiceProcess(IServiceProvider serviceProvider, bool multipleDestinationRowsCreated = false)
		: base(serviceProvider, multipleDestinationRowsCreated)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "smlShipmentID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "smlShipmentID", "smlShipmentLineID" };
		KeyValueTableName = "ShipmentLines";
		Description = "Select the shipment lines to be invoiced.";
		GridID = "M1ADDFROMARINVOICESHIPMENT";
		BindingSourceTable = "ARInvoices";
		HelpLink = "AR_TransferShipmentToARInvoice.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("smpClosed", fieldValue: false, "Shipment is closed."));
		HeaderSourceFields = new string[12]
		{
			"smpCustomerOrganizationID", "smpARInvoiceLocationID", "smpARInvoiceContactID", "smpShipOrganizationID", "smpShipLocationID", "smpShipContactID", "smpShippingMethodID", "smpShippingPaymentTypeID", "smpCurrencyRateID", "smpCustomRate",
			"smpExchangeRate", "smpProjectID"
		};
		HeaderDestinationFields = new string[12]
		{
			"arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpARInvoiceContactID", "arpShipOrganizationID", "arpShipLocationID", "arpShipContactID", "arpShippingMethodID", "arpShippingPaymentTypeID", "arpCurrencyRateID", "arpCustomRate",
			"arpExchangeRate", "arpProjectID"
		};
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		string text = string.Empty;
		string text2 = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text2.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		BindingSource.SetKeyToNextAvailable(currentAsDataRow);
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		if (currentAsDataRow.Field<byte>("arpInvoiceType") == 1)
		{
			DataTable dataTable = databaseForRow.GetDataTable("select smlShipmentID, smlShipmentLineID, smlInvoicedComplete, smpPostedToGL from ShipmentLines inner join Shipments on smlShipmentID = smpShipmentID where " + text2);
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					if (row.Field<bool>("smlInvoicedComplete"))
					{
						messages.Add(string.Format("Invoice for shipment line {0}/{1} was not added because it has already been invoiced", row.Field<string>("smlShipmentID"), row.Field<short>("smlShipmentLineID")));
					}
					if (databaseForRow.Props("FN").Field<bool>("xafGLCreateStockJournals") && !row.Field<bool>("smpPostedToGL"))
					{
						messages.Add(string.Format("Invoice for shipment line {0}/{1} was not added because shipment has not been posted yet.", row.Field<string>("smlShipmentID"), row.Field<short>("smlShipmentLineID")));
					}
				}
			}
			text2 += " and smlInvoicedComplete = 0 and (((Select xafGLCreateStockJournals From FinancialProperties) <> 0 And smpPostedToGL = 1) Or ((Select xafGLCreateStockJournals From FinancialProperties) = 0 And smpPostedToGL In (0,1)))";
		}
		M1DataDictionary obj = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("Shipments", "ARInvoices", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("ShipmentLines", "ARInvoiceLines", new string[18]
		{
			"smlPartID", "smlPartRevisionID", "smlOrgPartID", "smlOrgPartShortDescription", "smlUnitOfMeasure", "smlDescription", "smlPartLongDescriptionRTF", "smlPartLongDescriptionText", "smlPartGroupID", "smlProjectID",
			"smlProjectAreaID", "smlShippedComplete", "smlShipmentID", "smlShipmentLineID", "smlSalesOrderID", "smlSalesOrderLineID", "smlSalesOrderDeliveryID", "smlJobID"
		}, new string[18]
		{
			"arlPartID", "arlPartRevisionID", "arlOrgPartID", "arlOrgPartShortDescription", "arlUnitOfMeasure", "arlPartShortDescription", "arlPartLongDescriptionRTF", "arlPartLongDescriptionText", "arlPartGroupID", "arlProjectID",
			"arlProjectAreaID", "arlDeliveryInvoicedComplete", "arlShipmentID", "arlShipmentLineID", "arlSalesOrderID", "arlSalesOrderLineID", "arlSalesOrderDeliveryID", "arlJobID"
		});
		DataTable dataTable2 = databaseForRow.GetDataTable("select smpPlantID,smpReversalEntry,smpPlantDepartmentID,smlOverridePrice,smlUnitPriceForeign,smlFreightAmountForeign,smlUnitPrice,smlFreightAmount,jmpTimeAndMaterial," + matchingFieldsInfo2.GetSourceFieldList(string.Empty, ",") + "isnull(xayDoNotXferShipCostsToAR,0) as xayDoNotXferShipCostsToAR,isnull(cmlIgnoreAvalara,0) As cmlIgnoreAvalara,omlFullUnitPriceBase,omlFullUnitPriceForeign,omlUnitPriceBase,omlUnitPriceForeign,omlTimeAndMaterial,omdDeliveryQuantity,IsNull(omdAvalaraNonTaxReasonID, cmlNonTaxReasonID) As omdAvalaraNonTaxReasonID,omlTaxCodeID, omlNonTaxReasonID, omlSecondTaxCodeID, smpClosed,smpFreightCharge,smpFreightChargeForeign,smlQuantityShipped,smlJobQuantityShipped,IsNull((Select Sum(arlInvoiceQuantity) From ARInvoiceLines Where arlShipmentID = smlShipmentID And arlShipmentLineID = smlShipmentLineID),0) As alreadyInvoicedQuantity " + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from ShipmentLines inner join Shipments on smlShipmentID=smpShipmentID Left Outer Join Jobs on smlJobID = jmpJobID Left Outer Join SalesOrderLines On smlSalesOrderID = omlSalesOrderID And smlSalesOrderLineID = omlSalesOrderLineID Left Outer Join SalesOrderDeliveries On smlSalesOrderID = omdSalesOrderID And smlSalesOrderLineID = omdSalesOrderLineID And smlSalesOrderDeliveryID = omdSalesOrderDeliveryID left outer join ShippingPaymentTypes on xayShippingPaymentTypeID = smpShippingPaymentTypeID Inner Join OrganizationLocations On cmlORganizationID = smpCustomerOrganizationID And cmlLocationID = smpARInvoiceLocationID where " + text2 + " order by smlShipmentID,smlShipmentLineID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
		foreach (DataRow row2 in dataTable2.Rows)
		{
			if (!list.Contains(row2.Field<string>("smlShipmentID")))
			{
				list.Add(row2.Field<string>("smlShipmentID"));
			}
			CheckForHeaderKeyChange(this, row2, matchingFieldsInfo, currentAsDataRow);
			if (Convert.ToDecimal(currentAsDataRow["arpFreightAmountBase"]) != Convert.ToDecimal(row2["smpFreightCharge"]))
			{
				currentAsDataRow["arpFreightAmountBase"] = row2["smpFreightCharge"];
			}
			if (Convert.ToDecimal(currentAsDataRow["arpFreightAmountForeign"]) != Convert.ToDecimal(row2["smpFreightChargeForeign"]))
			{
				currentAsDataRow["arpFreightAmountForeign"] = row2["smpFreightChargeForeign"];
			}
			if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(row2.Field<string>("smlSalesOrderID")))
			{
				text = row2.Field<string>("smlSalesOrderID");
				transferOrderInfo(databaseForRow, BindingSource, text);
			}
			addInvoiceLine(databaseForRow, currentAsDataRow, childBindingSource, row2, matchingFieldsInfo2);
		}
		object[] item;
		if (currentAsDataRow.RowState != DataRowState.Detached)
		{
			List<object[]> keysCreated = arg.KeysCreated;
			item = new string[1] { currentAsDataRow.Field<string>("arpARInvoiceID") };
			keysCreated.Add(item);
		}
		arg.OpenKeysWithObjectID = BindingSource.PrimaryTable.DefaultFormCollectionID;
		object[] parameters = ((currentAsDataRow.RowState == DataRowState.Detached) ? null : new object[1] { currentAsDataRow.Field<string>("arpARInvoiceID") });
		item = list.ToArray();
		arg.ActionMessagesArgs = new ActionMessagesEventArgs("ADDSHIPMENTTOARINVOICE_FINISHED", parameters, item);
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		if (sourceHeaderRow.Field<bool>("smpReversalEntry"))
		{
			currentAsDataRow["arpInvoiceType"] = 2;
		}
		else
		{
			currentAsDataRow["arpInvoiceType"] = 1;
		}
		if (!sourceHeaderRow.Field<bool>("xayDoNotXferShipCostsToAR"))
		{
			if (HeaderFixForeign)
			{
				currentAsDataRow.SetField("arpFreightAmountForeign", currentAsDataRow.Field<decimal>("arpFreightAmountForeign") + ((currentAsDataRow.Field<byte>("arpInvoiceType") == 2) ? (-Math.Abs(sourceHeaderRow.Field<decimal>("smpFreightChargeForeign"))) : sourceHeaderRow.Field<decimal>("smpFreightChargeForeign")));
			}
			else
			{
				currentAsDataRow.SetField("arpFreightAmountBase", currentAsDataRow.Field<decimal>("arpFreightAmountBase") + ((currentAsDataRow.Field<byte>("arpInvoiceType") == 2) ? (-Math.Abs(sourceHeaderRow.Field<decimal>("smpFreightCharge"))) : sourceHeaderRow.Field<decimal>("smpFreightCharge")));
			}
		}
		currentAsDataRow["arpPlantID"] = sourceHeaderRow["smpPlantID"];
		currentAsDataRow["arpPlantDepartmentID"] = sourceHeaderRow["smpPlantDepartmentID"];
	}

	private void transferOrderInfo(M1Database database, M1BindingSource bsInvoice, string orderID)
	{
		if (!string.IsNullOrWhiteSpace(orderID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select ompPaymentTermID,ompOrderDate,ompFreeOnBoardDescription,ompFreightTaxCodeID,ompSecondFreightTaxCodeID,ompResellerOrganizationID,ompResellerLocationID,ompResellerContactID, ompOrderTaxAmountBase, ompOrderTaxAmountForeign From SalesOrders Where ompSalesOrderID = @OrderID");
			sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = orderID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow currentAsDataRow = bsInvoice.CurrentAsDataRow;
				DataRow dataRow = dataTable.Rows[0];
				currentAsDataRow["arpPaymentTermID"] = dataRow["ompPaymentTermID"];
				currentAsDataRow["arpOrderDate"] = dataRow["ompOrderDate"];
				currentAsDataRow["arpFreeOnBoardDescription"] = dataRow["ompFreeOnBoardDescription"];
				currentAsDataRow["arpFreightTaxCodeID"] = dataRow["ompFreightTaxCodeID"];
				currentAsDataRow["arpSecondFreightTaxCodeID"] = dataRow["ompSecondFreightTaxCodeID"];
				currentAsDataRow["arpInvoiceTaxAmountBase"] = dataRow["ompOrderTaxAmountBase"];
				currentAsDataRow["arpInvoiceTaxAmountForeign"] = dataRow["ompOrderTaxAmountForeign"];
				currentAsDataRow["arpResellerOrganizationID"] = dataRow["ompResellerOrganizationID"];
				currentAsDataRow["arpResellerLocationID"] = dataRow["ompResellerLocationID"];
				currentAsDataRow["arpResellerContactID"] = dataRow["ompResellerContactID"];
			}
			new AR().TransferOrderSalespeopleToInvoice(database, orderID, bsInvoice);
		}
	}

	private void addInvoiceLine(M1Database database, DataRow invoiceRow, M1BindingSource bsInvoiceLines, DataRow shipmentLineRow, MatchingFieldsInfo shipmentLineMatch)
	{
		DataRow dataRow = TransferLineInfo(this, shipmentLineRow, bsInvoiceLines, shipmentLineMatch);
		if (shipmentLineRow["omdDeliveryQuantity"] != DBNull.Value)
		{
			if (invoiceRow.Field<byte>("arpInvoiceType") == 2)
			{
				dataRow["arlOrderQuantity"] = -Math.Abs(shipmentLineRow.Field<decimal>("omdDeliveryQuantity"));
			}
			else
			{
				dataRow["arlOrderQuantity"] = shipmentLineRow.Field<decimal>("omdDeliveryQuantity");
			}
		}
		if (shipmentLineRow.Field<decimal>("smlQuantityShipped") + shipmentLineRow.Field<decimal>("smlJobQuantityShipped") < 0m)
		{
			dataRow["arlInvoiceQuantity"] = shipmentLineRow.Field<decimal>("smlQuantityShipped") + shipmentLineRow.Field<decimal>("smlJobQuantityShipped") + shipmentLineRow.Field<decimal>("alreadyInvoicedQuantity");
		}
		else
		{
			dataRow["arlInvoiceQuantity"] = shipmentLineRow.Field<decimal>("smlQuantityShipped") + shipmentLineRow.Field<decimal>("smlJobQuantityShipped") - shipmentLineRow.Field<decimal>("alreadyInvoicedQuantity");
		}
		if (HeaderFixForeign)
		{
			if (!shipmentLineRow.Field<bool>("smlOverridePrice") && shipmentLineRow["omlFullUnitPriceForeign"] != DBNull.Value)
			{
				dataRow["arlFullUnitPriceForeign"] = shipmentLineRow["omlFullUnitPriceForeign"];
				dataRow["arlUnitPriceForeign"] = shipmentLineRow["omlUnitPriceForeign"];
			}
			else
			{
				dataRow["arlFullUnitPriceForeign"] = shipmentLineRow["smlUnitPriceForeign"];
			}
			if (!shipmentLineRow.Field<bool>("xayDoNotXferShipCostsToAR"))
			{
				dataRow["arlFreightAmountForeign"] = shipmentLineRow["smlFreightAmountForeign"];
			}
		}
		else
		{
			if (!shipmentLineRow.Field<bool>("smlOverridePrice") && shipmentLineRow["omlFullUnitPriceBase"] != DBNull.Value)
			{
				dataRow["arlFullUnitPriceBase"] = shipmentLineRow["omlFullUnitPriceBase"];
				dataRow["arlUnitPriceBase"] = shipmentLineRow["omlUnitPriceBase"];
			}
			else
			{
				dataRow["arlFullUnitPriceBase"] = shipmentLineRow["smlUnitPrice"];
			}
			if (!shipmentLineRow.Field<bool>("xayDoNotXferShipCostsToAR"))
			{
				dataRow["arlFreightAmountBase"] = shipmentLineRow["smlFreightAmount"];
			}
		}
		if (Convert.ToDecimal(dataRow["arlFreightAmountBase"]) != Convert.ToDecimal(shipmentLineRow["smlFreightAmount"]))
		{
			dataRow["arlFreightAmountBase"] = shipmentLineRow["smlFreightAmount"];
		}
		if (Convert.ToDecimal(dataRow["arlFreightAmountForeign"]) != Convert.ToDecimal(shipmentLineRow["smlFreightAmountForeign"]))
		{
			dataRow["arlFreightAmountForeign"] = shipmentLineRow["smlFreightAmountForeign"];
		}
		addExistingDepositsToInvoice(database, bsInvoiceLines, dataRow);
		if (shipmentLineRow["jmpTimeAndMaterial"] != DBNull.Value && shipmentLineRow.Field<bool>("jmpTimeAndMaterial"))
		{
			dataRow["arlLineType"] = 1;
			new AR().AddTimeAndMaterial(database, bsInvoiceLines, dataRow.Field<string>("arlSalesOrderID"), dataRow.Field<short>("arlSalesOrderLineID"), dataRow.Field<string>("arlShipmentID"), dataRow.Field<short>("arlShipmentLineID"), string.Empty);
		}
		if (new Financial().IsAvalaraActivated(database) && !shipmentLineRow.Field<bool>("cmlIgnoreAvalara"))
		{
			dataRow["arlNonTaxReasonID"] = shipmentLineRow["omdAvalaraNonTaxReasonID"];
			if (string.IsNullOrWhiteSpace(dataRow["arlNonTaxReasonID"].ToString()))
			{
				dataRow["arlTaxCodeID"] = database.Props("FN").Field<string>("xafAvalaraTaxCodeID");
			}
			else
			{
				dataRow["arlTaxCodeID"] = string.Empty;
			}
			dataRow["arlSecondTaxCodeID"] = string.Empty;
			dataRow["arlTaxAmountforeign"] = 0;
			dataRow["arlSecondTaxAmountForeign"] = 0;
		}
		else if (shipmentLineRow["smlSalesOrderID"] != DBNull.Value && !string.IsNullOrWhiteSpace(shipmentLineRow["smlSalesOrderID"].ToString()) && string.IsNullOrWhiteSpace(shipmentLineRow["omlTaxCodeID"].ToString()))
		{
			dataRow["arlTaxCodeID"] = string.Empty;
			dataRow["arlNonTaxReasonID"] = shipmentLineRow["omlNonTaxReasonID"].ToString();
			dataRow["arlSecondTaxCodeID"] = shipmentLineRow["omlSecondTaxCodeID"].ToString();
		}
		if (dataRow.Field<decimal>("arlInvoiceQuantity") != 0m)
		{
			new AR().AddDepositsToInvoice(database, bsInvoiceLines, invoiceRow, dataRow);
		}
	}

	private void addExistingDepositsToInvoice(M1Database database, M1BindingSource bsInvoiceLines, DataRow invoiceLineRow)
	{
		if (string.IsNullOrWhiteSpace(invoiceLineRow.Field<string>("arlSalesOrderID")) || invoiceLineRow.Field<short>("arlSalesOrderLineID") == 0 || invoiceLineRow.Field<short>("arlSalesOrderDeliveryID") == 0)
		{
			return;
		}
		bool flag = false;
		switch (database.Props("FN").Field<byte>("xafARShowDeposits"))
		{
		case 1:
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select count(*) as rec_count from ARInvoiceLines Where arlSalesOrderID = @OrderID and arlSalesOrderLineID = @OrderLineID and arlSalesOrderDeliveryID = @OrderDeliveryID and arlShipmentID <> ''");
			sqlCommand.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = invoiceLineRow["arlSalesOrderID"];
			sqlCommand.Parameters.Add(new SqlParameter("@OrderLineID", SqlDbType.SmallInt)).Value = invoiceLineRow["arlSalesOrderLineID"];
			sqlCommand.Parameters.Add(new SqlParameter("@OrderDeliveryID", SqlDbType.SmallInt)).Value = invoiceLineRow["arlSalesOrderDeliveryID"];
			if (Convert.ToInt32(database.ExecuteScalar(sqlCommand)) == 0)
			{
				flag = true;
			}
			break;
		}
		case 2:
			if (invoiceLineRow.Field<bool>("arlDeliveryInvoicedComplete"))
			{
				flag = true;
			}
			break;
		}
		if (!flag)
		{
			return;
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("select * from ARInvoiceLines Where arlSalesOrderID = @OrderID and arlSalesOrderLineID = @OrderLineID and arlSalesOrderDeliveryID = @OrderDeliveryID and arlShipmentID = '' and arlExtendedPriceBase <> 0 and arlARInvoiceID <> @InvoiceID order by arlARInvoiceID");
		sqlCommand2.Parameters.Add(new SqlParameter("@OrderID", SqlDbType.NVarChar)).Value = invoiceLineRow["arlSalesOrderID"];
		sqlCommand2.Parameters.Add(new SqlParameter("@OrderLineID", SqlDbType.SmallInt)).Value = invoiceLineRow["arlSalesOrderLineID"];
		sqlCommand2.Parameters.Add(new SqlParameter("@OrderDeliveryID", SqlDbType.SmallInt)).Value = invoiceLineRow["arlSalesOrderDeliveryID"];
		sqlCommand2.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.NVarChar)).Value = invoiceLineRow["arlARInvoiceID"];
		foreach (DataRow row in database.GetDataTable(sqlCommand2).Rows)
		{
			DataRow dataRow2 = (DataRow)bsInvoiceLines.AddNew();
			dataRow2["arlSalesOrderID"] = row["arlSalesOrderID"];
			dataRow2["arlSalesOrderLineID"] = row["arlSalesOrderLineID"];
			dataRow2["arlSalesOrderDeliveryID"] = row["arlSalesOrderDeliveryID"];
			dataRow2["arlPartID"] = row["arlPartID"];
			dataRow2["arlPartRevisionID"] = row["arlPartRevisionID"];
			foreach (DataColumn column in row.Table.Columns)
			{
				if (!column.ColumnName.Equals("arlARInvoiceID", StringComparison.CurrentCultureIgnoreCase) && !column.ColumnName.Equals("arlARInvoiceLineID", StringComparison.CurrentCultureIgnoreCase) && !column.ColumnName.Equals("arlSalesOrderID", StringComparison.CurrentCultureIgnoreCase) && !column.ColumnName.Equals("arlSalesOrderLineID", StringComparison.CurrentCultureIgnoreCase) && !column.ColumnName.Equals("arlSalesOrderDeliveryID", StringComparison.CurrentCultureIgnoreCase) && !column.ColumnName.Equals("arlPartID", StringComparison.CurrentCultureIgnoreCase) && !column.ColumnName.Equals("arlPartRevisionID", StringComparison.CurrentCultureIgnoreCase) && !column.ColumnName.Equals("arlRowVersion", StringComparison.CurrentCultureIgnoreCase))
				{
					dataRow2[column.ColumnName] = row[column];
				}
			}
			dataRow2["arlOrderQuantity"] = -row.Field<decimal>("arlOrderQuantity");
			dataRow2["arlInvoiceQuantity"] = -row.Field<decimal>("arlInvoiceQuantity");
			dataRow2["arlFullUnitPriceBase"] = row.Field<decimal>("arlFullUnitPriceBase");
			dataRow2["arlUnitDiscountBase"] = row.Field<decimal>("arlUnitDiscountBase");
			dataRow2["arlUnitPriceBase"] = row.Field<decimal>("arlUnitPriceBase");
			dataRow2["arlTaxAmountBase"] = -row.Field<decimal>("arlTaxAmountBase");
			dataRow2["arlSecondTaxAmountBase"] = -row.Field<decimal>("arlSecondTaxAmountBase");
			dataRow2["arlDepositBalanceBase"] = 0;
			dataRow2["arlDepositTransferredBase"] = 0;
			dataRow2["arlDeliveryInvoicedComplete"] = invoiceLineRow["arlDeliveryInvoicedComplete"];
			dataRow2["arlPostedToGL"] = false;
			dataRow2["arlUniqueID"] = Guid.NewGuid();
		}
	}
}
