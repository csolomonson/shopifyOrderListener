using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferWHRequisitionToWHTransferProcess : ProcessParameters
{
	public TransferWHRequisitionToWHTransferProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "wqlWarehouseRequisitionID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "wqlWarehouseRequisitionID", "wqlWarehouseRequisitionLineID" };
		KeyValueTableName = "WarehouseRequisitionLines";
		Description = "Select the warehouse requisition lines to be transferred.";
		GridID = "M1ADDFROMWHTRANSFERWHREQ";
		BindingSourceTable = "WarehouseTransfers";
		HelpLink = "QM_TransferWHRequisitionToWHTransfer.htm";
		if (BindingSource != null && !string.IsNullOrWhiteSpace(BindingSource.CurrentAsDataRow.Field<string>("mwpSourceWarehouseID")))
		{
			PromptFieldValidations.Add(new PromptFieldValidationString("wqpSourceWarehouseID", BindingSource.CurrentAsDataRow.Field<string>("mwpSourceWarehouseID"), $"Source warehouse is different."));
		}
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Show requisitions not transferred only?")
		{
			Value = true,
			AdoFilterExpression = "wqlTransferredComplete = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "wqlTransferredComplete"
		});
		HeaderSourceFields = new string[4] { "wqpSourceWarehouseID", "wqpDestinationWarehouseID", "wqpShippingMethodID", "wqpShippingPaymentTypeID" };
		HeaderDestinationFields = new string[4] { "mwpSourceWarehouseID", "mwpDestinationWarehouseID", "mwpShippingMethodID", "mwpShippingPaymentTypeID" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.Messages;
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
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("WarehouseRequisitions", "WarehouseTransfers", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("WarehouseRequisitionLines", "WarehouseTransferLines", new string[8] { "wqlWarehouseRequisitionID", "wqlWarehouseRequisitionLineID", "wqlPartID", "wqlPartRevisionID", "wqlSourceWarehouseID", "wqlUnitOfMeasure", "wqlPartDescription", "wqlKitPart" }, new string[8] { "mwlWarehouseRequisitionID", "mwlWarehouseRequisitionLineID", "mwlPartID", "mwlPartRevisionID", "mwlSourceWarehouseID", "mwlUnitOfMeasure", "mwlPartDescription", "mwlKitPart" });
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("WarehouseRequisitionComponents, WarehouseRequisitionLines", "WarehouseTransferComponents", new string[11]
		{
			"wqoWarehouseRequisitionID", "wqoWarehouseRequisitionLineID", "wqoWarehouseReqComponentID", "wqoPartID", "wqoPartRevisionID", "wqoSourceWarehouseID", "wqoQuantityPerParent", "wqoAdditionalQuantity", "wqoUnitOfMeasure", "wqoDescription",
			"wqoWeight"
		}, new string[11]
		{
			"mwoWarehouseRequisitionID", "mwoWarehouseRequisitionLineID", "mwoWarehouseReqComponentID", "mwoPartID", "mwoPartRevisionID", "mwoSourceWarehouseID", "mwoQuantityPerParent", "mwoAdditionalQuantity", "mwoUnitOfMeasure", "mwoDescription",
			"mwoWeight"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select wqlRequestedQuantity, wqlQuantityTransferred" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from WarehouseRequisitionLines inner join WarehouseRequisitions on wqlWarehouseRequisitionID = wqpWarehouseRequisitionID  where " + text + " order by wqlWarehouseRequisitionID, wqlWarehouseRequisitionLineID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from WarehouseRequisitionComponents inner join WarehouseRequisitionLines on wqoWarehouseRequisitionID=wqlWarehouseRequisitionID and wqoWarehouseRequisitionLineID=wqlWarehouseRequisitionLineID where " + text + " and wqlTransferredComplete = 0 order by wqoWarehouseRequisitionID, wqoWarehouseRequisitionLineID, wqoWarehouseReqComponentID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("WarehouseTransferLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("WarehouseTransferComponents");
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addLine(childBindingSource, row, currentAsDataRow, childBindingSource2, dataTable2, matchingFieldsInfo2, matchingFieldsInfo3, GetItemValuesFromList(selectedItems, row));
		}
	}

	private void addLine(M1BindingSource bsWHTransferLines, DataRow whReqRow, DataRow WHTransferRow, M1BindingSource bsComponents, DataTable dtWHReqComponents, MatchingFieldsInfo whReqLineMatches, MatchingFieldsInfo componentMatch, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, whReqRow, bsWHTransferLines, whReqLineMatches);
		decimal num = default(decimal);
		if (itemValues.EditableValues != null && itemValues.EditableValues.ContainsKey("QtyShipped"))
		{
			num = Convert.ToDecimal(itemValues.EditableValues["QtyShipped"]);
			dataRow.SetField("mwlShipQuantity", num);
		}
		if (itemValues.EditableValues.ContainsKey("TransferredComplete"))
		{
			dataRow.SetField("mwlShippedComplete", Convert.ToBoolean(itemValues.EditableValues["TransferredComplete"]));
		}
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtWHReqComponents.Select("wqoWarehouseRequisitionID = " + dataRow.Field<string>("mwlWarehouseRequisitionID").Trim().ToLinq() + " and wqoWarehouseRequisitionLineID = " + Convert.ToInt32(dataRow["mwlWarehouseRequisitionLineID"]).ToLinq());
		foreach (DataRow sourceLineRow in array)
		{
			TransferLineInfo(this, sourceLineRow, bsComponents, componentMatch, dataRow);
		}
	}
}
