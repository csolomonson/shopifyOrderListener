using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferSalesOrderToRMAClaimProcess : ProcessParameters
{
	public TransferSalesOrderToRMAClaimProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "omdSalesOrderID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[3] { "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID" };
		KeyValueTableName = "SalesOrderDeliveries";
		Description = "Use this screen to create rma claims from a sales order.";
		GridID = "M1ADDFROMRMACLAIMSO";
		BindingSourceTable = "RMAClaims";
		HelpLink = "QM_TransferSalesOrderToRMAClaim.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("omdClosed", fieldValue: false, "Sales Order is closed."));
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Order Dates")
		{
			IgnoreWhenEmpty = true,
			ValueField = "ompOrderDate",
			AdditionalFields = "ompOrderDate"
		});
		HeaderSourceFields = new string[16]
		{
			"ompCustomerOrganizationID", "ompARInvoiceLocationID", "ompARInvoiceContactID", "ompShipOrganizationID=Case When omdDifferentLocation = 0 Then ompShipOrganizationID Else omdCustomerOrganizationID End", "ompShipLocationID=Case When omdDifferentLocation = 0 Then ompShipLocationID Else omdShipLocationID End", "ompShipContactID=Case When omdDifferentLocation = 0 Then ompShipContactID Else omdShipContactID End", "ompOrderDate", "ompResellerOrganizationID", "ompResellerLocationID", "ompResellerContactID",
			"ompPlantID", "ompPlantDepartmentID", "ompCurrencyRateID", "ompCustomRate", "ompExchangeRate", "ompProjectID"
		};
		HeaderDestinationFields = new string[16]
		{
			"rapCustomerOrganizationID", "rapARInvoiceLocationID", "rapARInvoiceContactID", "rapShipOrganizationID", "rapShipLocationID", "rapShipContactID", "rapClaimDate", "rapResellerOrganizationID", "rapResellerLocationID", "rapResellerContactID",
			"rapPlantID", "rapPlantDepartmentID", "rapCurrencyRateID", "rapCustomRate", "rapExchangeRate", "rapProjectID"
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
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("SalesOrders", "RMAClaims", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("SalesOrderDeliveries,SalesOrderLines,ShipmentLines", "RMAClaimLines", new string[16]
		{
			"omlPartID", "omlPartRevisionID", "omdPartWarehouseLocationID", "omdPartBinID", "omlOrgPartID", "omlOrgPartShortDescription", "omlUnitOfMeasure", "omlPartShortDescription", "omlPartLongDescriptionRTF", "omlPartLongDescriptionText",
			"omlPartGroupID", "omlProjectID", "omlProjectAreaID", "omdSalesOrderID", "omdSalesOrderLineID", "omdSalesOrderDeliveryID"
		}, new string[16]
		{
			"ralPartID", "ralPartRevisionID", "ralPartWarehouseLocationID", "ralPartBinID", "ralOrgPartID", "ralOrgPartShortDescription", "ralUnitOfMeasure", "ralPartShortDescription", "ralPartLongDescriptionRTF", "ralPartLongDescriptionText",
			"ralPartGroupID", "ralProjectID", "ralProjectAreaID", "ralSalesOrderID", "ralSalesOrderLineID", "ralSalesOrderDeliveryID"
		});
		MatchingFieldsInfo componentMatch = m1DataDictionary.FindMatchingFields("SalesOrderComponents", "RMAClaimComponents", new string[9] { "omoPartID", "omoPartRevisionID", "omoPartWarehouseLocationID", "omoPartBinID", "omoQuantityPerParent", "omoAdditionalQuantity", "omoUnitOfMeasure", "omoDescription", "omoWeight" }, new string[9] { "raoPartID", "raoPartRevisionID", "raoPartWarehouseLocationID", "raoPartBinID", "raoQuantityPerParent", "raoAdditionalQuantity", "raoUnitOfMeasure", "raoDescription", "raoWeight" });
		DataTable dataTable = databaseForRow.GetDataTable("select SalesOrderComponents.*, omoUniqueID, smoShipmentID, smoShipmentLineID, smoShipmentComponentID from SalesOrderComponents  inner join SalesOrderDeliveries on omdSalesOrderID=omoSalesOrderID and omdSalesOrderLineID=omoSalesOrderLineID And omdSalesOrderDeliveryID=omoSalesOrderDeliveryID  left join ShipmentComponents on smoSalesOrderID = omoSalesOrderID and smoSalesOrderLineID = omoSalesOrderLineID and smoSalesOrderDeliveryID = omoSalesOrderDeliveryID and smoSalesOrderComponentID = omoSalesOrderComponentID  where " + text + " And omdDeliveryType <> 3 order by omoSalesOrderID,omoSalesOrderLineID,omoSalesOrderDeliveryID,omoSalesOrderComponentID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select omdDeliveryType,omlUnitPriceForeign,omlFreightAmountForeign,omlUnitPriceBase,omlFreightAmountBase,omlFullUnitPriceBase,omlUnitDiscountBase,omlDiscountPercent,omlFullUnitPriceForeign,ompFreightAmountForeign,ompFreightAmountBase,smlShipmentID,smlShipmentLineID" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from SalesOrderDeliveries inner join SalesOrderLines on omdSalesOrderID=omlSalesOrderID And omdSalesOrderLineID=omlSalesOrderLineID Inner Join SalesOrders On ompSalesOrderID = omlSalesOrderID left join ShipmentLines on smlSalesOrderID = omdSalesOrderID and smlSalesOrderLineID = omdSalesOrderLineID and smlSalesOrderDeliveryID = omdSalesOrderDeliveryID where " + text + " order by omdSalesOrderID,omdSalesOrderLineID,omdSalesOrderDeliveryID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("RMAClaimLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("RMAClaimComponents");
		foreach (DataRow row in dataTable2.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addClaimLine(databaseForRow, currentAsDataRow, childBindingSource, row, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row), childBindingSource2, dataTable, componentMatch);
		}
		BindingSource.OnDataChanged(3);
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		if (HeaderFixForeign)
		{
			currentAsDataRow["rapFreightAmountForeign"] = sourceHeaderRow["ompFreightAmountForeign"];
		}
		else
		{
			currentAsDataRow["rapFreightAmount"] = sourceHeaderRow["ompFreightAmountBase"];
		}
	}

	private void addClaimLine(M1Database database, DataRow claimRow, M1BindingSource bsClaimLines, DataRow deliveryRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo componentMatch)
	{
		DataRow dataRow = TransferLineInfo(this, deliveryRow, bsClaimLines, lineMatches, claimRow);
		decimal num = default(decimal);
		if (itemValues.EditableValues != null && itemValues.EditableValues.ContainsKey("QtyToClaim"))
		{
			num = Convert.ToDecimal(itemValues.EditableValues["QtyToClaim"]);
		}
		dataRow["ralDiscountPercent"] = deliveryRow["omlDiscountPercent"];
		dataRow["ralFullUnitPriceBase"] = deliveryRow["omlFullUnitPriceBase"];
		dataRow["ralUnitDiscountBase"] = deliveryRow["omlUnitDiscountBase"];
		dataRow["ralUnitPrice"] = deliveryRow["omlUnitPriceBase"];
		dataRow["ralQuantity"] = num;
		if (!string.IsNullOrWhiteSpace(Convert.ToString(deliveryRow["smlShipmentID"])))
		{
			dataRow["ralShipmentID"] = deliveryRow["smlShipmentID"];
			dataRow["ralShipmentLineID"] = deliveryRow["smlShipmentLineID"];
		}
		PartCost partCosts = new Part().GetPartCosts(database, null, dataRow.Field<string>("ralPartID"), dataRow.Field<string>("ralPartRevisionID"));
		if (partCosts != null)
		{
			decimal num2 = partCosts.MaterialCost + partCosts.LaborCost + partCosts.OverheadCost + partCosts.SubcontractCost + partCosts.DutyCost + partCosts.MiscCost + partCosts.FreightCost;
			decimal num3 = claimRow.Field<decimal>("rapExchangeRate");
			dataRow.SetField("ralUnitCost", num2);
			dataRow.SetField("ralUnitCostForeign", Math.Round(num2 * num3, 5));
			dataRow.SetField("ralExtendedCost", Math.Round(dataRow.Field<decimal>("ralUnitCost") * dataRow.Field<decimal>("ralQuantity"), 2));
			dataRow.SetField("ralExtendedCostForeign", Math.Round(dataRow.Field<decimal>("ralUnitCostForeign") * dataRow.Field<decimal>("ralQuantity"), 2));
		}
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtComponents.Select("omoSalesOrderID = " + deliveryRow.Field<string>("omdSalesOrderID").Trim().ToLinq() + " and omoSalesOrderLineID = " + Convert.ToInt32(deliveryRow["omdSalesOrderLineID"]).ToLinq() + " and omoSalesOrderDeliveryID = " + Convert.ToInt32(deliveryRow["omdSalesOrderDeliveryID"]).ToLinq());
		foreach (DataRow dataRow2 in array)
		{
			DataRow dataRow3 = TransferLineInfo(this, dataRow2, bsComponents, componentMatch, dataRow);
			if (!string.IsNullOrWhiteSpace(Convert.ToString(dataRow2["smoShipmentID"])))
			{
				dataRow3["raoShipmentID"] = dataRow2["smoShipmentID"];
				dataRow3["raoShipmentLineID"] = dataRow2["smoShipmentLineID"];
				dataRow3["raoShipmentComponentID"] = dataRow2["smoShipmentComponentID"];
			}
		}
	}
}
