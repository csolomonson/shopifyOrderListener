using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferPurchaseOrderToReceiptProcess : ProcessParameters
{
	public TransferPurchaseOrderToReceiptProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "pmlPurchaseOrderID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "pmlPurchaseOrderID", "pmlPurchaseOrderLineID" };
		KeyValueTableName = "PurchaseOrderLines";
		Description = "Select the purchase order lines to be receipted.";
		CreatedBindingSourceCaption = "Create Receipt from PO";
		GridID = "M1ADDFROMRECEIPTPO";
		BindingSourceTable = "Receipts";
		HelpLink = "PM_TransferPurchaseOrderToReceipt.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("pmpClosed", fieldValue: false, "Purchase Order is closed."));
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Exclude purchase order line records already received complete?")
		{
			Value = true,
			AdoFilterExpression = "pmlReceivedComplete = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "pmlReceivedComplete"
		});
		HeaderSourceFields = new string[13]
		{
			"pmpSupplierOrganizationID", "pmpAPInvoiceLocationID", "pmpAPInvoiceContactID", "pmpPurchaseLocationID", "pmpPurchaseContactID", "pmpShippingMethodID", "pmpCurrencyRateID", "pmpCustomRate", "pmpExchangeRate", "pmpProjectID",
			"pmpPlantID", "pmpPlantDepartmentID", "pmpLandedCost"
		};
		HeaderDestinationFields = new string[13]
		{
			"rmpSupplierOrganizationID", "rmpAPInvoiceLocationID", "rmpAPInvoiceContactID", "rmpPurchaseLocationID", "rmpPurchaseContactID", "rmpShippingMethodID", "rmpCurrencyRateID", "rmpCustomRate", "rmpExchangeRate", "rmpProjectID",
			"rmpPlantID", "rmpPlantDepartmentID", "rmpLandedCost"
		};
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		arg.FilterErrorRegex.Add("' is inactive.");
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Lot, value: false);
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Serial, value: false);
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		BindingSourceTable = "Receipts";
		if (BindingSource == null)
		{
			BindingSource = new M1BindingSource(ServiceProvider);
			BindingSource.LoadDefinition(string.Empty, BindingSourceTable, null);
			BindingSource.AddNew();
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("PurchaseOrders", "Receipts", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("PurchaseOrderLines, PurchaseOrders, PartRevisions", "ReceiptLines", new string[7] { "pmlPurchaseOrderID", "pmlPurchaseOrderLineID", "pmlRMAClaimID", "pmlRMAClaimLineID", "pmlForm1099Box", "pmlReceivedComplete", "pmlKitPart" }, new string[7] { "rmlPurchaseOrderID", "rmlPurchaseOrderLineID", "rmlRMAClaimID", "rmlRMAClaimLineID", "rmlForm1099Box", "rmlPOReceivedComplete", "rmlKitPart" });
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("PurchaseOrderComponents, PurchaseOrderLines", "ReceiptComponents", new string[18]
		{
			"pmoJobID", "pmoJobAssemblyID", "pmoJobMaterialID", "pmoJobMaterialComponentID", "pmoPurchaseOrderID", "pmoPurchaseOrderLineID", "pmoPurchaseOrderComponentID", "pmoPartID", "pmoPartRevisionID", "pmoPartWarehouseLocationID",
			"pmoPartBinID", "pmoQuantityPerParent", "pmoAdditionalQuantity", "pmoUnitOfMeasure", "pmoDescription", "pmoWeight", "pmoPurchaseUnitCost", "pmoPurchaseUnitCostForeign"
		}, new string[18]
		{
			"rmoJobID", "rmoJobAssemblyID", "rmoJobMaterialID", "rmoJobMaterialComponentID", "rmoPurchaseOrderID", "rmoPurchaseOrderLineID", "rmoPurchaseOrderComponentID", "rmoPartID", "rmoPartRevisionID", "rmoPartWarehouseLocationID",
			"rmoPartBinID", "rmoQuantityPerParent", "rmoAdditionalQuantity", "rmoUnitOfMeasure", "rmoDescription", "rmoWeight", "rmoPurchaseUnitCost", "rmoPurchaseUnitCostForeign"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from PurchaseOrderComponents inner join PurchaseOrderLines on pmlPurchaseOrderID=pmoPurchaseOrderID and pmlPurchaseOrderLineID=pmoPurchaseOrderLineID where " + text + " and pmlReceivedComplete = 0 order by pmoPurchaseOrderID,pmoPurchaseOrderLineID,pmoPurchaseOrderComponentID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select pmpClosed,  pmlJobID, pmlJobAssemblyID, pmlJobType, pmlJobMaterialID, pmlJobOperationID,  pmlPartID, pmlPartRevisionID, pmlOrgPartID, pmlOrgPartShortDescription, pmlPartWarehouseLocationID, pmlPartBinID, pmlPartShortDescription, pmlPartLongDescriptionRTF, pmlPartLongDescriptionText, pmlPurchaseUnitOfMeasure, pmlInventoryUnitOfMeasure, pmlRequiresInspection, imrInspectionNotesRTF, imrInspectionNotesText,  pmlProjectID, pmlProjectAreaID, pmlSalesOrderID, pmlSalesOrderLineID, pmlSalesOrderDeliveryID, pmlPurchaseType, pmlConversionFactor, pmlSetupChargeBase, pmlSetupChargeForeign, pmlPurchaseUnitCostBase, pmlPurchaseUnitCostForeign, imrRequiresInspection,imrInspectionNotesRTF,imrInspectionNotesText, IsNull((Select Sum(rmlPurchaseQuantityReceived) From ReceiptLines Where rmlPurchaseOrderID = pmlPurchaseOrderID And rmlPurchaseOrderLineID = pmlPurchaseOrderLineID),0) As ReceivedQty,  isnull(jmpClosed,0) as jmpClosed,  isnull(prpClosed,0) as prpClosed,  isnull(ompClosed,0) as ompClosed,  isnull(impInactive,0) as impInactive " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from PurchaseOrderLines inner join PurchaseOrders on pmlPurchaseOrderID = pmpPurchaseOrderID  left outer join PartRevisions On pmlPartID = imrPartID And pmlPartRevisionID = imrPartRevisionID  left outer join Parts On pmlPartID = impPartID  left outer join PartClasses On impPartClassID = imcPartClassID  left outer join Jobs on jmpjobid = pmljobid  left outer join SalesOrders on ompsalesorderid = pmlsalesorderid  left outer join Projects on prpprojectid = pmlprojectid  where " + text + " order by pmlPurchaseOrderID,pmlPurchaseOrderLineID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ReceiptLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("ReceiptComponents");
		MultipleDestinationRowsCreated = false;
		foreach (DataRow row in dataTable2.Rows)
		{
			if (checkConditionsPO(row, currentAsDataRow, messages))
			{
				CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
				addReceiptLine(childBindingSource, row, currentAsDataRow, childBindingSource2, dataTable, matchingFieldsInfo2, matchingFieldsInfo3, GetItemValuesFromList(selectedItems, row));
			}
		}
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["rmpPlantID"] = sourceHeaderRow["pmpPlantID"];
		currentAsDataRow["rmpPlantDepartmentID"] = sourceHeaderRow["pmpPlantDepartmentID"];
	}

	private void addReceiptLine(M1BindingSource bsReceiptLines, DataRow poRow, DataRow receiptRow, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo orderLinematches, MatchingFieldsInfo componentMatch, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, poRow, bsReceiptLines, orderLinematches);
		_ = receiptRow.Field<decimal>("rmpExchangeRate") == 0m;
		if (!poRow.Field<bool>("jmpClosed"))
		{
			dataRow["rmlJobID"] = poRow["pmlJobID"];
			dataRow["rmlJobAssemblyID"] = poRow["pmlJobAssemblyID"];
			dataRow["rmlJobType"] = poRow["pmlJobType"];
			dataRow["rmlJobMaterialID"] = poRow["pmlJobMaterialID"];
			dataRow["rmlJobOperationID"] = poRow["pmlJobOperationID"];
		}
		if (!poRow.Field<bool>("impInactive"))
		{
			dataRow["rmlPartID"] = poRow["pmlPartID"];
			dataRow["rmlPartRevisionID"] = poRow["pmlPartRevisionID"];
			dataRow["rmlOrgPartID"] = poRow["pmlOrgPartID"];
			dataRow["rmlOrgPartShortDescription"] = poRow["pmlOrgPartShortDescription"];
			dataRow["rmlPartWarehouseLocationID"] = poRow["pmlPartWarehouseLocationID"];
			dataRow["rmlPartBinID"] = poRow["pmlPartBinID"];
			dataRow["rmlDescription"] = poRow["pmlPartShortDescription"];
			dataRow["rmlPartLongDescriptionRTF"] = poRow["pmlPartLongDescriptionRTF"];
			dataRow["rmlPartLongDescriptionText"] = poRow["pmlPartLongDescriptionText"];
			dataRow["rmlPurchaseUnitOfMeasure"] = poRow["pmlPurchaseUnitOfMeasure"];
			dataRow["rmlInventoryUnitOfMeasure"] = poRow["pmlInventoryUnitOfMeasure"];
			dataRow["rmlRequiresInspection"] = poRow["pmlRequiresInspection"];
			if (poRow["imrInspectionNotesRTF"].ToString().Length != 0)
			{
				dataRow["rmlInspectionNotesRTF"] = poRow["imrInspectionNotesRTF"];
				dataRow["rmlInspectionNotesText"] = poRow["imrInspectionNotesText"];
			}
		}
		if (itemValues.EditableValues != null)
		{
			if (itemValues.EditableValues.ContainsKey("PurchaseQtyRecd") && Convert.ToDecimal(itemValues.EditableValues["PurchaseQtyRecd"]) != 0m)
			{
				dataRow.SetField("rmlPurchaseQuantityReceived", Convert.ToDecimal(itemValues.EditableValues["PurchaseQtyRecd"]));
			}
			if (itemValues.EditableValues.ContainsKey("ReceivedComplete"))
			{
				dataRow.SetField("rmlPOReceivedComplete", Convert.ToBoolean(itemValues.EditableValues["ReceivedComplete"]));
			}
		}
		decimal num = poRow.Field<decimal>("pmlConversionFactor");
		if (num == 0m)
		{
			num = 1m;
		}
		dataRow["rmlConversionFactor"] = num;
		if (!HeaderFixForeign)
		{
			dataRow.SetField("rmlSetupCharge", poRow.Field<decimal>("pmlSetupChargeBase"));
		}
		else
		{
			dataRow.SetField("rmlSetupChargeForeign", poRow.Field<decimal>("pmlSetupChargeForeign"));
		}
		if (!HeaderFixForeign)
		{
			dataRow.SetField("rmlPurchaseUnitCost", poRow.Field<decimal>("pmlPurchaseUnitCostBase"));
		}
		else
		{
			dataRow.SetField("rmlPurchaseUnitCostForeign", poRow.Field<decimal>("pmlPurchaseUnitCostForeign"));
		}
		if (!HeaderFixForeign)
		{
			dataRow.SetField("rmlInventoryUnitCost", M1Math.Round(dataRow.Field<decimal>("rmlPurchaseUnitCost") * num, 5));
		}
		else
		{
			dataRow.SetField("rmlInventoryUnitCostForeign", M1Math.Round(dataRow.Field<decimal>("rmlPurchaseUnitCostForeign") * num, 5));
		}
		dataRow["rmlReference"] = "PO " + poRow.Field<string>("pmlPurchaseOrderID").Trim() + "-" + poRow.Field<short>("pmlPurchaseOrderLineID").ToString().Trim();
		if (!poRow.Field<bool>("prpClosed"))
		{
			dataRow["rmlProjectID"] = poRow["pmlProjectID"];
			dataRow["rmlProjectAreaID"] = poRow["pmlProjectAreaID"];
		}
		if (!poRow.Field<bool>("ompClosed"))
		{
			dataRow["rmlSalesOrderID"] = poRow["pmlSalesOrderID"];
			dataRow["rmlSalesOrderLineID"] = poRow["pmlSalesOrderLineID"];
			dataRow["rmlSalesOrderDeliveryID"] = poRow["pmlSalesOrderDeliveryID"];
		}
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtComponents.Select("pmoPurchaseOrderID = " + dataRow.Field<string>("rmlPurchaseOrderID").Trim().ToLinq() + " and pmoPurchaseOrderLineID = " + Convert.ToInt32(dataRow["rmlPurchaseOrderLineID"]).ToLinq());
		foreach (DataRow sourceLineRow in array)
		{
			TransferLineInfo(this, sourceLineRow, bsComponents, componentMatch, dataRow);
		}
	}

	private bool checkConditionsPO(DataRow drPO, DataRow ReceiptsRow, List<string> messages)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = ReceiptsRow.Field<string>("rmpReceiptID").Trim();
		if (ReceiptsRow.Field<bool>("rmpPostedToGL"))
		{
			stringBuilder.Append(", destination receipt " + text + " is already posted");
		}
		string text2 = drPO.Field<string>("pmlJobID").Trim();
		if (Convert.ToInt32(drPO["pmlPurchaseType"]) == 1 && Convert.ToBoolean(drPO["jmpClosed"]))
		{
			stringBuilder.Append(", job " + text2 + " is closed");
		}
		string text3 = drPO.Field<string>("pmlSalesOrderID").Trim();
		if (Convert.ToInt32(drPO["pmlPurchaseType"]) == 3 && Convert.ToBoolean(drPO["ompClosed"]))
		{
			stringBuilder.Append(", sales order " + text3 + " is closed");
		}
		string text4 = drPO.Field<string>("pmlProjectID").Trim();
		if (text4.Length != 0 && Convert.ToBoolean(drPO["prpClosed"]))
		{
			stringBuilder.Append(", project " + text4 + " is closed");
		}
		string text5 = drPO.Field<string>("pmlPartID").Trim();
		if (Convert.ToBoolean(drPO["impInactive"]))
		{
			stringBuilder.Append(", part " + text5 + " is inactive");
		}
		string text6 = ReceiptsRow.Field<string>("rmpSupplierOrganizationID").Trim();
		if (text6.Length != 0 && !text6.Equals(drPO.Field<string>("pmpSupplierOrganizationID").Trim(), StringComparison.CurrentCultureIgnoreCase))
		{
			stringBuilder.Append(", supplier org is not " + text6);
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Remove(0, 2);
			messages.Add("Some information for Purchase Order " + drPO.Field<string>("pmlPurchaseOrderID").Trim() + "/" + Convert.ToInt32(drPO["pmlPurchaseOrderLineID"]).ToString().Trim() + " was not added because " + stringBuilder.ToString() + ".");
			return false;
		}
		return true;
	}
}
