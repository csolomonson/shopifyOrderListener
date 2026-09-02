using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferSalesOrderToShipmentProcess : ProcessParameters
{
	public TransferSalesOrderToShipmentProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "omdSalesOrderID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[3] { "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID" };
		KeyValueTableName = "SalesOrderDeliveries";
		Description = "Select the sales order deliveries to be shipped.";
		CreatedBindingSourceCaption = "Create Shipment from Sales Order";
		GridID = "M1ADDFROMSHIPMENTSO";
		BindingSourceTable = "Shipments";
		HelpLink = "SM_TransferSalesOrderToShipment.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("ompClosed", fieldValue: false, "Sales Order is closed."));
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Add Firm Deliveries Only?")
		{
			AdoFilterExpression = "omdFirm <> 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "omdFirm"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Delivery Dates")
		{
			IgnoreWhenEmpty = true,
			ValueField = "omdDeliveryDate",
			AdditionalFields = "omdDeliveryDate"
		});
		HeaderSourceFields = new string[14]
		{
			"ompCustomerOrganizationID", "ompARInvoiceLocationID", "ompARInvoiceContactID", "omdCustomerOrganizationID=Case When omdDifferentLocation = 0 Then ompShipOrganizationID Else omdCustomerOrganizationID End", "omdShipLocationID=Case When omdDifferentLocation = 0 Then ompShipLocationID Else omdShipLocationID End", "omdShipContactID=Case When omdDifferentLocation = 0 Then ompShipContactID Else omdShipContactID End", "omdShippingMethodID=Case When omdShippingMethodID = '' Then ompShippingMethodID Else omdShippingMethodID End", "omdShippingPaymentTypeID=Case When omdShippingPaymentTypeID = '' Then ompShippingPaymentTypeID Else omdShippingPaymentTypeID End", "ompProjectID", "ompCurrencyRateID",
			"ompCustomRate", "ompExchangeRate", "ompFreightAmountBase", "ompFreightAmountForeign"
		};
		HeaderDestinationFields = new string[14]
		{
			"smpCustomerOrganizationID", "smpARInvoiceLocationID", "smpARInvoiceContactID", "smpShipOrganizationID", "smpShipLocationID", "smpShipContactID", "smpShippingMethodID", "smpShippingPaymentTypeID", "smpProjectID", "smpCurrencyRateID",
			"smpCustomRate", "smpExchangeRate", "smpFreightCharge", "smpFreightChargeForeign"
		};
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		arg.FilterErrorRegex.Add(" is inactive and has no quantity on hand or quantity to inspect.");
		List<object[]> promptFieldValues = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Lot, value: false);
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Serial, value: false);
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0 || BindingSource.CurrentAsDataRow == null)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("SalesOrders", "Shipments", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("SalesOrderDeliveries,SalesOrderLines", "ShipmentLines", new string[23]
		{
			"omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID", "omdPartID", "omdPartRevisionID", "omdPartWarehouseLocationID", "omdPartBinID", "omdShippedComplete", "omlOrgPartID", "omlOrgPartShortDescription",
			"omlUnitOfMeasure", "omlPartShortDescription", "omlPartLongDescriptionRTF", "omlPartLongDescriptionText", "omlUnitPriceBase", "omlUnitPriceForeign", "omlFreightAmountBase", "omlFreightAmountForeign", "omlPartGroupID", "omlWeight",
			"omlProjectID", "omlProjectAreaID", "KitPart=Case When omdDeliveryType = 4 Then 1 Else 0 End"
		}, new string[23]
		{
			"smlSalesOrderID", "smlSalesOrderLineID", "smlSalesOrderDeliveryID", "smlPartID", "smlPartRevisionID", "smlPartWarehouseLocationID", "smlPartBinID", "smlShippedComplete", "smlOrgPartID", "smlOrgPartShortDescription",
			"smlUnitOfMeasure", "smlDescription", "smlPartLongDescriptionRTF", "smlPartLongDescriptionText", "smlUnitPrice", "smlUnitPriceForeign", "smlFreightAmount", "smlFreightAmountForeign", "smlPartGroupID", "smlWeight",
			"smlProjectID", "smlProjectAreaID", "smlKitPart"
		});
		MatchingFieldsInfo componentMatch = m1DataDictionary.FindMatchingFields("SalesOrderComponents", "ShipmentComponents", new string[13]
		{
			"omoSalesOrderID", "omoSalesOrderLineID", "omoSalesOrderDeliveryID", "omoSalesOrderComponentID", "omoPartID", "omoPartRevisionID", "omoPartWarehouseLocationID", "omoPartBinID", "omoQuantityPerParent", "omoAdditionalQuantity",
			"omoUnitOfMeasure", "omoDescription", "omoWeight"
		}, new string[13]
		{
			"smoSalesOrderID", "smoSalesOrderLineID", "smoSalesOrderDeliveryID", "smoSalesOrderComponentID", "smoPartID", "smoPartRevisionID", "smoPartWarehouseLocationID", "smoPartBinID", "smoQuantityPerParent", "smoAdditionalQuantity",
			"smoUnitOfMeasure", "smoDescription", "smoWeight"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select SalesOrderComponents.*, omoUniqueID from SalesOrderComponents inner join SalesOrderDeliveries on omdSalesOrderID=omoSalesOrderID and omdSalesOrderLineID=omoSalesOrderLineID And omdSalesOrderDeliveryID=omoSalesOrderDeliveryID where " + text + " and omdShippedComplete = 0 And omdDeliveryType <> 3 order by omoSalesOrderID,omoSalesOrderLineID,omoSalesOrderDeliveryID,omoSalesOrderComponentID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select omdDeliveryType,ompPlantID,ompPlantDepartmentID,ompClosed,omdUniqueID,ompUPSBillingOption,ompFedExBillingOption,ompUPSAccountNumber,ompFedexAccountNumber,ompResellerOrganizationID,ompResellerLocationID,ompResellerContactID,ompFedEx3rdPartyOrganizationID,ompFedEx3rdPartyLocationID,ompUPS3rdPartyOrganizationID,ompUPS3rdPartyLocationID" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from SalesOrderDeliveries inner join SalesOrderLines on omdSalesOrderID=omlSalesOrderID and omdSalesOrderLineID=omlSalesOrderLineID inner join SalesOrders on omdSalesOrderID = ompSalesOrderID where " + text + " order by omdDeliveryDate,omdSalesOrderID,omdSalesOrderLineID,omdSalesOrderDeliveryID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ShipmentLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("ShipmentComponents");
		string text2 = string.Empty;
		if (PromptFieldNames != null && PromptFieldNames.Length != 0 && PromptFieldNames[0].Equals("jmpJobID", StringComparison.CurrentCultureIgnoreCase) && promptFieldValues.Count != 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < promptFieldValues.Count; i++)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" Or ");
				}
				stringBuilder.Append("omjJobID = " + promptFieldValues[i][0].ToString().ToSql());
			}
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Insert(0, " And (");
				stringBuilder.Append(')');
			}
			text2 = stringBuilder.ToString();
		}
		DataTable dataTable3 = databaseForRow.GetDataTable("select distinct omjSalesOrderID,omjSalesOrderLineID,omjSalesOrderDeliveryID,omjJobID,Jobs.jmpClosed,Jobs.jmpReleasedToFloor,Jobs.jmpOnHold from SalesOrderJobLinks inner join SalesOrderDeliveries on omdSalesOrderID = omjSalesOrderID and omdSalesOrderLineID = omjSalesOrderLineID and omdSalesOrderDeliveryID = case when omjsalesorderdeliveryid=0 then omdsalesorderdeliveryid else omjsalesorderdeliveryid end inner join Jobs on omjJobId = jmpJobID where " + text + text2);
		foreach (DataRow row in dataTable2.Rows)
		{
			if (!checkConditionsDelivery(row, currentAsDataRow, messages))
			{
				continue;
			}
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			if (Convert.ToDecimal(currentAsDataRow["smpFreightCharge"]) != Convert.ToDecimal(row["ompFreightAmountBase"]))
			{
				currentAsDataRow["smpFreightCharge"] = row["ompFreightAmountBase"];
			}
			if (Convert.ToDecimal(currentAsDataRow["smpFreightChargeForeign"]) != Convert.ToDecimal(row["ompFreightAmountForeign"]))
			{
				currentAsDataRow["smpFreightChargeForeign"] = row["ompFreightAmountForeign"];
			}
			switch (Convert.ToInt32(row["omdDeliveryType"]))
			{
			case 1:
			{
				DataRow[] array = dataTable3.Select("omjSalesOrderID = " + row.Field<string>("omdSalesOrderID").ToLinq() + " And omjSalesOrderLineID = " + row["omdSalesOrderLineID"].ToLinq());
				if (array.Length == 0)
				{
					foreach (ProcessSelectedItemValues item in selectedItems)
					{
						if (item.KeyValues[0].Equals(row.Field<string>("omdSalesOrderID").Trim()) && Convert.ToInt32(item.KeyValues[1]) == Convert.ToInt32(row["omdSalesOrderLineID"]) && Convert.ToInt32(item.KeyValues[2]) == Convert.ToInt32(row["omdSalesOrderDeliveryID"]))
						{
							item.DiscardSave = true;
							break;
						}
					}
					messages.Add("Delivery " + row.Field<string>("omdSalesOrderID").Trim() + "/" + Convert.ToInt32(row["omdSalesOrderLineID"]).ToString().Trim() + "/" + Convert.ToInt32(row["omdSalesOrderDeliveryID"]).ToString().Trim() + " was not added because no related job could be found.");
					break;
				}
				DataRow[] array2 = array;
				foreach (DataRow dataRow2 in array2)
				{
					if ((Convert.ToInt32(dataRow2["omjSalesOrderDeliveryID"]) == 0 || Convert.ToInt32(dataRow2["omjSalesOrderDeliveryID"]) == Convert.ToInt32(row["omdSalesOrderDeliveryID"])) && checkConditionsJob(dataRow2, row, messages))
					{
						addShipmentLine(childBindingSource, row, dataRow2.Field<string>("omjJobID"), childBindingSource2, dataTable, matchingFieldsInfo2, componentMatch, GetItemValuesFromList(selectedItems, row));
					}
				}
				break;
			}
			case 2:
			case 4:
			case 5:
				addShipmentLine(childBindingSource, row, string.Empty, childBindingSource2, dataTable, matchingFieldsInfo2, componentMatch, GetItemValuesFromList(selectedItems, row));
				break;
			}
		}
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["smpPlantID"] = sourceHeaderRow["ompPlantID"];
		currentAsDataRow["smpPlantDepartmentID"] = sourceHeaderRow["ompPlantDepartmentID"];
		setCarrierAndResellerFields(sourceHeaderRow, currentAsDataRow);
	}

	private static void setCarrierAndResellerFields(DataRow sourceHeaderRow, DataRow shipmentRow)
	{
		if (!string.IsNullOrWhiteSpace((string)sourceHeaderRow["ompUPSBillingOption"]))
		{
			shipmentRow["smpUPSBillingOption"] = sourceHeaderRow["ompUPSBillingOption"];
		}
		if (!string.IsNullOrWhiteSpace((string)sourceHeaderRow["ompUPSAccountNumber"]))
		{
			shipmentRow["smpUPSAccountNumber"] = sourceHeaderRow["ompUPSAccountNumber"];
		}
		if (!string.IsNullOrWhiteSpace((string)sourceHeaderRow["ompUPS3rdPartyOrganizationID"]))
		{
			shipmentRow["smpUPS3rdPartyOrganizationID"] = sourceHeaderRow["ompUPS3rdPartyOrganizationID"];
			shipmentRow["smpUPS3rdPartyLocationID"] = sourceHeaderRow["ompUPS3rdPartyLocationID"];
		}
		if (!string.IsNullOrWhiteSpace((string)sourceHeaderRow["ompFedExBillingOption"]))
		{
			shipmentRow["smpFedExBillingOption"] = sourceHeaderRow["ompFedExBillingOption"];
		}
		if (!string.IsNullOrWhiteSpace((string)sourceHeaderRow["ompFedExAccountNumber"]))
		{
			shipmentRow["smpFedExAccountNumber"] = sourceHeaderRow["ompFedExAccountNumber"];
		}
		if (!string.IsNullOrWhiteSpace((string)sourceHeaderRow["ompFedEx3rdPartyOrganizationID"]))
		{
			shipmentRow["smpFedEx3rdPartyOrganizationID"] = sourceHeaderRow["ompFedEx3rdPartyOrganizationID"];
			shipmentRow["smpFedEx3rdPartyLocationID"] = sourceHeaderRow["ompFedEx3rdPartyLocationID"];
		}
		if (!string.IsNullOrWhiteSpace((string)sourceHeaderRow["ompResellerOrganizationID"]))
		{
			shipmentRow["smpBlindShipOrganizationID"] = sourceHeaderRow["ompResellerOrganizationID"];
			shipmentRow["smpBlindShipLocationID"] = sourceHeaderRow["ompResellerLocationID"];
			shipmentRow["smpBlindShipContactID"] = sourceHeaderRow["ompResellerContactID"];
		}
	}

	private void addShipmentLine(M1BindingSource bsShipmentLines, DataRow deliveryRow, string jobID, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo deliveryMatch, MatchingFieldsInfo componentMatch, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, deliveryRow, bsShipmentLines, deliveryMatch);
		dataRow["smlJobID"] = jobID;
		if (itemValues.EditableValues != null && itemValues.EditableValues.ContainsKey("QuantityShipped"))
		{
			decimal value = Convert.ToDecimal(itemValues.EditableValues["QuantityShipped"]);
			if (string.IsNullOrWhiteSpace(jobID))
			{
				dataRow.SetField("smlQuantityShipped", value);
			}
			else
			{
				dataRow.SetField("smlJobQuantityShipped", value);
			}
			if (itemValues.EditableValues.ContainsKey("ShippedComplete"))
			{
				dataRow.SetField("smlShippedComplete", Convert.ToBoolean(itemValues.EditableValues["ShippedComplete"]));
			}
		}
		dataRow["smlSourceTableName"] = "SalesOrderDeliveries";
		dataRow["smlSourceTableUniqueID"] = deliveryRow.Field<Guid>("omdUniqueID");
		if (Convert.ToDecimal(dataRow["smlFreightAmount"]) != Convert.ToDecimal(deliveryRow["omlFreightAmountBase"]))
		{
			dataRow["smlFreightAmount"] = deliveryRow["omlFreightAmountBase"];
		}
		if (Convert.ToDecimal(dataRow["smlFreightAmountForeign"]) != Convert.ToDecimal(deliveryRow["omlFreightAmountForeign"]))
		{
			dataRow["smlFreightAmountForeign"] = deliveryRow["omlFreightAmountForeign"];
		}
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtComponents.Select("omoSalesOrderID = " + deliveryRow.Field<string>("omdSalesOrderID").Trim().ToLinq() + " and omoSalesOrderLineID = " + Convert.ToInt32(deliveryRow["omdSalesOrderLineID"]).ToLinq() + " and omoSalesOrderDeliveryID = " + Convert.ToInt32(deliveryRow["omdSalesOrderDeliveryID"]).ToLinq());
		foreach (DataRow dataRow2 in array)
		{
			DataRow dataRow3 = TransferLineInfo(this, dataRow2, bsComponents, componentMatch, dataRow);
			dataRow3["smoSourceTableName"] = "SalesOrderComponents";
			dataRow3["smoSourceTableUniqueID"] = dataRow2.Field<Guid>("omoUniqueID");
		}
	}

	private bool checkConditionsDelivery(DataRow drDelivery, DataRow shipmentsRow, List<string> messages)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (Convert.ToBoolean(drDelivery["ompClosed"]))
		{
			stringBuilder.Append(", is closed");
		}
		string text = shipmentsRow.Field<string>("smpCustomerOrganizationID").Trim();
		if (text.Length != 0 && !text.Equals(drDelivery.Field<string>("ompCustomerOrganizationID").Trim(), StringComparison.CurrentCultureIgnoreCase))
		{
			stringBuilder.Append(", customer org is not " + text);
		}
		string text2 = shipmentsRow.Field<string>("smpShipOrganizationID").Trim();
		if (text2.Length != 0 && !text2.Equals(drDelivery.Field<string>("omdCustomerOrganizationID").Trim(), StringComparison.CurrentCultureIgnoreCase))
		{
			stringBuilder.Append(", ship org is not " + text2);
		}
		if (Convert.ToBoolean(drDelivery["omdShippedComplete"]))
		{
			stringBuilder.Append(", delivery is shipped complete");
		}
		if (Convert.ToInt32(drDelivery["omdDeliveryType"]) == 3)
		{
			stringBuilder.Append(", delivery type is progress payment");
		}
		string text3 = shipmentsRow.Field<string>("smpShipmentID").Trim();
		if (shipmentsRow.Field<bool>("smpPostedToGL"))
		{
			stringBuilder.Append(", destination shipment " + text3 + " is already posted");
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Remove(0, 2);
			messages.Add("Delivery " + drDelivery.Field<string>("omdSalesOrderID").Trim() + "/" + Convert.ToInt32(drDelivery["omdSalesOrderLineID"]).ToString().Trim() + "/" + Convert.ToInt32(drDelivery["omdSalesOrderDeliveryID"]).ToString().Trim() + " was not added because " + stringBuilder.ToString() + ".");
			return false;
		}
		return true;
	}

	private bool checkConditionsJob(DataRow jobLinkRow, DataRow drDelivery, List<string> messages)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = jobLinkRow.Field<string>("omjJobID").Trim();
		if (Convert.ToBoolean(jobLinkRow["jmpClosed"]))
		{
			stringBuilder.Append(", job " + text + " is closed");
		}
		if (!Convert.ToBoolean(jobLinkRow["jmpReleasedToFloor"]))
		{
			stringBuilder.Append(", job " + text + " is not released to floor");
		}
		if (Convert.ToBoolean(jobLinkRow["jmpOnHold"]))
		{
			stringBuilder.Append(", job " + text + " is on hold");
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Remove(0, 2);
			messages.Add("Delivery " + drDelivery.Field<string>("omdSalesOrderID").Trim() + "/" + Convert.ToInt32(drDelivery["omdSalesOrderLineID"]).ToString().Trim() + "/" + Convert.ToInt32(drDelivery["omdSalesOrderDeliveryID"]).ToString().Trim() + " was not added because " + stringBuilder.ToString() + ".");
			return false;
		}
		return true;
	}
}
