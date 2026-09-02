using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferWHTransferToWHReceiptProcess : ProcessParameters
{
	public TransferWHTransferToWHReceiptProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "mwlWarehouseTransferID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "mwlWarehouseTransferID", "mwlWarehouseTransferLineID" };
		KeyValueTableName = "WarehouseTransferLines";
		Description = "Select the warehouse transfer lines to be receipted.";
		GridID = "M1ADDFROMWHRECEIPTWHTRANSFER";
		BindingSourceTable = "WarehouseReceipts";
		HelpLink = "QM_TransferWHReceiptToWHTransfer.htm";
		if (BindingSource != null && !string.IsNullOrWhiteSpace(BindingSource.CurrentAsDataRow.Field<string>("wrpDestinationWarehouseID")))
		{
			PromptFieldValidations.Add(new PromptFieldValidationString("mwpDestinationWarehouseID", BindingSource.CurrentAsDataRow.Field<string>("wrpDestinationWarehouseID"), $"Destination warehouse is different."));
		}
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Show transfers not receipted only?")
		{
			Value = true,
			AdoFilterExpression = "mwlReceivedComplete = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "mwlReceivedComplete"
		});
		PromptFieldValidations.Add(new PromptFieldValidationBool("mwpPosted", fieldValue: true, "Warehouse Transfer is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("mwpReversalEntry", fieldValue: false, "Warehouse Transfer is a reversal."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("mwpReversed", fieldValue: false, "Warehouse Transfer has already been reversed."));
		HeaderSourceFields = new string[5] { "mwpSourceWarehouseID", "mwpDestinationWarehouseID", "mwpShippingMethodID", "mwpShippingPaymentTypeID", "mwpFreightCharge" };
		HeaderDestinationFields = new string[5] { "wrpSourceWarehouseID", "wrpDestinationWarehouseID", "wrpShippingMethodID", "wrpShippingPaymentTypeID", "wrpFreightCharge" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Lot, value: false);
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Serial, value: false);
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("WarehouseTransfers", "WarehouseReceipts", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("WarehouseTransferLines", "WarehouseReceiptLines", new string[12]
		{
			"mwlWarehouseTransferID", "mwlWarehouseTransferLineID", "mwlWarehouseRequisitionID", "mwlWarehouseRequisitionLineID", "mwlPartID", "mwlPartRevisionID", "mwlSourceWarehouseID", "mwlSourcePartBinID", "mwlDestinationWarehouseID", "mwlUnitOfMeasure",
			"mwlPartDescription", "mwlKitPart"
		}, new string[12]
		{
			"wrlWarehouseTransferID", "wrlWarehouseTransferLineID", "wrlWarehouseRequisitionID", "wrlWarehouseRequisitionLineID", "wrlPartID", "wrlPartRevisionID", "wrlSourceWarehouseID", "wrlSourcePartBinID", "wrlDestinationWarehouseID", "wrlUnitOfMeasure",
			"wrlPartDescription", "wrlKitPart"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("WarehouseTransferComponents, WarehouseTransferLines", "WarehouseReceiptComponents", new string[16]
		{
			"mwoWarehouseTransferID", "mwoWarehouseTransferLineID", "mwoWarehouseTransComponentID", "mwoWarehouseRequisitionID", "mwoWarehouseRequisitionLineID", "mwoWarehouseReqComponentID", "mwoPartID", "mwoPartRevisionID", "mwoSourceWarehouseID", "mwoSourcePartBinID",
			"mwoDestinationWarehouseID", "mwoQuantityPerParent", "mwoAdditionalQuantity", "mwoUnitOfMeasure", "mwoDescription", "mwoWeight"
		}, new string[16]
		{
			"wroWarehouseTransferID", "wroWarehouseTransferLineID", "wroWarehouseTransComponentID", "wroWarehouseRequisitionID", "wroWarehouseRequisitionLineID", "wroWarehouseReqComponentID", "wroPartID", "wroPartRevisionID", "wroSourceWarehouseID", "wroSourcePartBinID",
			"wroDestinationWarehouseID", "wroQuantityPerParent", "wroAdditionalQuantity", "wroUnitOfMeasure", "wroDescription", "wroWeight"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select mwlShipQuantity, mwlReceivedQuantity, mwlUniqueID" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from WarehouseTransferLines inner join WarehouseTransfers on mwlWarehouseTransferID = mwpWarehouseTransferID  where " + text + " and mwpPosted = 1 AND mwpReversalEntry = 0 AND mwpReversed = 0 order by mwlWarehouseTransferID, mwlWarehouseTransferLineID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select mwoUniqueID, " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from WarehouseTransferComponents inner join WarehouseTransferLines on mwoWarehouseTransferID=mwlWarehouseTransferID and mwoWarehouseTransferLineID=mwlWarehouseTransferLineID where " + text + " and mwlReceivedComplete = 0 order by mwoWarehouseTransferID, mwoWarehouseTransferLineID, mwoWarehouseTransComponentID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("WarehouseReceiptLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("WarehouseReceiptComponents");
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addLine(childBindingSource, row, currentAsDataRow, childBindingSource2, dataTable2, matchingFieldsInfo2, matchingFieldsInfo3, GetItemValuesFromList(selectedItems, row));
		}
	}

	private void addLine(M1BindingSource bsWHReceiptLines, DataRow whTransRow, DataRow WHReceiptRow, M1BindingSource bsComponents, DataTable dtWHTransComponents, MatchingFieldsInfo whTransLineMatches, MatchingFieldsInfo componentMatch, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, whTransRow, bsWHReceiptLines, whTransLineMatches);
		string value = string.Empty;
		if (itemValues.EditableValues != null)
		{
			if (itemValues.EditableValues.ContainsKey("QtyReceived"))
			{
				decimal value2 = Convert.ToDecimal(itemValues.EditableValues["QtyReceived"]);
				dataRow.SetField("wrlQuantityReceived", value2);
			}
			if (itemValues.EditableValues.ContainsKey("ReceivedComplete"))
			{
				dataRow.SetField("wrlReceivedComplete", Convert.ToBoolean(itemValues.EditableValues["ReceivedComplete"]));
			}
			if (itemValues.EditableValues.ContainsKey("DestinationBin"))
			{
				value = Convert.ToString(itemValues.EditableValues["DestinationBin"]);
				dataRow.SetField("wrlDestinationPartBinID", value);
			}
		}
		dataRow["wrlSourceTableName"] = "WarehouseTransferLines";
		dataRow["wrlSourceTableUniqueID"] = whTransRow.Field<Guid>("mwlUniqueID");
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtWHTransComponents.Select("mwoWarehouseTransferID = " + dataRow.Field<string>("wrlWarehouseTransferID").Trim().ToLinq() + " and mwoWarehouseTransferLineID = " + Convert.ToInt32(dataRow["wrlWarehouseTransferLineID"]).ToLinq());
		foreach (DataRow dataRow2 in array)
		{
			DataRow dataRow3 = TransferLineInfo(this, dataRow2, bsComponents, componentMatch, dataRow);
			dataRow3["wroDestinationPartBinID"] = value;
			dataRow3["wroSourceTableName"] = "WarehouseTransferComponents";
			dataRow3["wroSourceTableUniqueID"] = dataRow2.Field<Guid>("mwoUniqueID");
		}
	}
}
