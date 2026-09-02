using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferSalesOrderToARInvoiceProcess : ProcessParameters
{
	public TransferSalesOrderToARInvoiceProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "omdSalesOrderID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[3] { "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID" };
		KeyValueTableName = "SalesOrderDeliveries";
		Description = "Use this screen to create invoice lines from a sales order. Note that the amount to invoice field is only used when the delivery type is progress payment.";
		GridID = "M1ADDFROMARINVOICESO";
		ContinueMessage = "This will create invoice lines from the {0} selected deliveries. Are you sure you want to continue?";
		BindingSourceTable = "ARInvoices";
		HelpLink = "AR_TransferSalesOrderToARInvoice.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("ompClosed", fieldValue: false, "Sales Order is closed."));
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Add Firm Deliveries Only?")
		{
			AdoFilterExpression = "omdFirm <> 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "omdFirm"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Exclude order delivery records already invoiced?")
		{
			Value = true,
			AdoFilterExpression = "omdInvoicedComplete = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "omdInvoicedComplete"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Delivery Dates")
		{
			IgnoreWhenEmpty = true,
			ValueField = "omdDeliveryDate",
			AdditionalFields = "omdDeliveryDate"
		});
		HeaderSourceFields = new string[22]
		{
			"ompCustomerOrganizationID", "ompARInvoiceLocationID", "ompARInvoiceContactID", "ompShipOrganizationID=Case When omdDifferentLocation = 0 Then ompShipOrganizationID Else omdCustomerOrganizationID End", "ompShipLocationID=Case When omdDifferentLocation = 0 Then ompShipLocationID Else omdShipLocationID End", "ompShipContactID=Case When omdDifferentLocation = 0 Then ompShipContactID Else omdShipContactID End", "omdShippingMethodID=Case When omdShippingMethodID = '' Then ompShippingMethodID Else omdShippingMethodID End", "omdShippingPaymentTypeID=Case When omdShippingPaymentTypeID = '' Then ompShippingPaymentTypeID Else omdShippingPaymentTypeID End", "ompFreightTaxCodeID", "ompSecondFreightTaxCodeID",
			"ompOrderDate", "ompFreeOnBoardDescription", "ompResellerOrganizationID", "ompResellerLocationID", "ompResellerContactID", "ompPaymentTermID", "ompCurrencyRateID", "ompCustomRate", "ompExchangeRate", "ompProjectID",
			"ompOrderTaxAmountForeign", "ompOrderTaxAmountBase"
		};
		HeaderDestinationFields = new string[22]
		{
			"arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpARInvoiceContactID", "arpShipOrganizationID", "arpShipLocationID", "arpShipContactID", "arpShippingMethodID", "arpShippingPaymentTypeID", "arpFreightTaxCodeID", "arpSecondFreightTaxCodeID",
			"arpOrderDate", "arpFreeOnBoardDescription", "arpResellerOrganizationID", "arpResellerLocationID", "arpResellerContactID", "arpPaymentTermID", "arpCurrencyRateID", "arpCustomRate", "arpExchangeRate", "arpProjectID",
			"arpInvoiceTaxAmountForeign", "arpInvoiceTaxAmountBase"
		};
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
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		BindingSource.SetKeyToNextAvailable(currentAsDataRow);
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary obj = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("SalesOrders", "ARInvoices", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("SalesOrderDeliveries,SalesOrderLines", "ARInvoiceLines", new string[17]
		{
			"omlPartID", "omlPartRevisionID", "omlOrgPartID", "omlOrgPartShortDescription", "omlUnitOfMeasure", "omlPartShortDescription", "omlPartLongDescriptionRTF", "omlPartLongDescriptionText", "omlPartGroupID", "omlProjectID",
			"omlProjectAreaID", "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID", "omlTaxCodeID", "omlNonTaxReasonID", "omlSecondTaxCodeID"
		}, new string[17]
		{
			"arlPartID", "arlPartRevisionID", "arlOrgPartID", "arlOrgPartShortDescription", "arlUnitOfMeasure", "arlPartShortDescription", "arlPartLongDescriptionRTF", "arlPartLongDescriptionText", "arlPartGroupID", "arlProjectID",
			"arlProjectAreaID", "arlSalesOrderID", "arlSalesOrderLineID", "arlSalesOrderDeliveryID", "arlTaxCodeID", "arlNonTaxReasonID", "arlSecondTaxCodeID"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select ompPlantID,ompPlantDepartmentID,omdDeliveryType,omlUnitPriceForeign,omlFreightAmountForeign,omlUnitPriceBase,omlFreightAmountBase,omlFullUnitPriceBase,omlUnitDiscountBase,omlFullUnitPriceForeign,ompFreightAmountForeign,ompFreightAmountBase,omdAvalaraNonTaxReasonID,isnull(xayDoNotXferShipCostsToAR,0) as xayDoNotXferShipCostsToAR, IsNull(cmlIgnoreAvalara,0) as cmlIgnoreAvalara " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from SalesOrderDeliveries inner join SalesOrderLines on omdSalesOrderID=omlSalesOrderID And omdSalesOrderLineID=omlSalesOrderLineID Inner Join SalesOrders On ompSalesOrderID = omlSalesOrderID left outer join ShippingPaymentTypes on xayShippingPaymentTypeID = ompShippingPaymentTypeID Inner Join OrganizationLocations On cmlOrganizationID = ompCustomerOrganizationID And cmlLocationID = ompARInvoiceLocationID where " + text + " order by omdSalesOrderID,omdSalesOrderLineID,omdSalesOrderDeliveryID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
		foreach (DataRow row in dataTable.Rows)
		{
			if (!list.Contains(row.Field<string>("omdSalesOrderID")))
			{
				list.Add(row.Field<string>("omdSalesOrderID"));
			}
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			if (Convert.ToDecimal(currentAsDataRow["arpFreightAmountBase"]) != Convert.ToDecimal(row["ompFreightAmountBase"]))
			{
				currentAsDataRow["arpFreightAmountBase"] = row["ompFreightAmountBase"];
			}
			if (Convert.ToDecimal(currentAsDataRow["arpFreightAmountForeign"]) != Convert.ToDecimal(row["ompFreightAmountForeign"]))
			{
				currentAsDataRow["arpFreightAmountForeign"] = row["ompFreightAmountForeign"];
			}
			addInvoiceLine(databaseForRow, currentAsDataRow, childBindingSource, row, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row));
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
		arg.ActionMessagesArgs = new ActionMessagesEventArgs("ADDORDERTOARINVOICE_FINISHED", parameters, item);
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		new AR().TransferOrderSalespeopleToInvoice(parm.BindingSource.Database, sourceHeaderRow.Field<string>("omdSalesOrderID"), parm.BindingSource);
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		if (!sourceHeaderRow.Field<bool>("xayDoNotXferShipCostsToAR"))
		{
			if (HeaderFixForeign)
			{
				currentAsDataRow.SetField("arpFreightAmountForeign", currentAsDataRow.Field<decimal>("arpFreightAmountForeign") + ((currentAsDataRow.Field<byte>("arpInvoiceType") == 2) ? (-Math.Abs(sourceHeaderRow.Field<decimal>("ompFreightAmountForeign"))) : sourceHeaderRow.Field<decimal>("ompFreightAmountForeign")));
			}
			else
			{
				currentAsDataRow.SetField("arpFreightAmountBase", currentAsDataRow.Field<decimal>("arpFreightAmountBase") + ((currentAsDataRow.Field<byte>("arpInvoiceType") == 2) ? (-Math.Abs(sourceHeaderRow.Field<decimal>("ompFreightAmountBase"))) : sourceHeaderRow.Field<decimal>("ompFreightAmountBase")));
			}
		}
		currentAsDataRow["arpPlantID"] = sourceHeaderRow["ompPlantID"];
		currentAsDataRow["arpPlantDepartmentID"] = sourceHeaderRow["ompPlantDepartmentID"];
	}

	private void addInvoiceLine(M1Database database, DataRow invoiceRow, M1BindingSource bsInvoiceLines, DataRow deliveryRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, deliveryRow, bsInvoiceLines, lineMatches);
		decimal num = default(decimal);
		if (itemValues.EditableValues.ContainsKey("QtyToInvoice"))
		{
			num = Convert.ToDecimal(itemValues.EditableValues["QtyToInvoice"]);
		}
		if (invoiceRow.Field<byte>("arpInvoiceType") == 2)
		{
			dataRow["arlOrderQuantity"] = -Math.Abs(num);
		}
		else
		{
			dataRow["arlOrderQuantity"] = num;
		}
		dataRow["arlInvoiceQuantity"] = dataRow["arlOrderQuantity"];
		decimal num2 = default(decimal);
		if (itemValues.EditableValues.ContainsKey("omdAmountToInvoice"))
		{
			num2 = Convert.ToDecimal(itemValues.EditableValues["omdAmountToInvoice"]);
		}
		if (itemValues.EditableValues.ContainsKey("InvoicedComplete"))
		{
			dataRow["arlDeliveryInvoicedComplete"] = Convert.ToBoolean(itemValues.EditableValues["InvoicedComplete"]);
		}
		if (deliveryRow.Field<byte>("omdDeliveryType") == 3)
		{
			if (num == 0m)
			{
				num = 1m;
			}
			dataRow["arlFullUnitPriceBase"] = M1Math.Round(num2 / num, 5);
			dataRow["arlUnitPriceBase"] = M1Math.Round(num2 / num, 5);
		}
		else
		{
			dataRow["arlFullUnitPriceBase"] = deliveryRow["omlFullUnitPriceBase"];
			dataRow["arlUnitDiscountBase"] = deliveryRow["omlUnitDiscountBase"];
			dataRow["arlUnitPriceBase"] = deliveryRow["omlUnitPriceBase"];
		}
		if (!deliveryRow.Field<bool>("xayDoNotXferShipCostsToAR"))
		{
			if (HeaderFixForeign)
			{
				dataRow["arlFreightAmountForeign"] = deliveryRow["omlFreightAmountForeign"];
			}
			else
			{
				dataRow["arlFreightAmountBase"] = deliveryRow["omlFreightAmountBase"];
			}
		}
		if (Convert.ToDecimal(dataRow["arlFreightAmountBase"]) != Convert.ToDecimal(deliveryRow["omlFreightAmountBase"]))
		{
			dataRow["arlFreightAmountBase"] = deliveryRow["omlFreightAmountBase"];
		}
		if (Convert.ToDecimal(dataRow["arlFreightAmountForeign"]) != Convert.ToDecimal(deliveryRow["omlFreightAmountForeign"]))
		{
			dataRow["arlFreightAmountForeign"] = deliveryRow["omlFreightAmountForeign"];
		}
		if (new Financial().IsAvalaraActivated(database) && !deliveryRow.Field<bool>("cmlIgnoreAvalara"))
		{
			dataRow["arlNonTaxReasonID"] = deliveryRow["omdAvalaraNonTaxReasonID"];
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
		if (dataRow.Field<decimal>("arlInvoiceQuantity") != 0m)
		{
			new AR().AddDepositsToInvoice(database, bsInvoiceLines, invoiceRow, dataRow);
		}
	}
}
