using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferDMRClaimToDMRShipmentProcess : ProcessParameters
{
	public TransferDMRClaimToDMRShipmentProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "dmlDMRClaimID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "dmlDMRClaimID", "dmlDMRClaimLineID" };
		KeyValueTableName = "DMRClaimLines";
		Description = "Select the DMR claim lines to be shipped.";
		GridID = "M1ADDFROMDMRSHIPMENTDMRCLAIM";
		BindingSourceTable = "DMRShipments";
		HelpLink = "QM_TransferDMRClaimToDMRShipment.htm";
		HeaderSourceFields = new string[10] { "dmpSupplierOrganizationID", "dmpAPInvoiceLocationID", "dmpPurchaseLocationID", "dmpPurchaseContactID", "dmpProjectID", "dmpPlantID", "dmpPlantDepartmentID", "dmpCurrencyRateID", "dmpCustomRate", "dmpExchangeRate" };
		HeaderDestinationFields = new string[10] { "dspSupplierOrganizationID", "dspAPInvoiceLocationID", "dspShipLocationID", "dspShipContactID", "dspProjectID", "dspPlantID", "dspPlantDepartmentID", "dspCurrencyRateID", "dspCustomRate", "dspExchangeRate" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		arg.FilterErrorRegex.Add(" is inactive and has no quantity on hand or quantity to inspect.");
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
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("DMRClaims", "DMRShipments", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("DMRClaimLines", "DMRShipmentLines", new string[21]
		{
			"dmlDMRClaimID", "dmlDMRClaimLineID", "dmlInspectionID", "dmlInspectionLineID", "dmlJobID", "dmlJobAssemblyID", "dmlJobMaterialID", "dmlJobOperationID", "dmlPartID", "dmlPartRevisionID",
			"dmlPartWarehouseLocationID", "dmlPartBinID", "dmlUnitOfMeasure", "dmlPartShortDescription", "dmlPartLongDescriptionRTF", "dmlPartLongDescriptionText", "dmlTransferredToDMRShipment", "dmlProjectID", "dmlProjectAreaID", "dmlConversionFactor",
			"dmlKitPart"
		}, new string[21]
		{
			"dslDMRClaimID", "dslDMRClaimLineID", "dslInspectionID", "dslInspectionLineID", "dslJobID", "dslJobAssemblyID", "dslJobMaterialID", "dslJobOperationID", "dslPartID", "dslPartRevisionID",
			"dslPartWarehouseLocationID", "dslPartBinID", "dslUnitOfMeasure", "dslDescription", "dslPartLongDescriptionRTF", "dslPartLongDescriptionText", "dslShippedComplete", "dslProjectID", "dslProjectAreaID", "dslConversionFactor",
			"dslKitPart"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("DMRClaimComponents, DMRClaimLines", "DMRShipmentComponents", new string[19]
		{
			"dmoDMRClaimID", "dmoDMRClaimLineID", "dmoDMRClaimComponentID", "dmoInspectionID", "dmoInspectionLineID", "dmoInspectionComponentID", "dmoJobID", "dmoJobAssemblyID", "dmoJobMaterialID", "dmoJobMaterialComponentID",
			"dmoPartID", "dmoPartRevisionID", "dmoPartWarehouseLocationID", "dmoPartBinID", "dmoQuantityPerParent", "dmoAdditionalQuantity", "dmoUnitOfMeasure", "dmoDescription", "dmoWeight"
		}, new string[19]
		{
			"dsoDMRClaimID", "dsoDMRClaimLineID", "dsoDMRClaimComponentID", "dsoInspectionID", "dsoInspectionLineID", "dsoInspectionComponentID", "dsoJobID", "dsoJobAssemblyID", "dsoJobMaterialID", "dsoJobMaterialComponentID",
			"dsoPartID", "dsoPartRevisionID", "dsoPartWarehouseLocationID", "dsoPartBinID", "dsoQuantityPerParent", "dsoAdditionalQuantity", "dsoUnitOfMeasure", "dsoDescription", "dsoWeight"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select dmlUnitCost, dmlUnitCostForeign " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from DMRClaimLines inner join DMRClaims on dmlDMRClaimID = dmpDMRClaimID  where " + text + " order by dmlDMRClaimID, dmlDMRClaimLineID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from DMRClaimComponents inner join DMRClaimLines on dmoDMRClaimID=dmlDMRClaimID and dmoDMRClaimLineID=dmlDMRClaimLineID where " + text + " and dmlInvoicedComplete = 0 order by dmoDMRClaimID,dmoDMRClaimLineID,dmoDMRClaimComponentID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("DMRShipmentLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("DMRShipmentComponents");
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addDMRShipmentLine(childBindingSource, row, currentAsDataRow, childBindingSource2, dataTable2, matchingFieldsInfo2, matchingFieldsInfo3, GetItemValuesFromList(selectedItems, row));
		}
	}

	private void addDMRShipmentLine(M1BindingSource bsDMRShipmentLines, DataRow claimRow, DataRow dmrShipmentRow, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo claimLinematches, MatchingFieldsInfo componentMatch, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, claimRow, bsDMRShipmentLines, claimLinematches);
		_ = dmrShipmentRow.Field<decimal>("dspExchangeRate") == 0m;
		if (!HeaderFixForeign)
		{
			dataRow.SetField("dslUnitPrice", claimRow.Field<decimal>("dmlUnitCost"));
		}
		else
		{
			dataRow.SetField("dslUnitPriceForeign", claimRow.Field<decimal>("dmlUnitcostForeign"));
		}
		decimal num = default(decimal);
		if (itemValues.EditableValues != null && itemValues.EditableValues.ContainsKey("QuantityShipped"))
		{
			num = Convert.ToDecimal(itemValues.EditableValues["QuantityShipped"]);
			dataRow.SetField("dslQuantityShipped", num);
		}
		if (itemValues.EditableValues.ContainsKey("ShippedComplete"))
		{
			dataRow.SetField("dslShippedComplete", Convert.ToBoolean(itemValues.EditableValues["ShippedComplete"]));
		}
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtComponents.Select("dmoDMRClaimID = " + dataRow.Field<string>("dslDMRClaimID").Trim().ToLinq() + " and dmoDMRClaimLineID = " + Convert.ToInt32(dataRow["dslDMRClaimLineID"]).ToLinq());
		foreach (DataRow sourceLineRow in array)
		{
			TransferLineInfo(this, sourceLineRow, bsComponents, componentMatch, dataRow);
		}
	}
}
